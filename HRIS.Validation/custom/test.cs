using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpecExpress;
namespace HRIS.Validation.custom
{
   public class EmailAddressValidator<T> : SpecExpress.Rules.RuleValidator<T,string>
{
    private readonly string VALID_EMAIL_REGEX = @"([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})";

    public EmailAddressValidator()
    {
        //Define either a Message Store Name or a default Message
        //MessageStoreName = "MyCompanyValidationMessages";
       //Message = "{PropertyName} is an invalid e-mail address";
    }

    public override object[] Parameters
    {
        get { return new object[]{}; }
    }

    public override ValidationResult Validate(RuleValidatorContext<T, string> context, , SpecificationContainer specificationContainer)
    {
        if (context.PropertyValue == null)
        {
            return null;
        }
        else
        {
            Regex emailRegex = new Regex(VALID_EMAIL_REGEX);
            return Evaluate(emailRegex.IsMatch(context.PropertyValue), context);
        }
    }
}}