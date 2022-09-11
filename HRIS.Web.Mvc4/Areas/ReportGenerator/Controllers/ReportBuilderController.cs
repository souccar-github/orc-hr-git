using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Validation;
using Souccar.Core.Extensions;
using Souccar.Domain.Extensions;
using Souccar.Domain.PersistenceSupport;
using Souccar.Domain.Validation;
using Souccar.Reflector;
using Souccar.ReportGenerator.Domain.Classification;
using Souccar.ReportGenerator.Domain.QueryBuilder;
using Souccar.Web.Mvc.KendoGrid;
using Souccar.Infrastructure.Extenstions;
using FilterOperator = Souccar.ReportGenerator.Domain.QueryBuilder.FilterOperator;
using Reporting.DynamicReports;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using Souccar.NHibernate;
using DevExpress.Web.Mvc;
using System.Drawing;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Helpers;

namespace HRIS.Web.Mvc4.Areas.ReportGenerator.Controllers
{
    public class ReportBuilderController : Controller
    {
        private string _message = string.Empty;
        private bool _isSuccess;
        private List<ValidationResult> _validationResults;
        private Dictionary<string, string> _errorsMessages;
        private void InitialzeDefaultValues()
        {
            _isSuccess = false;
            _message = GlobalResource.FailMessage;
        }

        [HttpPost]
        public ActionResult Create()
        {
            return PartialView("Create1");
        }

        public ActionResult DisplayReport(int? id)
        {
            if (!id.HasValue) return PartialView();
            var report = (Report)typeof(Report).GetById(id.GetValueOrDefault());
            Session["Report"] = report;
            DynamicReport dynamicReport = new DynamicReport(CultureInfo.CurrentCulture, report, UserExtensions.CurrentUser, NHibernateSession.Current);
            XtraReport finalreport = (XtraReport)dynamicReport;
            return View(finalreport); 
        }

        public ActionResult GenerateReport(int? id)
        {
            if (!id.HasValue) return PartialView();
            var report = (Report)typeof(Report).GetById(id.GetValueOrDefault());
            DynamicReport dynamicReport = new DynamicReport(CultureInfo.CurrentCulture, report, UserExtensions.CurrentUser, NHibernateSession.Current);
            Session["XtraReport"] = (XtraReport)dynamicReport;
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Test()
        {
            XtraReport report = new XtraReport();
            return View(report);
        }

        [HttpPost]
        public ActionResult SaveQueryTree(QueryTree queryTree, RequestInformation requestInformation)
        {
            InitialzeDefaultValues();
            var report =
                ServiceFactory.ORMService.GetById<Report>(requestInformation.NavigationInfo.Previous[0].RowId);
            report.AddQuery(assignParentQueryTree(queryTree));
            Session["Report"] = report;
            report.Save(UserExtensions.CurrentUser);
            return null;
        }

        [HttpPost]
        public ActionResult SaveReport(Report report)
        {
            //test
            
            Report newReport = new Report()
            { 
                IsVertualDeleted=report.IsVertualDeleted,
                Name=report.Name,
                ReportResourceName=report.ReportResourceName,
                //ReportType=report.ReportType,
                Template = report.Template
            };
            if (report.QueryTreesList.Any())
            {
                newReport.QueryTreesList.Add(assignParentQueryTree(report.QueryTreesList.FirstOrDefault()));
            }
            
            Session["Report"] = newReport;
            newReport.Save();
            return null;
        }

        static XtraReport CreateProductsReport()
        {
            XtraReport report = new XtraReport();

            ReportHeaderBand headerBand = new ReportHeaderBand()
            {
                HeightF = 80
            };
            report.Bands.Add(headerBand);

            headerBand.Controls.Add(new XRLabel()
            {
                Text = "Product Report",
                SizeF = new SizeF(650, 80),
                TextAlignment = TextAlignment.BottomCenter,
                Font = new Font("Arial", 36)
            });


            DetailBand detailBand = new DetailBand();
            report.Bands.Add(detailBand);



            XRLabel lbCategoryName = new XRLabel()
            {
                LocationF = new PointF(200, 10),
                SizeF = new SizeF(440, 50),
                TextAlignment = TextAlignment.BottomLeft,
                Font = new Font("Arial", 24)
            };
            detailBand.Controls.Add(lbCategoryName);
            lbCategoryName.DataBindings.Add("Text", null, "FirstName");

            XRLabel lbDescription = new XRLabel()
            {
                LocationF = new PointF(200, 60),
                SizeF = new SizeF(440, 40),
                TextAlignment = TextAlignment.TopLeft,
                Font = new Font("Arial", 14, FontStyle.Italic)
            };
            detailBand.Controls.Add(lbDescription);
            lbDescription.DataBindings.Add("Text", null, "LastName");

            report.DataSource = typeof(EmployeeBase).GetAll<EmployeeBase>().ToList<EmployeeBase>();
            
            
            return report;
        }
        public ActionResult DocumentViewerPartial()
        {
            DynamicReport report = new DynamicReport(CultureInfo.CurrentCulture, (Report)Session["Report"], UserExtensions.CurrentUser, NHibernateSession.Current);
            XtraReport finalreport = (XtraReport)report;
           //finalreport.Bands.AddRange( report.Bands.ToArray<Band>());
           // finalreport.DataSource = report.DataSource;
            //ViewData["Report"] = finalreport;
            return PartialView("ReportResult", finalreport);
        }
        [HttpPost]
        public ActionResult DocumentViewerExport()
        {
            DynamicReport report = new DynamicReport(CultureInfo.CurrentCulture, (Report)Session["Report"], UserExtensions.CurrentUser, NHibernateSession.Current);
            XtraReport finalreport = (XtraReport)report;
            //finalreport.Bands.AddRange(report.Bands.ToArray<Band>());
            //finalreport.DataSource = report.DataSource;
            //return ReportViewerExtension.ExportTo(report);
            return ReportViewerExtension.ExportTo(finalreport);
        }

        public QueryTree assignQueryTreeTypes(QueryTree querytree)
        {
            if (querytree == null)
                return null;


            foreach (var queryleaf in querytree.Leaves)
                queryleaf.QueryTree = querytree;
            IList<QueryTree> qtnodes = new List<QueryTree>();
            foreach (var qt in querytree.Nodes)
                qtnodes.Add(assignParentQueryTree(qt));
            querytree.Nodes = qtnodes;
            return querytree;
        }

        //public QueryTree assignParentQueryTree(QueryTree querytree,Type parenttype=null)
        //{
        //    if (querytree == null)
        //        return null;
        //    querytree.Type = Type.GetType(querytree.FullClassName + ",HRIS.Domain");
        //    querytree.DefiningType = parenttype;
        //    if (querytree.Leaves != null )
        //        foreach (var queryleaf in querytree.Leaves)
        //        {

        //            queryleaf.QueryTree = querytree;
        //            queryleaf.DefiningType = querytree.Type;
        //            queryleaf.ParentType = querytree.Type;
        //            if (queryleaf.IsReference)
        //                continue;
        //            queryleaf.PropertyType = querytree.Type.GetProperty( queryleaf.PropertyName ).PropertyType;
        //        }
        //    IList<QueryTree> qtnodes = new List<QueryTree>();
        //    foreach (var qt in querytree.Nodes)
        //        qtnodes.Add(assignParentQueryTree(qt,querytree.Type));
        //    querytree.Nodes = qtnodes;
        //    return querytree;
        //}

        public QueryTree assignParentQueryTree(QueryTree querytree, Type parenttype = null, QueryTree parentQueryTree = null)
        {
            if (querytree == null)
                return null;
            querytree.Type = Type.GetType(querytree.FullClassName + ",HRIS.Domain");
            querytree.DefiningType = parenttype;
            querytree.Parent = parentQueryTree;
            if (querytree.Leaves != null)
                foreach (var queryleaf in querytree.Leaves)
                {
                    queryleaf.QueryTree = querytree;
                    queryleaf.DefiningType = querytree.Type;
                    queryleaf.ParentType = querytree.Type;
                    if (queryleaf.IsReference)
                        continue;
                    queryleaf.PropertyType = querytree.Type.GetProperty(queryleaf.PropertyName).PropertyType;
                }
            var qtnodes = querytree.Nodes.Select(qt => assignParentQueryTree(qt, querytree.Type, querytree)).ToList();
            querytree.Nodes = qtnodes;
            return querytree;
        }

        //public ActionResult GetDataSources(string aggregateClass)
        //{
        //    Type t = Assembly.GetAssembly(typeof(Employee)).GetType(aggregateClass);
        //    var queryTree = QueryTreeFactory.Create(t);
        //    var report = new Report
        //    {
        //        Id = 1, Name = "ReportName",
        //        QueryTree = queryTree,
        //        Template = new ReportTemplate {
        //            Id = 1, Name = "TempName", Content = new ReportTemplateContent { ShowHeader = false }
        //        }
        //    };
        //    //report.QueryTree.Leaves[0].AddFilter(FilterOperator.Contains, "0");
        //    //report.QueryTree.Leaves[6].AddFilter(FilterOperator.IsEqualTo, "1");
        //    //report.QueryTree.Leaves[1].AddFilter(FilterOperator.IsGreaterThan, DateTime.Now.ToString("dd/MM/yyyy"));
        //    return new Souccar.Web.Mvc.JsonNet.JsonNetResult(report);

        //}

        public ActionResult GetDataSources(string aggregateClass)
        {
            var assembly = Assembly.GetAssembly(typeof(Employee));
            Type t = assembly.GetType(aggregateClass);
            var queryTree = QueryTreeFactory.Create(t);
            //foreach(var node in queryTree.Nodes)
            //{
            //    GenerateNodes(node, assembly);
            //}
            var result = new Souccar.Web.Mvc.JsonNet.JsonNetResult(queryTree);
            return result;
        }

        public  void GenerateNodes(QueryTree queryTree, Assembly assembly)
        {
            var type = assembly.GetType(queryTree.FullClassName);
            var nodes = QueryTreeFactory.Create(type).Nodes;
            foreach (var node in nodes)
            {
                queryTree.AddNode(node);
            }
        }
        public ActionResult GetAvailableFilterOperators()
        {
            var filtersStringOperators = new[]
                {
                    new {text = FilterOperator.IsEqualTo.GetDescription(), value = FilterOperator.IsEqualTo},
                    new {text = FilterOperator.IsNotEqualTo.GetDescription(), value = FilterOperator.IsNotEqualTo},
                    new {text = FilterOperator.StartsWith.GetDescription(), value = FilterOperator.StartsWith},
                    new {text = FilterOperator.Contains.GetDescription(),value = FilterOperator.Contains},
                    new {text = FilterOperator.EndsWith.GetDescription(), value = FilterOperator.EndsWith}
                };

            var filtersNumericAndDateOperators = new[]
                {
                    new {text = FilterOperator.IsEqualTo.GetDescription(), value = FilterOperator.IsEqualTo},
                    new {text = FilterOperator.IsNotEqualTo.GetDescription(), value = FilterOperator.IsNotEqualTo},
                    new {text = FilterOperator.IsGreaterThan.GetDescription(), value = FilterOperator.IsGreaterThan},
                    new {text = FilterOperator.IsGreaterThanOrEqualTo.GetDescription(),value = FilterOperator.IsGreaterThanOrEqualTo},
                    new {text = FilterOperator.IsLessThan.GetDescription(), value = FilterOperator.IsLessThan},
                    new {text = FilterOperator.IsLessThanOrEqualTo.GetDescription(),value = FilterOperator.IsLessThanOrEqualTo}
                };

            var aggregateFilterOperators = new[]
                {
                    new{text = FilterOperator.IsEqualTo.GetDescription(), value = FilterOperator.IsEqualTo},
                    new{text = FilterOperator.IsNotEqualTo.GetDescription(), value = FilterOperator.IsNotEqualTo},
                    new{text = FilterOperator.IsGreaterThan.GetDescription(), value = FilterOperator.IsGreaterThan},
                    new{text = FilterOperator.IsGreaterThanOrEqualTo.GetDescription(), value = FilterOperator.IsGreaterThanOrEqualTo},
                    new{text = FilterOperator.IsLessThan.GetDescription(), value = FilterOperator.IsLessThan},
                    new{text = FilterOperator.IsLessThanOrEqualTo.GetDescription(), value = FilterOperator.IsLessThanOrEqualTo}
                };
            var aggregateFilterFunctions = new[]
                {
                    new {text = AggregateFunction.Count.GetDescription(), value = AggregateFunction.Count},
                    new {text = AggregateFunction.Avg.GetDescription(), value = AggregateFunction.Avg},
                    new {text = AggregateFunction.Max.GetDescription(), value = AggregateFunction.Max},
                    new {text = AggregateFunction.Min.GetDescription(), value = AggregateFunction.Min},
                    new {text = AggregateFunction.Sum.GetDescription(), value = AggregateFunction.Sum}
                };
            var result = new
            {
                numericAndDateOperators = filtersNumericAndDateOperators,
                stringOperators = filtersStringOperators,
                aggregateOperators = aggregateFilterOperators,
                aggregateFunctions = aggregateFilterFunctions
            };
            return new Souccar.Web.Mvc.JsonNet.JsonNetResult(result);
        }

        public ActionResult GetAggregates()
        {
            var aggregates = Assembly.GetAssembly(typeof(Employee))
                .GetAggregateClasses().Select(x => new { text = x.Value, value = x.Key.FullName }).OrderBy(x => x.text).ToList();
            return Json(aggregates);
        }
        public ActionResult GetReportTemplates()
        {
            var ReportTemplates = typeof(ReportTemplate).GetAll<ReportTemplate>().Select(x => new { text = x.Name, value = x.Id }).ToList();
            return Json(ReportTemplates);
        }

        public ActionResult Index()
        {
            return PartialView();
        }

        private readonly IRepository<Report> _repository;
        public ReportBuilderController(IRepository<Report> repository)
        {
            _repository = repository;
        }

        public ActionResult Create1()
        {
            return View("Create1");
        }
        //public ActionResult DisplayReports(DynamicReport report)
        //{
        //    return View(report);
        //}


        [HttpPost]
        public ActionResult GetGridModel(RequestInformation requestInformation)
        {
            var type = requestInformation.NavigationInfo.Previous.Last().TypeName.ToType();
            if (type == null)
            {
                requestInformation.NavigationInfo.Previous.Remove(requestInformation.NavigationInfo.Previous.Last());
                type = requestInformation.NavigationInfo.Previous.Last().TypeName.ToType();
            }
            requestInformation.NavigationInfo.Next.Clear();
            return Json(new { gridModel = GridViewModelCreate(type), requestInfo = requestInformation }, JsonRequestBehavior.AllowGet);
        }

        public Project.Web.Mvc4.Models.GridModel.GridViewModel GridViewModelCreate(Type type)
        {
            const int multiLinesStringMaxLength = GlobalConstant.MultiLinesStringMaxLength;
            int width = 165;
            var classTree = new ClassTree();
            classTree.SimpleProperties.Add(new SimpleProperty { Name = "Name", ClassName = "Report", PropertyType = typeof(string) });
            classTree.SimpleProperties.Add(new SimpleProperty { Name = "ReportResourceName", ClassName = "Report", PropertyType = typeof(string) });
            View view1 = new View()
            {
                Id = 0,
                Title = "Default",
                Type = Project.Web.Mvc4.Models.GridModel.ViewType.GridView
            };

            View view2 = new View()
            {
                Id = 1,
                Title = "Simple Properties",
                Type = Project.Web.Mvc4.Models.GridModel.ViewType.GridView
            };

            var model = new Project.Web.Mvc4.Models.GridModel.GridViewModel() { TypeFullName = type.FullName };
            model.Views.Add(view1);
            model.Views.Add(view2);

            model.ToolbarCommands.Add(new ToolbarCommand()
            {
                Name = BuiltinCommand.Create.ToString().ToLower(),
                Text = "Create"
            });

            model.ToolbarCommands.Add(new ToolbarCommand()
            {
                Text = "Clear Filters",
                ClassName = "k-grid-clear-filters",
                ImageClass = "k-icon k-clear-filter",
                Additional = false
            });

            model.ToolbarCommands.Add(new ToolbarCommand()
            {
                Text = "Clear Sorting",
                ClassName = "k-grid-clear-sorting",
                ImageClass = "k-icon k-delete",
                Additional = false
            });

            model.ToolbarCommands.Add(new ToolbarCommand()
            {
                Template = "GridViewSelector"
            });

            model.ToolbarCommands.Add(new ToolbarCommand()
            {
                Template = "AdditionalToolbarCommands"
            });

            if (type == typeof(Employee))
            {
                view2.Filter.Logic = FilterLogic.And.ToString().ToLower();
                view2.Filter.Filters = new List<Grid.GridFilter>();
                view2.Filter.Filters.Add(new Grid.GridFilter()
                {
                    Field = "FirstName",
                    Operator = FilterOperator.StartsWith.ToString().ToLower(),
                    Value = "n"
                });

                //Grid.GridFilter filter = new Grid.GridFilter() { Logic = FilterLogic.Or.ToString().ToLower() };
                //filter.Filters = new List<Grid.GridFilter>();
                //filter.Filters.Add(new Grid.GridFilter() { Field = "NoOfChildren", Operator = FilterOperator.Lt.ToString().ToLower(), Value = "100" });
                //filter.Filters.Add(new Grid.GridFilter() { Field = "NoOfChildren", Operator = FilterOperator.Gte.ToString().ToLower(), Value = "10" });
                //view2.Filter.Filters.Add(filter);

                //view2.Filter.Filters.Add(new Grid.GridFilter() { Field = "SocialSecurityNo", Operator = FilterOperator.Eq.ToString().ToLower(), Value = "1121243123" });

                width = 165;
                model.ActionListHandler = "initializeEmployeeActionList";

                //model.EditHandler = "initializeEmployeeTemplate";
                //model.EditorTemplate = "EmployeeTemplate";
            }

            model.ActionList.Commands.Add(new ActionListCommand() { GroupId = 2, Order = 3, HandlerName = "showInformation", Name = "View", ShowCommand = true });
            model.ActionList.Commands.Add(new ActionListCommand() { GroupId = 2, Order = 2, HandlerName = "destroy", Name = "Delete", ShowCommand = true });
            model.ActionList.Commands.Add(new ActionListCommand() { GroupId = 2, Order = 1, HandlerName = "update", Name = "Edit", ShowCommand = true });

            model.ActionList.Commands.Add(new ActionListCommand() { GroupId = 1, Order = 1, HandlerName = "haha_actionlist", Name = "Additional", ShowCommand = true });

            model.ActionList.GroupsCount = 2;
            model.ActionList.OrderCommands();

            int columnOrder = 1;
            List<ValidationRules> validationRules = type.GetValidators();

            foreach (var property in classTree.SimpleProperties)
            {
                var prop = type.GetProperty(property.Name);
                var field = new Field()
                {
                    Name = property.Name,
                    Type = GridViewModelHelper.GetFieldTypeName(property.PropertyType, prop).ToString().ToLower(),
                    Editable = type.GetProperty(property.Name).CanWrite
                };

                if (columnOrder == 1)
                    view2.SortFields.Add(field.Name, GridSortDirection.Desc.ToString().ToLower());
                else if (columnOrder == 2)
                    view2.SortFields.Add(field.Name, GridSortDirection.Asc.ToString().ToLower());

                var title = property.Name;
                type.TryGetPropertyLocalizedName(property.Name, out title);

                var nativeValidationRules = validationRules.SingleOrDefault(x => x.PropertyName == property.Name);
             //   generateValidationRules(field, title, nativeValidationRules);
                model.SchemaFields.Add(field);

                var columnType = ColumnType.Simple;
                if (nativeValidationRules != null)
                {
                    var validator =
                        nativeValidationRules.Validators.SingleOrDefault(v => v.ValidatorType == ValidatorType.MaxLength);
                    if (validator != null)
                    {
                        var max = Convert.ToInt32(validator.ValidatorRules.First().Parameters.First());
                        if (max == multiLinesStringMaxLength)
                            columnType = ColumnType.TextArea;
                    }
                }

                var column = new Column()
                {
                    Width = width,
                    Title = title,
                    Order = columnOrder++,
                    FieldName = property.Name,
                    Type = columnType.ToString(),
                    Sortable = true,
                    Filterable = true,
                    Hidden = false
                };

                view1.Columns.Add(column);
                view2.Columns.Add(column);
                view2.OrderColumns();
            }

            foreach (var referencesProperty in classTree.ReferencesProperties.Where(p => p.PropertyType.IsIndex()).ToList())
            {
                var field = new Field()
                {
                    Name = referencesProperty.Name,
                    Type = Project.Web.Mvc4.Models.GridModel.FieldType.Complex.ToString().ToLower(),
                    Editable = type.GetProperty(referencesProperty.Name).CanWrite
                };

                var title = referencesProperty.Name;
                type.TryGetPropertyLocalizedName(referencesProperty.PropertyType.Name, out title);

               // generateValidationRules(field, title, validationRules.SingleOrDefault(x => x.PropertyName == referencesProperty.Name));
                model.SchemaFields.Add(field);

                ColumnType columnType = ColumnType.DropDown;
                if (referencesProperty.PropertyType == typeof(Nationality))
                    columnType = ColumnType.AutoComplete;

                var column = new Column()
                {
                    Width = width,
                    Title = title,
                    Order = columnOrder++,
                    FieldName = referencesProperty.Name,
                    Type = columnType.ToString(),
                    IndexName = referencesProperty.PropertyType.Name,
                    TypeFullName = referencesProperty.PropertyType.FullName,
                    Sortable = false,
                    Filterable = true,
                    Hidden = false
                };

                column.ValueField = "Id";
                var simpleProperty = referencesProperty.ClassTree.SimpleProperties.FirstOrDefault(s => s.IsPrimaryKey);
                if (simpleProperty != null)
                {
                    column.ValueField = simpleProperty.Name;
                }

                column.TextField = "Name";
                simpleProperty = referencesProperty.ClassTree.SimpleProperties.FirstOrDefault(s => !s.IsPrimaryKey);
                if (simpleProperty != null)
                {
                    column.TextField = simpleProperty.Name;
                }

                view1.Columns.Add(column);
            }

            if (type.Name.Contains("ReportTemplate"))
            {
                foreach (var view in model.Views)
                {
                    view.EditorMode = GridEditorMode.Popup.ToString().ToLower();
                    view.EditorTemplate = "ReportTemplateEditor";
                    view.EditHandler = "initializeReportTemplateEditor";
                    view.CreateUrl = "ReportGenerator/ReportTemplate/Create";
                    view.UpdateUrl = "ReportGenerator/ReportTemplate/Update";
                    view.DestroyUrl = "ReportGenerator/ReportTemplate/Delete";
                    // generateReportAdditionalFields(model, columnOrder);
                }
                
            }

            view1.OrderColumns();

            return model;
        }


        //#region Query Tree Operations

        //private void ReserveQueryTree(QueryTree queryTree)
        //{
        //    TempData["queryTree"] = queryTree;
        //}

        //private QueryTree GetReservedQueryTree()
        //{
        //    return (QueryTree)TempData["queryTree"];
        //}

        //public ActionResult LoadReservedQueryTree()
        //{
        //    var queryTree = GetReservedQueryTree();
        //    var lstQueryTree = new List<QueryTree> { queryTree };

        //    ReserveQueryTree(queryTree);
        //    return Json(new
        //    {
        //        Success = true,
        //        PartialViewHtml = RenderPartialViewToString("Tree", lstQueryTree),
        //    });
        //}

        //public ActionResult LoadQueryTree(string aggregateFullClassName)
        //{
        //    Type t = Assembly.GetAssembly(typeof(Employee)).GetType(aggregateFullClassName);
        //    var queryTree = QueryTreeFactory.Create(t);

        //    var lstQueryTree = new List<QueryTree> { queryTree };

        //    ReserveQueryTree(queryTree);
        //    return Json(new
        //    {
        //        Success = true,
        //        PartialViewHtml = RenderPartialViewToString("Tree", lstQueryTree),
        //    });
        //}

        //public ActionResult LoadQueryLeaf(string nodefullClassPath, string aggregateFullClassName)
        //{
        //    var queryTree = GetReservedQueryTree();
        //    var selectedNode = queryTree.FindByFullClassPath(nodefullClassPath);


        //    var stringLeaf = queryTree.Leaves.FirstOrDefault(x => x.PropertyType == typeof(string));
        //    var otherLeaf = queryTree.Leaves.FirstOrDefault(x => x.PropertyType != typeof(string));
        //    ViewData["StringOperators"] = stringLeaf == null ? null : stringLeaf.GetAvailableFilterOperators().ToList().SelectFromList(x => x.Key.ToString(), y => y.Value);
        //    ViewData["OtherOperators"] = otherLeaf == null ? null : otherLeaf.GetAvailableFilterOperators().ToList().SelectFromList(x => x.Key.ToString(), y => y.Value);
        //    ViewData["QueryLeafProperties"] = selectedNode.Leaves.ToList().SelectFromList(x => x.PropertyFullPath, y => y.DisplayName);

        //    ReserveQueryTree(queryTree);

        //    return Json(new
        //    {
        //        Success = true,
        //        PartialViewHtml = RenderPartialViewToString("TreeLeaf", selectedNode),
        //    });
        //} 

        //public ActionResult UpdateQueryLeaf(string nodefullClassPath, string aggregateFullClassName,
        //    List<string> selectedFields, List<string> selectedGroupingFields, List<string> selectedSortingFields, List<string> selectedSortDirection,
        //    List<string> selectedcmbLeafs, List<string> selectedcmbOperators, List<string> selectedtxtFilterValues, string lastTreeNodeSelectedValue,
        //    List<string> aggregateFields, List<string> aggregateFunction, List<string> aggregateOperators, List<string> aggregateValues)
        //{
        //    TempData["lastTreeNodeSelectedValue"] = lastTreeNodeSelectedValue;
        //    var queryTree = GetReservedQueryTree();

        //    var node = queryTree.FindByFullClassPath(nodefullClassPath);

        //    #region Reset Default Values

        //    for (int i = 0; i < node.Leaves.Count; i++)
        //    {
        //        node.Leaves[i].Selected = 0;
        //        node.Leaves[i].GroupDescriptor.GroupByOrder = 0;
        //        node.Leaves[i].SortDescriptor.SortOrder = 0;
        //        node.Leaves[i].SortDescriptor.SortDirection = ListSortDirection.Ascending;
        //    }

        //    node.AggregateFilters.Clear();

        //    for (int i = 0; i < node.Leaves.Count; i++)
        //    {
        //        node.Leaves[i].FilterDescriptors.Clear();
        //    }

        //    #endregion

        //    for (int i = 0; i < selectedFields.Count; i++)
        //    {
        //        var leave = node.Leaves.Find(x => x.PropertyFullPath == selectedFields[i]);
        //        if (leave != null)
        //        {
        //            leave.Selected = i + 1;
        //        }
        //    }
        //    for (int i = 0; i < selectedGroupingFields.Count; i++)
        //    {
        //        var leave = node.Leaves.Find(x => x.PropertyFullPath == selectedGroupingFields[i]);
        //        if (leave != null)
        //        {
        //            leave.GroupDescriptor.GroupByOrder = (uint)i + 1;
        //        }

        //    }
        //    for (int i = 0; i < selectedSortingFields.Count; i++)
        //    {
        //        var leave = node.Leaves.Find(x => x.PropertyFullPath == selectedSortingFields[i]);
        //        if (leave != null)
        //        {
        //            leave.SortDescriptor.SortOrder = (uint)i + 1;
        //            leave.SortDescriptor.SortDirection = selectedSortDirection[i] == "Asc" ? ListSortDirection.Ascending : ListSortDirection.Descending;
        //        }

        //    }

        //    for (int i = 0; i < selectedcmbLeafs.Count; i++)
        //    {
        //        var leave = node.Leaves.Find(x => x.PropertyFullPath == selectedcmbLeafs[i]);
        //        if (leave == null || selectedcmbOperators[i] == "" || selectedtxtFilterValues[i] == "")
        //        {
        //            continue;
        //        }
        //        leave.AddFilter((FilterOperator)Enum.Parse(typeof(FilterOperator), selectedcmbOperators[i]), selectedtxtFilterValues[i]);
        //    }

        //    for (int i = 0; i < aggregateFields.Count; i++)
        //    {
        //        if (String.IsNullOrEmpty(aggregateValues[i]))
        //        {
        //            continue;
        //        }
        //        node.AddAggregateFilter(new AggregateFilterDescriptor
        //        {
        //            AggregateFunction = (AggregateFunction)Enum.Parse(typeof(AggregateFunction), aggregateFunction[i]),
        //            FilterOperator = (FilterOperator)Enum.Parse(typeof(FilterOperator), aggregateOperators[i]),
        //            PropertyName = aggregateFields[i],
        //            Value = aggregateValues[i]
        //        });
        //    }

        //    var stringLeaf = queryTree.Leaves.FirstOrDefault(x => x.PropertyType == typeof(string));
        //    var otherLeaf = queryTree.Leaves.FirstOrDefault(x => x.PropertyType != typeof(string));
        //    ViewData["StringOperators"] = stringLeaf == null ? null : stringLeaf.GetAvailableFilterOperators().ToList().SelectFromList(x => x.Key.ToString(), y => y.Value);
        //    ViewData["OtherOperators"] = otherLeaf == null ? null : otherLeaf.GetAvailableFilterOperators().ToList().SelectFromList(x => x.Key.ToString(), y => y.Value);

        //    ViewData["QueryLeafProperties"] = node.Leaves.ToList().SelectFromList(x => x.PropertyFullPath, y => y.DisplayName);


        //    ReserveQueryTree(queryTree);

        //    return Json(new
        //    {
        //        Success = true,
        //        PartialViewHtml = RenderPartialViewToString("TreeLeaf", queryTree),
        //    });
        //}

        //public ActionResult GetAvailableFilterOperators(string leafPropertyFullName, string nodefullClassPath)
        //{
        //    var queryTree = GetReservedQueryTree();
        //    var selectedNode = queryTree.FindByFullClassPath(nodefullClassPath);
        //    ReserveQueryTree(queryTree);
        //    var leaf = selectedNode.FindLeafByPropertyFullPath(leafPropertyFullName);
        //    var availableFilterOperators = leaf.GetAvailableFilterOperators();


        //    return Json(new { Data = availableFilterOperators.ToList().SelectFromList(x => x.Key.ToString(), y => y.Value), ContentType = leaf.PropertyType.Name });
        //    //return new JsonResult { Data = availableFilterOperators.ToList().SelectFromList(x => x.Key.ToString(), y => y.Value), 
        //    //    ContentType = leaf.PropertyType.Name};
        //}

        //public ActionResult LoadFilterRow(string nodefullClassPath)
        //{
        //    var queryTree = GetReservedQueryTree();
        //    ReserveQueryTree(queryTree);
        //    var node = queryTree.FindByFullClassPath(nodefullClassPath);
        //    ViewData["QueryLeafProperties"] = node.Leaves.ToList().SelectFromList(x => x.PropertyFullPath, y => y.DisplayName);

        //    return Json(new
        //    {
        //        Success = true,
        //        PartialViewHtml = RenderPartialViewToString("FilterRow", queryTree),
        //    });
        //}

        //public ActionResult LoadAggregateFilterRow(string nodefullClassPath)
        //{
        //    var queryTree = GetReservedQueryTree();
        //    ReserveQueryTree(queryTree);

        //    return Json(new
        //    {
        //        Success = true,
        //        PartialViewHtml = RenderPartialViewToString("AggregateFilterRow", queryTree),
        //    });
        //}

        //public void ReorderNodes(string sourceNodeId, string destinationNodeId, string parentNodeId, string dropPosition)
        //{
        //    var queryTree = GetReservedQueryTree();

        //    var sourceNodeOrder = queryTree.FindByFullClassPath(sourceNodeId).SelectOrder;
        //    var destinationNodeOrder = queryTree.FindByFullClassPath(destinationNodeId).SelectOrder;
        //    var parentNode = queryTree.FindByFullClassPath(parentNodeId);
        //    if (dropPosition == "after")
        //    {
        //        destinationNodeOrder++;
        //    }
        //    if (sourceNodeOrder < destinationNodeOrder && dropPosition == "before")
        //    {
        //        destinationNodeOrder--;
        //    }
        //    parentNode.ChangeOrder(sourceNodeOrder, destinationNodeOrder);

        //    ReserveQueryTree(queryTree);
        //}

        //#endregion

    }
}
