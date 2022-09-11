using System.Linq.Dynamic;
using System.Runtime.InteropServices;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.JobDescription.RootEntities;
using HRIS.Domain.Objectives.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.PMS.RootEntities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using Project.Web.Mvc4.Areas.Appraisal;
using Souccar.Core.Fasterflect;
using Souccar.Infrastructure.Core;
using HRIS.Domain.PMS.Entities.JobDescription;
using HRIS.Domain.PMS.Entities.objective;
using HRIS.Domain.PMS.Entities.Organizational;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.Entities.Competency;
using HRIS.Domain.PMS.Configurations;
using Project.Web.Mvc4.Areas.Appraisals;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers.Resource;

namespace Project.Web.Mvc4.Helpers
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class AppraisalViewModelFactory
    {
        
        public static AppraisalViewModel Create(Position position, AppraisalTemplate template, AppraisalPhase phase, int phaseWorkflowId = 0)
        {
            var result = new AppraisalViewModel();
            
                      if(phase.Id!=null)
               result.PhaseId = phase.Id;
            var appraisalPhaseWorkflow = phaseWorkflowId != 0 ? ServiceFactory.ORMService.All<AppraisalPhaseWorkflow>().FirstOrDefault(x => x.Id == phaseWorkflowId) : ServiceFactory.ORMService.All<AppraisalPhaseWorkflow>().OrderByDescending(x=>x.WorkflowItem.Date).FirstOrDefault(x => x.Position == position);
            result.EmployeeId = appraisalPhaseWorkflow.Employee.Id;
            result.EmployeeName = appraisalPhaseWorkflow.Employee.TripleName;

            result.WorkflowId = appraisalPhaseWorkflow.WorkflowItem.Id;
            result.ShowRejectButton = appraisalPhaseWorkflow.WorkflowItem.FirstUser == UserExtensions.CurrentUser ? true : false;
            result.PhaseSettingId = phase.AppraisalPhaseSetting.Id;
            result.PhaseSettingTypeName = typeof(AppraisalPhaseSetting).FullName;


            result.MinMark = phase.AppraisalPhaseSetting.FromMark;
            result.MaxMark = phase.AppraisalPhaseSetting.ToMark;

            result.Step = phase.AppraisalPhaseSetting.MarkStep;
            result.StrongLimit = phase.AppraisalPhaseSetting.SkillThreshold;
            result.WeaknessLimit = phase.AppraisalPhaseSetting.GapThreshold;

            result.IsHiddenCompetanceSection = !template.Competency;
            result.IsHiddenJobDescriptionSection = !template.JobDescription;
            result.IsHiddenObjectiveSection = !template.Objective;

            var jobDescription = position.JobDescription;

            if (!result.IsHiddenCompetanceSection)
            {
                result.CompetenceSection = GetCompetenceSection(jobDescription, template.CompetencyWeight);
            }

            if (!result.IsHiddenJobDescriptionSection)
            {
                result.JobDescriptionSection = GetJobDescriptionSection(jobDescription,
                    template.JobDescriptionWeight);
            }

            if (!result.IsHiddenObjectiveSection)
            {
                var objectives = ServiceFactory.ORMService.All<Objective>().Where(x => x.Owner == position);
                result.ObjectiveSection = GetObjectiveSection(objectives, template.ObjectiveWeight);
            }

            foreach (var customSection in template.TemplateSectionWeights)
            {
                result.CustomSections.Add(new CustomSectionViewModel()
                {
                    Name = customSection.AppraisalSection.Name,
                    SectionWeight = customSection.Weight,
                    Description = customSection.AppraisalSection.Description,
                    Id = customSection.AppraisalSection.Id,
                    AppraisalItems = customSection.AppraisalSection.Items.Select(x => new AppraisalSectionItemViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Weight = x.Weight,
                        Kpis = x.Kpis.Select(y => new KpiViewModel() { Description = y.Description, Value = y.Value, Weight = y.Weight }).ToList()
                    }).ToList()

                });
            }

            return result;
        }

        private static CompetenceSectionViewModel GetCompetenceSection(JobDescription jobDescription,
            float sectionWeight)
        {
            var result = new CompetenceSectionViewModel()
            {
                JobDescriptionDescription = jobDescription.Summary,
                JobDescriptionName = jobDescription.Name,
                JobTitle = jobDescription.JobTitle.Name,
                SectionWeight = sectionWeight,
                AppraisalItems = jobDescription.Competencies.Select(x => new AppraisalCompetenceViewModel()
                {
                    Id = x.Id,
                    Name = x.CompetenceCategory.Name.Name,
                    Level = x.Level != null ? x.Level.Level.Name : "",
                    Type = x.Type != null ? x.Type.Name : "",
                    Weight = x.Weight
                }).ToList()
            };
            return result;
        }


        private static JobDescriptionSectionViewModel GetJobDescriptionSection(JobDescription jobDescription,
            float sectionWeight)
        {
            var result = new JobDescriptionSectionViewModel()
            {
                Description = jobDescription.Summary,
                SectionWeight = sectionWeight,
                Name = jobDescription.Name,
                JobTitle = jobDescription.JobTitle.Name,
                Roles = jobDescription.Roles.Select(x => new AppraisalRoleViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Summary,
                    Weight = x.Weight,
                    Kpis = x.RoleKpis.Select(y => new KpiViewModel()
                    { Description = y.Description, Value = y.Value, Weight = y.Weight }).ToList(),
                    AppraisalItems = x.Responsibilities.Select(y => new AppraisalResponsibilityViewModel()
                    {
                        Id = y.Id,
                        Name = y.Description,
                        Weight = y.Weight,
                        Kpis = y.ResponsibilityKpis.Select(k => new KpiViewModel()
                        { Description = k.Description, Value = k.Value, Weight = k.Weight }).ToList()
                    }).ToList()
                }).ToList()
            };
            return result;
        }

        private static ObjectiveSectionViewModel GetObjectiveSection(IEnumerable<Objective> objectives,
            float sectionWeight)
        {
            var result = new ObjectiveSectionViewModel()
            {
                Description = "Description",
                SectionWeight = sectionWeight,
                AppraisalItems = objectives.Select(x => new AppraisalObjectiveViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    EvaluationMark = x.IsFinishedAndEvaluated ? x.Mark.ToString() : ObjectiveLocalizationHelper.GetResource(ObjectiveLocalizationHelper.TheEvaluationProcessIsNotFinished),

                    Description = x.Description,
                    Weight = x.Weight,
                    Kpis = x.Kpis.Select(y => new KpiViewModel()
                    { Description = y.Description, Value = y.Value, Weight = y.Weight }).ToList()
                }).ToList()
            };
            return result;
        }
        
        public static float UpdateAppraisalFromViewModel(Appraisal appraisal, AppraisalViewModel appraisalViewModel)
        {
            appraisal.JobDescriptionSections.Clear();
            appraisal.AppraisalCompetences.Clear();
            appraisal.OrganizationalSections.Clear();
            appraisal.ObjectiveSections.Clear();
            appraisal.Step.Description = appraisalViewModel.Note;
            #region Weight
            appraisal.CompetenceSectionWeight = appraisalViewModel.IsHiddenCompetanceSection
                ? 0
                : appraisalViewModel.CompetenceSection.SectionWeight;

            appraisal.JobDescriptionSectionWeight = appraisalViewModel.IsHiddenJobDescriptionSection
                ? 0
                : appraisalViewModel.JobDescriptionSection.SectionWeight;

            appraisal.ObjectiveSectionWeight = appraisalViewModel.IsHiddenObjectiveSection
                ? 0
                : appraisalViewModel.ObjectiveSection.SectionWeight;
            #endregion
            #region custom section
            foreach (var section in appraisalViewModel.CustomSections)
            {
                var appraisalSection = new AppraisalCustomSection()
                {
                    Section = ServiceFactory.ORMService.GetById<AppraisalSection>(section.Id),
                    Weight = section.SectionWeight
                };
                foreach (var itemViewModel in section.AppraisalItems)
                {
                    var appraisalItem = new AppraisalCustomSectionItem()
                    {
                        Item = ServiceFactory.ORMService.GetById<AppraisalSectionItem>(itemViewModel.Id),
                        Description = itemViewModel.Note,
                        Weight = itemViewModel.Weight,
                        Rate = itemViewModel.Rate
                    };
                    appraisalSection.AddAppraisalCustomSectionItem(appraisalItem);
                }
                appraisal.AddOrganizationalSection(appraisalSection);
            }

            #endregion
            #region Competence
            //-------------------CompetanceSection-------------
            if (!appraisalViewModel.IsHiddenCompetanceSection)
            {
                foreach (var item in appraisalViewModel.CompetenceSection.AppraisalItems)
                {
                    var appraisalItem = new AppraisalCompetence()
                    {
                        Competence = ServiceFactory.ORMService.GetById<Competence>(item.Id),
                        Description = item.Note,
                        Weight = item.Weight,
                        Rate = item.Rate
                    };

                    appraisal.AddCompetencySection(appraisalItem);
                }
            }
            #endregion
            #region Job Description
            if (!appraisalViewModel.IsHiddenJobDescriptionSection)
            {
                foreach (var role in appraisalViewModel.JobDescriptionSection.Roles)
                {
                    foreach (var item in role.AppraisalItems)
                    {
                        var appraisalItem = new AppraisalJobDescription()
                        {
                            Responsibility = ServiceFactory.ORMService.GetById<Responsibility>(item.Id),
                            Description = item.Note,
                            Weight = item.Weight,
                            Rate = item.Rate
                        };
                        appraisal.AddJobDescriptionSection(appraisalItem);
                    }
                }
            }
            #endregion
            #region Objective
            if (!appraisalViewModel.IsHiddenObjectiveSection)
            {
                foreach (var item in appraisalViewModel.ObjectiveSection.AppraisalItems)
                {
                    var appraisalItem = new AppraisalObjective()
                    {
                        Objective = ServiceFactory.ORMService.GetById<HRIS.Domain.Objectives.RootEntities.Objective>(item.Id),
                        Description = item.Note,
                        Weight = item.Weight,
                        Rate = item.Rate
                    };

                    appraisal.AddObjectiveSection(appraisalItem);
                }
            }
            #endregion
            appraisal.UpdateAppraisalValue();
            return appraisal.AppraisalValue;
        }

        public static void UpdateViewModelFromAppraisal(Appraisal appraisal, AppraisalViewModel appraisalViewModel)
        {

            appraisalViewModel.Note = appraisal.Step.Description != null ? appraisal.Step.Description : string.Empty;
            #region custom section

            foreach (var viewModelItem in appraisalViewModel.CustomSections)
            {
                var section = appraisal.OrganizationalSections.SingleOrDefault(x => x.Section.Id == viewModelItem.Id);
                foreach (var appraisalItem in viewModelItem.AppraisalItems)
                {
                    if (section == null)
                        continue;
                    var item = section.AppraisalCustomSectionItems.SingleOrDefault(x => x.Item.Id == appraisalItem.Id);
                    if (item == null)
                        continue;
                    appraisalItem.Weight = item.Weight;
                    appraisalItem.Rate = item.Rate;
                    appraisalItem.Note = item.Description;
                }
            }

            #endregion
            #region Competence
            if (!appraisalViewModel.IsHiddenCompetanceSection)
            {
                foreach (var appraisalItem in appraisalViewModel.CompetenceSection.AppraisalItems)
                {
                    var item = appraisal.AppraisalCompetences.SingleOrDefault(x => x.Competence.Id == appraisalItem.Id);
                    if (item == null)
                        continue;
                    appraisalItem.Weight = item.Weight;
                    appraisalItem.Rate = item.Rate;
                    appraisalItem.Note = item.Description;
                }
            }
            #endregion
            #region Job Description
            if (!appraisalViewModel.IsHiddenJobDescriptionSection)
            {
                foreach (var appraisalItem in appraisalViewModel.JobDescriptionSection.Roles.SelectMany(x => x.AppraisalItems))
                {
                    var item = appraisal.JobDescriptionSections.SingleOrDefault(x => x.Responsibility.Id == appraisalItem.Id);
                    if (item == null)
                        continue;
                    appraisalItem.Weight = item.Weight;
                    appraisalItem.Rate = item.Rate;
                    appraisalItem.Note = item.Description;
                }
            }
            #endregion
            #region Objective
            if (!appraisalViewModel.IsHiddenObjectiveSection)
            {
                foreach (var appraisalItem in appraisalViewModel.ObjectiveSection.AppraisalItems)
                {
                    var item = appraisal.ObjectiveSections.SingleOrDefault(x => x.Objective.Id == appraisalItem.Id);
                    if (item == null)
                        continue;
                    appraisalItem.Weight = item.Weight;
                    appraisalItem.Rate = item.Rate;
                    appraisalItem.Note = item.Description;
                }
            }
            #endregion
        }


        public static PMSApprovalViewModel CreateApprovalViewModel(Position position, AppraisalPhaseWorkflow appraisalPhaseWorkflow, AppraisalTemplate template, AppraisalPhase appraisalPhase)
        {
            var result = new PMSApprovalViewModel();
            result.WorkflowId = appraisalPhaseWorkflow.WorkflowItem.Id;
            result.EmployeeId = appraisalPhaseWorkflow.Employee.Id;
            result.EmployeeName = appraisalPhaseWorkflow.Employee.TripleName;
            result.PhaseId = appraisalPhase.Id;
            result.TotalMark = appraisalPhaseWorkflow.FinalMark;
            result.PhaseSettingId = appraisalPhase.AppraisalPhaseSetting.Id;
            result.PhaseSettingTypeName = typeof(AppraisalPhaseSetting).FullName;
            result.IsHiddenCompetanceSection = !template.Competency;
            result.IsHiddenJobDescriptionSection = !template.JobDescription;
            result.IsHiddenObjectiveSection = !template.Objective;
            var jobDescription = position.JobDescription;

            if (!result.IsHiddenCompetanceSection)
            {
                result.CompetenceSection = getCompetenceSectionViewModel(jobDescription, appraisalPhaseWorkflow, template.CompetencyWeight);
            }
            if (!result.IsHiddenJobDescriptionSection)
            {
                result.JobDescriptionSection = getJobDescriptionSectionViewModel(jobDescription, appraisalPhaseWorkflow, template.JobDescriptionWeight);
            }
            if (!result.IsHiddenObjectiveSection)
            {
                var objectives = ServiceFactory.ORMService.All<Objective>().Where(x => x.Owner == position);
                result.ObjectiveSection = getObjectiveSectionViewModel(objectives, appraisalPhaseWorkflow, template.ObjectiveWeight);
            }
            foreach (var customSection in template.TemplateSectionWeights)
            {
                result.CustomSections.Add(getCustomSectionViewModel(customSection.AppraisalSection, appraisalPhaseWorkflow, customSection.Weight));
            }
            return result;
        }

        private static CustomSectionApprovalViewModel getCustomSectionViewModel(AppraisalSection appraisalSection, AppraisalPhaseWorkflow appraisalPhaseWorkflow, float CustomWeight)
        {
            var _AppraisalItems = new List<ApprovalSectionItemViewModel>();
            var _steps=new List<StepViewModel>();
            foreach(var item in appraisalSection.Items){
                foreach(var appraisal in appraisalPhaseWorkflow.Appraisals){
                var AppraisalCustomItem=appraisal.OrganizationalSections.FirstOrDefault(x=>x.Section==appraisalSection).AppraisalCustomSectionItems.FirstOrDefault(y=>y.Item==item);
                _steps.Add(new StepViewModel()
                {
                    Rate = AppraisalCustomItem.Rate,
                    Weight = AppraisalCustomItem.Weight,
                    Manager = appraisal.Appraiser.Employee.FullName,
                    Description = AppraisalCustomItem.Description
                });
                }
                _AppraisalItems.Add(new ApprovalSectionItemViewModel()
                {
                    Id=item.Id,
                    Name=item.Name,
                    Steps=_steps
                });
                _steps=new List<StepViewModel>();
            }
            var totalMark = 0.0;
            foreach (var _appraisal in appraisalPhaseWorkflow.Appraisals)
            {
                totalMark += _appraisal.OrganizationalSections.FirstOrDefault(x => x.Section == appraisalSection).AppraisalCustomSectionItems.Sum(x => x.Weight * x.Rate / 100);
            }
            totalMark = totalMark / appraisalPhaseWorkflow.Appraisals.Count;
            var result = new CustomSectionApprovalViewModel()
            {
                Id=appraisalSection.Id,
                Name=appraisalSection.Name,
                Description=appraisalSection.Description,
                SectionWeight=CustomWeight,
                SectionValue=totalMark,
                AppraisalItems=_AppraisalItems
            };
            return result;
        }

        private static ObjectiveSectionApprovalViewModel getObjectiveSectionViewModel(IQueryable<Objective> objectives, AppraisalPhaseWorkflow appraisalPhaseWorkflow, float ObjectiveWeight)
        {
             var _AppraisalItems=new List<ApprovalObjectiveViewModel>();
            var _steps=new List<StepViewModel>();
            foreach(var objective in objectives){
                foreach(var appraisal in appraisalPhaseWorkflow.Appraisals){
                var AppraisalObjective=appraisal.ObjectiveSections.FirstOrDefault(x=>x.Objective==objective);
                _steps.Add(new StepViewModel(){
                    Rate=AppraisalObjective.Rate,
                    Weight=AppraisalObjective.Weight,
                    Manager = appraisal.Appraiser.Employee.FullName,
                    Description = AppraisalObjective.Description
                    });
                }
                _AppraisalItems.Add(new ApprovalObjectiveViewModel()
                {
                    Id = objective.Id,
                    Name = objective.Name,
                    Steps = _steps
                });
                _steps=new List<StepViewModel>();
            }
            var totalMark = 0.0;
            foreach (var _appraisal in appraisalPhaseWorkflow.Appraisals)
            {
                totalMark += _appraisal.ObjectiveSectionValue;
            }
            totalMark = totalMark / appraisalPhaseWorkflow.Appraisals.Count;
            var result =new ObjectiveSectionApprovalViewModel(){
                Name="",
                Description="",
                SectionWeight=ObjectiveWeight,
                SectionValue=totalMark,
                AppraisalItems=_AppraisalItems
            };
            return result;
        }

        private static JobDescriptionSectionApprovalViewModel getJobDescriptionSectionViewModel(JobDescription jobDescription, AppraisalPhaseWorkflow appraisalPhaseWorkflow, float jobDescriptionWeight)
        {
             var _Roles=new List<ApprovalRoleViewModel>();
            var _AppraisalItems=new List<ApprovalResponsibilityViewModel>();
            var _steps=new List<StepViewModel>();
            foreach(var role in jobDescription.Roles){
                foreach(var responsibilty in role.Responsibilities){
                    foreach(var appraisal in appraisalPhaseWorkflow.Appraisals){
                        var AppraisalJobDescription=appraisal.JobDescriptionSections.FirstOrDefault(x=>x.Responsibility==responsibilty);
                        _steps.Add(new StepViewModel(){
                           Rate=AppraisalJobDescription.Rate,
                           Weight=AppraisalJobDescription.Weight,
                           Manager = appraisal.Appraiser.Employee.FullName,
                           Description = AppraisalJobDescription.Description
                        });
                    }
                    _AppraisalItems.Add(new ApprovalResponsibilityViewModel(){
                    Id=responsibilty.Id,
                    Name=responsibilty.Description,
                    Steps=_steps
                    });
                    _steps=new List<StepViewModel>();
                }
                _Roles.Add(new ApprovalRoleViewModel(){
                    Id=role.Id,
                    Name=role.Name,
                    Weight=role.Weight,
                    Description=role.Summary,
                    AppraisalItems=_AppraisalItems
                });
                _AppraisalItems=new List<ApprovalResponsibilityViewModel>();
            }
            var totalMark = 0.0;
            foreach (var _appraisal in appraisalPhaseWorkflow.Appraisals)
            {
                totalMark += _appraisal.JobDescriptionSectionValue;
            }
            totalMark = totalMark / appraisalPhaseWorkflow.Appraisals.Count;
            var result = new JobDescriptionSectionApprovalViewModel()
            {
                Name = jobDescription.Name,
                Description = jobDescription.Summary,
                JobTitle = jobDescription.JobTitle.Name,
                SectionWeight = jobDescriptionWeight,
                SectionValue=totalMark,
                Roles = _Roles
            };
            return result;
        }

        private static CompetenceSectionApprovalViewModel getCompetenceSectionViewModel(JobDescription jobDescription,AppraisalPhaseWorkflow appraisalPhaseWorkflow, float CompetencyWeight)
        {
            var _AppraisalItems=new List<ApprovalCompetenceViewModel>();
            var _steps=new List<StepViewModel>();
            foreach(var competency in jobDescription.Competencies){
                foreach(var appraisal in appraisalPhaseWorkflow.Appraisals){
                    var AppraisalCompetence=appraisal.AppraisalCompetences.FirstOrDefault(x=>x.Competence==competency);
                    _steps.Add(new StepViewModel(){
                        Rate=AppraisalCompetence.Rate,
                        Weight=AppraisalCompetence.Weight,
                        Manager = appraisal.Appraiser.Employee.FullName,
                        Description = AppraisalCompetence.Description
                    });
                }
                _AppraisalItems.Add(new ApprovalCompetenceViewModel(){
                    Id=competency.Id,
                    Name = competency.CompetenceCategory.NameForDropdown,
                    Type = competency.Type != null ? competency.Type.Name : "",
                    Steps=_steps
                });
                _steps=new List<StepViewModel>();
            }
            var totalMark = 0.0;
            foreach(var _appraisal in appraisalPhaseWorkflow.Appraisals){
                totalMark += _appraisal.CompetenceSectionValue;
            }
            totalMark = totalMark / appraisalPhaseWorkflow.Appraisals.Count;
            var result =new CompetenceSectionApprovalViewModel(){
                Name=jobDescription.Name,
                Description=jobDescription.Summary,
                JobTitle=jobDescription.JobTitle.Name,
                SectionWeight=CompetencyWeight,
                SectionValue=totalMark,
                AppraisalItems=_AppraisalItems
            };
            return result;
        }
    }
}