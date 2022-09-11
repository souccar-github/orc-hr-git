using FluentNHibernate.Mapping;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Domain.Recruitment.RootEntities;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Core;

namespace HRIS.Mapping.Recruitment.Entities
{
    public class RecruitmentRequestAttachmentMap: ClassMap<RecruitmentRequestAttachment>
    {
        public RecruitmentRequestAttachmentMap()
        {

            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.Attachment);

            References(x => x.AttachmentType);
            References(x => x.RecruitmentRequest);
        }
        
    }
}
