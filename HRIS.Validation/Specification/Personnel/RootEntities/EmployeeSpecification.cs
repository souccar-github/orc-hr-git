using System;
using System.Security.Cryptography.X509Certificates;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Personnel.RootEntities
{
    public class EmployeeSpecification : Validates<Employee>
    {

        public EmployeeSpecification() 
        {
            IsDefaultForType();

            #region Primitive Types
            Check(x => x.FirstName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.LastName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.FatherName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.MotherName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);

            Check(x => x.FirstNameL2).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.LastNameL2).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.MotherNameL2).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.FatherNameL2).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);


            Check(x => x.Gender).Required();
            Check(x => x.MaritalStatus).Required();
           // Check(x => x.DateOfBirth).Required().LessThan(DateTime.Now.AddYears(-GlobalConstant.MinimumEmployeeAge));
            Check(x => x.DateOfBirth).Required().Between(DateTime.Now.AddYears(-GlobalConstant.MaximumEmployeeAge), DateTime.Now.AddYears(-GlobalConstant.MinimumEmployeeAge)); ;
            //Check(x => x.PlaceOfBirth).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);

            //Check(x => x.BloodType).Required();
            Check(x => x.Religion).Required();
            Check(x => x.Status).Required();


            Check(x => x.CivilRecordPlaceAndNumber).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.PersonalRecordSource).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);

                //Check(x => x.NoOfChildren).Optional();
            //Check(x => x.NoOfDependents).Optional();
            

            Check(x => x.Address).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Phone).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.IsNumeric();
            Check(x => x.Mobile).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.IsNumeric();
            Check(x => x.Email).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            Check(x => x.POBox).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.WebSite).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.Twitter).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.Facebook).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);
            //Check(x => x.Code).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);

            Check(x => x.IdentificationNo).Required().MaxLength(GlobalConstant.SimpleStringMaxLength).And.IsNumeric();
            //Check(x => x.DisabilityExist).Optional();
            
            //Check(x => x.SocialInsuranceNo).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);
            //Check(x => x.SocialSecurityNo).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);
      
            #endregion

            #region Indexes

            Check(x => x.Nationality)
                   .Required()
                   .Expect((employee, nationality) => nationality.IsTransient() == false, "")
                   .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.PlaceOfBirth)
                   .Required()
                   .Expect((employee, placeOfBirth) => placeOfBirth.IsTransient() == false, "")
                   .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.OtherNationality)
                .Optional()
                //.Expect((employee, otherNationality) => otherNationality.IsTransient() == false, "")
                //.With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required))
                //.And
                .Expect((employee, otherNationality) => employee.Nationality.Id != otherNationality.Id, "")
                .With(x => x.MessageKey = CustomMessageKeysPersonnelModule.GetFullKey(CustomMessageKeysPersonnelModule.OtherNationalityCanNotEqualNationality));

            //Check(x => x.SocialInsuranceNoStatus)
            //     .Required()
            //     .Expect((employee, employeeStatus) => employeeStatus.IsTransient() == false, "")
            //     .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            //Check(x => x.CountryOfBirth)
            //              .Required()
            //              .Expect((employee, countryOfBirth) => countryOfBirth.IsTransient() == false, "")
            //              .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));


            #endregion
        }

      
    }
}
