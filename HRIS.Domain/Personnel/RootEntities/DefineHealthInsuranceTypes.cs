#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 26/02/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.Helpers;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
namespace HRIS.Domain.Personnel.RootEntities
{
    //[Module(ModulesNames.Personnel)]
    [Order(3)]
    public class DefineHealthInsuranceTypes : Entity, IAggregateRoot
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual string InsuranceType { get; set; }
        [UserInterfaceParameter(Order = 2)]
        public virtual int Percentage { get; set; }
        [UserInterfaceParameter(Order = 3)]
        public virtual bool WithSpouse { get; set; }
        [UserInterfaceParameter(Order = 4)]
        public virtual bool WithChildren { get; set; }
        [UserInterfaceParameter(Order = 5)]
        public virtual string Description { get; set; }
    }
}
