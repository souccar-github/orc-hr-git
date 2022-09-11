using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.ProjectManagement.RootEntities;

namespace HRIS.Mapping.ProjectManagement.RootEntities
{
    public sealed class EvaluationMap:ClassMap<Evaluation>
    {
        public EvaluationMap()
        {
            #region Default

            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);

            #endregion

            Map(x => x.EvaluationDate);
            Map(x => x.FromDate);
            Map(x => x.ToDate);
            Map(x => x.Quarter);
            Map(x => x.TotalRate);

            References(x => x.Project);
            References(x => x.Evaluator);
        }
    }
}
