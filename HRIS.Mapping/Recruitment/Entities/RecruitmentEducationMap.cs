using FluentNHibernate.Mapping;
using HRIS.Domain.Recruitment.Entities;
using Souccar.Core;

namespace HRIS.Mapping.Recruitment.Entities
{
    public class RecruitmentEducationMap: ClassMap<RecruitmentEducation>
    {
        public RecruitmentEducationMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x=>x.DateOfIssuance);
            Map(x=>x.AmendmentDocumentNo);
            Map(x=>x.AmendmentDocumentDate);
            Map(x=>x.Comments).Length(GlobalConstant.MultiLinesStringMaxLength);

            References(x=>x.Type);
            References(x=>x.Major);
            References(x=>x.University).Nullable();
            References(x=>x.Rank).Nullable();
            References(x=>x.ScoreType).Nullable();
            References(x=>x.Score).Nullable();
            References(x=>x.Country).Nullable();
            References(x=>x.JobApplication);
            
        }
        
    }
}
