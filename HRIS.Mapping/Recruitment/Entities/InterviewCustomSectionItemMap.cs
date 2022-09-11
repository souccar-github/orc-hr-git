using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.Recruitment.Entities.Evaluations;
using Souccar.Core;

namespace HRIS.Mapping.Recruitment.Entities
{
    public class InterviewCustomSectionItemMap:ClassMap<InterviewCustomSectionItem>
    {
        public InterviewCustomSectionItemMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.Rate);
            Map(x => x.CanNotEvaluate);
            Map(x => x.Weight);

            References(x => x.AppraisalSectionItem);
            References(x => x.InterviewCustomSection);
            
        }
    }
}
