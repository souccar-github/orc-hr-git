using HRIS.Domain.AttendanceSystem.Enums;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using LinqToExcel;
using LinqToExcel.Attributes;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers.Resource;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.Controllers
{
    public class ServiceController : Controller
    {
        //
        // GET: /AttendanceSystem/Service/
        private static List<Employee> employees = new List<Employee>();
        public ActionResult ImportEntranceExitRecordsFromExcel()
        {
            return PartialView();
        }
        public ActionResult ImportEntranceExitRecords(IEnumerable<HttpPostedFileBase> files)
        {
            var isSuccess = false;
            var message = "Failed";
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileName = string.Format("{0}{1}", Guid.NewGuid(), Path.GetExtension(file.FileName));
                    if (!fileName.Contains("xlsx"))
                    {
                        isSuccess = false;
                        message = GlobalResource.TheFileExtensionIsInvalid;
                        break;
                    }
                    //if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Content/ApplicantsScores"), fileName)))
                    //    System.IO.File.Delete(Path.Combine(Server.MapPath("~/Content/ApplicantsScores"), fileName));

                    Session["ExcelFilePhysicalPath"] = Path.Combine(System.IO.Path.GetTempPath(), fileName);
                    Session["fileName"] = fileName;
                    file.SaveAs(Session["ExcelFilePhysicalPath"].ToString());
                    //SaveIncedenceDefinitions();
                    isSuccess = true;
                    message = "";
                }
            }
            return Json(new
            {
                Success = isSuccess,
                Msg = message
            });
        }

        public ActionResult SaveEntranceExitRecords()
        {
            var start = DateTime.Now;
            var isSuccess = false;
            try
            {
                var fingerprintRecords = new List<FingerprintTransferredData>();
                var excel = new ExcelQueryFactory(Session["ExcelFilePhysicalPath"].ToString());
                var records = from record in excel.Worksheet<FingerPrintModel>("Records").AsEnumerable()
                              select record;
                employees = ServiceFactory.ORMService.All<Employee>().ToList();
                var num = 1;
                foreach (var record in records)
                {
                    num++;
                    var success = false;
                    var msg = "";

                    if (string.IsNullOrEmpty(record.EmployeeName))
                        continue;
                    var fingerprints = new List<FingerprintTransferredData>();
                    if (!CheckTheRecordValues(record, num, out fingerprints, out success, out msg))
                        return Json(new
                        {
                            Success = success,
                            Msg = msg
                        });
                    fingerprintRecords.AddRange(fingerprints);
                }
                var info = AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.ImportEntranceExitRecordsFromExcel);
                ServiceFactory.ORMService.SaveTransaction<FingerprintTransferredData>(fingerprintRecords, UserExtensions.CurrentUser, null, Souccar.Domain.Audit.OperationType.Update, info, start, null);

                AttendanceSystem.Services.AttendanceService.HandlingFingerPrintsDataAfterPulling(InsertSource.FromExcel);
                return Json(new
                {
                    Success = true,
                    Msg = AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.Done)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = isSuccess,
                    Msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message
                });
            }
        }
        public bool CheckTheRecordValues(FingerPrintModel record, int num, out List<FingerprintTransferredData> records, out bool success, out string msg)
        {
            success = false;
            msg = "";
            records = new List<FingerprintTransferredData>();
            var fingerprint = new FingerprintTransferredData();
            try
            {
                var defaultDate = new DateTime(2000, 1, 1, 0, 0, 0);
                var employeeValue = record.EmployeeName.Split('-').Count() > 1 &&
                    record.EmployeeName.Split('-').Count() < 3 ?
                    record.EmployeeName.Split('-')[1] :
                    null;
                var employee = string.IsNullOrEmpty(employeeValue) && employees.FirstOrDefault(x => Regex.Replace(x.FullName, @"\s+", "").ToLower() == Regex.Replace(record.EmployeeName, @"\s+", "").ToLower()) != null ?
                    employees.FirstOrDefault(x => Regex.Replace(x.FullName, @"\s+", "").ToLower() == Regex.Replace(record.EmployeeName, @"\s+", "").ToLower() ) :
                    string.IsNullOrEmpty(employeeValue) && employees.FirstOrDefault(x => Regex.Replace(x.FullName, @"\s+", "").ToLower() == Regex.Replace(record.EmployeeName, @"\s+", "").ToLower()) == null ?
                    employees.FirstOrDefault(x => 
                    (Regex.Replace(Regex.Replace(Regex.Replace(x.FullNameL2, @"(?<!<!--.*?)" + "ة", "ه"), @"(?<!<!--.*?)" + "أ", "ا"), @"\s+", "") == Regex.Replace(Regex.Replace(Regex.Replace(record.EmployeeName, @"(?<!<!--.*?)" + "ة", "ه"), @"(?<!<!--.*?)" + "أ", "ا"), @"\s+", "")) ||
                    (Regex.Replace(Regex.Replace(Regex.Replace(x.FullNameL2, @"(?<!<!--.*?)" + "ة", "ه"), @"(?<!<!--.*?)" + "إ", "ا"), @"\s+", "") == Regex.Replace(Regex.Replace(Regex.Replace(record.EmployeeName, @"(?<!<!--.*?)" + "ة", "ه"), @"(?<!<!--.*?)" + "إ", "ا"), @"\s+", "")) ||
                    (Regex.Replace(Regex.Replace(Regex.Replace(x.FullNameL2, @"(?<!<!--.*?)" + "ة", "ه"), @"(?<!<!--.*?)" + "أ", "ا"), @"\s+", "") == Regex.Replace(Regex.Replace(Regex.Replace(record.EmployeeName, @"(?<!<!--.*?)" + "ة", "ه"), @"(?<!<!--.*?)" + "أ", "ا"), @"\s+", ""))  ||
                    (Regex.Replace(Regex.Replace(Regex.Replace(x.FullNameL2, @"(?<!<!--.*?)" + "ة", "ه"), @"(?<!<!--.*?)" + "إ", "ا"), @"\s+", "") == Regex.Replace(Regex.Replace(Regex.Replace(record.EmployeeName, @"(?<!<!--.*?)" + "ة", "ه"), @"(?<!<!--.*?)" + "إ", "ا"), @"\s+", ""))):
                    employees.FirstOrDefault(x => x.Username == employeeValue);
                if (!string.IsNullOrEmpty(record.EnterLogTime) && employee == null)
                {
                    msg = AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.EmployeeNameIsNotValidInTheRowWhichNumberIs) + " " + num;
                    return false;
                }
                var logTimeValue = string.IsNullOrEmpty(record.EnterLogTime) ? defaultDate : DateTime.Parse(record.EnterLogTime);

                var logDateValue = !string.IsNullOrEmpty(record.LogDate) ? DateTime.Parse(record.LogDate) : defaultDate;
                if (string.IsNullOrEmpty(record.LogDate) && logDateValue == defaultDate)
                {
                    msg = AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.LogDateIsNotValidInTheRowWhichNumberIs) + " " + num;
                    return false;
                }
                if(logTimeValue != defaultDate)
                {
                    fingerprint = new FingerprintTransferredData()
                    {
                        Employee = employee,
                        LogType = (HRIS.Domain.AttendanceSystem.Enums.LogType) 0,
                        LogDateTime = new DateTime(logDateValue.Year, 
                                      logDateValue.Month,
                                      logDateValue.Day,
                                      logTimeValue.Hour,
                                      logTimeValue.Minute,
                                      logTimeValue.Second),
                        IsLogTypeIgnored = true,
                        IsTransfered = false,
                        IsOld = false
                    };
                    records.Add(fingerprint);
                }
                logTimeValue = string.IsNullOrEmpty(record.ExitLogTime) ? defaultDate : DateTime.Parse(record.ExitLogTime);
                //if (!string.IsNullOrEmpty(record.ExitLogTime) && logTimeValue == defaultDate)
                //{
                //    msg = AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.LogTimeIsNotValidInTheRowWhichNumberIs) + " " + num;
                //    return false;
                //}
                if (logTimeValue != defaultDate)
                {
                    fingerprint = new FingerprintTransferredData()
                    {
                        Employee = employee,
                        LogType = (HRIS.Domain.AttendanceSystem.Enums.LogType)1,
                        LogDateTime = new DateTime(logDateValue.Year,
                                      logDateValue.Month,
                                      logDateValue.Day,
                                      logTimeValue.Hour,
                                      logTimeValue.Minute,
                                      logTimeValue.Second),
                        IsLogTypeIgnored = true,
                        IsTransfered = false,
                        IsOld = false
                    };
                    records.Add(fingerprint);
                }
                success = true;
                return true;
            }
            catch (Exception ex)
            {
                success = false;
                msg = ex.InnerException != null ? ex.InnerException.Message + " In The Row Which Number Is " + num : ex.Message + " In The Row Which Number Is " + num;
                return false;
            }
        }
    }

    public class FingerPrintModel
    {

        [ExcelColumn("التاريخ")]
        public string LogDate { get; set; }

        [ExcelColumn("اسم الموظف")]
        public string EmployeeName { get; set; }

        [ExcelColumn("دخول")]
        public string EnterLogTime { get; set; }
        [ExcelColumn("خروج")]
        public string ExitLogTime { get; set; }
        [ExcelColumn("ملاحظات")]
        public string Note { get; set; }

    }

}

