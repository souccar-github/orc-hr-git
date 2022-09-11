#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 05/03/2015
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
using Souccar.Core;

#endregion
namespace HRIS.Mapping.EmployeeRelationServices.Entities
{
    public sealed class AssignmentMap : ClassMap<Assignment>
    {
        public AssignmentMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.AssigningDate);
            Map(x => x.CreationDate);
            Map(x => x.Comment).Nullable().Length(GlobalConstant.MultiLinesStringMaxLength);

            References(x => x.JobTitle);
            References(x => x.Position);
            References(x => x.EmployeeCard);
            References(x => x.Creator);
            References(x => x.AssigningEmployeeToPosition);
        }
    }
}