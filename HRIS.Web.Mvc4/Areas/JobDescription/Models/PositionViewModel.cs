using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using FluentNHibernate.Utils;
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using Project.Web.Mvc4.Areas.Grades.Models;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Models.GridModel;
using Project.Web.Mvc4.Helpers.Resource;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.JobDescription.Entities;
using Project.Web.Mvc4.Areas.JobDescription.Helpers;
using Souccar.Core.Extensions;
using Souccar.Core.Fasterflect;
using Souccar.Core.Utilities;
using Souccar.Infrastructure.Core;
using HRIS.Domain.JobDescription.RootEntities;
using Souccar.Domain.DomainModel;

using Souccar.Domain.Validation;
using Project.Web.Mvc4.Helpers;
using HRIS.Domain.JobDescription.Configurations;
using HRIS.Domain.Workflow;
using Project.Web.Mvc4.Helpers.DomainExtensions;

namespace Project.Web.Mvc4.Areas.JobDescription.Models
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class PositionViewModel:ViewModel
    {

        public override void CustomizeGridModel(Mvc4.Models.GridModel.GridViewModel model, Type type, Mvc4.Models.RequestInformation requestInformation)
        {
            //requestInformation.NavigationInfo.Next.Clear();
            model.ViewModelTypeFullName = typeof(PositionViewModel).FullName;
            if (!requestInformation.NavigationInfo.Previous[0].TypeName.EndsWith(typeof(HRIS.Domain.JobDescription.RootEntities.JobDescription).Name))
                model.Views[0].EditHandler = "PositionEditHandler";
            else
            {
                model.ActionListHandler = "initializeJobDesActionList";
                model.ToolbarCommands.RemoveAt(0);
              
                model.Views[0].ViewHandler = "JobDesPositionViewHandler";
            }

            //var project = ServiceFactory.ORMService.All<MTNProject>().FirstOrDefault();
            //var mtnSteps = project.MtnSteps.Where(x => x.StepOrder == project.WorkflowOrder).FirstOrDefault();
            //if (mtnSteps != null)
            //{

            //    foreach (var step in mtnSteps.ProjectSteps)
            //    {
            //        var approvals = MtnWorkflowHelper.getNextApproval(step.WorkflowItem);
            //        if (approvals != null && (step.Entity == "position") && (step.WorkflowItem.Status != WorkflowStatus.Completed) && (step.WorkflowItem.Status != WorkflowStatus.Canceled))
            //        {
            //            foreach (var approval in approvals)
            //                if (approval.User == UserExtensions.CurrentUser)
            //                    model.ToolbarCommands.RemoveAt(0);
            //        }
            //    }
            //}
            
        }
        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var position = (Position)entity;
            if (ServiceFactory.ORMService.All<PositionCode>().Any())
            {
                var positionCode = ServiceFactory.ORMService.All<PositionCode>().First();
                position.Code = JobDescriptionHelper.GetCode(positionCode, position);
            }

            var JobTitleID = position.JobDescription.JobTitle.Id;
            var JobTitle = ServiceFactory.ORMService.GetById<JobTitle>(JobTitleID);
            JobTitle.Vacancies++;
            JobTitle.Save();

        }

        public override void AfterInsert(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, string customInformation = null)
        {
            var position = entity as Position;
            var entities = new List<IAggregateRoot>();
            var codeSetting=ServiceFactory.ORMService.All<PositionCode>().FirstOrDefault();
            position.Code = JobDescriptionHelper.GetCode(codeSetting, position);
            var workflowSettings = ServiceFactory.ORMService.All<WorkflowSetting>();
            entities.Add(position);
            foreach (var setting in workflowSettings)
            {
                var overWrite = setting.SettingOverwritings.FirstOrDefault(x => x.JobDescription != null && x.JobDescription.Id == position.JobDescription.Id);
                if (overWrite != null) setting.AddPosition(new WorkflowSettingPosition() { Position = position, Count = overWrite.Count });
                else
                {
                    overWrite = setting.SettingOverwritings.FirstOrDefault(x => x.JobTitle != null && x.JobTitle.Id == position.JobDescription?.JobTitle?.Id);
                    if (overWrite != null) setting.AddPosition(new WorkflowSettingPosition() { Position = position, Count = overWrite.Count });
                    else
                    {
                        overWrite = setting.SettingOverwritings.FirstOrDefault(x => x.Grade != null && x.Grade.Id == position.JobDescription?.JobTitle?.Grade?.Id);
                        if (overWrite != null) setting.AddPosition(new WorkflowSettingPosition() { Position = position, Count = overWrite.Count });
                        else
                        {
                            overWrite = setting.SettingOverwritings.FirstOrDefault(x => x.OrganizationalLevel != null && x.OrganizationalLevel.Id == position.JobDescription?.JobTitle?.Grade?.OrganizationalLevel?.Id);
                            if (overWrite != null) setting.AddPosition(new WorkflowSettingPosition() { Position = position, Count = overWrite.Count });
                        }
                    }
                }
                entities.Add(setting);
            }
            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
        }

        public override void BeforeUpdate(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, IDictionary<string, object> originalState, string customInformation = null)
        {
            var position = entity as Position;

            var jobDescriptionString = originalState["NameForDropdown"].ToString();
            var str = jobDescriptionString.Substring(0, jobDescriptionString.IndexOf("="));
             
            HRIS.Domain.JobDescription.RootEntities.JobDescription oldJD = null;
            if (str!="")
            {
                oldJD = ServiceFactory.ORMService.All<HRIS.Domain.JobDescription.RootEntities.JobDescription>().FirstOrDefault(x => x.Name==str);
            }

            if (oldJD != null)
            {
                if (position.JobDescription.JobTitle != oldJD.JobTitle)
                {
                    oldJD.JobTitle.Vacancies--;
                    position.JobDescription.JobTitle.Vacancies++;
                }
            }

        }


        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var position = (Position)entity;


            if (operationType == CrudOperationType.Insert)
            {
                var positionCount = ServiceFactory.ORMService.All<Position>().Count(x => x.JobDescription.JobTitle == position.JobDescription.JobTitle);
                if (position.JobDescription.JobTitle.EmployeeCount <= positionCount)
                {
                    var prop = typeof (Position).GetProperty("JobDescription");
                    validationResults.Add(new ValidationResult()
                                          {
                                              Message =
                                                  JobDescriptionLocalizationHelper.GetResource(
                                                      JobDescriptionLocalizationHelper
                                                  .PositionCountMustBeLessThanOrEqualEmployeeCount),
                                              Property = prop
                                          });
                }
            }
            else if(operationType == CrudOperationType.Update)
            {
                var positionCount = ServiceFactory.ORMService.All<Position>().Where(x => x != position).Count(x => x.JobDescription.JobTitle == position.JobDescription.JobTitle);
                if (position.JobDescription.JobTitle.EmployeeCount <= positionCount)
                {
                    var prop = typeof(Position).GetProperty("JobDescription");
                    validationResults.Add(new ValidationResult()
                    {
                        Message =
                            JobDescriptionLocalizationHelper.GetResource(
                                JobDescriptionLocalizationHelper
                            .PositionCountMustBeLessThanOrEqualEmployeeCount),
                        Property = prop
                    });
                }    
            }


            var codeSettings = ServiceFactory.ORMService.All<PositionCode>();
            if (!codeSettings.Any())
            {
                var prop = typeof(Position).GetProperty("Step");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", prop.GetTitle(), 
                    JobDescriptionLocalizationHelper.GetResource(JobDescriptionLocalizationHelper.YouMustAddPositionCodeSetting)),
                    Property = prop
                });
            }


            //Position oldPosition = ServiceFactory.ORMService.All<Position>().FirstOrDefault(x => x.Code == position.Code);

            //if (oldPosition != null && oldPosition.Id != position.Id)
            //{
            //    var prop = typeof(Position).GetProperty("Code");
            //    validationResults.Add(
            //    new ValidationResult()
            //    {
            //        Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.AlreadyexistMessage),
            //        Property = prop
            //    });
            //}
        }


        public override void AfterUpdate(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, IDictionary<string, object> originalState, string customInformation = null)
        {
            var position = entity as Position;
            var codeSetting = ServiceFactory.ORMService.All<PositionCode>().FirstOrDefault();
            position.Code = JobDescriptionHelper.GetCode(codeSetting, position);

            position.Save();
        }
        public override void AfterDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var position = entity as Position;

            var JobTitle = position.JobDescription.JobTitle;
            JobTitle.Vacancies--;
            JobTitle.Save();
            position.Save();
        }

    }

}