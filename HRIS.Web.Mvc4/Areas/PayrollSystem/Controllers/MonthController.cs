using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FluentNHibernate.Testing.Values;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.JobDescription.Entities;

using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Validation.MessageKeys;
using Project.Web.Mvc4.Areas.PayrollSystem.Services;
using Project.Web.Mvc4.Controllers;
using Project.Web.Mvc4.Models;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using Project.Web.Mvc4.Extensions;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.Global.Constant;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Domain.Audit;

namespace Project.Web.Mvc4.Areas.PayrollSystem.Controllers
{
    public class MonthController : Controller
    {
        // كافة العمليات على الشهر مثل توليد احتساب رفض قبول قفل
        [HttpPost]
        public ActionResult MonthOperation(int monthId, string operation)
        {
            var message = String.Empty;
            var isSuccess = false;
            var month = (Month)typeof(Month).GetById(monthId);
            DateTime start = DateTime.Now;
            var command = "";
          
            var needAuditing = false;
           
            try
            {
                switch (operation)
                {
                    case "Calculate":
                        {
                            command = CommandsNames.CalculateMonth;
                            if (month.MonthStatus == MonthStatus.Approved || month.MonthStatus == MonthStatus.Locked || month.MonthStatus == MonthStatus.Created)
                            {
                                message = ServiceFactory.LocalizationService
                                    .GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.CannotCalculateLockedOrApprovedOrCreatedMonths));
                                break;
                            }
                            var generalOption = typeof(GeneralOption).GetAll<GeneralOption>();
                            if (!generalOption.Any())
                            {
                                message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.NoGeneralOptionInDatabase));
                                break;
                            }
                            var taxSlices = typeof(TaxSlice).GetAll<TaxSlice>();
                            if (!taxSlices.Any())
                            {
                                message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.NoTaxSliceInDatabase));
                                break;
                            }


                            MonthService.CalculateMonth(month);
                            needAuditing = true;
                            break;
                        }
                    case "Reject":
                        {
                            command = CommandsNames.RejectMonth;
                            if (month.MonthStatus != MonthStatus.Calculated)
                            {
                                message = ServiceFactory.LocalizationService
                                    .GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.CannotRejectOnlyCalculatedMonthsCanBeRejected));
                                break;
                            }
                            MonthService.RejectMonth(month);
                            needAuditing = true;
                            break;
                        }
                    case "Approve":
                        {
                            command = CommandsNames.ApproveMonth;
                            if (month.MonthStatus != MonthStatus.Calculated)
                            {
                                message = ServiceFactory.LocalizationService
                                    .GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.CannotApproveOnlyCalculatedMonthsCanBeApproved));
                                break;
                            }
                            MonthService.ApproveMonth(month);
                            needAuditing = true;
                            break;
                        }
                    case "Lock":
                        {
                            command = CommandsNames.LockMonth;
                            if (month.MonthStatus != MonthStatus.Approved)
                            {
                                message = ServiceFactory.LocalizationService
                                    .GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.CannotLockOnlyApprovedMonthsCanBeLocked));
                                break;
                            }
                            MonthService.LockMonth(month);
                            needAuditing = true;
                            break;
                        }
                }

                if (needAuditing)
                {
                    var commandName = ServiceFactory.LocalizationService.GetResource(CommandsNames.ResourceGroupName + "_" + command);
                    ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { month }, UserExtensions.CurrentUser, month, OperationType.Update, commandName, start);
                }
                if (String.IsNullOrEmpty(message))
                {
                    isSuccess = true;
                    message = Helpers.GlobalResource.DoneMessage;
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                message = ex.Message;
            }
            return Json(new
            {
                Success = isSuccess,
                Msg = message
            });
        }

        #region Generate Month By Filters

        #region Shared

        [HttpPost]
        public ActionResult GenerateFilteredData(Type type, IQueryable<IEntity> allData, int monthId, GridFilter filter = null)
        {
            string message;
            var start = DateTime.Now;
            var month = (Month)typeof(Month).GetById(monthId);

            if (month.MonthStatus == MonthStatus.Approved || month.MonthStatus == MonthStatus.Locked)
            {
                message = ServiceFactory.LocalizationService
                    .GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.CannotGenerateLockedOrApprovedMonths));
                return Json(new
                {
                    Success = false,
                    Msg = message,
                });
            }
            var generalOption = typeof(GeneralOption).GetAll<GeneralOption>().FirstOrDefault();
            if (generalOption == null)
            {
                message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(
                            CustomMessageKeysPayrollSystemModule.NoGeneralOptionInDatabase));
                return Json(new
                {
                    Success = false,
                    Msg = message,
                });
            }

            //var familyBenefitOption = typeof(FamilyBenefitOption).GetAll<FamilyBenefitOption>().FirstOrDefault();
            //if (familyBenefitOption == null)
            //{
            //    message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(
            //                CustomMessageKeysPayrollSystemModule.NoFamilyBenefitOptionInDatabase));
            //    return Json(new
            //    {
            //        Success = false,
            //        Msg = message,
            //    });
            //}

            var queryablePrimaryCards = FilterController.GetRelatedPrimaryCards(type, allData, filter);
            var entities = new List<IAggregateRoot>();
            var totalGeneratedCards = MonthService.GenerateMonth(queryablePrimaryCards, month);

            entities.Add(month);
            var commandName = ServiceFactory.LocalizationService.GetResource(CommandsNames.ResourceGroupName + "_" + CommandsNames.GenerateMonth);
            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, month, OperationType.Update, commandName, start);


            message = ServiceFactory.LocalizationService
                .GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.MonthlyCardGenerated)) + "{" + totalGeneratedCards + "}";

            return Json(new
            {
                Success = true,
                Msg = message,
            });
        }

        #endregion

        [HttpPost]
        public ActionResult GenerateFilteredPrimaryCards(int monthId, GridFilter filter = null)
        {
            return GenerateFilteredData(typeof(EmployeeCard), ServiceFactory.ORMService.AllWithVertualDeleted<EmployeeCard>(), monthId, filter);
        }

        [HttpPost]
        public ActionResult GenerateFilteredEmployees(int monthId, GridFilter filter = null)
        {
            return GenerateFilteredData(typeof(Employee), ServiceFactory.ORMService.AllWithVertualDeleted<Employee>(), monthId, filter);
        }

        [HttpPost]
        public ActionResult GenerateFilteredGrades(int monthId, GridFilter filter = null)
        {
            return GenerateFilteredData(typeof(HRIS.Domain.Grades.RootEntities.Grade), ServiceFactory.ORMService.AllWithVertualDeleted<HRIS.Domain.Grades.RootEntities.Grade>(), monthId, filter);
        }

        [HttpPost]
        public ActionResult GenerateFilteredJobTitles(int monthId, GridFilter filter = null)
        {
            return GenerateFilteredData(typeof(JobTitle), ServiceFactory.ORMService.AllWithVertualDeleted<JobTitle>(), monthId, filter);
        }

        [HttpPost]
        public ActionResult GenerateFilteredJobDescriptions(int monthId, GridFilter filter = null)
        {
            return GenerateFilteredData(typeof(HRIS.Domain.JobDescription.RootEntities.JobDescription), ServiceFactory.ORMService.AllWithVertualDeleted<HRIS.Domain.JobDescription.RootEntities.JobDescription>(), monthId, filter);
        }

        [HttpPost]
        public ActionResult GenerateFilteredPositions(int monthId, GridFilter filter = null)
        {
            return GenerateFilteredData(typeof(Position), ServiceFactory.ORMService.AllWithVertualDeleted<Position>(), monthId, filter);
        }

        [HttpPost]
        public ActionResult GenerateFilteredNodes(int monthId, GridFilter filter = null)
        {
            return GenerateFilteredData(typeof(Node), ServiceFactory.ORMService.AllWithVertualDeleted<Node>(), monthId, filter);
        }

        [HttpPost]
        public ActionResult GenerateFilteredMajorTypes(int monthId, GridFilter filter = null)
        {
            return GenerateFilteredData(typeof(MajorType), ServiceFactory.ORMService.AllWithVertualDeleted<MajorType>(), monthId, filter);
        }

        [HttpPost]
        public ActionResult GenerateFilteredMajors(int monthId, GridFilter filter = null)
        {
            return GenerateFilteredData(typeof(Major), ServiceFactory.ORMService.AllWithVertualDeleted<Major>(), monthId, filter);
        }

        #endregion

        [HttpPost]
        public ActionResult CheckMonthlyCard(int monthCardId)
        {

            MonthlyCard MonthlyCard = ServiceFactory.ORMService.All<MonthlyCard>().Where(m => m.Id == monthCardId).FirstOrDefault();
            string MonthStatus = MonthlyCard.Month.MonthStatus.ToString();

            return Json(new
            {
                monthStatus = MonthStatus
            });
        }


    }
}
