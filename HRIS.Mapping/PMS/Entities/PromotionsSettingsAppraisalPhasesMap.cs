using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.PMS.Entities;

namespace HRIS.Mapping.PMS.Entities
{
    
    public sealed class PromotionsSettingsAppraisalPhasesMap : ClassMap<PromotionsSettingsAppraisalPhases>
    {
        public PromotionsSettingsAppraisalPhasesMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.AppraisalPhase);
            References(x => x.PromotionsSettings).Column("PromotionsSettings_Id");
        }
    }
}
