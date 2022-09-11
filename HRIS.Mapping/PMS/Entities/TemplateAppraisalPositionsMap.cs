#region

using FluentNHibernate.Mapping;
using HRIS.Domain.PMS.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.PMS.Entities
{
    public sealed class TemplateAppraisalPositionsMap : ClassMap<TemplateAppraisalPositions>
    {
        public TemplateAppraisalPositionsMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            #region References

            References(x => x.AppraisalTemplate);//.Column("AppraisalTemplate_id");
            References(x => x.Position);
            References(x => x.AppraisalTemplateSetting);//.Column("TemplateSetting_id");
            #endregion

            #region Appraisal Section Item Kpi

            #endregion

        }
    }
}