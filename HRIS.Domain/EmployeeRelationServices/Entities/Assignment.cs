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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;

using HRIS.Domain.JobDescription.Entities;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Domain.Security;
using Souccar.Infrastructure.Core;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Grades.Entities;
#endregion

namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    /// <summary>
    /// تعيين موظف
    /// </summary>
    public class Assignment : Entity ,IAggregateRoot
    {
        public Assignment()
        {
            this.CreationDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        }

        #region Basic Info
        [UserInterfaceParameter(Order = 1,IsReference=true)]
        public virtual JobTitle JobTitle { get; set; }
        [UserInterfaceParameter(Order = 2, IsHidden = true)]
        public virtual Position Position { get; set; }
        [UserInterfaceParameter(Order = 3)]
        public virtual EmployeeCard EmployeeCard { get; set; }
        [UserInterfaceParameter(Order = 4)]
        public virtual DateTime AssigningDate { get; set; }
        [UserInterfaceParameter(Order = 8)]
        public virtual string PositionCode
        {
            get
            {
                if (Position != null)
                    return Position.Code ?? string.Empty;
                return string.Empty;
            }
        }
        [UserInterfaceParameter(Order = 9)]
        public virtual string JobDescriptionName
        {
            get
            {
                if (Position==null)
                    return string.Empty;
                if (Position.JobDescription == null)
                    return string.Empty;
                return Position.JobDescription.Name ?? string.Empty;
            }
        }
        [UserInterfaceParameter(Order = 10)]
        public virtual string JobTitleName
        {
            get
            {
                if (Position == null)
                    return string.Empty;
                if (Position.JobDescription == null)
                    return string.Empty;
                if (Position.JobDescription.JobTitle == null)
                    return string.Empty;
                return Position.JobDescription.JobTitle.Name ?? string.Empty;
            }
        }
        [UserInterfaceParameter(Order = 11)]
        public virtual string GradeName
        {
            get
            {
                if (Position == null)
                    return string.Empty;
                if (Position.JobDescription == null)
                    return string.Empty;
                if (Position.JobDescription.JobTitle == null)
                    return string.Empty;
                if (Position.JobDescription.JobTitle.Grade == null)
                    return string.Empty;
                return Position.JobDescription.JobTitle.Grade.Name ?? string.Empty;
            }
        }
        [UserInterfaceParameter(Order = 12)]
        public virtual string OrganizationalLevelName
        {
            get
            {
                if (Position == null)
                    return string.Empty;
                if (Position.JobDescription == null)
                    return string.Empty;
                if (Position.JobDescription.JobTitle == null)
                    return string.Empty;
                if (Position.JobDescription.JobTitle.Grade == null)
                    return string.Empty;
                if (Position.JobDescription.JobTitle.Grade.OrganizationalLevel == null)
                    return string.Empty;
                return Position.JobDescription.JobTitle.Grade.OrganizationalLevel.Name ?? string.Empty;
            }
        }
        [UserInterfaceParameter(Order = 13)]
        public virtual string NodeName
        {
            get
            {
                if (Position == null)
                    return string.Empty;
                if (Position.JobDescription == null)
                    return string.Empty;
                if (Position.JobDescription.Node == null)
                    return string.Empty;
                return Position.JobDescription.Node.Name ?? string.Empty;
            }
        }
        [UserInterfaceParameter(Order = 14)]
        public virtual string StepName
        {
            get
            {
                if (Position == null)
                    return string.Empty;
                if (Position.Step == null)
                    return string.Empty;
                return Position.Step != null ? Position.Step.Name: string.Empty;
            }
        }

        [UserInterfaceParameter(Order = 15)]
        public virtual bool IsAssigned
        {
            get
            {
                return AssigningEmployeeToPosition == null ? false : true;
            }
        }
        [UserInterfaceParameter(Order = 15)]
        public virtual DateTime? LeavingDate
        {
            get
            {
                return EmployeeCard != null && AssigningEmployeeToPosition == null ? 
                    (EmployeeCard.EndingSecondaryPositions.Any(x=> x.Position.Id == Position.Id) ?
                     EmployeeCard.EndingSecondaryPositions.FirstOrDefault(x => x.Position.Id == Position.Id).LeavingDate : 
                     EmployeeCard.EndWorkingDate) : null;
            }
        }
        [UserInterfaceParameter(Order = 16)]
        public virtual string Comment { get; set; }
        [UserInterfaceParameter(Order = 17, IsReference = true, IsNonEditable = true)]
        public virtual User Creator { get; set; }
        [UserInterfaceParameter(Order = 18, IsNonEditable = true)]
        public virtual DateTime CreationDate { get; set; }
        [UserInterfaceParameter(Order = 19)]
        public virtual AssigningEmployeeToPosition AssigningEmployeeToPosition { get; set; }
        #endregion

    }
}
