using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.Entities;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using  Project.Web.Mvc4.Models.Navigation;
using  Project.Web.Mvc4.Factories;
using HRIS.Domain.JobDescription.Entities;
using  Project.Web.Mvc4.Helpers.Resource;

namespace Project.Web.Mvc4.Areas.JobDescription.Models
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class JobDescriptionAdjustment: ModelAdjustment
    {
        private static Dictionary<string, ViewModel> parent = new Dictionary<string, ViewModel>();
        public override void AdjustModule(Module module)
        {

            
            if (module.ModuleId.Equals(ModulesNames.JobDescription))
            {

                var aggregate = module.Aggregates.SingleOrDefault(x => x.TypeFullName == typeof(HRIS.Domain.JobDescription.RootEntities.JobDescription).FullName);
                var details = aggregate.Details.SingleOrDefault(x => x.DetailId == "Roles");
                details.Details = DetailFactory.Create(typeof(Role));

                var subDetails = details.Details.SingleOrDefault(x => x.DetailId == "Responsibilities");
                subDetails.Details = DetailFactory.Create(typeof(Responsibility));

                var positionDetails = new List<string>()
                {
                    //"DelegateAuthoritiesFromPositions","DelegateAuthoritiesToPositions","DelegateRolesToPositions","DelegateRolesFromPositions","DelegateRolesAsSuperiorPositions",
                    "PositionBenefitDetails","PositionDeductionDetails"
                };
                module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(Position).FullName))
                    .Details = module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(Position).FullName))
                        .Details.Where(x => positionDetails.Contains(x.DetailId))
                        .ToList();

            }
            
            //module.Services.Add(new Service()
            //{
            //    Controller = "Incentive/Service",
            //    Action = "IncentiveAppraisalService",
            //    Title = " Incentive Appraisal",
            //    ServiceId = " IncentiveAppraisal",
            //    SecurityId = " IncentiveAppraisal"
            //});

            //module.Services.Add(new Service()
            //{
            //    Title = JobDescriptionLocalizationHelper.GetResource(JobDescriptionLocalizationHelper.DelegateRolesToPositionService),
            //    ServiceId = "DelegateRolesToPositionService",
            //    SecurityId = "DelegateRolesToPositionService",
            //    Controller = "JobDescription/Service",
            //    Action = "DelegateRolesToPositionService"
            //});

            //module.Services.Add(new Service()
            //{
            //    Title = JobDescriptionLocalizationHelper.GetResource(JobDescriptionLocalizationHelper.DelegateAuthoritiesToPositionService),
            //    ServiceId = "DelegateAuthoritiesToPositionService",
            //    SecurityId = "DelegateAuthoritiesToPositionService",
            //    Controller = "JobDescription/Service",
            //    Action = "DelegateAuthoritiesToPositionService"
            //});
        }

        public override ViewModel AdjustGridModel(string type)
        {





            if (parent.Count == 0)
            {
                parent.Add("JobDescription", new JobDescriptionViewModel());
                parent.Add("Position", new PositionViewModel());
                parent.Add("Responsibility", new ResponsibilityViewModel());
                parent.Add("Role", new RoleViewModel());
                parent.Add("JLanguage", new JLanguageViewModel());
                parent.Add("PositionCode", new PositionCodeViewModel());
                parent.Add("Competence", new CompetenceViewModel());
                parent.Add("CompetenceCategory", new CompetenceCategoryViewModel()); 
                parent.Add("CompetenceCategoryLevelDescription", new CompetenceCategoryLevelDescriptionViewModel());
                parent.Add("JExperience", new JExperienceViewModel());
                parent.Add("JEducation", new JEducationViewModel()); 
                parent.Add("JSkill", new JSkillViewModel());
                parent.Add("ComputerSkill", new ComputerSkillViewModel());
                parent.Add("Knowledge", new KnowledgeViewModel());
                parent.Add("PositionStatus", new PositionStatusViewModel());
                parent.Add("DelegateAuthoritiesToPosition", new DelegateAuthResultViewModel());
                parent.Add("DelegateAuthoritiesToPositionAuthority", new DelegateAuthResultViewModel());
                parent.Add("DelegateRolesToPosition", new DelegateRolResultViewModel());
                parent.Add("DelegateRolesToPositionRole", new DelegateRolResultViewModel());


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