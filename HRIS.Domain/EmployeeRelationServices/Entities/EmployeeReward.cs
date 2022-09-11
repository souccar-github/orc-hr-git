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
using HRIS.Domain.EmployeeRelationServices.Configurations;
#endregion
namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    /// <summary>
    /// منح مكافئة لموظف
    /// </summary>
    public class EmployeeReward : Entity, IAggregateRoot
    {
        public EmployeeReward()
        {
            //RewardDate = DateTime.Now;
            CreationDate = DateTime.Now;
        }

        #region Basic Info
        [UserInterfaceParameter(Order = 1, IsReference = true)]
        public virtual RewardSetting RewardSetting { get; set; }
        [UserInterfaceParameter(Order = 2)]
        public virtual DateTime RewardDate { get; set; }
        [UserInterfaceParameter(Order = 3)]
        public virtual string RewardReason { get; set; }
        [UserInterfaceParameter(Order = 4)]
        public virtual string Comment { get; set; }
        [UserInterfaceParameter(Order = 5)]
        public virtual EmployeeCard EmployeeCard { get; set; }
        [UserInterfaceParameter(Order = 6, IsNonEditable = true)]
        public virtual DateTime CreationDate { get; set; }
        [UserInterfaceParameter(Order = 7, IsReference = true, IsNonEditable = true)]
        public virtual User Creator { get; set; }
        [UserInterfaceParameter(Order = 8, IsHidden = true)]
        public virtual WorkflowItem WorkflowItem { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsTransferToPayroll { get; set; }
        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual Status RewardStatus { get; set; }
        #endregion

    }
}
