#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 11/03/2015
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
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.Indexes;
#endregion

namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    /// <summary>
    /// ترقية مالية
    /// </summary>
    public class FinancialPromotion : Entity, IAggregateRoot
    {
        public FinancialPromotion()
        {
            CreationDate = DateTime.Now;
        }

        #region Basic Info
        [UserInterfaceParameter(Order = 5)]
        public virtual bool IsPercentage { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual float FixedValue { get; set; }

        [UserInterfaceParameter(Order = 15)]
        public virtual float Percentage { get; set; }

        [UserInterfaceParameter(Order = 17)]
        public virtual SalaryType Salary { get; set; }

        [UserInterfaceParameter(Order = 20)]
        public virtual FinancialPromotionReason FinancialPromotionReason { get; set; }

        [UserInterfaceParameter(Order = 25)]
        public virtual string Comment { get; set; }

        [UserInterfaceParameter(Order = 30, IsNonEditable = true)]
        public virtual float NewSalary { get; set; }

        [UserInterfaceParameter(Order = 35)]
        public virtual DateTime CreationDate { get; set; }

        [UserInterfaceParameter(Order = 36)]
        public virtual DateTime? ApprovedDate { get; set; }

        [UserInterfaceParameter(Order = 40)]
        public virtual EmployeeCard EmployeeCard { get; set; }

        [UserInterfaceParameter(Order = 45, IsReference = true, IsNonEditable = true)]
        public virtual User Creator { get; set; }

        [UserInterfaceParameter(Order = 50, IsHidden = true)]
        public virtual WorkflowItem WorkflowItem { get; set; }

        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual Status FinancialPromotionStatus { get; set; }
        #endregion

    }
}