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

    public class Recycle : Entity, IAggregateRoot
    {
        public Recycle()
        {
            RequestDate = DateTime.Now;
        }

        #region Basic Info

        [UserInterfaceParameter(Order = 5)]
        public virtual RecycleType RecycleType { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual int Year { get; set; }

        [UserInterfaceParameter(Order = 15, IsNonEditable = true)]
        public virtual DateTime RequestDate { get; protected set; }

        public virtual LeaveSetting LeaveSetting { get; set; }

        #endregion

    }
}
