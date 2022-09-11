using HRIS.Domain.Global.Constant;
using HRIS.Domain.Workflow;
using  Project.Web.Mvc4.Helpers.Resource;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.Workflow.Models
{
    public class WorkflowSettingViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            if (type == typeof(WorkflowSetting))
            {
                requestInformation.NavigationInfo.Next.Clear();
                model.ViewModelTypeFullName = typeof(WorkflowSettingViewModel).FullName;
               
            }
        }

        public override void AfterRead(RequestInformation requestInformation, DataSourceResult result, int pageSize = 10, int skip = 0)
        {
            //switch (requestInformation.NavigationInfo.Module.Name)
            //{
            //    case ModulesNames.Incentive:
            //        var temp = (IQueryable<WorkflowSetting>)data.Data;
            //        var temp1 = temp.ToList();
            //        data.Data = temp1.Where(x => x.Type == WorkflowSettingType.Incentive).AsQueryable();
            //        data.Total = ((IQueryable<WorkflowSetting>)data.Data).Count();
            //        break;
            //    case ModulesNames.PMS:
            //        temp = (IQueryable<WorkflowSetting>)data.Data;
            //        temp1 = temp.ToList();
            //        data.Data = temp1.Where(x => x.Type == WorkflowSettingType.PerformanceAppraisal).AsQueryable();
            //        data.Total = ((IQueryable<WorkflowSetting>)data.Data).Count();
            //        break;

            //    case ModulesNames.Objective:
            //        temp = (IQueryable<WorkflowSetting>)data.Data;
            //        temp1 = temp.ToList();
            //        data.Data = temp1.Where(x => x.Type == WorkflowSettingType.Objective).AsQueryable();
            //        data.Total = ((IQueryable<WorkflowSetting>)data.Data).Count();
            //        break;
            //    case ModulesNames.EmployeeRelationServices:
            //        temp = (IQueryable<WorkflowSetting>)data.Data;
            //        temp1 = temp.ToList();
            //        data.Data = temp1.Where(x => x.Type == WorkflowSettingType.EmployeeRelationService).AsQueryable();
            //        data.Total = ((IQueryable<WorkflowSetting>)data.Data).Count();
            //        break;
            //    default:
            //        break;
            //}
            
        }
        public override void BeforeInsert(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, string customInformation = null)
        {
            //var setting = (WorkflowSetting)entity;
            //switch (requestInformation.NavigationInfo.Module.Name)
            //{

            //    case ModulesNames.Incentive:
            //        setting.Type = WorkflowSettingType.Incentive;
            //        break;
            //    case ModulesNames.PMS:
            //        setting.Type = WorkflowSettingType.PerformanceAppraisal;
            //        break;

            //    case ModulesNames.Objective:
            //        setting.Type = WorkflowSettingType.Objective;
            //        break;
            //    case ModulesNames.EmployeeRelationServices:
            //        setting.Type = WorkflowSettingType.EmployeeRelationService;
            //        break;
            //}
        }
    }
}