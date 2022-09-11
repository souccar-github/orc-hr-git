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
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Workflow;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
namespace HRIS.Domain.EmployeeRelationServices.Configurations
{
    [Module(ModulesNames.EmployeeRelationServices)]
    [Order(32)]
    public class RewardSetting : Entity, IConfigurationRoot
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual string Name { get; set; }
        [UserInterfaceParameter(Order = 2)]
        public virtual RewardType RewardType { get; set; }
        [UserInterfaceParameter(Order = 3)]
        public virtual int Order1 { get; set; }
        [UserInterfaceParameter(Order = 4)]
        public virtual bool IsAddedToSalary { get; set; }
        [UserInterfaceParameter(Order = 5)]
        public virtual bool IsPercentage  { get; set; }
        [UserInterfaceParameter(Order = 6)]
        public virtual float FixedValue { get; set; }
        [UserInterfaceParameter(Order = 7)]
        public virtual float Percentage { get; set; }
        [UserInterfaceParameter(Order = 8)]
        public virtual string Description { get; set; }
        [UserInterfaceParameter(Order = 9, IsReference=true)]
        public virtual WorkflowSetting WorkflowSetting { get; set; }
    }
}