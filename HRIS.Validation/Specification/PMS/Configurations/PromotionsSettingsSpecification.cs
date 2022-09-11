using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.PMS.RootEntities;
using HRIS.Domain.Workflow;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using SpecExpress;
using HRIS.Domain.PMS.Configurations;

namespace HRIS.Validation.Specification.PMS.Configurations
{
    public class PromotionsSettingsSpecification : Validates<PromotionsSettings>
    {
        public PromotionsSettingsSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.StartDate).Required();
            Check(x => x.EndDate).Required().GreaterThanEqualTo(x => x.StartDate).And.GreaterThan(DateTime.Now);

            #endregion Primitive Types
            
            #region Indexes

            #endregion Indexes

        }
    }
}
