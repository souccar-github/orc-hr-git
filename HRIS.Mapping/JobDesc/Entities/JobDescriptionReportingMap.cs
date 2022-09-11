using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Entities;

namespace HRIS.Mapping.JobDescription.Entities
{
    public sealed class JobDescriptionReportingMap : ClassMap<JobDescriptionReporting>
    {
        public JobDescriptionReportingMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.IsPrimary);
            References(x => x.JobDescription).Column("JobDescription_Id");
            References(x => x.ManagerJobDescription).Column("ManagerJobDescription_Id");
        }
    }
}
