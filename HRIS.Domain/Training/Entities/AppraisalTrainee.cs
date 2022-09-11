using System.Collections.Generic;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Training.Indexes;
using HRIS.Domain.Training.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;

namespace HRIS.Domain.Training.Entities
{
    public class AppraisalTrainee :Entity
    {
        public AppraisalTrainee()
        {
            Attachment = new List<AppraisalTraineeAttachment>();
        }

        [UserInterfaceParameter(Order = 1, IsReference = true, ReferenceReadUrl = "Training/Reference/GetAllTrainees")]
        public virtual Employee Employee { get; set; }
        [UserInterfaceParameter(Order = 2,IsNonEditable =true)]
        public virtual string FatherName
        {
            get
            {
                return Employee.FatherName;
            }

        }
        [UserInterfaceParameter(Order = 2)]
        public virtual DateTime ExamDate { get; set; }

        [UserInterfaceParameter(Order = 3)]
        public virtual int Score { get; set; }

        [UserInterfaceParameter(Order = 4)]
        public virtual AppraisalTraineeLevel Level { get; set; }

        [UserInterfaceParameter(Order = 5)]
        public virtual int NumberOfHoursOfAbsence { get; set; }

        [UserInterfaceParameter(Order = 6)]
        public virtual string AbsenceReason { get; set; }

        [UserInterfaceParameter(Order = 8)]
        public virtual bool ResponsibilityOfEmployee { get; set; }

        [UserInterfaceParameter(Order = 9)]
        public virtual string TrainerNote { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual double AttendanceRate
        {
            get
            {
                if (Course != null)
                {
                    return Math.Round(Course.Duration > 0
                        ? (((double)Course.Duration - (double)NumberOfHoursOfAbsence) / (double)Course.Duration) * 100
                        : 0.0, 2);
                }

                return 0;
            }
        }


        [UserInterfaceParameter(Order = 11)]
        public virtual bool TraineeAttendedTheCourse => AttendanceRate >= 60 ? true : false;

        public virtual Course Course { get; set; }
 
        public virtual IList<AppraisalTraineeAttachment> Attachment { get; set; }
        public virtual void AddCourseAppraisalTraineeAttachment(AppraisalTraineeAttachment courseAppraisalTraineeAttachment)
        {
            courseAppraisalTraineeAttachment.AppraisalTrainee = this;
            Attachment.Add(courseAppraisalTraineeAttachment);
        }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown => Employee?.FullName;
    }
}
