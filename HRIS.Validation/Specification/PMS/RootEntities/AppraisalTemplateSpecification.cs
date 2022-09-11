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
    public class AppraisalTemplateSpecification : Validates<AppraisalTemplate>
    {
        public AppraisalTemplateSpecification()
        {
            IsDefaultForType();

            #region Primitive Types
            Check(x => x.Name).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.CreationDate).Required();
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.CompetencyWeight).Optional().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);
            Check(x => x.JobDescriptionWeight).Optional().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);
            Check(x => x.ObjectiveWeight).Optional().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);
            #endregion Primitive Types
            #region Indexes
            Check(x => x.Type)
                .Required()
                .Expect((obj, prop) => prop.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            #endregion

        }
    }
}