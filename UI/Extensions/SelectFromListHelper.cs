#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

#endregion

namespace UI.Extensions
{
    public static class SelectFromListHelper
    {
        public static SelectList SelectFromList<TItem>(this List<TItem> values,
                                                       Expression<Func<TItem, string>> key,
                                                       Expression<Func<TItem, string>> value)
        {
            Func<TItem, string> Key = key.Compile();
            Func<TItem, string> Value = value.Compile();
            var kvp = new List<KeyValuePair<string, string>>(values.Count);
            values.ForEach(item =>
                           kvp.Add(new KeyValuePair<string, string>(Key.Invoke(item), Value.Invoke(item))));

            return new SelectList(kvp, "Key", "Value");
        }
    }
}