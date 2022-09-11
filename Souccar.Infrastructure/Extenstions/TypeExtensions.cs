using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Souccar.Core.Extensions;
using Souccar.Domain.DomainModel;
using SpecExpress;
using SpecExpress.RuleTree;
using Souccar.Domain.Validation;
using System.Reflection;
using Souccar.Infrastructure.Services.Sys;
using System.Globalization;
using Souccar.Infrastructure.Core;

namespace Souccar.Infrastructure.Extenstions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Auther: Yaseen Alrefaee
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>The title of type from localization service</returns>
        public static string GetTitle(this Type type)
        {
            var service = ServiceFactory.LocalizationService;
            return service.GetLocalizedEntity(type);
        }
        /// <summary>
        /// Auther: Yaseen Alrefaee
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>Titles of properties in type from localization service</returns>
        public static Dictionary<string, string> GetTitlesForAllProperties(this Type type)
        {
            var service = ServiceFactory.LocalizationService;
            return service.GetAllLocalizedEntityProperties(type);
        }

        public static IQueryable<T> GetAll<T>(this Type type, T t = null) where T : Entity ,IAggregateRoot
        {
            return ServiceFactory.ORMService.All<T>();
        }

        public static IQueryable<T> GetAllWithVertualDeleted<T>(this Type type, T t = null) where T : Entity, IAggregateRoot
        {
            return ServiceFactory.ORMService.AllWithVertualDeleted<T>();
        }

        /// <summary>
        /// Author: Yaseen Alrefaee
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Entity GetById(this Type type, int id)
        {
            var method = ServiceFactory.ORMService.GetType().GetMethod("GetById");
            method = method.MakeGenericMethod(new Type[] {type});
            return (Entity) method.Invoke(ServiceFactory.ORMService, new object[] {id});
        }

        public static List<ValidationRules> GetValidators(this Type type, SpecificationBase specification = null)
        {
            return new ValidationService().GetValidators(type, specification).ToList();
        }

        public static string GetPropertyNameAsString<T>(this Type type, Expression<Func<T, object>> expression)
        {
            var propertyName = String.Empty;
            var forReferanceType = expression.Body as MemberExpression;
            if (forReferanceType != null)
            {
                propertyName = ((MemberExpression)expression.Body).Member.Name;
            }
            else
            {
                var forPrimitiveType = expression.Body as UnaryExpression;
                if (forPrimitiveType != null)
                {
                    propertyName = ((MemberExpression)forPrimitiveType.Operand).Member.Name;
                }
            }

            if (String.IsNullOrEmpty(propertyName))
            {
                throw new Exception("This Property Not Supported, We Will Support It");
            }
            return propertyName;
        }
    }

}
