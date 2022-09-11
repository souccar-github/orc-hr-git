using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.JobDesc.Indexes;
using HRIS.Domain.OrgChart.Indexes;
using HRIS.Domain.Training1.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Training1.Enums;
using HRIS.Domain.Training1.Entities;
using HRIS.Domain.Personnel.Indexes;

namespace HRIS.Domain.Training1.RootEntities
{
    [Module("Training1")]
    public class Course : Entity, IAggregateRoot
    {
        public Course()
        {
            Days = new List<DayOfWeek>();
            Conditions = new List<CourseCondition>();
            SuggestionStaff = new List<SuggestionStaff>();
            InvolvedStaff = new List<InvolvedStaff>();
            CourseCosts = new List<CourseCost>();
            CourseAppraisals = new List<AppraisalCourse>();
            AppraisalTrainees = new List<AppraisalTrainee>();
           

        }

        #region basic info

        public virtual string Description { get; set; }
        public virtual int NumberOfEmployees { get; set; }
        public virtual CourseStatus Status { get; set; }
        public virtual int NumberOfSession { get; set; }
        public virtual int Duration { get; set; }
        public virtual string CourseObjective { get; set; }
        public virtual string CourseTitles { get; set; }
        public virtual string TitleFilePath { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual int StartHour { get; set; }
        public virtual int EndHour { get; set; }
        public virtual IList<DayOfWeek>Days { get; set; }
        public virtual CoursePlaceStatus PlaceStatus { get; set; }
        public virtual DateTime InvitationDate { get; set; }
        public virtual string InvitationDescription { get; set; }
        public virtual string CancelDescription { get; set; }

        #endregion 

        #region indexes

        public virtual CourseName Name { get; set; }
        public virtual CourseSpecialize Specialize { get; set; }
        public virtual TrainingCenterName TrainingCenterName { get; set; }
        public virtual TrainingPlace TrainingPlace{ get; set; }
        public virtual CourseType Type { get; set; }
        public virtual TimeInterval Per { get; set; }
        public virtual Priority Priority { get; set; }
        public virtual LanguageName LanguageName { get; set; }
        public virtual CourseSponsor Sponsor { get; set; }
        public virtual Country InvitationCountry { get; set; }
        
        #endregion

        #region Ref

        public virtual TrainingNeedsPool NeedsPool { get; set; }
        public virtual TrainingPlan Plan { get; set; }
        public virtual Trainer Trainer { get; set; }
        public virtual IList<CourseCondition>  Conditions { get; set; }
        public virtual void AddCourseCondition(CourseCondition condition)
        {
            condition.Course = this;
            Conditions.Add(condition);
        }


        public virtual IList<SuggestionStaff> SuggestionStaff { get; set; }
        public virtual void AddSuggestionStaff(SuggestionStaff staff)
        {
            staff.Course = this;
            SuggestionStaff.Add(staff);
        }

        public virtual IList<InvolvedStaff> InvolvedStaff { get; set; }
        public virtual void AddInvolvedStaff(InvolvedStaff staff)
        {
            staff.Course = this;
            InvolvedStaff.Add(staff);
        }

        public virtual IList<CourseCost> CourseCosts { get; set; }
        public virtual void AddCourseCost(CourseCost cost)
        {
            cost.Course = this;
            CourseCosts.Add(cost);
        }

        public virtual IList<AppraisalCourse> CourseAppraisals { get; set; }
        public virtual void AddAppraisalCourse(AppraisalCourse appraisalCourse)
        {
            appraisalCourse.Course = this;
            CourseAppraisals.Add(appraisalCourse);
        }

        public virtual IList<AppraisalTrainee> AppraisalTrainees { get; set; }
        public virtual void AddAppraisalTrainee(AppraisalTrainee appraisalTrainee)
        {
            appraisalTrainee.Course = this;
            AppraisalTrainees.Add(appraisalTrainee);
        }
        #endregion 
    }
}
