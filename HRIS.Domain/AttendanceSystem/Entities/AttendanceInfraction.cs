using HRIS.Domain.AttendanceSystem.Configurations;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.AttendanceSystem.Entities
{
    public class AttendanceInfraction : Entity, IAggregateRoot
    {
        public AttendanceInfraction()
        {
            this.CreationDate = DateTime.Now.Date;
        }
        public virtual InfractionForm Infraction { get; set; }
        public virtual AttendanceRecord AttendanceRecord { get; set; }
        public virtual int RepeationNumber { get; set; }
        public virtual DisciplinarySetting Penalty { get; set; }
        public virtual EmployeeCard EmployeeCard { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual DateTime PenaltyDate { get; set; }
        public virtual bool IsActiveForNextPenalties { get; set; }

    }
}
