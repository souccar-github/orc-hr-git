using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Models.GridModel
{
    //After Apply Master Detail Feature
    public enum ColumnType
    {
        Simple,
        TextArea,
        DropDown,
        AutoComplete,
    }

    public enum AggregatesType
    {
        Sum,
        Min,
        Max,
        Count,
        Average
    }

    [Serializable]
    public class Column
    {
        public Column()
        {
            GroupAggregates = new List<string>();
            GlobalAggregates = new List<string>();
        }
        public int Order { get; set; }
        public string Type { get; set; }

        public int Width { get; set; }
        public bool Hidden { get; set; }
        public string Title { get; set; }
        public string FieldName { get; set; }
        public string ImagePath { get; set; }
        public string DefaultImageName { get; set; }
        public string DetailName { get; set; }
        public string GroupName { get; set; }
        public string TypeFullName { get; set; }
        
        public string IndexName { get; set; }
        public string TextField { get; set; }
        public string ValueField { get; set; }

        public bool Editable { get; set; }
        public bool Sortable { get; set; }
        public bool Filterable { get; set; }

        public bool IsRequired { get; set; }
        public bool ShowCommaSeparator { get; set; }
        public int Step { get; set; }
        public string ReadUrl { get; set; }
        public bool HasParent { get; set; }
        public string CascadeFrom { get; set; }
        public string CreateUrl { get; set; }
        public bool ShowAddButton { get; set; }
        public bool ShowInfoButton { get; set; }

        public bool IsFile { get; set; }
        public bool IsDateTime { get; set; }
        public bool IsTime { get; set; } 
        public string FileAcceptExtension { get; set; }
        public int FileSize { get; set; }

        public IList<string> GroupAggregates { get; set; }
        public void AddGroupAggregate(AggregatesType aggregatesType)
        {
            GroupAggregates.Add(aggregatesType.ToString().ToLower());
        }

        public IList<string> GlobalAggregates { get; set; }
        public void AddGlobalAggregate(AggregatesType aggregatesType)
        {
            GlobalAggregates.Add(aggregatesType.ToString().ToLower());
        }

        public string FooterTemplate { get; set; }    
    }
}