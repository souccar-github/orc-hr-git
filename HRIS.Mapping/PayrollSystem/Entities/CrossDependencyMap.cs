using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.Entities;

namespace HRIS.Mapping.PayrollSystem.Entities
{//todo : Mhd Update changeset no.2
    public class CrossDependencyMap : ClassMap<CrossDependency>
    {
        public CrossDependencyMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            //Map(x => x.AuditState);

            References(x => x.CrossDeductionWithBenefit);
            References(x => x.DeductionCard);
        }
    }
}
