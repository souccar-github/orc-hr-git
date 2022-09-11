using HRIS.Domain.Training1.Indexes;
using HRIS.Domain.Training1.RootEntities;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Training1.Entities
{
    public class AppraisalCourse :Entity 
    {
        public virtual AppraisalCourseItem AppraisalKpi { get; set; }
        public virtual AppraisalCourseLevel AppraisalLevel { get; set; }
        public virtual int NumberOfTrainees { get; set; }
        public virtual float Weight { get; set; }
        public virtual string Description { get; set; }

        public virtual Course Course { get; set; }
    }
}
