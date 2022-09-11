using FluentNHibernate.Mapping;
using HRIS.Domain.Training.Entities;
using Souccar.Core;

namespace HRIS.Mapping.Training.Entities
{

    public sealed class CourseCostMap : ClassMap<CourseCost>
    {
        public CourseCostMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Cost);
            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);

            References(x => x.Name);
            References(x => x.Course);
            References(x => x.CostCenter);
        }
    }

}
