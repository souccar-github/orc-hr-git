using FluentNHibernate.Mapping;
using HRIS.Domain.Recruitment.Entities;
using Souccar.Core;

namespace HRIS.Mapping.Recruitment.Entities
{
    public class JobApplicationAttachmentMap : ClassMap<JobApplicationAttachment>
    {
        public JobApplicationAttachmentMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength); ;
            Map(x=>x.Attachment);

            References(x=>x.AttachmentType);
            References(x=>x.JobApplication);
        }
        
    }
}
