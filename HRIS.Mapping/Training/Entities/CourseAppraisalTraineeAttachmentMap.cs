using FluentNHibernate.Mapping;
using HRIS.Domain.Training.Entities;
using Souccar.Domain.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.Training.Entities
{
    public sealed class CourseAppraisalTraineeAttachmentMap : ClassMap<CourseAppraisalTraineeAttachment>
     {
         public CourseAppraisalTraineeAttachmentMap()
         {
             #region Default
             DynamicUpdate();
             DynamicInsert();
             Id(x => x.Id);
             Map(x => x.IsVertualDeleted);
             #endregion

             Map(x => x.Title).Not.Nullable();
             Map(x => x.Description);
             Map(x => x.CreationDate);
             Map(x => x.FileName);
             References(x => x.AppraisalTrainee).Column("AppraisalTrainee_id");

         }
     }
}
