using System;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using Souccar.Domain.Workflow.RootEntities;

namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    public class PaidSlice : Entity
    {

        #region Basic Info

        [UserInterfaceParameter(Order = 5)]
        public virtual double FromBalance { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual double ToBalance { get; set; }

        [UserInterfaceParameter(Order = 15)]
        public virtual double PaidPercentage { get; set; }
        public virtual LeaveSetting LeaveSetting { get; set; }

        #endregion

    }
}
