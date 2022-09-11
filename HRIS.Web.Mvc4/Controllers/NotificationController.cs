using System.Web.UI.WebControls;
using Castle.Components.DictionaryAdapter;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using Souccar.Domain.DomainModel;
using Souccar.Domain.PersistenceSupport;
using Souccar.Infrastructure.Core;
using Souccar.NHibernate;
using Souccar.Domain.Notification;
using WebMatrix.WebData;
using Souccar.Infrastructure.Extenstions;
using Project.Web.Mvc4.Factories;
using Souccar.Domain.Extensions;
using Project.Web.Mvc4.Models.GridModel;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Domain.Localization;

namespace Project.Web.Mvc4.Controllers
{

    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class NotificationController : Controller
    {
        [HttpPost]
        public ActionResult GetGridDataNotifications()
        {

            var gridModel = GridViewModelFactory.Create(typeof(Notify), null);

            gridModel.Views[0].ReadUrl = "Notification/ReadNotifications";
            gridModel.ToolbarCommands.Add(
                new ToolbarCommand
                {
                    Additional = false,
                    Name = "deleteAllNotifications",
                    ClassName = "grid-action-button deleteAllNotifications",
                    Handler = "",
                    ImageClass = "",
                    Text = NotificationLocalizationHelper.GetResource(NotificationLocalizationHelper.DeleteAll)
                }
            );
            foreach (var column in gridModel.Views[0].Columns)
            {
                if (column.FieldName != "Subject" && column.FieldName != "Date" && column.FieldName != "Time")
                    gridModel.Views[0].Columns.FirstOrDefault(x => x.FieldName == column.FieldName).Hidden = true;
            }
            return Json(new { GridModel = gridModel, IsExcption = false },
                JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ReadNotifications(int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null,
            GridFilter filter = null, IEnumerable<GridGroup> group = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null, string additionalInfo = null)
        {
            IQueryable<IEntity> queryable = ServiceFactory.ORMService.AllWithVertualDeleted<Notify>().Where(x => x.Receivers.Any(y => y.Receiver.Id == WebSecurity.CurrentUserId && !y.IsDeleted)).OrderByDescending(x=>x.Date).ThenByDescending(x=>x.Time);
            return ReadTypeData(typeof(Notify), queryable, pageSize, skip, serverPaging, sort, filter);
        }
        public ActionResult ReadTypeData(Type type, IQueryable<IEntity> allData, int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null)
        {
            if (filter == null)
            {
                filter = new GridFilter();
                filter.Logic = "and";
            }
            CrudController.UpdateFilter(filter, type);
            var dataSourse = new DataSourceResult();
            var filterQuery = DataSourceResult.Filter(allData, filter);
            dataSourse.Total = filterQuery.Count();
            var sortQuery = DataSourceResult.Sort(filterQuery, sort);
            dataSourse.Data = DataSourceResult.Page(sortQuery, pageSize, skip);
            var result = type.ToDynamicData(dataSourse.Data);

            return Json(new { Data = result, TotalCount = dataSourse.Total });
        }
        public ActionResult GetAll()
        {
            var notifications = NotificationService.Instance.GetNotifications(1);
            var result = new List<Dictionary<string, object>>();
            foreach (var notify in notifications)
            {
                var temp = new Dictionary<string, object>();
                temp["Subject"] = notify.Subject;
                temp["bBody"] = notify.Body;
                temp["Date"] = notify.Date;
                temp["SenderId"] = notify.Sender.Id;
                result.Add(temp);
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult ReadNotification(int itemId)
        {
            var item = (Notify)typeof(Notify).GetById(itemId);
            if (item != null)
            {
                item.Receivers.SingleOrDefault(x => x.Receiver.Id == WebSecurity.CurrentUserId).IsRead = true;
                item.Save();
            }
            var count =
                typeof(Notify).GetAll<Notify>()
                    .Count(
                        x =>
                            x.Receivers.Any(y => y.Receiver.Id == WebSecurity.CurrentUserId && !y.IsDeleted && !y.IsRead));

            return Json(new
            {
                Id = item.Id,
                Date = item.Date,
                Time = item.Time,
                Subject = item.Subject,
                Body = item.Body,
                DestinationTabName = item.DestinationTabName == null ? string.Empty : item.DestinationTabName,
                DestinationModuleName = item.DestinationModuleName == null ? string.Empty : item.DestinationModuleName,
                DestinationLocalizationModuleName = item.DestinationLocalizationModuleName == null ? string.Empty : item.DestinationLocalizationModuleName,
                DestinationLowerModuleName = item.DestinationModuleName == null ? string.Empty : item.DestinationModuleName.ToLower(),
                DestinationActionName = item.DestinationActionName == null ? string.Empty : item.DestinationActionName,
                DestinationControllerName = item.DestinationControllerName == null ? string.Empty : item.DestinationControllerName,
                DestinationData = item.DestinationData,
                DestinationEntityId = item.DestinationEntityId == null ? string.Empty : item.DestinationEntityId,
                DestinationEntityTypeFullName = item.DestinationEntityTypeFullName == null ? string.Empty : item.DestinationEntityTypeFullName,
                DestinationEntityTitle = item.DestinationEntityTitle == null ? string.Empty : item.DestinationEntityTitle,
                DestinationEntityOperationType = item.DestinationEntityOperationType != null ? item.DestinationEntityOperationType.ToString() : string.Empty,
                TotalCount = count,
                Type = new { Name = item.Type.ToString(), Id = (int)item.Type }
            });
        }

        [HttpPost]
        public ActionResult MarkAsRead(int itemId)
        {
            var item = (Notify)typeof(Notify).GetById(itemId);
            if (item != null)
            {
                item.Receivers.SingleOrDefault(x => x.Receiver.Id == WebSecurity.CurrentUserId).IsRead = true;
                item.Save();
            }
            var count = typeof(Notify).GetAll<Notify>().Count(x => x.Receivers.Any(y => y.Receiver.Id == WebSecurity.CurrentUserId && !y.IsDeleted && !y.IsRead));
            return Content(count.ToString());
        }

        [HttpPost]
        public ActionResult MarkAsDeleted(int itemId = 0)
        {
            var item = (Notify)typeof(Notify).GetById(itemId);
            if (item != null)
            {
                item.Receivers.SingleOrDefault(x => x.Receiver.Id == WebSecurity.CurrentUserId).IsDeleted = true;
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { item }, UserExtensions.CurrentUser, item, Souccar.Domain.Audit.OperationType.Delete);

            }
            return Json(true);
        }

        public ActionResult getLangDir()
        {
            //false means the dir LTR
            var result = false;
            if (Session["LangDir"] == null)
            {
                result = ServiceFactory.ORMService.All<Language>().Where(x => x.IsActive).FirstOrDefault().Rtl;
                Session["LangDir"] = result.ToString();
            }
            else
            {
                result =bool.Parse(Session["LangDir"].ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MarkAllAsDeleted()
        {
            var Entities = new List<IAggregateRoot>();
            var item = typeof(Notify).GetAll<Notify>().Where(x =>
                x.Receivers.Any(y => y.Receiver.Id == WebSecurity.CurrentUserId && !y.IsDeleted)).ToList();
            foreach (var notify in item)
            {
                notify.Receivers.SingleOrDefault(x => x.Receiver.Id == WebSecurity.CurrentUserId).IsDeleted = true;
                Entities.Add(notify);
            }
            //ServiceFactory.ORMService.SaveTransaction(Entities, UserExtensions.CurrentUser);
            ServiceFactory.ORMService.SaveTransaction(Entities, UserExtensions.CurrentUser, null, Souccar.Domain.Audit.OperationType.Delete, NotificationLocalizationHelper.GetResource(NotificationLocalizationHelper.AllNotifications));

            return Json(true);
        }

        [HttpPost]
        public ActionResult getReadNotifications(int[] Ids)
        {
            var result = new List<int>();
            var receivers = ServiceFactory.ORMService.All<NotifyReceiver>().Where(x => x.Receiver.Id == WebSecurity.CurrentUserId && !x.IsDeleted && !x.IsRead).ToList();
            for (int i=0; i < Ids.Length; i++)
            {
                var NotifyReceiver = receivers.Where(x => x.Notify.Id == Ids[i]).FirstOrDefault();
                if (NotifyReceiver != null && NotifyReceiver.Id != 0)
                    result.Add(Ids[i]);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
