#region

using System.Web.Mvc;
using System.Web.Routing;

#endregion

namespace UI.Helpers.Attributes
{
    public class AutoCompleteAttribute : MetaDataAttribute
    {
        public RouteValueDictionary RouteValueDictionary;

        public AutoCompleteAttribute(string controller, string action, string parameterName)
        {
            RouteValueDictionary = new RouteValueDictionary
                                       {
                                           {"Controller", controller},
                                           {"Action", action},
                                           {parameterName, string.Empty}
                                       };
        }

        public override void Process(ModelMetadata modelMetaData)
        {
            modelMetaData.AdditionalValues.Add("AutoCompleteUrlData", RouteValueDictionary);
            modelMetaData.TemplateHint = "AutoComplete";
        }
    }
}