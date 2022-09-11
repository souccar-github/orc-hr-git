using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Core.CustomAttribute;

namespace Souccar.Core.Extensions
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public static class TypeExtensions
    {
        public static int GetOrder(this Type type)
        {
            var attrs = type.GetCustomAttributes(typeof(OrderAttribute), false);
            return attrs.Count() != 0 ? ((OrderAttribute)attrs.First()).Order : 0;
        }
                public static bool GetIsDetailHidden(this Type type)
        {
            var attrs = type.GetCustomAttributes(typeof(DetailsAttribute), false);
            return attrs.Count() != 0 ? ((DetailsAttribute)attrs.First()).IsDetailHidden : true;
        }
                public static bool GetIsDetailOutSideGrid(this Type type)
        {
            var attrs = type.GetCustomAttributes(typeof(GridUi), false);
            return attrs.Count() != 0 ? ((GridUi)attrs.First()).IsDetailOutSideGrid : true;
        }


    }
}
