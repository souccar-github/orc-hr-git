using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.AttendanceSystem.Configurations;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Personnel.Entities
{
    public class EmployeeTemporaryWorkshop : Entity, IConfigurationRoot
    {
        [UserInterfaceParameter(Order = 5)]
        public virtual EmployeeCard EmployeeCard { get; set; }

        [UserInterfaceParameter(Order = 10, IsReference = true)]
        public virtual Workshop Workshop { get; set; }

        [UserInterfaceParameter(Order = 15)]
        public virtual DateTime FromDate { get; set; }

        [UserInterfaceParameter(Order = 20)]
        public virtual DateTime ToDate { get; set; }
    }
}
