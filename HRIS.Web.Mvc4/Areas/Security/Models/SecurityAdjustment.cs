using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Web.Mvc4.Models.GridModel;
using Project.Web.Mvc4.Models.Navigation;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Factories;
using HRIS.Domain.Security.Configuration;
using HRIS.Domain.Global.Constant;

namespace Project.Web.Mvc4.Areas.Security.Models
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class SecurityAdjustment: ModelAdjustment
    {
        private static Dictionary<string, ViewModel> parent = new Dictionary<string, ViewModel>();
        public override void AdjustModule(Module module)
        {
            var temp = module.Configurations.ToList();
            temp.AddRange(ConfigurationFactory.Create(typeof(FullAuthorityRole).Assembly, ModulesNames.Security).ToList());
            module.Configurations = temp;
            module.Services.Add(new Service()
            {
                Controller = "Security/Service",
                Action = "ManageRole",
                Title = GlobalResource.RoleManagement,
                ServiceId = "ManageRole",
                SecurityId = "ManageRole"
            });
            module.Aggregates.SingleOrDefault(x => x.AggregateId == "User").Details.Clear();


            //module.Services.Add(new Service()
            //{
            //    Controller = "Security/Service",
            //    Action = "ManageFieldSecurity",
            //    Title = SecurityLocalizationHelper.GetResource(SecurityLocalizationHelper.ManageFieldSecurity),
            //    ServiceId = "ManageFieldSecurity",
            //    SecurityId = "ManageFieldSecurity"
            //});
            module.Aggregates.SingleOrDefault(x => x.AggregateId == "User").Details.Clear();

        }




        public override ViewModel AdjustGridModel(string type)
        {
            if (parent.Count == 0)
            {
                parent.Add("User", new UserViewModel());

                parent.Add("Role", new RoleViewModel());


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