#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Grades.Entities;


#endregion

namespace HRIS.Mapping.Grade.Entities
{
    public sealed class GradeByEducationQualificationMap : ClassMap<GradeByEducationQualification>
    {
        public GradeByEducationQualificationMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.MajorType);
            References(x => x.Major);
            Map(x => x.FirstSalary);

            Map(x => x.Note);


            References(x => x.CurrencyType);
            References(x => x.GradeByEducation);
        }
    }
}