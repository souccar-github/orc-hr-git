using HRIS.Domain.Global.Constant;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.Training.Entities;
using HRIS.Domain.Training.RootEntities;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Helpers.Resource;

namespace Project.Web.Mvc4.Areas.Training.Models
{
    public class TrainingAdjustment: ModelAdjustment
    {
        private static Dictionary<string, ViewModel> parent = new Dictionary<string, ViewModel>();
        public override ViewModel AdjustGridModel(string type)
        {
            var assembly = typeof(ViewModel).Assembly;
            var viewModelType = assembly.GetType($"Project.Web.Mvc4.Areas.Training.Models.{type}ViewModel");

            return (ViewModel)Activator.CreateInstance(viewModelType);
        }

        public override void AdjustModule(Module module)
        {
            if (module.ModuleId.Equals(ModulesNames.Training))
            {
                module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(TrainingPlan).FullName))
                .Details.SingleOrDefault(x => x.TypeFullName == (typeof(Course).FullName))
                .Details = DetailFactory.Create(typeof(Course));

                module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(TrainingPlan).FullName))
                    .Details.SingleOrDefault(x => x.TypeFullName == (typeof(Course).FullName))
                    .Details.SingleOrDefault(x => x.TypeFullName == (typeof(AppraisalTrainee).FullName))
                    .Details = DetailFactory.Create(typeof(AppraisalTrainee));

                var course = module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(TrainingPlan).FullName))
                    .Details.SingleOrDefault(x => x.TypeFullName == (typeof(Course).FullName));

                if (course != null)
                {
                    var courseEmployee =
                        course.Details.FirstOrDefault(x => x.TypeFullName == (typeof(CourseEmployee).FullName));

                    var courseTrainingNeed =
                        course.Details.FirstOrDefault(x => x.TypeFullName == (typeof(CourseTrainingNeed).FullName));

                    course.Details.Remove(courseEmployee);
                    course.Details.Remove(courseTrainingNeed);
                }
            }

            module.Dashboards.Add(new Dashboard()
            {
                Title = TrainingLocalizationHelper.GetResource(TrainingLocalizationHelper.TrainingDashboard),
                Controller = "Training/Dashboard",
                Action = "TrainingDashboard",
                DashboardId = "TrainingDashboard",
                SecurityId = "TrainingDashboard"
            });
        }
    }
}