using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using Souccar.Web.Mvc.KendoGrid;
using Souccar.Infrastructure.Core;
using Souccar.Core.Extensions;
namespace Project.Web.Mvc4.Models.GridModel
{
    public enum ViewType
    {
        GridView
    }

    public enum GridSortDirection
    {
        Desc,
        Asc
    }


    [Serializable]
    public class View
    {
        public View()
        {
            this.Columns = new List<Column>();

            Filter = new Grid.GridFilter();
            SortFields = new Dictionary<string, string>();

            ReadUrl = "Crud/Read";
            CreateUrl = "Crud/Create";
            UpdateUrl = "Crud/Update"; 
            //DestroyUrl = "Crud/Destroy";
            DestroyUrl = "Crud/Delete";

            EditHandler = "";
            GridHandler = "";
            ViewHandler = "";
            EditorTemplate = "";

            ServerPaging = true;
            ServerFiltering = true;
            ServerSorting = true;
            ShowGroup = true;
            //ShowTwoColumns = true;
            IsDetailOutSideGrid = true;
            EditorMode = GridEditorMode.Popup.ToString().ToLower();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public ViewType Type { get; set; }
        public bool IsDetailOutSideGrid { get; set; }
        public string EditorMode { get; set; }
        public string AfterRequestEnd { get; set; }
        public string EditHandler { get; set; }
        public string GridHandler { get; set; }
        public string DataBoundHandler { get; set; }
        public string ViewHandler { get; set; }
        public string EditorTemplate { get; set; }

        public string ReadUrl { get; set; }
        public string CreateUrl { get; set; }
        public string UpdateUrl { get; set; }
        public string DestroyUrl { get; set; }

        public bool ShowGroup { get; set; }
        public bool ShowTwoColumns { get; set; }

        public IList<Group> Groups
        {
            get
            {
                return Columns.Where(x => !x.FieldName.Equals("Id")&&!string.IsNullOrEmpty(x.GroupName)&&!x.Hidden).Select(x => x.GroupName).Distinct()
                    .Select(x =>
                        new Group()
                        {
                            Title=ServiceFactory.LocalizationService.GetResource(x) ?? x.ToCapitalLetters(),
                            Name = x,
                            Columns = Columns.Where(y => y.GroupName==x).Select(y => new GroupItem()
                            {
                                FieldName = y.FieldName,
                                Title= y.Title,
                                Type=y.Type.ToString(),
                                Order = y.Order,
                                IsRequired = y.IsRequired,
                            }).OrderBy(y => y.Order).ToList()
                        }).OrderBy(x => x.Order).ToList();

            }
        }


        public IList<Column> Columns { get;  set; }

        public Grid.GridFilter Filter { get; set; }
        public IDictionary<string, string> SortFields { get; private set; }

        public bool ServerPaging { get; set; }
        public bool ServerSorting { get; set; }
        public bool ServerFiltering { get; set; }
        public bool ServerAggregates { get; set; }

        public void OrderColumns()
        {
            var maxOrder = Columns.Max(x => x.Order);
            foreach (var column in Columns.Where(x=>x.Order==0))
            {
                column.Order = maxOrder;
            }
            this.Columns = this.Columns.OrderBy(c => c.Order).ToList();
        }
    }

    public class Group
    {
        public string Name { get; set; }
        public string Title { get; set; }

        public int Order 
        {
            get
            {
                return Columns.Min(x => x.Order);
            }
        }


        public List<GroupItem> Columns { get; set; }
    }
    public class GroupItem
    {
        public string FieldName { get; set; }
        public string Title { get; set; }
        public bool IsRequired { get; set; }
        public int Order { get; set; }
        public string Type{ get; set; }

    }

}