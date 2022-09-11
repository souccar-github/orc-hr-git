using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.Recruitment.Entities.Evaluations;
using Souccar.Core;

namespace HRIS.Mapping.Recruitment.Entities
{
    public class InterviewCustomSectionMap:ClassMap<InterviewCustomSection>
    {
        public InterviewCustomSectionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Rate);
            Map(x => x.Weight);
            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);
            References(x => x.Evaluator);
            References(x => x.AppraisalSection);

            HasMany(x => x.InterviewCustomSectionItems).Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
