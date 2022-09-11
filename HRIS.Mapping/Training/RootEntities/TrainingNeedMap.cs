using FluentNHibernate.Mapping;
using HRIS.Domain.Training.RootEntities;
using Souccar.Core;

namespace HRIS.Mapping.Training.RootEntities
{

    public sealed class TrainingNeedMap : ClassMap<TrainingNeed>
    {
        public TrainingNeedMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Name).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.Status);
            Map(x => x.Source);
            Map(x => x.CreationDate);

            //References(x => x.Employee);
            References(x => x.Level);
            References(x => x.WorkflowItem).Nullable();

        }
    }

}
