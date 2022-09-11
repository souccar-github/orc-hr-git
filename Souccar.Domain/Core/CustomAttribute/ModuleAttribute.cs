#region

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

#endregion

namespace Souccar.Core.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Interface|AttributeTargets.Enum|AttributeTargets.Struct,AllowMultiple = true)]
    public class ModuleAttribute : Attribute
    {
       
        public ModuleAttribute(string moduleName)
        {
            ModuleName = moduleName;
            Exclude = false;
        }
        public string ModuleName { get; private set; }
        public bool Exclude { get; set; }

    }
}