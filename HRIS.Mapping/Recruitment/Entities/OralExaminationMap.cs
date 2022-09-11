using FluentNHibernate.Mapping;
using HRIS.Domain.Recruitment.Entities;

namespace HRIS.Mapping.Recruitment.Entities
{

    public sealed class OralExaminationMap : ClassMap<OralExamination>
    {
        public OralExaminationMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            Map(x => x.AcceptedPersonsDecisionNumber).Not.Nullable();

            Map(x => x.AcceptedPersonsDecisionDate).Not.Nullable();

            References(x => x.Place).Not.Nullable();

            Map(x => x.Date).Not.Nullable();

            Map(x => x.Time).Not.Nullable();

            References(x => x.Advertisement);

        }
    }
}