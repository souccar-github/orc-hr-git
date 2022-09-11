using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Recruitment.Entities
{
    public class RecruitmentInformationSpecification : Validates<RecruitmentInformation>
    {
        public RecruitmentInformationSpecification()
        {

            IsDefaultForType();

            #region Primitive Types

            Check(x => x.PersonsNumberToBeAppointed).Required().GreaterThan(GlobalConstant.MinimumValue);
            Check(x => x.RecruitmentConditions).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.RequiredDocuments).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.BooksDescription).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            #endregion

            #region Indexes

            Check(x => x.JobTitle)
                .Required()
                .Expect((recruitmentInformation, jobTitle) => jobTitle.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Grade)
                .Required()
                .Expect((recruitmentInformation, grade) => grade.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }
    }
}
