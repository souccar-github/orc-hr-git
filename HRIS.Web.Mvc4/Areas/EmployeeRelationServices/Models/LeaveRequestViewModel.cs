using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentNHibernate.Testing.Values;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.RootEntities;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Helper;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Mapping;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Domain.Workflow.Enums;
using Souccar.Infrastructure.Core;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Services;
using Project.Web.Mvc4.Helpers.Resource;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.Workflow;
using Project.Web.Mvc4.Extensions;
using Souccar.Domain.Workflow.RootEntities;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class LeaveRequestViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(LeaveRequestViewModel).FullName;
            model.Views[0].EditHandler = "LeaveRequestEditHandler";
            model.Views[0].ViewHandler = "LeaveRequestViewHandler";
            model.ActionListHandler = "initializeLeaveRequestActionList";

        }
        public override System.Web.Mvc.ActionResult BeforeCreate(RequestInformation requestInformation, string customInformation = null)
        {
            var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);
            if (employeeCard.CardStatus != EmployeeCardStatus.OnHeadOfHisWork)

                return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new
                {
                    Data = false,
                    message =
                    EmployeeRelationServicesLocalizationHelper.GetResource(
                        employeeCard.CardStatus == EmployeeCardStatus.New ?
                        EmployeeRelationServicesLocalizationHelper.TheEmployeeWhoYouHaveSelectedIsNew :
                    EmployeeRelationServicesLocalizationHelper.TheEmployeeWhoYouHaveSelectedIsResignedOrTerminated)
                });
            else
                return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { Data = true, message = "" });
        }
        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            this.PreventDefault = true;
            var start = DateTime.Now;
            var leaveRequest = (LeaveRequest)entity;
            leaveRequest.LeaveStatus = Status.Approved;
            leaveRequest.Creator = UserExtensions.CurrentUser;
            leaveRequest.CreationDate = DateTime.Now;
            var leaveSetting = leaveRequest.LeaveSetting;
            var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);
            ServiceHelper.GetLeaveInfo(leaveRequest, leaveSetting, employeeCard.Employee);

            var startDate = new DateTime(leaveRequest.StartDate.Year, leaveRequest.StartDate.Month, leaveRequest.StartDate.Day, 0, 0, 0);
            var endDate = new DateTime(leaveRequest.EndDate.Year, leaveRequest.EndDate.Month, leaveRequest.EndDate.Day, 0, 0, 0);
            leaveRequest.StartDate = startDate;
            leaveRequest.EndDate = endDate;

            if (leaveRequest.IsHourlyLeave)
            {
                var fromTime = new DateTime(2000, 1, 1, leaveRequest.FromTime.Value.Hour, leaveRequest.FromTime.Value.Minute, leaveRequest.FromTime.Value.Second);
                var toTime = new DateTime(2000, 1, 1, leaveRequest.ToTime.Value.Hour, leaveRequest.ToTime.Value.Minute, leaveRequest.ToTime.Value.Second);
                leaveRequest.FromTime = fromTime;
                leaveRequest.ToTime = toTime;

                leaveRequest.FromDateTime = new DateTime(leaveRequest.StartDate.Year, leaveRequest.StartDate.Month,
                    leaveRequest.StartDate.Day, leaveRequest.FromTime.Value.Hour, leaveRequest.FromTime.Value.Minute,
                    leaveRequest.FromTime.Value.Second);

                leaveRequest.ToDateTime = new DateTime(leaveRequest.EndDate.Year, leaveRequest.EndDate.Month,
                leaveRequest.EndDate.Day, leaveRequest.ToTime.Value.Hour, leaveRequest.ToTime.Value.Minute,
                leaveRequest.ToTime.Value.Second);
                if (leaveRequest.ToDateTime < leaveRequest.FromDateTime)
                {
                    var dateswapFornull = new DateTime(leaveRequest.EndDate.Year, leaveRequest.EndDate.Month,
                    leaveRequest.EndDate.Day, leaveRequest.ToTime.Value.Hour, leaveRequest.ToTime.Value.Minute,
                    leaveRequest.ToTime.Value.Second);
                    leaveRequest.ToDateTime = dateswapFornull.AddDays(1);
                }
            }
            else
            {
                if (leaveRequest.FromTime != null)
                {
                    leaveRequest.FromTime = null;
                    leaveRequest.FromDateTime = null;
                }
                if (leaveRequest.ToTime != null)
                {
                    leaveRequest.ToTime = null;
                    leaveRequest.ToDateTime = null;
                }
            }




            //if (leaveSetting.IsIndivisible)
            //{
            //    var balance = LeaveService.GetBalance(leaveSetting.Type, employeeCard.Employee);
            //    var recycledBalance = LeaveService.GetRecycledBalance(employeeCard.Employee, leaveSetting, DateTime.Today.Year - 1);
            //    balance += recycledBalance;
            //    leaveRequest.SpentDays = balance;
            //    leaveRequest.EndDate = LeaveService.GetEndDate(leaveRequest.StartDate, balance, leaveSetting.IsContinuous);
            //}
            //else
            //{
            //    if (leaveRequest.IsHourlyLeave)
            //    {
            //        var minutes = (leaveRequest.ToTime.TimeOfDay - leaveRequest.FromTime.TimeOfDay).TotalMinutes;
            //        var spentDays =
            //            Math.Round(1 / ((leaveSetting.HoursEquivalentToOneLeaveDay * EmployeeRelationServicesConstants.NumberOfMinutesInHour) / minutes), 2);
            //        leaveRequest.SpentDays = spentDays;
            //    }
            //    else
            //    {
            //        leaveRequest.SpentDays = LeaveService.GetSpentDays(leaveRequest.StartDate, leaveRequest.EndDate,
            //        leaveSetting.IsContinuous);
            //    }
            //}

            //employeeCard.AddLeaveRequest(leaveRequest);
            //ServiceFactory.ORMService.Save(employeeCard, UserExtensions.CurrentUser);
            var entities = new List<IAggregateRoot>();
            entities.Add(employeeCard);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AddLeave);

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, leaveRequest, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { leaveRequest.EmployeeCard.Employee });

        }

        public override void BeforeUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, string customInformation = null)
        {
            this.PreventDefault = true;
            var start = DateTime.Now;
            var leaveRequest = (LeaveRequest)entity;
            var leaveSetting = leaveRequest.LeaveSetting;
            var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);
            ServiceHelper.GetLeaveInfo(leaveRequest, leaveSetting, employeeCard.Employee);

            var startDate = new DateTime(leaveRequest.StartDate.Year, leaveRequest.StartDate.Month, leaveRequest.StartDate.Day, 0, 0, 0);
            var endDate = new DateTime(leaveRequest.EndDate.Year, leaveRequest.EndDate.Month, leaveRequest.EndDate.Day, 0, 0, 0);
            leaveRequest.StartDate = startDate;
            leaveRequest.EndDate = endDate;

            if (leaveRequest.IsHourlyLeave)
            {
                var fromTime = new DateTime(2000, 1, 1, leaveRequest.FromTime.Value.Hour, leaveRequest.FromTime.Value.Minute, leaveRequest.FromTime.Value.Second);
                var toTime = new DateTime(2000, 1, 1, leaveRequest.ToTime.Value.Hour, leaveRequest.ToTime.Value.Minute, leaveRequest.ToTime.Value.Second);
                leaveRequest.FromTime = fromTime;
                leaveRequest.ToTime = toTime;

                leaveRequest.FromDateTime = new DateTime(leaveRequest.StartDate.Year, leaveRequest.StartDate.Month,
                  leaveRequest.StartDate.Day, leaveRequest.FromTime.Value.Hour, leaveRequest.FromTime.Value.Minute,
                  leaveRequest.FromTime.Value.Second);

                leaveRequest.ToDateTime = new DateTime(leaveRequest.EndDate.Year, leaveRequest.EndDate.Month,
                 leaveRequest.EndDate.Day, leaveRequest.ToTime.Value.Hour, leaveRequest.ToTime.Value.Minute,
                 leaveRequest.ToTime.Value.Second);
                if (leaveRequest.ToDateTime < leaveRequest.FromDateTime)
                {
                    var dateswapFornull = new DateTime(leaveRequest.EndDate.Year, leaveRequest.EndDate.Month,
                    leaveRequest.EndDate.Day, leaveRequest.ToTime.Value.Hour, leaveRequest.ToTime.Value.Minute,
                    leaveRequest.ToTime.Value.Second);
                    leaveRequest.ToDateTime = dateswapFornull.AddDays(1);

                }
            }

            //if (leaveSetting.IsIndivisible)
            //{
            //    var balance = LeaveService.GetBalance(leaveSetting.Type, employeeCard.Employee);
            //    var recycledBalance = LeaveService.GetRecycledBalance(employeeCard.Employee, leaveSetting, DateTime.Today.Year - 1);
            //    balance += recycledBalance;
            //    leaveRequest.SpentDays = balance;
            //    leaveRequest.EndDate = LeaveService.GetEndDate(leaveRequest.StartDate, balance, leaveSetting.IsContinuous);
            //}
            //else
            //{
            //    if (leaveRequest.IsHourlyLeave)
            //    {
            //        var minutes = (leaveRequest.ToTime.TimeOfDay - leaveRequest.FromTime.TimeOfDay).TotalMinutes;
            //        var spentDays =
            //            Math.Round(1 / ((leaveSetting.HoursEquivalentToOneLeaveDay * EmployeeRelationServicesConstants.NumberOfMinutesInHour) / minutes), 2);
            //        leaveRequest.SpentDays = spentDays;
            //    }
            //    else
            //    {
            //        leaveRequest.SpentDays = LeaveService.GetSpentDays(leaveRequest.StartDate, leaveRequest.EndDate,
            //        leaveSetting.IsContinuous);
            //    }
            //}

            //employeeCard.AddLeaveRequest(leaveRequest);
            //ServiceFactory.ORMService.Save(employeeCard, UserExtensions.CurrentUser);
            var entities = new List<IAggregateRoot>();
            entities.Add(employeeCard);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.UpdateLeave);

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, leaveRequest, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { leaveRequest.EmployeeCard.Employee });

        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<Souccar.Domain.Validation.ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var leaveRequest = (LeaveRequest)entity;
            var leaveSetting = leaveRequest.LeaveSetting;
            var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);
            if (leaveSetting.MaturityPeriod > (DateTime.Now.Subtract((DateTime)employeeCard.StartWorkingDate)).Days / (365.25 / 12))
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgTheMaturityPeriodHasNotBeenMet),
                });
                return;
            }
            if (leaveRequest.StartDate.Year != leaveRequest.EndDate.Year)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgPleaseSeprateTheLeaveInTwoLeavesEveryOneInDifferentYear),
                    Property = typeof(LeaveRequest).GetProperty("StartDate")
                });
                return;
            }
            if (leaveRequest.StartDate == DateTime.MinValue)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgStartDateIsRequired),
                    Property = typeof(LeaveRequest).GetProperty("StartDate")
                });
                return;
            }

            if (leaveRequest.EndDate == DateTime.MinValue)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEndDateIsRequired),
                    Property = typeof(LeaveRequest).GetProperty("EndDate")
                });
                return;
            }

            if (leaveRequest.RequestDate == DateTime.MinValue)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgRequestDateIsRequired),
                    Property = typeof(LeaveRequest).GetProperty("RequestDate")
                });
                return;
            }



            double spentDaysBeforUpdate = 0;

            if (operationType == CrudOperationType.Update)
            {
                spentDaysBeforUpdate = LeaveService.GetDaysCountInLeave(leaveRequest, DateTime.Today.Year);
            }

            if (operationType == CrudOperationType.Insert)
            {
                if (leaveSetting.HasMaximumNumber)
                {
                    var countInYear = LeaveService.GetCountInYears(employeeCard.Employee, leaveSetting);
                    if (countInYear == leaveSetting.MaximumNumber)
                    {
                        var prop = typeof(LeaveSetting).GetProperty("MaximumNumber");
                        validationResults.Add(new ValidationResult()
                        {
                            Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgSorryYouPassedMaximumNumberForThisLeave),
                            Property = prop
                        });

                        return;
                    }
                }
            }

            //Check Balance & Monthly Balance
            var balance = LeaveService.GetBalance(leaveSetting, employeeCard.Employee, false, leaveRequest.StartDate.Date);
            if (leaveSetting.HasMaximumNumber)
                balance = balance * leaveSetting.MaximumNumber;
            var recycledBalance = LeaveService.GetRecycledBalance(employeeCard.Employee, leaveSetting, leaveRequest.StartDate.Year - 1);
            balance += recycledBalance;
            var granted = LeaveService.GetGranted(employeeCard.Employee, leaveSetting, leaveRequest.StartDate.Year);
            if (operationType == CrudOperationType.Update)
                granted -= spentDaysBeforUpdate;
            var remain = Math.Round(balance - granted, 2);

            var hasMonthlyBalance = LeaveService.HasMonthlyBalance(leaveSetting, employeeCard.Employee);
            double monthlyBalance = 0;
            double monthlyGranted = 0;
            if (hasMonthlyBalance)
            {
                monthlyBalance = LeaveService.GetMonthlyBalance(leaveSetting, employeeCard.Employee, leaveRequest.StartDate);
                monthlyGranted = LeaveService.GetMonthlyGranted(employeeCard.Employee, leaveSetting, leaveRequest.StartDate);
                if (operationType == CrudOperationType.Update)
                    monthlyGranted -= spentDaysBeforUpdate;
            }
            var monthlyRemain = Math.Round(monthlyBalance - monthlyGranted, 2);

            if (leaveSetting.IsIndivisible)
            {
                var endDate = LeaveService.GetEndDate(leaveRequest.StartDate, balance, leaveSetting.IsContinuous, employeeCard.Employee);
                if (leaveSetting.HasMaximumNumber)
                    balance = balance / leaveSetting.MaximumNumber;
                if (balance > remain)
                {
                    if (leaveSetting.HasMaximumNumber)
                    {
                        var prop = typeof(LeaveRequest).GetProperty("StartDate");
                        validationResults.Add(new ValidationResult()
                        {
                            Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgSorryYouPassedMaximumNumberForThisLeave),
                            Property = prop
                        });
                    }
                    else
                    {
                        var prop = typeof(LeaveRequest).GetProperty("StartDate");
                        validationResults.Add(new ValidationResult()
                        {
                            Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouDoNotHaveEnoughBalanceTheRemainDaysIsGreaterThanTheRequiredDays),
                            Property = prop
                        });
                    }

                    return;
                }

                //Stopped Here....

                if (!IsHourlyLeave)
                {
                    if (operationType == CrudOperationType.Insert)
                    {
                        if (!LeaveService.IsValidLeaveDate(employeeCard, leaveSetting, DateTime.Parse(leaveRequest.StartDate.ToShortDateString()),
                            DateTime.Parse(endDate.ToShortDateString())))
                        {
                            var prop = typeof(LeaveRequest).GetProperty("StartDate");
                            validationResults.Add(new ValidationResult()
                            {
                                Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgThereIsLeaveWithSameDate),
                                Property = prop
                            });

                            return;
                        }
                    }

                    if (operationType == CrudOperationType.Update)
                    {
                        if (!LeaveService.IsValidLeaveDate(employeeCard, leaveSetting, DateTime.Parse(leaveRequest.StartDate.ToShortDateString()),
                            DateTime.Parse(endDate.ToShortDateString()), leaveRequest))
                        {
                            var prop = typeof(LeaveRequest).GetProperty("StartDate");
                            validationResults.Add(new ValidationResult()
                            {
                                Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgThereIsLeaveWithSameDate),
                                Property = prop
                            });

                            return;
                        }
                    }
                }

            }
            else
            {
                if (leaveRequest.IsHourlyLeave)
                {
                    var minutes = (leaveRequest.ToTime.GetValueOrDefault().TimeOfDay - leaveRequest.FromTime.GetValueOrDefault().TimeOfDay).TotalMinutes;
                    var spentDays =
                        Math.Round(1 / ((leaveSetting.HoursEquivalentToOneLeaveDay * EmployeeRelationServicesConstants.NumberOfMinutesInHour) / minutes), 2);
                    if (spentDays == 0)
                    {
                        var prop = typeof(LeaveRequest).GetProperty("Description");
                        validationResults.Add(new ValidationResult()
                        {
                            Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouCanNotTakeLeaveBecauseTheDurationIsZero),
                            Property = prop
                        });

                        return;
                    }
                    if (spentDays > remain)
                    {
                        var prop = typeof(LeaveRequest).GetProperty("FromTime");
                        validationResults.Add(new ValidationResult()
                        {
                            Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouDoNotHaveEnoughBalanceTheRemainDaysIsGreaterThanTheRequiredDays),
                            Property = prop
                        });

                        return;
                    }

                    if (hasMonthlyBalance)
                    {
                        if (spentDays > monthlyRemain)
                        {
                            var prop = typeof(LeaveRequest).GetProperty("Description");
                            validationResults.Add(new ValidationResult()
                            {
                                Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouDoNotHaveEnoughMonthlyBalanceTheRemainMonthlyDaysIsAndTheRequiredDaysIs),
                                Property = prop
                            });

                            return;
                        }
                    }

                    var minutesGrantedInSameDay =
                        employeeCard.LeaveRequests.Where(x => DateTime.Parse(x.StartDate.ToShortDateString()) == DateTime.Parse(leaveRequest.StartDate.ToShortDateString()))
                            .Sum(x => (x.ToTime.GetValueOrDefault().TimeOfDay - x.FromTime.GetValueOrDefault().TimeOfDay).TotalMinutes);

                    if ((leaveSetting.MaximumHoursPerDay * EmployeeRelationServicesConstants.NumberOfMinutesInHour) < minutesGrantedInSameDay)
                    {
                        var prop = typeof(LeaveRequest).GetProperty("Description");
                        validationResults.Add(new ValidationResult()
                        {
                            Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveExceededMaximumHoursPerDay),
                            Property = prop
                        });

                        return;
                    }

                }
                else
                {
                    var spentDays = LeaveService.GetSpentDays(leaveRequest.StartDate, leaveRequest.EndDate, leaveSetting.IsContinuous, employeeCard.Employee);
                    if (spentDays == 0)
                    {
                        var prop = typeof(LeaveRequest).GetProperty("Description");
                        validationResults.Add(new ValidationResult()
                        {
                            Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouCanNotTakeLeaveBecauseTheDurationIsZero),
                            Property = prop
                        });

                        return;
                    }
                    if (spentDays > remain)
                    {
                        var prop = typeof(LeaveRequest).GetProperty("Description");
                        validationResults.Add(new ValidationResult()
                        {
                            Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouDoNotHaveEnoughBalanceTheRemainDaysIsGreaterThanTheRequiredDays),
                            Property = prop
                        });

                        return;
                    }

                    if (hasMonthlyBalance)
                    {
                        var prop = typeof(LeaveRequest).GetProperty("Description");
                        if (spentDays > monthlyRemain)
                        {
                            validationResults.Add(new ValidationResult()
                            {
                                Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouDoNotHaveEnoughMonthlyBalanceTheRemainMonthlyDaysIsAndTheRequiredDaysIs),
                                Property = prop
                            });

                            return;
                        }
                    }
                }

                if (operationType == CrudOperationType.Insert)
                {
                    bool isValidLeaveDate = LeaveService.IsValidLeaveDate(employeeCard, leaveSetting,
                        DateTime.Parse(leaveRequest.StartDate.ToShortDateString()),
                        DateTime.Parse(leaveRequest.EndDate.ToShortDateString()));

                    if (!isValidLeaveDate && !leaveRequest.IsHourlyLeave && !IsHourlyLeave)
                    {
                        var prop = typeof(LeaveRequest).GetProperty("StartDate");
                        validationResults.Add(new ValidationResult()
                        {
                            Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgThereIsLeaveWithSameDate),
                            Property = prop
                        });

                        return;
                    }
                }

                if (operationType == CrudOperationType.Update)
                {
                    bool isValidLeaveDate = LeaveService.IsValidLeaveDate(employeeCard, leaveSetting,
                        DateTime.Parse(leaveRequest.StartDate.ToShortDateString()),
                        DateTime.Parse(leaveRequest.EndDate.ToShortDateString()), leaveRequest);

                    if (!isValidLeaveDate && !leaveRequest.IsHourlyLeave)
                    {
                        var prop = typeof(LeaveRequest).GetProperty("StartDate");
                        validationResults.Add(new ValidationResult()
                        {
                            Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgThereIsLeaveWithSameDate),
                            Property = prop
                        });

                        return;
                    }
                }
            }

            if (leaveRequest.IsHourlyLeave)
            {
                if (string.IsNullOrEmpty(leaveRequest.FromTime.GetValueOrDefault().ToShortTimeString()))
                {
                    var prop = typeof(LeaveRequest).GetProperty("FromTime");
                    validationResults.Add(new ValidationResult()
                    {
                        Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgFromTimeIsRequired),
                        Property = prop
                    });

                    return;
                }

                if (string.IsNullOrEmpty(leaveRequest.ToTime.GetValueOrDefault().ToShortTimeString()))
                {
                    var prop = typeof(LeaveRequest).GetProperty("ToTime");
                    validationResults.Add(new ValidationResult()
                    {
                        Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgToTimeIsRequired),
                        Property = prop
                    });

                    return;
                }

                var minutes = 0.00;
                if (leaveRequest.FromTime > leaveRequest.ToTime)
                {
                    var maxDay = new DateTime(2000, 1, 1, 23, 59, 59);
                    var minDay = new DateTime(2000, 1, 1, 0, 0, 0);
                    var minutesbefore = (maxDay.TimeOfDay - leaveRequest.FromTime.GetValueOrDefault().TimeOfDay).TotalMinutes;
                    var minutesafter = (leaveRequest.ToTime.GetValueOrDefault().TimeOfDay - minDay.TimeOfDay).TotalMinutes;
                    minutes = Math.Round(minutesafter + minutesbefore, 0);

                }
                else
                {
                    minutes = (leaveRequest.ToTime.GetValueOrDefault().TimeOfDay - leaveRequest.FromTime.GetValueOrDefault().TimeOfDay).TotalMinutes;

                }
                var maximumMinutesPerDay =
                    leaveSetting.MaximumHoursPerDay * EmployeeRelationServicesConstants.NumberOfMinutesInHour;
                var spentDays =
                    Math.Round(1 / ((leaveSetting.HoursEquivalentToOneLeaveDay * EmployeeRelationServicesConstants.NumberOfMinutesInHour) / minutes), 2);
                if (minutes > maximumMinutesPerDay)
                {
                    var prop = typeof(LeaveRequest).GetProperty("FromTime");
                    validationResults.Add(new ValidationResult()
                    {
                        Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgRequiredHoursIsGreaterThanAllowedHoursPerDay),
                        Property = prop
                    });

                    return;
                }

                if (operationType == CrudOperationType.Insert)
                {
                    bool isHourlyLeaveValidLeave = LeaveService.IsHourlyLeaveValidLeave(
                        employeeCard,
                        leaveSetting,
                        DateTime.Parse(leaveRequest.StartDate.ToShortDateString()),
                        leaveRequest.FromTime.Value.TimeOfDay,
                        leaveRequest.ToTime.Value.TimeOfDay,
                        leaveRequest);

                    if (!isHourlyLeaveValidLeave)
                    {
                        var prop = typeof(LeaveRequest).GetProperty("FromTime");
                        validationResults.Add(new ValidationResult()
                        {
                            Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.LeaveHoursIsNotAvaliable),
                            Property = prop
                        });

                        return;
                    }
                }

                if (operationType == CrudOperationType.Update)
                {
                    bool isHourlyLeaveValidLeave = LeaveService.IsHourlyLeaveValidLeave(
                        employeeCard,
                        leaveSetting,
                        DateTime.Parse(leaveRequest.StartDate.ToShortDateString()),
                        leaveRequest.FromTime.Value.TimeOfDay,
                        leaveRequest.ToTime.Value.TimeOfDay, leaveRequest, leaveRequest);

                    if (!isHourlyLeaveValidLeave)
                    {
                        var prop = typeof(LeaveRequest).GetProperty("FromTime");
                        validationResults.Add(new ValidationResult()
                        {
                            Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.LeaveHoursIsNotAvaliable),
                            Property = prop
                        });

                        return;
                    }
                }

            }
            else
            {
                if (string.IsNullOrEmpty(leaveRequest.StartDate.ToShortDateString()))
                {
                    var prop = typeof(LeaveRequest).GetProperty("StartDate");
                    validationResults.Add(new ValidationResult()
                    {
                        Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgStartDateIsRequired),
                        Property = prop
                    });

                    return;
                }

                if (!leaveRequest.LeaveSetting.IsIndivisible)
                {
                    if (string.IsNullOrEmpty(leaveRequest.EndDate.ToShortDateString()))
                    {
                        var prop = typeof(LeaveRequest).GetProperty("EndDate");
                        validationResults.Add(new ValidationResult()
                        {
                            Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEndDateIsRequired),
                            Property = prop
                        });

                        return;
                    }

                    if (leaveRequest.EndDate < leaveRequest.StartDate)
                    {
                        var prop = typeof(LeaveRequest).GetProperty("EndDate");
                        validationResults.Add(new ValidationResult()
                        {
                            Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEndDateShouldBeGreaterThanOrEqualStartDate),
                            Property = prop
                        });

                        return;
                    }
                }
            }

            if (!LeaveService.IsValidIntervalDays(leaveRequest.RequestDate, leaveRequest.StartDate, leaveSetting.IntervalDays))
            {
                var prop = typeof(LeaveRequest).GetProperty("StartDate");
                validationResults.Add(new ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgDifferenceBetweenRequestDateAndStartDateSmallerThanIntervalDays),
                    Property = prop
                });

                return;
            }

        }

        public override void AfterDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var leaveRequest = (LeaveRequest)entity;
            var workflowItem = leaveRequest.WorkflowItem;
                //ServiceFactory.ORMService.All<WorkflowItem>().Where(x => x.Id == leaveRequest.WorkflowItem.Id).FirstOrDefault();
            if (workflowItem != null)
                workflowItem.Delete();
        }
        public int EmployeeId { get; set; }
        public int PositionId { get; set; }
        public string FullName { get; set; }
        public string PositionName { get; set; }
        public int LeaveId { get; set; }
        public int LeaveSettingId { get; set; }
        public string LeaveSettingName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsHourlyLeave { get; set; }
        public bool IsSummerDate { get; set; }
        public DateTime? FromTime { get; set; }
        public DateTime? ToTime { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public string Note { get; set; }
        public double SpentDays { get; set; }
        public string LeaveReason { get; set; }
        public int LeaveReasonId { get; set; }
        public DateTime RequestDate { get; set; }
        public List<string> AttachmentsPaths { get; set; }
        public string Description { get; set; }
        public int WorkflowItemId { get; set; }
        public WorkflowPendingType PendingType { get; set; }


    }
}