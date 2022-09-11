using System;

namespace HRIS.Domain
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Enum, AllowMultiple = false)]
    public class LocalizeAttribute : Attribute, IResouceSource
    {
        public LocalizeAttribute(bool apply = true)
        {
            Apply = apply;
        }

        public bool Apply { get; set; }
    }
}