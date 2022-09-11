using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.PMS.Entities.JobDescription;

namespace HRIS.Mapping.PMS.Entities.JobDescription
{
    public sealed class AppraisalJobDescriptionMap : ClassMap<AppraisalJobDescription>
    {
        public AppraisalJobDescriptionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.Responsibility);
            Map(x => x.Weight);
            Map(x => x.Rate);
            Map(x => x.Description);

            References(x => x.Appraisal).Column("PhaseAppraisal_id");
        }
    }
}