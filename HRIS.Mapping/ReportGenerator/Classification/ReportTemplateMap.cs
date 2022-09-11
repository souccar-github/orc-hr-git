#region

using FluentNHibernate.Mapping;
using Souccar.ReportGenerator.Domain.Classification;

#endregion

namespace HRIS.Mapping.ReportGenerator.Classification
{
    public sealed class ReportTemplateMap : ClassMap<ReportTemplate>
    {
        public ReportTemplateMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            DynamicUpdate();
            DynamicInsert();
            Map(x => x.Name);
            Map(x => x.RtfReportFooter);
            Map(x => x.RtfReportHeader);
            Map(x => x.ShowDateTime);
            Map(x => x.ShowFooter);
            Map(x => x.ShowHeader);
            Map(x => x.ShowPageNumber);
            Map(x => x.ShowUserName);
        }
    }
}