using DotNetOpenAuth.Messaging;
using Project.Web.Mvc4.Areas.Appraisal;
using  Project.Web.Mvc4.Helpers.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Web.Mvc4.Areas.Recruitment.Models.ApplicantsEvaluation;

namespace Project.Web.Mvc4.Areas.Appraisals
{
    public class DevelopmentSectionItemsViewModel
    {
        public DevelopmentSectionItemsViewModel()
        {
            Kpis = new List<KpiViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public float Weight { get; set; }
        public float Rate { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public bool IsChecked { get; set; }

        public List<KpiViewModel> Kpis { get; set; }
    }

    public class DevelopmentViewModel
    {
        public DevelopmentViewModel()
        {
            WeakPoints = new List<DevelopmentSectionItemsViewModel>();
            StrongPoints = new List<DevelopmentSectionItemsViewModel>();
        }

        public string SectionName { get; set; }

        public IList<DevelopmentSectionItemsViewModel> WeakPoints { get; set; }
        public IList<DevelopmentSectionItemsViewModel> StrongPoints { get; set; }

        public static List<DevelopmentViewModel> CreateInstance(AppraisalViewModel appraisalViewModel)
        {
            var results = new List<DevelopmentViewModel>();

            if (!appraisalViewModel.IsHiddenCompetanceSection)
            {
                var result = new DevelopmentViewModel();
                result.SectionName = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.CompetanceSection);

                result.WeakPoints =
                    appraisalViewModel.CompetenceSection.AppraisalItems.Select(
                        x => new DevelopmentSectionItemsViewModel()
                                 {
                                     Id = x.Id,
                                     Name = x.Name,
                                     Weight = x.Weight,
                                     Rate = x.Rate,
                                     Note = x.Note == null ? string.Empty : x.Note,
                                     IsChecked=false
                                 }).Where(x => x.Rate <= appraisalViewModel.WeaknessLimit).ToList();

                result.StrongPoints =
                    appraisalViewModel.CompetenceSection.AppraisalItems.Select(
                        x => new DevelopmentSectionItemsViewModel()
                                 {
                                     Id = x.Id,
                                     Name = x.Name,
                                     Weight = x.Weight,
                                     Rate = x.Rate,
                                     Note = x.Note == null ? string.Empty : x.Note,
                                     IsChecked = false
                                 }).Where(x => x.Rate >= appraisalViewModel.StrongLimit).ToList();
            results.Add(result);
            }

            if (!appraisalViewModel.IsHiddenJobDescriptionSection)
            {
                var result = new DevelopmentViewModel();
                result.SectionName = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.JobDescriptionSection);

                foreach (var role in appraisalViewModel.JobDescriptionSection.Roles)
                {
                    //result.WeakPoints = role.AppraisalItems.Select(
                    //    x => new DevelopmentSectionItemsViewModel()
                    //    {
                    //        Id = x.Id,
                    //        Name = x.Name,
                    //        Weight = x.Weight,
                    //        Rate = x.Rate,
                    //        Description = x.Note == null ? string.Empty : x.Note,
                    //        Note = x.Note == null ? string.Empty : x.Note,
                    //        IsChecked = false
                    //    }).Where(x => x.Rate <= appraisalViewModel.WeaknessLimit).ToList();

                    //result.StrongPoints = role.AppraisalItems.Select(
                    //    x => new DevelopmentSectionItemsViewModel()
                    //    {
                    //        Id = x.Id,
                    //        Name = x.Name,
                    //        Weight = x.Weight,
                    //        Rate = x.Rate,
                    //        Description = x.Note == null ? string.Empty : x.Note,
                    //        Note = x.Note == null ? string.Empty : x.Note,
                    //        IsChecked = false
                    //    }).Where(x => x.Rate >= appraisalViewModel.StrongLimit).ToList();  

                    result.WeakPoints.AddRange(role.AppraisalItems.Select(
                        x => new DevelopmentSectionItemsViewModel()
                             {
                                 Id = x.Id,
                                 Name = x.Name,
                                 Weight = x.Weight,
                                 Rate = x.Rate,
                                 Note = x.Note == null ? string.Empty : x.Note,
                                 IsChecked = false
                             }).Where(x => x.Rate <= appraisalViewModel.WeaknessLimit).ToList());

                    result.StrongPoints.AddRange(role.AppraisalItems.Select(
                        x => new DevelopmentSectionItemsViewModel()
                             {
                                 Id = x.Id,
                                 Name = x.Name,
                                 Weight = x.Weight,
                                 Rate = x.Rate,
                                 Note = x.Note == null ? string.Empty : x.Note,
                                 IsChecked = false
                             }).Where(x => x.Rate >= appraisalViewModel.StrongLimit).ToList());
                }
                results.Add(result);
            }

            if (!appraisalViewModel.IsHiddenObjectiveSection)
            {
                var result = new DevelopmentViewModel();
                result.SectionName = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.ObjectiveSection); 

                result.WeakPoints =
                    appraisalViewModel.ObjectiveSection.AppraisalItems.Select(
                        x => new DevelopmentSectionItemsViewModel()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Weight = x.Weight,
                            Rate = x.Rate,
                            Note = x.Note == null ? string.Empty : x.Note,
                            IsChecked = false
                        }).Where(x => x.Rate <= appraisalViewModel.WeaknessLimit).ToList();

                result.StrongPoints =
                    appraisalViewModel.ObjectiveSection.AppraisalItems.Select(
                        x => new DevelopmentSectionItemsViewModel()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Weight = x.Weight,
                            Rate = x.Rate,
                            Note = x.Note == null ? string.Empty : x.Note,
                            IsChecked = false
                        }).Where(x => x.Rate >= appraisalViewModel.StrongLimit).ToList();
                results.Add(result);
            }

            foreach (var customSection in appraisalViewModel.CustomSections)
            {
                var result = new DevelopmentViewModel();
                result.SectionName = customSection.Name;

                result.WeakPoints = customSection.AppraisalItems.Select(
                        x => new DevelopmentSectionItemsViewModel()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Weight = x.Weight,
                            Rate = x.Rate,
                            Note = x.Note == null ? string.Empty : x.Note,
                            IsChecked = false
                        }).Where(x => x.Rate <= appraisalViewModel.WeaknessLimit).ToList();

                result.StrongPoints = customSection.AppraisalItems.Select(
                        x => new DevelopmentSectionItemsViewModel()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Weight = x.Weight,
                            Rate = x.Rate,
                            Note = x.Note == null ? string.Empty : x.Note,
                            IsChecked = false
                        }).Where(x => x.Rate >= appraisalViewModel.StrongLimit).ToList();

                results.Add(result);
            }
            return results;
        }

        public static List<DevelopmentViewModel> CreateInstance(EvaluationViewModel evaluationViewModel)
        {
            var results = new List<DevelopmentViewModel>();
            foreach (var customSection in evaluationViewModel.CustomSections)
            {
                var result = new DevelopmentViewModel();
                result.SectionName = customSection.Name;

                result.WeakPoints = customSection.AppraisalItems.Select(
                    x => new DevelopmentSectionItemsViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Weight = x.Weight,
                        Rate = x.Rate,
                        Note = x.Note == null ? string.Empty : x.Note,
                        IsChecked = false
                    }).Where(x => x.Rate <= evaluationViewModel.WeaknessLimit).ToList();

                result.StrongPoints = customSection.AppraisalItems.Select(
                    x => new DevelopmentSectionItemsViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Weight = x.Weight,
                        Rate = x.Rate,
                        Note = x.Note == null ? string.Empty : x.Note,
                        IsChecked = false
                    }).Where(x => x.Rate >= evaluationViewModel.StrongLimit).ToList();

                results.Add(result);
            }
            return results;
        }
    }
}
