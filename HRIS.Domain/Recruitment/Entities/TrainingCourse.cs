using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Domain.DomainModel;
using HRIS.Domain.JobDescription.Indexes;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Recruitment.Indexes;
using HRIS.Domain.Recruitment.RootEntities;
using HRIS.Domain.Training.Indexes;
using Souccar.Core.CustomAttribute;

namespace HRIS.Domain.Recruitment.Entities
{
    public class TrainingCourse : Entity
    {
        [UserInterfaceParameter(Order = 5)]
        public virtual CourseSpecialize CompetencyName { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual Level CompetencyLevel { get; set; }

        [UserInterfaceParameter(Order = 15)]
        public virtual string CourseName { get; set; }

        [UserInterfaceParameter(Order = 20)]
        public virtual string CourseDuration { get; set; }

        [UserInterfaceParameter(Order = 25)]
        public virtual string TrainingCenter { get; set; }

        [UserInterfaceParameter(Order = 30)]
        public virtual string TrainingLocation { get; set; }

        [UserInterfaceParameter(Order = 30)]
        public virtual DateTime? AttendanceCertificateIssuanceDate { get; set; }

        [UserInterfaceParameter(Order = 35)]
        public virtual Status Status { get; set; }

        [UserInterfaceParameter(Order = 40)]
        public virtual string Description { get; set; }


        public virtual JobApplication JobApplication { get; set; }
    }
}
