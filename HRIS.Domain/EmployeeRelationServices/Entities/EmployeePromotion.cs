#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 09/03/2015
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
using HRIS.Domain.Global.Enums;
using Souccar.Domain.DomainModel;

using HRIS.Domain.JobDescription.Entities;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Domain.Security;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Grades.Entities;
#endregion

namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    /// <summary>
    /// ترقية موظف
    /// </summary>
    public class EmployeePromotion : Entity,IAggregateRoot
    {
        public EmployeePromotion()
        {
            this.CreationDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        }

        #region Basic Info
        [UserInterfaceParameter(Order = 1, IsReference = true)]
        public virtual JobTitle JobTitle { get; set; }
        [UserInterfaceParameter(Order = 2, IsHidden = true)]
        public virtual Position Position { get; set; }
        [UserInterfaceParameter(Order = 3)]
        public virtual DateTime PositionSeparationDate { get; set; }
        [UserInterfaceParameter(Order = 4)]
        public virtual DateTime PositionJoiningDate { get; set; }
        [UserInterfaceParameter(Order = 5)]
        public virtual string PromotionReason { get; set; }
        [UserInterfaceParameter(Order = 6)]
        public virtual string Comment { get; set; }
        [UserInterfaceParameter(Order = 7)]
        public virtual EmployeeCard EmployeeCard { get; set; }
        [UserInterfaceParameter(Order = 8, IsNonEditable = true)]
        public virtual DateTime CreationDate { get; set; }
        [UserInterfaceParameter(Order = 9, IsReference = true, IsNonEditable = true)]
        public virtual User Creator { get; set; }
        [UserInterfaceParameter(Order = 10, IsHidden = true)]
        public virtual WorkflowItem WorkflowItem { get; set; }
        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual Status PromotionStatus { get; set; }
        #endregion

    }
}