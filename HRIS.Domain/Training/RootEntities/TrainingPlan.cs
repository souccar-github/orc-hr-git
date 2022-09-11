using System;
using System.Collections.Generic;
using HRIS.Domain.Global.Enums;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Training.Entities;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Training.RootEntities
{
    //[Command(CommandsNames.addCoursesFromNeedHandler, Order = 1)]
    [Module(ModulesNames.Training)]
    public class TrainingPlan:Entity,IAggregateRoot
    {
        public TrainingPlan()
        {
            Courses = new List<Course>();
        }
        [UserInterfaceParameter(Order = 1)]
        public virtual string PlanName { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual DateTime StartDate { get; set; }

        [UserInterfaceParameter(Order = 3)]
        public virtual DateTime EndDate { get; set; }

        //[UserInterfaceParameter(Order = 4)]
        //public virtual Quarter Quarter { get; set; }

        [UserInterfaceParameter(Order = 5, IsNonEditable = true)]
        public virtual DateTime CreationDate { get; set; }

        public virtual IList<Course> Courses { get; set; }
        public virtual void AddCourse(Course course)
        {
            Courses.Add(course);
            course.TrainingPlan = this;
        }


        [UserInterfaceParameter(Order = 200, IsHidden = true)]
        public virtual string NameForDropdown => PlanName;
    }
}
