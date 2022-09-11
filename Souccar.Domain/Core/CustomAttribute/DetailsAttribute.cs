#region

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

#endregion

namespace Souccar.Core.CustomAttribute
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DetailsAttribute : Attribute
    {
        public DetailsAttribute()
        {
           
            IsDetailHidden = true;
        }
      
        public bool IsDetailHidden { get; set; }

    }
}