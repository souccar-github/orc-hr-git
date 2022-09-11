using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Entities;
using Souccar.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.EmployeeRelationServices.Entities
{
    public class ResignationAttachmentMap : ClassMap<ResignationAttachment>
    {
        public ResignationAttachmentMap()
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
            Map(x => x.Date);
            Map(x => x.Time);
            References(x => x.EmployeeResignation);
            References(x => x.User);
        }
    }
}
