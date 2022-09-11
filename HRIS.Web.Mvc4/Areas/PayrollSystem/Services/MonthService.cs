using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.DTO;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Validation.MessageKeys;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Services;
using Souccar.Core.Utilities;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using WebGrease.Css.Extensions;
using Project.Web.Mvc4.Extensions;
using HRIS.Domain.PayrollSystem.Configurations;
using Souccar.Domain.DomainModel;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using HRIS.Domain.AttendanceSystem.Configurations;

namespace Project.Web.Mvc4.Areas.PayrollSystem.Services
{
    public static class MonthService
    {
        public static List<HourlyMission> HourlyMissions = new List<HourlyMission>();
        public static List<TravelMission> TravelMissions = new List<TravelMission>();
        public static List<PublicHoliday> PublicHolidays = new List<PublicHoliday>();
        public static List<ChangeableHoliday> ChangeableHolidays = new List<ChangeableHoliday>();
        public static List<FixedHoliday> FixedHolidays = new List<FixedHoliday>();
        public static List<OvertimeOrder> OvertimeOrders = new List<OvertimeOrder>();
        public static GeneralOption GeneralOptionObject = new GeneralOption();
        public static Dictionary<Employee, AttendanceForm> DictionaryEmployeeAttendanceForm = new Dictionary<Employee, AttendanceForm>();
        enum ImportFrom
        {
            EmployeeRelationServices = 0,
            Attendance = 1
        }
        public static int GenerateMonth(IQueryable primaryCards, Month month)
        {
            if (primaryCards == null)
            {
                return 0;
            }

            var allPrimaryCards = (IEnumerable<EmployeeCard>)primaryCards;

            if (!allPrimaryCards.Any())
            {
                return 0;
            }
            // الشهور الشبيهة هي الشهور التي لها نفس رقم الشهر ونفس السنة وليست من نمط تعويضات فقط
            // لانه يجب التأكد عند توليد البطاقات الشهرية ان الموظف ليس له بطاقة شهرية اخرى بنفس الشهر لما يسمح وجود بطاقات اخرى بشرط انها عويضات فقط
            var similarMonths = typeof(Month).GetAll<Month>()
                .Where(x =>
                    x.Id != month.Id &&
                    x.MonthType != MonthType.BenefitOnly &&
                    x.Year == month.Year &&
                    x.MonthName == month.MonthName);

            // لمعرفة البطاقة الاساسية التي لم يتم توليد بطاقات شهرية لها سواء بنفس الشهر او بغير اشهر
            // ويكون الرابط بين الاشهر هو رقم الشهر
            // اذا كان الشهر من نوع رواتب يمنع التكرار للبطاقات الشهرية بنفس الشهر وبغير اشهر مماثلة
            // أما اذا كان الشهر من نوع تعويضات يمنع التكرار بنفس الشهر ويسمح مع غير اشهر

            IEnumerable<EmployeeCard> primaryCardsWithNoMonthlyCards;

            if (month.MonthType == MonthType.BenefitOnly)
            {
                primaryCardsWithNoMonthlyCards =
                    allPrimaryCards.Where(x => x.FinancialCard.SalaryDeservableType != SalaryDeservableType.Nothing && month.MonthlyCards.All(y => y.PrimaryCard.Id != x.Id));
            }
            else
            {
                primaryCardsWithNoMonthlyCards =
                    allPrimaryCards.Where(x => x.FinancialCard.SalaryDeservableType == SalaryDeservableType.SalaryAndBenefit &&
                                               month.MonthlyCards.All(y => y.PrimaryCard.Id != x.Id) &&
                                               similarMonths.ToList()
                                                   .SelectMany(z => z.MonthlyCards)
                                                   .All(a => a.PrimaryCard.Id != x.Id));
                //todo Mhd Alsaadi: similarMonths.ToList() تم تحويلها الى ليست لوجود مشكلة بالكيويري الكومبلكس في انهايبرنيت
            }

            var cardsWithNoMonthlyCards = primaryCardsWithNoMonthlyCards as IList<EmployeeCard> ??
                                          primaryCardsWithNoMonthlyCards.ToList();

            GeneralOptionObject = ServiceFactory.ORMService.All<GeneralOption>().FirstOrDefault();
            OvertimeOrders = ServiceFactory.ORMService.All<OvertimeOrder>().ToList();
            PublicHolidays = ServiceFactory.ORMService.All<PublicHoliday>().ToList();
            ChangeableHolidays = ServiceFactory.ORMService.All<ChangeableHoliday>().ToList();
            FixedHolidays = ServiceFactory.ORMService.All<FixedHoliday>().ToList();
            HourlyMissions = ServiceFactory.ORMService.All<HourlyMission>().
                        Where(x => x.Status == HRIS.Domain.Global.Enums.Status.Approved && !x.IsTransferedToPayroll &&
                        x.Date >= month.FromDate && x.Date <= month.ToDate)
                        .ToList();
            TravelMissions = ServiceFactory.ORMService.All<TravelMission>().
                        Where(x => x.Status == HRIS.Domain.Global.Enums.Status.Approved && !x.IsTransferedToPayroll &&
                        ((x.FromDate <= month.FromDate && x.ToDate >= month.ToDate) ||
                         (x.FromDate <= month.ToDate && x.ToDate >= month.ToDate) ||
                         (x.FromDate <= month.FromDate && x.ToDate >= month.FromDate) ||
                         (x.FromDate >= month.FromDate && x.ToDate <= month.ToDate)))
                        .ToList();
            var employees = ServiceFactory.ORMService.All<Employee>().ToList().Where(x => cardsWithNoMonthlyCards.Any(y => y.Employee.Id == x.Id));

            DictionaryEmployeeAttendanceForm = AttendanceSystem.Services.AttendanceService.GetAttendanceForms(employees.ToList());
            foreach (var primaryCardsWithNoMonthlyCard in cardsWithNoMonthlyCards)
            {
                month.AddMonthlyCard(GenerateMonthlyCard(primaryCardsWithNoMonthlyCard, month));
            }

            month.MonthStatus = MonthStatus.Generated;

            return cardsWithNoMonthlyCards.Count();
        }
        public static MonthlyCard GenerateMonthlyCard(EmployeeCard primaryCard, Month month)
        {

            //var familyBenefitOption = typeof(FamilyBenefitOption).GetAll<FamilyBenefitOption>().FirstOrDefault();

            #region Primary Info
            var currentMonthStart = new DateTime(month.FromDate.Year, month.FromDate.Month, month.FromDate.Day);
            var currentMonthEnd = new DateTime(month.ToDate.Year, month.ToDate.Month, month.ToDate.Day);
            var employeeForm = DictionaryEmployeeAttendanceForm[primaryCard.Employee];
            var hireDate = primaryCard.StartWorkingDate;
            var abruptionDate = primaryCard.EndWorkingDate;
            var daysConflictWithHireDate =
                hireDate.HasValue &&
                hireDate.Value > currentMonthStart &&
                hireDate.Value < currentMonthEnd
                    ? (hireDate.Value - currentMonthStart).TotalDays
                    : 0;
            var daysConflictWithAbruptionDate =
                abruptionDate.HasValue &&
                abruptionDate.Value > currentMonthStart &&
                abruptionDate.Value < currentMonthEnd
                    ? (abruptionDate.Value - currentMonthEnd).TotalDays
                    : 0;
            var workDays = GeneralOptionObject.TotalMonthDays - (daysConflictWithHireDate + daysConflictWithAbruptionDate);
            workDays = workDays < 0 ? 0 : workDays;

            var salary = workDays > 0
                ? RoundUtility.PreDefinedRoundValue(PreDefinedRound.ToOne,
                    primaryCard.FinancialCard.PackageSalary / GeneralOptionObject.TotalMonthDays * workDays)
                : primaryCard.FinancialCard.PackageSalary;

            var monthlyCard = new MonthlyCard
            {
                PackageSalary = (float)salary,
                Salary = primaryCard.FinancialCard.Salary,
                InsuranceSalary = primaryCard.FinancialCard.InsuranceSalary,
                TempSalary1 = primaryCard.FinancialCard.TempSalary1,
                TempSalary2 = primaryCard.FinancialCard.TempSalary2,
                BenefitSalary = primaryCard.FinancialCard.BenefitSalary,
                IsCalculated = false,
                PrimaryCard = primaryCard,
                Threshold = primaryCard.FinancialCard.Threshold,
                WorkDays = (int)workDays,
                TotalWorkingHours = primaryCard.FinancialCard.TotalWorkingHours,
                HourlyMissionValue = primaryCard.FinancialCard.HourlyMissionValue,
                InternalTravelMissionValue = primaryCard.FinancialCard.InternalTravelMissionValue,
                ExternalTravelMissionValue = primaryCard.FinancialCard.ExternalTravelMissionValue
                //AuditState = AuditState.Audited
            };

            #endregion

            #region Benefits
            #region Import Primary Benefits
            if (month.ImportPrimaryBenefits)
            {
                var primaryEmployeeBenefits =
                    primaryCard.PrimaryEmployeeBenefits.Where(x =>
                        ((x.HasExpiryDate && x.ExpiryDate > month.FromDate.Date) || x.HasExpiryDate == false) &&
                        ((x.HasStartDate && x.StartDate < month.FromDate.Date) || x.HasStartDate == false));
                foreach (var primaryEmployeeBenefit in primaryEmployeeBenefits)
                {
                    monthlyCard.AddMonthlyEmployeeBenefit(new MonthlyEmployeeBenefit
                    {
                        BenefitCard = primaryEmployeeBenefit.BenefitCard,
                        Value = primaryEmployeeBenefit.Value,
                        Formula = primaryEmployeeBenefit.Formula,
                        ExtraValue = primaryEmployeeBenefit.ExtraValue,
                        ExtraValueFormula = primaryEmployeeBenefit.ExtraValueFormula,
                        CeilValue = primaryEmployeeBenefit.CeilValue,
                        CeilFormula = primaryEmployeeBenefit.CeilFormula,
                        Note = primaryEmployeeBenefit.Note,
                        //AuditState = AuditState.Audited
                    });
                }
            }
            #endregion
            #region Benefit Distribution

            if (month.ImportBenefitDistribution)
            {
                if (primaryCard.Employee.Positions.Any())
                {
                    #region Grade
                    var employeeGrades =
                                primaryCard.Employee.Positions.Select(x => x.Position.JobDescription.JobTitle.Grade);

                    foreach (var gradeDistribution in employeeGrades)
                    {
                        foreach (var benefitInfo in gradeDistribution.GradeBenefitDetails)
                        {
                            if (monthlyCard.MonthlyEmployeeBenefits.Any(x => x.BenefitCard.Id == benefitInfo.BenefitCard.Id)) continue;
                            monthlyCard.AddMonthlyEmployeeBenefit(new MonthlyEmployeeBenefit
                            {
                                BenefitCard = benefitInfo.BenefitCard,
                                Value = benefitInfo.Value,
                                Formula = benefitInfo.Formula,
                                ExtraValue = benefitInfo.ExtraValue,
                                ExtraValueFormula = benefitInfo.ExtraValueFormula,
                                CeilValue = benefitInfo.CeilValue,
                                CeilFormula = benefitInfo.CeilFormula,
                                Note = benefitInfo.Note
                            });
                        }
                    }

                    #endregion

                    #region JobTitle
                    var employeeJobTitles =
                                primaryCard.Employee.Positions.Select(q => q.Position.JobDescription.JobTitle);

                    foreach (var jobTitleDistribution in employeeJobTitles)
                    {
                        foreach (var benefitInfo in jobTitleDistribution.JobTitleBenefitDetails)
                        {
                            if (monthlyCard.MonthlyEmployeeBenefits.Any(x => x.BenefitCard.Id == benefitInfo.BenefitCard.Id)) continue;
                            monthlyCard.AddMonthlyEmployeeBenefit(new MonthlyEmployeeBenefit
                            {
                                BenefitCard = benefitInfo.BenefitCard,
                                Value = benefitInfo.Value,
                                Formula = benefitInfo.Formula,
                                ExtraValue = benefitInfo.ExtraValue,
                                ExtraValueFormula = benefitInfo.ExtraValueFormula,
                                CeilValue = benefitInfo.CeilValue,
                                CeilFormula = benefitInfo.CeilFormula,
                                Note = benefitInfo.Note
                            });
                        }
                    }
                    #endregion

                    #region JobDescription
                    var employeeJobDescriptions =
                                primaryCard.Employee.Positions.Select(q => q.Position.JobDescription);

                    foreach (var jobDescriptionDistribution in employeeJobDescriptions)
                    {
                        foreach (var benefitInfo in jobDescriptionDistribution.JobDescriptionBenefitDetails)
                        {
                            if (monthlyCard.MonthlyEmployeeBenefits.Any(x => x.BenefitCard.Id == benefitInfo.BenefitCard.Id)) continue;
                            monthlyCard.AddMonthlyEmployeeBenefit(new MonthlyEmployeeBenefit
                            {
                                BenefitCard = benefitInfo.BenefitCard,
                                Value = benefitInfo.Value,
                                Formula = benefitInfo.Formula,
                                ExtraValue = benefitInfo.ExtraValue,
                                ExtraValueFormula = benefitInfo.ExtraValueFormula,
                                CeilValue = benefitInfo.CeilValue,
                                CeilFormula = benefitInfo.CeilFormula,
                                Note = benefitInfo.Note
                            });
                        }
                    }
                    #endregion

                    #region Position
                    var employeePositions =
                                primaryCard.Employee.Positions.Select(q => q.Position);

                    foreach (var positionDistribution in employeePositions)
                    {
                        foreach (var benefitInfo in positionDistribution.PositionBenefitDetails)
                        {
                            if (monthlyCard.MonthlyEmployeeBenefits.Any(x => x.BenefitCard.Id == benefitInfo.BenefitCard.Id)) continue;
                            monthlyCard.AddMonthlyEmployeeBenefit(new MonthlyEmployeeBenefit
                            {
                                BenefitCard = benefitInfo.BenefitCard,
                                Value = benefitInfo.Value,
                                Formula = benefitInfo.Formula,
                                ExtraValue = benefitInfo.ExtraValue,
                                ExtraValueFormula = benefitInfo.ExtraValueFormula,
                                CeilValue = benefitInfo.CeilValue,
                                CeilFormula = benefitInfo.CeilFormula,
                                Note = benefitInfo.Note
                            });
                        }
                    }
                    #endregion

                    #region Node
                    var employeeNodes =
                                primaryCard.Employee.Positions.Select(q => q.Position.JobDescription.Node);
                    foreach (var nodeDistribution in employeeNodes)
                    {
                        foreach (var benefitInfo in nodeDistribution.NodeBenefitDetails)
                        {
                            if (monthlyCard.MonthlyEmployeeBenefits.Any(x => x.BenefitCard.Id == benefitInfo.BenefitCard.Id)) continue;
                            monthlyCard.AddMonthlyEmployeeBenefit(new MonthlyEmployeeBenefit
                            {
                                BenefitCard = benefitInfo.BenefitCard,
                                Value = benefitInfo.Value,
                                Formula = benefitInfo.Formula,
                                ExtraValue = benefitInfo.ExtraValue,
                                ExtraValueFormula = benefitInfo.ExtraValueFormula,
                                CeilValue = benefitInfo.CeilValue,
                                CeilFormula = benefitInfo.CeilFormula,
                                Note = benefitInfo.Note
                            });
                        }
                    }
                    #endregion
                }
            }

            #endregion

            #region Import From Employee Relation
            if (month.ImportFromEmployeeRelation)
            {
                var rewards = PayrollIntegrationService.GetRewards(primaryCard.Employee, false);
                AddBenefit(rewards, monthlyCard, GeneralOptionObject.RewardBenefit);

                var recycledLeaves = PayrollIntegrationService.GetRecycledLeaves(primaryCard.Employee, false);
                AddBenefit(recycledLeaves, monthlyCard, GeneralOptionObject.RecycledLeaveBenefit);
            }
            #endregion

            #region Import From Attendance
            if (month.ImportFromAttendance)
            {
                var hourlyMission = PayrollIntegrationService.GetHourlyMissionsBenefit(primaryCard.Employee,
                    HourlyMissions.Where(x => x.Employee == primaryCard.Employee).ToList(), GeneralOptionObject);
                if (hourlyMission != null)
                {
                    AddBenefit(new List<PayrollSystemIntegrationDTO>() { hourlyMission }, monthlyCard, GeneralOptionObject.HourlyMissionBenefit);
                }
                var internalMission = PayrollIntegrationService.GetInternalTravelMissionsBenefit(primaryCard.Employee,
                    TravelMissions.Where(x => x.Employee == primaryCard.Employee &&
                        x.Type == HRIS.Domain.AttendanceSystem.Enums.TravelMissionType.Internal).ToList(), GeneralOptionObject);
                if (internalMission != null)
                {
                    AddBenefit(new List<PayrollSystemIntegrationDTO>() { internalMission }, monthlyCard, GeneralOptionObject.InternalTravelMissionBenefit);
                }
                var externalMission = PayrollIntegrationService.GetExternalTravelMissionsBenefit(primaryCard.Employee,
                    TravelMissions.Where(x => x.Employee == primaryCard.Employee &&
                        x.Type == HRIS.Domain.AttendanceSystem.Enums.TravelMissionType.External).ToList(), GeneralOptionObject);
                if (externalMission != null)
                {
                    AddBenefit(new List<PayrollSystemIntegrationDTO>() { externalMission }, monthlyCard, GeneralOptionObject.ExternalTravelMissionBenefit);
                }
                var overtime = PayrollIntegrationService.ImportFromAttendance(PayrollIntegrationService.AttendanceType.Overtime,
                    primaryCard.Employee, month.FromDate.Date, month.ToDate.Date, false, GeneralOptionObject, PublicHolidays, employeeForm);
                AddBenefit(overtime, monthlyCard, GeneralOptionObject.OvertimeBenefit);
            }
            #endregion


            #endregion

            #region Deductions

            #region Deduction Distribution

            if (month.ImportDeductionDistribution)
            {
                if (primaryCard.Employee.Positions.Any())
                {
                    #region Grade
                    var employeeGrades =
                                primaryCard.Employee.Positions.Select(x => x.Position.JobDescription.JobTitle.Grade);
                    foreach (var gradeDistribution in employeeGrades)
                    {
                        foreach (var deductionInfo in gradeDistribution.GradeDeductionDetails)
                        {
                            if (monthlyCard.MonthlyEmployeeDeductions.Any(x => x.DeductionCard.Id == deductionInfo.DeductionCard.Id)) continue;
                            monthlyCard.AddMonthlyEmployeeDeduction(new MonthlyEmployeeDeduction
                            {
                                DeductionCard = deductionInfo.DeductionCard,
                                Value = deductionInfo.Value,
                                Formula = deductionInfo.Formula,
                                ExtraValue = deductionInfo.ExtraValue,
                                ExtraValueFormula = deductionInfo.ExtraValueFormula,
                                Note = deductionInfo.Note,
                                //AuditState = AuditState.Audited
                            });
                        }
                    }

                    //}
                    #endregion

                    #region JobTitle
                    var employeeJobTitles =
                                primaryCard.Employee.Positions.Select(q => q.Position.JobDescription.JobTitle);

                    foreach (var jobTitleDistribution in employeeJobTitles)
                    {
                        foreach (var deductionInfo in jobTitleDistribution.JobTitleDeductionDetails)
                        {
                            if (monthlyCard.MonthlyEmployeeDeductions.Any(x => x.DeductionCard.Id == deductionInfo.DeductionCard.Id)) continue;
                            monthlyCard.AddMonthlyEmployeeDeduction(new MonthlyEmployeeDeduction
                            {
                                DeductionCard = deductionInfo.DeductionCard,
                                Value = deductionInfo.Value,
                                Formula = deductionInfo.Formula,
                                ExtraValue = deductionInfo.ExtraValue,
                                ExtraValueFormula = deductionInfo.ExtraValueFormula,
                                Note = deductionInfo.Note,
                                //AuditState = AuditState.Audited
                            });
                        }
                    }
                    #endregion

                    #region JobDescription
                    var employeeJobDescriptions =
                                primaryCard.Employee.Positions.Select(q => q.Position.JobDescription);

                    foreach (var jobDescriptionDistribution in employeeJobDescriptions)
                    {
                        foreach (var deductionInfo in jobDescriptionDistribution.JobDescriptionDeductionDetails)
                        {
                            if (monthlyCard.MonthlyEmployeeDeductions.Any(x => x.DeductionCard.Id == deductionInfo.DeductionCard.Id)) continue;
                            monthlyCard.AddMonthlyEmployeeDeduction(new MonthlyEmployeeDeduction
                            {
                                DeductionCard = deductionInfo.DeductionCard,
                                Value = deductionInfo.Value,
                                Formula = deductionInfo.Formula,
                                ExtraValue = deductionInfo.ExtraValue,
                                ExtraValueFormula = deductionInfo.ExtraValueFormula,
                                Note = deductionInfo.Note,
                                //AuditState = AuditState.Audited
                            });
                        }
                    }
                    #endregion

                    #region Position
                    var employeePositions = primaryCard.Employee.Positions.Select(q => q.Position);

                    foreach (var positionDistribution in employeePositions)
                    {
                        foreach (var deductionInfo in positionDistribution.PositionDeductionDetails)
                        {
                            if (monthlyCard.MonthlyEmployeeDeductions.Any(x => x.DeductionCard.Id == deductionInfo.DeductionCard.Id)) continue;
                            monthlyCard.AddMonthlyEmployeeDeduction(new MonthlyEmployeeDeduction
                            {
                                DeductionCard = deductionInfo.DeductionCard,
                                Value = deductionInfo.Value,
                                Formula = deductionInfo.Formula,
                                ExtraValue = deductionInfo.ExtraValue,
                                ExtraValueFormula = deductionInfo.ExtraValueFormula,
                                Note = deductionInfo.Note,
                                //AuditState = AuditState.Audited
                            });
                        }
                    }
                    #endregion

                    #region Node
                    var employeeNodes =
                                primaryCard.Employee.Positions.Select(q => q.Position.JobDescription.Node);
                    foreach (var nodeDistribution in employeeNodes)
                    {
                        foreach (var deductionInfo in nodeDistribution.NodeDeductionDetails)
                        {
                            if (monthlyCard.MonthlyEmployeeDeductions.Any(x => x.DeductionCard.Id == deductionInfo.DeductionCard.Id)) continue;
                            monthlyCard.AddMonthlyEmployeeDeduction(new MonthlyEmployeeDeduction
                            {
                                DeductionCard = deductionInfo.DeductionCard,
                                Value = deductionInfo.Value,
                                Formula = deductionInfo.Formula,
                                ExtraValue = deductionInfo.ExtraValue,
                                ExtraValueFormula = deductionInfo.ExtraValueFormula,
                                Note = deductionInfo.Note,
                                //AuditState = AuditState.Audited
                            });
                        }
                    }
                    #endregion
                }
            }

            #endregion

            #region Import From Employee Relation
            if (month.ImportFromEmployeeRelation)
            {
                var leaves = PayrollIntegrationService.GetLeaves(primaryCard.Employee, GeneralOptionObject,
                    PublicHolidays, FixedHolidays, ChangeableHolidays, employeeForm,
                    false, month.FromDate, month.ToDate);
                if (leaves.Count() != 0)
                {
                    AddDeduction(leaves, monthlyCard, GeneralOptionObject.LeaveDeduction, true);
                }

                var penalties = PayrollIntegrationService.GetPenalties(primaryCard.Employee, false);
                AddDeduction(penalties, monthlyCard, GeneralOptionObject.PenaltyDeduction);
            }
            #endregion

            #region Import From Attendance
            if (month.ImportFromAttendance)
            {
                var hourlyMission = PayrollIntegrationService.GetHourlyMissionsDeduction(primaryCard.Employee,
                    HourlyMissions.Where(x => x.Employee == primaryCard.Employee).ToList(), GeneralOptionObject);
                if (hourlyMission != null)
                {
                    AddDeduction(new List<PayrollSystemIntegrationDTO>() { hourlyMission }, monthlyCard, GeneralOptionObject.HourlyMissionDeduction);
                }
                var dailyMission = PayrollIntegrationService.GetDailyMissionsDeduction(primaryCard.Employee,
                    TravelMissions.Where(x => x.Employee == primaryCard.Employee).ToList(), GeneralOptionObject,
                    PublicHolidays, FixedHolidays, ChangeableHolidays, employeeForm, month.FromDate, month.ToDate);
                if (dailyMission != null)
                {
                    AddDeduction(new List<PayrollSystemIntegrationDTO>() { dailyMission }, monthlyCard, GeneralOptionObject.TravelMissionDeduction);
                }
                var overtimeOrdersEmployee = OvertimeOrders.Where(x => x.Employee?.Id == primaryCard.Employee?.Id).ToList();
                var holiday = PayrollIntegrationService.GetHolidaysDeduction(primaryCard.Employee, GeneralOptionObject,
                    PublicHolidays, FixedHolidays, ChangeableHolidays, employeeForm,
                    overtimeOrdersEmployee, month.FromDate, month.ToDate);
                if (holiday != null)
                {
                    AddDeduction(new List<PayrollSystemIntegrationDTO>() { holiday }, monthlyCard, GeneralOptionObject.HolidayDeduction);
                }
                var absences = PayrollIntegrationService.ImportFromAttendance(PayrollIntegrationService.AttendanceType.Absence,
                    primaryCard.Employee, month.FromDate.Date, month.ToDate.Date, false, GeneralOptionObject, PublicHolidays, employeeForm);
                AddDeduction(absences, monthlyCard, GeneralOptionObject.AbsenceDaysDeduction);

                var nonAttendance = PayrollIntegrationService.ImportFromAttendance(PayrollIntegrationService.AttendanceType.NonAttendance,
                    primaryCard.Employee, month.FromDate.Date, month.ToDate.Date, false, GeneralOptionObject, PublicHolidays, employeeForm);
                AddDeduction(nonAttendance, monthlyCard, GeneralOptionObject.NonAttendanceDeduction);

                var lateness = PayrollIntegrationService.ImportFromAttendance(PayrollIntegrationService.AttendanceType.Lateness,
                    primaryCard.Employee, month.FromDate.Date, month.ToDate.Date, false, GeneralOptionObject, PublicHolidays, employeeForm);
                AddDeduction(lateness, monthlyCard, GeneralOptionObject.LatenessDeduction);

            }
            #endregion

            #region Import Primary Deductions
            var primaryEmployeeDeductions =
                    primaryCard.PrimaryEmployeeDeductions.Where(x =>
                        ((x.HasExpiryDate && x.ExpiryDate > month.FromDate.Date) || x.HasExpiryDate == false) &&
                        ((x.HasStartDate && x.StartDate < month.FromDate.Date) || x.HasStartDate == false));

            if (GeneralOptionObject.StoppingTaxByReserveMilitaryService && primaryCard.Employee.MilitaryStatus == MilitaryStatus.Reserve)
            {
                primaryEmployeeDeductions = primaryEmployeeDeductions.Where(x => x.DeductionCard.Id != GeneralOptionObject.TaxDeduction.Id);
            }

            foreach (var primaryEmployeeDeduction in primaryEmployeeDeductions)
            {
                monthlyCard.AddMonthlyEmployeeDeduction(new MonthlyEmployeeDeduction
                {
                    DeductionCard = primaryEmployeeDeduction.DeductionCard,
                    Value = primaryEmployeeDeduction.Value,
                    Formula = primaryEmployeeDeduction.Formula,
                    ExtraValue = primaryEmployeeDeduction.ExtraValue,
                    ExtraValueFormula = primaryEmployeeDeduction.ExtraValueFormula,
                    Note = primaryEmployeeDeduction.Note
                });
            }
            #endregion

            #endregion
            #region Advances
            var advances = primaryCard.EmployeeAdvances.Where(x => x.IsTransferToPayroll == false && x.OperationDate <= month.ToDate && x.OperationDate >= month.FromDate && x.AdvanceStatus == HRIS.Domain.Global.Enums.Status.Approved);

            foreach (var advance in advances)
            {
                advance.SourceId = 1;
                monthlyCard.AddMonthlyEmployeeAdvance(new MonthlyEmployeeAdvance
                {

                    AdvanceAmount = advance.AdvanceAmount,
                    OperationDate = advance.OperationDate,


                    Note = advance.Note
                });

            }

            #endregion
            #region Loans

            if (month.MonthType == MonthType.SalaryAndBenefit)
            {
                var employeeLoans =
                    primaryCard.EmployeeLoans.Where(x => x.RemainingAmountOfLoan > 0 && x.RequestStatus == HRIS.Domain.Global.Enums.Status.Approved);
                foreach (var employeeLoan in employeeLoans)
                {
                    double actualPaymentValue = employeeLoan.MonthlyInstalmentValue > employeeLoan.RemainingAmountOfLoan
                               ? employeeLoan.RemainingAmountOfLoan
                               : employeeLoan.MonthlyInstalmentValue;
                    monthlyCard.AddLoanPayment(new LoanPayment
                    {
                        IsExternalPayment = false,
                        PaymentValue =
                            employeeLoan.MonthlyInstalmentValue > employeeLoan.RemainingAmountOfLoan
                                ? employeeLoan.RemainingAmountOfLoan
                                : employeeLoan.MonthlyInstalmentValue,
                        RemainingValueAfterPaymentValue = employeeLoan.RemainingAmountOfLoan - actualPaymentValue,
                        EmployeeLoan = employeeLoan
                    });
                }
            }

            #endregion

            return monthlyCard;
        }
        private static void AddDeduction(IEnumerable<PayrollSystemIntegrationDTO> payrollSystemIntegrationDtos,
            MonthlyCard monthlyCard, DeductionCard deductionCard, bool isLeaves = false)
        {
            foreach (var integrationDetails in payrollSystemIntegrationDtos)
            {
                //if (integrationDetails.Repetition > 1)
                //{
                //    if (monthlyCard.PrimaryCard.PrimaryEmployeeDeductions.Any(x => x.DeductionCard.Id == deductionCard.Id && x.SourceId == integrationDetails.SourceId.FirstOrDefault()))
                //    {
                //        for (int i = 0; i < monthlyCard.PrimaryCard.PrimaryEmployeeDeductions.Count; i++)
                //        {
                //            if (monthlyCard.PrimaryCard.PrimaryEmployeeDeductions[i].DeductionCard.Id == deductionCard.Id
                //                && monthlyCard.PrimaryCard.PrimaryEmployeeDeductions[i].SourceId == integrationDetails.SourceId.FirstOrDefault())
                //            {
                //                monthlyCard.PrimaryCard.PrimaryEmployeeDeductions.RemoveAt(i);
                //                i--;
                //            }
                //        }
                //    }

                //    var newDeductionResult = new PrimaryEmployeeDeduction
                //    {
                //        DeductionCard = deductionCard,
                //        Value = integrationDetails.Value,
                //        Formula = integrationDetails.Formula,
                //        ExtraValue = integrationDetails.ExtraValue,
                //        ExtraValueFormula = integrationDetails.ExtraValueFormula,
                //        HasStartDate = true,
                //        StartDate = DateTime.Now,
                //        HasExpiryDate = true,
                //        ExpiryDate = DateTime.Now.AddMonths(integrationDetails.Repetition),
                //        SourceId = integrationDetails.SourceId.FirstOrDefault(),
                //        Note = ""
                //    };


                //    monthlyCard.PrimaryCard.AddPrimaryEmployeeDeduction(newDeductionResult);

                //    if (deductionCard.Id == generalOption.LeaveDeduction.Id)
                //    {
                //        PayrollIntegrationService.AcceptLeave(monthlyCard.PrimaryCard.Employee, newDeductionResult.SourceId);
                //    }
                //    else if (deductionCard.Id == generalOption.PenaltyDeduction.Id)
                //    {
                //        PayrollIntegrationService.AcceptPenalty(monthlyCard.PrimaryCard.Employee, newDeductionResult.SourceId);
                //    }
                //    else if (deductionCard.Id == generalOption.AbsenceDaysDeduction.Id)
                //    {
                //        PayrollIntegrationService.AcceptAttendance(
                //            PayrollIntegrationService.AttendanceType.Absence, monthlyCard.Month.Date, newDeductionResult.SourceId);
                //    }
                //    else if (deductionCard.Id == generalOption.NonAttendanceDeduction.Id)
                //    {
                //        PayrollIntegrationService.AcceptAttendance(
                //            PayrollIntegrationService.AttendanceType.NonAttendance, monthlyCard.Month.Date, newDeductionResult.SourceId);
                //    }
                //    else if (deductionCard.Id == generalOption.LatenessDeduction.Id)
                //    {
                //        PayrollIntegrationService.AcceptAttendance(
                //            PayrollIntegrationService.AttendanceType.Lateness, monthlyCard.Month.Date, newDeductionResult.SourceId);
                //    }
                //}
                //else
                //{
                //if (monthlyCard.MonthlyEmployeeDeductions.Any(x => x.DeductionCard.Id == deductionCard.Id))
                //{
                //    for (int i = 0; i < monthlyCard.MonthlyEmployeeDeductions.Count; i++)
                //    {
                //        if (monthlyCard.MonthlyEmployeeDeductions[i].DeductionCard.Id == deductionCard.Id
                //            && (monthlyCard.PrimaryCard.PrimaryEmployeeDeductions.Any() && monthlyCard.PrimaryCard.PrimaryEmployeeDeductions[i].SourceId == integrationDetails.SourceId.FirstOrDefault()))
                //        {
                //            monthlyCard.MonthlyEmployeeDeductions.RemoveAt(i);
                //            i--;
                //        }
                //    }
                //}

                monthlyCard.AddMonthlyEmployeeDeduction(new MonthlyEmployeeDeduction
                {
                    DeductionCard = integrationDetails.DeductionCard != null && integrationDetails.DeductionCard.Id != 0 ? integrationDetails.DeductionCard : deductionCard,
                    Value = integrationDetails.Value,
                    Formula = integrationDetails.Formula,
                    ExtraValue = integrationDetails.ExtraValue,
                    ExtraValueFormula = integrationDetails.ExtraValueFormula,
                    Note = integrationDetails.Note,
                    SourceId = integrationDetails.SourceId != null && integrationDetails.SourceId.Any() ?
                       integrationDetails.SourceId.FirstOrDefault() : 0
                });
                if (isLeaves)
                    foreach (var sourceId in integrationDetails.SourceId)
                    {
                        monthlyCard.MonthlyEmployeeDeductions.OrderByDescending(x => x.Id).FirstOrDefault().AddLeaveDeduction(new LeaveDeduction()
                        {
                            LeaveId = sourceId
                        });
                    }
                //}
            }
        }
        private static void AddBenefit(IEnumerable<PayrollSystemIntegrationDTO> payrollSystemIntegrationDtos,
            MonthlyCard monthlyCard, BenefitCard benefitCard)
        {
            foreach (var integrationDetails in payrollSystemIntegrationDtos)
            {
                //if (integrationDetails.Repetition > 1)
                //{
                //    if (monthlyCard.PrimaryCard.PrimaryEmployeeBenefits.Any(x => x.BenefitCard.Id == benefitCard.Id && x.SourceId == integrationDetails.SourceId))
                //    {
                //        for (int i = 0; i < monthlyCard.PrimaryCard.PrimaryEmployeeBenefits.Count; i++)
                //        {
                //            if (monthlyCard.PrimaryCard.PrimaryEmployeeBenefits[i].BenefitCard.Id == benefitCard.Id
                //                && monthlyCard.PrimaryCard.PrimaryEmployeeBenefits[i].SourceId == integrationDetails.SourceId)
                //            {
                //                monthlyCard.PrimaryCard.PrimaryEmployeeBenefits.RemoveAt(i);
                //                i--;
                //            }
                //        }
                //    }


                //    var newBenefitResult = new PrimaryEmployeeBenefit
                //    {
                //        BenefitCard = benefitCard,
                //        Value = integrationDetails.Value,
                //        Formula = integrationDetails.Formula,
                //        ExtraValue = integrationDetails.ExtraValue,
                //        ExtraValueFormula = integrationDetails.ExtraValueFormula,
                //        HasStartDate = true,
                //        StartDate = DateTime.Now,
                //        HasExpiryDate = true,
                //        ExpiryDate = DateTime.Now.AddMonths(integrationDetails.Repetition),
                //        SourceId = integrationDetails.SourceId,
                //        Note =""

                //    };

                //    monthlyCard.PrimaryCard.AddPrimaryEmployeeBenefit(newBenefitResult);

                //    if (benefitCard.Id == generalOption.RewardBenefit.Id)
                //    {
                //        PayrollIntegrationService.AcceptReward(monthlyCard.PrimaryCard.Employee, newBenefitResult.SourceId);
                //    }
                //    else if (benefitCard.Id == generalOption.OvertimeBenefit.Id)
                //    {
                //        PayrollIntegrationService.AcceptAttendance(
                //            PayrollIntegrationService.AttendanceType.Overtime, monthlyCard.Month.Date, newBenefitResult.SourceId);
                //    }
                //}
                //else
                //{
                //    if (monthlyCard.MonthlyEmployeeBenefits.Any(x => x.BenefitCard.Id == benefitCard.Id && x.SourceId == integrationDetails.SourceId))
                //    {
                //        for (int i = 0; i < monthlyCard.MonthlyEmployeeBenefits.Count; i++)
                //        {
                //            if (monthlyCard.MonthlyEmployeeBenefits[i].BenefitCard.Id == benefitCard.Id
                //                && monthlyCard.PrimaryCard.PrimaryEmployeeBenefits[i].SourceId == integrationDetails.SourceId)
                //            {
                //                monthlyCard.MonthlyEmployeeBenefits.RemoveAt(i);
                //                i--;
                //            }
                //        }
                //    }

                var newBenefitResult = new MonthlyEmployeeBenefit
                {
                    BenefitCard = benefitCard,
                    Value = integrationDetails.Value,
                    Formula = integrationDetails.Formula,
                    ExtraValue = integrationDetails.ExtraValue,
                    ExtraValueFormula = integrationDetails.ExtraValueFormula,
                    Note = "",
                    SourceId = integrationDetails.SourceId.FirstOrDefault()
                };
                monthlyCard.AddMonthlyEmployeeBenefit(newBenefitResult);
                // }
            }
        }
        public static void RejectMonth(Month month)
        {
            month.MonthStatus = MonthStatus.Rejected;
        }

        public static void ApproveMonth(Month month)
        {
            month.MonthStatus = MonthStatus.Approved;
        }

        public static void LockMonth(Month month)
        {
            month.MonthStatus = MonthStatus.Locked;
           
            var entities = new List<IAggregateRoot>();
            if (month.ImportFromEmployeeRelation)
            {
                var generalOption = typeof(GeneralOption).GetAll<GeneralOption>().Single();

                #region Reward
                var monthlyRecycledLeaves = month.MonthlyCards.SelectMany(x => x.MonthlyEmployeeBenefits)
                            .Where(x => x.BenefitCard.Id == generalOption.RecycledLeaveBenefit.Id && x.SourceId > 0);
                foreach (var item in monthlyRecycledLeaves)
                {
                    var recycledLeave = PayrollIntegrationService.AcceptRecycledLeave(item.MonthlyCard.PrimaryCard.Employee, item.SourceId);
                    if (recycledLeave != null)
                        entities.Add(recycledLeave);
                }
                #endregion
                #region Reward
                var monthlyRewards = month.MonthlyCards.SelectMany(x => x.MonthlyEmployeeBenefits)
                            .Where(x => x.BenefitCard.Id == generalOption.RewardBenefit.Id && x.SourceId > 0);
                foreach (var item in monthlyRewards)
                {
                    var reward = PayrollIntegrationService.AcceptReward(item.MonthlyCard.PrimaryCard.Employee, item.SourceId);
                    if (reward != null)
                        entities.Add(reward);
                }
                #endregion

                #region LeaveDeduction
                var monthlyLeaves = month.MonthlyCards.SelectMany(x => x.MonthlyEmployeeDeductions)
                            .Where(x => x.DeductionCard.Id == generalOption.LeaveDeduction.Id);
                foreach (var item in monthlyLeaves)
                {
                    entities.AddRange(PayrollIntegrationService.AcceptLeave(item.MonthlyCard.PrimaryCard.Employee, item));
                }
                #endregion


                #region Penalties
                var monthlyPenalties = month.MonthlyCards.SelectMany(x => x.MonthlyEmployeeDeductions)
                            .Where(x => x.DeductionCard.Id == generalOption.PenaltyDeduction.Id && x.SourceId > 0);
                foreach (var item in monthlyPenalties)
                {
                    var penalty = PayrollIntegrationService.AcceptPenalty(item.MonthlyCard.PrimaryCard.Employee, item.SourceId);
                    if (penalty != null)
                        entities.Add(penalty);
                }

                #endregion

            }
            #region Advances
            var monthEmployees = month.MonthlyCards.Select(x => x.PrimaryCard.Employee).ToList();
            //ServiceFactory.ORMService.All<EmployeeAdvance>().Where(x=>x.EmployeeCard.Employee.Id== monthEmployees)
            foreach (var item in monthEmployees)
            {
                var advances = PayrollIntegrationService.AcceptAdvances(item);
                if (advances.Count() == 0)
                    entities = advances;
                var allTravelMission = ServiceFactory.ORMService.All<TravelMission>().
             Where(x => x.FromDate >= month.FromDate && x.ToDate <= month.ToDate && x.Employee.Id == item.Id 
             && x.Status == HRIS.Domain.Global.Enums.Status.Approved).ToList();
                foreach (var mission in allTravelMission)

                {
                    mission.IsTransferedToPayroll = true;

                    //var travelMission = (TravelMission)typeof(TravelMission).GetById(mission.Id);
                    mission.Save();
                }
                var allHourlyMission = ServiceFactory.ORMService.All<HourlyMission>().
           Where(x => x.StartDateTime >= month.FromDate && x.EndDateTime <= month.ToDate && x.Employee.Id == item.Id
            && x.Status == HRIS.Domain.Global.Enums.Status.Approved).ToList();
                foreach (var hourMission in allHourlyMission)

                {
                    hourMission.IsTransferedToPayroll = true;

                    //var hourMissions = (HourlyMission)typeof(HourlyMission).GetById(hourMission.Id);
                    hourMission.Save();
                }
            }
           

            #endregion
            if (month.ImportFromAttendance)
            {
                var generalOption = typeof(GeneralOption).GetAll<GeneralOption>().Single();

                var attendanceRecord = ServiceFactory.ORMService.All<AttendanceRecord>().FirstOrDefault(x => x.FromDate == month.FromDate && x.ToDate == month.ToDate);
                #region Overtime
                var monthlyOvertime = month.MonthlyCards.SelectMany(x => x.MonthlyEmployeeBenefits)
                            .Where(x => x.BenefitCard.Id == generalOption.OvertimeBenefit.Id && x.SourceId > 0);
                monthlyOvertime.ForEach(x => PayrollIntegrationService.AcceptAttendance(attendanceRecord,
                    PayrollIntegrationService.AttendanceType.Overtime, x.SourceId));

                #endregion

                #region Absence
                var monthlyAbsence = month.MonthlyCards.SelectMany(x => x.MonthlyEmployeeDeductions)
                            .Where(x => x.DeductionCard.Id == generalOption.AbsenceDaysDeduction.Id && x.SourceId > 0);
                monthlyAbsence.ForEach(x => PayrollIntegrationService.AcceptAttendance(attendanceRecord,
                    PayrollIntegrationService.AttendanceType.Absence, x.SourceId));

                #endregion

                #region NonAttendance
                var monthlyNonAttendance = month.MonthlyCards.SelectMany(x => x.MonthlyEmployeeDeductions)
                            .Where(x => x.DeductionCard.Id == generalOption.NonAttendanceDeduction.Id && x.SourceId > 0);
                monthlyNonAttendance.ForEach(x => PayrollIntegrationService.AcceptAttendance(attendanceRecord,
                    PayrollIntegrationService.AttendanceType.NonAttendance, x.SourceId));

                #endregion

                #region Lateness
                var monthlyLateness = month.MonthlyCards.SelectMany(x => x.MonthlyEmployeeDeductions)
                            .Where(x => x.DeductionCard.Id == generalOption.LatenessDeduction.Id && x.SourceId > 0);
                monthlyLateness.ForEach(x => PayrollIntegrationService.AcceptAttendance(attendanceRecord,
                    PayrollIntegrationService.AttendanceType.Lateness, x.SourceId));

                #endregion
                entities.Add(attendanceRecord);
            }

            ServiceFactory.ORMService.SaveTransaction<IAggregateRoot>(entities, UserExtensions.CurrentUser);
        }

        public static void CalculateMonth(Month month)
        {
            var generalOption = typeof(GeneralOption).GetAll<GeneralOption>().First();
            foreach (var monthlyCard in month.MonthlyCards)
            {
                if (!monthlyCard.IsCalculated)
                {
                    CalculateMonthlyCard(monthlyCard, generalOption);
                    monthlyCard.IsCalculated = true;
                }
            }
            month.MonthStatus = MonthStatus.Calculated;
        }

        public static void CalculateMonthlyCard(MonthlyCard monthlyCard, GeneralOption generalOption)
        {

            // ترتيب استدعاءات الميثود هام جدا يجب عدم تغيير ترتيب الاستدعاء
            CalculateMonthlyBenefit(monthlyCard, generalOption);
            CalculateMonthlyDeduction(monthlyCard, generalOption);

            //SetBenefitCeilForParents(monthlyCard);
            SetBenefitCeilForParents(monthlyCard, generalOption);
            MonthlyCardRoundInitialValues(monthlyCard);
            CalculateCrossedInitialBenefitValue(monthlyCard, generalOption);
            MonthlyCardRoundCrossedInitialBenefitValue(monthlyCard, generalOption);
            CalculateTaxDeduction(monthlyCard, generalOption);

            CalculateDeductionCrossBenefit(monthlyCard, generalOption);
            if (monthlyCard.Month.MonthType == MonthType.SalaryAndBenefit)
                CalculateDeductionCrossDeduction(monthlyCard, generalOption);

            CalculateExtraValuesOnFinalValues(monthlyCard);
            MonthlyCardRoundFinalValues(monthlyCard);
            var TotalLoanPayments = monthlyCard.LoanPayments.Where(x => x.IsExternalPayment == false).Sum(x => x.PaymentValue);
            // حساب اجمالي المستحق والراتب الفعلي
            if (monthlyCard.Month.MonthType == MonthType.BenefitOnly)
                monthlyCard.ActualMonthSalary = monthlyCard.TotalBenefitsValue - monthlyCard.TotalDeducationsValue - TotalLoanPayments - monthlyCard.TotalAdvances;
            else
                monthlyCard.ActualMonthSalary = monthlyCard.PackageSalary + monthlyCard.TotalBenefitsValue - monthlyCard.TotalDeducationsValue - TotalLoanPayments - monthlyCard.TotalAdvances;
            //الراتب النهائي
            if (monthlyCard.ActualMonthSalary < 0)
                monthlyCard.FinalMonthSalary = 0;
            else
                monthlyCard.FinalMonthSalary = monthlyCard.ActualMonthSalary;
        }

        #region Set Benefit Ceil For Parents

        public static double GetBenefitChildrenTotalValue(BenefitCard benefitCard, IQueryable<BenefitCard> allBenefitCards,
            IList<MonthlyEmployeeBenefit> employeeBenefitCards)
        {
            // يعيد كامل تعويضات الابناء للاب الحالي بشرط ان يكون الموظف يملك هذا التعويض اي موجود في الليست الممررة بالبارامتر
            var leafChildren = // اما أنه ابن ويمكله الموظف
                employeeBenefitCards.Where(x => x.BenefitCard.ParentBenefitCard != null).Where(x => x.BenefitCard.ParentBenefitCard.Id == benefitCard.Id).ToList();
            var parentChildren =
                allBenefitCards.Where(x => x.ParentBenefitCard != null).Where(x => // أو أنه أب لتعويضات أخرى لأحتمال ان يكون الموظف يملكها بالتالي يجب أخذها بعين الاعتبار
                        x.ParentBenefitCard.Id == benefitCard.Id && allBenefitCards.Any(y => y.ParentBenefitCard.Id == x.Id));

            var result = leafChildren.Sum(x => x.InitialValue);

            foreach (var parentChild in parentChildren)
            {
                result += GetBenefitChildrenTotalValue(parentChild, allBenefitCards, employeeBenefitCards);
            }

            return result;
        }

        /// <summary>
        /// مهمة الميثود انقاص  قيم التعويضات الفرعية حسب قيمة محددة بالباراميتر
        /// </summary>
        /// <param name="value"> القيمة المطلوب ان يتم الانقاص حسبها</param>
        /// <param name="benefitCard">التعويض الاب الذي سيتم انقاص تعويضات ابناءه حسب القيمة التي تم تمريرها بالباراميتر الاول</param>
        /// <param name="employeeBenefitCards">مجموعة التعويضات التي يملكها الموظف والتي سيتم العمل على انقاص قيمها</param>
        public static double ApplyMinusValueOnBenefitChildren(double value, BenefitCard benefitCard, IQueryable<BenefitCard> allBenefitCards,
            IList<MonthlyEmployeeBenefit> employeeBenefitCards)
        {
            if (value <= 0)
            {
                return 0;
            }

            // يعيد كامل تعويضات الابناء للاب الحالي بشرط ان يكون الموظف يملك هذا التعويض اي موجود في الليست الممررة بالبارامتر
            var leafChildren = // اما أنه ابن ويمكله الموظف
                employeeBenefitCards.Where(x => x.BenefitCard.ParentBenefitCard != null && x.BenefitCard.ParentBenefitCard.Id == benefitCard.Id).ToList();
            var parentChildren =
                allBenefitCards.Where(
                    x => // أو أنه أب لتعويضات أخرى لأحتمال ان يكون الموظف يملكها بالتالي يجب أخذها بعين الاعتبار
                        x.ParentBenefitCard.Id == benefitCard.Id && allBenefitCards.Any(y => y.ParentBenefitCard.Id == x.Id));

            foreach (var child in leafChildren)
            {
                if (value > child.InitialValue)
                {
                    value -= child.InitialValue;
                    child.InitialValue = 0;
                    child.FinalValue = 0;
                }
                else
                {
                    child.InitialValue -= value;
                    child.FinalValue -= value;
                    value = 0;
                    break;
                }
            }


            foreach (var parentChild in parentChildren)
            {
                if (value <= 0)
                {
                    break;
                }
                value = ApplyMinusValueOnBenefitChildren(value, parentChild, allBenefitCards, employeeBenefitCards);
            }
            return value;
        }

        private static void SetBenefitCeilForParents(MonthlyCard monthlyCard, GeneralOption generalOption)
        {
            var allBenefitCards = typeof(BenefitCard).GetAll<BenefitCard>();
            var roots = new List<BenefitCard>();
            foreach (var monthlyEmployeeBenefit in monthlyCard.MonthlyEmployeeBenefits)
            {
                var tempRoot = monthlyEmployeeBenefit.BenefitCard;
                while (tempRoot.ParentBenefitCard != null)
                {
                    tempRoot = tempRoot.ParentBenefitCard;
                }

                if (roots.All(x => x.Id != tempRoot.Id))
                {
                    roots.Add(tempRoot);
                }
            }

            foreach (var root in roots)
            {
                ApplyCeilOnBenefitChildren(allBenefitCards, root, monthlyCard, generalOption);
            }
        }

        private static void ApplyCeilOnBenefitChildren(IQueryable<BenefitCard> allBenefitCards, BenefitCard rootCard, MonthlyCard monthlyCard, GeneralOption generalOption)
        {
            var subRoot =
                allBenefitCards.Where(x =>// أو أنه أب لتعويضات أخرى لأحتمال ان يكون الموظف يملكها بالتالي يجب أخذها بعين الاعتبار
                    x.ParentBenefitCard.Id == rootCard.Id && allBenefitCards.Any(y => y.ParentBenefitCard.Id == x.Id));

            foreach (var root in subRoot)
            {
                ApplyCeilOnBenefitChildren(allBenefitCards, root, monthlyCard, generalOption);
            }

            if (rootCard.CeilValue > 0)
            {

                var ceilValue =
                    GetValueOfFormula(rootCard.CeilValue, rootCard.CeilFormula, monthlyCard, 0, generalOption);

                var benefitChildrenTotalValue = GetBenefitChildrenTotalValue(rootCard, allBenefitCards, monthlyCard.MonthlyEmployeeBenefits);
                if (benefitChildrenTotalValue > ceilValue)
                {
                    var variance = benefitChildrenTotalValue - ceilValue;
                    ApplyMinusValueOnBenefitChildren(variance, rootCard, allBenefitCards, monthlyCard.MonthlyEmployeeBenefits);
                }
            }
        }

        #endregion

        public static void CalculateMonthlyBenefit(MonthlyCard monthlyCard, GeneralOption generalOption)
        {
            foreach (var monthlyEmployeeBenefit in monthlyCard.MonthlyEmployeeBenefits)
            {
                var benefitCard = monthlyEmployeeBenefit.BenefitCard;

                #region Initial Value

                monthlyEmployeeBenefit.InitialValue =
                    GetValueOfFormula(monthlyEmployeeBenefit.Value, monthlyEmployeeBenefit.Formula,
                        monthlyEmployeeBenefit.MonthlyCard, 0, generalOption);

                #endregion

                #region Extra Value On Initial Values

                if (monthlyEmployeeBenefit.ExtraValueFormula == ExtraValueFormula.PercentageOfInitialValue ||
                     monthlyEmployeeBenefit.ExtraValueFormula == ExtraValueFormula.MultipleWithInitialValue)
                {
                    monthlyEmployeeBenefit.InitialValue =
                        GetValueOfExtraFormula(monthlyEmployeeBenefit.ExtraValue,
                            monthlyEmployeeBenefit.ExtraValueFormula, monthlyEmployeeBenefit.InitialValue);
                }

                #endregion

                #region Ceil Value

                if (monthlyEmployeeBenefit.CeilValue > 0)
                {
                    var ceilValue =
                        GetValueOfFormula(monthlyEmployeeBenefit.CeilValue, monthlyEmployeeBenefit.CeilFormula,
                            monthlyEmployeeBenefit.MonthlyCard, 0, generalOption);
                    monthlyEmployeeBenefit.InitialValue = monthlyEmployeeBenefit.InitialValue > ceilValue
                        ? ceilValue
                        : monthlyEmployeeBenefit.InitialValue;
                }

                #endregion

                #region Effectable By Partial Work Days

                if (benefitCard.EffectableByPartialWorkDays)
                {
                    if (generalOption.TotalMonthDays != monthlyEmployeeBenefit.MonthlyCard.WorkDays)
                    {
                        monthlyEmployeeBenefit.InitialValue = monthlyEmployeeBenefit.InitialValue /
                                                              generalOption.TotalMonthDays *
                                                              monthlyEmployeeBenefit.MonthlyCard.WorkDays;
                    }
                }

                #endregion

                monthlyEmployeeBenefit.InitialValue = RoundUtility.Round(monthlyEmployeeBenefit.InitialValue, RoundDirection.None, RoundSite.AfterComma, 2);
                monthlyEmployeeBenefit.FinalValue = monthlyEmployeeBenefit.InitialValue;
            }
        }

        public static double GetDeductionValue(MonthlyEmployeeDeduction monthlyEmployeeDeduction, double reductionValue, GeneralOption generalOption)
        {
            var deductionCard = monthlyEmployeeDeduction.DeductionCard;
            if (monthlyEmployeeDeduction.DeductionCard.Id == generalOption.TaxDeduction.Id)
            {
                return 0;
            }

            #region Initial Value

            var finalValue = GetValueOfFormula(monthlyEmployeeDeduction.Value, monthlyEmployeeDeduction.Formula,
                monthlyEmployeeDeduction.MonthlyCard, reductionValue, generalOption);

            #endregion

            #region Extra Value On Initial Values

            if (monthlyEmployeeDeduction.ExtraValueFormula == ExtraValueFormula.PercentageOfInitialValue ||
                monthlyEmployeeDeduction.ExtraValueFormula == ExtraValueFormula.MultipleWithInitialValue)
            {
                finalValue =
                    GetValueOfExtraFormula(monthlyEmployeeDeduction.ExtraValue, monthlyEmployeeDeduction.ExtraValueFormula, finalValue);
            }

            #endregion

            #region Effectable By Partial Work Days

            if (deductionCard.EffectableByPartialWorkDays)
            {
                if (generalOption.TotalMonthDays != monthlyEmployeeDeduction.MonthlyCard.WorkDays)
                {
                    finalValue = finalValue / generalOption.TotalMonthDays * monthlyEmployeeDeduction.MonthlyCard.WorkDays;
                }
            }

            #endregion
            return RoundUtility.Round(finalValue, RoundDirection.None, RoundSite.AfterComma, 2);
        }

        public static void CalculateMonthlyDeduction(MonthlyCard monthlyCard, GeneralOption generalOption)
        {
            if (monthlyCard.Month.MonthType == MonthType.SalaryAndBenefit)
            {
                foreach (var monthlyEmployeeDeduction in monthlyCard.MonthlyEmployeeDeductions)
                {
                    monthlyEmployeeDeduction.InitialValue = GetDeductionValue(monthlyEmployeeDeduction, 0, generalOption);
                    monthlyEmployeeDeduction.FinalValue = 0;
                }
            }
        }

        public static void MonthlyCardRoundInitialValues(MonthlyCard monthlyCard)
        {
            foreach (var monthlyEmployeeBenefit in monthlyCard.MonthlyEmployeeBenefits)
            {
                monthlyEmployeeBenefit.InitialValue = RoundUtility.PreDefinedRoundValue(monthlyEmployeeBenefit.BenefitCard.InitialRound,
                    monthlyEmployeeBenefit.InitialValue);
            }

            foreach (var monthlyEmployeeDeduction in monthlyCard.MonthlyEmployeeDeductions)
            {
                monthlyEmployeeDeduction.InitialValue = RoundUtility.PreDefinedRoundValue(monthlyEmployeeDeduction.DeductionCard.InitialRound,
                    monthlyEmployeeDeduction.InitialValue);
            }
        }

        public static void MonthlyCardRoundCrossedInitialBenefitValue(MonthlyCard monthlyCard, GeneralOption generalOption)
        {
            foreach (var monthlyEmployeeBenefit in monthlyCard.MonthlyEmployeeBenefits)
            {
                monthlyEmployeeBenefit.CrossDependencyInitialValue = RoundUtility.PreDefinedRoundValue(monthlyEmployeeBenefit.BenefitCard.InitialRound, monthlyEmployeeBenefit.CrossDependencyInitialValue);
            }
            //foreach (var monthlyEmployeeDeduction in monthlyCard.MonthlyEmployeeDeductions)
            //{
            //    monthlyEmployeeDeduction.CrossDependencyInitialValue = RoundUtility.PreDefinedRoundValue(monthlyEmployeeDeduction.DeductionCard.InitialRound, monthlyEmployeeDeduction.CrossDependencyInitialValue);
            //}
        }

        public static void CalculateDeductionCrossDeduction(MonthlyCard monthlyCard, GeneralOption generalOption)
        {
            foreach (var item in monthlyCard.MonthlyEmployeeDeductions)
            {
                if (item.DeductionCard.Id == generalOption.TaxDeduction.Id)
                {
                    continue;
                }
                var tempItem = item;
                var availableCrossDeductions =
                    monthlyCard.MonthlyEmployeeDeductions.Where(x =>
                        tempItem.DeductionCard.CrossDeductionWithDeductions.Any(y => y.DeductionCard.Id == x.DeductionCard.Id));



                var reductionValue = 0.0;//availableCrossDeductions.Sum(x => x.InitialValue);
                //item.FinalValue


                foreach (var availableCrossDeduction in availableCrossDeductions)
                {
                    var crossDetails =
                        item.DeductionCard.CrossDeductionWithDeductions.First(
                            x => x.DeductionCard.Id == availableCrossDeduction.DeductionCard.Id);
                    switch (crossDetails.CrossType)
                    {
                        case CrossType.AsDefined:
                            {
                                reductionValue += availableCrossDeduction.InitialValue;
                                break;
                            }
                        case CrossType.Custom:
                            {
                                switch (crossDetails.CrossFormula)
                                {
                                    case CrossFormula.Nothing:
                                        {
                                            break;
                                        }
                                    case CrossFormula.FixedValue:
                                        {
                                            reductionValue += crossDetails.Value;
                                            break;
                                        }
                                    case CrossFormula.Percentage:
                                        {
                                            reductionValue += GetValueOfFormula(crossDetails.Value, availableCrossDeduction.Formula, availableCrossDeduction.MonthlyCard, 0, generalOption);
                                            break;
                                        }
                                    default:
                                        {
                                            throw new Exception("Passed formula not supported");
                                        }
                                }
                                break;
                            }
                        default:
                            {
                                throw new Exception("Passed formula not supported");
                            }
                    }
                }
                item.FinalValue += GetDeductionValue(item, reductionValue, generalOption);
            }
        }

        public static void CalculateDeductionCrossBenefit(MonthlyCard monthlyCard, GeneralOption generalOption)
        {
            foreach (var item in monthlyCard.MonthlyEmployeeBenefits)
            {
                var item1 = item;
                var availableCrossDeductions =
                    monthlyCard.MonthlyEmployeeDeductions.Where(
                        x => item1.BenefitCard.CrossDeductions.Any(y => y.DeductionCard.Id == x.DeductionCard.Id));

                foreach (var availableCrossDeduction in availableCrossDeductions)
                {
                    if (availableCrossDeduction.DeductionCard.Id == generalOption.TaxDeduction.Id)
                    {
                        if (item.BenefitCard.TaxValue > 0)
                        {
                            switch (item.BenefitCard.TaxFormula)
                            {
                                case TaxFormula.Nothing:
                                    {
                                        break;
                                    }
                                case TaxFormula.SpecificValue:
                                    {
                                        //todo Mhd Alsaadi: يفضل ان يكون الضريبة الثابتة للتعويض ضمن القيم القابلة للتعديل في البطاقة الشهرية وليس في بطاقة التعويض
                                        item.FinalValue -= item.BenefitCard.TaxValue;
                                        availableCrossDeduction.FinalValue +=
                                            RoundUtility.PreDefinedRoundValue(availableCrossDeduction.DeductionCard.InitialRound,
                                                item.BenefitCard.TaxValue);
                                        break;
                                    }
                                case TaxFormula.Percentage:
                                    {
                                        item.FinalValue -= (item.BenefitCard.TaxValue * item.InitialValue) / 100;
                                        availableCrossDeduction.FinalValue +=
                                            RoundUtility.PreDefinedRoundValue(availableCrossDeduction.DeductionCard.InitialRound, ((item.BenefitCard.TaxValue * item.InitialValue) / 100));
                                        break;
                                    }
                                default:
                                    {
                                        throw new Exception("Passed formula not supported");
                                    }
                            }
                        }
                        continue;
                    }

                    var crossDetails =
                        item.BenefitCard.CrossDeductions.First(x => x.DeductionCard.Id == availableCrossDeduction.DeductionCard.Id);
                    //var benefitValue = item.InitialValue;
                    //if (crossDetails.DependsOnCrossedInitialValue)
                    //{
                    //    benefitValue = item.CrossedInitialValue;
                    //}
                    var benefitValue = item.InitialValue - GetDeductionCrossedInitialBenefitValue(item, availableCrossDeduction.DeductionCard, monthlyCard, generalOption);


                    switch (crossDetails.CrossType)
                    {
                        case CrossType.AsDefined:
                            {
                                var tempValue = GetValueOfDeductionCrossBenefit(availableCrossDeduction.Value, monthlyCard, availableCrossDeduction.Formula, benefitValue, generalOption);
                                item.FinalValue -= tempValue;
                                //availableCrossDeduction.FinalValue += tempValue;//todo: changeset no.Temp
                                availableCrossDeduction.FinalValue += RoundUtility.PreDefinedRoundValue(availableCrossDeduction.DeductionCard.InitialRound, tempValue);
                                break;
                            }
                        case CrossType.Custom:
                            {
                                switch (crossDetails.CrossFormula)
                                {
                                    case CrossFormula.Nothing:
                                        {
                                            break;
                                        }
                                    case CrossFormula.FixedValue:
                                        {
                                            item.FinalValue -= crossDetails.Value;
                                            //availableCrossDeduction.FinalValue += crossDetails.Value;//todo: changeset no.Temp
                                            availableCrossDeduction.FinalValue += RoundUtility.PreDefinedRoundValue(availableCrossDeduction.DeductionCard.InitialRound, crossDetails.Value);
                                            break;
                                        }
                                    case CrossFormula.Percentage:
                                        {
                                            item.FinalValue -= (crossDetails.Value * benefitValue) / 100;
                                            //availableCrossDeduction.FinalValue += (crossDetails.Value * benefitValue) / 100;//todo: changeset no.Temp
                                            availableCrossDeduction.FinalValue += RoundUtility.PreDefinedRoundValue(availableCrossDeduction.DeductionCard.InitialRound, (crossDetails.Value * benefitValue) / 100);
                                            break;
                                        }
                                    default:
                                        {
                                            throw new Exception("Passed formula not supported");
                                        }
                                }
                                break;
                            }
                        default:
                            {
                                throw new Exception("Passed formula not supported");
                            }
                    }
                }
            }
        }

        public static double GetDeductionCrossedInitialBenefitValue(MonthlyEmployeeBenefit monthlyEmployeeBenefit, DeductionCard deductionCard, MonthlyCard monthlyCard, GeneralOption generalOption)
        {
            var availableCrossDependencies =
            monthlyCard.MonthlyEmployeeDeductions.Where(x => monthlyEmployeeBenefit.BenefitCard.CrossDeductions.Any(y => y.DeductionCard.Id == deductionCard.Id && y.CrossDependencys.Any(z => z.DeductionCard.Id == x.DeductionCard.Id))).ToList();

            var availableCrossDependencyValue = 0.0;
            foreach (var availableCrossDependency in availableCrossDependencies)
            {
                if (availableCrossDependency.DeductionCard.Id == generalOption.TaxDeduction.Id)
                {
                    if (monthlyEmployeeBenefit.BenefitCard.TaxValue > 0)
                    {
                        switch (monthlyEmployeeBenefit.BenefitCard.TaxFormula)
                        {
                            case TaxFormula.Nothing:
                                {
                                    break;
                                }
                            case TaxFormula.SpecificValue:
                                {
                                    availableCrossDependencyValue += monthlyEmployeeBenefit.BenefitCard.TaxValue;
                                    break;
                                }
                            case TaxFormula.Percentage:
                                {
                                    availableCrossDependencyValue += (monthlyEmployeeBenefit.BenefitCard.TaxValue * monthlyEmployeeBenefit.InitialValue) / 100;
                                    break;
                                }
                            default:
                                {
                                    throw new Exception("Passed formula not supported");
                                }
                        }
                    }
                    continue;
                }

                var crossDetails = monthlyEmployeeBenefit.BenefitCard.CrossDeductions.FirstOrDefault(x => x.DeductionCard.Id == availableCrossDependency.DeductionCard.Id);
                if (crossDetails != null)
                {
                    switch (crossDetails.CrossType)
                    {
                        case CrossType.AsDefined:
                            {
                                availableCrossDependencyValue += GetValueOfDeductionCrossBenefit(availableCrossDependency.Value, monthlyCard, availableCrossDependency.Formula, monthlyEmployeeBenefit.InitialValue, generalOption);
                                break;
                            }
                        case CrossType.Custom:
                            {
                                switch (crossDetails.CrossFormula)
                                {
                                    case CrossFormula.Nothing:
                                        {
                                            break;
                                        }
                                    case CrossFormula.FixedValue:
                                        {
                                            availableCrossDependencyValue += crossDetails.Value;
                                            break;
                                        }
                                    case CrossFormula.Percentage:
                                        {
                                            availableCrossDependencyValue += (crossDetails.Value * monthlyEmployeeBenefit.InitialValue) / 100;
                                            break;
                                        }
                                    default:
                                        {
                                            throw new Exception("Passed formula not supported");
                                        }
                                }
                                break;
                            }
                        default:
                            {
                                throw new Exception("Passed formula not supported");
                            }
                    }
                }

            }
            availableCrossDependencyValue = RoundUtility.PreDefinedRoundValue(deductionCard.InitialRound, availableCrossDependencyValue);
            return availableCrossDependencyValue;
        }


        public static void CalculateCrossedInitialBenefitValue(MonthlyCard monthlyCard, GeneralOption generalOption)
        {
            //foreach (var monthlyEmployeeDeduction in monthlyCard.MonthlyEmployeeDeductions)
            //{
            //    monthlyEmployeeDeduction.CrossDependencyInitialValue = 0;
            //}
            foreach (var item in monthlyCard.MonthlyEmployeeBenefits)
            {
                item.CrossDependencyInitialValue = item.InitialValue;
                var item1 = item;



                var availableCrossDependencies =
                monthlyCard.MonthlyEmployeeDeductions.Where(x => item1.BenefitCard.CrossDeductions.Any(y => y.CrossDependencys.Any(z => z.DeductionCard.Id == x.DeductionCard.Id))).ToList();

                foreach (var availableCrossDependency in availableCrossDependencies)
                {
                    var availableCrossDependencyValue = 0.0;
                    if (availableCrossDependency.DeductionCard.Id == generalOption.TaxDeduction.Id)
                    {
                        if (item.BenefitCard.TaxValue > 0)
                        {
                            switch (item.BenefitCard.TaxFormula)
                            {
                                case TaxFormula.Nothing:
                                    {
                                        break;
                                    }
                                case TaxFormula.SpecificValue:
                                    {
                                        //item.CrossDependencyInitialValue -= item.BenefitCard.TaxValue;
                                        //availableCrossDeduction.CrossDependencyInitialValue += item.BenefitCard.TaxValue;
                                        availableCrossDependencyValue = item.BenefitCard.TaxValue;
                                        break;
                                    }
                                case TaxFormula.Percentage:
                                    {
                                        //item.CrossDependencyInitialValue -= (item.BenefitCard.TaxValue * item.InitialValue) / 100;
                                        //availableCrossDeduction.CrossDependencyInitialValue += (item.BenefitCard.TaxValue * item.InitialValue) / 100;
                                        availableCrossDependencyValue = (item.BenefitCard.TaxValue * item.InitialValue) / 100;
                                        break;
                                    }
                                default:
                                    {
                                        throw new Exception("Passed formula not supported");
                                    }
                            }
                        }
                        item.CrossDependencyInitialValue -= availableCrossDependencyValue;
                        continue;
                    }

                    var crossDetails =
                        item.BenefitCard.CrossDeductions.FirstOrDefault(x => x.DeductionCard.Id == availableCrossDependency.DeductionCard.Id);
                    if (crossDetails != null)
                    {
                        switch (crossDetails.CrossType)
                        {
                            case CrossType.AsDefined:
                                {
                                    availableCrossDependencyValue = GetValueOfDeductionCrossBenefit(availableCrossDependency.Value, monthlyCard, availableCrossDependency.Formula, item.InitialValue, generalOption);
                                    //item.CrossDependencyInitialValue -= tempValue;
                                    //availableCrossDeduction.CrossDependencyInitialValue += tempValue;
                                    break;
                                }
                            case CrossType.Custom:
                                {
                                    switch (crossDetails.CrossFormula)
                                    {
                                        case CrossFormula.Nothing:
                                            {
                                                break;
                                            }
                                        case CrossFormula.FixedValue:
                                            {
                                                availableCrossDependencyValue = crossDetails.Value;
                                                //availableCrossDeduction.CrossDependencyInitialValue += crossDetails.Value;
                                                break;
                                            }
                                        case CrossFormula.Percentage:
                                            {
                                                availableCrossDependencyValue = (crossDetails.Value * item.InitialValue) / 100;
                                                //availableCrossDeduction.CrossDependencyInitialValue += (crossDetails.Value * item.InitialValue) / 100;
                                                break;
                                            }
                                        default:
                                            {
                                                throw new Exception("Passed formula not supported");
                                            }
                                    }
                                    break;
                                }
                            default:
                                {
                                    throw new Exception("Passed formula not supported");
                                }
                        }
                        var availableCrossDeductions =
                            monthlyCard.MonthlyEmployeeDeductions.Where(x => item1.BenefitCard.CrossDeductions
                                .Any(y => y.DeductionCard.Id == x.DeductionCard.Id && y.CrossDependencys.Any(z => z.DeductionCard.Id == availableCrossDependency.DeductionCard.Id))).ToList();
                        if (availableCrossDeductions.Any())
                        {
                            item.CrossDependencyInitialValue -= availableCrossDependencyValue;
                            //foreach (var availableCrossDeduction in availableCrossDeductions)
                            //{
                            //    availableCrossDeduction.CrossDependencyInitialValue += availableCrossDependencyValue;
                            //}
                        }
                    }

                }
            }







            //var availableCrossDeductions =
            //    monthlyCard.MonthlyEmployeeDeductions.Where(x => item1.BenefitCard.CrossDeductions.Any(y => y.DeductionCard.Id == x.DeductionCard.Id && y.CrossDependencys.Any())).ToList();
            //foreach (var availableCrossDeduction in availableCrossDeductions)
            //{
            //    availableCrossDeduction.CrossDependencyInitialValue = 0;
            //    var availableCrossDependencys =
            //    monthlyCard.MonthlyEmployeeDeductions.Where(x => item1.BenefitCard.CrossDeductions.Any(y => y.DeductionCard.Id == availableCrossDeduction.DeductionCard.Id &&
            //        y.CrossDependencys.Any(z => z.DeductionCard.Id == x.DeductionCard.Id))).ToList();

            //    foreach (var crossDependency in availableCrossDependencys)
            //    {
            //        var generalOption = typeof(GeneralOption).GetAll<GeneralOption>().First();
            //        if (crossDependency.DeductionCard.Id == generalOption.TaxDeduction.Id)
            //        {
            //            if (item.BenefitCard.TaxValue > 0)
            //            {
            //                switch (item.BenefitCard.TaxFormula)
            //                {
            //                    case TaxFormula.Nothing:
            //                        {
            //                            break;
            //                        }
            //                    case TaxFormula.SpecificValue:
            //                        {
            //                            //todo Mhd Alsaadi: يفضل ان يكون الضريبة الثابتة للتعويض ضمن القيم القابلة للتعديل في البطاقة الشهرية وليس في بطاقة التعويض
            //                            item.CrossDependencyInitialValue -= item.BenefitCard.TaxValue;
            //                            availableCrossDeduction.CrossDependencyInitialValue += item.BenefitCard.TaxValue;
            //                            break;
            //                        }
            //                    case TaxFormula.Percentage:
            //                        {
            //                            item.CrossDependencyInitialValue -= (item.BenefitCard.TaxValue * item.InitialValue) / 100;
            //                            availableCrossDeduction.CrossDependencyInitialValue += (item.BenefitCard.TaxValue * item.InitialValue) / 100;
            //                            break;
            //                        }
            //                    default:
            //                        {
            //                            throw new Exception("Passed formula not supported");
            //                        }
            //                }
            //            }
            //            continue;
            //        }

            //        var crossDetails =
            //            item.BenefitCard.CrossDeductions.First(x => x.DeductionCard.Id == availableCrossDeduction.DeductionCard.Id);
            //        switch (crossDetails.CrossType)
            //        {
            //            case CrossType.AsDefined:
            //                {
            //                    var tempValue = GetValueOfDeductionCrossBenefit(crossDependency.Value, crossDependency.Formula, item.InitialValue);
            //                    item.CrossDependencyInitialValue -= tempValue;
            //                    availableCrossDeduction.CrossDependencyInitialValue += tempValue;
            //                    break;
            //                }
            //            case CrossType.Custom:
            //                {
            //                    switch (crossDetails.CrossFormula)
            //                    {
            //                        case CrossFormula.Nothing:
            //                            {
            //                                break;
            //                            }
            //                        case CrossFormula.FixedValue:
            //                            {
            //                                item.CrossDependencyInitialValue -= crossDetails.Value;
            //                                availableCrossDeduction.CrossDependencyInitialValue += crossDetails.Value;
            //                                break;
            //                            }
            //                        case CrossFormula.Percentage:
            //                            {
            //                                item.CrossDependencyInitialValue -= (crossDetails.Value * item.InitialValue) / 100;
            //                                availableCrossDeduction.CrossDependencyInitialValue += (crossDetails.Value * item.InitialValue) / 100;
            //                                break;
            //                            }
            //                        default:
            //                            {
            //                                throw new Exception("Passed formula not supported");
            //                            }
            //                    }
            //                    break;
            //                }
            //            default:
            //                {
            //                    throw new Exception("Passed formula not supported");
            //                }
            //        }
            //    }


            //}
            //}
        }

        public static void CalculateTaxDeduction(MonthlyCard monthlyCard, GeneralOption generalOption)
        {

            var taxDeductionRecord =
                monthlyCard.MonthlyEmployeeDeductions.FirstOrDefault(
                    x => x.DeductionCard.Id == generalOption.TaxDeduction.Id);
            if (taxDeductionRecord != null)
            {
                var availableCrossDeductions = monthlyCard.MonthlyEmployeeDeductions
                    .Where(x => taxDeductionRecord.DeductionCard.CrossDeductionWithDeductions
                        .Any(y => y.DeductionCard.Id == x.DeductionCard.Id));
                var availableCrossBenefits = monthlyCard.MonthlyEmployeeBenefits
                    .Where(x =>
                            x.BenefitCard.CrossDeductions.Any(y => y.DeductionCard.Id == taxDeductionRecord.DeductionCard.Id) &&
                            x.BenefitCard.TaxValue <= 0); // لاستثناء التعويضات التي لها ضريبة ثابتة

                var taxThreshold = generalOption.TaxThreshold;
                if (taxDeductionRecord.DeductionCard.EffectableByPartialWorkDays)
                {
                    taxThreshold = taxThreshold / generalOption.TotalMonthDays * taxDeductionRecord.MonthlyCard.WorkDays;
                }
                var salary = taxDeductionRecord.MonthlyCard.PackageSalary;
                if (salary == 0 && monthlyCard.Month.MonthType == MonthType.BenefitOnly)
                {
                    // في حال تعويضات فقط والموظف ليس له راتب حيث يقبض راتبه من جهة وتعويضاته من البنك عندها يتم اخذ عتبة الراتب لحساب القيمة الخاضعة للضريبة 
                    salary = monthlyCard.Threshold > 0 ? monthlyCard.Threshold : 0;

                }
                var totalValue = salary - taxThreshold;
                //availableCrossBenefits.ForEach(x => totalValue += x.InitialValue);
                availableCrossDeductions.ForEach(x => totalValue -= x.InitialValue);

                var taxableAmount = totalValue; // المبلغ الخاضع للضريبة

                // availableCrossBenefits.ForEach(x => totalValue += x.InitialValue);
                if (totalValue < 0)
                {
                    taxDeductionRecord.FinalValue = 0;
                    taxableAmount = 0;
                }
                else
                {
                    //  availableCrossBenefits.ForEach(x => totalValue -= x.InitialValue);
                    taxableAmount = totalValue;


                    var taxSlices = typeof(TaxSlice).GetAll<TaxSlice>().OrderBy(x => x.StartSlice);

                    //escapedValue : مفيدة في حالة العتبة ليتم تجاوز حسابها في حال وجودها وفيدة ايضا اذا كان الشهر من نوع تعويضات فقط بحيث يتم تجاوز قيمة ضريبة الراتب
                    // مع الاستفادة من هذه القيم للرفع في عامود الشرائح
                    //var escapedValue = monthlyCard.Month.MonthType == MonthType.BenefitOnly
                    //? monthlyCard.Salary
                    //: 0;
                    //escapedValue += monthlyCard.Threshold > 0 ? monthlyCard.Threshold : 0;
                    var originalEscapedValue = totalValue;// وضع قيمة الخاضع للضريبة لاستخدامها لاحقا في حساب الضريبة على التعويضات
                    // يفيد عند حساب الضريبة على التعويضات بحيث تصبح هذه القيمة هي العتبة لحساب التعويض
                    //
                    if (monthlyCard.Month.MonthType != MonthType.BenefitOnly)
                    {
                        foreach (var slice in taxSlices)
                        {
                            var sliceRange = slice.EndSlice - slice.StartSlice;
                            var range = sliceRange;
                            //if (escapedValue > range)
                            //{
                            //    escapedValue -= range;
                            //    //range = 0;
                            //    continue;
                            //}
                            //range -= escapedValue;
                            //escapedValue = 0;

                            if (totalValue > sliceRange)
                            {
                                var value = range * slice.Percentage / 100;
                                taxDeductionRecord.FinalValue +=
                                    RoundUtility.PreDefinedRoundValue(taxDeductionRecord.DeductionCard.InitialRound, value);
                                totalValue -= sliceRange;
                            }
                            else
                            {

                                //totalValue = escapedValue > totalValue ? 0 : totalValue - escapedValue;
                                var value = totalValue * slice.Percentage / 100;
                                taxDeductionRecord.FinalValue +=
                                    RoundUtility.PreDefinedRoundValue(taxDeductionRecord.DeductionCard.InitialRound, value);
                                break;
                            }
                        }
                    }
                    taxableAmount = monthlyCard.Month.MonthType == MonthType.BenefitOnly ? 0 : taxableAmount;
                    foreach (var monthlyEmployeeBenefit in availableCrossBenefits)
                    {
                        //var dependsOnCrossedInitialValue = monthlyEmployeeBenefit.BenefitCard.CrossDeductions.First(y => y.DeductionCard.Id == taxDeductionRecord.DeductionCard.Id).DependsOnCrossedInitialValue;
                        var escapedValue = (float)originalEscapedValue;

                        var affectableValue = (float)monthlyEmployeeBenefit.InitialValue -
                                              GetDeductionCrossedInitialBenefitValue(monthlyEmployeeBenefit,
                                                  taxDeductionRecord.DeductionCard, monthlyCard, generalOption);

                        totalValue = affectableValue;
                        taxableAmount += affectableValue;



                        originalEscapedValue += affectableValue;
                        foreach (var slice in taxSlices)
                        {
                            var sliceRange = slice.EndSlice - slice.StartSlice;
                            var range = sliceRange;
                            if (escapedValue > range)
                            {
                                escapedValue -= range;
                                continue;
                            }
                            range -= escapedValue;
                            escapedValue = 0;

                            if (totalValue > range)
                            {
                                var value = range * slice.Percentage / 100;
                                taxDeductionRecord.FinalValue +=
                                    RoundUtility.PreDefinedRoundValue(taxDeductionRecord.DeductionCard.InitialRound,
                                        value);
                                monthlyEmployeeBenefit.FinalValue -=
                                    RoundUtility.PreDefinedRoundValue(taxDeductionRecord.DeductionCard.InitialRound,
                                        value);
                                totalValue -= range;
                            }
                            else
                            {



                                totalValue = escapedValue > totalValue ? 0 : totalValue - escapedValue;
                                var value = totalValue * slice.Percentage / 100;
                                taxDeductionRecord.FinalValue +=
                                    RoundUtility.PreDefinedRoundValue(taxDeductionRecord.DeductionCard.InitialRound,
                                        value);
                                monthlyEmployeeBenefit.FinalValue -=
                                    RoundUtility.PreDefinedRoundValue(taxDeductionRecord.DeductionCard.InitialRound,
                                        value);
                                break;

                            }
                        }
                    }
                }
                monthlyCard.TaxableAmount = RoundUtility.Round(taxableAmount, RoundDirection.Normal, RoundSite.AfterComma, 2);

            }

        }


        public static void CalculateExtraValuesOnFinalValues(MonthlyCard monthlyCard)
        {
            #region Extra Value

            foreach (var monthlyEmployeeBenefit in monthlyCard.MonthlyEmployeeBenefits)
            {
                if (monthlyEmployeeBenefit.ExtraValueFormula == ExtraValueFormula.PercentageOfFinalValue ||
                     monthlyEmployeeBenefit.ExtraValueFormula == ExtraValueFormula.MultipleWithFinalValue)
                {
                    monthlyEmployeeBenefit.FinalValue =
                        GetValueOfExtraFormula(monthlyEmployeeBenefit.ExtraValue,
                            monthlyEmployeeBenefit.ExtraValueFormula, monthlyEmployeeBenefit.FinalValue);
                }
            }

            #endregion


            #region Extra Value

            foreach (var monthlyEmployeeDeduction in monthlyCard.MonthlyEmployeeDeductions)
            {
                if (monthlyEmployeeDeduction.ExtraValueFormula == ExtraValueFormula.PercentageOfFinalValue ||
                     monthlyEmployeeDeduction.ExtraValueFormula == ExtraValueFormula.MultipleWithFinalValue)
                {
                    monthlyEmployeeDeduction.FinalValue =
                        GetValueOfExtraFormula(monthlyEmployeeDeduction.ExtraValue,
                            monthlyEmployeeDeduction.ExtraValueFormula, monthlyEmployeeDeduction.FinalValue);
                }
            }

            #endregion

        }

        public static void MonthlyCardRoundFinalValues(MonthlyCard monthlyCard)
        {
            foreach (var monthlyEmployeeBenefit in monthlyCard.MonthlyEmployeeBenefits)
            {
                monthlyEmployeeBenefit.FinalValue = RoundUtility.PreDefinedRoundValue(monthlyEmployeeBenefit.BenefitCard.FinalRound,
                    monthlyEmployeeBenefit.FinalValue);
            }

            foreach (var monthlyEmployeeDeduction in monthlyCard.MonthlyEmployeeDeductions)
            {
                monthlyEmployeeDeduction.FinalValue = RoundUtility.PreDefinedRoundValue(monthlyEmployeeDeduction.DeductionCard.FinalRound,
                    monthlyEmployeeDeduction.FinalValue);
            }
        }

        public static double GetValueOfFormula(double value, Formula formula, MonthlyCard monthlyCard, double reductionValue, GeneralOption generalOption)
        {// reductionValue: مفيدة لحالة التقاطع حسم مع حسم بحيث ان القيم المخففة للحسم تطرح قبل الحساب للقيمة
            double finalValue;
            var totalWorkingHours = generalOption.TakingTheTotalWorkingHoursInTheFinancialCard && monthlyCard.TotalWorkingHours > 0 ? monthlyCard.TotalWorkingHours : generalOption.TotalDayHours;
            switch (formula)
            {
                case Formula.Nothing:
                    {
                        finalValue = 0;
                        break;
                    }
                case Formula.FixedValue:
                    {
                        finalValue = value - reductionValue;
                        break;
                    }
                case Formula.PercentageOfPackageSalary:
                    {
                        finalValue = value / 100 * (monthlyCard.PackageSalary - reductionValue);
                        break;
                    }
                case Formula.PercentageOfSalary:
                    {
                        finalValue = value / 100 * (monthlyCard.Salary - reductionValue);
                        break;
                    }
                case Formula.PercentageOfInsuranceSalary:
                    {
                        finalValue = value / 100 * (monthlyCard.InsuranceSalary - reductionValue);
                        break;
                    }
                case Formula.PercentageOfBenefitSalary:
                    {
                        finalValue = value / 100 * (monthlyCard.BenefitSalary - reductionValue);
                        break;
                    }
                case Formula.PercentageOfTempSalary1:
                    {
                        finalValue = value / 100 * (monthlyCard.TempSalary1 - reductionValue);
                        break;
                    }
                case Formula.PercentageOfTempSalary2:
                    {
                        finalValue = value / 100 * (monthlyCard.TempSalary2 - reductionValue);
                        break;
                    }
                case Formula.PercentageOfCategoryCeil:
                    {
                        finalValue = value / 100 * (GeneralService.GetEmployeeCategoryMaxCeil(monthlyCard.PrimaryCard.Employee) - reductionValue);
                        break;
                    }

                case Formula.DaysOfPackageSalary:
                    {
                        finalValue = value * (monthlyCard.PackageSalary - reductionValue) / generalOption.TotalMonthDays;
                        break;
                    }
                case Formula.DaysOfSalary:
                    {
                        finalValue = value * (monthlyCard.Salary - reductionValue) / generalOption.TotalMonthDays;
                        break;
                    }
                case Formula.DaysOfInsuranceSalary:
                    {
                        finalValue = value * (monthlyCard.InsuranceSalary - reductionValue) / generalOption.TotalMonthDays;
                        break;
                    }
                case Formula.DaysOfBenefitSalary:
                    {
                        finalValue = value * (monthlyCard.BenefitSalary - reductionValue) / generalOption.TotalMonthDays;
                        break;
                    }
                case Formula.DaysOfTempSalary1:
                    {
                        finalValue = value * (monthlyCard.TempSalary1 - reductionValue) / generalOption.TotalMonthDays;
                        break;
                    }
                case Formula.DaysOfTempSalary2:
                    {
                        finalValue = value * (GeneralService.GetEmployeeCategoryMaxCeil(monthlyCard.PrimaryCard.Employee) - reductionValue) / generalOption.TotalMonthDays;
                        break;
                    }
                case Formula.DaysOfCategoryCeil:
                    {
                        finalValue = value * (monthlyCard.TempSalary2 - reductionValue) / generalOption.TotalMonthDays;
                        break;
                    }
                case Formula.HoursOfPackageSalary:
                    {
                        finalValue = value * (monthlyCard.PackageSalary - reductionValue) / generalOption.TotalMonthDays / totalWorkingHours;
                        break;
                    }
                case Formula.HoursOfSalary:
                    {
                        finalValue = value * (monthlyCard.Salary - reductionValue) / generalOption.TotalMonthDays / totalWorkingHours;
                        break;
                    }
                case Formula.HoursOfInsuranceSalary:
                    {
                        finalValue = value * (monthlyCard.InsuranceSalary - reductionValue) / generalOption.TotalMonthDays /
                                     totalWorkingHours;
                        break;
                    }
                case Formula.HoursOfBenefitSalary:
                    {
                        finalValue = value * (monthlyCard.BenefitSalary - reductionValue) / generalOption.TotalMonthDays /
                                     totalWorkingHours;
                        break;
                    }
                case Formula.HoursOfTempSalary1:
                    {
                        finalValue = value * (monthlyCard.TempSalary1 - reductionValue) / generalOption.TotalMonthDays / totalWorkingHours;
                        break;
                    }
                case Formula.HoursOfTempSalary2:
                    {
                        finalValue = value * (monthlyCard.TempSalary2 - reductionValue) / generalOption.TotalMonthDays / totalWorkingHours;
                        break;
                    }
                case Formula.HoursOfCategoryCeil:
                    {
                        finalValue = value * (GeneralService.GetEmployeeCategoryMaxCeil(monthlyCard.PrimaryCard.Employee) - reductionValue) / generalOption.TotalMonthDays / totalWorkingHours;
                        break;
                    }
                default:
                    throw new Exception("Passed formula not supported");
            }
            return finalValue;
        }

        public static double GetValueOfExtraFormula(double extraValue, ExtraValueFormula extraValueFormula, double value)
        {
            double tempFinalValue;

            if (extraValueFormula == ExtraValueFormula.None)
            {
                tempFinalValue = value;
            }
            else if (extraValueFormula == ExtraValueFormula.MultipleWithFinalValue || extraValueFormula == ExtraValueFormula.MultipleWithInitialValue)
            {
                tempFinalValue = value * extraValue;
            }
            else if (extraValueFormula == ExtraValueFormula.PercentageOfFinalValue || extraValueFormula == ExtraValueFormula.PercentageOfInitialValue)
            {
                tempFinalValue = value + (value * extraValue / 100);
            }
            else
            {
                throw new Exception("Passed extra formula not supported");
            }
            return tempFinalValue;
        }

        public static double GetValueOfDeductionCrossBenefit(double value, MonthlyCard monthlyCard, Formula formula, double initialValue, GeneralOption generalOption)
        {
            double finalValue;
            var totalWorkingHours = generalOption.TakingTheTotalWorkingHoursInTheFinancialCard && monthlyCard.TotalWorkingHours > 0 ? monthlyCard.TotalWorkingHours : generalOption.TotalDayHours;

            if (formula == Formula.Nothing)
            {
                finalValue = 0;
            }
            else if (formula == Formula.FixedValue)
            {
                finalValue = value;
            }
            else if (formula == Formula.HoursOfPackageSalary || formula == Formula.PercentageOfSalary || formula == Formula.PercentageOfInsuranceSalary
                     || formula == Formula.PercentageOfBenefitSalary || formula == Formula.PercentageOfTempSalary1 ||
                     formula == Formula.PercentageOfTempSalary2)
            {
                finalValue = value / 100 * initialValue;
            }
            else if (formula == Formula.DaysOfPackageSalary || formula == Formula.DaysOfSalary || formula == Formula.DaysOfInsuranceSalary
                     || formula == Formula.DaysOfBenefitSalary || formula == Formula.DaysOfTempSalary1 ||
                     formula == Formula.DaysOfTempSalary2)
            {
                finalValue = value * initialValue / generalOption.TotalMonthDays;
            }
            else if (formula == Formula.HoursOfPackageSalary || formula == Formula.HoursOfSalary || formula == Formula.HoursOfInsuranceSalary
                     || formula == Formula.HoursOfBenefitSalary || formula == Formula.HoursOfTempSalary1 ||
                     formula == Formula.HoursOfTempSalary2)
            {
                finalValue = value * initialValue / generalOption.TotalMonthDays / totalWorkingHours;
            }
            else
            {
                throw new Exception("Passed formula not supported");
            }
            return finalValue;
        }

        public static void SetMonthStatusToPartialyCalculated(int monthId)
        {
            var month = (Month)typeof(Month).GetById(monthId);
            month.MonthStatus =
                month.MonthStatus == MonthStatus.Calculated
                ? MonthStatus.PartialyCalculated
                : month.MonthStatus;
            month.Save();
        }

        public static void SetMonthlyCardStatusToUnCalculated(int monthlyCardId)
        {
            var monthlyCard = (MonthlyCard)typeof(MonthlyCard).GetById(monthlyCardId);
            monthlyCard.IsCalculated = false;
            monthlyCard.Save();
            SetMonthStatusToPartialyCalculated(monthlyCard.Month.Id);
        }

        public static void SetMonthlyCardStatusToNotAudited(int monthlyCardId)
        {
            var monthlyCard = (MonthlyCard)typeof(MonthlyCard).GetById(monthlyCardId);
            //monthlyCard.AuditState = AuditState.NotAudited;
            monthlyCard.Save();
        }
    }
}