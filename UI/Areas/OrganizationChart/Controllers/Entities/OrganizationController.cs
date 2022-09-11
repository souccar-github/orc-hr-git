#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.OrgChart.Entities;
using HRIS.Domain.OrgChart.ValueObjects;
using UI.Areas.OrganizationChart.Controllers.EntitiesRoots;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.OrganizationChart.Entities;

#endregion

namespace UI.Areas.OrganizationChart.Controllers.Entities
{
    public class OrganizationController : OrganizationAggregateController, IRule<Organization>
    {
        #region Overrides of OrganizationAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Location.Name");
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return new List<BrokenBusinessRule>();
        }

        public override void FillList()
        {
        }

        #endregion

        #region Implementation of IRule<Organization>

        public ObjectRules<Organization> Rules
        {
            get { return new OrganizationRules(); }
        }

        #endregion

        #region CRUD

        #region Read

        public ActionResult Index()
        {
            PrePublish();

            AddToPath(MasterRecordOrder.First, RibbonLevels.Root, actionName: "Index",
                      areaName: OrganizationChartAreaRegistration.GetAreaName, nodeName: Resources.Areas.OrgChart.Views.Shared.Navigator.OrganizationChart,
                      controllerName: "Organization");

            return View("Index", Service.GetAll().SingleOrDefault());
        }

        public PartialViewResult Load()
        {
            PrePublish();

            AddToPath(MasterRecordOrder.First, RibbonLevels.Root, actionName: "Index",
                      areaName: OrganizationChartAreaRegistration.GetAreaName, nodeName: Resources.Areas.OrgChart.Views.Shared.Navigator.OrganizationChart,
                      controllerName: "Organization");

            if (Service.GetList().Count != 0)
            {
                Organization organization = Service.GetList().Single();

                if (ViewData != null && bool.Parse(ViewData["CanRead"].ToString()))
                {
                    return PartialView("Select", organization);
                }
            }
            else
            {
                if (ViewData != null && bool.Parse(ViewData["CanCreate"].ToString()))
                {
                    return PartialView("Edit", new Organization());
                }

                return ErrorPartialMessage(Resources.Shared.Messages.General.CanCreateMessage);
            }

            return ErrorPartialMessage(Resources.Shared.Messages.General.GeneralErrorOccurred);
        }

        public PartialViewResult Edit()
        {
            return PartialView("Edit", Service.GetAll().Single());
        }

        #endregion

        #region Create

        [HttpPost]
        public ActionResult Save(Organization organization)
        {
            PrePublish();

            if (organization.IsTransient())
            {
                var node = new Node { Code = "0000", Name = organization.Name, Type = null };
                organization.AddNode(node);
            }


            if ((Rules.GetBrokenRules(organization).Count == 0) && (TryValidateModel(organization)))
            {
                if (organization.Id != 0)
                {
                    var node = NodeService.GetAll().SingleOrDefault(x => x.Code == "0000");

                    if (node != null)
                    {
                        node.Name = organization.Name;
                    }

                    NodeService.Update(node);
                }

                Service.Update(organization);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(organization));
                return Json(new
                                {
                                    Success = true,
                                    PartialViewHtml = RenderPartialViewToString("Edit", organization)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.First, organization.Id);

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Select", Service.GetById(organization.Id))
                            });
        }

        #endregion

        #region Delete

        [HttpPost]
        public ActionResult Delete()
        {
            Organization org = Service.GetList().Single();

            try
            {
                Service.Delete(org);

                SetMasterRecordValue(MasterRecordOrder.First, 0);
            }
            catch (Exception exception)
            {
                //return ErrorPartialMessage(exception.Message);
                return Json(new
                                {
                                    Success = true,
                                    PartialViewHtml = exception.Message//RenderPartialViewToString("Edit", new Organization())
                                });
            }

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Edit", new Organization())
                            });
        }

        #endregion

        #endregion
    }
}