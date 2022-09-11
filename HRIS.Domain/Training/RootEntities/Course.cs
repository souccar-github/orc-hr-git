//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//project manager:
//supervisor:
//author: Yaseen Alrefaee
//description:
//start date:
//end date:
//last update:
//update by: Ammar Alziebak
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
using System;
using System.Collections.Generic;
using FluentNHibernate.Data;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.JobDesc.Indexes;
using HRIS.Domain.OrgChart.Indexes;
using HRIS.Domain.Training.Entities;
using HRIS.Domain.Training.Enums;
using HRIS.Domain.Training.Indexes;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.Attachment;
using Souccar.Domain.Attachment.Entities;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Personnel.Indexes;
using Entity = Souccar.Domain.DomainModel.Entity;
using HRIS.Domain.PMS.Helpers;
using HRIS.Domain.OrgChart.Entities;
using HRIS.Domain.JobDesc.RootEntities;
using HRIS.Domain.OrgChart.RootEntities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using Souccar.Infrastructure.Services;

namespace HRIS.Domain.Training.RootEntities
{
    [Command(CommandsNames.ActivateTrainingCourse, Order = 1)]
    [Command(CommandsNames.AddSuggestionStaffActionListHandler, Order = 2)]
    [Command(CommandsNames.ActiveCourseActionListHandler, Order = 3)]
    [Command(CommandsNames.SetApprovalDefinitionActionListHandler, Order = 4)]
    [Command(CommandsNames.TrainingCourseCancellation, Order = 5)]
    [Command(CommandsNames.CloseTrainingCourseActionListHandler, Order = 6)]
    //[Command(CommandsNames.PlannedCourseActionListHandler, Order = 3)]
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    [Module(ModulesNames.Training)]
    [Order(4)]
    public class Course : Entity, IAggregateRoot
    {
        public Course()
        {
            Conditions = new List<CourseCondition>();
            SuggestionStaff = new List<SuggestionStaff>();
            InvolvedStaff = new List<InvolvedStaff>();
            CourseCosts = new List<CourseCost>();
            CourseAppraisals = new List<AppraisalCourse>();
            AppraisalTrainees = new List<AppraisalTrainee>();
            Attachment = new List<CourseAttachments>();

        }

        #region CourseInformation

        //اسم الدورة 
        [UserInterfaceParameter(Order = 1, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseInformation, IsReference = true)]
        public virtual CourseName CourseName { get; set; }

        [UserInterfaceParameter(Order = 2, IsNonEditable = true)]
        public virtual string CourseNameL2
        {
            get
            {
                return (CourseName != null) ? CourseName.EnglishName : string.Empty;
            }
        }
        //اختصاص الدورة 
        [UserInterfaceParameter(Order = 3, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseInformation)]
        public virtual CourseSpecialize Specialize { get; set; }
        //نوع الدورة 
        [UserInterfaceParameter(Order = 4, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseInformation)]
        public virtual CourseType Type { get; set; }
        //أهمية الدورة 
        [UserInterfaceParameter(Order = 5, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseInformation)]
        public virtual Priority Priority { get; set; }
        //مستوى الدورة 
        [UserInterfaceParameter(Order = 6, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseInformation)]
        public virtual CourseLevel Level { get; set; }
        //عنوان الدورة 
        [UserInterfaceParameter(Order = 7, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseInformation)]
        public virtual string CourseTitles { get; set; }
        //لغة الدورة 
        [UserInterfaceParameter(Order = 8, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseInformation)]
        public virtual LanguageName LanguageName { get; set; }
        //مدة الدورة 
        [UserInterfaceParameter(Order = 9, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseInformation)]
        public virtual int Duration { get; set; }
        //عدد الجلسات 
        [UserInterfaceParameter(Order = 10, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseInformation)]
        public virtual int NumberOfSession { get; set; }
        //حالة الدورة 
        [UserInterfaceParameter(Order = 11, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseInformation)]
        public virtual CourseStatus Status { get; set; }
        //هدف الدورة 
        [UserInterfaceParameter(Order = 12, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseInformation)]
        public virtual string CourseObjective { get; set; }
        //عدد الموظفين 
        [UserInterfaceParameter(Order = 13, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseInformation)]
        public virtual int NumberOfEmployees { get; set; }
        [UserInterfaceParameter(Order = 14, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseInformation)]
        public virtual TimeInterval Per { get; set; }
        //مسار الملف 
        [UserInterfaceParameter(Order = 15, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseInformation)]
        public virtual string TitleFilePath { get; set; }
        //سبب إلغاء الدورة 
        [UserInterfaceParameter(Order = 16, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseInformation)]
        public virtual string CancelDescription { get; set; }
        //todo عدد الساعات التدريبية /  عدد الجلسات المقترحة
        public virtual float OnceSessionLength
        {
            get { return (NumberOfSession > 0) ? (float)Math.Round(Duration / (float)NumberOfSession, 2) : 0; }
        }
        //توصيف 
        [UserInterfaceParameter(Order = 17, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseInformation)]
        public virtual string Description { get; set; }

        #endregion

        #region Training Place info

        [UserInterfaceParameter(Order = 18, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.TrainingPlaceInfo)]
        public virtual CourseSponsor Sponsor { get; set; }
        [UserInterfaceParameter(Order = 19, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.TrainingPlaceInfo)]
        //جغرافية الدورة 
        //دورة داخلية أو خارجية
        public virtual CoursePlaceStatus PlaceStatus { get; set; }
        //مكان الدورة
        [UserInterfaceParameter(Order = 20, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.TrainingPlaceInfo)]
        public virtual TrainingPlace TrainingPlace { get; set; }
        [UserInterfaceParameter(Order = 21, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.TrainingPlaceInfo)]
        public virtual TrainingCenterName TrainingCenterName { get; set; }
        [UserInterfaceParameter(Order = 22, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.TrainingPlaceInfo)]
        public virtual Trainer Trainer { get; set; }
        [UserInterfaceParameter(Order = 23, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.TrainingPlaceInfo)]
        public virtual Country InvitationCountry { get; set; }
        [UserInterfaceParameter(Order = 24, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.TrainingPlaceInfo)]
        public virtual DateTime? InvitationDate { get; set; }
        [UserInterfaceParameter(Order = 25, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.TrainingPlaceInfo)]
        public virtual string InvitationDescription { get; set; }


        #endregion

        #region Course Time

        [UserInterfaceParameter(Order = 26, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseTime)]
        public virtual DateTime? StartDate { get; set; }
        [UserInterfaceParameter(Order = 27, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseTime)]
        public virtual DateTime? EndDate { get; set; }
        [UserInterfaceParameter(Order = 28, IsTime = true, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseTime)]
        public virtual DateTime? StartHour { get; set; }
        [UserInterfaceParameter(Order = 29, IsTime = true, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseTime, IsNonEditable = true)]
        public virtual DateTime? EndHour { get; set; }
        [UserInterfaceParameter(Order = 30, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseTime)]
        public virtual bool Saturday { get; set; }
        [UserInterfaceParameter(Order = 31, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseTime)]
        public virtual bool Sunday { get; set; }
        [UserInterfaceParameter(Order = 32, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseTime)]
        public virtual bool Monday { get; set; }
        [UserInterfaceParameter(Order = 33, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseTime)]
        public virtual bool Tuesday { get; set; }
        [UserInterfaceParameter(Order = 34, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseTime)]
        public virtual bool Wednesday { get; set; }
        [UserInterfaceParameter(Order = 35, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseTime)]
        public virtual bool Thursday { get; set; }
        [UserInterfaceParameter(Order = 36, Group = TrainingGoupesNames.ResourceGroupName + "_" + TrainingGoupesNames.CourseTime)]
        public virtual bool Friday { get; set; }

        #endregion

        #region ApprovalDefinition

        //نوع الموافقة 
        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual DocumentType DocumentType { get; set; }
        //رقم الموافقة
        //نوع الموافقة 
        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual string DocumentNumber { get; set; }
        //تاريخ  الموافقة
        //نوع الموافقة 
        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual DateTime? DocumentDate { get; set; }
        //توصيف
        //نوع الموافقة 
        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual string ApprovalDescription { get; set; }

        #endregion

        #region Ref
        [UserInterfaceParameter(IsReference = true, IsNonEditable = true)]
        public virtual TrainingNeed NeedsPool { get; set; }
        public virtual TrainingPlan Plan { get; set; }

        public virtual IList<CourseCondition> Conditions { get; set; }
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

        public virtual IList<CourseAttachments> Attachment { get; set; }
        public virtual void AddCourseAttachments(CourseAttachments courseAttachments)
        {
            courseAttachments.Course = this;
            Attachment.Add(courseAttachments);
        }
        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual double TraineeCost {
            get
            {

                return Math.Round(CourseCosts.Sum(x => x.CostPerTrainee), 2);
            }
        }
        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual double CourseCost
        {
            get
            { return Math.Round(CourseCosts.Sum(item => item.TotalCostInDefaultCurrency),2); }
        }
        #endregion
        [UserInterfaceParameter(Order = 200, IsHidden = true)]
        public virtual string NameForDropdown
        {
            get
            {
                return string.Format("{0},{1}", CourseName.NameForDropdown, Type != null ? Type.Name : string.Empty);
            }
        }
    }






}
