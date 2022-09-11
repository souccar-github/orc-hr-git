using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.JobDescription.Indexes;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.PMS.Helpers;
using HRIS.Domain.Training.Enums;
using HRIS.Domain.Training.Indexes;
using HRIS.Domain.Training.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using Entity = Souccar.Domain.DomainModel.Entity;

namespace HRIS.Domain.Training.Entities
{
    [Command(CommandsNames.AddTrainingNeedsToTrainingCourse, Order = 1)]
    [Command(CommandsNames.ActivateTrainingCourse, Order = 2)]
    [Command(CommandsNames.TrainingCourseCancellation, Order = 3)]
    [Command(CommandsNames.SuggestStaffsToTrainingCourse, Order = 4)]
    [Command(CommandsNames.AddTraineesToTrainingCourse, Order = 5)]
    [Command(CommandsNames.CloseTheTrainingCourse, Order = 6)]
    
    public class Course : Entity, IAggregateRoot
    {
        public Course()
        {
            Conditions = new List<CourseCondition>();
            CourseCosts = new List<CourseCost>();
            AppraisalCourses = new List<AppraisalCourse>();
            AppraisalTrainees = new List<AppraisalTrainee>();
            Attachments = new List<CourseAttachment>();
            CourseTrainingNeeds = new List<CourseTrainingNeed>();
            CourseEmployees = new List<CourseEmployee>();
        }

        #region Course information

        //اسم الدورة 
        [UserInterfaceParameter(Order = 2, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseInformation)]
        public virtual CourseName CourseName { get; set; }

        //اختصاص الدورة 
        [UserInterfaceParameter(Order = 4, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseInformation)]
        public virtual CourseSpecialize Specialize { get; set; }

        //نوع الدورة 
        [UserInterfaceParameter(Order = 6, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseInformation)]
        public virtual CourseType CourseType { get; set; }

        //اولوية الدورة 
        [UserInterfaceParameter(Order = 8, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseInformation)]
        public virtual Priority Priority { get; set; }

        //مستوى الدورة 
        [UserInterfaceParameter(Order = 10, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseInformation)]
        public virtual CourseLevel CourseLevel { get; set; }

        //عنوان الدورة 
        [UserInterfaceParameter(Order = 12, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseInformation)]
        public virtual string CourseTitle { get; set; }

        //لغة الدورة 
        [UserInterfaceParameter(Order = 14, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseInformation)]
        public virtual LanguageName LanguageName { get; set; }

        //هدف الدورة 
        [UserInterfaceParameter(Order = 16, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseInformation)]
        public virtual string CourseObjective { get; set; }

        //مدة الدورة 
        [UserInterfaceParameter(Order = 20, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseInformation)]
        public virtual int Duration { get; set; }

        //عدد الجلسات 
        [UserInterfaceParameter(Order = 22, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseInformation)]
        public virtual int NumberOfSession { get; set; }

        //عدد الموظفين 
        [UserInterfaceParameter(Order = 24, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseInformation)]
        public virtual int ExpectedNumberOfEmployees { get; set; }

        //توصيف 
        [UserInterfaceParameter(Order = 26, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseInformation)]
        public virtual string Description { get; set; }

        //حالة الدورة 
        [UserInterfaceParameter(Order = 28, IsNonEditable = true, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseInformation)]
        public virtual CourseStatus Status { get; set; }

        
        [UserInterfaceParameter(Order = 41, IsNonEditable = true, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseInformation)]
        public virtual string CancellationDescription { get; set; }

        //مدة الجلسة
        [UserInterfaceParameter(Order = 36, IsNonEditable = true, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseInformation)]
        public virtual double SessionDuration => (NumberOfSession > 0) ? (double)Duration / (double)NumberOfSession : 0;

        [UserInterfaceParameter(Order = 38, IsNonEditable = true, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseInformation)]
        public virtual double TraineeCost
        {
            get { return CourseCosts.Any() ? Math.Round(CourseCosts.Sum(x => x.CostPerTrainee), 2) : 0; }
        }
        
        [UserInterfaceParameter(Order = 39, IsNonEditable = true, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseInformation)]
        public virtual double CourseCost
        {
            get { return CourseCosts.Any() ? Math.Round(CourseCosts.Sum(item => item.Cost), 2) : 0; }
        }

        [UserInterfaceParameter(Order = 40, IsNonEditable = true, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseInformation)]
        public virtual int NumberOfCandidates
        {
            get { return CourseEmployees.Any() ? CourseEmployees.Count : 0; }
        }
        [UserInterfaceParameter(Order = 41, IsNonEditable = true, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseInformation)]
        public virtual int NumberOfTrainees
        {
            get { return CourseEmployees.Any() ? CourseEmployees.Count(x => x.Type == CourseEmployeeType.Trainee) : 0; }
        }
        #endregion

        #region Training Place info

        [UserInterfaceParameter(Order = 42,  Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.TrainingPlaceInfo)]
        public virtual CourseSponsor Sponsor { get; set; }

        [UserInterfaceParameter(Order = 44,  Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.TrainingPlaceInfo)]
        public virtual TrainingCenterName TrainingCenterName { get; set; }

        [UserInterfaceParameter(Order = 46,  Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.TrainingPlaceInfo)]
        public virtual TrainingPlace TrainingPlace { get; set; }

        [UserInterfaceParameter(Order = 48,  Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.TrainingPlaceInfo)]
        public virtual Trainer Trainer { get; set; }

        #endregion

        #region Course Time
        //تاريخ البدء المخطط
        [UserInterfaceParameter(Order = 32, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseTime)]
        public virtual DateTime PlannedStartDate { get; set; }

        //تاريخ الانتهاء المخطط
        [UserInterfaceParameter(Order = 34, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseTime)]
        public virtual DateTime PlannedEndDate { get; set; }

        [UserInterfaceParameter(Order = 50,  Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseTime)]
        public virtual DateTime? StartDate { get; set; }

        [UserInterfaceParameter(Order = 52,  Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseTime)]
        public virtual DateTime? EndDate { get; set; }

        [UserInterfaceParameter(Order = 57,  Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseTime)]
        public virtual bool Saturday { get; set; }

        [UserInterfaceParameter(Order = 58,  Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseTime)]
        public virtual bool Sunday { get; set; }

        [UserInterfaceParameter(Order = 60,  Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseTime)]
        public virtual bool Monday { get; set; }

        [UserInterfaceParameter(Order = 62,  Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseTime)]
        public virtual bool Tuesday { get; set; }

        [UserInterfaceParameter(Order = 64, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseTime)]
        public virtual bool Wednesday { get; set; }

        [UserInterfaceParameter(Order = 66, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseTime)]
        public virtual bool Thursday { get; set; }

        [UserInterfaceParameter(Order = 68,  Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseTime)]
        public virtual bool Friday { get; set; }

        [UserInterfaceParameter(Order = 54, IsTime = true, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseTime)]
        public virtual DateTime? StartHour { get; set; }

        [UserInterfaceParameter(Order = 56, IsTime = true, Group = TrainingGroupsNames.ResourceGroupName + "_" + TrainingGroupsNames.CourseTime, IsNonEditable = true)]
        public virtual DateTime? EndHour
        {
            get
            {
                DateTime? date = null;
                if (StartDate != null)
                {
                    var hours = Math.Truncate(SessionDuration);
                    var minutes = (SessionDuration - hours) * 60;
                    if(StartHour != null)
                        date = new DateTime(2000, 1, 1, (int) (hours + StartHour.Value.Hour), (int)(minutes + StartHour.Value.Minute) , 0);
                }

                return date;
            }
        }

        #endregion

        #region Reference
        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual TrainingPlan TrainingPlan { get; set; }
        #endregion

        #region List
        //شروط الدورة
        public virtual IList<CourseCondition> Conditions { get; set; }
        public virtual void AddCourseCondition(CourseCondition condition)
        {
            condition.Course = this;
            Conditions.Add(condition);
        }

        //الموظفين المرشحين والمتدربين
        public virtual IList<CourseEmployee> CourseEmployees { get; set; }
        public virtual void AddCourseEmployee(CourseEmployee employee)
        {
            employee.Course = this;
            CourseEmployees.Add(employee);
        }

        //حاجات تدريبية
        public virtual IList<CourseTrainingNeed> CourseTrainingNeeds { get; set; }
        public virtual void AddTrainingNeed(CourseTrainingNeed trainingNeed)
        {
            trainingNeed.Course = this;
            CourseTrainingNeeds.Add(trainingNeed);
        }
        //تكاليف الدورة
        public virtual IList<CourseCost> CourseCosts { get; set; }
        public virtual void AddCourseCost(CourseCost cost)
        {
            cost.Course = this;
            CourseCosts.Add(cost);
        }

        //تقييم الدورة
        public virtual IList<AppraisalCourse> AppraisalCourses { get; set; }
        public virtual void AddAppraisalCourse(AppraisalCourse appraisalCourse)
        {
            appraisalCourse.Course = this;
            AppraisalCourses.Add(appraisalCourse);
        }

        //تقييم المتدربين
        public virtual IList<AppraisalTrainee> AppraisalTrainees { get; set; }
        public virtual void AddAppraisalTrainee(AppraisalTrainee appraisalTrainee)
        {
            appraisalTrainee.Course = this;
            AppraisalTrainees.Add(appraisalTrainee);
        }

        public virtual IList<CourseAttachment> Attachments { get; set; }
        public virtual void AddCourseAttachments(CourseAttachment courseAttachments)
        {
            courseAttachments.Course = this;
            Attachments.Add(courseAttachments);
        }
        #endregion


        [UserInterfaceParameter(Order = 200, IsHidden = true)]
        public virtual string NameForDropdown
        {
            get
            {
                return CourseName.Name;
            }
        }

       
    }
}
