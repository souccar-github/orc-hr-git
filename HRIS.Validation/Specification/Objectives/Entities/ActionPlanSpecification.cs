using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRIS.Domain.Objectives.Entities;
using SpecExpress;

namespace HRIS.Validation.Specification.Objectives.Entities
{
    public class ActionPlanSpecification:Validates<ActionPlan>
    {
        public ActionPlanSpecification()
        {
            IsDefaultForType();

            #region Primitive types

            Check(x => x.Description).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Status).Required();

            Check(x => x.PlannedStartDate).Required().LessThanEqualTo(x => x.PlannedEndDate);
            Check(x => x.ActualStartDate).Required();
            Check(x => x.PlannedEndDate).Required();
            Check(x => x.ExpectedResult).Required();
          
            Check(x => x.Owner).Required();

            Check(x => x.PercentageOfCompletion).Optional().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);


            #endregion

            #region Indexs

            #endregion

        }
    }
}
