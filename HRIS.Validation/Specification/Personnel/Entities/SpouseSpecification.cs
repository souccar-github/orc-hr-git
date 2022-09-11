using System;
using System.Security.Cryptography.X509Certificates;
using HRIS.Domain.Personnel.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Personnel.Entities
{
    public class SpouseSpecification : Validates<Spouse>
    {
        public SpouseSpecification()
        {
            IsDefaultForType();

            #region Primitive Types
            Check(x => x.IdentificationNo).Required().MaxLength(GlobalConstant.SimpleStringMaxLength).And.IsNumeric();
            Check(x => x.FirstName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.LastName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.FatherName).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.MatherName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.DateOfBirth).Optional().LessThanEqualTo(DateTime.Now);

            //Check(x => x.DateOfFamilyBenefitActivation).Optional().GreaterThanEqualTo(x => x.MarriageDate);

            
            Check(x => x.DivorceDate).If(x=>x.IsDivorce).Optional().GreaterThan(x => x.MarriageDate)
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.GreaterThan));
            Check(x => x.DeathDate).If(x => x.IsDeath).Optional().GreaterThan(x => x.MarriageDate)
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.GreaterThan));

            Check(x => x.MarriageDate).If(x=>x.DateOfBirth != null).Optional().GreaterThanEqualTo(x => (DateTime)x.DateOfBirth)
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.GreaterThanEqualTo));         
            


            Check(x => x.ResidencyNo).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.IsNumeric();
            //Check(x => x.ResidencyExpiryDate).Optional();

            Check(x => x.PassportNo).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.IsNumeric();
            //Check(x => x.PassportExpiryDate).Optional();

            Check(x => x.Note).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            Check(x => x.Order).Required().GreaterThan(0);

            Check(x => x.MarriageDate).Required().LessThan(x => DateTime.Now.Date)
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.LessThanEqualTo));

            Check(x => x.FirstContactNumber).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.IsNumeric();
            Check(x => x.SecondContactNumber).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.IsNumeric();

            Check(x => x.Email).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

            //Check(x => x.HasJob).If(x =>
            //    !string.IsNullOrEmpty(x.CompanyName)
            //    || !string.IsNullOrEmpty(x.WorkAddress)
            //    || !string.IsNullOrEmpty(x.WorkEmail)
            //    || !string.IsNullOrEmpty(x.CompanyName))
            //    .Required().IsTrue();

            //Check(x => x.IsGovernmentWork).Optional();

            Check(x => x.CompanyName).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);


            Check(x => x.WorkPhone).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.IsNumeric();
            Check(x => x.WorkEmail).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            Check(x => x.WorkAddress).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            //Check(x => x.Employee.MaritalStatus)
            // .Optional()
            // .Expect((spouse, maritalStatus) => maritalStatus != Domain.Personnel.Enums.MaritalStatus.Married, "")
            // .With(x => x.MessageKey = CustomMessageKeysPersonnelModule.GetFullKey(CustomMessageKeysPersonnelModule.AddMaritalStatusToUnMarried));

            #endregion

            #region Indexes

            Check(x => x.PlaceOfBirth)
                .Optional()
                .Expect((spouse, placeOfBirth) => placeOfBirth.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Nationality)
                .Optional()
                .Expect((spouse, nationality) => nationality.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.JobTitle)
                .Optional()
                .Expect((spouse, nationality) => nationality.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

         

            Check(x => x.ResidencyExpiryDate)
                .If(x => string.IsNullOrEmpty(x.ResidencyNo) == false)
                .Optional()
                .GreaterThan(DateTime.Today);

            Check(x => x.PassportExpiryDate)
                .If(x => string.IsNullOrEmpty(x.PassportNo) == false)
                .Optional()
                .GreaterThan(DateTime.Today);
            #endregion
        }
    }
}