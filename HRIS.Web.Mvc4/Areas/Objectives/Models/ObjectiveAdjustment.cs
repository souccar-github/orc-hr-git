using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Objectives.RootEntities;
using HRIS.Domain.Objectives.Entities;
using  Project.Web.Mvc4.Factories;
using  Project.Web.Mvc4.Models.GridModel;
using  Project.Web.Mvc4.Models.Navigation;
using  Project.Web.Mvc4.Helpers.Resource;
using  Project.Web.Mvc4.Models;

namespace Project.Web.Mvc4.Areas.Objectives.Models
{
    public class ObjectiveAdjustment: ModelAdjustment
    {
        private static Dictionary<string, ViewModel> parent = new Dictionary<string, ViewModel>();
        public override ViewModel AdjustGridModel(string type)
        {
            if (parent.Count == 0)
            {
                parent.Add("Objective", new ObjectiveViewModel());
                parent.Add("ObjectiveAppraisalPhase", new ObjectiveAppraisalPhaseViewModel());
                parent.Add("ObjectiveCreationPhase", new ObjectiveCreationPhaseViewModel());
                parent.Add("StrategicObjective", new StrategicObjectiveViewModel());
                parent.Add("SharedWith", new SharedWithViewModel());
                parent.Add("ActionPlan", new ActionPlanViewModel());


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

        public override void AdjustModule(Module module)
        {
            var details = new List<string>()
                          {
                              "SharedWiths", "Kpis", "Constraints", "ActionPlans"};
            //module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(HRIS.Domain.Objectives.RootEntities.Objective).FullName))
            //    .Details = module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(HRIS.Domain.Objectives.RootEntities.Objective).FullName))
            //    .Details.Where(x => details.Contains(x.DetailId))
            //    .ToList();

            module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(HRIS.Domain.Objectives.RootEntities.StrategicObjective).FullName))
                .Details.SingleOrDefault(x=>x.TypeFullName==typeof(HRIS.Domain.Objectives.RootEntities.Objective).FullName)
                .Details = DetailFactory.Create(typeof(HRIS.Domain.Objectives.RootEntities.Objective))
                .Where(x => details.Contains(x.DetailId))
                .ToList();

            module.Services.Add(new Service()
            {
                Title = ObjectiveLocalizationHelper.GetResource(ObjectiveLocalizationHelper.ApprovalService),
                ServiceId = "ApprovalObjectiveService",
                SecurityId = "ApprovalObjectiveService",
                Controller = "Objectives/Home",
                Action = "ApprovalService"
            });

            module.Services.Add(new Service()
            {
                Title = ObjectiveLocalizationHelper.GetResource(ObjectiveLocalizationHelper.TrackingService),
                ServiceId = "TrackingObjectiveService",
                SecurityId = "TrackingObjectiveService",
                Controller = "Objectives/Home",
                Action = "TrackingService"
            });

            module.Services.Add(new Service()
            {
                Title = ObjectiveLocalizationHelper.GetResource(ObjectiveLocalizationHelper.AppraisalService),
                ServiceId = "AppraisalService",
                SecurityId = "AppraisalService",
                Controller = "Objectives/Home",
                Action = "AppraisalService"
            });

        }
    }
}