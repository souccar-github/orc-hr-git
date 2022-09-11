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
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using FluentNHibernate.Mapping;
//using HRIS.Domain.Training.RootEntities;

//namespace HRIS.Mapping.Training.RootEntities
//{
//    public sealed class NewCourseMap : ClassMap<NewCourse>
//    {
//        public NewCourseMap()
//        {
//            DynamicUpdate();
//            DynamicInsert();

//            Id(x => x.Id);

//            Map(x => x.Description).Length(250);
//            Map(x => x.NumberOfEmployees);
//            Map(x => x.Status);
//            Map(x => x.NumberOfSession);
//            Map(x => x.Duration);
//            Map(x => x.CourseObjective).Length(50);
//            Map(x => x.CourseTitles).Length(50);
//            Map(x => x.TitleFilePath).Length(50); 
//            Map(x => x.StartDate);
//            Map(x => x.EndDate);
//            Map(x => x.StartHour);
//            Map(x => x.EndHour);
//            Map(x => x.PlaceStatus);
//            Map(x => x.InvitationDate);
//            Map(x => x.InvitationDescription).Length(50);
//            Map(x => x.CancelDescription).Length(250);
//            Map(x => x.Saturday);
//            Map(x => x.Sunday);
//            Map(x => x.Monday);
//            Map(x => x.Tuesday);
//            Map(x => x.Wednesday);
//            Map(x => x.Thursday);
//            Map(x => x.Friday);

//            References(x => x.TrainingCenterName).Not.Nullable();
//            References(x => x.TrainingPlace).Not.Nullable();
//            References(x => x.Priority).Not.Nullable();
//            References(x => x.LanguageName).Not.Nullable();
//            References(x => x.Trainer).Not.Nullable();
//            References(x => x.Sponsor).Not.Nullable();

//            References(x => x.Name).Not.Nullable();
//            References(x => x.Per).Not.Nullable();
//            References(x => x.Specialize).Not.Nullable();
//            References(x => x.Level).Not.Nullable();
//            References(x => x.Type).Not.Nullable();
//            References(x => x.InvitationCountry).Nullable();

//            HasMany(x => x.AppraisalTrainees).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
//            HasMany(x => x.Conditions).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
//            HasMany(x => x.CourseAppraisals).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
//            HasMany(x => x.CourseCosts).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
//            HasMany(x => x.InvolvedStaff).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
//            HasMany(x => x.SuggestionStaff).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

//        }
//    }
//}
