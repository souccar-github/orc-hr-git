using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Domain.Recruitment.Enums;
using HRIS.Domain.Recruitment.RootEntities;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Project.Web.Mvc4.Helpers.Resource;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Domain.Audit;

namespace Project.Web.Mvc4.Areas.Recruitment.Models
{
    public class WrittenExaminationViewModel : ViewModel
    {
        
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(WrittenExaminationViewModel).FullName;
        }

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            this.PreventDefault = true;

            var writtenExamination = (WrittenExamination)entity;
            var advertisement = ServiceFactory.ORMService.GetById<Advertisement>(requestInformation.NavigationInfo.Previous[0].RowId);
            advertisement.Status = AdvertisementStatus.Underway;
            advertisement.AddWrittenExamination(writtenExamination);
          
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { advertisement }, UserExtensions.CurrentUser, writtenExamination, OperationType.Insert);

        }

    }
}