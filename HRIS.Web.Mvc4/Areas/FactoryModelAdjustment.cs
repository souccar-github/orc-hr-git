using Project.Web.Mvc4.Areas.AttendanceSystem.Models;
using Project.Web.Mvc4.Areas.Audit.Models;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Models;
using Project.Web.Mvc4.Areas.Grades.Models;
using Project.Web.Mvc4.Areas.JobDescription.Models;
using Project.Web.Mvc4.Areas.Localization.Models;
using Project.Web.Mvc4.Areas.Objectives.Models;
using Project.Web.Mvc4.Areas.OrganizationChart.Models;
using Project.Web.Mvc4.Areas.PayrollSystem.Models;
using Project.Web.Mvc4.Areas.PMS.Models;
using Project.Web.Mvc4.Areas.ProjectManagement.Models;
using Project.Web.Mvc4.Areas.Recruitment.Models;
using Project.Web.Mvc4.Areas.ReportGenerator.Models;
using Project.Web.Mvc4.Areas.Reporting.Models;
using Project.Web.Mvc4.Areas.Security.Models;
using Project.Web.Mvc4.Areas.TaskManagement.Models;
using Project.Web.Mvc4.Areas.Workflow.Models;
using Project.Web.Mvc4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Web.Mvc4.Areas.Training.Models;

namespace Project.Web.Mvc4.Areas
{
    public static class FactoryModelAdjustment
    {

        private static Dictionary<string, ModelAdjustment> parent = new Dictionary<string, ModelAdjustment>();

        public static ModelAdjustment Create(string type)
        {

            //to do :  match name in modules name with namespace
            if (parent.Count == 0)
            {

                parent.Add("Personnel", new PersonnelAdjustment());
                parent.Add("AttendanceSystem", new AttendanceSystemAdjustment());
                parent.Add("EmployeeRelationServices", new EmployeeRelationServicesAdjustment());
                parent.Add("Grades", new GradeAdjustment());
                parent.Add("JobDescription", new JobDescriptionAdjustment());
                parent.Add("Localization", new LocalizationAdjustment());
                parent.Add("Objectives", new ObjectiveAdjustment());
                parent.Add("OrganizationChart", new OrganizationChartAdjustment());
                parent.Add("PayrollSystem", new PayrollSystemAdjustment());
                parent.Add("PMS", new PMSAdjustment());
                parent.Add("ProjectManagement", new ProjectManagementAdjustment());
                parent.Add("Recruitment", new RecruitmentAdjustment());
                parent.Add("ReportGenerator", new ReportGeneratorAdjustment());
                parent.Add("Reporting", new ReportingAdjustment());
                parent.Add("Security", new SecurityAdjustment());
                parent.Add("TaskManagement", new TaskManagementAdjustment());
                parent.Add("Workflow", new WorkflowAdjustment());
                parent.Add("Training", new TrainingAdjustment());
                parent.Add("Audit", new AuditAdjustment());
            }

            try
            {
                return parent[type];
            }
            catch
            {

                return new ModelAdjustment();
            }
        }
    }
}