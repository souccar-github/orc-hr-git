#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//project manager:
//supervisor:
//author: Ammar Alziebak
//description:
//start date:
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.PMS.Entities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.PMS.Helpers;
using HRIS.Domain.PMS.Configurations;

#endregion

namespace HRIS.Domain.PMS.RootEntities
{
    //[Command(CommandsNames.UpdateAppraisalPhase, Order = 1)]
    /// <summary>
    /// Ammar Alziebak
    /// </summary>
    [Module(ModulesNames.PMS)]
    [Order(7)]
    public class AppraisalPhase : HRIS.Domain.Workflow.PhasePeriod
    {
        public AppraisalPhase()
        {
            PhaseWorkflows = new List<AppraisalPhaseWorkflow>();
        }

        #region Activation Details
      
        

        [UserInterfaceParameter(IsReference = true, Order = 7)]
        public virtual AppraisalTemplateSetting AppraisalTemplateSetting { get; set; }

        [UserInterfaceParameter(IsReference = true, Order = 8)]
        public virtual AppraisalPhaseSetting AppraisalPhaseSetting { get; set; }

        #endregion
    
        //[UserInterfaceParameter(IsHidden = true)]
        public virtual IList<AppraisalPhaseWorkflow> PhaseWorkflows { get; set; }
        public virtual void AddPhaseWorkflow(AppraisalPhaseWorkflow phaseWorkflow)
        {
            phaseWorkflow.AppraisalPhase = this;
            PhaseWorkflows.Add(phaseWorkflow);
        }
        
    }
}