using FluentNHibernate.Mapping;
using HRIS.Domain.Training.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.Training.Entities
{

    public sealed class CareerPathNodeMap : ClassMap<CareerPathNode>
    {
        public CareerPathNodeMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            References(x => x.JobTitle).Nullable();
            References(x => x.JobDescription).Nullable();
            References(x => x.GradeStep).Nullable();
            References(x => x.CareerPathFamily).Nullable();
            
        }
    }
}
