//using System;
//using DevExpress.XtraReports.UI;
//using HRIS.Domain.Personnel.Entities;
//using Service;

//namespace Reporting.JobDesc
//{
//    public partial class SubReport : XtraReport
//    {
//        public SubReport()
//        {
//            InitializeComponent();
//        }


//        private void SubReport_DataSourceDemanded(object sender, EventArgs e)
//        {
//            //IJobDescriptionReporting jobDescriptionReporting = new JobDescriptionReporting(new EntityService<JobDescription>(), new EntityService<Position>());
//            //var template = jobDescriptionReporting.GetJobDescriptionTemplateByPositionID(_positionId);
//            DataSource = new EntityService<Employee>().GetList();
//        }
//    }
//}