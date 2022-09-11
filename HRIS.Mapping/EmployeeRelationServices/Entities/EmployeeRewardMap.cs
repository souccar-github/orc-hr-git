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
using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
namespace HRIS.Mapping.EmployeeRelationServices.Entities
{
    public sealed class EmployeeRewardMap : ClassMap<EmployeeReward>
    {
        public EmployeeRewardMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.RewardDate);
            Map(x => x.RewardReason);
            Map(x => x.Comment);
            Map(x => x.CreationDate);
            Map(x => x.RewardStatus);
            Map(x => x.IsTransferToPayroll).Default("0").Not.Nullable();

            References(x => x.WorkflowItem);
            References(x => x.Creator);
            References(x => x.RewardSetting);
            References(x => x.EmployeeCard).Column("EmployeeCard_Id");

        }
    }
}
