using HRIS.Domain.Training1.Indexes;
using HRIS.Domain.Training1.RootEntities;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Training1.Entities
{
    public class CourseCost : Entity
    {
        public virtual CostName Name { get; set; }
        public virtual double Cost { get; set; }
        public virtual int Quantity { get; set; }
        public virtual string Description { get; set; }

        public virtual Course Course { get; set; }
    }
}
