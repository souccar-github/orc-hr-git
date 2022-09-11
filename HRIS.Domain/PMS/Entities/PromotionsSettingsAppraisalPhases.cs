
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Domain.DomainModel;
using HRIS.Domain.PMS.RootEntities;
using HRIS.Domain.PMS.Configurations;

namespace HRIS.Domain.PMS.Entities
{
    public class PromotionsSettingsAppraisalPhases : Entity
    {
        public virtual AppraisalPhase AppraisalPhase { get; set; }
        public virtual PromotionsSettings PromotionsSettings { get; set; }
    }
}
