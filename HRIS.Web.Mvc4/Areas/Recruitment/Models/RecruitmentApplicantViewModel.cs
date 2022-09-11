using System;
using System.Linq;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Domain.Recruitment.RootEntities;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Domain.Audit;
using System.Collections.Generic;

namespace Project.Web.Mvc4.Areas.Recruitment.Models
{
    public class RecruitmentApplicantViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(RecruitmentApplicantViewModel).FullName;

            //model.ActionList.Commands.Add(new ActionListCommand() { GroupId = 1, Order = 1, HandlerName = "SetResult", 
            //    Name =RecruitmentLocalizationHelper.GetResource( RecruitmentLocalizationHelper.SetResult), ShowCommand = true });
            //model.ActionList.Commands.Add(new ActionListCommand() { GroupId = 1, Order = 2, HandlerName = "SetWrittenExaminationMark", 
            //    Name =RecruitmentLocalizationHelper.GetResource( RecruitmentLocalizationHelper.SetWrittenExaminationMark), ShowCommand = true });
            //model.ActionList.Commands.Add(new ActionListCommand() { GroupId = 1, Order = 3, HandlerName = "SetOralExaminationMark", 
            //    Name =RecruitmentLocalizationHelper.GetResource( RecruitmentLocalizationHelper.SetOralExaminationMark), ShowCommand = true});
        }

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            this.PreventDefault = true;

            var newApplicantNumber = 1;
            var recruitmentApplicant = (RecruitmentApplicant)entity;
            var advertisement = ServiceFactory.ORMService.GetById<Advertisement>(requestInformation.NavigationInfo.Previous[0].RowId);
            var recruitmentInformation = advertisement.RecruitmentInformations.FirstOrDefault(x => x.Id == requestInformation.NavigationInfo.Previous[1].RowId);

            if (recruitmentInformation != null && recruitmentInformation.Applicants != null)
            {
                if (recruitmentInformation.Applicants != null && recruitmentInformation.Applicants.Count > 0)
                {
                    var maxApplicantNumber = recruitmentInformation.Applicants.Max(x => x.ApplicantNumber);
                    newApplicantNumber = maxApplicantNumber + 1;
                }

                recruitmentApplicant.ApplicantNumber = newApplicantNumber;
                recruitmentInformation.AddApplicant(recruitmentApplicant);
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { advertisement }, UserExtensions.CurrentUser, recruitmentApplicant, OperationType.Insert);

            }
        }
        

    }
}