using HRIS.Domain.TaskManagement.RootEntities;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using  Project.Web.Mvc4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.TaskManagement.Models
{
    public class TaskViewModel:ViewModel
    {
public override void CustomizeGridModel(Mvc4.Models.GridModel.GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(TaskViewModel).FullName;
        }
       
        public override void BeforeValidation(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, IDictionary<string, object> originalState, CrudOperationType operationType, string customInformation = null)
        {
            if (operationType == CrudOperationType.Insert)
                (entity as Task).Employee = EmployeeExtensions.CurrentEmployee;
        }
        public override void AfterRead(RequestInformation requestInformation, DataSourceResult result, int pageSize = 10, int skip = 0)
        {
            var temp = (IQueryable<Task>)result.Data;
            var temp1 = temp.ToList();
            result.Data = temp1.Where(x => x.Employee == EmployeeExtensions.CurrentEmployee).AsQueryable();
            result.Total = ((IQueryable<Task>)result.Data).Count();
        }
    }
}