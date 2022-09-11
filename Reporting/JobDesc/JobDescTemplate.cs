//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using HRIS.Domain.JobDesc.Entities;
//using HRIS.Domain.OrgChart.ValueObjects;
//using Reporting.Infrastructure;
//using Service;
//using Service.DTO.JobDesc;
//using Service.Reporting.JobDesc;

//namespace Reporting.JobDesc
//{
//    public partial class JobDescTemplate : BaseReport
//    {
//        private readonly int _positionId;

//        public JobDescTemplate(int positionId)
//        {
//            _positionId = positionId;
//            InitializeComponent();
//        }

//        public JobDescTemplate(int positionId, CultureInfo culture)
//            : base(culture, "Resources.Areas.JobDesc.Reports.JobDescriptionTemplate")

//        {
//            _positionId = positionId;
//            InitializeComponent();
//        }

//        private void JobDescTemplate_DataSourceDemanded(object sender, EventArgs e)
//        {
//            IJobDescriptionReporting jobDescriptionReporting =
//                new JobDescriptionReporting(new EntityService<JobDescription>(), new EntityService<Position>());
//            JobDescriptionTemplate template = jobDescriptionReporting.GetJobDescriptionTemplateByPositionID(_positionId);
//            DataSource = new List<JobDescriptionTemplate> {template};
//        }
//    }
//}