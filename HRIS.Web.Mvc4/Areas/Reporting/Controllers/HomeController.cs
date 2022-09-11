using System.Linq;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.Global.Constant;
using Project.Web.Mvc4.ProjectModels;
using Souccar.Domain.Reporting;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Areas.Reporting.Helpers;
using Souccar.CodeGenerator.Resourecs;
using Souccar.Domain.Localization;

namespace Project.Web.Mvc4.Areas.Reporting.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index(RequestInformation.Navigation.Step moduleInfo)
        {
            if (TempData["Module"] == null)
                return RedirectToAction("Welcome", "Module", new { area = "", id = ModulesNames.Reporting });

            return View();
        }

        public ActionResult GetFullPath(IEnumerable<HttpPostedFileBase> files, string fieldName, string typeName)
        {
            ReportingHelper.SourcePath = Directory.CreateDirectory(Server.MapPath("~/Content/UploadedFiles/" + typeName+"/" + fieldName)).FullName;
            ReportingHelper.DestinationPath = Directory.CreateDirectory(Server.MapPath("~/Content/UploadedFiles/Souccar.Domain.Report.ReportDefinition/SSRS")).FullName;
            
            return Content("");
        }

        public ActionResult GetReportName(string fileName)
        {
            var directoryInfo = new DirectoryInfo(ReportingHelper.SourcePath);
            var filesInfo = directoryInfo.GetFiles("*.rdl").Where(x => x.Name.Contains(fileName));
            if (filesInfo.Any())
            {
                var date=new DateTime();
                string name = "";
                foreach (var fileInfo in filesInfo)
                {
                    int compareValue = date.CompareTo(fileInfo.LastWriteTime);
                    if (compareValue<0)
                    {
                        name = fileInfo.Name;
                        date = fileInfo.LastWriteTime;
                    }
                }

                ReportingHelper.ReportName = name;
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult removeFiles(string[] fileNames, string fieldName, string typeName)
        {
            //var physicalPath = Server.MapPath("~/Content/UploadedFiles/Souccar.Domain.Report.ReportDefinition/SSRS/" + fieldName);
           
            //System.IO.File.Delete(Path.Combine(physicalPath, (string)Session["Uploadfile-" + typeName + "-" + fieldName]));
            return Content("");
        }

        private string GetUniqueFileName(string physicalPath, int fileSize)
        {
            var fileName = Path.GetFileName(physicalPath);
            var extension = Path.GetExtension(physicalPath);
            var directory = Path.GetDirectoryName(physicalPath);
            fileName = string.Format("{0}_{1}_{2}_{3}", Guid.NewGuid().ToString(), fileSize, extension, fileName);
            physicalPath = Path.Combine(directory, fileName);
            return physicalPath;
        }

        public ActionResult GetImgFileName(string fieldName, string typeName)
        {
            return Json(Session["Uploadfile-" + typeName + "-" + fieldName], JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerateBasicReports()
        {
            string message;
            var isSuccess = false;

            //Reports


            //Personnel
            /*
            CreateReport("تقرير نسب الاناث والذكور", "PersonnelReport1", ModulesNames.Personnel);
            //CreateReport("تقرير نسب الاناث والذكور لكامل المؤسسة ", "PersonnelReport1.1.frx", ModuleName.Personnel);//اضافي عن الدراسة التحليلية
            CreateReport("تقرير توزع الاناث والذكور حسب الاقسام", "PersonnelReport2.frx", ModulesNames.Personnel);
            CreateReport("تقرير نسب زمرة الدم ضمن المؤسسة", "PersonnelReport3.frx", ModulesNames.Personnel);
            //CreateReport("تقرير الموظفين حسب زمرة الدم", "PersonnelReport4.frx", ModuleName.Personnel);//Create By Fast Report
            CreateReport("تقرير الموظفين حسب زمرة الدم", "PSPersonnelReport4", ModulesNames.Personnel);
            CreateReport("تقرير نسب ديانة الموظفين", "PersonnelReport5", ModulesNames.Personnel);
            CreateReport("تقرير توزع الديانة حسب الاقسام", "PersonnelReport6.frx", ModulesNames.Personnel);
            CreateReport("نسب الحالة الاجتماعية ضمن المؤسسة", "PersonnelReport7.frx", ModulesNames.Personnel);
            //CreateReport("تقرير الموظفين حسب الحالة الاجتماعية", "PersonnelReport8.frx", ModuleName.Personnel);//Create By Fast Report
            //CreateReport("تقرير الموظفين حسب مجال العمر", "PersonnelReport9.frx", ModuleName.Personnel);//Create By Fast Report
            //CreateReport("تقرير الموظفين حسب بلد الولادة", "PersonnelReport10.frx", ModuleName.Personnel);//Create By Fast Report
            CreateReport("تقرير الموظفين حسب بلد الولادة", "PSPersonnelReport10", ModulesNames.Personnel);
            CreateReport("تقرير عناوين الموظفين", "PersonnelReport11.frx", ModulesNames.Personnel);
            CreateReport("تقرير نسب الجنسية الاساسية ضمن المؤسسة", "PersonnelReport12.frx", ModulesNames.Personnel);
            //CreateReport("تقرير الموظفين حسب الجنسية الاساسية", "PersonnelReport13.frx", ModuleName.Personnel);//Create By Fast Report
            CreateReport("تقرير نسب الجنسية الاخرى ضمن المؤسسة", "PersonnelReport14.frx", ModulesNames.Personnel);
            //CreateReport("تقرير الموظفين حسب الجنسية الأخرى", "PersonnelReport15.frx", ModuleName.Personnel);//Create By Fast Report
            //CreateReport("تقرير الموظفين حسب حالة خدمة العلم", "PersonnelReport16.frx", ModuleName.Personnel);//Create By Fast Report
            CreateReport("تقرير الموظفين حسب حالة خدمة العلم", "PSPersonnelReport16", ModulesNames.Personnel);
            CreateReport("تقرير الموظفين الغير مسجلين بالتأمينات الاجتماعية", "PersonnelReport18.frx", ModulesNames.Personnel);
            CreateReport("تقرير نسب العجز ضمن المؤسسة", "PersonnelReport19.frx", ModulesNames.Personnel);
            CreateReport("تقرير الموظفين ذوي الاحتياجات الخاصة", "PersonnelReport20.frx", ModulesNames.Personnel);
            //CreateReport("تقرير الموظفين حسب تاريخ انتهاء جواز السفر", "PersonnelReport21.frx", ModuleName.Personnel);//Create By Fast Report
            CreateReport("تقرير الموظفين حسب تاريخ انتهاء جواز السفر", "PSPersonnelReport21", ModulesNames.Personnel);
            //CreateReport("تقرير الموظفين حسب تاريخ انتهاء الاقامة", "PersonnelReport22.frx", ModuleName.Personnel);//Create By Fast Report
            CreateReport("تقرير الموظفين حسب تاريخ انتهاء الاقامة", "PSPersonnelReport22", ModulesNames.Personnel);
            //CreateReport("تقرير الموظفين حسب تاريخ انتهاء شهادة القيادة", "PersonnelReport23.frx", ModuleName.Personnel);//Create By Fast Report
            CreateReport("تقرير الموظفين حسب تاريخ انتهاء شهادة القيادة", "PSPersonnelReport23", ModulesNames.Personnel);
            //CreateReport("تقرير بيانات الشريك", "PersonnelReport24.frx", ModuleName.Personnel);//Create By Fast Report
            //CreateReport("تقرير الموظفين حسب عدد الاولاد", "PersonnelReport25.frx", ModuleName.Personnel);//Create By Fast Report
            CreateReport("تقرير الموظفين حسب عدد الاولاد", "PSPersonnelReport25", ModulesNames.Personnel);
            //CreateReport("تقرير الموظفين حسب عدد المعالين", "PersonnelReport26.frx", ModuleName.Personnel);//Create By Fast Report
            CreateReport("تقرير الموظفين حسب عدد المعالين", "PSPersonnelReport26", ModulesNames.Personnel);
            //CreateReport("تقرير بيانات المستوى التعليمي", "PersonnelReport27.frx", ModuleName.Personnel);//Create By Fast Report
            CreateReport("تقرير بيانات المستوى التعليمي", "PSPersonnelReport27", ModulesNames.Personnel);
            //CreateReport("تقرير بيانات الشهادة", "PersonnelReport28.frx", ModuleName.Personnel);//Create By Fast Report
            CreateReport("تقرير بيانات الشهادة", "PSPersonnelReport28", ModulesNames.Personnel);
            //CreateReport("تقرير الموظفين حسب مجال سنوات الخبرة", "PersonnelReport29.frx", ModuleName.Personnel);//Create By Fast Report
            CreateReport("تقرير الموظفين حسب مجال سنوات الخبرة", "PSPersonnelReport29", ModulesNames.Personnel);
            CreateReport("تقرير بيانات الدورات التدريبية", "PersonnelReport30.frx", ModulesNames.Personnel);
            //CreateReport("تقرير الموظفين حسب بيانات اللغة", "PersonnelReport31.frx", ModuleName.Personnel);//Create By Fast Report
            CreateReport("تقرير الموظفين حسب بيانات اللغة", "PSPersonnelReport31", ModulesNames.Personnel);
            CreateReport("تقرير الموظفين المحكومين", "PersonnelReport32.frx", ModulesNames.Personnel);
            //CreateReport("تقرير تعويضات الموظف", "PersonnelReport33.frx", ModuleName.Personnel);//Create By Fast Report
            CreateReport("تقرير تعويضات الموظف", "PSPersonnelReport33", ModulesNames.Personnel);
            //CreateReport("تقرير العهد للموظف", "PersonnelReport34.frx", ModuleName.Personnel);//Create By Fast Report
            CreateReport("تقرير العهد للموظف", "PSPersonnelReport34", ModulesNames.Personnel);
            //CreateReport("تقرير الموظفين حسب الضمان الصحي", "PersonnelReport35.frx", ModuleName.Personnel);//Create By Fast Report
            CreateReport("تقرير الموظفين حسب الضمان الصحي", "PSPersonnelReport35", ModulesNames.Personnel);
            //CreateReport("تقرير المعلومات المصرفية للموظفين", "PersonnelReport36.frx", ModuleName.Personnel);//Create By Fast Report
            CreateReport("تقرير المعلومات المصرفية للموظفين", "PSPersonnelReport36", ModulesNames.Personnel);
            //CreateReport("تقرير الموظفين تحت التجربة", "PersonnelReport37.frx", ModuleName.Personnel);//Create By Fast Report
            CreateReport("تقرير الموظفين تحت التجربة", "PSPersonnelReport37", ModulesNames.Personnel); 
            */
            
            CreateReport("Employees By Marital Status", "EmployeesByMaritalStatus", ModulesNames.Personnel); 
            CreateReport("Spouse Data", "SpouseData", ModulesNames.Personnel); 
            CreateReport("Employees By Age Range", "EmployeesByAgeRange", ModulesNames.Personnel); 
            CreateReport("Employees By Country Of Birth", "EmployeesByCountryOfBirth", ModulesNames.Personnel); 
            CreateReport("Employees By Primary Nationality", "EmployeesByPrimaryNationality", ModulesNames.Personnel);
            CreateReport("Employees By Other Nationality", "EmployeesByOtherNationality", ModulesNames.Personnel); 
            CreateReport("Employees By Military Status", "EmployeesByMilitaryStatus", ModulesNames.Personnel); 
            CreateReport("Employees Social Insurance Information", "EmployeesSocialInsuranceInformation", ModulesNames.Personnel);
            CreateReport("Addresses Of Employees", "AddressesOfEmployees", ModulesNames.Personnel); 
            CreateReport("Employees With Disability", "EmployeesWithDisability", ModulesNames.Personnel);
            CreateReport("Employees By Passport Expiry Date", "EmployeesByPassportExpiryDate", ModulesNames.Personnel);
            CreateReport("Employees By Residency Expiry Date", "EmployeesByResidencyExpiryDate", ModulesNames.Personnel);
            CreateReport("Employees By Driving License Expiry Date", "EmployeesByDrivingLicenseExpiryDate", ModulesNames.Personnel);
            CreateReport("Employees By Number Of Children", "EmployeesByNumberOfChildren", ModulesNames.Personnel);
            CreateReport("Employees By Number Of Dependents", "EmployeesByNumberOfDependents", ModulesNames.Personnel);//
            CreateReport("Education Level Data", "EducationLevelData", ModulesNames.Personnel);
            CreateReport("Certificate Data", "CertificateData", ModulesNames.Personnel);
            CreateReport("Employees By Years Of Experience", "EmployeesByYearsOfExperience", ModulesNames.Personnel);
            CreateReport("Employee Training Data", "TrainingData", ModulesNames.Personnel);
            CreateReport("Employees By Languages", "EmployeesByLanguage", ModulesNames.Personnel);
            CreateReport("Convicted Employees", "ConvictedEmployees", ModulesNames.Personnel);
            CreateReport("Employees Benefits", "EmployeesBenefits", ModulesNames.Personnel);
            CreateReport("Employee Custodies By Node", "EmployeeCustodiesByNode", ModulesNames.Personnel);
            CreateReport("Employees By Health Insurance", "EmployeesByHealthInsurance", ModulesNames.Personnel);
            CreateReport("Banking Information", "BankingInformationReport", ModulesNames.Personnel);
            CreateReport("Employees Of Under Probation", "EmployeesOfUnderProbation", ModulesNames.Personnel);
            CreateReport("Employees Custodies", "EmployeesCustodies", ModulesNames.Personnel);

            //Organization Chart 
            //Create By Fast Report
            //CreateReport("تقرير الهيكل التنظيمي", "OrganizationChartReport1.frx", ModuleName.OrganizationChart);
            //CreateReport("تقرير الهيكل التنظيمي مع الفئات", "OrganizationChartReport2.frx", ModuleName.OrganizationChart);
            //Create By SSRS
            CreateReport("Organization Chart", "PSOrganizationChartReport1", ModulesNames.OrganizationChart);
            CreateReport("Organization Chart With Grade", "PSOrganizationChartReport2", ModulesNames.OrganizationChart);

            //Grade
            //Create By Fast Report
            //CreateReport("تقرير الموظفين حسب الفئات", "GradeReport1.frx", ModuleName.Grade);
            //CreateReport("تقرير الموظفين الغير مرتبطين بفئة", "GradeReport2.frx", ModuleName.Grade);
            //CreateReport("تقرير سلم الاجور حسب الفئات", "GradeReport3.frx", ModuleName.Grade);
            //CreateReport("تقرير تعويضات الفئة", "GradeReport4.frx", ModuleName.Grade);
            //Create By SSRS
            CreateReport("Employees By Grades", "PSGradeReport1", ModulesNames.Grades);
            CreateReport("Employees not Related To A Grade", "PSGradeReport2", ModulesNames.Grades);
           // CreateReport("تقرير سلم الاجور حسب الفئات", "PSGradeReport3", ModulesNames.Grade);
            CreateReport("Grade Benefits", "PSGradeReport4", ModulesNames.Grades);

            //Job Description
            //Create By Fast Report
            //CreateReport("تقرير قائمة الوصف الوظيفي", "JobDescriptionReport1.frx", ModuleName.JobDescription);
            //CreateReport("تقرير المواقع الوظيفية حسب الوصف الوظيفي", "JobDescriptionReport2.frx", ModuleName.JobDescription);
            //CreateReport("تقرير المواقع الوظيفية للموظف", "JobDescriptionReport3.frx", ModuleName.JobDescription);
            //CreateReport("تقرير الموظفين الغير معينين بموقع وظيفي", "JobDescriptionReport4.frx", ModuleName.JobDescription);
            //CreateReport("تقرير كفاءات الوصف الوظيفي", "JobDescriptionReport5.frx", ModuleName.JobDescription);
            //CreateReport("توليد تقرير مطابقة المهارات", "JobDescriptionReport6.frx", ModuleName.JobDescription);
            //CreateReport("تقرير تاريخ الموقع الوظيفي", "JobDescriptionReport7.frx", ModuleName.JobDescription);
            //CreateReport("تقرير المواقع الوظيفية الشاغرة", "JobDescriptionReport8.frx", ModuleName.JobDescription);
            //CreateReport("تقرير المواقع الوظيفية الجديدة", "JobDescriptionReport9.frx", ModuleName.JobDescription);
            //Create By SSRS
            CreateReport("Job Description List", "PSJobDescriptionReport", ModulesNames.JobDescription);
            CreateReport("Positions According To JobDescription", "PSJobDescriptionReport2", ModulesNames.JobDescription);
            CreateReport("Employee Positions", "PSJobDescriptionReport3", ModulesNames.JobDescription);
            CreateReport("Employees NotAssigned To Position", "PSJobDescriptionReport4", ModulesNames.JobDescription);
            CreateReport("Job Description Competencies", "PSJobDescriptionReport5", ModulesNames.JobDescription);
            CreateReport("Matching Skills", "PSJobDescriptionReport6", ModulesNames.JobDescription);
            CreateReport("Position History", "PSJobDescriptionReport7", ModulesNames.JobDescription);
            CreateReport("Vacant Positions", "PSJobDescriptionReport8", ModulesNames.JobDescription);
            CreateReport("New Positions", "PSJobDescriptionReport9", ModulesNames.JobDescription);

            //Payroll System
            CreateReport("Employees Finance Details", "PSPayrollSystem1", ModulesNames.PayrollSystem);
            CreateReport("Employees Has Primary Deductions", "PSPayrollSystem2", ModulesNames.PayrollSystem);
            CreateReport("Employees Has Primary Benefits", "PSPayrollSystem3", ModulesNames.PayrollSystem);
            CreateReport("Employees Loans", "PSPayrollSystem4", ModulesNames.PayrollSystem);
            CreateReport("Month Deductions", "PSPayrollSystem5", ModulesNames.PayrollSystem);
            CreateReport("Month Benefits", "PSPayrollSystem6", ModulesNames.PayrollSystem);
            CreateReport("Monthly Salaries", "PSPayrollSystem11", ModulesNames.PayrollSystem);
            CreateReport("Month Loans", "PSPayrollSystem8", ModulesNames.PayrollSystem);
            CreateReport("Employees Has Non Primary Deductions", "PSPayrollSystem9", ModulesNames.PayrollSystem);
            CreateReport("Employee Incomes", "PSPayrollSystem10", ModulesNames.PayrollSystem);
            CreateReport("Employees Has Non PrimaryBenefits", "PSPayrollSystem12", ModulesNames.PayrollSystem);
            //CreateReport("تقرير الرواتب الشهرية", "PSPayrollSystem11", ModulesNames.PayrollSystem);
            //CreateReport("تقرير مقبوضات موظف", "PSPayrollSystem10", ModulesNames.PayrollSystem);

            //Security
            CreateReport("Roles Related To Users", "Security_1", ModulesNames.Security);
            CreateReport("Users Related To Roles", "Security_2", ModulesNames.Security);
            CreateReport("Roles Responsibilities", "Security_3", ModulesNames.Security);
            //Attendance System
            //Create By Fast Report
            //CreateReport("تقرير الموظفين حسب نموذج الدوام", "AttendanceSystemReport1.frx", ModuleName.AttendanceSystem);
            //CreateReport("تقرير تعديلات قراءات الدوام", "AttendanceSystemReport2.frx", ModuleName.AttendanceSystem);
            //CreateReport("تقرير احصائيات الدوام - تقاص شهري", "AttendanceSystemReport3.frx", ModuleName.AttendanceSystem);
            //CreateReport("تقرير احصائيات الدوام - تقاص يومي", "AttendanceSystemReport4.frx", ModuleName.AttendanceSystem);
            //CreateReport("تقرير احصائيات الدوام - بدون تقاص", "AttendanceSystemReport5.frx", ModuleName.AttendanceSystem);
            //CreateReport("تقرير الدوام لموظف", "AttendanceSystemReport6.frx", ModuleName.AttendanceSystem);
            //Create By SSRS
            CreateReport("Employee Card Attendance Form", "PSAttendanceSystemReport1", ModulesNames.AttendanceSystem);
            CreateReport("Attendance Readings Updates", "PSAttendanceSystemReport2", ModulesNames.AttendanceSystem);
            CreateReport("Attendance Statistics - Monthly Adjustment", "PSAttendanceSystemReport3", ModulesNames.AttendanceSystem);
            CreateReport("Attendance Statistics - Daily Adjustment", "PSAttendanceSystemReport4", ModulesNames.AttendanceSystem);
            CreateReport("Attendance Statistics - Without Adjustment", "PSAttendanceSystemReport5", ModulesNames.AttendanceSystem);
            CreateReport("Employee Attendance", "PSAttendanceSystemReport6", ModulesNames.AttendanceSystem);
            CreateReport("Employee Personal Attendance", "PSAttendanceSystemReport7", ModulesNames.AttendanceSystem);
            CreateReport("Entry And Exit Records For An Employee", "PSAttendanceRecords1", ModulesNames.AttendanceSystem);
            CreateReport("Employee Attendance According To Employee Entry and Exit Records", "PSAttendanceRecords2", ModulesNames.AttendanceSystem);
            CreateReport("Attendance Report ", "PSAttandance8_CurrentUser", ModulesNames.AttendanceSystem);
            CreateReport("Entry and Exit Records ", "PS_Attendance_EERecords7", ModulesNames.AttendanceSystem);

            //Employee Relation Services
            //Create By Fast Report
            //CreateReport("تقرير رصيد اجازات موظف", "EmployeeRelationServicesReport1.frx", ModuleName.EmployeeRelationServices);
            //CreateReport("تقرير اجازات موظف بين تاريخين", "EmployeeRelationServicesReport2.frx", ModuleName.EmployeeRelationServices);
            //CreateReport("تقرير رصيد اجازات موظفي عقدة", "EmployeeRelationServicesReport3.frx", ModuleName.EmployeeRelationServices);
            //CreateReport("تقرير اجازات موظفي عقدة بين تاريخين", "EmployeeRelationServicesReport4.frx", ModuleName.EmployeeRelationServices);
            //CreateReport("تقرير عمليات النقل والترقية لموظف", "EmployeeRelationServicesReport5.frx", ModuleName.EmployeeRelationServices);
            //CreateReport("تقرير تاريخ الموظف", "EmployeeRelationServicesReport6.frx", ModuleName.EmployeeRelationServices);
            //CreateReport("تقرير الموظفين المستقيلين", "EmployeeRelationServicesReport7.frx", ModuleName.EmployeeRelationServices);
            //CreateReport("تقرير الموظفين المنتهية خدمتهم", "EmployeeRelationServicesReport8.frx", ModuleName.EmployeeRelationServices);
            //Create By SSRS
            CreateReport("Employee leave Balance", "PSEmployeeRelationServices1", ModulesNames.EmployeeRelationServices);
            CreateReport("Employee Leaves Between Two Dates", "PSEmployeeRelationServices2", ModulesNames.EmployeeRelationServices);
            CreateReport("Leave Balance of Node Employees", "PSEmployeeRelationServices3", ModulesNames.EmployeeRelationServices);
            CreateReport("Node Employees Leaves Between Two Dates", "PSEmployeeRelationServices4", ModulesNames.EmployeeRelationServices);
            CreateReport("Employee Transfers and Promotions Operations", "PSEmployeeRelationServices5", ModulesNames.EmployeeRelationServices);
            CreateReport("Employee History", "PSEmployeeRelationServices6", ModulesNames.EmployeeRelationServices);
            CreateReport("Resigned Employees", "PSEmployeeRelationServices7", ModulesNames.EmployeeRelationServices);
            CreateReport("End-service Employees", "PSEmployeeRelationServices8", ModulesNames.EmployeeRelationServices);
            CreateReport("UnAssigned Employees", "PSEmployeeRelationServices9", ModulesNames.EmployeeRelationServices);
            CreateReport("Leaves Between Two Dates", "PSEmployeeRelationServices_CurrentEmployee", ModulesNames.EmployeeRelationServices);
            //PMS
            //Create By Fast Report
            //CreateReport("تقرير تقييم الموظفين التابعين لعقدة معينة خلال مرحلة معينة ", "PMSReport1.frx", ModuleName.PMS);
            //Create By SSRS
            //CreateReport("تقرير تقييم الموظفين التابعين لعقدة معينة خلال مرحلة معينة ", "PSPMSReport1", ModulesNames.PMS);
            CreateReport("Performance Appraisal Report", "PMSApprisal", ModulesNames.PMS);
            CreateReport("EmployeeAppraisalBetweenTwoDates", "PMSApprisal2", ModulesNames.PMS);

            //Training
            CreateReport("Training Plan Information", "TrainingPlanInformation", ModulesNames.Training);
            CreateReport("Training Need Information", "TrainingNeedInformation", ModulesNames.Training);
            CreateReport("Course Information", "CourseInformation", ModulesNames.Training);
            CreateReport("Appraisal Of Course Trainees", "AppraisalOfCourseTrainees", ModulesNames.Training);
            CreateReport("Appraisal Of Training Courses", "AppraisalOfTrainingCourses", ModulesNames.Training);
            CreateReport("Training Courses Costs", "TrainingCoursesCosts", ModulesNames.Training);
            CreateReport("Course Candidates Information", "CourseCandidatesInformation", ModulesNames.Training);
            CreateReport("Course Trainees Information", "CourseTraineesInformation", ModulesNames.Training);
            CreateReport("Employee Training History", "EmployeeTrainingHistory", ModulesNames.Training);

            var lan = ServiceFactory.ORMService.All<Language>().FirstOrDefault(x => x.IsActive);
            if (lan != null)
            {
                var generator = new NHibernateResourceGenerator();
                generator.GenerateResourceForReports(lan.Id);
            }
            message = GlobalResource.Success;
            isSuccess = true;
            
            return Json(new
            {
                Success = isSuccess,
                Msg = message,
            });
        }

        public ActionResult Download()
        {
            return Content(ReportingHelper.DownloadReports());
        }
        public ActionResult Deploy()
        {
            bool b;
            return Content(ReportingHelper.SynchronizeReports(out b));
        }
        public static void CreateReport(string name, string fileName, string moduleName)
        {
            ReportDefinition reportDefinition =
                ServiceFactory.ORMService.All<ReportDefinition>().FirstOrDefault(x => x.Title == name && x.FileName == fileName && x.ModuleName == moduleName);
            if (reportDefinition == null)
            {
                var report = new ReportDefinition()
                {
                    Title = name,
                    ModuleName = moduleName,
                    CreationDate = DateTime.Now,
                    FileName = fileName,

                }.Save();
                // add report to authorizableReports
                BuildNavigation.GetModule(moduleName).UpdateReport();
            }
        }

        public ActionResult GetModules()
        {
            var navigationTabs = BuildNavigation.CurrentNavigation.GetNavigationTab();
            var modules = navigationTabs.SelectMany(navigationTab => navigationTab.Modules).Select(x => new
            {
                Id = x.ModuleId,
                Name = x.ModuleId,
                ModuleName = x.ModuleId
            }).ToList();

            return Json(new { Data = modules }, JsonRequestBehavior.AllowGet);
        }
    }
}
