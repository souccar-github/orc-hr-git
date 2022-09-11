#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author:  
//description:
//start date:  
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
using HRIS.Domain.ProjectManagement.RootEntities;
using Souccar.Domain.DomainModel;
using HRIS.Domain.OrgChart.Entities;
using HRIS.Domain.JobDesc.Entities;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Domain.Security;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
#endregion
namespace HRIS.Domain.ProjectManagement.Entities
{
    public class EvaluateProject : Entity, IAggregateRoot
    {
        public EvaluateProject()
        {
            EvaluationDate = DateTime.Now;
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
        }

        #region Basic Info

        [UserInterfaceParameter(Order = 1)]
        public virtual DateTime EvaluationDate { get; set; }
        [UserInterfaceParameter(Order = 2)]
        public virtual DateTime FromDate { get; set; }
        [UserInterfaceParameter(Order = 3)]
        public virtual DateTime ToDate { get; set; }
        [UserInterfaceParameter(Order = 4)]
        public virtual Quarter Quarter { get; set; }
        [UserInterfaceParameter(Order = 5, IsHidden = true)]
        public virtual Project Project { get; set; }
        [UserInterfaceParameter(Order = 6, IsHidden = true)]
        public virtual Employee Evaluator { get; set; }
        [UserInterfaceParameter(Order = 7)]
        public virtual float TotalRate { get; set; }
        #endregion

    }
}
