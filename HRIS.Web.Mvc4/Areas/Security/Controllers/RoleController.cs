using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using Project.Web.Mvc4.Areas.Security.Models;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.Controls;
using Souccar.Domain.Security;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using Project.Web.Mvc4.Areas.Security.Helpers;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Domain.DomainModel;
using System;
using Project.Web.Mvc4.Helpers.Resource;

namespace Project.Web.Mvc4.Areas.Security.Controllers
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class RoleController : Controller
    {
        [HttpGet]
        public ActionResult GetAllRoles()
        {
            var data = ServiceFactory.SecurityService.Roles.Select(x => new { Name = x.Name, Id = x.Id }).ToList();
            return Json(new { Data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetAuthorizableElements(string roleId)
        {
            var result = new ManageRoleViewModel();
            if (string.IsNullOrEmpty(roleId))
                return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
            var role = (Role)typeof(Role).GetById(int.Parse(roleId));
            var temp = ServiceFactory.SecurityService.GetAuthorizeTypeRolesForRole(role.Name);

            result.RoleId = role.Id;

            result.AuthorizableModules = AuthorizableElementFactory.AuthorizableModules();
            foreach (var model in result.AuthorizableModules.Where(x => temp.Select(y => y.AuthorizableElementId).Contains(x.Value)))
            {
                model.Dir = "Right";
            }

            result.AuthorizableAggregates = AuthorizableElementFactory.AuthorizableAggregates();
            foreach (var model in result.AuthorizableAggregates.Where(x => temp.Select(y => y.AuthorizableElementId).Contains(x.Value)))
            {
                model.Dir = "Right";
                foreach (var authorizableElementRole in temp.Where(x => x.AuthorizableElementId == model.Value))
                {
                    if (authorizableElementRole.AuthorizeType == AuthorizeType.Addable && !model.Metadata.Select(x => x.Id).Contains("Insert"))
                        model.Metadata.Add(new MetadataItem() { Id = "Insert", Name = "Insert" });
                    if (authorizableElementRole.AuthorizeType == AuthorizeType.Editable && !model.Metadata.Select(x => x.Id).Contains("Edit"))
                        model.Metadata.Add(new MetadataItem() { Id = "Edit", Name = "Edit" });
                    if (authorizableElementRole.AuthorizeType == AuthorizeType.Deleteable && !model.Metadata.Select(x => x.Id).Contains("Delete"))
                        model.Metadata.Add(new MetadataItem() { Id = "Delete", Name = "Delete" });
                }
            }

            result.AuthorizableReports = AuthorizableElementFactory.AuthorizableReports();
            foreach (var model in result.AuthorizableReports.Where(x => temp.Select(y => y.AuthorizableElementId).Contains(x.Value)))
            {
                model.Dir = "Right";
            }

            result.AuthorizableIndexs = AuthorizableElementFactory.AuthorizableIndexs();
            foreach (var model in result.AuthorizableIndexs.Where(x => temp.Select(y => y.AuthorizableElementId).Contains(x.Value)))
            {
                model.Dir = "Right";
                foreach (var authorizableElementRole in temp.Where(x => x.AuthorizableElementId == model.Value))
                {
                    if (authorizableElementRole.AuthorizeType == AuthorizeType.Addable && !model.Metadata.Select(x => x.Id).Contains("Insert"))
                        model.Metadata.Add(new MetadataItem() { Id = "Insert", Name = "Insert" });
                    if (authorizableElementRole.AuthorizeType == AuthorizeType.Editable && !model.Metadata.Select(x => x.Id).Contains("Edit"))
                        model.Metadata.Add(new MetadataItem() { Id = "Edit", Name = "Edit" });
                    if (authorizableElementRole.AuthorizeType == AuthorizeType.Deleteable && !model.Metadata.Select(x => x.Id).Contains("Delete"))
                        model.Metadata.Add(new MetadataItem() { Id = "Delete", Name = "Delete" });
                }
            }

            result.AuthorizableDashboards = AuthorizableElementFactory.AuthorizableDashboards();
            foreach (var model in result.AuthorizableDashboards.Where(x => temp.Select(y => y.AuthorizableElementId).Contains(x.Value)))
            {
                model.Dir = "Right";
            }

            result.AuthorizableServices = AuthorizableElementFactory.AuthorizableServices();
            foreach (var model in result.AuthorizableServices.Where(x => temp.Select(y => y.AuthorizableElementId).Contains(x.Value)))
            {
                model.Dir = "Right";
            }


            result.AuthorizableConfigurations = AuthorizableElementFactory.AuthorizableConfigurations();
            foreach (var model in result.AuthorizableConfigurations.Where(x => temp.Select(y => y.AuthorizableElementId).Contains(x.Value)))
            {
                model.Dir = "Right";
                foreach (var authorizableElementRole in temp.Where(x => x.AuthorizableElementId == model.Value))
                {
                    if (authorizableElementRole.AuthorizeType == AuthorizeType.Addable && !model.Metadata.Select(x => x.Id).Contains("Insert"))
                        model.Metadata.Add(new MetadataItem() { Id = "Insert", Name = "Insert" });
                    if (authorizableElementRole.AuthorizeType == AuthorizeType.Editable && !model.Metadata.Select(x => x.Id).Contains("Edit"))
                        model.Metadata.Add(new MetadataItem() { Id = "Edit", Name = "Edit" });
                    if (authorizableElementRole.AuthorizeType == AuthorizeType.Deleteable && !model.Metadata.Select(x => x.Id).Contains("Delete"))
                        model.Metadata.Add(new MetadataItem() { Id = "Delete", Name = "Delete" });
                }
            }

            result.AuthorizableDetails = AuthorizableElementFactory.AuthorizableDetails();
            foreach (var model in result.AuthorizableDetails.Where(x => temp.Select(y => y.AuthorizableElementId).Contains(x.Value)))
            {
                model.Dir = "Right";
                foreach (var authorizableElementRole in temp.Where(x => x.AuthorizableElementId == model.Value))
                {
                    if (authorizableElementRole.AuthorizeType == AuthorizeType.Addable && !model.Metadata.Select(x => x.Id).Contains("Insert"))
                        model.Metadata.Add(new MetadataItem() { Id = "Insert", Name = "Insert" });
                    if (authorizableElementRole.AuthorizeType == AuthorizeType.Editable && !model.Metadata.Select(x => x.Id).Contains("Edit"))
                        model.Metadata.Add(new MetadataItem() { Id = "Edit", Name = "Edit" });
                    if (authorizableElementRole.AuthorizeType == AuthorizeType.Deleteable && !model.Metadata.Select(x => x.Id).Contains("Delete"))
                        model.Metadata.Add(new MetadataItem() { Id = "Delete", Name = "Delete" });
                }
            }

            result.AuthorizableActionLists = AuthorizableElementFactory.AuthorizableActionLists();
            foreach (var model in result.AuthorizableActionLists.Where(x => temp.Select(y => y.AuthorizableElementId).Contains(x.Value)))
            {
                model.Dir = "Right";
            }

            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetAuthorizableElementsForFields(string roleId)
        {
            var result = new ManageRoleViewModel();
            if (string.IsNullOrEmpty(roleId))
                return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
            var role = (Role)typeof(Role).GetById(int.Parse(roleId));
            var temp = ServiceFactory.SecurityService.GetAuthorizeTypeRolesForRole(role.Name);
            var temp2 = ServiceFactory.SecurityService.GetAuthorizeFieldTypeRolesForRole(role.Name);
            var temp3 = ServiceFactory.SecurityService.GetAuthorizeFieldDetailsTypeRolesForRole(role.Name);
            result.RoleId = role.Id;

            result.AuthorizableModules = AuthorizableElementFactory.AuthorizableModules();
            foreach (var model in result.AuthorizableModules.Where(x => temp.Select(y => y.AuthorizableElementId).Contains(x.Value)))
            {
                model.Dir = "Right";
            }

            var authorizableAggregatesest = AuthorizableElementFactory.AuthorizableAggregates();
            foreach (var model in authorizableAggregatesest.Where(x => temp.Select(y => y.AuthorizableElementId).Contains(x.Value)))
            {

                var authorizElements = temp.Where(x => x.AuthorizableElementId == model.Value);

                if (authorizElements.Any(x => x.AuthorizeType == AuthorizeType.Addable ||
                                              x.AuthorizeType == AuthorizeType.Editable))
                {

                    if (temp2.Count(x => x.AuthorizableElementId == model.Value) > 0)
                        model.Dir = "Right";
                    result.AuthorizableAggregates.Add(
                        model);

                }


            }




            var authorizableConfigurations = AuthorizableElementFactory.AuthorizableConfigurations();

            foreach (var model in authorizableConfigurations.Where(x => temp.Select(y => y.AuthorizableElementId).Contains(x.Value)))
            {

                var authorizElements = temp.Where(x => x.AuthorizableElementId == model.Value);

                if (authorizElements.Any(x => x.AuthorizeType == AuthorizeType.Addable ||
                                              x.AuthorizeType == AuthorizeType.Editable))
                {

                    if (temp2.Count(x => x.AuthorizableElementId == model.Value) > 0)
                        model.Dir = "Right";
                    result.AuthorizableConfigurations.Add(
                        model);

                }


            }

            var authorizableDetails = AuthorizableElementFactory.AuthorizableDetails();
            foreach (var model in authorizableDetails.Where(x => temp.Select(y => y.AuthorizableElementId).Contains(x.Value)))
            {

                var authorizElements = temp.Where(x => x.AuthorizableElementId == model.Value);

                if (authorizElements.Any(x => x.AuthorizeType == AuthorizeType.Addable ||
                                              x.AuthorizeType == AuthorizeType.Editable))
                {

                    if (temp3.Count(x => x.AuthorizableElementId == model.Value) > 0)
                        model.Dir = "Right";

                    result.AuthorizableDetails.Add(
                        model);

                }


            }




            result.AuthorizableConfigurationField = AuthorizableElementFactory.AuthorizableField(result.AuthorizableConfigurations);
            foreach (var model in result.AuthorizableConfigurationField.Where(x => temp2.Any(y => y.AuthorizableFieldId == x.Value && y.AuthorizableElementId == x.Group)))
            {
                model.Dir = "Right";
            }
            result.AuthorizableAggregateField = AuthorizableElementFactory.AuthorizableField(result.AuthorizableAggregates);
            foreach (var model in result.AuthorizableAggregateField.Where(x => temp2.Any(y => y.AuthorizableFieldId == x.Value && y.AuthorizableElementId == x.Group)))
            {
                model.Dir = "Right";
            }

            result.AuthorizableDetailField = AuthorizableElementFactory.AuthorizableField(result.AuthorizableDetails);
            foreach (var model in result.AuthorizableDetailField.Where(x => temp3.Any(y => y.AuthorizableFieldId == x.Value && y.AuthorizableElementId == x.Group)))
            {
                model.Dir = "Right";
            }

            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Save(ManageRoleViewModel model)
        {
            var role = (Role)typeof(Role).GetById(model.RoleId);
            List<AuthorizableElementRole> authorizableElements = typeof(AuthorizableElementRole).GetAll<AuthorizableElementRole>().Where(x => x.Role.Id == role.Id).ToList();

            ServiceFactory.ORMService.DeleteTransaction<AuthorizableElementRole>(authorizableElements, UserExtensions.CurrentUser);

            var items = new List<AuthorizableElementRole>();

            foreach (var element in model.AuthorizableAggregates.Where(x => x.Dir.Equals("Right")))
            {
                var authorizeType = AuthorizeType.Visible;
                var start = DateTime.Now;
                var info = SecurityLocalizationHelper.GetResource(SecurityLocalizationHelper.ManageRole);
                var entities = new List<IAggregateRoot>();
                var item = new AuthorizableElementRole()
                {
                    AuthorizableElementId = element.Value,
                    AuthorizableElementType = AuthorizableElementType.Aggregate,
                    AuthorizeType = authorizeType,
                    ModuleName = element.Group,
                    Role = role
                };

                items.Add(item);
                //item.Save();

                foreach (var metadata in element.Metadata)
                {
                    switch (metadata.Id)
                    {
                        case "Edit":
                            authorizeType = AuthorizeType.Editable;
                            break;
                        case "Insert":
                            authorizeType = AuthorizeType.Addable;
                            break;
                        case "Delete":
                            authorizeType = AuthorizeType.Deleteable;
                            break;
                    }
                    item = new AuthorizableElementRole()
                    {
                        AuthorizableElementId = element.Value,
                        AuthorizableElementType = AuthorizableElementType.Aggregate,
                        AuthorizeType = authorizeType,
                        ModuleName = element.Group,
                        Role = role
                    };

                    entities.Add(item);
                }
                ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, role, Souccar.Domain.Audit.OperationType.Update, info, start);



            }

            foreach (var element in model.AuthorizableActionLists.Where(x => x.Dir.Equals("Right")))
            {
                var item = new AuthorizableElementRole()
                {
                    AuthorizableElementId = element.Value,
                    AuthorizableElementType = AuthorizableElementType.ActionListCommand,
                    AuthorizeType = AuthorizeType.Visible,
                    ModuleName = element.Group,
                    Role = role
                };
                items.Add(item);
                //item.Save();
            }

            foreach (var element in model.AuthorizableConfigurations.Where(x => x.Dir.Equals("Right")))
            {
                var authorizeType = AuthorizeType.Visible;
                var item = new AuthorizableElementRole()
                {
                    AuthorizableElementId = element.Value,
                    AuthorizableElementType = AuthorizableElementType.Config,
                    AuthorizeType = authorizeType,
                    ModuleName = element.Group,
                    Role = role
                };
                items.Add(item);
                //item.Save();
                foreach (var metadata in element.Metadata)
                {
                    switch (metadata.Id)
                    {
                        case "Edit":
                            authorizeType = AuthorizeType.Editable;
                            break;
                        case "Insert":
                            authorizeType = AuthorizeType.Addable;
                            break;
                        case "Delete":
                            authorizeType = AuthorizeType.Deleteable;
                            break;
                    }
                    item = new AuthorizableElementRole()
                    {
                        AuthorizableElementId = element.Value,
                        AuthorizableElementType = AuthorizableElementType.Config,
                        AuthorizeType = authorizeType,
                        ModuleName = element.Group,
                        Role = role
                    };
                    items.Add(item);
                    //item.Save();
                }

            }

            foreach (var element in model.AuthorizableDashboards.Where(x => x.Dir.Equals("Right")))
            {
                var item = new AuthorizableElementRole()
                {
                    AuthorizableElementId = element.Value,
                    AuthorizableElementType = AuthorizableElementType.Dashboard,
                    AuthorizeType = AuthorizeType.Visible,
                    ModuleName = element.Group,
                    Role = role
                };
                items.Add(item);
                //item.Save();
            }

            foreach (var element in model.AuthorizableDetails.Where(x => x.Dir.Equals("Right")))
            {
                var authorizeType = AuthorizeType.Visible;
                var item = new AuthorizableElementRole()
                {
                    AuthorizableElementId = element.Value,
                    AuthorizableElementType = AuthorizableElementType.Detail,
                    AuthorizeType = authorizeType,
                    ModuleName = element.Group,
                    Role = role
                };
                items.Add(item);
                //item.Save();
                foreach (var metadata in element.Metadata)
                {
                    switch (metadata.Id)
                    {
                        case "Edit":
                            authorizeType = AuthorizeType.Editable;
                            break;
                        case "Insert":
                            authorizeType = AuthorizeType.Addable;
                            break;
                        case "Delete":
                            authorizeType = AuthorizeType.Deleteable;
                            break;
                    }
                    item = new AuthorizableElementRole()
                    {
                        AuthorizableElementId = element.Value,
                        AuthorizableElementType = AuthorizableElementType.Detail,
                        AuthorizeType = authorizeType,
                        ModuleName = element.Group,
                        Role = role
                    };
                    items.Add(item);
                    //item.Save();
                }
            }

            foreach (var element in model.AuthorizableIndexs.Where(x => x.Dir.Equals("Right")))
            {
                var authorizeType = AuthorizeType.Visible;
                var item = new AuthorizableElementRole()
                {
                    AuthorizableElementId = element.Value,
                    AuthorizableElementType = AuthorizableElementType.Index,
                    AuthorizeType = AuthorizeType.Visible,
                    ModuleName = element.Group,
                    Role = role
                };
                items.Add(item);
                //item.Save();
                foreach (var metadata in element.Metadata)
                {
                    switch (metadata.Id)
                    {
                        case "Edit":
                            authorizeType = AuthorizeType.Editable;
                            break;
                        case "Insert":
                            authorizeType = AuthorizeType.Addable;
                            break;
                        case "Delete":
                            authorizeType = AuthorizeType.Deleteable;
                            break;
                    }
                    item = new AuthorizableElementRole()
                    {
                        AuthorizableElementId = element.Value,
                        AuthorizableElementType = AuthorizableElementType.Index,
                        AuthorizeType = authorizeType,
                        ModuleName = element.Group,
                        Role = role
                    };
                    items.Add(item);
                    //item.Save();
                }
            }

            foreach (var element in model.AuthorizableModules.Where(x => x.Dir.Equals("Right")))
            {
                var item = new AuthorizableElementRole()
                {
                    AuthorizableElementId = element.Value,
                    AuthorizableElementType = AuthorizableElementType.Module,
                    AuthorizeType = AuthorizeType.Visible,
                    ModuleName = element.Group,
                    Role = role
                };
                items.Add(item);
                //item.Save();
            }

            foreach (var element in model.AuthorizableReports.Where(x => x.Dir.Equals("Right")))
            {
                var item = new AuthorizableElementRole()
                {
                    AuthorizableElementId = element.Value,
                    AuthorizableElementType = AuthorizableElementType.Report,
                    AuthorizeType = AuthorizeType.Visible,
                    ModuleName = element.Group,
                    Role = role
                };
                items.Add(item);
                //item.Save();
            }

            foreach (var element in model.AuthorizableServices.Where(x => x.Dir.Equals("Right")))
            {
                var item = new AuthorizableElementRole()
                {
                    AuthorizableElementId = element.Value,
                    AuthorizableElementType = AuthorizableElementType.Service,
                    AuthorizeType = AuthorizeType.Visible,
                    ModuleName = element.Group,
                    Role = role
                };
                items.Add(item);
                //item.Save();
            }

            ServiceFactory.ORMService.SaveTransaction<AuthorizableElementRole>(items, UserExtensions.CurrentUser);

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveFieldSecurity(ManageRoleViewModel model)
        {

            var role = (Role)typeof(Role).GetById(model.RoleId);

            var authorizableField = typeof(AuthorizableFieldRole).GetAll<AuthorizableFieldRole>().Where(x => x.Role.Id == role.Id);
            var authorizablDetaileField = typeof(AuthorizableDetailsFieldRole).GetAll<AuthorizableDetailsFieldRole>().Where(x => x.Role.Id == role.Id);

            foreach (var element in authorizableField)
            {
                element.Delete();
            }
            foreach (var element in authorizablDetaileField)
            {
                element.Delete();
            }

            foreach (var element in model.AuthorizableAggregateField)
            {
                var item = new AuthorizableFieldRole()
                {
                    ModuleName = element.Description,
                    AuthorizableElementId = element.Group,
                    AuthorizableFieldId = element.Value,
                    IsHidden = true,
                    Role = role
                };
                item.Save();
            }
            foreach (var element in model.AuthorizableConfigurationField)
            {
                var item = new AuthorizableFieldRole()
                {
                    ModuleName = element.Description,
                    AuthorizableElementId = element.Group,
                    AuthorizableFieldId = element.Value,
                    IsHidden = true,
                    Role = role
                };
                item.Save();
            }
            foreach (var element in model.AuthorizableDetailField)
            {
                var item = new AuthorizableDetailsFieldRole()
                {
                    ModuleName = element.Description,
                    AuthorizableElementId = element.Group,
                    AuthorizableFieldId = element.Value,
                    IsHidden = true,
                    Role = role
                };
                item.Save();
            }
            return Json(true, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetRole(int userId)
        {
            var roles = ServiceFactory.SecurityService.Roles.Select(
                x => new DualSelectListModel()
                {
                    Value = x.Id.ToString(),
                    Title = x.Name,
                    Description = x.Description,
                    Dir = DualSelectListDirection.Left.ToString()
                }).ToList();
            foreach (var role in roles.Where(x => ServiceFactory.SecurityService.RolesForUser(userId).Select(y => y.Name).Contains(x.Title)))
            {
                role.Dir = DualSelectListDirection.Right.ToString();
            }
            return Json(roles.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveRolesOfUser(int userId, IList<DualSelectListModel> roles)
        {
            var start = DateTime.Now;
            var command = SecurityLocalizationHelper.GetResource(SecurityLocalizationHelper.AddUserToRole);
            var user = ServiceFactory.ORMService.GetById<User>(userId);
            ServiceFactory.SecurityService.SetRolesToUser(userId, new List<Role>());
            foreach (var role in roles.Where(x => x.Dir == DualSelectListDirection.Right.ToString()))
            {
                ServiceFactory.SecurityService.AddRoleToUser(userId, int.Parse(role.Value));
            }
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { }, UserExtensions.CurrentUser, user, Souccar.Domain.Audit.OperationType.Update, command, start);
            return Json(true, JsonRequestBehavior.AllowGet);

        }
    }
}