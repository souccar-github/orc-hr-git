using FluentNHibernate.Mapping;
using HRIS.Domain.Training.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.Training.Entities
{
    public sealed class CandidateEmployeeForJDItemsMap : ClassMap<CandidateEmployeeForJDItem>
    {
        public CandidateEmployeeForJDItemsMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Mark);

            References(x => x.Employee);
            References(x => x.CandidateEmployeeForJD);

        }
    }
}
