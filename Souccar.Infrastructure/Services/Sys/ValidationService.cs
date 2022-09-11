using System;
using System.Collections.Generic;
using System.Linq;
using Souccar.Core.Services;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using SpecExpress;
using SpecExpress.RuleTree;
using ValidationResult = Souccar.Domain.Validation.ValidationResult;

namespace Souccar.Infrastructure.Services.Sys
{
    public class ValidationService : IValidationService
    {
        public bool Validate(object entity)
        {
            var result = ValidationCatalog.Validate(entity);
            return result.IsValid;
        }

        public IList<ValidationResult> Validate(IEntity entity, SpecificationBase specification = null)
        {
            var notificationResult = 
                specification == null ? ValidationCatalog.Validate(entity) : ValidationCatalog.Validate(entity, specification);

            return notificationResult.IsValid ? new List<ValidationResult>()
                       : notificationResult.Errors.Select(x => new ValidationResult { Message = x.Message, Property = x.Property }).ToList();
        }

        public IList<ValidationRules> GetValidators(Type type, SpecificationBase pSpecification)
        {
            var specifications = new List<SpecificationBase>();
            if (pSpecification == null)
            {
                specifications = ValidationCatalog.SpecificationContainer.GetAllSpecifications().Where(x => x.ForType == type && x.DefaultForType).ToList();
            }
            else
            {
                specifications.Add(pSpecification);
            }

            var lstValidationRules = new List<ValidationRules>();

            foreach (var specification in specifications)
            {
                foreach (var propertyValidator in specification.PropertyValidators)
                {
                    var currentValidationRules = new ValidationRules
                    {
                        PropertyName = propertyValidator.PropertyName,
                        Validators = GetChildRules((RuleNode)propertyValidator.RuleTree.Root)
                    };

                    var existsValidationRule = lstValidationRules.Find(x => x.PropertyName == propertyValidator.PropertyName);
                    if (existsValidationRule != null)
                    {
                        existsValidationRule.Validators.AddRange(currentValidationRules.Validators.Where(x => existsValidationRule.Validators.All(y => y.ValidatorType != x.ValidatorType)));
                    }
                    else
                    {
                        lstValidationRules.Add(currentValidationRules);
                    }
                }
            }
            return lstValidationRules;
        }

        private static List<ValidatorInfo> GetChildRules(RuleNode node)
        {
            var childRules = new List<ValidatorInfo>();
            var validatorInfo = new ValidatorInfo();

            if (node != null)
            {
                var ruleTypeName = node.Rule.GetType().Name;
                validatorInfo.ValidatorRules.Add(new ValidatorRule { Message = node.Rule.ErrorMessageTemplate, IsValue = false });

                if (node.Rule.Params.Count != 0)
                {
                    for (var i = 0; i < node.Rule.Params.Count; i++)
                    {
                        string parameter;
                        if (node.Rule.Params[i].CompiledExpression != null)
                        {
                            parameter = node.Rule.Params[i].CompiledExpression.Expression.Body.ToString();
                            parameter = parameter.Remove(0, parameter.IndexOf('.') + 1);
                        }
                        else
                        {
                            parameter = node.Rule.Params[i].GetParamValue().ToString();
                            validatorInfo.ValidatorRules[0].IsValue = true;
                        }

                        validatorInfo.ValidatorRules[0].Parameters.Add(parameter);
                        validatorInfo.ValidatorRules[0].Message = validatorInfo.ValidatorRules[0].Message.Replace("{" + i + "}", parameter);
                    }
                }

                ValidatorType validatorType;//this variable decalared because passing validatorInfo.ValidatorType directly not working
                var isValidatorTypeSupported = Enum.TryParse(ruleTypeName.Split('`').First(), out validatorType);
                if (isValidatorTypeSupported)
                {
                    validatorInfo.ValidatorType = validatorType;
                    childRules.Add(validatorInfo);
                }

                if (node.HasChild)
                {
                    var nestedChildRules = GetChildRules((RuleNode)node.ChildNode);

                    foreach (var nestedChildRule in nestedChildRules)
                    {
                        if (childRules.Any(x => x.ValidatorType == nestedChildRule.ValidatorType))
                        {
                            var tempChildRule = childRules.Find(x => x.ValidatorType == nestedChildRule.ValidatorType);
                            tempChildRule.ValidatorRules.AddRange(nestedChildRule.ValidatorRules.Where(x => tempChildRule.ValidatorRules.All(y => y.Message != x.Message)));
                        }
                        else
                        {
                            childRules.Add(nestedChildRule);
                        }
                    }
                }   
            }
            
            return childRules;
        }

    }

}
