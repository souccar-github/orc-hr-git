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

using HRIS.Domain.PMS.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Validation.Specification.PMS.RootEntities
{
    public class AppraisalPhaseSpecification : Validates<AppraisalPhase>
    {
        public AppraisalPhaseSpecification()
        {
            IsDefaultForType();
            #region Primitive Types

            Check(x => x.Period).Required();
            Check(x => x.StartDate).Required();
            Check(x => x.EndDate).Required().GreaterThanEqualTo(x => x.StartDate);
            Check(x => x.Description).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
         
            #endregion Primitive Types
            #region Primitive Types

            #endregion

            #region Indexes

            Check(x => x.AppraisalPhaseSetting)
            .Required()
            .Expect((obj, prop) => prop.IsTransient() == false, "")
            .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.AppraisalTemplateSetting)
            .Required()
            .Expect((obj, prop) => prop.IsTransient() == false, "")
            .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion

        }
    }
}