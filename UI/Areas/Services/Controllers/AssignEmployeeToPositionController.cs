using System;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Infrastructure.Entities;
using HRIS.Domain.OrgChart.ValueObjects;
using Repository.UnitOfWork;
using Service;
using Service.Personnel;
using UI.Areas.OrganizationChart.Controllers.EntitiesRoots;
using HRIS.Domain.Personnel.Entities;
using UI.Helpers.Controllers;
using UI.Utilities;


namespace UI.Areas.Services.Controllers
{

    public class AssignEmployeeToPositionController : RootEntityController
    {

        #region // Unit of Work //

        private UnitOfWork unitOfWork = new UnitOfWork();

        #endregion

        #region // Tree //

        public ActionResult GetTreeNodes()
        {
            ViewData["PositionID"] = GetMasterRecordValue(MasterRecordOrder.First);
            var nodeService = new EntityService<Node>(unitOfWork);

            return PartialView("Index", nodeService.GetAll());
        }

        #endregion

        #region // Dropdown List

        [HttpPost]
        public ActionResult GetEmployees(string text)
        {
            //Thread.Sleep(100);
            var employeeService = new EntityService<Employee>(unitOfWork);
            var employees = employeeService.GetAll();

            if (!string.IsNullOrEmpty(text))
                employees = employees.Where(e => e.FirstName.Contains(text) || e.LastName.Contains(text));

            var result = from emp in employees
                         select new { Text = emp.FirstName + " " + emp.LastName, Value = emp.Id.ToString() };
            var data = result.ToList();

            return new JsonResult { Data = data };
        }

        #endregion

        #region // New Position Fulfillment //

        private bool ValidateData(Position position, Employee employee, decimal weight, out string errorMessage)
        {
            if (position.IsTransient())
            {
                errorMessage = Resources.Areas.Services.AssignEmployeeToPosition.AssignEmployeeToPositionRules.PositionReq;
                return false;
            }

            if (employee.IsTransient())
            {
                errorMessage = Resources.Areas.Services.AssignEmployeeToPosition.AssignEmployeeToPositionRules.EmployeeReq;
                return false;
            }

            if (position.PositionFulfillments.Any(full => full.Employee.Id == employee.Id && full.IsActive))
            {
                errorMessage = string.Format(Resources.Areas.Services.AssignEmployeeToPosition.AssignEmployeeToPositionRules.EmployeeAlreadyAssigned,
                        employee.FirstName, employee.LastName, position.Code);
                return false;
            }

            if (EmployeeHelpers.IsExistEmployeePrimaryPositionFulfillment(unitOfWork, employee.Id) || EmployeeHelpers.IsExistPrimaryPositionFulfillment(position))
            {
                if (weight == 0)
                {
                    errorMessage = Resources.Areas.Services.AssignEmployeeToPosition.AssignEmployeeToPositionRules.WeightReq;
                    return false;
                }
                else
                {
                    if (EmployeeHelpers.IsExistEmployeePrimaryPositionFulfillment(unitOfWork, employee.Id))
                    {
                        decimal primaryPositionFulfillmentToatalWeights =
                            EmployeeHelpers.GetEmployeePositionFulfillmentToatalWeights(unitOfWork, employee.Id, PositionFulfillmentType.Primary);

                        if (weight < primaryPositionFulfillmentToatalWeights)
                        {
                            decimal secondaryPositionFulfillmentToatalWeights =
                                EmployeeHelpers.GetEmployeePositionFulfillmentToatalWeights(unitOfWork, employee.Id, PositionFulfillmentType.Secondary);

                            if ((secondaryPositionFulfillmentToatalWeights + weight) >= 100)
                            {
                                errorMessage = string.Format(Resources.Areas.Services.AssignEmployeeToPosition.AssignEmployeeToPositionRules.
                                    SecondaryPositionFulfillmentToatalWeightsRule, (secondaryPositionFulfillmentToatalWeights + weight).ToString());
                                return false;
                            }
                        }
                        else
                        {
                            errorMessage = string.Format(Resources.Areas.Services.AssignEmployeeToPosition.AssignEmployeeToPositionRules.
                                    PrimaryPositionFulfillmentWeightRule, primaryPositionFulfillmentToatalWeights.ToString());
                            return false;
                        }
                    }
                }    
            }
            
            
                
            errorMessage = string.Empty;
            return true;
        }

        private JsonResult ReturnedJson(bool isSucceeded, string message)
        {
            return Json(new
            {
                Success = isSucceeded,
                Msg = message
            });
        }

        public ActionResult AddPositionFulfillment(int positionId, int employeeId, decimal weight)
        {
            var positionService = new Service.EntityService<Position>(unitOfWork);
            var position = positionService.GetById(positionId);

            var employeeService = new Service.EntityService<Employee>(unitOfWork);
            var employee = employeeService.GetById(employeeId);

            string errorMessage;

            if (!ValidateData(position, employee, weight, out  errorMessage))
                return ReturnedJson(false, errorMessage);

            try
            {
                position.AddPositionFulfillment(NewPositionFulfillment(position, employeeId, weight));
                positionService.UpdateEntity(position);

                if (EmployeeHelpers.IsExistEmployeePrimaryPositionFulfillment(unitOfWork, employeeId))
                {
                    decimal totalSecondaryWeights = EmployeeHelpers.GetEmployeePositionFulfillmentToatalWeights(unitOfWork, employeeId, PositionFulfillmentType.Secondary);
                    positionService.UpdateEntity(EmployeeHelpers.UpdatePrimaryPositionFulfillmentWeight(unitOfWork, employeeId, totalSecondaryWeights));
                }

                unitOfWork.Commit();
            }
            catch(Exception ex)
            {
                unitOfWork.Rollback();

                return ReturnedJson(true, Resources.Areas.Services.AssignEmployeeToPosition.AssignEmployeeToPositionRules.OperationFailed);
            }

            return ReturnedJson(true, string.Format(
                Resources.Areas.Services.AssignEmployeeToPosition.AssignEmployeeToPositionRules.OperationSuccessed, 
                employee.FirstName, employee.LastName, position.Code));
        }

        private PositionFulfillment NewPositionFulfillment(Position position, int employeeId, decimal weight)
        {
            var newPositionFulfillmentType = PositionFulfillmentType.Primary;
            decimal newWeight = 100;

            if (EmployeeHelpers.IsExistEmployeePrimaryPositionFulfillment(unitOfWork, employeeId) || EmployeeHelpers.IsExistPrimaryPositionFulfillment(position))
            {
                newPositionFulfillmentType = PositionFulfillmentType.Secondary;
                newWeight = weight;
            }

            var newPositionFulfillment = new PositionFulfillment
            {
                Position = new Position { Id = position.Id },
                Employee = new Employee { Id = employeeId },
                Weight = newWeight,
                Type = newPositionFulfillmentType
            };

            return newPositionFulfillment;
        }

        public ActionResult IsPrimaryPositionFulfillment(int positionId, int employeeId)
        {
            if (positionId != 0 && employeeId != 0)
            {
                var positionService = new Service.EntityService<Position>(unitOfWork);
                var position = positionService.GetById(positionId);

                if (EmployeeHelpers.IsExistEmployeePrimaryPositionFulfillment(unitOfWork, employeeId) || EmployeeHelpers.IsExistPrimaryPositionFulfillment(position))
                {
                    return Json(new
                    {
                        Success = false
                    });
                }
            }
            
            return Json(new
            {
                Success = true
            });
        }

        #endregion

    }
}
