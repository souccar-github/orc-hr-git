using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRIS.Domain.Objectives.Entities;
using SpecExpress;

namespace HRIS.Validation.Specification.Objectives.Entities
{
    public class ObjectiveKpiSpecification:Validates<ObjectiveKpi>
    {
        public ObjectiveKpiSpecification()
        {
            IsDefaultForType();

            #region Primitive types

            Check(x => x.Description).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Weight).Optional().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);
            Check(x => x.Value).Required().GreaterThanEqualTo(GlobalConstant.MinimumValue);

            #endregion

            #region Indexs

            #endregion
        }
    }
}
