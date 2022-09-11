using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Helpers.Resource
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class NotificationLocalizationHelper
    {
        public const string ResourceGroupName = "Notification";
        public const string Notification = "Notification";
        public const string ShowAllNotification = "ShowAllNotification";
        public const string Subject = "Subject";
        public const string Date = "Date";
        public const string Time = "Time";
        public const string ItemPerPage = "ItemPerPage";
        public const string Display = "Display";
        public const string Of = "Of";
        public const string Empty = "Empty";
        public const string First = "First";
        public const string Page = "Page";
        public const string Previous = "Previous";
        public const string Next = "Next";
        public const string Last = "Last";
        public const string Refresh = "Refresh";
        public const string YouHaveDoneThisOperation = "YouHaveDoneThisOperation";
        public const string DeleteAll = "DeleteAll";
        public const string AreYouSureToDeleteAllNotifications = "AreYouSureToDeleteAllNotifications";
        
        public const string AllNotifications = "AllNotifications";

        public static string GetResource(string key)
        {
            var result = ServiceFactory.LocalizationService.GetResource(ResourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }
    }
}