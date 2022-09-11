using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.Recruitment.Entities.Evaluations;

namespace HRIS.Mapping.Recruitment.Entities
{
    public class EvaluatorMap : ClassMap<Evaluator>
    {
        public EvaluatorMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Date);
            Map(x => x.Mark);
            References(x => x.User);
            References(x => x.Interview);

            HasMany(x => x.InterviewCustomSections).Inverse().Cascade.AllDeleteOrphan();

        }
    }
}