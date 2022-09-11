using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Configurations;
using HRIS.Domain.JobDescription.Entities;
using Souccar.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.JobDescription.Entities
{
    public sealed class CompetenceCategoryLevelDescriptionMap : ClassMap<CompetenceCategoryLevelDescription>
    {
        public CompetenceCategoryLevelDescriptionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.Level);

            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);

            References(x => x.CompetenceCategory);
        }
    }
    
}
