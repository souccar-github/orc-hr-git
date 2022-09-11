#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 08/03/2015
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
#endregion

namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    /// <summary>
    /// انهاء خدمة موظف
    /// </summary>
    public class EmployeeTermination : Entity,IAggregateRoot
    {
        public EmployeeTermination()
        {
            this.CreationDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        }

        #region Basic Info
        [UserInterfaceParameter(Order = 1)]
        public virtual DateTime LastWorkingDate { get; set; }
        [UserInterfaceParameter(Order = 2)]
        public virtual string TerminationReason { get; set; }
        [UserInterfaceParameter(Order = 3)]
        public virtual string Comment { get; set; }
        [UserInterfaceParameter(Order = 4)]
        public virtual EmployeeCard EmployeeCard { get; set; }
        [UserInterfaceParameter(Order = 5, IsNonEditable = true)]
        public virtual DateTime CreationDate { get; set; }
        [UserInterfaceParameter(Order = 6, IsReference = true, IsNonEditable = true)]
        public virtual User Creator { get; set; }
        [UserInterfaceParameter(Order = 7, IsNonEditable = true)]
        public virtual bool HasExitInterView { get; set; }
        [UserInterfaceParameter(Order = 8, IsHidden = true)]
        public virtual WorkflowItem WorkflowItem { get; set; }
        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual Status TerminationStatus { get; set; }
        #endregion

    }
}
