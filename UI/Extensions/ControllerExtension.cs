#region

using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

#endregion

namespace UI.Extensions
{
    public static class ControllerExtension
    {
        public static void UpdateValueObject<T>(this Controller controller, T source, T destination)
        {
            Type sourceObjectType = source.GetType();
            
            PropertyInfo[] destinationPropertyInfos =
                sourceObjectType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance );

            foreach (PropertyInfo propertyInfo in destinationPropertyInfos)
            {
                if (!propertyInfo.PropertyType.IsGenericType)
                {
                    if (propertyInfo.CanWrite)
                        propertyInfo.SetValue(destination, propertyInfo.GetValue(source, null), null);
                }
            }
        }

        public static void StringDecode<T>(this Controller controller, T source)
        {
            Type myObjectType = source.GetType();

            PropertyInfo[] propertyInfos = myObjectType.GetProperties();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (propertyInfo.PropertyType.Name.ToLower() == "string")
                {
                    var temp = propertyInfo.GetValue(source, null);
                    if (temp != null) propertyInfo.SetValue(source, HttpUtility.HtmlDecode(temp.ToString()), null);
                }
            }
        }

    }
}