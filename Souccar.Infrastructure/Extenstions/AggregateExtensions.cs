using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Core.Extensions;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using SpecExpress;
using ValidationResult = Souccar.Domain.Validation.ValidationResult;
using Souccar.Domain.Security;

namespace Souccar.Infrastructure.Extenstions
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public static class AggregateExtensions
    {
        public static IList<ValidationResult> Save(this IAggregateRoot aggregateRoot,User user, SpecificationBase specificationBase = null)
        {
            var validationResults = ServiceFactory.ValidationService.Validate((IEntity)aggregateRoot, specificationBase);
            if (validationResults.Any())
            {
                var errorsMessages = new Dictionary<string, string>();
                foreach (var error in validationResults)
                {
                    if (errorsMessages.Keys.All(x => x != error.Property.Name))
                        errorsMessages.Add(error.Property.Name, error.Message);
                }
            }
            else
            {
                var objType = aggregateRoot.GetType();
                var obj = Convert.ChangeType(aggregateRoot, objType);
                var method = ServiceFactory.ORMService.GetType().GetMethod("Save");
                method = method.MakeGenericMethod(new Type[] {objType});
                method.Invoke(ServiceFactory.ORMService, new[] { obj, user });
            }
            return validationResults;
        }

        public static IList<ValidationResult> Delete(this IAggregateRoot aggregateRoot, User user, SpecificationBase specificationBase = null)
        {
            var validationResults = ServiceFactory.ValidationService.Validate((IEntity)aggregateRoot, specificationBase);
            if (validationResults.Any())
            {
                var errorsMessages = new Dictionary<string, string>();
                foreach (var error in validationResults)
                {
                    if (errorsMessages.Keys.All(x => x != error.Property.Name))
                        errorsMessages.Add(error.Property.Name, error.Message);
                }
            }
            else
            {
                var objType = aggregateRoot.GetType();
                var obj = Convert.ChangeType(aggregateRoot, objType);
                var method = ServiceFactory.ORMService.GetType().GetMethod("Delete");
                method = method.MakeGenericMethod(new Type[] { objType });
                method.Invoke(ServiceFactory.ORMService, new[] { obj, user });
            }
            return validationResults;
        }
        public static void SaveWithoutValidation(this IAggregateRoot aggregateRoot, User user, SpecificationBase specificationBase = null)
        {
           
                var objType = aggregateRoot.GetType();
                var obj = Convert.ChangeType(aggregateRoot, objType);
                var method = ServiceFactory.ORMService.GetType().GetMethod("Save");
                method = method.MakeGenericMethod(new Type[] { objType });
                method.Invoke(ServiceFactory.ORMService, new[] { obj, user });
           
        }

        public static void DeleteWithoutValidation(this IAggregateRoot aggregateRoot, User user, SpecificationBase specificationBase = null)
        {
           
            
                var objType = aggregateRoot.GetType();
                var obj = Convert.ChangeType(aggregateRoot, objType);
                var method = ServiceFactory.ORMService.GetType().GetMethod("Delete");
                method = method.MakeGenericMethod(new Type[] { objType });
                method.Invoke(ServiceFactory.ORMService, new[] { obj, user });
            
        }

    }
}
