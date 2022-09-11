#region

using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

#endregion

namespace UI.Helpers.Route
{
    public static class RouteHelper
    {
        private const string ReplaceFormatString = "REPLACE{0}";

        public static string GetUrl(RequestContext requestContext, RouteValueDictionary routeValueDictionary)
        {
            var urlData = new RouteValueDictionary();
            var urlHelper = new UrlHelper(requestContext);

            int i = 0;
            foreach (KeyValuePair<string, object> item in routeValueDictionary)
            {
                if (string.IsNullOrEmpty(item.Value.ToString()))
                {
                    i++;
                    urlData.Add(item.Key, string.Format(ReplaceFormatString, i));
                }
                else
                {
                    urlData.Add(item.Key, item.Value);
                }
            }

            string url = urlHelper.RouteUrl(urlData);

            for (int index = 1; index <= i; index++)
            {
                url = url.Replace(string.Format(ReplaceFormatString, index), string.Empty);
            }

            return url;
        }
    }
}