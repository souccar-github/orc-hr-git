#region

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Souccar.Core.CustomAttribute
{
    [AttributeUsage(AttributeTargets.All,
 AllowMultiple = false)]
    public class LocalizationDisplayNameAttribute : DisplayNameAttribute
    {
        private readonly DisplayAttribute _displayAttribute;

        public LocalizationDisplayNameAttribute(string resourceName, Type resourceType)
        {
            _displayAttribute = new DisplayAttribute
                                    {
                                        ResourceType = resourceType,
                                        Name = resourceName
                                    };
        }

        public override string DisplayName
        {
            get { return _displayAttribute.GetName(); }
        }
    }
}