#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 03/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference

using System;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Domain.DomainModel;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Personnel.Configurations;
using Souccar.Domain.Workflow.RootEntities;
using HRIS.Domain.Global.Enums;
#endregion
namespace HRIS.Domain.Personnel.Entities
{
    public class EmployeeCustodie : Entity, IAggregateRoot
    {
        public EmployeeCustodie()
        {
            CustodyStartDate = DateTime.Now;
            OperationDate = DateTime.Now;
        }
        [UserInterfaceParameter(Order = 1, IsReference=true)]
        public virtual CustodieDetails CustodyName { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual int Quantity { get; set; }
        [UserInterfaceParameter(Order = 3)]
        public virtual AdditionalInformation AdditionalInformation { get; set; }
        [UserInterfaceParameter(Order = 4)]
        public virtual string Note { get; set; }
        [UserInterfaceParameter(Order = 4)]
        public virtual EmployeeCard EmployeeCard { get; set; }
        [UserInterfaceParameter(Order = 5)]
        public virtual DateTime CustodyStartDate { get; set; }
        [UserInterfaceParameter(Order = 6)]
        public virtual DateTime? CustodyEndDate { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual WorkflowItem WorkflowItem { get; set; }
        [UserInterfaceParameter(Order = 7, IsNonEditable = true)]
        public virtual DateTime OperationDate { get; set; }
        [UserInterfaceParameter(Order = 8, IsNonEditable = true)]
        public virtual Souccar.Domain.Security.User OperationCreator { get; set; }
       
        [UserInterfaceParameter(Order = 9, IsNonEditable = true)]
        public virtual Global.Enums.Status CustodieStatus { get; set; }
    }
}
