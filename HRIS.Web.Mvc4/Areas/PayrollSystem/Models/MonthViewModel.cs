using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using  Project.Web.Mvc4.Helpers.Resource;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Extenstions;
using Souccar.Domain.DomainModel;

namespace Project.Web.Mvc4.Areas.PayrollSystem.Models
{
    public class MonthViewModel : ViewModel
    {
        //
        // GET: /PayrollSystem/MonthViewModel/
       public override void CustomizeGridModel(GridViewModel model,Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof (MonthViewModel).FullName;
        }

        public override void AfterValidation(RequestInformation requestInformation,
            Souccar.Domain.DomainModel.Entity entity,
            IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var month = (Month)entity;
            if (month.MonthStatus == MonthStatus.Locked)
            {
                var prop = typeof(Month).GetProperty("MonthStatus");
                validationResults.Add(
                    new ValidationResult()
                    {
                        Message = string.Format("{0} {1}", prop.GetTitle(),
                           PayrollSystemLocalizationHelper.GetResource(
                                    PayrollSystemLocalizationHelper.YouCannotEditLockedMonth)),
                        Property = prop
                    });
            }
        }
     

    }
}
