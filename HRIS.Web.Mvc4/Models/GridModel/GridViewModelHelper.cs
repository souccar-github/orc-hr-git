using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.Validation;

namespace Project.Web.Mvc4.Models.GridModel
{
    public enum FilterLogic
    {
        And,
        Or
    }

    public enum FilterOperator
    {
        Eq,
        Neq,
        Lt,
        Ltr,
        Gt,
        Gte,
        StartsWith,
        EndsWith,
        Contains
    }

    public static class GridViewModelHelper
    {
        public static FieldType GetFieldTypeName(Type type,PropertyInfo propertyInfo)
        {
            if (type == typeof (int))
                return FieldType.Number;
            if (type == typeof (float))
                return FieldType.Number;
            if (type == typeof (double))
                return FieldType.Number;
            if (type == typeof (decimal))
                return FieldType.Number;
            if (type == typeof (DateTime))
                return FieldType.Date;
            if (type == typeof(DateTime?))
                return FieldType.Date;
            if (type == typeof (bool))
                return FieldType.Boolean;
            if (type == typeof (Single))
                return FieldType.Number;
            if (type == typeof (short))
                return FieldType.Number;
            if (type == typeof (string))
            {
                var attrs = propertyInfo.GetCustomAttributes(typeof(UserInterfaceParameterAttribute), false);
                if (attrs.Count() != 0)
                {
                    var isImageColumn = ((UserInterfaceParameterAttribute) attrs.First()).IsImageColumn;
                    if(isImageColumn)
                        return FieldType.Image;

                    var isFileColumn = ((UserInterfaceParameterAttribute)attrs.First()).IsFile;
                    if (isFileColumn)
                        return FieldType.File;
                }
            }
            return FieldType.String;
        }
        public static FieldType GetFieldTypeName(Type type)
        {
            if (type == typeof(int))
                return FieldType.Number;
            if (type == typeof(float))
                return FieldType.Number;
            if (type == typeof(double))
                return FieldType.Number;
            if (type == typeof(decimal))
                return FieldType.Number;
            if (type == typeof(DateTime))
                return FieldType.Date;
            if (type == typeof(bool))
                return FieldType.Boolean;
            if (type == typeof(Single))
                return FieldType.Number;
            if (type == typeof(short))
                return FieldType.Number;
           
            return FieldType.String;
        }

        public static string GetValidationTypeName(ValidatorType type)
        {
            return validationTypesMap[type];
        }

        private static readonly Dictionary<ValidatorType, string> validationTypesMap = new Dictionary
            <ValidatorType, string>()
        {
            {ValidatorType.Required, "required"},
            {ValidatorType.MinLength, "min"},
            {ValidatorType.MaxLength, "maxlength"},
            {ValidatorType.LessThan, "max"},
            {ValidatorType.LessThanEqualTo, "max"},
            {ValidatorType.GreaterThan, "min"},
            {ValidatorType.GreaterThanEqualTo, "min"}
        };

        public static bool ShowCommaSeparatorForType(System.Reflection.PropertyInfo prop)
        {
            var type = prop.PropertyType;
            if (type == typeof (float) || type == typeof (double) || type == typeof (decimal))
                return true;
            return false;
        }
    }
}