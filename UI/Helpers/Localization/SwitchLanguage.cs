#region

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

#endregion

namespace UI.Helpers.Localization
{
    public static class SwitchLanguage
    {
        public static Language LanguageUrl(this HtmlHelper helper, string cultureName,
                                           string languageRouteName = "lang", bool strictSelected = false)
        {
            cultureName = cultureName.ToLower();

            var routeValues = new RouteValueDictionary(helper.ViewContext.RouteData.Values);

            var queryString = helper.ViewContext.HttpContext.Request.QueryString;
            foreach (
                string key in
                    queryString.Cast<string>().Where(key => queryString[key] != null && !string.IsNullOrWhiteSpace(key))
                )
            {
                if (routeValues.ContainsKey(key))
                {
                    routeValues[key] = queryString[key];
                }
                else
                {
                    routeValues.Add(key, queryString[key]);
                }
            }

            var actionName = routeValues["action"].ToString();
            var controllerName = routeValues["controller"].ToString();

            routeValues[languageRouteName] = cultureName;

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);

            var url = urlHelper.RouteUrl(routeValues);

            var currentLangName = Thread.CurrentThread.CurrentUICulture.Name.ToLower();
            var isSelected = strictSelected
                                 ? currentLangName == cultureName
                                 : currentLangName.StartsWith(cultureName);
            return new Language
                       {
                           Url = url,
                           ActionName = actionName,
                           ControllerName = controllerName,
                           RouteValues = routeValues,
                           IsSelected = isSelected
                       };
        }

        public static MvcHtmlString LanguageSelectorLink(this HtmlHelper helper,
                                                         string cultureName, string selectedText, string unselectedText,
                                                         IDictionary<string, object> htmlAttributes,
                                                         string languageRouteName = "lang", bool strictSelected = false)
        {
            var language = helper.LanguageUrl(cultureName, languageRouteName, strictSelected);

            MvcHtmlString link;

            if (language.RouteValues["controller"].ToString() == "Account")
            {
                link = helper.RouteLink(language.IsSelected ? selectedText : unselectedText,
                                        "Localization", language.RouteValues, htmlAttributes);
            }
            else
            {
                link = helper.RouteLink(language.IsSelected ? selectedText : unselectedText, language.RouteValues,
                                        htmlAttributes);
            }


            return link;
        }

        #region Nested type: Language

        public class Language
        {
            public string Url { get; set; }
            public string ActionName { get; set; }
            public string ControllerName { get; set; }
            public RouteValueDictionary RouteValues { get; set; }
            public bool IsSelected { get; set; }

            public MvcHtmlString HtmlSafeUrl
            {
                get { return MvcHtmlString.Create(Url); }
            }
        }

        #endregion
    }
}