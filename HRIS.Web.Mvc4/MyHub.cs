
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using  Project.Web.Mvc4.Services;
using WebMatrix.WebData;


namespace Project.Web.Mvc4
{
    [HubName("myhub")]
    public class MyHub : Hub
    {


        public int ChkNotfyChang()
        {

            try
            {
                var user = UserExtensions.CurrentUser;
                if (user != null)
                {


                    var userId = WebSecurity.GetUserId(user.Username)
                    ;

                    var count = NotificationService.Instance.GetUnreadNotifications((int)userId).Count;

                    return count;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public List<Dictionary<string, string>> GetNotifis()
        {
            var user = UserExtensions.CurrentUser;
            List<Dictionary<string, string>> listdic = new List<Dictionary<string, string>>();
            if (user != null)
            {


                var userId = WebSecurity.GetUserId(user.Username);
                var notfi = NotificationService.Instance.GetNotifications(userId);
                var isReadNotify = "";
                if (notfi.Count != 0)
                {
                    foreach (var notify in notfi)
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("Id", notify.Id.ToString());
                        dic.Add("Type", notify.Type.ToString());
                        dic.Add("Subject", notify.Subject);
                        if (notify.Receivers.Any(x => x.IsRead == false))
                        {
                            isReadNotify = "not-readed-notify";
                        }
                        else
                        {
                            isReadNotify = "";
                        }
                        dic.Add("IsReaded", isReadNotify);
                        listdic.Add(dic);
                    }
                }
                return listdic;
            }
            else
            {
                return listdic;
            }
        }

    }
}