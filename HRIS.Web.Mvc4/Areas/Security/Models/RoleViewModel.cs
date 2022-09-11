using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Security;
using Souccar.Domain.Validation;
using Project.Web.Mvc4.Extensions;

using Souccar.Infrastructure.Extenstions;
using Project.Web.Mvc4.Models.MasterDetailModels.DetailGridModels;
using Project.Web.Mvc4.Helpers;

namespace Project.Web.Mvc4.Areas.Security.Models
{
    public class RoleViewModel:ViewModel
    {
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults,
                    CrudOperationType operationType, string customInformation = null, Entity parententity = null, IList<DetailData> Details = null)
        {
            var role = typeof(Role).GetAll<Role>()
                .SingleOrDefault(x => x.Name.Equals((entity as Role).Name));
            if (role != null && role.Id != entity.Id)
                validationResults.Add(new ValidationResult() { Message = GlobalResource.AlreadyExistsMessage, Property = typeof(Role).GetProperty("Name") });
        }

        /// <summary>
        /// -
        /// </summary>
        /// <param name="model">-</param>
        /// <param name="type">-</param>
        /// <param name="requestInformation">-</param>
        public override void CustomizeGridModel(Project.Web.Mvc4.Models.GridModel.GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(RoleViewModel).FullName;
            model.Views[0].EditHandler = "RoleEditHandler";
            model.ActionListHandler = "RoleinitializeActionList";
        }
    }
}