using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Souccar.Core.CustomAttribute;

namespace Souccar.Domain.Extensions
{
    public static class AssemblyExtensions
    {
        public static Dictionary<Type, string> GetAggregateClasses(this Assembly assembly)
        {
            var result = from classType in assembly.GetTypes()
                         where classType.IsClass && classType.IsEntity() 
                         select new { ClassName = classType.Name.Split('.').Last(), ClassType = classType };
            return result.ToDictionary(x => x.ClassType, x => x.ClassName);
        }

        public static Dictionary<Type, string> GetAggregates(this Assembly assembly)
        {
            var result = from classType in assembly.GetTypes()
                         where classType.IsClass && classType.IsEntity()
                         && classType.GetCustomAttributes(true).Any(attr => attr is ModuleAttribute  && ((ModuleAttribute)attr).Exclude == false)
                         select new { ClassName = classType.Name.Split('.').Last(), ClassType = classType };
            return result.ToDictionary(x => x.ClassType, x => x.ClassName);
        }
        public static Dictionary<Type, string> GetAggregateClassesByModule(this Assembly assembly,string moduleName)
        {
            var result = from classType in assembly.GetTypes()
                         where classType.IsClass && classType.IsAggregateRoot() 
                         && classType.GetCustomAttributes(true).Any(attr => attr is ModuleAttribute && ((ModuleAttribute)attr).ModuleName == moduleName && ((ModuleAttribute)attr).Exclude==false)

                         select new { ClassName = classType.Name.Split('.').Last(), ClassType = classType };
            return result.ToDictionary(x => x.ClassType, x => x.ClassName);
        }
        
        public static Dictionary<Type, string> GetConfigurationClassesByModule(this Assembly assembly, string moduleName)
        {
            var result = from classType in assembly.GetTypes()
                         where classType.IsClass && classType.IsConfigurationRoot()
                         && classType.GetCustomAttributes(true).Any(attr => attr is ModuleAttribute && 
                             ((ModuleAttribute)attr).ModuleName == moduleName && ((ModuleAttribute)attr).Exclude == false)

                         select new { ClassName = classType.Name.Split('.').Last(), ClassType = classType };
            return result.ToDictionary(x => x.ClassType, x => x.ClassName);
        }
        public static Dictionary<Type, string> GetIndexClassesByModule(this Assembly assembly, string moduleName)
        {
            var result = from classType in assembly.GetTypes()
                         where classType.IsClass && classType.IsIndex()
                         && classType.GetCustomAttributes(true).Any(attr => attr is ModuleAttribute && ((ModuleAttribute)attr).ModuleName == moduleName  && ((ModuleAttribute)attr).Exclude==false)

                         select new { ClassName = classType.Name.Split('.').Last(), ClassType = classType };
            return result.ToDictionary(x => x.ClassType, x => x.ClassName);
        }
        public static Dictionary<Type, string> GetIndexClasses(this Assembly assembly)
        {
            var result = from classType in assembly.GetTypes()
                         where classType.IsClass && classType.IsIndex()
                        select new { ClassName = classType.Name.Split('.').Last(), ClassType = classType };
            return result.ToDictionary(x => x.ClassType, x => x.ClassName);
        }

        public static MethodInfo GetExtensionMethod(this Assembly assembly, string methodName)
        {
            var query = from type in assembly.GetTypes()
                        where type.IsSealed && !type.IsGenericType && !type.IsNested
                        from method in type.GetMethods(BindingFlags.Static
                                                       | BindingFlags.Public | BindingFlags.NonPublic)
                        where method.IsDefined(typeof(ExtensionAttribute), false) && method.Name == methodName
                        select method;
            return query.FirstOrDefault();
        }

        public static MethodInfo GetExtensionMethod(this Assembly assembly, string methodName, params Type[] parameterTypes)
        {
            var query = from type in assembly.GetTypes()
                        where type.IsSealed && !type.IsGenericType && !type.IsNested
                        from method in type.GetMethods(BindingFlags.Static
                                                       | BindingFlags.Public | BindingFlags.NonPublic)
                        where method.IsDefined(typeof(ExtensionAttribute), false) && method.Name == methodName
                        select method;
            return query.First(method => method.GetParameters().Select(parameter => parameter.ParameterType.IsGenericType ? parameter.ParameterType.GetGenericTypeDefinition() : parameter.ParameterType).SequenceEqual(parameterTypes));
        }
    }
}
