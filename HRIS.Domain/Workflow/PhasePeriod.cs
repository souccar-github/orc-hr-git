using HRIS.Domain.Global.Enums;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Workflow
{
    public abstract class PhasePeriod : Entity, IAggregateRoot
    {
        public PhasePeriod()
        {
            CreationDate = DateTime.Now;
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string Name { set; get; }
        [UserInterfaceParameter(Order=1,IsNonEditable=true)]
        public virtual DateTime CreationDate { set; get; }

        [UserInterfaceParameter(Order = 20)]
        public virtual Period Period { get; set; }

        [UserInterfaceParameter(Order = 30)]
        public virtual DateTime StartDate { set; get; }

        [UserInterfaceParameter(Order = 40)]
        public virtual DateTime EndDate { get; set; }

        [UserInterfaceParameter(Order = 50)]
        public virtual int Year { get; set; }

        [UserInterfaceParameter(Order = 60,  ReferenceReadUrl = "Workflow/Reference/ReadQuarter")]
        public virtual Quarter Quarter { get; set; }

        [UserInterfaceParameter(Order = 70 , ReferenceReadUrl = "Workflow/Reference/ReadSemiAnnual")]
        public virtual SemiAnnual SemiAnnual { get; set; }

        [UserInterfaceParameter(Order = 80, ReferenceReadUrl = "Workflow/Reference/ReadMonth")]
        public virtual Month Month { get; set; }

        [UserInterfaceParameter(Order = 90)]
        public virtual string Description { set; get; }
        public virtual string getPhaseName()
        {
            return Enum.GetName(typeof(Period), Period) == Enum.GetName(typeof(Period), Period.Annual)
                ? ServiceFactory.LocalizationService.GetResource("HRIS.Domain.Global.Enums.Period." + Enum.GetName(typeof(Period), Period.Annual)) + "," + Year
                : Enum.GetName(typeof(Period), Period) == Enum.GetName(typeof(Period), Period.SemiAnnual)
                ? ServiceFactory.LocalizationService.GetResource("HRIS.Domain.Global.Enums.SemiAnnual." + Enum.GetName(typeof(SemiAnnual), SemiAnnual)) + "," + Year
                : Enum.GetName(typeof(Period), Period) == Enum.GetName(typeof(Period), Period.Quarterly)
                ? ServiceFactory.LocalizationService.GetResource("HRIS.Domain.Global.Enums.Quarter." + Enum.GetName(typeof(Quarter), Quarter)) + "," + Year
                : Enum.GetName(typeof(Period), Period) == Enum.GetName(typeof(Period), Period.Monthly)
                ? ServiceFactory.LocalizationService.GetResource("HRIS.Domain.Global.Enums.Month." + Enum.GetName(typeof(Month), Month)) + "," + Year
                : StartDate.ToString("dd/MM/yyyy") + "-" + EndDate.ToString("dd/MM/yyyy");
        }
    }
}
