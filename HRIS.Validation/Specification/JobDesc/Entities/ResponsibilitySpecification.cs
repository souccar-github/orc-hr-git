using HRIS.Domain.JobDescription.Entities;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Validation.Specification.JobDescription.Entities
{
    public class ResponsibilitySpecification : Validates<Responsibility>
    {
        public ResponsibilitySpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Description).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Weight).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);

            #endregion Primitive Types

            #region Indexes

            #endregion Indexes
        }
    }
}
