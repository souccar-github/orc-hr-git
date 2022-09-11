using Souccar.Core.Services;
using Souccar.Domain;
using Souccar.Domain.Validation;

namespace Souccar.NHibernate.NHibernateValidator
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using global::NHibernate.Event;

    using Souccar.Domain.DomainModel;

    [Serializable]
    internal class ValidationEventListener : IPreUpdateEventListener, IPreInsertEventListener
    {
        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            if (@event.Entity is ValidatableObject)
            {
                var entity = @event.Entity;
                var validationService = SafeServiceLocator<IValidationService>.GetService();
                if (validationService != null)
                    return validationService.Validate(entity);
                else
                Validator.ValidateObject(entity, new ValidationContext(entity, null, null), true);
            }
            return false;
        }

        public bool OnPreInsert(PreInsertEvent @event)
        {
            if (@event.Entity is ValidatableObject)
            {
                var entity = @event.Entity;
                var validationService = SafeServiceLocator<IValidationService>.GetService();
                if (validationService != null)
                    return validationService.Validate(entity);
                else
                    Validator.ValidateObject(entity, new ValidationContext(entity, null, null), true);
            }
            return false;
        }
    }
}
