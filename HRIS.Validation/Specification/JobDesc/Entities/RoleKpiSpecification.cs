using HRIS.Domain.JobDescription.Entities;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Validation.Specification.JobDescription.Entities
{
    public class RoleKpiSpecification : Validates<RoleKpi>
    {
        public RoleKpiSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Description).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Value).Required().GreaterThanEqualTo(GlobalConstant.MinimumValue);
            Check(x => x.Weight).Optional().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);

            #endregion Primitive Types

            #region Indexes

            #endregion Indexes
        }
    }
}
