#region

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

#endregion

namespace Souccar.Core.CustomAttribute
{
    [AttributeUsage(AttributeTargets.All,AllowMultiple = false)]
    public class LocalizationKeyAttribute : Attribute
    {
        private readonly Expression<Func<Type, object>> _expression;

        public LocalizationKeyAttribute(Expression<Func<Type, object>> expression)
        {
            _expression = expression;
        }

       
    }
}