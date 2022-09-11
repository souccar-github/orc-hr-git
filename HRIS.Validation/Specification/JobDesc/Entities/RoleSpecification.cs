using HRIS.Domain.JobDescription.Entities;
using HRIS.Validation.MessageKeys;
using HRIS.Validation.Specification.Index;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Validation.Specification.JobDescription.Entities
{
    public class RoleSpecification : Validates<Role>
    {
        public RoleSpecification()
        {
            IsDefaultForType();

            #region Primitive Types
            Check(x => x.Name).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.Summary).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Weight).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);
           
            #endregion Primitive Types
     

        }
    }
}

