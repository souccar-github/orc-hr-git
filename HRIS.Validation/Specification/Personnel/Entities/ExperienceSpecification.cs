using System;
using HRIS.Domain.Personnel.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Personnel.Entities
{
    public class ExperienceSpecification : Validates<Experience>
    {
        public ExperienceSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.CompanyName).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.CompanyLocation).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.CompanyWebSite).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.Matches(@"^([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$");
            Check(x => x.StartDate).Optional().LessThan(x=>x.EndDate);
           

            Check(x => x.LeaveReason).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            Check(x => x.ReferenceFullName).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);

          
            Check(x => x.ReferenceContact).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.ReferenceEmail).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            Check(x => x.ReferenceAddress).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Notes).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            #endregion

            #region Indexes
            Check(x => x.JobTitle)
                .Required()
                .Expect((spouse, jobTitle) => jobTitle.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            //Check(x => x.ReferenceJobTitle)
            //    .Optional()
            //    .Expect((spouse, nationality) => nationality.IsTransient() == false, "")
            //    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Industry)
                .Required()
                .Expect((spouse, industry) => industry.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }
    }
}
