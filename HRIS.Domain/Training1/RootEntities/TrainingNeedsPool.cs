using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Training1.Indexes;
using HRIS.Domain.OrgChart.RootEntities;

namespace HRIS.Domain.Training1.RootEntities
{
    [Module("Training1")]
    public class TrainingNeedsPool : Entity, IAggregateRoot
    {
        public TrainingNeedsPool()
        {
        }

        #region basic info
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        #endregion 

        #region indexes

        public virtual TrainingNeedLevel Level { get; set; }

        #endregion

        public virtual Node Department { get; set; }

        public virtual List<Course> Courses { get; set; }
        public virtual void AddCourse(Course course)
        {
            this.Courses.Add(course);
            course.NeedsPool = this;
        }
    }
}
