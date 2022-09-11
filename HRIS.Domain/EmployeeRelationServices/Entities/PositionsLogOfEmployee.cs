using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    public class PositionsLogOfEmployee : Entity, IAggregateRoot
    {
        public PositionsLogOfEmployee()
        {
            CreationDate = DateTime.Now;
        }

        #region Basic Info

        [UserInterfaceParameter(Order = 100, IsHidden = true, IsNonEditable = true)]
        public virtual Position Position { get; set; }
        [UserInterfaceParameter(Order = 1, IsReference = true, IsNonEditable = true)]
        public virtual HRIS.Domain.JobDescription.RootEntities.JobDescription JobDescription { get; set; }
        [UserInterfaceParameter(Order = 2,  IsNonEditable = true)]
        public virtual string Code { get { return Position != null ? Position.Code : ""; } }
        [UserInterfaceParameter(Order = 3, IsNonEditable = true)]
        public virtual AssigmentType AssigmentType { get; set; }
       
        [UserInterfaceParameter(Order = 4, IsNonEditable = true)]
        public virtual DateTime? AssigningDate { get; set; }
        [UserInterfaceParameter(Order = 5, IsNonEditable = true)]
        public virtual DisengagementType DisengagementType { get; set; }
        [UserInterfaceParameter(Order = 6, IsNonEditable = true)]
        public virtual DateTime? LeavingDate { get; set; }
        [UserInterfaceParameter(Order = 7, IsNonEditable = true)]
        public virtual DateTime CreationDate { get; set; }
        [UserInterfaceParameter(Order = 8, IsNonEditable = true)]
        public virtual bool IsPrimary { get; set; }

        [UserInterfaceParameter(Order = 10, IsNonEditable = true)]
        public virtual Employee Employee { get; set; }
        #endregion



    }
}
