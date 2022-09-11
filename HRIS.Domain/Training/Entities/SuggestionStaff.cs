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
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Training.RootEntities;
using Souccar.Domain.DomainModel;
using System;

namespace HRIS.Domain.Training.Entities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class SuggestionStaff : Entity, IAggregateRoot
    {
        public  SuggestionStaff()
        {
            TestDate = DateTime.Today;
        }

        public virtual Employee Employee { get; set; }
        public virtual Course Course { get; set; }
        //public virtual EmployeeApprovalDefinition EmployeeApprovalDefinition { get; set; }
        public virtual float EnglishMark { get; set; }
        public virtual DateTime TestDate { get; set; }
        public virtual bool IsChecked { get; set; }
        public virtual bool IsAttendance { get; set; }
        //public virtual NewCourse NewCourse { get; set; }
    }
}
