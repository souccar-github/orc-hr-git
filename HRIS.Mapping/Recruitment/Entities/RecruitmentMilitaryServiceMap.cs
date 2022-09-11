using FluentNHibernate.Mapping;
using HRIS.Domain.Recruitment.Entities;
using Souccar.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.Recruitment.Entities
{
    public class RecruitmentMilitaryServiceMap:ClassMap<RecruitmentMilitaryService>
    {
        public RecruitmentMilitaryServiceMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Status);
            Map(x => x.IsPermamentExemption);
            Map(x => x.ExemptionReason);
            Map(x => x.DateOfExemption).Nullable();
            Map(x => x.DelayReason);
            Map(x => x.DateOfDelay).Nullable();
            Map(x => x.SendDelayExpirationNotification);
            Map(x => x.MilitiryServiceNo).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.MilitiryServiceDocIssuance).Nullable();

            Map(x => x.Months);
            Map(x => x.Years);
            Map(x => x.Days);

            Map(x => x.ServiceStartDate).Nullable();
            Map(x => x.ServiceEndDate).Nullable();
            Map(x => x.HoldDate).Nullable();
            Map(x => x.ReserveStartDate).Nullable();
            Map(x => x.Notes).Length(GlobalConstant.MultiLinesStringMaxLength);

            References(x => x.Granter);
            References(x => x.JobApplication);
        }
    }
}
