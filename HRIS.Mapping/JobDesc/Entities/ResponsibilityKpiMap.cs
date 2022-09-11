#region

using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.JobDescription.Entities
{
    public sealed class ResponsibilityKpiMap : ClassMap<ResponsibilityKpi>
    {
        public ResponsibilityKpiMap()
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

            Map(x => x.Value);

            References(x => x.Responsibility);

            #endregion

        }
    }
}