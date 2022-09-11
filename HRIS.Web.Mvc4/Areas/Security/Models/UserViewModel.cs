using Project.Web.Mvc4.Models;
using Souccar.Domain.DomainModel;
using Souccar.Domain.PersistenceSupport;
using Souccar.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Castle.Core.Internal;
using Souccar.Domain.Validation;
using Souccar.NHibernate;
using WebMatrix.WebData;
using Project.Web.Mvc4.Areas.Security.Helpers;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.Resource;
using Souccar.Infrastructure.Extenstions;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Extensions;
using Souccar.Domain.Audit;
using Souccar.Domain.Notification;
using Project.Web.Mvc4.Models.MasterDetailModels.DetailGridModels;
using System.IO;

namespace Project.Web.Mvc4.Areas.Security.Models
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class UserViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(UserViewModel).FullName;
            model.Views[0].AfterRequestEnd = "UserAfterRequestEnd";
            model.Views[0].ViewHandler = "initializeView";
            model.Views[0].EditHandler = "UserEditHandler";
            model.ActionListHandler = "initializeActionList";
        }
        public override void BeforeInsert(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, string customInformation = null)
        {
            this.PreventDefault = true;
            var user = (User)entity;

            user.Save();
            WebSecurity.CreateAccount(user.Username, UserHelper.DefaultPassword);

        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity,
            IDictionary<string, object> originalState, IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null, IList<DetailData> Details = null)
        {

            var user = operationType == CrudOperationType.Update ? UserHelper.GetUserByUsername((string)originalState["Username"]) : (User)entity;

            if (user != null && user.Id != entity.Id)
            {
                var prop = typeof(User).GetProperty("Username");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.AlreadyExistsMessage),
                    Property = prop
                });
            }
            DirectoryInfo directoryInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Content/images/theme-" + user.ThemingType.ToString().ToLower()));
            if (!directoryInfo.Exists)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = SecurityLocalizationHelper.GetResource(SecurityLocalizationHelper.ThisThemingIsNotSupported),
                    Property = typeof(User).GetProperty("ThemingType")
                });
            }
            if (operationType == CrudOperationType.Insert)
            {
                if ((UserHelper.NumberOfUser + 1) >= UserHelper.MaxNumberOfUser)
                {
                    validationResults.Add(new ValidationResult()
                    {
                        Message = AccountLocalizationHelper.GetResource(
                            AccountLocalizationHelper.LicenseViolationError),
                        Property = typeof(User).GetProperty("IsEnabled")
                    });

                }
            }
            if (operationType == CrudOperationType.Update)
            {
                var oldIsActive = Convert.ToBoolean(originalState["IsEnabled"]);
                if (user != null && oldIsActive != user.IsEnabled && user.IsEnabled)
                {
                    if (UserHelper.NumberOfUser >= UserHelper.MaxNumberOfUser)
                    {
                        validationResults.Add(new ValidationResult()
                        {
                            Message = AccountLocalizationHelper.GetResource(
                                AccountLocalizationHelper.GetResource(AccountLocalizationHelper.LicenseViolationError)),
                            Property = typeof(User).GetProperty("IsEnabled")
                        });

                    }
                }
            }


        }

        public override void BeforeDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            this.PreventDefault = true;
            var user = entity as User;
            ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(user.Username);
            ServiceFactory.ORMService.All<UserRole>().Where(x => x.User.Username.Equals(user.Username))
                .ForEach(x => x.Delete());
            ServiceFactory.ORMService.AllWithVertualDeleted<Log>().Where(x => x.User.Username.Equals(user.Username))
                .ForEach(x => x.DeleteWithoutValidation());
            ServiceFactory.ORMService.AllWithVertualDeleted<Notify>().Where(x => x.Sender.Username.Equals(user.Username))
                .ForEach(x =>
                {
                    x.Receivers.Clear();
                    x.Save();
                    x.DeleteWithoutValidation();
                });
            ServiceFactory.ORMService.All<User>().Where(x => x.Username.Equals(user.Username))
                .ForEach(x => x.Delete());
        }
    }
}