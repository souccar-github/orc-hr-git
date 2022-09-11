using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.Training.RootEntities;
using Souccar.Domain.DomainModel;

namespace Project.Web.Mvc4.Areas.Training.Models
{
    public class TrainingPlanViewModel: ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(TrainingPlanViewModel).FullName;
        }

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var trainingPlan = entity as TrainingPlan;

            if (trainingPlan != null)
                trainingPlan.CreationDate = DateTime.Now;
        }
    }
}