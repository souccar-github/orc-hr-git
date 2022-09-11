using System;
using HRIS.Domain.Recruitment.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Recruitment.RootEntities
{
    public class JobApplicationSpecification : Validates<JobApplication>
    {
        public JobApplicationSpecification()
        {
            IsDefaultForType();

            Check(x => x.ApplicantNo).Required();
            Check(x => x.ApplicationDate).Required();
            Check(x => x.FirstName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.LastName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            //Check(x => x.FatherName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            //Check(x => x.MotherName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.DateOfBirth).Required()
                .Between(DateTime.Now.AddYears(-GlobalConstant.MaximumEmployeeAge), DateTime.Now.AddYears(-GlobalConstant.MinimumEmployeeAge))
                .With(x => x.MessageKey = CustomMessageKeysRecruitmentModule.GetFullKey(CustomMessageKeysRecruitmentModule.DateOfBirthMustBeBetween10And100));

            //Check(x => x.PersonalRecordSource).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);

            //Check(x => x.CivilRecordPlaceAndNumber).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            //Check(x => x.Gender).Required();
            //Check(x => x.Religion).Required();
            //Check(x => x.MaritalStatus).Required();
            //Check(x => x.Email).Required().MaxLength(GlobalConstant.SimpleStringMaxLength).And.Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$").With(x => x.Message = CustomMessageKeysRecruitmentModule.GetFullKey(CustomMessageKeysRecruitmentModule.EmailNotValid));
            //Check(x => x.Mobile).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            //Check(x => x.Address).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            //Check(x => x.IdentificationNo).Required().MaxLength(GlobalConstant.SimpleStringMaxLength).And.IsNumeric();
            //Check(x => x.OtherDetails).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            //Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            //Check(x => x.InterviewArrangements).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);


            //Check(x => x.JoiningStatus)
            //    .Required()
            //    .Expect((jobApplication, joiningStatus) => joiningStatus.IsTransient() == false, "")
            //    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            //Check(x => x.PlaceOfBirth)
            //    .Required()
            //    .Expect((jobApplication, placeOfBirth) => placeOfBirth.IsTransient() == false, "")
            //    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            //Check(x => x.Nationality)
            //    .Required()
            //    .Expect((jobApplication, nationality) => nationality.IsTransient() == false, "")
            //    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            //Check(x => x.OtherNationality).Optional()
            //    .Expect((jobApplication, otherNationality) => jobApplication.Nationality.Id != otherNationality.Id, "")
            //    .With(x => x.MessageKey = CustomMessageKeysPersonnelModule.GetFullKey(CustomMessageKeysPersonnelModule.OtherNationalityCanNotEqualNationality));

            //Check(x => x.ApplicationSource)
            //    .Required()
            //    .Expect((jobApplication, applicationSource) => applicationSource.IsTransient() == false, "")
            //    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

        }
    }
}
