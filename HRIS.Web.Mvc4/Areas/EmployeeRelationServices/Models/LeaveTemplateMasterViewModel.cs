using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Helpers.Resource;
using HRIS.Domain.EmployeeRelationServices.Helpers;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class LeaveTemplateMasterViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(LeaveTemplateMasterViewModel).FullName;
           
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)

        {
            var Leavetemplatemaster = entity as LeaveTemplateMaster;
            if (Leavetemplatemaster.LeaveTemplateDetails.Count == 0)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0}", EmployeeRelationServicesGroupNames.GetResourceKey(EmployeeRelationServicesGroupNames
                       .YouMustToAddOneLeaveTemplateDetailAtLeast))
                
                });
            }
            


        }


    }
}