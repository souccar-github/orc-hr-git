using FluentNHibernate.Mapping;
using Souccar.Core;
using Souccar.Domain.Notification;

namespace HRIS.Mapping.Notification.RootEntities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class NotifyMap : ClassMap<Notify>
    {
        public NotifyMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            Map(x => x.Date);
            Map(x => x.Time);
            Map(x => x.Type);

            References(x => x.Sender);


            Map(x => x.Subject).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.Body).Length(GlobalConstant.MultiLinesStringMaxLength);

            Map(x => x.DestinationTabName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.DestinationModuleName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.DestinationLocalizationModuleName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.DestinationControllerName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.DestinationActionName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.DestinationEntityId).Length(GlobalConstant.SimpleStringMaxLength);

            Map(x => x.DestinationEntityTypeFullName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.DestinationEntityTitle).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.DestinationEntityOperationType).Length(GlobalConstant.SimpleStringMaxLength);


            HasMany(x => x.Receivers).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.DestinationData).Table("NotifyDestinationData").AsMap<string>("Name").Element("Value");
        }
    }
}
