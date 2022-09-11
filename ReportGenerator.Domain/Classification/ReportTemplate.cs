using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using Souccar.ReportGenerator.Domain.Helper;

namespace Souccar.ReportGenerator.Domain.Classification
{
    [Module("ReportGenerator")]
    public class ReportTemplate : Entity, IAggregateRoot

    {
        [UserInterfaceParameter(Group = ReportGeneratorGroupsNames.ReportGeneratorGroupName + "_" + ReportGeneratorGroupsNames.BasicInfo, Order = 1)]
        public virtual string Name { get; set; }

        [UserInterfaceParameter(Group = ReportGeneratorGroupsNames.ReportGeneratorGroupName + "_" + ReportGeneratorGroupsNames.FooterInfo, Order = 21)]
        public virtual bool ShowFooter { get; set; }

        [UserInterfaceParameter(Group = ReportGeneratorGroupsNames.ReportGeneratorGroupName + "_" + ReportGeneratorGroupsNames.FooterInfo, Order = 22)]
        public virtual bool ShowDateTime { get; set; }

        [UserInterfaceParameter(Group = ReportGeneratorGroupsNames.ReportGeneratorGroupName + "_" + ReportGeneratorGroupsNames.FooterInfo, Order = 24)]
        public virtual bool ShowUserName { get; set; }

        [UserInterfaceParameter(Group = ReportGeneratorGroupsNames.ReportGeneratorGroupName + "_" + ReportGeneratorGroupsNames.FooterInfo, Order = 26)]
        public virtual bool ShowPageNumber { get; set; }

        [UserInterfaceParameter(Group = ReportGeneratorGroupsNames.ReportGeneratorGroupName + "_" + ReportGeneratorGroupsNames.HeaderInfo, Order = 30)]
        public virtual bool ShowHeader { get; set; }

        [UserInterfaceParameter(Group = ReportGeneratorGroupsNames.ReportGeneratorGroupName + "_" + ReportGeneratorGroupsNames.HeaderInfo, Order = 30, IsFile = true, AcceptExtension = ".rtf", FileSize = 5000000)]
        public virtual string RtfReportHeader { get; set; }

        [UserInterfaceParameter(Order = 11, IsFile = true, AcceptExtension = ".rtf", FileSize = 5000000,IsHidden = true)]
        public virtual string RtfReportFooter { get; set; }
    }
}