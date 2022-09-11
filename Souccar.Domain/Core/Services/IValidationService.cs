using System;
using System.Collections.Generic;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using SpecExpress;
using ValidationResult = Souccar.Domain.Validation.ValidationResult;

namespace Souccar.Core.Services
{
    public interface IValidationService : IService
    {
        bool Validate(object entity);
        IList<ValidationResult> Validate(IEntity entity, SpecificationBase specification);
        IList<ValidationRules> GetValidators(Type type, SpecificationBase specification);
    }
}
