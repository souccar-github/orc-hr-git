#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 04/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Workflow;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

#endregion
namespace HRIS.Domain.EmployeeRelationServices.Configurations
{
    [Module(ModulesNames.EmployeeRelationServices)]
    [Order(31)]
    public class DisciplinarySetting : Entity, IConfigurationRoot
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual string Name { get; set; }
        [UserInterfaceParameter(Order = 2)]
        public virtual DisciplinaryType DisciplinaryType { get; set; }
        [UserInterfaceParameter(Order = 3)]
        public virtual int Order1 { get; set; }
        [UserInterfaceParameter(Order = 4)]
        public virtual bool IsDeductFromSalary  { get; set; }
        [UserInterfaceParameter(Order = 5)]
        public virtual DeductionType DeductionType { get; set; }
        [UserInterfaceParameter(Order = 6)]
        public virtual float Value { get; set; }
        [UserInterfaceParameter(Order = 8)]
        public virtual string Description { get; set; }
        [UserInterfaceParameter(Order = 9)]
        public virtual int DisciplinaryNumber { get; set; }
        [UserInterfaceParameter(Order = 10, IsReference=true)]
        public virtual WorkflowSetting WorkflowSetting { get; set; }
    }
}