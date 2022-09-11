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
using HRIS.Domain.Personnel.Helpers;
using HRIS.Domain.PMS.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Workflow.RootEntities;
using HRIS.Domain.Personnel.RootEntities;

namespace HRIS.Domain.PMS.Entities
{
    //Ammar Alziebak
    public class AppraisalPhaseWorkflow : Entity, IAggregateRoot
    {
        public AppraisalPhaseWorkflow()
        {
            Appraisals = new List<Appraisal>();
        }
        [UserInterfaceParameter(Order = 1,IsHidden=true)]
        public virtual WorkflowItem WorkflowItem { get; set; }
        [UserInterfaceParameter(Order = 2, IsHidden = true)]
        public virtual Employee Employee { get; set; }
       
            [UserInterfaceParameter(Order = 2, IsHidden = true)]
        public virtual Position Position { get; set; }
        [UserInterfaceParameter(Order = 3, IsHidden = true)]
        public virtual AppraisalPhase AppraisalPhase { get; set; }
       
        public virtual string EmployeeName
        {
            get
            {
               
                return Employee.FullName;
            }

        }
    public virtual string FatherName
        {
            get
            {
                return Employee.FatherName;
            }
        }
        public virtual float FinalMark
        {
            get
            {
                return Appraisals.Count == 0 ? 0 : Appraisals.Sum(x => x.AppraisalValue) / (float)Appraisals.Count;
            }
        }
        public virtual float MinMark
        {
            get
            {
                return Appraisals.Count == 0 ? 0 : Appraisals.Min(x => x.AppraisalValue);
            }
        }
        public virtual float MaxMark
        {
            get
            {
                return Appraisals.Count == 0 ? 0 : Appraisals.Max(x => x.AppraisalValue) ;
            }
        }

        public virtual IList<Appraisal> Appraisals { get; set; }
        public virtual void AddAppraisal(Appraisal appraisal)
        {
            appraisal.PhaseWorkflow = this;
            Appraisals.Add(appraisal);
        }

    }
}
