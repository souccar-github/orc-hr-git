using System;
using System.ComponentModel.DataAnnotations;
using Souccar.Core;
using Souccar.Core.DesignByContract;
using Souccar.Domain;
using Souccar.Domain.DomainModel;
using Souccar.Domain.PersistenceSupport;

namespace Souccar.NHibernate.NHibernateValidator
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HasUniqueDomainSignatureWithStringIdAttribute : ValidationAttribute
    {
        public HasUniqueDomainSignatureWithStringIdAttribute()
            : base("Provided values matched an existing, duplicate entity")
        {
        }

        public override bool IsValid(object value)
        {
            var entityToValidate = value as IEntityWithTypedId<string>;
            Check.Require(
                entityToValidate != null,
                "This validator must be used at the class level of an IDomainWithTypedId<string>. The type you provided was " +
                value.GetType());

            IEntityDuplicateChecker duplicateChecker = SafeServiceLocator<IEntityDuplicateChecker>.GetService();
            return !duplicateChecker.DoesDuplicateExistWithTypedIdOf(entityToValidate);
        }
    }
}