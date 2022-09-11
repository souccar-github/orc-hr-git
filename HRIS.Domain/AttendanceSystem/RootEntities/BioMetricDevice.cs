using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.AttendanceSystem.RootEntities
{
    public class BioMetricDevice : Entity, IAggregateRoot
    {
        public virtual string Name { get; set; } // مميز الجهاز وهو مولد تلقائيا
        public virtual string DeviceTypeFullName { get; set; } // كلاس الجهاز في الدومين
    }
}
