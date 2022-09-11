using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.OrgChart.Entities;
using HRIS.Domain.PMS.RootEntities;
using Infrastructure.Repositories;
using UI.Areas.PMSComprehensive.DTO.ViewModels;

namespace UI.Areas.PMSComprehensive.DTO.Adapters
{
    public class AppraisalPhaseAdapter
    {
        public static void UpdateAppraisalPhase(AppraisalPhaseViewModel appraisalPhaseViewModel,
                                                  AppraisalPhase appraisalPhase, IRepository<Grade> gradeRepository)
        {
            appraisalPhase.Id = appraisalPhaseViewModel.AppraisalPhase.Id;
            appraisalPhase.CloseDate = appraisalPhaseViewModel.AppraisalPhase.CloseDate;
            appraisalPhase.DirectManagerWeight = appraisalPhaseViewModel.AppraisalPhase.DirectManagerWeight;
            appraisalPhase.FullMarkThreshold = appraisalPhaseViewModel.AppraisalPhase.FullMarkThreshold;
            appraisalPhase.GapThreshold = appraisalPhaseViewModel.AppraisalPhase.GapThreshold;
            appraisalPhase.IndirectManagerWeight = appraisalPhaseViewModel.AppraisalPhase.IndirectManagerWeight;
            appraisalPhase.OpenDate = appraisalPhaseViewModel.AppraisalPhase.CloseDate;
            appraisalPhase.Period = appraisalPhaseViewModel.AppraisalPhase.Period;
            appraisalPhase.SelfAssessmentWeight = appraisalPhaseViewModel.AppraisalPhase.SelfAssessmentWeight;
            appraisalPhase.SkillThreshold = appraisalPhaseViewModel.AppraisalPhase.SkillThreshold;
            appraisalPhase.StartQuarter = appraisalPhaseViewModel.AppraisalPhase.StartQuarter;
            appraisalPhase.WithSecondLevelSuperior = appraisalPhaseViewModel.AppraisalPhase.WithSecondLevelSuperior;
            appraisalPhase.WithSelfAssessment = appraisalPhaseViewModel.AppraisalPhase.WithSelfAssessment;
            appraisalPhase.Year = appraisalPhaseViewModel.AppraisalPhase.Year;
            List<Grade> grades = gradeRepository.GetAll().ToList();
            foreach (GradeViewModel gradeViewModel in appraisalPhaseViewModel.Grades)
            {
                Grade tempAppraisalTemplateGrade = appraisalPhase.ExcludedGrades.SingleOrDefault(x => x.Id == gradeViewModel.Id);
                if (tempAppraisalTemplateGrade == null && gradeViewModel.Checked)
                {
                    appraisalPhase.ExcludedGrades.Add(grades.Single(x => x.Id == gradeViewModel.Id));
                }
                else if (tempAppraisalTemplateGrade != null && !gradeViewModel.Checked)
                {
                    appraisalPhase.ExcludedGrades.Remove(tempAppraisalTemplateGrade);
                }
            }
        }

        public static List<GradeViewModel> GetGrades(IRepository<Grade> gradeRepository, AppraisalPhase appraisalPhase = null)
        {
            var grades = gradeRepository.GetAll().Select(GradeViewModel.Create).ToList();
            if (appraisalPhase == null)
                return grades;
            foreach (var excludedGrade in appraisalPhase.ExcludedGrades)
            {
                grades.Single(x => x.Id == excludedGrade.Id).Checked = true;
            }
            return grades;
        }
    }
}