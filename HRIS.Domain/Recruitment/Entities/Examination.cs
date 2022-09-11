
using System;
using HRIS.Domain.Recruitment.Indexes;
using HRIS.Domain.Recruitment.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Recruitment.Entities
{

    public class Examination : Entity
    {

        #region Basic Info

        [UserInterfaceParameter(Order = 1)]
        public virtual string AcceptedPersonsDecisionNumber { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual DateTime AcceptedPersonsDecisionDate { get; set; }

        [UserInterfaceParameter(Order = 3)]
        public virtual Place Place { get; set; }

        [UserInterfaceParameter(Order = 4)]
        public virtual DateTime Date { get; set; }

        [UserInterfaceParameter(Order = 5, IsTime = true)]
        public virtual DateTime Time { get; set; }

        public virtual Advertisement Advertisement { get; set; }

        #endregion

    }
}
