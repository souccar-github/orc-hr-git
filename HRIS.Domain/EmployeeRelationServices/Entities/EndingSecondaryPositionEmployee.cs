using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.JobDescription.Entities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    public class EndingSecondaryPositionEmployee : Entity, IAggregateRoot
    {
        public EndingSecondaryPositionEmployee()
        {
            this.CreationDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        }

        #region Basic Info
        [UserInterfaceParameter(IsHidden = true)]
        public virtual Position Position { get; set; }
        [UserInterfaceParameter(Order = 1)]
        public virtual string JobDescription
        {
            get
            {
                if (Position == null)
                    return string.Empty;
                if (Position.JobDescription == null)
                    return string.Empty;
                return Position.JobDescription.Name ?? string.Empty;
            }
        }
        [UserInterfaceParameter(Order = 4)]
        public virtual DateTime LeavingDate { get; set; }
        [UserInterfaceParameter(Order = 5)]
        public virtual string PositionCode
        {
            get
            {
                if (Position != null)
                    return Position.Code ?? string.Empty;
                return string.Empty;
            }
        }

        [UserInterfaceParameter(Order = 6)]
        public virtual string Comment { get; set; }
        [UserInterfaceParameter(Order = 7)]
        public virtual EmployeeCard EmployeeCard { get; set; }
        [UserInterfaceParameter(Order = 8, IsNonEditable = true)]
        public virtual DateTime CreationDate { get; set; }
        [UserInterfaceParameter(Order = 9, IsReference = true, IsNonEditable = true)]
        public virtual User Creator { get; set; }
        
        [UserInterfaceParameter(Order = 10)]
        public virtual string JobTitle
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
        [UserInterfaceParameter(Order = 12)]
        public virtual string Grade
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
        [UserInterfaceParameter(Order = 13)]
        public virtual string OrganizationalLevel
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
        [UserInterfaceParameter(Order = 14)]
        public virtual string Node
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
        #endregion

    }
}
