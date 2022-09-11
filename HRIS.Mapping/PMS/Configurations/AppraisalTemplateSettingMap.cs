using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.PMS.RootEntities;
using Souccar.Domain.DomainModel;
using HRIS.Domain.PMS.Configurations;

namespace HRIS.Mapping.PMS.Configurations
{
    public sealed class AppraisalTemplateSettingMap : ClassMap<AppraisalTemplateSetting>
    {
        public AppraisalTemplateSettingMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Name);
            Map(x => x.CreationDate);
            References(x => x.DefaultTemplate);

            HasMany(x => x.AppraisalTemplatePositions).Inverse().LazyLoad().Cascade.AllDeleteOrphan();//.KeyColumn("PhaseWorkflow_id");
        }
    }
}