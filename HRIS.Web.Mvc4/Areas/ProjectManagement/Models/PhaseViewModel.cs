using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;

namespace Project.Web.Mvc4.Areas.ProjectManagement.Models
{
    public class PhaseViewModel:ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(PhaseViewModel).FullName;
        }

        public int PhaseId { get; set; }
        public string PhaseName { get; set; }
        public int Status { get; set; }
        public float CompletionPercent { get; set; }
        public float PhaseRate { get; set; }
    }
}