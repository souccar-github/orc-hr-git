using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.JobDescription.Entities;

using HRIS.Domain.PMS.RootEntities;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Infrastructure.Core;
using Souccar.Domain.Notification;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Extenstions;
using  Project.Web.Mvc4.Areas.PMS.Models;
using HRIS.Domain.Global.Enums;
using  Project.Web.Mvc4.Areas.EmployeeRelationServices.Services;
using  Project.Web.Mvc4.Helpers.Resource;
using Souccar.Domain.Validation;
using Souccar.Domain.DomainModel;
using  Project.Web.Mvc4.Helpers;
using HRIS.Domain.PMS.Entities;
using  Project.Web.Mvc4.Extensions;
using HRIS.Domain.PMS.Configurations;


namespace Project.Web.Mvc4.Areas.PMS.Controllers
{
    public class PromotionController : Controller
    {
        public ActionResult GetPromotionsSettings()
        {
            var promotionsSettings = ServiceFactory.ORMService.All<PromotionsSettings>();

            var result = new ArrayList();
            foreach (var item in promotionsSettings)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = item.Id;
                temp["Name"] = item.NameForDropdown;
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployees()
        {
            var employees = ServiceFactory.ORMService.All<Employee>();

            var result = new ArrayList();
            foreach (var employee in employees)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = employee.Id;
                temp["Name"] = employee.NameForDropdown;
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetEmployeesPromotion(int id, string promotionsDate, double promotionRate)
        //{
        //    var result = new List<EmployeePromotionViewModel>();
            
        //    if (id == 0)
        //        return Json(new { Data = result });

        //    var promotionsSettings = ServiceFactory.ORMService.GetById<PromotionsSettings>(id) ??
        //     ServiceFactory.ORMService.All<PromotionsSettings>().LastOrDefault();

        //    var employees = ServiceFactory.ORMService.All<Employee>().ToList();

        //    if (!employees.Any())
        //        return Json(result, JsonRequestBehavior.AllowGet);

        //    var isPromotionBlocking=false;
        //    var note = "";
        //    foreach (var employee in employees)
        //    {
        //        var employeePrimaryCard = ServiceFactory.ORMService.All<EmployeeCard>().ToList().FirstOrDefault(x => x.Employee == employee);
        //        var salary = 0.0;
        //        if (employeePrimaryCard != null)
        //        {
        //            salary = employeePrimaryCard.Salary;
        //        }

        //        var position = employee.PrimaryPosition();
        //        var JobTitle="";
        //        var JobDescription="";
        //        var maxSalaryIsAllows = 0.0;
        //        if (position!=null)
        //        {
        //            JobTitle = employee.PrimaryPosition().JobDescription.JobTitle.Name;
        //            JobDescription = employee.PrimaryPosition().JobDescription.Name;
        //            var grade = ServiceFactory.ORMService.GetById<HRIS.Domain.Grades.RootEntities.Grade>(position.JobDescription.JobTitle.Grade.Id);
        //            if (grade!=null)
        //            {
        //                maxSalaryIsAllows = grade.MaxSalary;
        //            }                        
        //        }

        //        var avr = 0.0;

        //        if (promotionsSettings != null)
        //        {
        //            var phases = promotionsSettings.PromotionsSettingsPhases.Select(x => x.AppraisalPhase);
        //            var phaseWorkflows = phases.SelectMany(y => y.PhaseWorkflows).Where(x => x.Position == position);
        //            var count = 0;
        //            float sum = 0;
        //            foreach (var item in phaseWorkflows)
        //            {
        //                sum += item.FinalMark;
        //                count++;
        //            }
        //            avr = sum / count;
        //        }

        //        float promotionPercentage = 0;
        //        var appraisalPhaseSetting = ServiceFactory.ORMService.All<AppraisalPhaseSetting>().ToList().LastOrDefault();
        //        if (appraisalPhaseSetting.FromMarkBelowExpected <= avr && appraisalPhaseSetting.ToMarkBelowExpected >= avr)
        //        {
        //            promotionPercentage = appraisalPhaseSetting.MarkBelowExpectedPercentageOfSalaryIncrease;
        //        }
        //        else if (appraisalPhaseSetting.FromMarkDistinct <= avr && appraisalPhaseSetting.ToMarkDistinct >= avr)
        //        {
        //            promotionPercentage = appraisalPhaseSetting.MarkDistinctPercentageOfSalaryIncrease;
        //        }
        //        else if (appraisalPhaseSetting.FromMarkExpected <= avr && appraisalPhaseSetting.ToMarkExpected >= avr)
        //        {
        //            promotionPercentage = appraisalPhaseSetting.MarkExpectedPercentageOfSalaryIncrease;
        //        }
        //        else if (appraisalPhaseSetting.FromMarkUpExpected <= avr && appraisalPhaseSetting.ToMarkUpExpected >= avr)
        //        {
        //            promotionPercentage = appraisalPhaseSetting.MarkUpExpectedPercentageOfSalaryIncrease;
        //        }
        //        else if (appraisalPhaseSetting.FromMarkNeedTraining <= avr && appraisalPhaseSetting.ToMarkNeedTraining >= avr)
        //        {
        //            promotionPercentage = appraisalPhaseSetting.MarkNeedTrainingPercentageOfSalaryIncrease;
        //        }
        //        var efficiencyDegree = promotionPercentage/100;
        //        //var efficiencyDegree = 9.0 / 100;
        //        var absenceDays = AbsenceService.GetEmployeeAbsence(employee, promotionsSettings.StartDate, promotionsSettings.EndDate);//todo تؤخذ من برنامج الدوام
        //        //var dueDays = 720 - absenceDays;

        //        //حساب الفرق بين تاريخين
        //        DateTime oldDate = promotionsSettings.StartDate;
        //        DateTime newDate = promotionsSettings.EndDate;
        //        TimeSpan ts = newDate - oldDate;
        //        // Difference in days.
        //        int differenceInDays = ts.Days;
        //        var dueDays = differenceInDays - absenceDays;

        //        //var benefit = (salary*efficiencyDegree*dueDays)/720;
        //        var benefit = (salary * efficiencyDegree * dueDays) / differenceInDays;
        //        var finalSalary=salary+benefit;

        //        if(finalSalary>maxSalaryIsAllows && maxSalaryIsAllows>0)
        //        {
        //            benefit = maxSalaryIsAllows - salary;
        //            finalSalary = salary + benefit;
        //        }
                
        //        result.Add(new EmployeePromotionViewModel()
        //        {
        //            Id = employee.Id,
        //            FullName = employee.FullName,
        //            JobTitle = JobTitle,
        //            JobDescription = JobDescription,
        //            PayBeforePromotion = Math.Round(salary, 2),
        //            EfficiencyDegree = Math.Round(efficiencyDegree, 2),
        //            DaysEffectOnPromotion = dueDays,
        //            AbsenceDaysEffectOnPromotion = absenceDays,
        //            BenefitAmount = Math.Round(benefit, 2),
        //            PayAfterPromotion = Math.Round(finalSalary, 2),
        //            Note = note,
        //            IsChecked=false,
        //            IsPromotionBlocking=isPromotionBlocking
        //        });
        //    }

        //    return Json(new { Data = result });
        //}

        public ActionResult GetMilitaryEmployeesPromotion(int id, string promotionsDate, double promotionRate)
        {
            var result = new List<EmployeePromotionViewModel>();

            if (id==0)
                return Json(new { Data = result });

            var promotionsSettings = ServiceFactory.ORMService.GetById<PromotionsSettings>(id) ??
                                     ServiceFactory.ORMService.All<PromotionsSettings>().LastOrDefault();
            
            var employees = ServiceFactory.ORMService.All<Employee>().ToList();

            if (!employees.Any())
                return Json(result, JsonRequestBehavior.AllowGet);

            foreach (var employee in employees)
            {
                var employeePrimaryCard = ServiceFactory.ORMService.All<EmployeeCard>().ToList().FirstOrDefault(x => x.Employee.Id == employee.Id);
                var salary = 0.0;
                if (employeePrimaryCard != null)
                {
                    salary = employeePrimaryCard.FinancialCard.Salary;
                }

                var position = employee.PrimaryPosition();
                var JobTitle = "";
                var JobDescription = "";
                var maxSalaryIsAllows = 0.0;
                if (position != null)
                {
                    JobTitle = employee.PrimaryPosition().JobDescription.JobTitle.Name;
                    JobDescription = employee.PrimaryPosition().JobDescription.Name;
                    var grade = ServiceFactory.ORMService.GetById<HRIS.Domain.Grades.RootEntities.Grade>(position.JobDescription.JobTitle.Grade.Id);
                    if (grade != null)
                    {
                        maxSalaryIsAllows = grade.MaxSalary;
                    }
                }

                double efficiencyDegree = promotionRate / 100;
                //var absenceDays = 5;//todo تؤخذ من برنامج الدوام
                //var dueDays = 720 - absenceDays;
                //var benefit = (salary * efficiencyDegree * dueDays) / 720;
                var benefit = (salary * efficiencyDegree);
                var finalSalary = salary + benefit;

                if (finalSalary > maxSalaryIsAllows && maxSalaryIsAllows > 0)
                {
                    benefit = maxSalaryIsAllows - salary;
                    finalSalary = salary + benefit;
                }

                result.Add(new EmployeePromotionViewModel()
                {
                    Id = employee.Id,
                    FullName = employee.FullName,
                    JobTitle = JobTitle,
                    JobDescription = JobDescription,
                    PayBeforePromotion = Math.Round(salary,2),
                    EfficiencyDegree = Math.Round(efficiencyDegree,2),
                    //DaysEffectOnPromotion = dueDays,
                    //AbsenceDaysEffectOnPromotion = absenceDays,
                    BenefitAmount = Math.Round(benefit,2),
                    PayAfterPromotion = Math.Round(finalSalary,2),
                    Note = "",
                    IsChecked = false
                });
            }

            return Json(new { Data = result });
        }

        public ActionResult GetEmployeePromotion(int id, string promotionsDate, double promotionRate, int employeeId)
        {
            
            var result = new List<EmployeePromotionViewModel>();
            if (id == 0)
                return Json(new { Data = result });

            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            if (employee == null)
                return Json(result, JsonRequestBehavior.AllowGet);

            var employeePrimaryCard = ServiceFactory.ORMService.All<EmployeeCard>().ToList().FirstOrDefault(x => x.Employee.Id == employee.Id);
            var salary = 0.0;
            if (employeePrimaryCard != null)
            {
                salary = employeePrimaryCard.FinancialCard.Salary;
            }

            var position = employee.PrimaryPosition();
            var JobTitle = "";
            var JobDescription = "";
            var maxSalaryIsAllows = 0.0;
            if (position != null)
            {
                JobTitle = employee.PrimaryPosition().JobDescription.JobTitle.Name;
                JobDescription = employee.PrimaryPosition().JobDescription.Name;
                var grade = ServiceFactory.ORMService.GetById<HRIS.Domain.Grades.RootEntities.Grade>(position.JobDescription.JobTitle.Grade.Id);
                if (grade != null)
                {
                    maxSalaryIsAllows = grade.MaxSalary;
                }
            }

            double efficiencyDegree = promotionRate / 100;
            //var absenceDays = 5;//todo تؤخذ من برنامج الدوام
            //var dueDays = 720 - absenceDays;
            //var benefit = (salary * efficiencyDegree * dueDays) / 720;
            var benefit = (salary * efficiencyDegree);
            var finalSalary = salary + benefit;

            if (finalSalary > maxSalaryIsAllows && maxSalaryIsAllows > 0)
            {
                benefit = maxSalaryIsAllows - salary;
                finalSalary = salary + benefit;
            }

            result.Add(new EmployeePromotionViewModel()
            {
                Id = employee.Id,
                FullName = employee.FullName,
                JobTitle = JobTitle,
                JobDescription = JobDescription,
                PayBeforePromotion = Math.Round(salary,2),
                EfficiencyDegree = Math.Round(efficiencyDegree,2),
                //DaysEffectOnPromotion = dueDays,
                //AbsenceDaysEffectOnPromotion = absenceDays,
                BenefitAmount = Math.Round(benefit,2),
                PayAfterPromotion = Math.Round(finalSalary,2),
                Note = ""//,
                //IsChecked = true
            });

            return Json(new { Data = result });
        }

        public ActionResult GetAllJobTitle()
        {
            var jobTitles = ServiceFactory.ORMService.All<JobTitle>();
            var result = new ArrayList();
            foreach (var item in jobTitles)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = item.Id;
                temp["Name"] = item.Name;
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployeesByJobTitle(int id)
        {
            var positions = ServiceFactory.ORMService.All<Position>().Where(x => x.JobDescription.JobTitle.Id == id).ToList();
            var employees = positions.Select(x => x.Employee).Where(x => x != null);
            var result = new ArrayList();
            foreach (var item in employees)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = item.Id;
                temp["Name"] = item.FullName;
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

      

        public ActionResult GetCustomAppraisalPhases(int id,DateTime fromDate,DateTime toDate)
        {
            var result = ServiceFactory.ORMService.All<AppraisalPhase>()
                .Where(x => fromDate <= x.StartDate && toDate >= x.EndDate)
                .Select(x => new PromotionsSettingsViewModel() {
                    Id = x.Id,
                    Period = Enum.GetName(typeof(Period), x.Period),
                    OpenDate = x.StartDate.ToShortDateString(),
                    CloseDate=x.EndDate.ToShortDateString(),
                    Description=x.Description
                }).ToList();
            var promotionsSettings = ServiceFactory.ORMService.GetById<PromotionsSettings>(id);
            if (promotionsSettings != null)
            {
                foreach (var phase in promotionsSettings.PromotionsSettingsPhases)
                {
                    var temp = result.SingleOrDefault(x => x.Id == phase.AppraisalPhase.Id);
                    temp.IsIncluded = true;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetCustomAppraisalPhasesById(int id)
        {
            var promotionsSettings = ServiceFactory.ORMService.GetById<PromotionsSettings>(id);
            var result = ServiceFactory.ORMService.All<AppraisalPhase>()
                .Where(x => promotionsSettings.StartDate <= x.StartDate && promotionsSettings.EndDate >= x.EndDate)
                .Select(x => new PromotionsSettingsViewModel()
                {
                    Id = x.Id,
                    Period = Enum.GetName(typeof(Period), x.Period),
                    OpenDate = x.StartDate.ToShortDateString(),
                    CloseDate = x.EndDate.ToShortDateString(),
                    Description = x.Description
                }).ToList();

            if (promotionsSettings != null)
            {
                foreach (var phase in promotionsSettings.PromotionsSettingsPhases)
                {
                    var temp = result.SingleOrDefault(x => x.Id == phase.AppraisalPhase.Id);
                    temp.IsIncluded = true;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployeePromotionEndDate(int id)
        {
            var result = new Dictionary<string, object>();
            var firstDayInNewYear = DateTime.Today.Year;
            var promotionsSettings = ServiceFactory.ORMService.GetById<PromotionsSettings>(id);
            if (promotionsSettings != null)
                firstDayInNewYear = promotionsSettings.EndDate.Year + 1;// new DateTime(promotionsSettings.EndDate.Year + 1, 1, 1);

            result["firstDayInNewYear"] = firstDayInNewYear;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public class EmployeePromotionViewModel
        {
            public int Id { get; set; }//رقم معرف
            public string FullName { get; set; }//الاسم الثلاثي
            public string JobTitle { get; set; }//المسمى الوظيفي
            public string JobDescription { get; set; }//الوصف الوظيفي
            public double PayBeforePromotion { get; set; }//الأجر قبل الترفيع - الراتب المقطوع
            public double EfficiencyDegree { get; set; }//درجة الكفاءة
            public int DaysEffectOnPromotion { get; set; }//المدة المستحق عنها الترفيع
            public int AbsenceDaysEffectOnPromotion { get; set; }//الغياب المتوجب إنزاله من الترفيع
            public double BenefitAmount { get; set; }//مقدار العلاوة
            public double PayAfterPromotion { get; set; }//الأجر بعد الترفيع
            public string Note { get; set; }//ملاحظات
            public bool IsChecked { get; set; }//هل السجل مختار
            public bool IsPromotionBlocking  { get; set; }//حجب ترفيع

        }
    }
}
