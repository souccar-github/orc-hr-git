using FluentNHibernate.Mapping;
using HRIS.Domain.Recruitment.RootEntities;
using Souccar.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.Recruitment.RootEntities
{
    public class JobApplicationMap : ClassMap<JobApplication>
    {
        public JobApplicationMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            //Personal Info
            Map(x => x.ApplicantNo).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.FirstName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.LastName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.FatherName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.MotherName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.FirstNameL2).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.LastNameL2).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.FatherNameL2).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.MotherNameL2).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.PlaceOfBirthL2).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Gender);
            Map(x => x.MaritalStatus);
            Map(x => x.DateOfBirth);
            Map(x => x.IdentificationNo).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.CivilRecordPlaceAndNumber).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.PersonalRecordSource).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.OtherNationalityExist);
            Map(x => x.BloodType).Not.Nullable();
            Map(x => x.Religion).Not.Nullable();
            Map(x => x.MilitaryStatus).Nullable();
            Map(x => x.Status).Nullable();
            Map(x => x.Address).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.Phone).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Mobile).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Fax).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.SecondaryMobile).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Email).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.SecondaryEmail).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.POBox).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.WebSite).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Twitter).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Facebook).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.NoOfChildren);
            Map(x => x.NoOfDependents);
            References(x => x.PlaceOfBirth).Nullable();
            References(x => x.CountryOfBirth).Nullable();
            References(x => x.Nationality).Nullable();
            References(x => x.OtherNationality).Nullable();
            References(x => x.Race).Nullable();

            //Application Details
            Map(x => x.ApplicationDate);
            References(x => x.RecruitmentRequest).Nullable();
            References(x => x.Position).Nullable();
            References(x => x.JoiningStatus);

            //Foreign Applicant Information 
            Map(x => x.HaveWorkPermit);
            Map(x => x.Duration);
            Map(x => x.HaveResidencyCard);

            //Medical Information
            Map(x => x.DisabilityExist);
            Map(x => x.InterviewArrangements);
            References(x => x.DisabilityType);

            //Other Info
            Map(x => x.OtherDetails).Length(GlobalConstant.MultiLinesStringMaxLength); ;
            Map(x => x.ApplicationYear);
            Map(x => x.ApplicationStatus);
            Map(x => x.EnterBy);
            References(x => x.ApplicationSource);
            References(x => x.Requester);

            HasMany(x => x.Educations).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.WorkingExperiences).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.JobApplicationAttachments).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.ProfessionalCertifications).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.TrainingCourses).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.Languages).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.ComputerSkills).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.PersonalSkills).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.Interviews).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.RecruitmentMilitaryServices).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

        }
    }
}
