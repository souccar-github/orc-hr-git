//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//project manager:
//supervisor:
//author: Ammar Alziebak
//description:
//start date:
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//

using HRIS.Domain.PMS.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Validation.Specification.PMS.Entities
{
    public class AppraisalSectionItemKpiSpecification : Validates<AppraisalSectionItemKpi>
    {
        public AppraisalSectionItemKpiSpecification()
        {
            IsDefaultForType();

            #region Primitive Types
            
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Weight).Optional().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);
            Check(x => x.Value).Required().GreaterThanEqualTo(GlobalConstant.MinimumValue);
            #endregion Primitive Types 

            #region KPI

            Check(x => x.Value).Required();
            Check(x => x.Description).Required();

            #endregion

            #region Indexes
            #endregion Indexes

        }
    }
}