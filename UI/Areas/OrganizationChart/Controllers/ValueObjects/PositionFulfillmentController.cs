#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domain.Seedwork;
using Infrastructure.Validation;
using HRIS.Domain.OrgChart.ValueObjects;
using Repository.UnitOfWork;
using Resources.Areas.OrgChart.ValueObjects.PositionFulfillment;
using Resources.Shared.Messages;
using Service;
using Service.Personnel;
using StructureMap;
using Telerik.Web.Mvc;
using UI.Areas.OrganizationChart.Controllers.EntitiesRoots;
using UI.Areas.OrganizationChart.DTO.Adapters;
using UI.Areas.OrganizationChart.DTO.ViewModels;
using UI.Helpers.Controllers;
using UI.Utilities;

#endregion

namespace UI.Areas.OrganizationChart.Controllers.ValueObjects
{
    public class PositionFulfillmentController : RootEntityController
    {
        #region // Unit of Work //

        private readonly UnitOfWork unitOfWork = ObjectFactory.GetInstance<UnitOfWork>();

        #endregion

        #region Implementation of IRule<PositionFulfillment>

        public ObjectRules<PositionFulfillment> Rules
        {
            get { return new Validation.OrganizationChart.ValueObjects.PositionFulfillmentRules(); }
        }

        #endregion

        #region // CRUD //

        public ActionResult Index(int positionId){
            SetMasterRecordValue(MasterRecordOrder.First, positionId);

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Grid")
                            });
        }

        [OutputCache(Duration = 0)]
        public ActionResult Read()
        {
            return Json(RefreshGridModel(unitOfWork), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(int id)
        {
            if (id != 0)
            {
                try
                {
                   
                    var positionFulfillmentViewModel = new PositionFulfillmentViewModel();
                    EntityService<Position> positionUnitSevice = GetPositionUnitSevice(unitOfWork);
                    Position position = positionUnitSevice.GetById(GetMasterRecordValue(MasterRecordOrder.First));
                    PositionFulfillment positionFulfillment = GetPositionFulfillmentById(position, id);

                    if (IsExpiredDate(positionFulfillment))
                        return new ErrorResult(PositionFulfillmentRules.CanNotUpdateAnExpiredRecord);

                    if (positionFulfillment.Type == PositionFulfillmentType.Secondary)
                    {
                        if (TryUpdateModel(positionFulfillmentViewModel))
                        {
                            PositionFulfillmentAdapter.Adapte(positionFulfillmentViewModel, positionFulfillment);

                            if ((Rules.GetBrokenRules(positionFulfillment).Count == 0) && (TryValidateModel(positionFulfillment)))
                            {
                                string errorMessage;
                                if (IsValidData(positionFulfillment, unitOfWork, out errorMessage))
                                {
                                    try
                                    {
                                        positionFulfillment.Position = position;
                                        positionUnitSevice.UpdateEntity(position);
                                        UpdatePrimaryPositionFulfillmentWeight(positionUnitSevice, unitOfWork, positionFulfillment);

                                        unitOfWork.Commit();
                                    }
                                    catch
                                    {
                                        unitOfWork.Rollback();
                                        return new ErrorResult(General.ErrorWhileUpdate);
                                    }
                                }
                                else
                                {
                                    unitOfWork.Rollback();
                                    return new ErrorResult(errorMessage);
                                }
                            }
                            else
                            {
                                unitOfWork.Rollback();
                                return new ErrorResult(Rules.GetBrokenRules(positionFulfillment)[0].Rule);
                            }

                        }
                    }
                    else
                    {
                        unitOfWork.Rollback(); 
                        return new ErrorResult(PositionFulfillmentRules.CanNotUpdatePrimaryRecord);
                    }
                }
                catch(Exception ex)
                {
                    unitOfWork.Rollback();
                    return new ErrorResult(General.ErrorWhileUpdate);
                }
            }

            return Read();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                try
                {
                   
                    EntityService<Position> positionUnitSevice = GetPositionUnitSevice(unitOfWork);
                    Position position = positionUnitSevice.LoadById(GetMasterRecordValue(MasterRecordOrder.First));
                    PositionFulfillment positionFulfillment = GetPositionFulfillmentById(position, id);

                    if (IsExpiredDate(positionFulfillment))
                        return new ErrorResult(PositionFulfillmentRules.CanNotDeleteAnExpiredRecord);


                    if (TryUpdateModel(positionFulfillment))
                    {
                        try
                        {
                            position.PositionFulfillments.Remove(positionFulfillment);
                            positionUnitSevice.UpdateEntity(position);
                            UpdatePrimaryPositionFulfillmentWeight(positionUnitSevice,unitOfWork, positionFulfillment);

                            unitOfWork.Commit();
                        }
                        catch
                        {
                            unitOfWork.Rollback();
                            return new ErrorResult(General.ErrorDuringDelete);
                        }
                    }
                }
                catch
                {
                    unitOfWork.Rollback();
                    return ErrorPartialMessage(General.ErrorDuringDelete);
                }
            }

            return Read();
        }

        [HttpPost]
        public ActionResult Expire(int id)
        {
            if (id != 0)
            {
                try
                {
                  
                    EntityService<Position> positionUnitSevice = GetPositionUnitSevice(unitOfWork);
                    Position position = positionUnitSevice.LoadById(GetMasterRecordValue(MasterRecordOrder.First));
                    PositionFulfillment positionFulfillment = GetPositionFulfillmentById(position, id);

                    if (IsExpiredDate(positionFulfillment))
                        return new ErrorResult(PositionFulfillmentRules.AlreadyExpired);

                    try
                    {
                        positionFulfillment.MakeHistory();
                        positionFulfillment.Position = position;
                        positionUnitSevice.UpdateEntity(position);
                        UpdatePrimaryPositionFulfillmentWeight(positionUnitSevice,unitOfWork, positionFulfillment);

                        unitOfWork.Commit();
                    }
                    catch
                    {
                        unitOfWork.Rollback();
                        return new ErrorResult(General.ErrorWhileUpdate);
                    }
                }
                catch
                {
                    unitOfWork.Rollback();
                    return new ErrorResult(General.ErrorWhileUpdate);
                }
            }

            return Read();
        }

        private void UpdatePrimaryPositionFulfillmentWeight(EntityService<Position> positionUnitSevice,UnitOfWork unitOfWork, PositionFulfillment positionFulfillment)
        {
            if (EmployeeHelpers.IsExistEmployeePrimaryPositionFulfillment(unitOfWork, positionFulfillment.Employee.Id))
            {
                decimal totalSecondaryWeights =
                    EmployeeHelpers.GetEmployeePositionFulfillmentToatalWeights(unitOfWork, positionFulfillment.Employee.Id, PositionFulfillmentType.Secondary);

                positionUnitSevice.UpdateEntity(EmployeeHelpers.UpdatePrimaryPositionFulfillmentWeight(unitOfWork, positionFulfillment.Employee.Id, totalSecondaryWeights));
            }
        }
        public IList<PositionFulfillmentViewModel> GetPositionFulfillmentsList(IList<PositionFulfillment> positionFulfillments)
        {
            return positionFulfillments.Select(
                    positionFulfillment => PositionFulfillmentAdapter.Adapte(positionFulfillment)).ToList();
        }

        private GridModel RefreshGridModel(UnitOfWork unitOfWork)
        {
            IList<PositionFulfillment> positionFulfillmentList =
                GetPositionUnitSevice(unitOfWork).GetById(GetMasterRecordValue(MasterRecordOrder.First)).PositionFulfillments;
            var model = new GridModel();
            IList<PositionFulfillmentViewModel> list = GetPositionFulfillmentsList(positionFulfillmentList);
            model.Data = list;
            model.Total = list.Count;

            return model;
        }

        private PositionFulfillment GetPositionFulfillmentById(Position position, int positionFulfillmentId)
        {return position.PositionFulfillments.Where(c => c.Id == positionFulfillmentId).FirstOrDefault();
        }

        private EntityService<Position> GetPositionUnitSevice(UnitOfWork unitOfWork)
        {
            return new EntityService<Position>(unitOfWork);
        }

        private bool IsExpiredDate(PositionFulfillment positionFulfillment)
        {
            if (positionFulfillment.ExpireDate != null)
                return true;

            return false;
        }

        private bool IsValidData(PositionFulfillment positionFulfillment,UnitOfWork unitOfWork, out string errorMessage)
        {
            if (EmployeeHelpers.IsExistEmployeePrimaryPositionFulfillment(unitOfWork, positionFulfillment.Employee.Id))
            {
                decimal primaryPositionFulfillmentToatalWeights =
                    EmployeeHelpers.GetEmployeePositionFulfillmentToatalWeights(unitOfWork, positionFulfillment.Employee.Id, PositionFulfillmentType.Primary);

                if (positionFulfillment.Weight < primaryPositionFulfillmentToatalWeights)
                {
                    decimal secondaryPositionFulfillmentToatalWeights =
                        EmployeeHelpers.GetEmployeePositionFulfillmentToatalWeights(unitOfWork, positionFulfillment.Employee.Id, PositionFulfillmentType.Secondary);

                    if ((secondaryPositionFulfillmentToatalWeights + positionFulfillment.Weight) >= 100)
                    {
                        errorMessage = string.Format(Resources.Areas.Services.AssignEmployeeToPosition.AssignEmployeeToPositionRules.
                            SecondaryPositionFulfillmentToatalWeightsRule, (secondaryPositionFulfillmentToatalWeights + positionFulfillment.Weight).ToString());
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

            errorMessage = string.Empty;
            return true;
        }

        #endregion
    }
}