using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.PMS.Entities.Organizational;

namespace HRIS.Mapping.PMS.Entities.Organizational
{
    public sealed class AppraisalCustomSectionMap : ClassMap<AppraisalCustomSection>
    {
        public AppraisalCustomSectionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.Section);
            Map(x => x.Weight);
            Map(x => x.Rate);
            HasMany(x => x.AppraisalCustomSectionItems).Inverse().LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("Section_Id");

            References(x => x.Appraisal).Column("PhaseAppraisal_id");
        }
    }
}