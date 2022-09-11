#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 08/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace HRIS.Mapping.EmployeeRelationServices.Entities
{
    public sealed class EmployeeTransferMap : ClassMap<EmployeeTransfer>
    {
        public EmployeeTransferMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            //Map(x => x.IsPrimary);
            //Map(x => x.Weight);
            //Map(x => x.AssigningDate);
            Map(x => x.LeavingDate);
            Map(x => x.StartingDate);
            Map(x => x.TransferReason);
            Map(x => x.Comment);
            Map(x => x.CreationDate);

            References(x => x.SourcePosition);
            References(x => x.DestinationJobTitle);
            References(x => x.DestinationPosition);
            References(x => x.EmployeeCard).Column("EmployeeCard_Id");
            References(x => x.Creator);
        }
    }
}
