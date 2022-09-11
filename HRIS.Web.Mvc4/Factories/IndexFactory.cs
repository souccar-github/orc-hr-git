using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using  Project.Web.Mvc4.Extensions;
using  Project.Web.Mvc4.Models.Navigation;
using Souccar.Core.Extensions;
using Souccar.Domain.Extensions;

namespace Project.Web.Mvc4.Factories
{
    public class IndexFactory
    {
        
        public static IList<Index> Create(Assembly assembly, string moduleName)
        {
            var indexes = assembly.GetIndexClassesByModule(moduleName);
            return indexes.Select(x => Create(x.Key)).ToList();
        }
        
        private static Index Create(Type type)
        {
            var index = new Index();
            var classTree = Souccar.Reflector.ClassTreeFactory.Create(type);
            index.IndexId = type.FullName;
            index.SecurityId = type.FullName;
            index.Title= classTree.Type.GetLocalized();
            index.Order = classTree.Type.GetOrder();
            return index;
        }
    }
}