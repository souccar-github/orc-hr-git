#region

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

#endregion

//test merge and branch 

namespace Souccar.Core.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DetailAttribute : Attribute
    {
        public DetailAttribute()
        {
            GroupOrder = 1;
            GroupName = "General";
            Exclude = false;
        }
        public string GroupName { get; set; }
        public bool Exclude { get; set; }
        public int GroupOrder { get; set; }

    }
}