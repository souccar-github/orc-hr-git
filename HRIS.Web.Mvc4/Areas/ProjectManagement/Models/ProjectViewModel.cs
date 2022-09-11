//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//project manager:
//supervisor:
//author: Ammar Alziebak
//description:
//start date:
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
using System;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;


namespace Project.Web.Mvc4.Areas.ProjectManagement.Models
{
    public class ProjectViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model,Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(ProjectViewModel).FullName;
        }

        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string PositionName { get; set; }
        public string RoleName { get; set; }
        public string Evaluator { get; set; }
        public int EvaluatorId { get; set; }
        public DateTime EvaluationDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public float ProjectRate { get; set; }
        public string Quarter { get; set; }
        public int EvaluationId { get; set; }
    }
}

