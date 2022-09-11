#region

using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Personnel.RootEntities
{
    public sealed class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            #region Relations

            //TODO Recheck EmployeeMap -- The complete scenario Need Test 
            //Comment after delete employee from appraisal
            // HasMany(x => x.Appraisals);

            #endregion

            
            Map(x => x.FirstName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.LastName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.FatherName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.MotherName).Length(GlobalConstant.SimpleStringMaxLength);
            
            Map(x => x.FirstNameL2).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.LastNameL2).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.FatherNameL2).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.MotherNameL2).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.PlaceOfBirthL2).Length(GlobalConstant.SimpleStringMaxLength);

            References(x => x.PlaceOfBirth).Nullable();
            References(x => x.CountryOfBirth).Nullable();
           
            Map(x => x.Gender);

            Map(x => x.MaritalStatus);

            Map(x => x.DateOfBirth);

            Map(x => x.CivilRecordPlaceAndNumber).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.PersonalRecordSource).Length(GlobalConstant.SimpleStringMaxLength);
            
            References(x => x.Nationality).Nullable();
            References(x => x.User).Nullable();

            Map(x => x.OtherNationalityExist);

            References(x => x.OtherNationality).Nullable();
          

            Map(x => x.BloodType).Not.Nullable();
            Map(x => x.Religion).Not.Nullable();
            Map(x => x.MilitaryStatus).Nullable();
            Map(x => x.Status).Nullable();

                //Map(x => x.NoOfChildren);
                //Map(x => x.NoOfDependents);

            Map(x => x.Address).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.Phone).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Mobile).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Email).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.POBox).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.WebSite).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Twitter).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Facebook).Length(GlobalConstant.SimpleStringMaxLength);

            Map(x => x.IdentificationNo).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Code).Length(GlobalConstant.SimpleStringMaxLength);

            //Map(x => x.SocialSecurityNo).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.IsRetired);
            Map(x => x.PhotoId);
           
            //References(x => x.SocialInsuranceNoStatus).Nullable();
            //Map(x => x.SocialInsuranceNo).Length(GlobalConstant.SimpleStringMaxLength);
            
            Map(x => x.SalaryStatus).Not.Nullable();

            Map(x => x.DisabilityExist).Not.Nullable();
           // Map(x => x.DisabilityDescription);
            References(x => x.DisabilityType);
            //References(x => x.DisabilityType);

            References(x => x.RecruitmentInformation).Nullable();

            HasMany(x => x.Spouse).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.DrivingLicense).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.MilitaryService).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.Children).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.Dependents).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.Certifications).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.Educations).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.Experiences).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.Skills).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.Trainings).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.Passports).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            //-------------------------
            
            HasMany(x => x.Attachments).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            //-----------------------
            HasMany(x => x.Residencies).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.Convictions).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.Languages).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.JobRelatedInfos).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            //HasMany(x => x.EmployeeBenefits).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            //HasMany(x => x.EmployeeDeduction).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            //HasMany(x => x.Retrieval).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            //HasMany(x => x.Recantation).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            //HasMany(x => x.Loan).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            //HasMany(x => x.Bonuse).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            HasMany(x => x.Positions).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.LogOfPositions).Inverse().LazyLoad().Cascade.AllDeleteOrphan();



            HasOne(x => x.EmployeeCard).Cascade.All().PropertyRef("Employee");
        }
    }
}