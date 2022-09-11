#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Objectives.Entities;
using HRIS.Domain.Objectives.RootEntities;

#endregion

namespace HRIS.Mapping.Objectives.Entities
{
    public sealed class TestCascadeMap : ClassMap<TestCascade>
    {
        public TestCascadeMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            References(x=>x.Grade);
            References(x=>x.JobTitle);
            References(x=>x.JobDescription);
            References(x=>x.Position);

            Map(x => x.D);
            Map(x => x.DT);
            Map(x => x.T);
            Map(x => x.NDT);
        }
    }
}