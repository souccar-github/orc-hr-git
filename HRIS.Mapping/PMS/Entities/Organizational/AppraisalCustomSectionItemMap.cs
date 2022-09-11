
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.PMS.Entities.Organizational;

namespace HRIS.Mapping.PMS.Entities.Organizational
{
    public sealed class AppraisalCustomSectionItemMap : ClassMap<AppraisalCustomSectionItem>
    {
        public AppraisalCustomSectionItemMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.Section).Column("Section_Id");
            Map(x => x.Weight);
            Map(x => x.Rate);
            Map(x => x.Description);

            References(x => x.Item);
        }
    }
}
