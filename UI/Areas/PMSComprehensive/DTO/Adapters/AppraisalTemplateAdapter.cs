using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.OrgChart.Entities;
using HRIS.Domain.PMS.RootEntities;
using Infrastructure.Repositories;
using UI.Areas.PMSComprehensive.DTO.ViewModels;

namespace UI.Areas.PMSComprehensive.DTO.Adapters
{
    public static class AppraisalTemplateAdapter
    {
        public static void UpdateAppraisalTemplate(AppraisalTemplateViewModel appraisalTemplateModel,
                                                   AppraisalTemplate destination, IRepository<Grade> gradeRepository)
        {
            destination.Id = appraisalTemplateModel.AppraisalTemplate.Id;
            destination.Name = appraisalTemplateModel.AppraisalTemplate.Name;
            destination.Type = appraisalTemplateModel.AppraisalTemplate.Type;
            List<Grade> grades = gradeRepository.GetAll().ToList();
            foreach (GradeViewModel gradeViewModel in appraisalTemplateModel.Grades)
            {
                Grade tempAppraisalTemplateGrade = destination.Grades.SingleOrDefault(x => x.Id == gradeViewModel.Id);
                if (tempAppraisalTemplateGrade == null && gradeViewModel.Checked)
                {
                    destination.AddGrade(grades.Single(x => x.Id == gradeViewModel.Id));
                }
                else if (tempAppraisalTemplateGrade != null && !gradeViewModel.Checked)
                {
                    destination.Grades.Remove(tempAppraisalTemplateGrade);
                }
            }
            foreach (SectionWeight sectionWeight in appraisalTemplateModel.SectionWeights)
            {
                if (destination.SectionWeights.ContainsKey(sectionWeight.Name))
                {
                    destination.SectionWeights[sectionWeight.Name] = sectionWeight.Weight;
                }
                else
                {
                    destination.SectionWeights.Add(sectionWeight.Name, sectionWeight.Weight);
                }
            }
        }

        public static List<GradeViewModel> GetAvailableGrades(
            IRepository<AppraisalTemplate> appraisalTemplateRepository, IRepository<Grade> gradeRepository,
            AppraisalTemplate appraisalTemplate)
        {
            if (appraisalTemplate != null)
            {
                var result = gradeRepository.GetAll().Where(
                     grade =>
                     !appraisalTemplateRepository.GetAll().Any(x => x.Grades.Contains(grade) && x.Id != appraisalTemplate.Id))
                     .Select(GradeViewModel.Create).ToList();
                foreach (var gradeViewModel in result)
                {
                    if (appraisalTemplate.Grades.SingleOrDefault(g => g.Id == gradeViewModel.Id) != null)
                        gradeViewModel.Checked = true;
                }
                return result;
            }
            return gradeRepository.GetAll().Where(
                grade =>
                !appraisalTemplateRepository.GetAll().Any(x => x.Grades.Contains(grade)))
                .Select(GradeViewModel.Create).ToList();
        }

        public static IList<Grade> GetDuplicatedAppraisalTemplateGrade(AppraisalTemplate appraisalTemplate,
                                                                       IRepository<AppraisalTemplate>
                                                                           appraisalTemplateRepository,
                                                                       IRepository<Grade> gradeRepository)
        {
            return gradeRepository.GetAll().Where(
                grade =>
                appraisalTemplate.Grades.Contains(grade) &&
                appraisalTemplateRepository.GetAll().Any(
                    template => template.Grades.Contains(grade) && template.Id != appraisalTemplate.Id)).ToList();
        }
    }
}