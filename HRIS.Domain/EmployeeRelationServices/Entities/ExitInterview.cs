#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 05/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Domain.Security;

#endregion
namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    /// <summary>
    /// مقابلة الانتهاء
    /// </summary>
    //[Module(ModulesNames.EmployeeRelationServices)]
    public class ExitInterview : Entity, IAggregateRoot
    {
        public ExitInterview()
        {
            InterviewAnswers = new List<ExitInterviewAnswer>();
        }

        [UserInterfaceParameter(Order = 10)]
        public virtual EmployeeCard EmployeeCard { get; set; }
        [UserInterfaceParameter(Order = 15, IsReference = true, IsNonEditable = true)]
        public virtual Employee Interviewer { get; set; }
        [UserInterfaceParameter(Order = 20)]
        public virtual EmployeeResignation EmployeeResignation { get; set; }
        [UserInterfaceParameter(Order = 30)]
        public virtual EmployeeTermination EmployeeTermination { get; set; }
        [UserInterfaceParameter(Order = 40)]
        public virtual DateTime InterviewDate { get; set; }

        #region Job Information
        public virtual DateTime WorkStartDate
        { get { return EmployeeCard.StartWorkingDate != null ? EmployeeCard.StartWorkingDate.GetValueOrDefault() : DateTime.Today; } }
        public virtual DateTime WorkEndDate
        { get { return EmployeeResignation != null ? EmployeeResignation.LastWorkingDate : EmployeeTermination != null ? EmployeeTermination.LastWorkingDate : DateTime.Today; } }
        public virtual string LeaveReason
        { get { return EmployeeResignation != null ? EmployeeResignation.ResignationReason : EmployeeTermination != null ? EmployeeTermination.TerminationReason : string.Empty; } }
        public virtual int Years { get { return ((int)(WorkEndDate - WorkStartDate).TotalDays) / 365; } }
        public virtual int Months { get { return (((int)(WorkEndDate - WorkStartDate).TotalDays % 365)) / 30; } }
        public virtual int Days { get { return (((int)(WorkEndDate - WorkStartDate).TotalDays % 365)) % 30; } }
        #endregion

        [UserInterfaceParameter(Order = 45, IsReference = true, IsNonEditable = true)]
        public virtual User Creator { get; set; }
        [UserInterfaceParameter(Order = 50)]
        public virtual IList<ExitInterviewAnswer> InterviewAnswers { get; set; }
        public virtual void AddExitInterviewAnswer(ExitInterviewAnswer InterviewAnswer)
        {
            InterviewAnswer.SurveyExitInterview = this;
            this.InterviewAnswers.Add(InterviewAnswer);
        }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return EmployeeCard.NameForDropdown; } }

    }
}
