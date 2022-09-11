using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.JobDescription.Models
{




   


    public class DelegateAuthResultViewModel :ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(DelegateAuthResultViewModel).FullName;
            model.ActionListHandler = "initializeJobDesActionList";
            model.Views[0].ViewHandler = "DelegateAuthoritiesToPositionViewHandler";
            model.ToolbarCommands.RemoveAt(0);

        }

        public int FromPositionId { get; set; }
        public int ToPositionId { get; set; }
        public string ToPosition { get; set; }
        public IList<int> AuthChecked { get; set; }
        public string Reason { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Comment { get; set; }
        
    }
}