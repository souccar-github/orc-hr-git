//using System.Collections.Generic;
//using HRIS.Domain.Training.Indexes;
//using Souccar.Core.CustomAttribute;
//using Souccar.Domain.DomainModel;
//using HRIS.Domain.OrgChart.RootEntities;

//namespace HRIS.Domain.Training.RootEntities
//{
//    [Module("Training")]
//    public class TrainingNeedsPool : Entity, IAggregateRoot
//    {
//        public TrainingNeedsPool()
//        {
//            Courses=new List<Course>();
//        }

//        #region basic info
//        public virtual string Name { get; set; }
//        public virtual string Description { get; set; }
//        #endregion 

//        #region indexes

//        public virtual TrainingNeedLevel Level { get; set; }

//        #endregion

//        public virtual Node Department { get; set; }

//        public virtual List<Course> Courses { get; set; }
//        public virtual void AddCourse(Course course)
//        {
//            this.Courses.Add(course);
//            course.NeedsPool = this;
//        }
//    }
//}
