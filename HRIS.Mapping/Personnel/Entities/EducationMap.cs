#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Personnel.Entities
{
    public sealed class EducationMap : ClassMap<Education>
    {
        public EducationMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.Type);
            References(x => x.Major);

            References(x => x.Rank);

            References(x => x.ScoreType);
            References(x => x.Score);

            Map(x => x.DateOfIssuance);

            References(x => x.Country);
            References(x => x.University);

            //Map(x => x.University).Length(GlobalConstant.SimpleStringMaxLength);

            Map(x => x.AmendmentDocumentNo);

            Map(x => x.AmendmentDocumentDate);

            Map(x => x.Comments).Length(GlobalConstant.MultiLinesStringMaxLength);

            References(x => x.Employee);
            HasMany(x => x.Attachments).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}