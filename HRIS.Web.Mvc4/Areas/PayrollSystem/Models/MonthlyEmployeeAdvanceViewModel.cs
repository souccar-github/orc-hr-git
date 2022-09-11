using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.PayrollSystem.RootEntities;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Extenstions;
using Souccar.Infrastructure.Core;
using Souccar.Domain.DomainModel;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using Souccar.Domain.Security;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.Personnel.Enums;
using Souccar.Domain.Workflow.Enums;

namespace Project.Web.Mvc4.Areas.PayrollSystem.Models
{
    public class MonthlyEmployeeAdvanceViewModel : ViewModel
    {
        //
        // GET: /PayrollSystem/EmployeeLoan/
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(MonthlyEmployeeAdvanceViewModel).FullName;
            
                model.Views[0].ViewHandler = "EmployeeAdvanceViewHandler";
              

                model.ToolbarCommands.RemoveAt(0);

                model.ActionListHandler = "initializeAdvanceMonthlyCardActionList";

          
        }


    }
}