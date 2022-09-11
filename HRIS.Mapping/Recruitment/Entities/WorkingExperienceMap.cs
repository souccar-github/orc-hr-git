using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using HRIS.Domain.Recruitment.Indexes;
using HRIS.Domain.Recruitment.RootEntities;
using FluentNHibernate.Mapping;
using HRIS.Domain.Recruitment.Entities;

namespace HRIS.Mapping.Recruitment.Entities
{
    public class WorkingExperienceMap:ClassMap<WorkingExperience>
    {
        public WorkingExperienceMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x=>x.CompanyName);
            Map(x=>x.StartDate);
            Map(x=>x.EndDate);
            Map(x=>x.WorkingDuration);
            Map(x=>x.LeaveReason);
            Map(x=>x.AuthorizationToCheck);
            Map(x=>x.ReferenceFullName);
            Map(x=>x.ReferenceContact);
            Map(x=>x.ReferenceEmail);

            References(x=>x.Industry);
            References(x=>x.ReferenceJobTitle);
            References(x=>x.JobTitle).Nullable();
            References(x=>x.JobApplication);
        }
        
    }
}
