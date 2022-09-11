using HRIS.Domain.Global.Constant;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.TaskManagement.RootEntities
{
    [Module(ModulesNames.TaskManagement)]
    public class DailyWork : Entity, IAggregateRoot
    {
        public DailyWork()
        {
            CreationDate = DateTime.Now;
        }
        [UserInterfaceParameter(Order = 5, IsNonEditable = true)]
        public virtual DateTime CreationDate { get; set; }

        [UserInterfaceParameter(Order = 7)]
        public virtual DateTime Date { get; set; }

        [UserInterfaceParameter(Order = 10, IsReference = true, ReferenceReadUrl = "TaskManagement/Reference/ReadTask")]
        public virtual Task Task { get; set; }

        [UserInterfaceParameter(Order = 45)]
        public virtual double Progress { get; set; }

        [UserInterfaceParameter(Order = 20,IsReference=true)]
        public virtual string Description { get; set; }

        [UserInterfaceParameter(Order = 100, IsNonEditable = true)]
        public virtual Employee Employee { get; set; }
    }
}
