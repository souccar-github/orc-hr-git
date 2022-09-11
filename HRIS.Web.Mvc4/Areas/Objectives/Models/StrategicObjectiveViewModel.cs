using System;
using System.Collections.Generic;
using System.Linq;
using  Project.Web.Mvc4.Models.GridModel;
using System.Web;
using  Project.Web.Mvc4.Models;
using HRIS.Domain.Objectives.RootEntities;
using Souccar.Domain.DomainModel;
using  Project.Web.Mvc4.Extensions;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Extenstions;


namespace Project.Web.Mvc4.Areas.Objectives.Models
{
    public class StrategicObjectiveViewModel:ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {

            model.ViewModelTypeFullName = typeof(StrategicObjectiveViewModel).FullName;
        }
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)
      
        {
            //Unique Code.
            var strategicObjective = typeof (StrategicObjective).GetAll<StrategicObjective>()
                .SingleOrDefault(x => x.Code == ((StrategicObjective) entity).Code);
            if (strategicObjective != null && strategicObjective.Id != entity.Id)
               validationResults.Add(new ValidationResult() { Message = "Code already exist.", Property = typeof(StrategicObjective).GetProperty("Code") });
            //Unique Name.
            strategicObjective = typeof(StrategicObjective).GetAll<StrategicObjective>()
                .SingleOrDefault(x => x.Name == ((StrategicObjective)entity).Name);   
            if (strategicObjective != null && strategicObjective.Id != entity.Id)
                validationResults.Add(new ValidationResult() { Message = "Name already exist.", Property = typeof(StrategicObjective).GetProperty("Name") });
        }

    }
}
