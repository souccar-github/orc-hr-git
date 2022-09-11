using HRIS.Domain.TaskManagement.RootEntities;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using  Project.Web.Mvc4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.TaskManagement.Models
{
    public class DailyWorkViewModel:ViewModel
    {
     public override void CustomizeGridModel(Mvc4.Models.GridModel.GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(DailyWorkViewModel).FullName;
        }
        public override void BeforeInsert(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, string customInformation = null)
        {
            base.BeforeInsert(requestInformation, entity, customInformation);
        }
        public override void BeforeValidation(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, IDictionary<string, object> originalState, CrudOperationType operationType, string customInformation = null)
        {
            if(operationType== CrudOperationType.Insert)
                (entity as DailyWork).Employee = EmployeeExtensions.CurrentEmployee;
        }
        public override void AfterRead(RequestInformation requestInformation, DataSourceResult result, int pageSize = 10, int skip = 0)
        {
            var temp = (IQueryable<DailyWork>)result.Data;
            var temp1 = temp.ToList();
            result.Data = temp1.Where(x => x.Employee == EmployeeExtensions.CurrentEmployee).AsQueryable();
            result.Total = ((IQueryable<DailyWork>)result.Data).Count();
        }
    }
}