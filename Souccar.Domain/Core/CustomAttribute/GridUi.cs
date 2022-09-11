#region

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

#endregion

namespace Souccar.Core.CustomAttribute
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GridUi : Attribute
    {
        public GridUi()
        {

            IsDetailOutSideGrid = true;
        }

        public bool IsDetailOutSideGrid { get; set; }

    }
}