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
using HRIS.Domain.PMS.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Validation.Specification.PMS.Entities
{
    public class AppraisalSectionItemSpecification : Validates<AppraisalSectionItem>
    {
        public AppraisalSectionItemSpecification()
        {
            IsDefaultForType();
            
            #region Primitive Types

            Check(x => x.Name).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Weight).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);

            #endregion Primitive Types

            #region Indexes
            Check(x => x.AppraisalSection)
                .Required()
                .Expect((obj, prop) => prop.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            #endregion Indexes

        }
    }
}