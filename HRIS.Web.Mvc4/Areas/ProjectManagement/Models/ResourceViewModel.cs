using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.ProjectManagement.Entities;
using HRIS.Domain.ProjectManagement.RootEntities;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Core.Fasterflect;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.ProjectManagement.Models
{
    public class ResourceViewModel : ViewModel
    {
        //
        // GET: /ProjectManagement/ResourceViewModel/
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(ResourceViewModel).FullName;
        }

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            this.PreventDefault = true;

            var newResourceNumber = 1;
            var resource = (Resource)entity;
            var project = ServiceFactory.ORMService.GetById<HRIS.Domain.ProjectManagement.RootEntities.Project>(requestInformation.NavigationInfo.Previous[0].RowId);

            if (project.Resources != null && project.Resources.Count > 0)
                {
                    var maxResourceNumber = project.Resources.Max(x => x.Number);
                    newResourceNumber = maxResourceNumber + 1;
                }
            resource.Number = newResourceNumber;
            project.AddResource(resource);
                ServiceFactory.ORMService.Save(project, UserExtensions.CurrentUser);
            
        }
    }
}
