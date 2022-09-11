using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Training1.RootEntities
{
    [Module("Training1")]
    public class TrainingPlan:Entity,IAggregateRoot
    {
        public TrainingPlan()
        {
            this.Courses = new List<Course>();
        }

        public virtual DateTime CreationDate { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual int Quarter { get; set; }

        public virtual IList<Course> Courses { get; set; }
        public virtual void AddCourse(Course course)
        {
            Courses.Add(course);
            course.Plan = this;
        }
    }
}
