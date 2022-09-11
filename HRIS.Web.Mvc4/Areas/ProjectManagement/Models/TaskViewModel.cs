using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using HRIS.Domain.ProjectManagement.Entities;
using HRIS.Domain.ProjectManagement.RootEntities;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.ProjectManagement.Models
{
    public class TaskViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(TaskViewModel).FullName;
        }

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            this.PreventDefault = true;

            var newTaskNumber = 1;
            var task = (Task)entity;
            var project = ServiceFactory.ORMService.GetById<HRIS.Domain.ProjectManagement.RootEntities.Project>(requestInformation.NavigationInfo.Previous[0].RowId);
            var phase = project.Phases.FirstOrDefault(x => x.Id == requestInformation.NavigationInfo.Previous[1].RowId);

            if (phase != null && phase.Tasks != null)
            {
                if (phase.Tasks != null && phase.Tasks.Count > 0)
                {
                    var maxTaskNumber = phase.Tasks.Max(x => x.Number);
                    newTaskNumber = maxTaskNumber + 1;
                }
                task.Number = newTaskNumber;
                phase.AddTask(task);
                ServiceFactory.ORMService.Save(project, UserExtensions.CurrentUser);
            }
        }

        public int TaskId { get; set; }
        public int TaskNumber { get; set; }
        public int Status { get; set; }
        public float TaskRate { get; set; }
        public float Weight { get; set; }
        
    }
}
