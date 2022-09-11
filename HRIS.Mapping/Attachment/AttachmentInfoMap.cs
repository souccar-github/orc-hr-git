//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//project manager:
//supervisor:
//author: ammar alziebak
//description:
//start date:
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Souccar.Core;
using Souccar.Domain.Attachment;
using Souccar.Domain.Attachment.Entities;
using Souccar.Domain.Attachment.Enums;

namespace HRIS.Mapping.Attachment
{
    public sealed class AttachmentInfoMap : ClassMap<AttachmentInfo>
    { 
        public AttachmentInfoMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Path).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.PhysicalFileName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.OriginalFileName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.ModelFullClassName).Length(GlobalConstant.SimpleStringMaxLength);
            //Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength); ;
            Map(x => x.UploadDate);
            Map(x => x.EntityType);
            //Map(x => x.BaseId).Column("Base_Id"); 

        }
    }
}
