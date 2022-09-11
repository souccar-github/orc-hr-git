using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Training1.RootEntities;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Personnel.RootEntities;

namespace HRIS.Domain.Training1.Entities
{
    public class InvolvedStaff:Entity
    {
        public virtual Employee Employee { get; set; }
        public virtual Course Course { get; set; }
    }
}
