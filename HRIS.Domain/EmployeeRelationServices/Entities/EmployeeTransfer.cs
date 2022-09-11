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
    /// نقل موظف
    /// </summary>
    public class EmployeeTransfer : Entity,IAggregateRoot
    {
        public EmployeeTransfer()
        {
            this.CreationDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        }

        #region Basic Info
        [UserInterfaceParameter(Order = 1, IsHidden = true)]
        public virtual Position SourcePosition { get; set; }
        [UserInterfaceParameter(Order = 2, IsReference = true)]
        public virtual JobTitle DestinationJobTitle { get; set; }
        [UserInterfaceParameter(Order = 3, IsHidden = true)]
        public virtual Position DestinationPosition { get; set; }
        
        [UserInterfaceParameter(Order = 8)]
        public virtual EmployeeCard EmployeeCard { get; set; }
        [UserInterfaceParameter(Order = 9)]
        public virtual string PositionCode
        {
            get
            {
                if (SourcePosition != null)
                    return SourcePosition.Code ?? string.Empty;
                return string.Empty;
            }
        }
        [UserInterfaceParameter(Order = 10)]
        public virtual string JobDescriptionName
        {
            get
            {
                if (SourcePosition == null)
                    return string.Empty;
                if (SourcePosition.JobDescription == null)
                    return string.Empty;
                return SourcePosition.JobDescription.Name ?? string.Empty;
            }
        }
        [UserInterfaceParameter(Order = 11)]
        public virtual string JobTitleName
        {
            get
            {
                if (SourcePosition == null)
                    return string.Empty;
                if (SourcePosition.JobDescription == null)
                    return string.Empty;
                if (SourcePosition.JobDescription.JobTitle == null)
                    return string.Empty;
                return SourcePosition.JobDescription.JobTitle.Name ?? string.Empty;
            }
        }
        [UserInterfaceParameter(Order = 12)]
        public virtual string GradeName
        {
            get
            {
                if (SourcePosition == null)
                    return string.Empty;
                if (SourcePosition.JobDescription == null)
                    return string.Empty;
                if (SourcePosition.JobDescription.JobTitle == null)
                    return string.Empty;
                if (SourcePosition.JobDescription.JobTitle.Grade == null)
                    return string.Empty;
                return SourcePosition.JobDescription.JobTitle.Grade.Name ?? string.Empty;
            }
        }
        [UserInterfaceParameter(Order = 13)]
        public virtual string OrganizationalLevelName
        {
            get
            {
                if (SourcePosition == null)
                    return string.Empty;
                if (SourcePosition.JobDescription == null)
                    return string.Empty;
                if (SourcePosition.JobDescription.JobTitle == null)
                    return string.Empty;
                if (SourcePosition.JobDescription.JobTitle.Grade == null)
                    return string.Empty;
                if (SourcePosition.JobDescription.JobTitle.Grade.OrganizationalLevel == null)
                    return string.Empty;
                return SourcePosition.JobDescription.JobTitle.Grade.OrganizationalLevel.Name ?? string.Empty;
            }
        }
        [UserInterfaceParameter(Order = 14)]
        public virtual string NodeName
        {
            get
            {
                if (SourcePosition == null)
                    return string.Empty;
                if (SourcePosition.JobDescription == null)
                    return string.Empty;
                if (SourcePosition.JobDescription.Node == null)
                    return string.Empty;
                return SourcePosition.JobDescription.Node.Name ?? string.Empty;
            }
        }
        [UserInterfaceParameter(Order = 15)]
        public virtual DateTime LeavingDate { get; set; }
        [UserInterfaceParameter(Order = 16)]
        public virtual DateTime StartingDate { get; set; }
        [UserInterfaceParameter(Order = 17)]
        public virtual string TransferReason { get; set; }
        [UserInterfaceParameter(Order = 18)]
        public virtual string Comment { get; set; }
        [UserInterfaceParameter(Order = 19, IsNonEditable = true)]
        public virtual DateTime CreationDate { get; set; }
        [UserInterfaceParameter(Order = 20, IsReference = true, IsNonEditable = true)]      
        public virtual User Creator { get; set; }
        #endregion

    }
}
