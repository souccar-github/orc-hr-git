#region

using FluentNHibernate.Mapping;
using HRIS.Domain.PMS.Entities;

#endregion

namespace HRIS.Mapping.PMS.Entities
{
    //Ammar Alziebak
    public sealed class TemplateSectionWeightMap : ClassMap<TemplateSectionWeight>
    {
        public TemplateSectionWeightMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Weight);
            References(x => x.AppraisalSection);
            References(x => x.AppraisalTemplate);

        }
    }
}