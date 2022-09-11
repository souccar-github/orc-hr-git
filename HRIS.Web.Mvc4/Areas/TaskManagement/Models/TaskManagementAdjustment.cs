using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using  Project.Web.Mvc4.Models.GridModel;
using  Project.Web.Mvc4.Models.Navigation;
using  Project.Web.Mvc4.Helpers.Resource;
using HRIS.Domain.TaskManagement.RootEntities;
using  Project.Web.Mvc4.Models;

namespace Project.Web.Mvc4.Areas.TaskManagement.Models
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class TaskManagementAdjustment: ModelAdjustment
    {
        private static Dictionary<string, ViewModel> parent = new Dictionary<string, ViewModel>();
        public override void AdjustModule(Module module)
        {
            
        }

        public override ViewModel AdjustGridModel(string type)
        {


            if (parent.Count == 0)
            {
                parent.Add("Task", new TaskViewModel());
                parent.Add("DailyWork", new DailyWorkViewModel());
       



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