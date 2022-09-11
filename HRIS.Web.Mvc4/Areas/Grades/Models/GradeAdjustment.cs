using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.PayrollSystem.Entities;
using Project.Web.Mvc4.Areas.JobDescription.Models;
using Project.Web.Mvc4.Areas.OrganizationChart.Models;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Project.Web.Mvc4.Models.Navigation;
using Project.Web.Mvc4.Helpers;

namespace Project.Web.Mvc4.Areas.Grades.Models
{
    public class GradeAdjustment:ModelAdjustment
    {
        private static Dictionary<string, ViewModel> parent = new Dictionary<string, ViewModel>();
        public override void AdjustModule(Module module)
        {
            var grade = module.Aggregates.SingleOrDefault(x => x.TypeFullName == typeof(HRIS.Domain.Grades.RootEntities.Grade).FullName);
            var jobTitle = grade.Details.SingleOrDefault(x => x.TypeFullName == typeof(JobTitle).FullName);
            jobTitle.Details = DetailFactory.Create(typeof(JobTitle));

            module.Dashboards.Add(new Dashboard()
            {
                Title = GlobalResource.Dashboard,
                Controller = "Grades/Dashboard",
                Action = "GradeDashboard",
                DashboardId = "GradeDashboard",
                SecurityId = "GradeDashboard"
            });

        }

        public override ViewModel AdjustGridModel(string type)
        {
            if (parent.Count == 0)
            {
                parent.Add("Grade", new GradeViewModel());
                parent.Add("GradeByEducation", new GradeByEducationViewModel());
                parent.Add("GradeStep", new GradeStepViewModel());
                parent.Add("GradeByEducationQualification", new GradeByEducationQualificationViewModel());
                parent.Add("GradeBenefitDetail", new GradeBenefitDetailViewModel());
                parent.Add("GradeDeductionDetail", new GradeDeductionDetailViewModel());
                parent.Add("JobTitleBenefitDetail", new JobTitleBenefitDetailViewModel());
                parent.Add("JobTitle", new JobTitleViewModel());
                parent.Add("JobTitleDeductionDetail", new JobTitleDeductionDetailViewModel());


            }
            try
            {
                return parent[type];
            }
            catch
            {

                return new ViewModel();
            }





        }
   
    }
}