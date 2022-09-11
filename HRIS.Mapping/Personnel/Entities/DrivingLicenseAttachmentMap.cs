﻿using FluentNHibernate.Mapping;
using Souccar.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.Personnel.Entities
{
    public class DrivingLicenseAttachmentMap : ClassMap<HRIS.Domain.Personnel.Entities.DrivingLicenseAttachment>
    {
        public DrivingLicenseAttachmentMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.Title).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.FilePath).Length(GlobalConstant.SimpleStringMaxLength);
            References(x => x.DrivingLicense);
           

        }

    }
}
