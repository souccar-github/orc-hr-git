#region

using FluentNHibernate.Mapping;
using HRIS.Domain.PMS.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.PMS.Entities
{
    public sealed class PromotionHistoryMap : ClassMap<PromotionHistory>
    {
        public PromotionHistoryMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            #region Basic Info.

            Map(x => x.OldSalary);
            Map(x => x.Rate);
            Map(x => x.NewSalary);
            Map(x => x.Note);
            Map(x => x.Benefit);

            #endregion

            #region References

            References(x => x.Employee);
            References(x => x.PromotionsSettings);

            #endregion

        }
    }
}