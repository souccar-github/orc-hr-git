using Newtonsoft.Json;
using Project.Web.Mvc4.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Project.Web.Mvc4.Areas.MobileApp.Helpers
{
    public class PushNotification
    {
        public PushNotification(string title, string body, string deviceId)
        {
            if (!NavigationHelper.FireBaseDisabled)
            {
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                //serverKey - Key from Firebase cloud messaging server  
                tRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAAkrPLbmM:APA91bFh6APiPtrtVgJ8xCBMCHoycxOriQjg_SbMR0DAOXqX0FB1zKhSVmt2VIQ6_arABslUc4cuzSXwa6Nl1gSpJBRD5uAY0mGeEGn_zNr9YkihF8R5TsDowxYI9PGFW9NMbBIbhjlX"));
                //Sender Id - From firebase project setting  
                tRequest.Headers.Add(string.Format("Sender: id={0}", "630081678947"));
                tRequest.ContentType = "application/json";
                var payload = new
                {
                    to = deviceId,
                    priority = "high",
                    content_available = true,
                    notification = new
                    {
                        body = body,
                        title = title,
                        badge = 1
                    },
                    data = new
                    {
                        click_action = "FLUTTER_NOTIFICATION_CLICK"
                    },
                };

                string postbody = JsonConvert.SerializeObject(payload).ToString();
                Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    String sResponseFromServer = tReader.ReadToEnd();
                                    //result.Response = sResponseFromServer;
                                }
                        }
                    }
                }
            }
        }
    }
}