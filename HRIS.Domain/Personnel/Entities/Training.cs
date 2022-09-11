#region

using System;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Training.Indexes;
using Souccar.Domain.DomainModel;
using Souccar.Core.CustomAttribute;
using System.Collections.Generic;
using HRIS.Domain.Personnel.Helpers;

#endregion

namespace HRIS.Domain.Personnel.Entities
{
    public class Training : Entity
    {
        public Training()
        {
            Attachments = new List<TrainingAttachment>();
        }


        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.CourseDetails, Order = 10, IsReference = true)]
        public virtual CourseSpecialize Specialize { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.CourseDetails, Order = 20)]
        public virtual string CourseName { get; set; }


        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.CourseDetails, Order = 30)]
        public virtual DateTime CourseStartDate { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.CourseDetails, Order = 40)]
        public virtual DateTime CourseEndDate { get; set; }


        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.CourseDetails, Order = 50)]
        public virtual int CourseDurationPerHour { get; set; }



        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.TrainingCenterDetails, Order = 60, IsReference = true)]
        public virtual TrainingCenter TrainingCenter { get; set; }


        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.TrainingCenterDetails, Order = 65)]
        public virtual string TrainingCenterLocation { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.TrainingCenterDetails, Order = 70)]
        public virtual string instructor { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.TrainingCenterDetails, Order = 75)]
        public virtual string instructorPhone { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.TrainingCenterDetails, Order = 80)]
        public virtual DateTime CertificateIssuanceDate { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.TrainingCenterDetails, Order = 90, IsReference = true)]
        public virtual Status Status { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.TrainingCenterDetails, Order = 100)]
        public virtual string Notes { get; set; }

        public virtual Employee Employee { get; set; }
        [UserInterfaceParameter(Order = 1)]
        public virtual IList<TrainingAttachment> Attachments { get; set; }

        public virtual void AddAttachment(TrainingAttachment attachment)
        {
            attachment.Training = this;
            Attachments.Add(attachment);
        }



    }
}