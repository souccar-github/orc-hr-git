using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.OrgChart.ValueObjects;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Services;
using Repository.UnitOfWork;
using Service;
using Souccar.Core;
using Telerik.Web.Mvc;
using UI.Areas.PMSComprehensive.Controllers.EntitiesRoots;
using UI.Areas.Services.Controllers.EntitiesRoots;
using UI.Areas.Services.DTO.Adapters;
using UI.Areas.Services.DTO.ViewModels;
using System.Linq;
using System.Web.Script.Serialization;
using UI.Helpers.Controllers;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.Services;
using UI.Extensions;

namespace UI.Areas.Services.Controllers
{
    public class DelegationController : ServicesAggregateController, IRule<Delegation>
    {
        [HttpPost]
        public ActionResult LoadRoles(int positionId)
        {
            var roles = new List<RoleViewModel>();
            JobDesc.Helpers.DropDownListHelpers.ListOfPositionRoles(positionId).ToList()
                .ForEach(x => roles.Add(new RoleViewModel
                                            {
                                                Id = int.Parse(x.Value),
                                                Name = x.Text
                                            }));

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Roles", roles),
                                Data = roles
                            });
        }

        [HttpPost]
        public ActionResult LoadAuthorities(int positionId)
        {
            var authorities = new List<AuthorityViewModel>();
            JobDesc.Helpers.DropDownListHelpers.ListOfPositionAuthorities(positionId).ToList()
                .ForEach(x => authorities.Add(new AuthorityViewModel
                                                  {
                                                      Id = int.Parse(x.Value),
                                                      Name = x.Text
                                                  }));

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Authorities", authorities),
                                Data = authorities
                            });
        }

        [HttpPost]
        public ActionResult BindEmployeeToSelectedRolesAndAuthorities(int employeeId, int positionId, List<int> roleIds,
                                                                      List<int> authorityIds,
                                                                      string hiddenAssignedEmployeesData)
        {
            var assignedEmployee = new AssignedEmployeeViewModel();
            var emp = new EntityService<Employee>().LoadById(employeeId);
            assignedEmployee.Employee.Id = emp.Id;
            assignedEmployee.Employee.FirstName = emp.FirstName;
            assignedEmployee.Employee.LastName = emp.LastName;

            (from indexItem in JobDesc.Helpers.DropDownListHelpers.ListOfPositionAuthorities(positionId).ToList()
             where authorityIds.Contains(int.Parse(indexItem.Value))
             select indexItem).ToList().ForEach(x => assignedEmployee.Authorities.Add(new AuthorityViewModel
                                                                                          {
                                                                                              Id = int.Parse(x.Value),
                                                                                              Name = x.Text
                                                                                          }));
            (from indexItem in JobDesc.Helpers.DropDownListHelpers.ListOfPositionRoles(positionId).ToList()
             where roleIds.Contains(int.Parse(indexItem.Value))
             select indexItem).ToList().ForEach(x => assignedEmployee.Roles.Add(new RoleViewModel
                                                                                    {
                                                                                        Id = int.Parse(x.Value),
                                                                                        Name = x.Text
                                                                                    }));

            var assignedEmployees = new List<AssignedEmployeeViewModel>();
            if (!string.IsNullOrEmpty(hiddenAssignedEmployeesData))
            {
                var ser = new JavaScriptSerializer();
                assignedEmployees = ser.Deserialize<List<AssignedEmployeeViewModel>>(hiddenAssignedEmployeesData);
            }

            if (assignedEmployees.Any(x => x.Employee.Id == assignedEmployee.Employee.Id))
            {
                assignedEmployees.Single(x => x.Employee.Id == employeeId).Roles.AddRange(
                    assignedEmployee.Roles.Where(
                        x =>
                        !(assignedEmployees.Single(z => z.Employee.Id == assignedEmployee.Employee.Id).Roles.Any(
                            y => y.Id == x.Id))).ToList());
                assignedEmployees.Single(x => x.Employee.Id == employeeId).Authorities.AddRange(
                    assignedEmployee.Authorities.Where(
                        x =>
                        !(assignedEmployees.Single(z => z.Employee.Id == assignedEmployee.Employee.Id).Authorities.Any(
                            y => y.Id == x.Id))).ToList());
            }
            else
            {
                assignedEmployees.Add(assignedEmployee);
            }

            assignedEmployees.RemoveAll(x => (x.Authorities.Count == 0 && x.Roles.Count == 0));

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("AssignedEmployee", assignedEmployees),
                                Data = assignedEmployees
                            });
        }

        public ObjectRules<Delegation> Rules
        {
            get { return new DelegationRules(); }
        }

        #region CRUD

        #region Read

        [GridAction]
        public ActionResult Index()
        {
            PrePublish();

            #region Security

            if (ViewData["CanRead"] != null && !(bool)ViewData["CanRead"])
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.CanReadMessage);
            }

            #endregion

            #region Get Data

            var delegations = new EntityService<Delegation>().GetAll();

            var pageNo = 1;

            if (GetMasterRecordValue(MasterRecordOrder.First) != 0)
            {
                var masterRecordValue = GetMasterRecordValue(MasterRecordOrder.First);

                var count = delegations.Count(c => (c.Id >= masterRecordValue));

                count = (int)Math.Ceiling((decimal)count / 5);

                //count=  (int)Math.Floor((double)(/5));

                pageNo = count != 0 ? count : 1;
            }

            ViewData["delegations"] = delegations;
            ViewData["SelectedRow"] = GetMasterRecordValue(MasterRecordOrder.First);
            ViewData["PageTo"] = pageNo;

            #endregion

            return View();
        }

        public ActionResult PartialMasterInfo(int selectedRowId = 0)
        {
            PrePublish();
            if (selectedRowId != 0)
            {
                SetMasterRecordValue(MasterRecordOrder.First, selectedRowId);
                CurrentlyInFirstLevel = true;
            }

            var delegation = new EntityService<Delegation>().LoadById(selectedRowId);

            var delegationViewModel = new DelegationViewModel
                                          {
                                              Delegation = delegation,
                                              PositionId = delegation.Position.Id,
                                              AssignedEmployees =
                                                  (from positionFulfillmentItem in
                                                       delegation.Position.PositionFulfillments
                                                   where positionFulfillmentItem.Type == PositionFulfillmentType.Delegated
                                                   select new AssignedEmployeeViewModel
                                                              {
                                                                  Employee = new EmployeeViewModel
                                                                                 {
                                                                                     Id =
                                                                                         positionFulfillmentItem.
                                                                                         Employee.Id,
                                                                                     FirstName =
                                                                                         positionFulfillmentItem.
                                                                                         Employee.FirstName,
                                                                                     LastName =
                                                                                         positionFulfillmentItem.
                                                                                         Employee.LastName
                                                                                 },
                                                                  Roles =
                                                                      (from roleItem in positionFulfillmentItem.Roles
                                                                       select new RoleViewModel
                                                                                  {
                                                                                      Id = roleItem.Role.Id,
                                                                                      Name = roleItem.Role.Name,
                                                                                      Checked = true
                                                                                  }).ToList(),
                                                                  Authorities =
                                                                      (from authorityItem in
                                                                           positionFulfillmentItem.Authoritys
                                                                       select new AuthorityViewModel
                                                                                  {
                                                                                      Id = authorityItem.Authority.Id,
                                                                                      Name =
                                                                                          authorityItem.Authority.
                                                                                          JobTitle.Name,
                                                                                      Checked = true
                                                                                  }).ToList()

                                                              }).ToList()
                                          };

            return PartialView("BasicInfo", delegationViewModel);
        }

        #endregion

        #region Create

        public ActionResult Insert()
        {
            LoadStepsList();

            AddToPath(MasterRecordOrder.First, RibbonLevels.A, areaName: ServicesAreaRegistration.GetAreaName,
                      nodeName: Resources.Areas.Services.Shared.Navigator.Delegation);

            DelegationViewModel delegationViewModel = new DelegationViewModel();

            return View("Insert", delegationViewModel);
        }

        public ActionResult Delegate(DelegationViewModel delegationViewModel)
        {
            #region Permission Check

            if (ViewData["CanUpdate"] != null && !(bool)ViewData["CanUpdate"])
            {
                ModelState.AddModelError(DomainErrors.SecurityError.ToString(),
                                         Resources.Shared.Messages.General.CanUpdateMessage);
                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml =
                                RenderPartialViewToString("Insert", delegationViewModel)
                                });
            }

            #endregion

            #region Variable Decleration

            var unitOfWork = new UnitOfWork();
            var delegationService = new EntityService<Delegation>(unitOfWork);
            var positionService = new EntityService<Position>(unitOfWork);
            var originalPosition = positionService.GetById(delegationViewModel.Delegation.Position.Id);

            #endregion

            #region Serialize Java Script Data

            var ser = new JavaScriptSerializer();

            var roles = ser.Deserialize<List<RoleViewModel>>(Request.Form["hiddenRoles"]);
            roles = roles ?? new List<RoleViewModel>();
            delegationViewModel.Roles = roles;

            var authorities = ser.Deserialize<List<AuthorityViewModel>>(Request.Form["hiddenAuthorities"]);
            authorities = authorities ?? new List<AuthorityViewModel>();
            delegationViewModel.Authorities = authorities;

            var assignedEmployees =
                ser.Deserialize<List<AssignedEmployeeViewModel>>(Request.Form["hiddenAssignedEmployees"]);
            assignedEmployees = assignedEmployees ?? new List<AssignedEmployeeViewModel>();
            delegationViewModel.AssignedEmployees = assignedEmployees;

            #endregion

            #region Validation

            if (assignedEmployees.Count == 0)
            {
                ModelState.AddModelError("AssignedEmployees",
                                         Resources.Areas.Services.Delegation.DelegationRules.AssignedEmployeesReq);
                ModelState.AddModelErrors(Rules.GetBrokenRules(delegationViewModel.Delegation));

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml =
                                RenderPartialViewToString("DelegationService", delegationViewModel),
                                    Roles = roles,
                                    Authorities = authorities,
                                    AssignedEmployees = assignedEmployees
                                });
            }

            if (Rules.GetBrokenRules(delegationViewModel.Delegation).Count > 0)
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(delegationViewModel.Delegation));

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml =
                                RenderPartialViewToString("DelegationService", delegationViewModel),
                                    Roles = roles,
                                    Authorities = authorities,
                                    AssignedEmployees = assignedEmployees
                                });
            }

            #endregion

            #region check if position delegated before

            //var duplicatedDelegatedEmployees = DelegationAdapter.GetDuplicatedDelegatedEmployees(originalPosition,delegationViewModel);
            //if (duplicatedDelegatedEmployees != null)
            //{
            //    if (duplicatedDelegatedEmployees.Any())
            //    {
            //        string duplicationErrorMsg = String.Empty;
            //        foreach (var item in duplicatedDelegatedEmployees)
            //        {
            //            duplicationErrorMsg +=
            //                String.Format(
            //                    Resources.Areas.Services.Delegation.DelegationRules.duplicatedDelegatedEmployees,
            //                    item.FullName);
            //        }
            //        ModelState.AddModelError("EmployeeId", duplicationErrorMsg);

            //        return Json(new
            //                        {
            //                            Success = false,
            //                            PartialViewHtml =
            //                        RenderPartialViewToString("DelegationService", delegationViewModel),
            //                            Roles = roles,
            //                            Authorities = authorities,
            //                            AssignedEmployees = assignedEmployees
            //                        });
            //    }
            //}
            if(originalPosition.PositionFulfillments.Count(x=>x.Type == PositionFulfillmentType.Delegated && x.IsActive)>0)
            {
                ModelState.AddModelError("PositionDelegated",
                                         Resources.Areas.Services.Delegation.DelegationRules.PositionDelegated);
                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml =
                                RenderPartialViewToString("DelegationService", delegationViewModel),
                                    Roles = roles,
                                    Authorities = authorities,
                                    AssignedEmployees = assignedEmployees
                                });
            }


            #endregion

            #region Update

            DelegationAdapter.UpdatePositionFulfillmentsFromViewModel(delegationViewModel, originalPosition);
            try
            {
                positionService.UpdateEntity(originalPosition);
                delegationService.AddEntity(delegationViewModel.Delegation);
                unitOfWork.Commit();
                SetMasterRecordValue(MasterRecordOrder.First, 0);
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                ModelState.AddModelError(DomainErrors.InternalError.ToString(),
                                         Resources.Shared.Messages.General.ErrorWhileUpdate);
                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml =
                                RenderPartialViewToString("DelegationService", delegationViewModel),
                                    Roles = roles,
                                    Authorities = authorities,
                                    AssignedEmployees = assignedEmployees
                                });
            }
            return Json(new
                            {
                                Success = true,
                                Message = Resources.Areas.Services.Delegation.Messages.DelegationComletedSuccessfully
                            });

            #endregion
        }

        #endregion

        #region Update

        public ActionResult Edit(int id)
        {
            var delegation = new EntityService<Delegation>().LoadById(id);

            DelegationViewModel delegationViewModel = new DelegationViewModel
                                                          {
                                                              Delegation = delegation
                                                          };

            return PartialView("Edit", delegationViewModel);
        }

        [HttpPost]
        public ActionResult JsonEdit(DelegationViewModel delegationViewModel)
        {
            #region Permission Check

            if (ViewData["CanUpdate"] != null && !(bool)ViewData["CanUpdate"])
            {
                ModelState.AddModelError(DomainErrors.SecurityError.ToString(),
                                         Resources.Shared.Messages.General.CanUpdateMessage);
                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("Edit", delegationViewModel)
                                });
            }

            #endregion

            var delegationService = new EntityService<Delegation>();
            var originalDelegation = delegationService.LoadById(delegationViewModel.Delegation.Id);
            this.UpdateValueObject(delegationViewModel.Delegation, originalDelegation);


            PrePublish();

            if ((Rules.GetBrokenRules(originalDelegation).Count == 0))
            {
                try
                {
                    delegationService.Update(originalDelegation);
                }
                catch (Exception)
                {
                    ModelState.AddModelError(DomainErrors.InternalError.ToString(),
                                             Resources.Shared.Messages.General.ErrorWhileUpdate);
                    return Json(new
                                    {
                                        Success = false,
                                        PartialViewHtml = RenderPartialViewToString("Edit", delegationViewModel)
                                    });
                }
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(originalDelegation));
                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("Edit", delegationViewModel)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.First, originalDelegation.Id);

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("BasicInfo", delegationViewModel)
                            });
        }

        #endregion

        #region Delete

        [HttpPost]
        public ActionResult Delete(int id)
        {
            PrePublish();

            if (ViewData["CanDelete"] != null && !(bool)ViewData["CanDelete"])
            {
                return RedirectToAction("Index");
            }

            #region Variable Decleration

            var unitOfWork = new UnitOfWork();
            var delegationService = new EntityService<Delegation>(unitOfWork);
            var delegation = delegationService.GetById(id);
            var positionService = new EntityService<Position>(unitOfWork);
            //var originalPosition = positionService.Find(delegation.Position.Id);

            #endregion


            try
            {
                var tempPositionFulfillments = delegation.Position.PositionFulfillments.Where(x => x.Type == PositionFulfillmentType.Delegated).ToList();
                foreach (var item in tempPositionFulfillments)
                {
                    delegation.Position.PositionFulfillments.Remove(item);
                }

                positionService.UpdateEntity(delegation.Position);
                delegationService.DeletEntity(delegation);
                unitOfWork.Commit();
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
            }

            SetMasterRecordValue(MasterRecordOrder.First, 0);
            return RedirectToAction("Index");
        }

        #endregion

        #endregion
    }
}


