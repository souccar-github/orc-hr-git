using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using HRIS.Domain.Personnel.Entities;
using Souccar.Core;
using Souccar.ReportGenerator.Domain.QueryBuilder;
using Telerik.Web.Mvc;
using UI.Extensions;
using Infrastructure.Validation;
using UI.Areas.Reporting.Controllers.EntitiesRoots;
using UI.Helpers.Model;
using Validation.Reporting;
using AggregateFunction = Souccar.ReportGenerator.Domain.QueryBuilder.AggregateFunction;
using FilterOperator = Souccar.ReportGenerator.Domain.QueryBuilder.FilterOperator;

namespace UI.Areas.Reporting.Controllers
{
    public class QueryBuilderController : ReportAggregateController, IRule<Report>
    {
        #region IRule<Report> Members

        public ObjectRules<Report> Rules
        {
            get { return new QueryBuilderRules(); }
        }

        #endregion

        #region Overrides of ReportAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Template.Name");
        }

        #endregion

        #region CRUD

        #region Read

        [GridAction]
        public ActionResult Index()
        {
            PrePublish();

            #region Get Data

            var reports = Service.GetAll();
            ViewData["reports"] = reports;

            #endregion

            return View();
        }

        #endregion

        #region Create & Update

        public ActionResult Insert(int id = 0)
        {
            LoadStepsList();

            if (id != 0)
            {
                Report report = Service.GetById(id);
                ReserveQueryTree(report.QueryTree);
                return View("Insert", report);
            }

            return View("Insert", new Report());
        }

        [HttpPost]
        public ActionResult Save(Report report)
        {
            var queryTree = GetReservedQueryTree();
            report.QueryTree = queryTree;
            PrePublish();

            if ((Rules.GetBrokenRules(report).Count == 0))
            {
                try
                {
                    Service.Update(report);
                }
                catch (Exception ex)
                {
                    ReserveQueryTree(queryTree);
                    ModelState.AddModelError(DomainErrors.InternalError.ToString(), ex.Message);
                    return View("Insert", report);
                }
            }
            else
            {
                ReserveQueryTree(queryTree);
                ModelState.AddModelErrors(Rules.GetBrokenRules(report));
                return View("Insert", report);
            }

            PrePublish();

            return RedirectToAction("Index", new { id = report.Id });

        }

        #endregion

        #region Delete

        [HttpPost]
        public ActionResult Delete(int id)
        {
            PrePublish();

            try
            {
                Report report = Service.LoadById(id);
                Service.Delete(report);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(DomainErrors.InternalError.ToString(), ex.Message);
            }

            return RedirectToAction("Index");
        }

        #endregion

        #endregion

        #region Query Tree Operations

        private void ReserveQueryTree(QueryTree queryTree)
        {
            TempData["queryTree"] = queryTree;
        }

        private QueryTree GetReservedQueryTree()
        {
            return (QueryTree)TempData["queryTree"];
        }

        public ActionResult LoadReservedQueryTree()
        {
            var queryTree = GetReservedQueryTree();
            var lstQueryTree = new List<QueryTree> { queryTree };

            ReserveQueryTree(queryTree);
            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("Tree", lstQueryTree),
            });
        }

        public ActionResult LoadQueryTree(string aggregateFullClassName)
        {
            Type t = Assembly.GetAssembly(typeof(Employee)).GetType(aggregateFullClassName);
            var queryTree = QueryTreeFactory.Create(t);

            var lstQueryTree = new List<QueryTree> { queryTree };

            ReserveQueryTree(queryTree);
            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("Tree", lstQueryTree),
            });
        }

        public ActionResult LoadQueryLeaf(string nodefullClassPath, string aggregateFullClassName)
        {
            var queryTree = GetReservedQueryTree();
            var selectedNode = queryTree.FindByFullClassPath(nodefullClassPath);


            var stringLeaf = queryTree.Leaves.FirstOrDefault(x => x.PropertyType == typeof(string));
            var otherLeaf = queryTree.Leaves.FirstOrDefault(x => x.PropertyType != typeof(string));
            ViewData["StringOperators"] = stringLeaf == null ? null : stringLeaf.GetAvailableFilterOperators().ToList().SelectFromList(x => x.Key.ToString(), y => y.Value);
            ViewData["OtherOperators"] = otherLeaf == null ? null : otherLeaf.GetAvailableFilterOperators().ToList().SelectFromList(x => x.Key.ToString(), y => y.Value);

            ViewData["QueryLeafProperties"] = selectedNode.Leaves.ToList().SelectFromList(x => x.PropertyFullPath, y => y.DisplayName);

            ReserveQueryTree(queryTree);

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("TreeLeaf", selectedNode),
            });
        }

        public ActionResult UpdateQueryLeaf(string nodefullClassPath, string aggregateFullClassName,
            List<string> selectedFields, List<string> selectedGroupingFields, List<string> selectedSortingFields,List<string> selectedSortDirection, 
            List<string> selectedcmbLeafs, List<string> selectedcmbOperators,List<string> selectedtxtFilterValues, string lastTreeNodeSelectedValue,
            List<string> aggregateFields, List<string> aggregateFunction, List<string> aggregateOperators, List<string> aggregateValues)
        {
            TempData["lastTreeNodeSelectedValue"] = lastTreeNodeSelectedValue;
            var queryTree = GetReservedQueryTree();

            var node = queryTree.FindByFullClassPath(nodefullClassPath);

            #region Reset Default Values

            for (int i = 0; i < node.Leaves.Count; i++)
            {
                node.Leaves[i].Selected = 0;
                node.Leaves[i].GroupDescriptor.GroupByOrder = 0;
                node.Leaves[i].SortDescriptor.SortOrder = 0;
                node.Leaves[i].SortDescriptor.SortDirection = ListSortDirection.Ascending;
            }

            node.AggregateFilters.Clear();

            for (int i = 0; i < node.Leaves.Count; i++)
            {
                node.Leaves[i].FilterDescriptors.Clear();
            }

            #endregion

            for (int i = 0; i < selectedFields.Count; i++)
            {
                var leave = node.Leaves.Find(x => x.PropertyFullPath == selectedFields[i]);
                if (leave != null)
                {
                    leave.Selected = i + 1;
                }
            }
            for (int i = 0; i < selectedGroupingFields.Count; i++)
            {
                var leave = node.Leaves.Find(x => x.PropertyFullPath == selectedGroupingFields[i]);
                if (leave != null)
                {
                    leave.GroupDescriptor.GroupByOrder = (uint)i + 1;
                }

            }
            for (int i = 0; i < selectedSortingFields.Count; i++)
            {
                var leave = node.Leaves.Find(x => x.PropertyFullPath == selectedSortingFields[i]);
                if (leave != null)
                {
                    leave.SortDescriptor.SortOrder = (uint)i + 1;
                    leave.SortDescriptor.SortDirection = selectedSortDirection[i] == "Asc" ? ListSortDirection.Ascending : ListSortDirection.Descending;
                }

            }
            
            for (int i = 0; i < selectedcmbLeafs.Count; i++)
            {
                var leave = node.Leaves.Find(x => x.PropertyFullPath == selectedcmbLeafs[i]);
                if (leave == null || selectedcmbOperators[i] == "" || selectedtxtFilterValues[i] == "")
                {
                    continue;
                }
                leave.AddFilter((FilterOperator)Enum.Parse(typeof(FilterOperator), selectedcmbOperators[i]), selectedtxtFilterValues[i]);
            }

            for (int i = 0; i < aggregateFields.Count; i++)
            {
                if (String.IsNullOrEmpty(aggregateValues[i]))
                {
                    continue;
                }
                node.AddAggregateFilter(new AggregateFilterDescriptor
                {
                    AggregateFunction = (AggregateFunction)Enum.Parse(typeof(AggregateFunction), aggregateFunction[i]),
                    FilterOperator = (FilterOperator)Enum.Parse(typeof(FilterOperator), aggregateOperators[i]),
                    PropertyName = aggregateFields[i],
                    Value = aggregateValues[i]
                });
            }

            var stringLeaf = queryTree.Leaves.FirstOrDefault(x => x.PropertyType == typeof(string));
            var otherLeaf = queryTree.Leaves.FirstOrDefault(x => x.PropertyType != typeof(string));
            ViewData["StringOperators"] = stringLeaf == null ? null : stringLeaf.GetAvailableFilterOperators().ToList().SelectFromList(x => x.Key.ToString(), y => y.Value);
            ViewData["OtherOperators"] = otherLeaf == null ? null : otherLeaf.GetAvailableFilterOperators().ToList().SelectFromList(x => x.Key.ToString(), y => y.Value);

            ViewData["QueryLeafProperties"] = node.Leaves.ToList().SelectFromList(x => x.PropertyFullPath, y => y.DisplayName);


            ReserveQueryTree(queryTree);

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("TreeLeaf", queryTree),
            });
        }

        public ActionResult GetAvailableFilterOperators(string leafPropertyFullName, string nodefullClassPath)
        {
            var queryTree = GetReservedQueryTree();
            var selectedNode = queryTree.FindByFullClassPath(nodefullClassPath);
            ReserveQueryTree(queryTree);
            var leaf = selectedNode.FindLeafByPropertyFullPath(leafPropertyFullName);
            var availableFilterOperators = leaf.GetAvailableFilterOperators();


            return Json(new { Data = availableFilterOperators.ToList().SelectFromList(x => x.Key.ToString(), y => y.Value), ContentType = leaf.PropertyType.Name });
            //return new JsonResult { Data = availableFilterOperators.ToList().SelectFromList(x => x.Key.ToString(), y => y.Value), 
            //    ContentType = leaf.PropertyType.Name};
        }

        public ActionResult LoadFilterRow(string nodefullClassPath)
        {
            var queryTree = GetReservedQueryTree();
            ReserveQueryTree(queryTree);
            var node = queryTree.FindByFullClassPath(nodefullClassPath);
            ViewData["QueryLeafProperties"] = node.Leaves.ToList().SelectFromList(x => x.PropertyFullPath, y => y.DisplayName);

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("FilterRow", queryTree),
            });
        }
        
        public ActionResult LoadAggregateFilterRow(string nodefullClassPath)
        {
            var queryTree = GetReservedQueryTree();
            ReserveQueryTree(queryTree);

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("AggregateFilterRow", queryTree),
            });
        }

        public void ReorderNodes(string sourceNodeId, string destinationNodeId, string parentNodeId, string dropPosition)
        {
            var queryTree = GetReservedQueryTree();
            
            var sourceNodeOrder = queryTree.FindByFullClassPath(sourceNodeId).SelectOrder;
            var destinationNodeOrder = queryTree.FindByFullClassPath(destinationNodeId).SelectOrder;
            var parentNode = queryTree.FindByFullClassPath(parentNodeId);
            if (dropPosition == "after")
            {
                destinationNodeOrder++;
            }
            if (sourceNodeOrder < destinationNodeOrder && dropPosition == "before")
            {
                destinationNodeOrder--;
            }
            parentNode.ChangeOrder(sourceNodeOrder, destinationNodeOrder);

            ReserveQueryTree(queryTree);
        }

        #endregion
    }
}
