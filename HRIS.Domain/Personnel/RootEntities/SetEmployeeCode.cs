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
    [Order(2)]
    public class SetEmployeeCode : Entity, IAggregateRoot
    {
        public SetEmployeeCode()
        {

        }

        [UserInterfaceParameter(Order = 1)]
        public virtual string FixedPrefix { get; set; }
        [UserInterfaceParameter(Order = 2)]
        public virtual string FixedSuffix { get; set; }
        [UserInterfaceParameter(Order = 3)]
        public virtual CustomType CustomPrefix { get; set; }
        [UserInterfaceParameter(Order = 4)]
        public virtual int CustomPrefixLength { get; set; }
        [UserInterfaceParameter(Order = 5)]
        public virtual int CustomPrefixStartingPosition { get; set; }
        [UserInterfaceParameter(Order = 6)]
        public virtual CustomType CustomSuffix { get; set; }
        [UserInterfaceParameter(Order = 7)]
        public virtual int CustomSuffixLength { get; set; }
        [UserInterfaceParameter(Order = 8)]
        public virtual int CustomSuffixStartingPosition { get; set; }
        [UserInterfaceParameter(Order = 9)]
        public virtual SeparatorSymbol SeparatorSymbol { get; set; }
    }
}
