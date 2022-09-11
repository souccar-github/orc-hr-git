using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Recruitment.Enums;
using HRIS.Domain.Recruitment.Indexes;
using HRIS.Domain.Recruitment.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Recruitment.Entities;
using FluentNHibernate.Mapping;
using Souccar.Core;

namespace HRIS.Mapping.Recruitment.Entities
{
    public class ProfessionalCertificationMap : ClassMap<ProfessionalCertification>
    {
        public ProfessionalCertificationMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.DateOfIssuance);
            Map(x => x.ExpirationDate);
            Map(x => x.Notes).Length(GlobalConstant.MultiLinesStringMaxLength); 

            References(x=>x.Type);
            References(x=>x.PlaceOfIssuance).Nullable();
            References(x=>x.JobApplication);

        }

       
    }
}
