using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using System;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Training.RootEntities;
using HRIS.Domain.Training.Enums;

namespace Project.Web.Mvc4.Areas.Training.Models
{
    public class TrainingNeedViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(TrainingNeedViewModel).FullName;
        }

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var trainingNeed = entity as TrainingNeed;
            
            if (trainingNeed != null)
            {
                trainingNeed.CreationDate = DateTime.Now;
                trainingNeed.Source = TrainingNeedSource.ManualEntry;
                trainingNeed.Status = TrainingNeedStatus.Initial;
            }
        }

        
    }
}