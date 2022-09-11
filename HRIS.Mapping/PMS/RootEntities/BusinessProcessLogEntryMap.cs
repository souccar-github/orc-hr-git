#region

using FluentNHibernate.Mapping;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Mapping.PMS.RootEntities
{
    public sealed class BusinessProcessLogEntryMap : ClassMap<BusinessProcessLogEntry>
    {
        public BusinessProcessLogEntryMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Created);
            Map(x => x.Message);
            Map(x => x.UserName);
            Map(x => x.BusinessProcessId);
            Map(x => x.Bookmark);
            Map(x => x.AssignedTo);
            Map(x => x.State);
            Map(x => x.Content).CustomType<Entity>();
        }
    }
}