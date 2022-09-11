using System;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Objectives.Entities;
using HRIS.Domain.Objectives.Enums;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Objectives.Entities
{
    public abstract class AbstractObjective : Entity
    {

        [UserInterfaceParameter(Order = 35)]
        public virtual string Code { get; set; }

        [UserInterfaceParameter(Order = 80)]
        public virtual string Name { get; set; }

        [UserInterfaceParameter(Order = 90)]
        public virtual string Description { get; set; }



        #region Evalution Criteria
        
        [UserInterfaceParameter(Order = 150)]
        public virtual string DoesNotMeetExpectation { get; set; }

        [UserInterfaceParameter(Order = 160)]
        public virtual string MeetExpectation { get; set; }

        [UserInterfaceParameter(Order = 170)]
        public virtual string AboveExpectation { get; set; }

        #endregion
    }
}