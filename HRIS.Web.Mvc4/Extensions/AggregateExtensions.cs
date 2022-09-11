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
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Infrastructure.Extenstions;
using  Project.Web.Mvc4.Extensions;
namespace Project.Web.Mvc4.Extensions
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public static class AggregateExtensions
    {
        public static IList<ValidationResult> Save(this IAggregateRoot aggregateRoot, SpecificationBase specificationBase = null)
        {
           return  aggregateRoot.Save(UserExtensions.CurrentUser, null);
        }

        public static IList<ValidationResult> Delete(this IAggregateRoot aggregateRoot, SpecificationBase specificationBase = null)
        {
            return aggregateRoot.Delete(UserExtensions.CurrentUser, null);
        }
        public static void SaveWithoutValidation(this IAggregateRoot aggregateRoot, SpecificationBase specificationBase = null)
        {
             aggregateRoot.SaveWithoutValidation(UserExtensions.CurrentUser, null);
        }

        public static void DeleteWithoutValidation(this IAggregateRoot aggregateRoot, SpecificationBase specificationBase = null)
        {
             aggregateRoot.DeleteWithoutValidation(UserExtensions.CurrentUser, null);
        }
    }
}
