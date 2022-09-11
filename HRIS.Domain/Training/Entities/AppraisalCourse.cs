using System;
using System.Linq;
using HRIS.Domain.Training.Enums;
using HRIS.Domain.Training.Indexes;
using HRIS.Domain.Training.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Training.Entities
{
    public class AppraisalCourse :Entity ,IAggregateRoot
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual AppraisalCourseItem AppraisalKpi { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual AppraisalCourseLevel AppraisalLevel { get; set; }

        [UserInterfaceParameter(Order = 3)]
        public virtual int NumberOfTrainees { get; set; }

        [UserInterfaceParameter(Order = 5)]
        public virtual string Description { get; set; }
        public virtual double Weight
        {
            get
            {
                if (Course!=null && Course.CourseEmployees.Any(x => x.Type == CourseEmployeeType.Trainee))
                {
                    return Math.Round(((float)NumberOfTrainees / (float)Course.CourseEmployees.Count(x => x.Type == CourseEmployeeType.Trainee)) * 100,2);      
                }
                else
                {
                    return 0;
                }
            }
        }

        public virtual Course Course { get; set; }
    }
}
