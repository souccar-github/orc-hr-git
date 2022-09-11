using FluentNHibernate.Mapping;
using HRIS.Domain.Recruitment.RootEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.Recruitment.RootEntities
{
    public class RecruitmentRequestMap : ClassMap<RecruitmentRequest>
    {
        public RecruitmentRequestMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.RequestDate);
            Map(x => x.PositionBudget);
            Map(x => x.SalaryRange);
            Map(x => x.ExpectedHiringDate);
            Map(x => x.DurationToFillPosition);
            Map(x => x.RequestStatus);
            Map(x => x.RequestCode);

            References(x => x.RequestType);
            References(x => x.VacancyReason);
            References(x => x.JobType);
            References(x => x.RequestedPosition);
            References(x => x.Requester);
            References(x => x.RequesterPosition);

            HasMany(x => x.RecruitmentRequestAttachments).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}