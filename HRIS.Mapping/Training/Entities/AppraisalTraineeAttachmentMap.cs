using FluentNHibernate.Mapping;
using HRIS.Domain.Training.Entities;
using Souccar.Core;

namespace HRIS.Mapping.Training.Entities
{
    public class AppraisalTraineeAttachmentMap: ClassMap<AppraisalTraineeAttachment>
    {
        public AppraisalTraineeAttachmentMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Attachment).Not.Nullable();
            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);

            References(x => x.AppraisalTrainee);
            References(x => x.AttachmentType);
        }
    }
}
