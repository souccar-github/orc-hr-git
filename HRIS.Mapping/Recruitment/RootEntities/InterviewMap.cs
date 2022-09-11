using FluentNHibernate.Mapping;
using HRIS.Domain.Recruitment.RootEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Core;

namespace HRIS.Mapping.Recruitment.RootEntities
{
    public class InterviewMap : ClassMap<Interview>
    {
        public InterviewMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.InterviewDate);
            Map(x => x.InterviewStartingTime);
            Map(x => x.InterviewEndTime);
            //Map(x => x.SubTopic);
            Map(x => x.InterviewGuidelines).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.FinalMark);

            References(x => x.InterviewType);
            References(x => x.InterviewAppraisalSetting);
            References(x => x.InterviewAppraisalTemplate);
            References(x => x.WorkflowItem);
            References(x => x.JobApplication);

            HasMany(x => x.Evaluators).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}