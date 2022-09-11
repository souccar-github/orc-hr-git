using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.MasterDetailModels.DetailGridModels;
using Souccar.Core.Extensions;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Extensions;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using Souccar.Reflector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Project.Web.Mvc4.Models.Navigation;
using ObjectExtensions = Souccar.Core.Extensions.ObjectExtensions;

namespace Project.Web.Mvc4.Areas.MobileApplication.Controllers
{
    public class MobileCrudController : BaseApiController
    {
        #region helper

        private List<Models.RequestInformation.Navigation.Step> getRibbonDetails(List<Detail> details)
        {
            return details.Select(x => new RequestInformation.Navigation.Step
            {
                Name = x.DetailId,
                Title = x.Title,
                IsDetailHide = x.IsDetailHidden,
                Description = x.Description,
                ImageClass = x.ImageClass,
                TypeName = x.TypeFullName,
                GroupName = x.GroupName,
                GroupOrder = x.GroupOrder
            }).ToList();
        }
        private object getValidationResult(Entity entity, ViewModel viewMode)
        {
            viewMode.BeforeValidation(entity);
            var validationResults = ServiceFactory.ValidationService.Validate((IEntity)entity,null);

            viewMode.AfterValidation(entity, validationResults);

            if (validationResults.Any())
            {
                var errorsMessages = new Dictionary<string, string>();
                foreach (var error in validationResults)
                {
                    if (error.Property == null)
                    {
                        if (errorsMessages.Keys.All(x => x != ""))
                        {
                            errorsMessages.Add("", error.Message);
                        }
                    }
                    else if (errorsMessages.Keys.All(x => x != error.Property.Name))
                        errorsMessages.Add(error.Property.Name, error.Message);
                }
                return new { Data = entity.GetType().ToDynamicData(entity), Errors = errorsMessages };
            }
            return null;
        }
        public static IQueryable<Entity> GetAllWithVertualDeleted(Type t)
        {
            var method = typeof(Souccar.Infrastructure.Extenstions.TypeExtensions).GetMethod("GetAllWithVertualDeleted");
            method = method.MakeGenericMethod(new Type[] { t });
            return (IQueryable<Entity>)method.Invoke(typeof(Souccar.Infrastructure.Extenstions.TypeExtensions), new object[] { typeof(Souccar.Infrastructure.Extenstions.TypeExtensions), Activator.CreateInstance(t) });
        }
        public static IQueryable<Entity> GetAll(Type t)
        {
            var method = typeof(Souccar.Infrastructure.Extenstions.TypeExtensions).GetMethod("GetAll");
            method = method.MakeGenericMethod(new Type[] { t });
            return (IQueryable<Entity>)method.Invoke(typeof(Souccar.Infrastructure.Extenstions.TypeExtensions), new object[] { typeof(Souccar.Infrastructure.Extenstions.TypeExtensions), Activator.CreateInstance(t) });
        }
        private ViewModel getViewModel(string type)
        {
            return string.IsNullOrEmpty(type) ? new ViewModel() : (ViewModel)Activator.CreateInstance(type.ToType());
        }
        #endregion


        [System.Web.Http.HttpPost]
        public IHttpActionResult Read(string viewModelTypeFullName, Type type, int pageSize = 10, int skip = 0, bool serverPaging = true)
        {
            try
            {
                var viewMode = getViewModel(viewModelTypeFullName);
                IQueryable<IEntity> queryable = null;

                queryable = GetAllWithVertualDeleted(type);

                DataSourceResult dataSourse = new DataSourceResult();
                viewMode.BeforeRead();
                if (viewMode.PreventDefault)
                {
                    dataSourse = DataSourceResult.GetDataSourceResult(queryable, type, pageSize, skip, false, null, null, null);
                }
                else
                {
                    dataSourse = DataSourceResult.GetDataSourceResult(queryable, type, pageSize, skip, serverPaging, null, null, null);
                }
                viewMode.AfterRead();

                var data = type.ToDynamicData(dataSourse.Data);
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [System.Web.Http.HttpPost]
        public IHttpActionResult Create(Entity entity, string viewModelTypeFullName, string notificationString = null, List<int> recievers = null)
        {
            var viewMode = getViewModel(viewModelTypeFullName);
            try
            {
                var errorResult = getValidationResult(entity, viewMode);
                if (errorResult != null)
                {
                    return Json(errorResult);
                }
                viewMode.BeforeInsert(entity);
                if (!viewMode.PreventDefault)
                {
                    var result = new List<IAggregateRoot>();
                    result.Add((IAggregateRoot)entity);
                    ServiceFactory.ORMService.SaveTransaction(result, UserExtensions.CurrentUser);
                }
                viewMode.AfterInsert(entity);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(GlobalResource.ExceptionMessage);
            }
        }
        public IHttpActionResult Update(Entity entity, string viewModelTypeFullName)
        {
            var viewMode = getViewModel(viewModelTypeFullName);
            var selectedDetail = entity;
            try
            {
                var errorResult = getValidationResult(selectedDetail, viewMode);
                if (errorResult != null)
                {
                    return Json(errorResult);
                }

                viewMode.BeforeUpdate(selectedDetail);
                if (!viewMode.PreventDefault)
                {
                    var result = new List<IAggregateRoot>();
                    result.Add((IAggregateRoot)entity);
                    ServiceFactory.ORMService.SaveTransaction(result, UserExtensions.CurrentUser);
                }
                viewMode.AfterUpdate((Entity)selectedDetail);
            }
            catch (Exception e)
            {
                return BadRequest(GlobalResource.ExceptionMessage);
            }
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult Delete(Entity entity, List<Detail> _details = null, string viewModelTypeFullName = null)
        {
            var viewMode = getViewModel(viewModelTypeFullName);
            try
            {
                if (_details != null)
                {
                    viewMode.BeforeDelete((Entity) entity);
                    if (!viewMode.PreventDefault)
                    {
                        ((Entity) entity).IsVertualDeleted = true;
                        //-------------------
                        var details = getRibbonDetails(_details);
                        foreach (var detial in details)
                        {
                            var lists = ObjectExtensions.GetPropertyValue(entity, detial.Name);
                            var tempLists = new List<Entity>((lists as IEnumerable<Entity>).AsQueryable());
                            ;


                            var deletedType = detial.TypeName.ToType();
                            foreach (var list in tempLists)
                            {
                                //var test = list;
                                ObjectExtensions.CallMethod(lists, "Remove", new Type[] {deletedType},
                                    new object[] {list});

                            }

                        }

                        //------------------
                        ((IAggregateRoot) entity).Save();
                    }
                    viewMode.AfterDelete((Entity) entity);
                }
                else
                {
                    viewMode.BeforeDelete((Entity) entity);
                    if (!viewMode.PreventDefault)
                    {
                        ObjectExtensions.CallMethod(entity.Id.ToString(), "Remove", new Type[] {entity.GetType()}, new object[] {entity});
                        ((IAggregateRoot) entity).Save();
                    }
                    viewMode.AfterDelete((Entity) entity);
                }
            }
            catch (Exception e)
            {
                return BadRequest(GlobalResource.ExceptionMessage);
            }
            return Ok();
        }
    }
}