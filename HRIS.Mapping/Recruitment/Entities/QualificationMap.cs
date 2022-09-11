using FluentNHibernate.Mapping;
using HRIS.Domain.Recruitment.Entities;

namespace HRIS.Mapping.Recruitment.Entities
{

    public sealed class QualificationMap : ClassMap<Qualification>
    {
        public QualificationMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.MajorType).Not.Nullable();
            References(x => x.Major).Not.Nullable();
            References(x => x.RecruitmentInformation);

        }
    }
}