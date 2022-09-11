using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.JobDescription.Models
{
    public class DelegateRolResultViewModel:ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(DelegateRolResultViewModel).FullName;
            model.ActionListHandler = "initializeJobDesActionList";
            model.Views[0].ViewHandler = "DelegateRolesToPositionViewHandler";
           model.ToolbarCommands.RemoveAt(0);

        }

        public int FromPositionId { get; set; }
        public int ToPositionId { get; set; }
        public string ToPosition { get; set; }
        public bool PerformanceAppraisal { get; set; }
        public int SuperiorName { get; set; }
        public IList<int> RoleChecked { get; set; }
        public string Reason { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Comment { get; set; }

    }
}