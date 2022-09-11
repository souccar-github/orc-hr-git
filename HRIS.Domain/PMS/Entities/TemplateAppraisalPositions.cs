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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Objectives.Entities;
using HRIS.Domain.Objectives.Enums;
using HRIS.Domain.PMS.RootEntities;
using Souccar.Domain.DomainModel;
using HRIS.Domain.PMS.Configurations;

namespace HRIS.Domain.PMS.Entities
{
    /// <summary>
    /// Ammar Alziebak
    /// </summary>
    public class TemplateAppraisalPositions : Entity,IAggregateRoot
    {
        public virtual Position Position { get; set; }
        public virtual AppraisalTemplate AppraisalTemplate { get; set; }
        //public virtual WorkflowApplyFlag WorkflowApplyFlag { get; set; }
        //public virtual int OperationNo { get; set; }
        public virtual AppraisalTemplateSetting AppraisalTemplateSetting { get; set; }
    }
}
