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
    public sealed class EmployeeResignationMap : ClassMap<EmployeeResignation>
    {
        public EmployeeResignationMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.NoticeStartDate);
            Map(x => x.NoticeEndDate);
            Map(x => x.LastWorkingDate);
            Map(x => x.CreationDate);
            Map(x => x.ResignationReason);
            Map(x => x.Comment);
            Map(x => x.HasExitInterView);
            Map(x => x.ResignationStatus);

            References(x => x.WorkflowItem);
            References(x => x.EmployeeCard).Column("EmployeeCard_Id");
            References(x => x.Creator);

            HasMany(x => x.ResignationAttachments).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

        }
    }
}
