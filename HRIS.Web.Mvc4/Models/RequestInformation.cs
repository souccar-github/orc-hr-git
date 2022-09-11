

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Models
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class RequestInformation
    {
        //After Apply Master Detail Feature
        public RequestInformation()
        {
            this.NavigationInfo = new Navigation();
        }
        
        public Navigation NavigationInfo { get; set; }
        public class Navigation
        {
            public Navigation()
            {
                this.Previous = new List<Step>();
                this.Next = new List<Step>();         
            }

            public Step Module { get; set; }
            public IList<Step> Next { get; set; }
            public IList<Step> Previous { get; set; }
            public string Status { get; set; }

            public class Step
            {
                public int RowId { get; set; }
                public string Name { get; set; }
                public string Description { get; set; }
                public string Url { get; set; }
                public string ImageClass { get; set; }
                public string CSSClass { get; set; }
                public string SmallImageClass { get; set; }
                public string Title { get; set; }
                public bool IsDetailHide { get; set; }
                public bool IsMasterContainsDetailsWithSameInterface { get; set; }
                public bool IsFromMasterDetail { get; set; }
                public bool IsMasterContainsThisDetail { get; set; }
                public string StepInformation { get; set; }
                public string TypeName { get; set; }
                public string GroupName { get; set; }
                public int GroupOrder { get; set; }
                public int PageSize { get; set; }
                public int PageNumber { get; set; }
                public int SkipElement { get; set; }
                public bool FromBreadcrumb { get; set; }
                public IEnumerable<GridSort> Sort { get; set; }
                public GridFilter Filter { get; set; }
                public IEnumerable<GridGroup> Group { get; set; }
            }

            public enum NavigationStatus
            {
                Module,
                Dashboard,
                Aggregate,
                Index,
                Service,
                Report,
                Notification,
                Configuaration
            }
        }
    }
}
