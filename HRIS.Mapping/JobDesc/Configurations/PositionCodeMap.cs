#region

using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Configurations;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.JobDescription.Configurations
{
    public sealed class PositionCodeMap : ClassMap<PositionCode>
    {
        public PositionCodeMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            #region Position Code
            Map(x => x.FixedPrefix).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.FixedSuffix).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.CustomPrefix);
            Map(x => x.CustomPrefixLength);
            Map(x => x.CustomPrefixStartingPosition);
            Map(x => x.CustomSuffix);
            Map(x => x.CustomSuffixLength);
            Map(x => x.CustomSuffixStartingPosition);
            Map(x => x.SeparatorSymbol);
            #endregion

        }
    }
}
