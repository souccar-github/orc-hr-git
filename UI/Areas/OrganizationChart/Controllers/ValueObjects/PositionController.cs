#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.OrgChart.Entities;
using HRIS.Domain.OrgChart.ValueObjects;
using HRIS.Domain.ProjectManagment.ValueObjects;
using Repository.UnitOfWork;
using Service;
using Service.PMSComprehensive;
using Service.OrgChart;
using Souccar.Core;
using UI.Areas.OrganizationChart.Controllers.EntitiesRoots;
using UI.Areas.OrganizationChart.DTO.ViewModels;
using UI.Areas.OrganizationChart.Helpers;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.OrganizationChart.ValueObjects;

#endregion

namespace UI.Areas.OrganizationChart.Controllers.ValueObjects
{
    public class PositionController : NodeAggregateController, IRule<Position>
    {
        #region Parents Chain

        #region Node

        private Node _node;

        public Node FirstEntity
        {
            get
            {
                if (_node!=null)
                    return _node;
                var nodeId = GetMasterRecordValue(MasterRecordOrder.First);
                if (nodeId != 0)
                {
                    _node = Service.LoadById(nodeId);
                    return _node;    
                    
                }
                return null;


            }
        }
      
        #endregion

        #endregion

        #region IRule<Position> Members

        public ObjectRules<Position> Rules
        {
            get { return new PositionRules(); }
        }

        #endregion

        #region Overrides of PositionAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Position.Id");
            ModelState.Remove("Position.Status.Name");
            ModelState.Remove("Position.Level.Name");
            ModelState.Remove("Position.Type.Name");
            ModelState.Remove("Position.JobTitle.Name");
            ModelState.Remove("Position.CostCenter.Name");
            ModelState.Remove("Position.DisabilityStatus.Name");
            ModelState.Remove("Position.GradeId.Name");
            ModelState.Remove("Position.Per.Name");
            ModelState.Remove("Position.Interval.Name");
        }

        #endregion

        #region Utilities

        public ActionResult ClearSelection()
        {
            SetMasterRecordValue(MasterRecordOrder.Second, 0);

            return RedirectToAction("Index");
        }

        #endregion

        #region CRUD

        #region Read

        public ActionResult Index(int id = 0, int selectedTabOrder = 0, bool ribbon = false)
        {
            PrePublish();

            #region Security

            if (ViewData["CanRead"] != null && !(bool)ViewData["CanRead"])
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.CanReadMessage);
            }

            #endregion

            #region Manage Tab, Path, and MastersList

            if (ribbon)
            {
                //ClearMasterRecords();
                SaveTabIndex(0);
            }
            else
            {
                if (id != 0)
                {
                    SetMasterRecordValue(MasterRecordOrder.Second, id);

                    //if (GetMasterRecordValue(MasterRecordOrder.First)==0)
                    //{
                    //    SetMasterRecordValue(MasterRecordOrder.First, new EntityService<Position>().LoadById(id).Node.Id);
                    //}

                    CurrentlyInSecondLevel = id;
                }

                if (selectedTabOrder > 0)
                { 
                    SaveTabIndex(selectedTabOrder);
                }
            }

            PrePublish();

            AddToPath(MasterRecordOrder.Second, RibbonLevels.C, areaName: OrganizationChartAreaRegistration.GetAreaName,
                nodeName: Resources.Areas.OrgChart.Views.Shared.Navigator.Position);

            #endregion

            #region Get Data

            int pageNo = 1;
            if (GetMasterRecordValue(MasterRecordOrder.Second) != 0)
            {
                int masterRecordValue = GetMasterRecordValue(MasterRecordOrder.Second);

                int count = FirstEntity.Positions.Where(position => (position.Id >= masterRecordValue)).Count();

                pageNo = count / 5;

                if (count % 5 > 0)
                {
                    pageNo++;
                }
            }

            if (OrganizationService.GetList().Count == 0)
            {
                return RedirectToAction("Index", "Organization");
            }

            if (FirstEntity==null)
            {
                 SetGlobalErrorMessage(Resources.Areas.OrgChart.ValueObjects.Node.Messages.SelectNodeToProceedPosition);
                return RedirectToAction("LoadTree", "Node");
            }
            ViewData["positions"] = FirstEntity.Positions;
            ViewData["SelectedRow"] = GetMasterRecordValue(MasterRecordOrder.Second);
            ViewData["PageTo"] = pageNo;

            #endregion

            return View();
        }
        public ActionResult PartialInfo(int selectedPositionId = 0)
        {
            PrePublish();

            if (selectedPositionId == 0)
            {
                return PartialView("BasicInfo", new PositionViewModel { Position = new Position() });
            }
            SetMasterRecordValue(MasterRecordOrder.Second, selectedPositionId);
            CurrentlyInSecondLevel = selectedPositionId;
            var positionService = new EntityService<Position>();
            var position = positionService.GetById(selectedPositionId);
            var positionViewModel = new PositionViewModel { Position = position };

            return PartialView("BasicInfo", positionViewModel);
        }
        #endregion

        #region Update

        [HttpGet]
        public ActionResult Edit()
        {
            PrePublish();
            var positionId = GetMasterRecordValue(MasterRecordOrder.Second);
            var nodeId = GetMasterRecordValue(MasterRecordOrder.First);
            ViewData["NodeId"] = nodeId;

            if (positionId != 0)
            {
                var servicePosition = new EntityService<Position>();
                var position = servicePosition.LoadById(positionId);
                var positionViewModel = new PositionViewModel
                {
                    Position = position,
                    NodeId = nodeId
                };
                if (position.ParentPosition!=null)
                {
                    positionViewModel.ParentPositionId = position.ParentPosition.Id;
                }

                return PartialView("Edit", positionViewModel);
            }

            return ErrorPartialMessage(Resources.Shared.Messages.General.GeneralErrorOccurred);
        }

        [HttpPost]
        public ActionResult JsonEdit(PositionViewModel positionViewModel)
        {
            PrePublish();
            var nodeId = GetMasterRecordValue(MasterRecordOrder.First);
            if (nodeId == 0)
            {
                SetGlobalErrorMessage(Resources.Areas.OrgChart.ValueObjects.Node.Messages.SelectNodeToProceedPosition);
                return RedirectToAction("LoadTree", "Node");
               // throw new ApplicationException("node value must be defined");
            }
            else
            {
                ViewData["NodeID"] = nodeId;
                positionViewModel.NodeId = nodeId;
            }
            var servicePosition = new EntityService<Position>();
            var positionId = GetMasterRecordValue(MasterRecordOrder.Second);
            Position original = servicePosition.LoadById(positionId);
            var position = positionViewModel.Position;
            original.Code = position.Code;
            original.JobTitle = position.JobTitle;
            original.Level = position.Level;
            original.Type = position.Type;
            original.Budget = position.Budget;
            original.CostCenter = position.CostCenter;
            original.WorkingHours = position.WorkingHours;
            if (positionViewModel.ParentPositionId.HasValue)
            {
                var anyActiveParentPosition =
                    original.PositionReportings.Where(
                        pr => pr.IsActive && pr.ParentPosition.Id == positionViewModel.ParentPositionId.Value).Any();
                if (anyActiveParentPosition)
                {
                    ModelState.AddModelError("DupLicateReportingPosition", Resources.Areas.OrgChart.ValueObjects.Position.PositionRules.DupLicateReportingPosition);
                }
                else
                {
                    var isPrimary = !original.PositionReportings.Where(pr => pr.IsActive && pr.IsPrimary).Any();
                    original.AddPositionReporting(new Position() {Id = positionViewModel.ParentPositionId.Value},isPrimary);
                }
            }

            if ((Rules.GetBrokenRules(original).Count == 0) && (TryValidateModel(original)))
            {
                
                servicePosition.Update(original);
                
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(original));
                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("Edit",
                                      positionViewModel)
                    
                });
            }
            PrePublish();
            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("BasicInfo",
                                      positionViewModel)
            });
        }

        #endregion

        #region Create

        public ActionResult Insert()
        {
            PrePublish();
            ViewData["NodeID"] = GetMasterRecordValue(MasterRecordOrder.First);
            return PartialView("Create", new PositionViewModel());
        }

        [HttpPost]
        public ActionResult JsonInsert(PositionViewModel positionViewModel)
        {

            PrePublish();
            var nodeId = GetMasterRecordValue(MasterRecordOrder.First);
            if (nodeId==0)
            {

                SetGlobalErrorMessage(Resources.Areas.OrgChart.ValueObjects.Node.Messages.SelectNodeToProceedPosition);

                return RedirectToAction("LoadTree", "Node");
            }
            else
            {
                ViewData["NodeID"] = nodeId;   

            }

          
            var node = new Node() {Id = nodeId};
            var position = positionViewModel.Position;
            var positionService = new EntityService<Position>();

            
            position.AddPositionNode(node);
            

            if (positionViewModel.ParentPositionId.HasValue)
                position.AddPositionReporting(new Position() { Id = positionViewModel.ParentPositionId.Value });


            if (positionService.GetAll().Where(p => p.Code == position.Code).Any())
            {
                ModelState.AddModelError("DupLicateCode", Resources.Areas.OrgChart.ValueObjects.Position.PositionRules.DupLicateCode);
            }
            if ((Rules.GetBrokenRules(position).Count == 0) && (TryValidateModel(position)))
            {

                if (positionViewModel.GradeId.HasValue)
                {
                    var positionGrade = GradeHelpers.CreateGradePositionFromGrad(positionViewModel.GradeId.Value);
                    position.AddGrade(positionGrade);

                }
                try
                {
                    positionService.Add(position);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(DomainErrors.InternalError.ToString(), Resources.Shared.Messages.General.ErrorWhileUpdate+" "+ex.Message );
                    return Json(new
                    {
                        Success = false,
                        PartialViewHtml = RenderPartialViewToString("Create", positionViewModel)
                    });
                }
                
                
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(position));
                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("Create", positionViewModel)
                });
            }

            PrePublish();
            
            SetMasterRecordValue(MasterRecordOrder.Second, position.Id);


            return Json(new
            {
                Success = true,
                PartialViewHtml =
                RenderPartialViewToString("BasicInfo", positionViewModel)
            });
        }

        #endregion

        #region Delete

        [HttpPost]
        public ActionResult Delete(int id)
        {
            
            var positionService = new EntityService<Position>();
            var position = positionService.LoadById(id);


            try
            {


                positionService.Delete(position);

                PrePublish();

                SetMasterRecordValue(MasterRecordOrder.Second, 0);

               // return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                SetGlobalErrorMessage(Resources.Shared.Messages.General.ErrorDuringDelete + " " + ex.Message);
                //ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete +" "+ex.Message);
              //  return RedirectToAction("Index");
            }
            
                return RedirectToAction("Index");
            
        
        }

        #endregion

        #region ReadOnly Action Will be used to display data on show the Node Interface of reasing position to node
        public ActionResult ReadOnly(int nodeId)
        {
            PrePublish();

            if (OrganizationService.GetList().Count == 0)
            {
                return RedirectToAction("Index", "Organization");
            }

            IList<Position> positions = Service.LoadById(nodeId).Positions;

            if (positions.Count > 0)
            {
                ViewData["Positions"] = positions.ToList();
            }

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("ReadOnly")
            });
        }
        #endregion
        //public override void FillList()
        //{
        //    base.FillList();
        //    ViewBag.ListOfGrades = DropDownListHelpers.ListOfGrades;
        //    ViewBag.ListOfJobTitles = DropDownListHelpers.ListOfJobTitle;
        //    ViewBag.ListOfPositionTypes = DropDownListHelpers.ListOfPositionType;
        //    ViewBag.ListOfPositionLevels = DropDownListHelpers.ListOfPositionLevel;
        //    ViewBag.ListOfCostCenters = DropDownListHelpers.ListOfCostCenter;
        //    ViewBag.ListOfTimeIntervals = DropDownListHelpers.ListOfTimeIntervals;
        //    ViewBag.ListOfDisabilityStatus = DropDownListHelpers.ListOfDisabilityStatus;
        //    var nodeId = GetMasterRecordValue(MasterRecordOrder.First);
        //    if (nodeId!=0)
        //        ViewBag.ListOfParentPositions = DropDownListHelpers.ListOfPositionsOfParentNode(nodeId);
        //    else
        //    {
        //        var item = new SelectListItem() {Text = "Select Correct Node", Value = "0"};
        //        ViewBag.ListOfParentPositions = new SelectList(new[] {item});
        //    }
            
        //}
        #endregion
    }
}