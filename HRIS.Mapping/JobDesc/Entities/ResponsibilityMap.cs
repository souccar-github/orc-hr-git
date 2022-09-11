#region

using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.JobDescription.Entities
{
    public sealed class ResponsibilityMap : ClassMap<Responsibility>
    {
        public ResponsibilityMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            #region Basic Info.

           

            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);
            
            Map(x => x.Weight);


            References(x => x.Role);
          
            HasMany(x => x.ResponsibilityKpis).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            #endregion

        }
    }
}