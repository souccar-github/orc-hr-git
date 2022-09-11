using System;
using Souccar.Domain.DomainModel;

namespace Souccar.ReportGenerator.Domain.Classification
{
    [Serializable]
    public class ReportTemplateContent : ValueObject, IContent
    {
        public byte[] RtfReportHeader { get; set; }
        public byte[] RtfReportFooter { get; set; }
        public bool ShowDateTime { get; set; }
        public bool ShowUserName { get; set; }
        public bool ShowPageNumber { get; set; }
        public bool ShowHeader { get; set; }
        public bool ShowFooter { get; set; }
    }
}