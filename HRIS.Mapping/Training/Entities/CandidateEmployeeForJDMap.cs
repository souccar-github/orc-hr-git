using FluentNHibernate.Mapping;
using HRIS.Domain.Training.Entities;
using Souccar.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.Training.Entities
{
    public sealed class CandidateEmployeeForJDMap : ClassMap<CandidateEmployeeForJD>
    {
        public CandidateEmployeeForJDMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.CreateDate);

            References(x => x.JobDescription);

            HasMany(x => x.CandidateEmployeeForJDItems).Inverse().LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("CandidateEmployeeForJD_id");
        }
    }
}
