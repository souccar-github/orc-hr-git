using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.PMS.Configurations;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.RootEntities;
using HRIS.Domain.Recruitment.Configurations;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Domain.Recruitment.RootEntities;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Module = Project.Web.Mvc4.Models.Navigation.Module;
using Project.Web.Mvc4.Models.Navigation;
using HRIS.Domain.Recruitment.Entities.Evaluations;

namespace Project.Web.Mvc4.Areas.Recruitment.Models
{
    public class RecruitmentAdjustment : ModelAdjustment
    {
        private static Dictionary<string, ViewModel> parent = new Dictionary<string, ViewModel>();
        public override ViewModel AdjustGridModel(string type)
        {
            var assembly = typeof(ViewModel).Assembly;
            var viewModelType = assembly.GetType($"Project.Web.Mvc4.Areas.Recruitment.Models.{type}ViewModel");

            return (ViewModel)Activator.CreateInstance(viewModelType);
        }

        public override void AdjustModule(Module module)
        {
            module.Services.Add(new Service()
            {
                Controller = "Recruitment/Interview",
                Action = "ApplicantsEvaluation",
                Title = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.ApplicantsEvaluation),
                ServiceId = "ApplicantsEvaluation",
                SecurityId = "ApplicantsEvaluation"
            });

            if (module.ModuleId.Equals(ModulesNames.Recruitment))
            {
                var applicantDetails = new List<string>()
                {
                    "RChildren","RSpouse","REducations","RJobRelatedInfos"
                };
                var applicant = module.Aggregates.SingleOrDefault(x => x.TypeFullName == typeof(Applicant).FullName);
                applicant.Details = DetailFactory.Create(typeof(Applicant));
                applicant.Details = applicant.Details.Where(x => applicantDetails.Contains(x.DetailId))
                        .ToList();

                var jobApplication =
                    module.Aggregates.SingleOrDefault(x => x.TypeFullName == typeof(JobApplication).FullName);

                var detail = jobApplication.Details.FirstOrDefault(x => x.Name == typeof(Interview).Name);
                jobApplication.Details.Remove(detail);

                var interview = module.Aggregates.SingleOrDefault(x => x.TypeFullName == typeof(Interview).FullName);
                if (interview != null)
                {
                    var evaluator = interview.Details.FirstOrDefault(x => x.Name == typeof(Evaluator).Name);
                    interview.Details.Remove(evaluator);
                }

                var appraisalTemplate = module.Configurations.FirstOrDefault(x => x.TypeFullName == typeof(AppraisalTemplate).FullName);
                if (appraisalTemplate != null)
                {
                    var templateSectionWeight = appraisalTemplate.Details.FirstOrDefault(x => x.Name == typeof(TemplateSectionWeight).Name);
                    appraisalTemplate.Details.Remove(templateSectionWeight);
                }

                module.Dashboards.Add(new Dashboard()
                {
                    Title = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.RecruitmentDashboard),
                    Controller = "Recruitment/Dashboard",
                    Action = "RecruitmentDashboard",
                    DashboardId = "RecruitmentDashboard",
                    SecurityId = "RecruitmentDashboard"
                });


                module.Aggregates.Remove(module.Aggregates.SingleOrDefault(x => x.TypeFullName == typeof(Applicant).FullName));
                module.Aggregates.Remove(module.Aggregates.SingleOrDefault(x => x.TypeFullName == typeof(Advertisement).FullName));
                module.Configurations.Remove(module.Configurations.SingleOrDefault(x => x.TypeFullName == typeof(EvaluationSettings).FullName));
                
            }


        }
    }
}