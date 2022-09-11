using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.ProjectManagement.Entities;

namespace HRIS.Mapping.ProjectManagement.Entites
{
    public sealed class EvaluateProjectMap:ClassMap<EvaluateProject>
    {
        public EvaluateProjectMap()
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
