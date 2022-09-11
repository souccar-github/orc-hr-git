using Project.Web.Mvc4.Areas.Appraisal;
using Project.Web.Mvc4.Areas.Appraisals;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Helpers
{
    public class ApprovalViewModelFactory
    {
        
        public static ApprovalViewModel Create(HRIS.Domain.PMS.Entities.AppraisalPhaseWorkflow phaseWorkflow)
        {
            var result = new ApprovalViewModel();
            if (phaseWorkflow.Appraisals.Count != 0)
            {
                result.WorkflowId = phaseWorkflow.WorkflowItem.Id;
                var appraisalCount = phaseWorkflow.Appraisals.Count;
                var jobDescriptionValue = phaseWorkflow.Appraisals.Sum(x => x.JobDescriptionSectionWeight) / appraisalCount;
                var competenceValue = phaseWorkflow.Appraisals.Sum(x => x.CompetenceSectionWeight) / appraisalCount;
                var objectiveValue = phaseWorkflow.Appraisals.Sum(x => x.ObjectiveSectionWeight) / appraisalCount;
                result.AppraisalMarks.Add(new AppraisalMark()
                {
                    SectionName = "Job Description",
                    SectionValue = jobDescriptionValue
                });
                result.AppraisalMarks.Add(new AppraisalMark()
                {
                    SectionName = "Competence",
                    SectionValue = competenceValue
                });
                result.AppraisalMarks.Add(new AppraisalMark()
                {
                    SectionName = "Objective",
                    SectionValue = objectiveValue
                });
                var temp = new List<AppraisalMark>();

                var appraisalPhaseAppraisal = phaseWorkflow.Appraisals.FirstOrDefault();
                if (appraisalPhaseAppraisal != null)
                {
                    result.AppraisalMarks[0].SectionWeight = appraisalPhaseAppraisal.JobDescriptionSectionWeight;
                    result.AppraisalMarks[1].SectionWeight = appraisalPhaseAppraisal.CompetenceSectionWeight;
                    result.AppraisalMarks[2].SectionWeight = appraisalPhaseAppraisal.ObjectiveSectionWeight;
                    foreach (var customSection in appraisalPhaseAppraisal.OrganizationalSections)
                    {
                        temp.Add(new AppraisalMark() { SectionName = customSection.Section.Name, SectionWeight = customSection.Weight });
                    }
                    foreach (var phaseAppraisal in phaseWorkflow.Appraisals)
                    {
                        for (int i = 0; i < phaseAppraisal.OrganizationalSections.Count; i++)
                        {
                            phaseAppraisal.OrganizationalSections[i].Rate = phaseAppraisal.OrganizationalSections[i].AppraisalCustomSectionItems.Average(x => x.Rate);
                            temp[i].SectionValue += phaseAppraisal.OrganizationalSections[i].Rate;
                        }

                    }
                    foreach (var x in temp)
                    {
                        x.SectionValue /= appraisalCount;
                    }
                }
                result.AppraisalMarks.AddRange(temp);

                result.Total = result.AppraisalMarks.Sum(x => x.SectionValue * x.SectionWeight / 100);
            }
            return result;
        }

    }
}