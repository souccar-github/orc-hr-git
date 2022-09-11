#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//project manager:
//supervisor:
//author: Ammar Alziebak
//description:تطوير استمارة تقييم الأداء
//start date:
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion

#region

using FluentNHibernate.Mapping;
using HRIS.Domain.PMS.RootEntities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.PMS.RootEntities
{
    public sealed class AppraisalTemplateMap : ClassMap<AppraisalTemplate>
    {
        public AppraisalTemplateMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            
            #region Basic Info
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.CreationDate);
            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.Competency);
            Map(x => x.JobDescription);
            Map(x => x.Objective);
            Map(x => x.CompetencyWeight);
            Map(x => x.JobDescriptionWeight);
            Map(x => x.ObjectiveWeight);
            #endregion

            #region References
            References(x => x.Type);
            #endregion

            #region HasMany
            HasMany(x => x.TemplateSectionWeights).Inverse().LazyLoad().Cascade.AllDeleteOrphan();            
            #endregion
        }
    }
}