using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.ProjectManagement.Entities;
using HRIS.Domain.ProjectManagement.RootEntities;
using  Project.Web.Mvc4.Areas.ProjectManagement.Models;
using  Project.Web.Mvc4.Factories;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Models.GridModel;
using  Project.Web.Mvc4.Models.Navigation;
using  Project.Web.Mvc4.Helpers.Resource;
using HRIS.Domain.Global.Constant;
using  Project.Web.Mvc4.Models;

namespace Project.Web.Mvc4.Areas.ProjectManagement.Models
{
    /// <summary>
    /// Author: Amer Farhat
    /// </summary>
    public class ProjectManagementAdjustment: ModelAdjustment
    {
        private static Dictionary<string, ViewModel> parent = new Dictionary<string, ViewModel>();
        public override void AdjustModule(Module module)
        {

            module.Services.Add(new Service()
            {
                Controller = "ProjectManagement/Service",
                Action = "Evaluation",
                Title = ProjectManagementLocalizationHelper.GetResource(ProjectManagementLocalizationHelper.Evaluation),
                ServiceId = "Evaluation",
                SecurityId = "Evaluation"
            });

            var project = module.Aggregates.SingleOrDefault(x => x.TypeFullName == typeof(HRIS.Domain.ProjectManagement.RootEntities.Project).FullName);
            var teamAndRole = project.Details.SingleOrDefault(x => x.TypeFullName == typeof(Team).FullName);
            teamAndRole.Details = DetailFactory.Create(typeof(Team));


            var teamRole = teamAndRole.Details.SingleOrDefault(x => x.TypeFullName == typeof(TRole).FullName);
            teamRole.Details = DetailFactory.Create(typeof(TRole));
            var managerInfo =
                teamRole.Details.SingleOrDefault(x => x.TypeFullName == typeof(IndirectManagerInfo).FullName);
            managerInfo.Details = DetailFactory.Create(typeof(IndirectManagerInfo));

            var phase = project.Details.SingleOrDefault(x => x.TypeFullName == typeof(Phase).FullName);
            phase.Details = DetailFactory.Create(typeof(Phase));
            var task = phase.Details.SingleOrDefault(x => x.TypeFullName == typeof(Task).FullName);
            task.Details = DetailFactory.Create(typeof(Task));

            //var resource = project.Details.SingleOrDefault(x => x.TypeFullName == typeof (Resource).FullName);
            //resource.Details = DetailFactory.Create(typeof (Resource));
        }

        public override ViewModel AdjustGridModel(string type)
        {
            if (parent.Count == 0)
            {
                parent.Add("ProjectManagement", new ProjectViewModel());
                parent.Add("Task", new TaskViewModel());
                parent.Add("Resource", new ResourceViewModel());
                parent.Add("Phase", new PhaseViewModel());
             


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
    }


}