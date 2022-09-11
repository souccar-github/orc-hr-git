using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.Workflow;
using  Project.Web.Mvc4.Models.GridModel;
using  Project.Web.Mvc4.Models.Navigation;
using  Project.Web.Mvc4.Helpers.Resource;
using HRIS.Domain.TaskManagement.RootEntities;
using  Project.Web.Mvc4.Factories;
using Souccar.Domain.Workflow.RootEntities;
using HRIS.Domain.Global.Constant;
using  Project.Web.Mvc4.Models;

namespace Project.Web.Mvc4.Areas.Workflow.Models
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class WorkflowAdjustment: ModelAdjustment
    {
        private static Dictionary<string, ViewModel> parent = new Dictionary<string, ViewModel>();

        public override void AdjustModule(Module module)
        {
            var temp=module.Aggregates.ToList();
            temp.AddRange(AggregateFactory.Create(typeof(WorkflowItem).Assembly, ModulesNames.Workflow).ToList()); 
            module.Aggregates = temp;
        }

        public override ViewModel AdjustGridModel(string type)
        {

            if (parent.Count == 0)
            {
                parent.Add("WorkflowItem", new WorkflowItemsViewModel());
                parent.Add("WorkflowStep", new WorkflowStepViewModel());
                parent.Add("WorkflowApproval", new WorkflowApprovalViewModel());
                parent.Add("WorkflowSetting", new WorkflowSettingViewModel());



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