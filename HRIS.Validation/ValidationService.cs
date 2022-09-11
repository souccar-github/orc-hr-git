using System;
using System.Collections.Generic;
using System.Linq;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using SpecExpress;
using SpecExpress.RuleTree;

namespace HRIS.Validation
{
    public class ValidationService : IValidationService
    {
        public bool Validate(object entity)
        {
            var result = ValidationCatalog.Validate(entity);
            return result.IsValid;


        }

        public IList<PropertyValidationRules> Validate(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public IList<PropertyValidationRules> GetValidators(Type type)
        {
            throw new NotImplementedException();
        }
    }

}
