#region

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.OrgChart.ValueObjects;
using Service.OrgChart;
using UI.Areas.OrganizationChart.Controllers.EntitiesRoots;
using UI.Areas.OrganizationChart.Helpers;
using UI.Extensions;
using UI.Helpers.Cache;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.OrganizationChart.ValueObjects;

#endregion

namespace UI.Areas.OrganizationChart.Controllers.Entities
{
    public class NodeController : OrganizationAggregateController, IRule<Node>
    {
        #region IRule<Node> Members

        public ObjectRules<Node> Rules
        {
            get { return new NodeRules(); }
        }

        #endregion

        #region Overrides of NodeAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Type.Name");
            ModelState.Remove("Parent.Name");
        }

        #endregion

        #region CRUD

        #region Read

        public ActionResult LoadTree()
        {
            PrePublish();

            if (Service.GetList().Count == 0)
            {
                return RedirectToAction("Index", "Organization");
            }

            AddToPath(MasterRecordOrder.First, RibbonLevels.A, actionName: "LoadTree",
                      areaName: OrganizationChartAreaRegistration.GetAreaName, nodeName: Resources.Areas.OrgChart.Views.Shared.Navigator.OrganizationChart);


            return View("NodesTree");
            //return View("JqueryTree");
        }

        [HttpPost]
        public JsonResult GetSubTree(int nodeId)
        {
            return Json(new
                            {
                                Message = NodeService.GetById(nodeId).ToString()
                            }
                );
        }

        [HttpPost]
        public JsonResult JsonSelect(int nodeId)
        {
            Node tempNode = NodeService.LoadById(nodeId);

            #region To Disable The GoToPosition Link From The Select Partial

            if (tempNode.Code == "0000")
            {
                ViewData["RootNodeSelected"] = true;
            }
            else
            {
                ViewData["RootNodeSelected"] = false;
            }

            #endregion

            ClearMasterRecords();
            SetMasterRecordValue(MasterRecordOrder.First, nodeId);

            return Json(new
                            {
                                Success = true,
                                ParentName = tempNode.Name,
                                PartialViewHtml = RenderPartialViewToString("Select", NodeService.LoadById(nodeId))
                            }
                );
        }

        [HttpPost]
        public JsonResult GetNodeCode(string nodeID)
        {
            int iD = int.Parse(nodeID);
            Node temp = NodeService.GetById(iD);
            return Json(new
                            {
                                ParentName = temp.Name,
                                Message = temp.Code
                            }
                );
        }

        #endregion

        #region Update

        [HttpPost]
        public JsonResult JsonEdit(int nodeId)
        {
            Node selectedNode = NodeService.GetById(nodeId);

            if (selectedNode.Children.Count != 0)
            {
                ViewData["CanEditType"] = false;
            }
            else
            {
                ViewData["CanEditType"] = true;

                if (selectedNode.Type == null)
                {
                    ViewData["SelectedTypeId"] = 0;
                }
                else
                {
                    ViewData["SelectedTypeId"] = selectedNode.Type.NodeOrder;
                }
            }

            SetMasterRecordValue(MasterRecordOrder.First, nodeId);

            if (selectedNode.Code != "0000")
            {
                return Json(new
                                {
                                    Success = true,
                                    PartialViewHtml = RenderPartialViewToString("Edit", selectedNode)
                                }
                    );
            }

            return Json(new
                            {
                                Success = false,
                                Message = Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.EditRootMessage
                            }
                );
        }

        #endregion

        #region Create

        [HttpPost]
        public ActionResult JsonAdd(int nodeId)
        {
            Node node = NodeService.GetById(nodeId);

            SetMasterRecordValue(MasterRecordOrder.First, nodeId);

            #region To Check The Allowed Node Types In The DropDownList

            if (node.Type == null)
            {
                ViewData["SelectedTypeId"] = 0;
            }
            else
            {
                ViewData["SelectedTypeId"] = node.Type.NodeOrder;
            }

            #endregion

            return Json(new
                            {
                                PartialViewHtml = RenderPartialViewToString("New", new Node())
                            }
                );
        }

        [HttpPost]
        public ActionResult Save(Node node)
        {
            PrePublish();

            Node originalNode = NodeService.GetById(GetMasterRecordValue(MasterRecordOrder.First));

            if (node.IsTransient())
            {
                node.Parent = originalNode;
                originalNode.Children.Add(node);
            }
            else
            {
                node.Parent = originalNode.Parent;
                //node.Positions = originalNode.Positions;
                node.Children = originalNode.Children;

                if (originalNode.Children.Count > 0)
                {
                    node.Type = originalNode.Type;
                }
                if (node.Type.IsTransient())
                {
                    node.Type = originalNode.Type;
                }

                this.UpdateValueObject(node, originalNode);
            }

            if ((Rules.GetBrokenRules(node).Count == 0) && (TryValidateModel(node)))
            {
                NodeService.Update(originalNode);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(node));

                originalNode.Children.Remove(node);

                if (node.IsTransient())
                {
                    if (node.Parent.Type != null)
                    {
                        ViewData["SelectedTypeId"] = node.Parent.Type.Id;
                    }
                    else
                    {
                        ViewData["SelectedTypeId"] = 0;
                    }
                }

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("Edit", node)
                                });
            }

            PrePublish();

            CacheProvider.ForceUpdate(OrganizationChartCacheKeys.Node.ToString());

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Tree")
                            });
        }

        [HttpPost]
        public ActionResult SaveNew(Node node)
        {
            PrePublish();

            Node originalNode = NodeService.GetById(GetMasterRecordValue(MasterRecordOrder.First));

            if (node.IsTransient())
            {
                node.Parent = originalNode;
                originalNode.Children.Add(node);
            }
            else
            {
                node.Parent = originalNode.Parent;
                
                //node.Positions = originalNode.Positions;
                node.Children = originalNode.Children;

                if (originalNode.Children.Count > 0)
                {
                    node.Type = originalNode.Type;
                }
                if (node.Type.IsTransient())
                {
                    node.Type = originalNode.Type;
                }

                this.UpdateValueObject(node, originalNode);
            }

            if ((Rules.GetBrokenRules(node).Count == 0) && (TryValidateModel(node)))
            {
                NodeService.Update(originalNode);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(node));

                originalNode.Children.Remove(node);

                ViewData["SelectedTypeId"] = node.Parent.Type.Id;

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("New", node)
                                });
            }

            PrePublish();

            CacheProvider.ForceUpdate(OrganizationChartCacheKeys.Node.ToString());

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Tree")
                            });
        }

        #endregion

        #region Delete

        [HttpPost]
        public ActionResult Delete(int nodeId)
        {
            PrePublish();

            Node deletedNode = NodeService.GetById(nodeId);
            Node rootNode = Service.GetAll().First().RootNode.SingleOrDefault();

            if (deletedNode.Id != rootNode.Id)
            {
                NodeService.Delete(deletedNode);

                SetMasterRecordValue(MasterRecordOrder.First, deletedNode.Parent.Id);

                return Json(new
                                {
                                    Success = true,
                                    PartialViewHtml = RenderPartialViewToString("Tree")
                                }
                    );
            }

            return Json(new
                            {
                                Success = false,
                                Message = Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.DeleteRootMessage
                            }
                );
        }

        #endregion

        #endregion

        #region Positions

        public ActionResult GoToPositions(int nodeID = 0)
        {
            if (nodeID != 0)
            {
                SetMasterRecordValue(MasterRecordOrder.First, nodeID);
            }

            AddToPath(MasterRecordOrder.First, RibbonLevels.Root, actionName: "LoadTree",
                      areaName: OrganizationChartAreaRegistration.GetAreaName, nodeName: Resources.Areas.OrgChart.Views.Shared.Navigator.OrganizationChart);

            return RedirectToAction("Index", "Position");
        }

        #endregion

        #region Nodes Services

        #region Reorder

        public ActionResult LoadNodesReorder()
        {
            PrePublish();
            GetMasterRecordValue(MasterRecordOrder.First);

            if (Service.GetList().Count > 0)
            {
                return View("NodesTreeReorder", Service.GetList().Single().RootNode);
            }


            return RedirectToAction("Index", "Organization");
        }

        public ActionResult NodesReorder(int parentId, List<int> childrenIDs)
        {
            if (parentId == 0 || childrenIDs == null)
            {
                return Json(new
                                {
                                    Success = false,
                                    Message = Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.NodesReorderMergeErrorMessage
                                }
                    );
            }

            Node parentNode = NodeService.GetById(parentId);
            Node childNode;

            foreach (int index in childrenIDs)
            {
                childNode = NodeService.GetById(index);

                if (parentNode.Type != null)

                    if (childNode.Code != "0000")
                    {
                        if (childNode.Type.Id < parentNode.Type.Id)
                        {
                            return Json(new
                                            {
                                                Success = false,
                                                Message =
                                            string.Format(Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.NodesReorderTypeErrorMessage,
                                                parentNode.Name, childNode.Name)
                                            }
                                );
                        }

                        if (CheckRelation(parentNode, childNode))
                        {
                            return Json(new
                                            {
                                                Success = false,
                                                Message =
                                            string.Format(Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.NodesReorderParentsChainErrorMessage,
                                                parentNode.Name, childNode.Name)
                                            }
                                );
                        }
                    }
                    else
                    {
                        return Json(new
                                        {
                                            Success = false,
                                            Message = Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.MergedRootErrorMessage
                                        });
                    }
            }

            NodeHelper.ReassignNodesToNewParent(parentId, childrenIDs);

            return LoadNodesReorder();
        }

        private static bool CheckRelation(Node parent, Node child)
        {
            if (parent.Code == "0000")
            {
                return false;
            }

            if (child.Id == parent.Id)
            {
                return true;
            }

            return CheckRelation(parent.Parent, child);
        }

        #endregion

        #region Reassign

        public ActionResult LoadReassignPositions(int destinationId = 0, int sourceId = 0)
        {
            GetMasterRecordValue(MasterRecordOrder.Second);

            if (destinationId != 0 && sourceId != 0)
            {
                Node destinationNode = NodeService.GetById(destinationId);
                Node sourceNode = NodeService.GetById(sourceId);

                ViewData["SourceID"] = sourceId;
                ViewData["DestinationID"] = destinationId;

                ViewData["SourceNodeCode"] = sourceNode.Name;
                ViewData["DestinationNodeCode"] = destinationNode.Name;


                return View("ReassignPositions", NodeService.GetById(sourceId).Positions);
                //return Json(new
                //{
                //    Success = true,
                //    PartialViewHtml = RenderPartialViewToString("ReassignPositions", NodeService.GetById(SourceID).Positions)
                //});
            }

            return Json(new
                            {
                                Success = false,
                                Message = Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.SetDestinationSourceMessage
                            }
                );
        }

        public ActionResult ReassignPositions(int destination, int source, List<int> positionsIDs)
        {
            if (destination == 0 || source == 0)
            {
                return Json(new
                                {
                                    Success = false,
                                    Message = Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.SetDestinationSourceMessage
                                }
                    );
            }

            Node sourceNode = NodeService.GetById(source);

            if (sourceNode.Positions.Count == 0)
            {
                return Json(new
                                {
                                    Success = false,
                                    Message = Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.NoPositionsToMoveMessage
                                }
                    );
            }

            NodeHelper.MoveSelectedPositionsBetweenTwoNodes(destination, source, positionsIDs);

            return RedirectToAction("LoadReassignNodesPositions");
        }

        public ActionResult LoadReassignNodesPositions()
        {
            PrePublish();


            GetMasterRecordValue(MasterRecordOrder.First);


            if (Service.GetList().Count > 0)
            {
                return View("ReassignNodesPositions", Service.GetList().Single().RootNode);
            }

            return RedirectToAction("Index", "Organization");
        }

        public ActionResult ReassignNodesPositions(int destination, int source)
        {
            if (destination == 0 || source == 0)
            {
                return Json(new
                                {
                                    Success = false,
                                    Message = Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.SetDestinationSourceMessage
                                }
                    );
            }

            Node sourceNode = NodeService.GetById(source);

            if (sourceNode.Positions.Count == 0)
            {
                return Json(new
                                {
                                    Success = false,
                                    Message = Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.NoPositionsToMoveMessage
                                }
                    );
            }

            NodeHelper.MoveNodePositionsToAnotherNode(destination, source);

            return Json(new
                            {
                                Success = false,
                                Message = "Done"
                            }
                );
        }

        #endregion

        #endregion

        #region Tree Related

        #region Render Tree

        public ActionResult NodeToJson()
        {
            Node node = GetMasterRecordValue(MasterRecordOrder.First) != 0
                            ? NodeService.LoadById(GetMasterRecordValue(MasterRecordOrder.First))
                            : Service.GetAll().Single().RootNode.Single();

            string result = node.ToString();

            return Json(new
                            {
                                Success = true,
                                NodeId = node.Id,
                                NodeCode = node.Code,
                                Message = result
                            });
        }


        public ActionResult BackOneLevel(int reset = 0)
        {
            if (GetMasterRecordValue(MasterRecordOrder.First) != 0 & reset == 0)
            {
                Node node = NodeService.LoadById(GetMasterRecordValue(MasterRecordOrder.First));
                SetMasterRecordValue(MasterRecordOrder.First, node.Parent.Id);
            }
            else
            {
                SetMasterRecordValue(MasterRecordOrder.First, 0);
            }

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Tree")
                            });
        }


        public  ActionResult GenerateJqueryTree()
        {
           

            var finalHtmlString = @" <ul id=""org"" style=""display:none"">";
          //  var finalHtmlString = @" <ul id=""org"">";




            var rootNode = NodeService.GetAll().Single(x => x.Code == "0000");

            finalHtmlString += "<li>" + rootNode.Name + " <ul>";

            finalHtmlString = GenerateNodeHTML(rootNode, finalHtmlString);

            finalHtmlString += "</ul> </li> </ul>";
         
            return Json(new
            {
                Success = true,
                txt = finalHtmlString
            });

        }

        public string GenerateNodeHTML(Node node, string result)
        {


            foreach (var child in node.Children)
            {
                result += "<li>" + child.Name;

                var temp = string.Empty;
                if (child.Children.Count != 0)
                {
                    result += "<ul>" + GenerateNodeHTML(child, temp) + "</ul>";
                }
                else
                {
                    result += GenerateNodeHTML(child, temp);
                }


                result += "</li>";
            }



            return result;
        }

        public ActionResult LoadJqueryTree()
        {
            return View("JqueryTree");
        }


        #endregion

        #endregion
    }
}