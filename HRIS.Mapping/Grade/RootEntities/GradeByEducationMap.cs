using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.Grades.RootEntities;
using HRIS.Domain.OrganizationChart.RootEntities;
using Souccar.Core;

namespace HRIS.Mapping.Grade.RootEntities
{
    public sealed class GradeByEducationMap : ClassMap<GradeByEducation>
    {
        public GradeByEducationMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Order).Column("Step_Order");
            
            Map(x => x.MinSalary);
            Map(x => x.MaxSalary);
            References(x => x.CurrencyType);

            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);

            HasMany(x => x.GradeByEducationQualifications).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}
