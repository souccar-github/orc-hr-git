using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Models.GridModel;
using Project.Web.Mvc4.Models.Navigation;
using Souccar.Core.Fasterflect;
using Souccar.Core.Utilities;
using Souccar.Domain.DomainModel;
using Project.Web.Mvc4.Models;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Services.Notification;
using Souccar.Reflector;
using Souccar.Domain.Extensions;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Extenstions;

using Project.Web.Mvc4.Helpers;
using Souccar.Domain.Notification;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Infrastructure.Helpers;
using ObjectExtensions = Souccar.Core.Extensions.ObjectExtensions;
using Project.Web.Mvc4.ProjectModels;
using Project.Web.Mvc4.Models.MasterDetailModels.DetailGridModels;
using Project.Web.Mvc4.Areas;

namespace Project.Web.Mvc4.Controllers
{
    //After Apply Master Detail Feature
    public class CrudController : Controller
    {
        #region Actions
        [HttpPost]
        public ActionResult Index(RequestInformation requestInformation)
        {
            var type = requestInformation.NavigationInfo.Previous.Last().TypeName.ToType();
            if (type == null)
            {
                requestInformation.NavigationInfo.Previous.Remove(requestInformation.NavigationInfo.Previous.Last());
                type = requestInformation.NavigationInfo.Previous.Last().TypeName.ToType();
            }
            requestInformation.NavigationInfo.Next.Clear();
            requestInformation.NavigationInfo.Next = getRibbonDetails(getAuthorizedDetails(getDetails(requestInformation)));
            return Json(new { gridModel = GridViewModelFactory.Create(type, requestInformation), requestInfo = requestInformation }, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// Author: Yaseen Alrefaee
        /// Update: 07/12/2013
        /// Update: 11/12/2013
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="skip"></param>
        /// <param name="serverPaging"> </param>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="group"> </param>
        /// <param name="requestInformation"></param>
        /// <param name="viewModelTypeFullName"> </param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Read(int pageSize = 10, int skip = 0, bool serverPaging = true, IList<DetailData> Details = null, IEnumerable<GridSort> sort = null, GridFilter filter = null, IEnumerable<GridGroup> group = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null)
        {
            var viewMode = getViewModel(viewModelTypeFullName);
            var previous = requestInformation.NavigationInfo.Previous;
            var entityType = previous.Last().TypeName.ToType();
            UpdateFilter(filter, entityType);
            IQueryable<IEntity> queryable = null;

            if (previous.Count == 1)
                queryable = GetAllWithVertualDeleted(previous[0].TypeName.ToType());
            else
            {
                var selectedDetail = (Entity)previous[0].TypeName.ToType().GetById(previous[0].RowId);
                for (var i = 1; i < previous.Count; i++)
                {
                    var list = ObjectExtensions.GetPropertyValue(selectedDetail, previous[i].Name);
                    //if (previous[i].RowId == 0)
                    if (i == previous.Count - 1)
                        queryable = (list as IEnumerable<IEntity>).AsQueryable();
                    else
                        selectedDetail = (list as IEnumerable<Entity>).AsQueryable().SingleOrDefault(x => x.Id == previous[i].RowId);
                }
            }
            DataSourceResult dataSourse = new DataSourceResult();
            viewMode.BeforeRead(requestInformation);
            if (viewMode.PreventDefault)
            {
                dataSourse = DataSourceResult.GetDataSourceResult(queryable, entityType, pageSize, skip, false, sort, filter, requestInformation);
            }
            else
            {
                dataSourse = DataSourceResult.GetDataSourceResult(queryable, entityType, pageSize, skip, serverPaging, sort, filter, requestInformation);
            }
            viewMode.AfterRead(requestInformation, dataSourse, pageSize, skip);

            var data = entityType.ToDynamicData(dataSourse.Data);

            //return Json(new { Data = data, TotalCount = dataSourse.Total });
            return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { Data = data, TotalCount = dataSourse.Total });
        }
        /// <summary>
        /// Update: Yaseen Alrefaee
        /// Date: 14/09/2013
        /// Description: add enum loop
        /// </summary>
        /// <param name="data"></param>
        /// <param name="requestInformation"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult BeforeCreate(RequestInformation requestInformation = null, string viewModelTypeFullName = null, string customInformation = null)
        {
            var viewMode = getViewModel(viewModelTypeFullName);
            var previous = requestInformation.NavigationInfo.Previous;
            var next = requestInformation.NavigationInfo.Next;
            var msg = string.Empty;

            return viewMode.BeforeCreate(requestInformation, customInformation);

        }

        [HttpPost]
        public ActionResult Create(IDictionary<string, object> data = null, IList<DetailData> Details = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null, List<Column> columns = null, string customInformation = null, string notificationTitle = null, string notificationString = null, List<int> recievers = null)
        {
            List<Entity> affectedEntities = new List<Entity>();
            DateTime startTimeOfProcess = DateTime.Now;
            var viewMode = getViewModel(viewModelTypeFullName);

            var previous = requestInformation.NavigationInfo.Previous;
            var next = requestInformation.NavigationInfo.Next;
            try
            {
                if (previous.Count == 1)
                {
                    var entity = (Entity)Activator.CreateInstance(previous[0].TypeName.ToType());
                    setValue(data, entity);
                    updataDetails(entity, Details);

                    var errorResult = getValidationResult(entity, null, requestInformation, viewMode,
                        CrudOperationType.Insert, customInformation, Details);


                    resetNewDetailsIdsForAll(entity, Details);
                    if (errorResult != null)
                    {

                        var test =
                           columns.Where(x => x.IsRequired == false && x.Hidden)
                                .Select(x => x.FieldName).ToList();
                        var propertyInfo = (IDictionary<string, string>)ObjectExtensions.GetPropertyValue(errorResult, "Errors");

                        foreach (var key in propertyInfo.Keys.ToList())
                        {
                            if (!test.Contains(key))
                            {
                                ObjectExtensions.SetPropertyValue(errorResult, "Errors", propertyInfo);


                                return Json(errorResult);
                            }
                            else
                            {
                                propertyInfo.Remove(key);

                            }


                        }

                    }
                    viewMode.BeforeInsert(requestInformation, entity, customInformation);
                    if (!viewMode.PreventDefault)
                    {
                        saveEntityWithNotification(entity, CrudOperationType.Insert, startTimeOfProcess, notificationTitle, notificationString, recievers);
                        //((IAggregateRoot) entity).Save();
                    }
                    viewMode.AfterInsert(requestInformation, entity, customInformation);
                    return Json(new { Data = previous[0].TypeName.ToType().ToDynamicData(entity) });
                }
                else
                {
                    var entity = previous[0].TypeName.ToType().GetById(previous[0].RowId);
                    var selectedDetail = entity;
                    affectedEntities.Add(selectedDetail);
                    for (var i = 1; i < previous.Count; i++)
                    {
                        var list = ObjectExtensions.GetPropertyValue(selectedDetail, previous[i].Name);
                        if (previous[i] != previous.Last())
                        {
                            selectedDetail = (list as IEnumerable<Entity>).AsQueryable()
                                    .SingleOrDefault(x => x.Id == previous[i].RowId);
                            affectedEntities.Add(selectedDetail);
                        }
                        else
                        {
                            var createdType = previous.Last().TypeName.ToType();
                            var createdEntity = (Entity)Activator.CreateInstance(createdType);
                            setValue(data, createdEntity);
                            updataDetails(createdEntity, Details);
                            resetNewDetailsIdsForAll(createdEntity, Details);
                            var errorResult = getValidationResult(createdEntity, null, requestInformation, viewMode, CrudOperationType.Insert, customInformation);
                            if (errorResult != null)
                            {

                                var allowError = columns.Where(x => x.IsRequired == false && x.Hidden)
                                        .Select(x => x.FieldName).ToList();
                                var propertyInfo = (IDictionary<string, string>)ObjectExtensions.GetPropertyValue(errorResult, "Errors");
                                foreach (var key in propertyInfo.Keys)
                                {
                                    if (!allowError.Contains(key))
                                    {
                                        return Json(errorResult);
                                    }
                                }

                            }

                            viewMode.BeforeInsert(requestInformation, createdEntity, customInformation);
                            if (!viewMode.PreventDefault)
                            {
                                var method = GetAddMethod(selectedDetail, createdType);
                                method.Invoke(selectedDetail, new[] { createdEntity });
                                saveEntityWithNotification((Entity)entity, CrudOperationType.Insert, startTimeOfProcess,
                                    notificationTitle, notificationString, recievers, (Entity)createdEntity, affectedEntities);
                                //((IAggregateRoot)entity).Save();
                            }
                            viewMode.AfterInsert(requestInformation, (Entity)createdEntity, customInformation);
                            return Json(new { Data = createdType.ToDynamicData((Entity)createdEntity) });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { Data = data, Errors = new { Exception = GlobalResource.ExceptionMessage } });
                //return Json(new { Data = data, Errors = new { Exception = e.Message } });
            }
            return null;
        }



        /// <summary>
        /// Author: Yaseen Alrefaee
        /// Update: 14/09/2013
        /// Update: 07/12/2013
        /// Description: add enum loop
        /// </summary>
        /// <param name="data"></param>
        /// <param name="requestInformation"></param>
        /// <returns></returns>

        public ActionResult Update(IDictionary<string, object> data = null, IList<DetailData> Details = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null, List<Column> columns = null, string customInformation = null, string notificationTitle = null, string notificationString = null, List<int> recievers = null)
        {
            List<Entity> affectedEntities = new List<Entity>();
            DateTime startTimeOfProcess = DateTime.Now;
            var viewMode = getViewModel(viewModelTypeFullName);
            var previous = requestInformation.NavigationInfo.Previous;
            var entity = previous[0].TypeName.ToType().GetById(previous.Count > 1 ? previous[0].RowId : (int)data["Id"]);
            var selectedDetail = entity;
            if (previous.Count > 1)
                affectedEntities.Add(selectedDetail);
            for (var i = 1; i < previous.Count; i++)
            {
                var list = ObjectExtensions.GetPropertyValue(selectedDetail, previous[i].Name);
                selectedDetail = (list as IEnumerable<Entity>).AsQueryable().SingleOrDefault(
                              x => x.Id == (previous[i].RowId != 0 ? previous[i].RowId : (int)data["Id"]));
                if (previous[i].RowId != 0)
                    affectedEntities.Add(selectedDetail);
            }
            try
            {
                var orginalObject = selectedDetail.ToDynamicObj();
                setValue(data, selectedDetail);
                updataDetails(selectedDetail, Details);
                var errorResult = getValidationResult((Entity)selectedDetail, orginalObject, requestInformation, viewMode,
                    CrudOperationType.Update, customInformation, Details);

                resetNewDetailsIdsForAll(selectedDetail, Details);
                if (errorResult != null)
                {

                    var allowError =
                       columns.Where(x => x.IsRequired == false && x.Hidden)
                            .Select(x => x.FieldName).ToList();
                    var propertyInfo = (IDictionary<string, string>)ObjectExtensions.GetPropertyValue(errorResult, "Errors");
                    foreach (var key in propertyInfo.Keys)
                    {
                        if (!allowError.Contains(key))
                        {
                            return Json(errorResult);
                        }


                    }

                }

                viewMode.BeforeUpdate(requestInformation, (Entity)selectedDetail, orginalObject, customInformation);
                if (!viewMode.PreventDefault)
                {
                    saveEntityWithNotification(entity, CrudOperationType.Update,
                        startTimeOfProcess, notificationTitle, notificationString, recievers, (Entity)selectedDetail, affectedEntities);
                    //((IAggregateRoot)entity).Save();
                }
                viewMode.AfterUpdate(requestInformation, (Entity)selectedDetail, orginalObject, customInformation);
                viewMode.AfterUpdate(requestInformation, (Entity)selectedDetail, orginalObject, customInformation, Details);
            }
            catch (Exception e)
            {
                return Json(new { Data = previous.Last().TypeName.ToType().ToDynamicData((Entity)selectedDetail), Errors = new { Exception = GlobalResource.ExceptionMessage } });
                //return Json(new { Data = previous.Last().TypeName.ToType().ToDynamicData((Entity)selectedDetail), Errors = new { Exception = e.Message } });
            }
            return Json(new { Data = previous.Last().TypeName.ToType().ToDynamicData((Entity)selectedDetail) });
        }
        /// <summary>
        /// Yaseen Alrefaee
        /// </summary>
        /// <param name="data"></param>
        /// <param name="requestInformation"></param>
        /// <param name="viewModelTypeFullName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(IDictionary<string, object> data = null, IList<DetailData> Details = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null, string customInformation = null)
        {
            List<Entity> affectedEntities = new List<Entity>();
            DateTime startTimeOfProcess = DateTime.Now;
            string Notification = null;
            var viewMode = getViewModel(viewModelTypeFullName);
            var previous = requestInformation.NavigationInfo.Previous;
            var entity = previous[0].TypeName.ToType().GetById(previous.Count > 1 ? previous[0].RowId : (int)data["Id"]);
            try
            {
                if (previous.Count == 1)
                {
                    viewMode.BeforeDelete(requestInformation, (Entity)entity, customInformation);
                    if (!viewMode.PreventDefault)
                    {
                        ((Entity)entity).IsVertualDeleted = true;
                        //-------------------
                        var detials = getRibbonDetails((getDetails(requestInformation)));
                        foreach (var detial in detials)
                        {
                            var lists = ObjectExtensions.GetPropertyValue(entity, detial.Name);
                            var tempLists = new List<Entity>((lists as IEnumerable<Entity>).AsQueryable()); ;


                            var deletedType = detial.TypeName.ToType();
                            foreach (var list in tempLists)
                            {
                                //var test = list;
                                ObjectExtensions.CallMethod(lists, "Remove", new Type[] { deletedType }, new object[] { list });

                            }

                        }

                        //------------------
                        var result = new List<IAggregateRoot>();
                        result.Add((IAggregateRoot)entity);
                        ServiceFactory.ORMService.SaveTransaction(result, UserExtensions.CurrentUser,
                            entity, Souccar.Domain.Audit.OperationType.Delete,
                            Notification, startTimeOfProcess);

                    }
                    viewMode.AfterDelete(requestInformation, (Entity)entity, customInformation);
                }
                else
                {
                    var selectedDetail = entity;
                    affectedEntities.Add(selectedDetail);
                    for (var i = 1; i < previous.Count; i++)
                    {
                        var list = ObjectExtensions.GetPropertyValue(selectedDetail, previous[i].Name);
                        if (previous[i] != previous.LastOrDefault())
                        {
                            selectedDetail = (list as IEnumerable<Entity>).AsQueryable().SingleOrDefault(x => x.Id == previous[i].RowId);
                            affectedEntities.Add(selectedDetail);
                        }
                        else
                        {
                            var deletedType = previous.Last().TypeName.ToType();
                            var deletedEntity = (list as IEnumerable<Entity>).AsQueryable().SingleOrDefault(x => x.Id == (int)data["Id"]);
                            viewMode.BeforeDelete(requestInformation, (Entity)deletedEntity, customInformation);
                            if (!viewMode.PreventDefault)
                            {
                                ObjectExtensions.CallMethod(list, "Remove", new Type[] { deletedType }, new object[] { deletedEntity });
                                //((IAggregateRoot)entity).Save();
                                var result = new List<IAggregateRoot>();
                                result.Add((IAggregateRoot)entity);
                                ServiceFactory.ORMService.SaveTransaction(result, UserExtensions.CurrentUser,
                                    deletedEntity, Souccar.Domain.Audit.OperationType.Delete,
                                    Notification, startTimeOfProcess, affectedEntities);
                            }
                            viewMode.AfterDelete(requestInformation, (Entity)deletedEntity, customInformation);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { Data = data, Errors = new { Exception = GlobalResource.ExceptionMessage } });
                //return Json(new { Data = data, Errors = new { Exception = e.Message } });
            }
            return Json(new { Data = data });
        }
        /// <summary>
        /// Update: Yaseen Alrefaee
        /// Date: 14/09/2013
        /// Description: add enum loop
        /// </summary>
        /// <param name="data"></param>
        /// <param name="requestInformation"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Destroy(IDictionary<string, object> data = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null, string customInformation = null)
        {
            var viewMode = getViewModel(viewModelTypeFullName);
            var previous = requestInformation.NavigationInfo.Previous;
            var entity = previous[0].TypeName.ToType().GetById(previous.Count > 1 ? previous[0].RowId : (int)data["Id"]);
            try
            {
                if (previous.Count == 1)
                {
                    viewMode.BeforeDelete(requestInformation, (Entity)entity, customInformation);
                    if (!viewMode.PreventDefault)
                        ((IAggregateRoot)entity).Delete();
                    viewMode.AfterDelete(requestInformation, (Entity)entity, customInformation);
                }
                else
                {
                    var selectedDetail = entity;
                    for (var i = 1; i < previous.Count; i++)
                    {
                        var list = ObjectExtensions.GetPropertyValue(selectedDetail, previous[i].Name);
                        if (previous[i] != previous.LastOrDefault())
                            selectedDetail = (list as IEnumerable<Entity>).AsQueryable().SingleOrDefault(x => x.Id == previous[i].RowId);
                        else
                        {
                            var deletedType = previous.Last().TypeName.ToType();
                            var deletedEntity = (list as IEnumerable<Entity>).AsQueryable().SingleOrDefault(x => x.Id == (int)data["Id"]);
                            viewMode.BeforeDelete(requestInformation, (Entity)deletedEntity, customInformation);
                            if (!viewMode.PreventDefault)
                            {
                                ObjectExtensions.CallMethod(list, "Remove", new Type[] { deletedType }, new object[] { deletedEntity });
                                ((IAggregateRoot)entity).Save();
                            }
                            viewMode.AfterDelete(requestInformation, (Entity)deletedEntity, customInformation);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { Data = data, Errors = new { Exception = GlobalResource.ExceptionMessage } });
                //return Json(new { Data = data, Errors = new { Exception = e.Message } });
            }
            return Json(new { Data = data });
        }

        [HttpPost]
        public ActionResult UpdateRequestInformation(RequestInformation requestInformation)
        {
            initializeNavigation(requestInformation);
            if (requestInformation.NavigationInfo.Module != null &&
                requestInformation.NavigationInfo.Module.Name != null &&
                requestInformation.NavigationInfo.Previous.Count > 1)
            {
                var Details = BuildNavigation.GetModule(requestInformation.NavigationInfo.Module.Name).GetAggregate(requestInformation.NavigationInfo.Previous[0].Name).GetDetail(requestInformation.NavigationInfo.Previous[1].Name);
                for (int i = 1; i < requestInformation.NavigationInfo.Previous.Count; i++)
                {
                    requestInformation.NavigationInfo.Previous[i].Title = Details.Title;
                    Details = Details.GetDetail(requestInformation.NavigationInfo.Previous[i].Name);
                }
            }
            return Json(new { requestInfo = requestInformation }, JsonRequestBehavior.AllowGet);
        }





        public ActionResult ValidateMasterDetail(IDictionary<string, object> data = null, IDictionary<string, object> orginaldata = null, IList<DetailData> Details = null, IDictionary<string, object> parent = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null, string TypeFullName = null, string customInformation = null, string notificationString = null, List<int> recievers = null)
        {
            //entity
            var entityType = TypeFullName.ToType();
            var entity = (Entity)Activator.CreateInstance(entityType);
            setValue(data, entity);
            string[] tokens = TypeFullName.Split('.');
            var orginalModelName = tokens[2];
            var type = tokens[4];
            var modelAdjustment = FactoryModelAdjustment.Create(orginalModelName);
            var viewMode = modelAdjustment.AdjustGridModel(type);
            var previous = requestInformation.NavigationInfo.Previous;
            //parent 
            var parentcreatedType = previous.Last().TypeName.ToType();
            var parentcreatedEntity = entity;
            parentcreatedEntity = (Entity)Activator.CreateInstance(parentcreatedType);
            setValue(parent, parentcreatedEntity);
            if ((int)parent["Id"] != 0)
            {
                updataDetailsForUpdateOperation(parentcreatedEntity, Details);
            }
            updataDetails(parentcreatedEntity, Details);


            if (orginaldata.Count > 0)
                entity.Id = (int)orginaldata["Id"];

            var errorResult = getValidationResult(entity, orginaldata, requestInformation, viewMode, CrudOperationType.Insert, customInformation, Details, parentcreatedEntity);



            if (errorResult != null)
                return Json(errorResult);
            else
            {

                if (orginaldata.Count == 0)
                {
                    foreach (var detail in Details)
                    {
                        if (detail.TypeFullNameViewModel == viewModelTypeFullName)
                        {
                            var otherId = 0;
                            if (detail.RemovedObjects.Count > 0)
                                otherId = detail.RemovedObjects.Max(x => x.Id);
                            entity.Id = detail.List.Count > 0 ? Math.Max((detail.List.Max(x => x.Id)), otherId) + 1 : 1;
                        }
                    }
                }
                return Json(new { Data = entityType.ToDynamicData((Entity)entity) });



            }
        }

        [HttpPost]
        public ActionResult GetTitleOfBeforeCreatePopup(RequestInformation requestInformation = null)
        {
            return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { Title = GlobalResource.Information });
        }

        #endregion Actions






        #region Helper Methods

        private static void updataDetailsForUpdateOperation(Entity entity, IList<DetailData> details = null)
        {

            if (details == null || details.Count == 0)
                return;
            foreach (var item in details)
            {
                var entityType = entity.GetType();
                var prop = entityType.GetProperty(item.DetailName).GetValue(entity);
                var propType = prop.GetType().GetGenericArguments().FirstOrDefault();

                restNewDetailsParent(entity, item.UpdatedObjects);
                foreach (var obj in item.List)
                {

                    if (item.InsertedObjects.Where(x => x.Id == obj.Id).ToList().Count == 0 && obj.Id != 0)
                    {
                        var createdEntity = (Entity)Activator.CreateInstance(propType);

                        foreach (var i in obj.Properties)
                        {
                            var test = i.GetType().Name;
                        }
                        getRealDetailObj(createdEntity, obj);
                        createdEntity.Id = obj.Id;
                        var method = GetAddMethod(entity, propType);
                        method.Invoke(entity, new[] { createdEntity });
                    }
                }

                foreach (var obj in item.RemovedObjects)
                {

                    if (item.List.Where(x => x.Id == obj.Id).ToList().Count == 0 && obj.Id != 0)
                    {
                        var createdEntity = (Entity)Activator.CreateInstance(propType);

                        foreach (var i in obj.Properties)
                        {
                            var test = i.GetType().Name;
                        }
                        getRealDetailObj(createdEntity, obj);
                        createdEntity.Id = obj.Id;
                        var method = GetAddMethod(entity, propType);
                        method.Invoke(entity, new[] { createdEntity });
                    }
                }



            }
        }

        private static void restNewDetailsParent(Entity entity, IList<DetailObj> obj)
        {

            foreach (var item in obj)
            {
                item.Properties.Where(x => x.PropName == entity.GetType().Name).FirstOrDefault().Value = entity.Id;

            }

        }

        private void saveEntityWithNotification(Entity entity, CrudOperationType crudOperationType, DateTime StartTimeOfProcess, string notificationTitle = null, string notificationString = null, List<int> recievers = null, Entity AffectedEntity = null, List<Entity> affectedEntities = null)
        {
            var result = new List<IAggregateRoot>();
            result.Add((IAggregateRoot)entity);
            var title = "";
            switch (crudOperationType)
            {
                case CrudOperationType.Insert:
                    title = string.Format(GlobalResource.AddNotificationTitle, entity.GetType().GetTitle());
                    break;
                case CrudOperationType.Update:
                    title = string.Format(GlobalResource.UpdateNotificationTitle, entity.GetType().GetTitle());
                    break;
                case CrudOperationType.Delete:
                    title = string.Format(GlobalResource.DeleteNotificationTitle, entity.GetType().GetTitle());
                    break;
                default:
                    break;
            }
            var notify = new Notify()
            {
                Body = notificationString,
                Subject = !string.IsNullOrEmpty(notificationTitle) ? notificationTitle : title,
                Type = NotificationType.Information,
                Sender = UserExtensions.CurrentUser
            };
            if (recievers != null && recievers.Count > 0)
            {
                var to = new List<string>();

                result.Add(notify);
                foreach (var userId in recievers)
                {
                    var user = ServiceFactory.ORMService.GetById<Souccar.Domain.Security.User>(userId);
                    notify.AddNotifyReceiver(new NotifyReceiver() { Receiver = user, Date = DateTime.Now, });
                    to.Add(user.Email);
                }

                EmailHelper.SendMail(notify.Subject, notify.Body, to);

            }

            string Notification = null;
            ServiceFactory.ORMService.SaveTransaction(result, UserExtensions.CurrentUser,
                AffectedEntity == null ? entity : AffectedEntity, crudOperationType == CrudOperationType.Insert ?
                Souccar.Domain.Audit.OperationType.Insert : Souccar.Domain.Audit.OperationType.Update, Notification,
                StartTimeOfProcess, affectedEntities);
        }
        private static MethodInfo GetAddMethod(Entity selectedDetail, Type createdType)
        {
            var method = selectedDetail.GetType().GetMethods().Where(x => x.Name.Contains("Add")
                                                                          &&
                                                                          x.GetParameters()
                                                                              .Where(
                                                                                  y =>
                                                                                      y
                                                                                          .ParameterType
                                                                                          .Name ==
                                                                                      createdType
                                                                                          .Name)
                                                                              .Count() > 0)
                .SingleOrDefault();
            return method;
        }

        public static void UpdateFilter(GridFilter filter, Type type)
        {
            AddDeleteVertualDeletedFilter(filter, type);
            RecursiveUpdateFilter(filter, type);

        }

        public static void RecursiveUpdateFilter(GridFilter filter, Type type)
        {
            if (filter == null) return;
            if (filter.Field != null)
            {
                var propType = type.GetProperty(filter.Field).PropertyType;
                if (propType == typeof(DateTime) || propType == typeof(DateTime?))
                    filter.Value = DateTime.Parse(filter.Value.ToString());
                else if (propType.IsEnum)
                {
                    filter.Value = Enum.ToObject(propType, Convert.ToInt32(filter.Value));
                }
                else
                    if (propType.IsIndex())
                {
                    filter.Value = propType.GetById(Convert.ToInt32(filter.Value));
                }

                else if (propType.IsSubclassOf(typeof(Entity)) && !propType.IsIndex())
                {
                    int Number = 0;
                    bool result = int.TryParse(Convert.ToString(filter.Value), out Number);
                    if (result)
                    {

                        filter.Value = propType.GetById(Convert.ToInt32(filter.Value));

                    }
                    else
                    {
                        filter.Value = null;
                    }
                }







            }
            if (filter.Filters == null) return;
            foreach (var gridFilter in filter.Filters)
            {
                RecursiveUpdateFilter(gridFilter, type);
            }
        }



        private static void AddDeleteVertualDeletedFilter(GridFilter filter, Type type)
        {
            if (filter == null)
            {
                filter = new GridFilter();
                filter.Logic = "and";
            }
            if (filter.Filters == null)
            {
                filter.Filters = new List<GridFilter>().AsEnumerable();
                filter.Logic = "and";
            }
            var temp = filter.Filters.ToList();
            temp.Add(new GridFilter()
            {
                Field = "IsVertualDeleted",
                Operator = "eq",
                Value = false
            });
            filter.Filters = temp.AsEnumerable();
        }

        private object getValidationResult(Entity entity, IDictionary<string, object> originalState, RequestInformation requestInformation, ViewModel viewMode, CrudOperationType operationType, string customInformation, IList<DetailData> details = null, Entity parent = null)
        {
            viewMode.BeforeValidation(requestInformation, entity, originalState, operationType, customInformation);
            var specificationType = SpecificationHelper.GetSpecificationType(requestInformation, entity);
            var validationResults = ServiceFactory.ValidationService.Validate((IEntity)entity, specificationType);

            viewMode.AfterValidation(requestInformation, entity, originalState, validationResults, operationType, customInformation, parent, details);

            viewMode.AfterValidation(requestInformation, entity, originalState, validationResults, operationType, customInformation, parent);

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
        private void resetNewDetailsIdsForAll(Entity entity, IList<DetailData> details)
        {
            if (details == null || details.Count == 0)
                return;
            foreach (var item in details)
            {
                var entityType = entity.GetType();
                var list = ObjectExtensions.GetPropertyValue(entity, item.DetailName);
                var objects = (list as IEnumerable<Entity>);
                resetNewDetailsIds(objects, item.InsertedObjectsIds);

                var listType = list.GetType().GetGenericArguments().FirstOrDefault();

            }
        }
        private void resetNewDetailsIds(IEnumerable<Entity> objects, IList<int> ids)
        {
            objects.Where(x => ids.Contains(x.Id)).ToList().ForEach(x => x.Id = 0);
        }
        private static void setValue(IDictionary<string, object> data, Entity selectedDetail)
        {
            var classTree = ClassTreeFactory.Create(selectedDetail.GetType());

            foreach (var prop in classTree.SimpleProperties.Where(x => !x.IsPrimaryKey))
            {
                if (data.Keys.All(x => x != prop.Name) || (data[prop.Name] == null && prop.PropertyType != typeof(DateTime)) || (!isStringProp(prop) && data[prop.Name] == string.Empty))
                    continue;
                if (data[prop.Name] == null && prop.PropertyType == typeof(DateTime) && ObjectExtensions.GetPropertyValue(selectedDetail, prop.Name) != null)
                    continue;
                ObjectExtensions.SetPropertyValue(selectedDetail, prop.Name, data[prop.Name].To(prop.PropertyType));
            }

            foreach (var prop in classTree.ReferencesProperties.Where(p => p.PropertyType == typeof(DateTime?)).ToList())
            {
                if (data.Keys.All(x => x != prop.Name))
                    continue;
                //if (data[prop.Name] == null && ObjectExtensions.GetPropertyValue(selectedDetail, prop.Name) != null)
                //    continue;
                ObjectExtensions.SetPropertyValue(selectedDetail, prop.Name, data[prop.Name].To(prop.PropertyType));
            }

            foreach (var prop in classTree.ReferencesProperties.Where(p => p.PropertyType.IsIndex()).ToList())
            {
                if (data.Keys.All(x => x != prop.Name) || data[prop.Name] == null || data[prop.Name] == string.Empty)
                    continue;
                var value = prop.PropertyType.GetById(Convert.ToInt32(data[prop.Name]));
                ObjectExtensions.SetPropertyValue(selectedDetail, prop.Name, value);
            }


            foreach (var prop in classTree.ReferencesProperties.Where(p => p.PropertyType.IsEnum()).ToList())
            {
                if (data.Keys.All(x => x != prop.Name) || data[prop.Name] == null || data[prop.Name] == string.Empty)
                    continue;
                ObjectExtensions.SetPropertyValue(selectedDetail, prop.Name, Enum.ToObject(prop.PropertyType, Convert.ToInt32(data[prop.Name])));
            }

            foreach (
                var prop in classTree.ReferencesProperties
                .Where(p => p.PropertyType.IsSubclassOf(typeof(Entity)) && !p.PropertyType.IsIndex()).ToList())
            {
                if (data.Keys.All(x => x != prop.Name) || data[prop.Name] == null || data[prop.Name] == string.Empty)
                    continue;
                ObjectExtensions.SetPropertyValue(selectedDetail, prop.Name, prop.PropertyType.GetById(Convert.ToInt32(data[prop.Name])));
            }
        }
        public static void updataDetails(Entity entity, IList<DetailData> details = null)
        {

            if (details == null || details.Count == 0)
                return;
            foreach (var item in details)
            {
                var entityType = entity.GetType();
                var prop = entityType.GetProperty(item.DetailName).GetValue(entity);
                var propType = prop.GetType().GetGenericArguments().FirstOrDefault();

                restNewDetailsParent(entity, item.UpdatedObjects);
                foreach (var obj in item.InsertedObjects)
                {
                    if (!cheackObjIsExistinUpdatedObjects(obj, item.UpdatedObjects))
                    {
                        var createdEntity = (Entity)Activator.CreateInstance(propType);
                        getRealDetailObj(createdEntity, obj);
                        createdEntity.Id = obj.Id;
                        var method = GetAddMethod(entity, propType);
                        method.Invoke(entity, new[] { createdEntity });
                    }

                }
                foreach (var obj in item.UpdatedObjects)
                {
                    if (entity.Id != 0 && item.OldObjects.Any(x => x.Id == obj.Id))
                    {
                        var list = ObjectExtensions.GetPropertyValue(entity, item.DetailName);
                        var updatedEntity = (list as IEnumerable<Entity>).AsQueryable().SingleOrDefault(x => x.Id == obj.Id);
                        getRealDetailObj(updatedEntity, obj);
                    }
                    else
                    {
                        var updatedEntity = (Entity)Activator.CreateInstance(propType);
                        getRealDetailObj(updatedEntity, obj);
                        var method = GetAddMethod(entity, propType);
                        method.Invoke(entity, new[] { updatedEntity });
                    }


                }
                foreach (var obj in item.RemovedObjects)
                {
                    var list = ObjectExtensions.GetPropertyValue(entity, item.DetailName);
                    var removedEntity = (list as IEnumerable<Entity>).AsQueryable().SingleOrDefault(x => x.Id == obj.Id);
                    var removedEntityType = removedEntity.GetType();
                    ObjectExtensions.CallMethod(list, "Remove", new Type[] { removedEntityType }, new object[] { removedEntity });
                }

            }
        }

        private static bool cheackObjIsExistinUpdatedObjects(DetailObj obj, IList<DetailObj> updatedObjects)
        {
            return updatedObjects.Any(x => x.Id == obj.Id);
        }

        private static void getRealDetailObj(Entity selectedDetail, DetailObj detailObj)
        {
            var classTree = ClassTreeFactory.Create(selectedDetail.GetType());

            foreach (var prop in classTree.SimpleProperties.Where(x => !x.IsPrimaryKey))
            {
                var virtualProp = detailObj.Properties.SingleOrDefault(x => x.PropName == prop.Name);
                if (virtualProp == null || virtualProp.Value == null || (!isStringProp(prop) && virtualProp.Value == string.Empty))
                    continue;
                ObjectExtensions.SetPropertyValue(selectedDetail, prop.Name, virtualProp.Value.To(prop.PropertyType));
            }

            foreach (var prop in classTree.ReferencesProperties.Where(p => p.PropertyType == typeof(DateTime?)).ToList())
            {
                var virtualProp = detailObj.Properties.SingleOrDefault(x => x.PropName == prop.Name);
                if (virtualProp == null || virtualProp.Value == null || (!isStringProp(prop) && virtualProp.Value == string.Empty))
                    continue;
                ObjectExtensions.SetPropertyValue(selectedDetail, prop.Name, virtualProp.Value.To(prop.PropertyType));
            }

            foreach (var prop in classTree.ReferencesProperties.Where(p => p.PropertyType.IsIndex()).ToList())
            {
                var virtualProp = detailObj.Properties.SingleOrDefault(x => x.PropName == prop.Name);
                if (virtualProp == null || virtualProp.Value == null || virtualProp.Value == string.Empty)
                    continue;
                var value = prop.PropertyType.GetById(Convert.ToInt32(virtualProp.Value));
                ObjectExtensions.SetPropertyValue(selectedDetail, prop.Name, value);
            }


            foreach (var prop in classTree.ReferencesProperties.Where(p => p.PropertyType.IsEnum()).ToList())
            {
                var virtualProp = detailObj.Properties.SingleOrDefault(x => x.PropName == prop.Name);
                if (virtualProp == null || virtualProp.Value == null || virtualProp.Value == string.Empty)
                    continue;
                ObjectExtensions.SetPropertyValue(selectedDetail, prop.Name, Enum.ToObject(prop.PropertyType, Convert.ToInt32(virtualProp.Value)));
            }

            foreach (
              var prop in classTree.ReferencesProperties
              .Where(p => p.PropertyType.IsSubclassOf(typeof(Entity)) && !p.PropertyType.IsIndex()).ToList())
            {
                var virtualProp = detailObj.Properties.SingleOrDefault(x => x.PropName == prop.Name);
                if (virtualProp == null || virtualProp.Value == null || virtualProp.Value == string.Empty)
                    continue;

                ObjectExtensions.SetPropertyValue(selectedDetail, prop.Name, prop.PropertyType.GetById(Convert.ToInt32(virtualProp.Value)));
            }
        }

        private static bool isStringProp(SimpleProperty prop)
        {
            return prop.PropertyType == typeof(string);
        }
        private static bool isStringProp(ReferenceProperty prop)
        {
            return prop.PropertyType == typeof(string);
        }



        private ViewModel getViewModel(string type)
        {
            return string.IsNullOrEmpty(type) ? new ViewModel() : (ViewModel)Activator.CreateInstance(type.ToType());
        }


        private List<Detail> getDetails(RequestInformation requestInformation)
        {
            var previous = requestInformation.NavigationInfo.Previous;

            if (requestInformation.NavigationInfo.Status == "Aggregate")
            {
                var aggregate = BuildNavigation.GetModule(requestInformation.NavigationInfo.Module.Name).GetAggregate(previous[0].Name);
                if (previous.Count == 1)
                    return aggregate.Details.ToList();
                var details = aggregate.GetDetail(previous[1].Name);
                for (var i = 2; i < previous.Count; i++)
                    details = details.GetDetail(previous[i].Name);
                return details.Details.ToList();
            }
            else
            {
                var configuration = BuildNavigation.GetModule(requestInformation.NavigationInfo.Module.Name).GetConfiguration(previous[0].Name);
                if (previous.Count == 1)
                    return configuration.Details.ToList();
                var details = configuration.GetDetail(previous[1].Name);
                for (var i = 2; i < previous.Count; i++)
                    details = details.GetDetail(previous[i].Name);
                return details.Details.ToList();
            }

        }

        private List<Detail> getAuthorizedDetails(List<Detail> allDetails)
        {
            return allDetails.Where(x => x.IsAuthorized).ToList();
        }

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

        private void initializeNavigation(RequestInformation requestInformation)
        {
            if (requestInformation.NavigationInfo.Module.Name != null)
            {
                requestInformation.NavigationInfo.Module.ImageClass = BuildNavigation.GetModule(requestInformation.NavigationInfo.Module.Name).SmallImageClass;
                requestInformation.NavigationInfo.Module.Title = BuildNavigation.GetModule(requestInformation.NavigationInfo.Module.Name).Title;
            }
            if (requestInformation.NavigationInfo.Status == RequestInformation.Navigation.NavigationStatus.Aggregate.ToString())
                requestInformation.NavigationInfo.Previous[0].Title = BuildNavigation.GetModule(requestInformation.NavigationInfo.Module.Name).GetAggregate(requestInformation.NavigationInfo.Previous[0].Name).Title;
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

        #endregion Helper Methods



        /// <summary>
        /// Writer: Duaa Ahmed
        /// Date: 14/12/2015
        /// Description: Validate Master Details
        /// </summary>

        // public ActionResult ValidateMasterDetail(RequestInformation requestInformation, string viewModelTypeFullName = null)
        //[HttpPost]
        //public ActionResult ValidateMasterDetail(IDictionary<string, object> data = null, string detailName = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null)
        //{
        //    var viewMode = getViewModel(viewModelTypeFullName);

        //    var previous = requestInformation.NavigationInfo.Previous;
        //    //var createdType = previous.Last().TypeName.ToType();
        //    //var createdEntity = (Entity)Activator.CreateInstance(createdType);

        //    var createdType = detailName.ToType();
        //    var createdEntity = (Entity)Activator.CreateInstance(createdType);
        //     setValue(data, createdEntity);
        //    var errorResult = getValidationResult(createdEntity, null, requestInformation, viewMode, CrudOperationType.Insert, null);
        //    if (errorResult != null)
        //        return Json(errorResult);
        //    return null;

        //}
    }






}
