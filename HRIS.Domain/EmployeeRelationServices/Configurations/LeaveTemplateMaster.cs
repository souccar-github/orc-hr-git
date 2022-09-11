
using System;
using System.Collections.Generic;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Workflow;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.EmployeeRelationServices.Configurations
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>
    

    [Module(ModulesNames.EmployeeRelationServices)]
    [Order(23)]
    public class LeaveTemplateMaster : Entity, IConfigurationRoot
    {
        #region Basic Info

        public LeaveTemplateMaster()
        {
            InsertDate = DateTime.Now;
            LeaveTemplateDetails = new List<LeaveTemplateDetail>();
            
        }

        [UserInterfaceParameter(Order = 5)]
        public virtual string Name { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual string Description { get; set; }

        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual DateTime InsertDate { get; protected set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Name; } }
        #endregion

        #region References

        public virtual IList<LeaveTemplateDetail> LeaveTemplateDetails { get; set; }
        public virtual void AddLeaveTemplateDetail(LeaveTemplateDetail leaveTemplateDetail)
        {
            leaveTemplateDetail.LeaveTemplateMaster = this;
            LeaveTemplateDetails.Add(leaveTemplateDetail);
        }

        #endregion

    }
}
