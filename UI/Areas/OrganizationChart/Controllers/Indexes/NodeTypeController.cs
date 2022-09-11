#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.OrgChart.Indexes;
using HRIS.Domain.OrgChart.ValueObjects;
using Telerik.Web.Mvc.Extensions;
using UI.Areas.OrganizationChart.Controllers.EntitiesRoots;
using UI.Areas.OrganizationChart.Helpers;
using UI.Extensions;
using UI.Helpers.Cache;
using UI.Helpers.Model;
using Validation.OrganizationChart.Indexes;

#endregion

namespace UI.Areas.OrganizationChart.Controllers.Indexes
{
    public class NodeTypeController : IndexesControllerOrgChart<NodeType>, IRule<NodeType>
    {
        #region IRule<NodeType> Members

        public ObjectRules<NodeType> Rules
        {
            get { return new NodeTypeRules(); }
        }

        #endregion

        public ActionResult Index()
        {
            PrePublish();

            IQueryable<NodeType> list = Service.GetAll();

            ViewData["nodeTypes"] = list.OrderBy(o => o.NodeOrder);

            return View();
        }

        [HttpPost]
        public ActionResult Save(NodeType nodeType)
        {
            if ((Rules.GetBrokenRules(nodeType).Count == 0) && (TryValidateModel(nodeType)))
            {
                var semilarTypes = Service.GetAll().Where(x => x.NodeOrder == nodeType.NodeOrder);

                if (semilarTypes.Count() == 0)
                {
                    Service.Update(nodeType);
                    CacheProvider.ForceUpdate(OrganizationChartCacheKeys.NodeType.ToString());

                    return Json(new
                                    {
                                        Success = true
                                    });
                }

                var error = new List<BrokenBusinessRule>
                                {
                                    new BrokenBusinessRule("NodeOrder",
                                                           "You Already Have The Same Order In Your Types")
                                };

                ModelState.AddModelErrors(error);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("Create", nodeType)
                                });
            }

            ModelState.AddModelErrors(Rules.GetBrokenRules(nodeType));
            return Json(new
                            {
                                Success = false,
                                PartialViewHtml = RenderPartialViewToString("Create", nodeType)
                            });
        }

        public ActionResult Edit(int id)
        {
            NodeType nodeType = Service.LoadById(id);

            return PartialView("Edit", nodeType);
        }

        public ActionResult Insert()
        {
            return PartialView("Create", new NodeType());
        }

        [HttpPost]
        public ActionResult JsonEdit(NodeType nodeType)
        {
            NodeType nodeTypeOriginal = Service.LoadById(nodeType.Id);

            if (nodeTypeOriginal.NodeOrder != nodeType.NodeOrder)
            {
                List<Node> nodeList = NodeService.GetAll().Where(x => x.Type.Id == nodeType.Id).ToList();

                if ((Rules.GetBrokenRules(nodeType).Count == 0) && (TryValidateModel(nodeType)))
                {
                    if (nodeList.Count != 0)
                    {
                        var error = new List<BrokenBusinessRule>
                                        {
                                            new BrokenBusinessRule("NodeOrder", "You Can't Edit In Use Node's Order")
                                        };

                        ModelState.AddModelErrors(error);

                        return Json(new
                                        {
                                            Success = false,
                                            PartialViewHtml = RenderPartialViewToString("Edit", nodeType)
                                        });
                    }


                    this.UpdateValueObject(nodeType, nodeTypeOriginal);

                    Service.Update(nodeTypeOriginal);
                    CacheProvider.ForceUpdate(OrganizationChartCacheKeys.NodeType.ToString());

                    return Json(new
                                    {
                                        Success = true
                                    });
                }

                ModelState.AddModelErrors(Rules.GetBrokenRules(nodeType));

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("Edit", nodeType)
                                });
            }

            if ((Rules.GetBrokenRules(nodeType).Count == 0) && (TryValidateModel(nodeType)))
            {
                this.UpdateValueObject(nodeType, nodeTypeOriginal);
                Service.Update(nodeTypeOriginal);
                CacheProvider.ForceUpdate(OrganizationChartCacheKeys.NodeType.ToString());

                return Json(new
                                {
                                    Success = true
                                });
            }

            ModelState.AddModelErrors(Rules.GetBrokenRules(nodeType));

            return Json(new
                            {
                                Success = false,
                                PartialViewHtml = RenderPartialViewToString("Edit", nodeType)
                            });
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            NodeType nodeType = Service.GetById(int.Parse(id));
            try
            {
                if (TryUpdateModel(nodeType))
                {
                    var nodeList = NodeService.GetAll().Where(x => x.Type.Id == nodeType.Id);
                    if (nodeList.Count() == 0)
                    {
                        Service.Delete(nodeType);
                        CacheProvider.ForceUpdate(OrganizationChartCacheKeys.NodeType.ToString());
                        return RedirectToAction("Index", this.GridRouteValues());
                    }
                    else
                    {
                        ViewData["Error"] = "You Can't Delete This Node";
                        IQueryable<NodeType> list = Service.GetAll();

                        ViewData["nodeTypes"] = list.OrderBy(o => o.NodeOrder);
                        return View("Index");
                    }
                }
                return ErrorPartialMessage("You Can't Delete This Node");
            }
            catch (Exception)
            {
                return ErrorPartialMessage("You Cant' Delete This Node");
            }
        }

        public ActionResult PartialMasterInfo(int selectedRowId = 0)
        {
            PrePublish();

            if (selectedRowId != 0)
            {
                NodeType nodeType = Service.LoadById(selectedRowId);

                return PartialView("BasicInfo", nodeType);
            }

            return ErrorPartialMessage("Error Occurred !!");
        }
    }
}