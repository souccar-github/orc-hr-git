using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using Project.Web.Mvc4.Controllers;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Resources.Shared.Messages;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using Project.Web.Mvc4.Helpers.Resource;
using Souccar.Domain.Audit;
using Project.Web.Mvc4.Helpers.DomainExtensions;

namespace Project.Web.Mvc4.Areas.PayrollSystem.Controllers
{
    public class CopySalariesServiceController : Controller
    {
        public ActionResult CopySalaries()
        {
            return PartialView("../Service/CopySalariesService");
        }

        [HttpPost]
        public ActionResult GetPrimaryCardGridModel()
        {
            var gridModel = GridViewModelFactory.Create(typeof(EmployeeCard), null);
            gridModel.Views[0].ReadUrl = "PayrollSystem/CopySalariesService/ReadPrimaryCardData";
            gridModel.ToolbarCommands = new List<ToolbarCommand>
            {
                new ToolbarCommand
                {
                    Additional = false,
                    Name = "CopySalariesService",
                    ClassName = "grid-action-button CopySalaries",
                    Handler = "ApplyCopySalaries",
                    ImageClass = "",
                    Text = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule
                    .GetFullKey(CustomMessageKeysPayrollSystemModule.CopySalariesServiceGridButtonTitle))
                }
            };
            var displayColumnsList = new List<string> { "Employee","FatherName", "SalaryDeservableType", "StartWorkingDate", "ContractType", "EmployeeType", "Salary", 
                "BenefitSalary", "TempSalary1", "TempSalary2" };
            gridModel.Views[0].Columns = gridModel.Views[0].Columns.Where(x => displayColumnsList.Contains(x.FieldName)).ToList();
            return Json(gridModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadPrimaryCardData(int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null, IEnumerable<GridGroup> group = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null)
        {
            var entityType = typeof(EmployeeCard);
            CrudController.UpdateFilter(filter, entityType);
            IQueryable<IEntity> queryable = ServiceFactory.ORMService.AllWithVertualDeleted<EmployeeCard>();
            var dataSourse = DataSourceResult.GetDataSourceResult(queryable, entityType, pageSize, skip, serverPaging, sort, filter);
            var data = entityType.ToDynamicData(dataSourse.Data);
            return Json(new { Data = data, TotalCount = dataSourse.Total });
        }

        [HttpPost]
        public ActionResult ApplyCopySalaries(
            Salaries fromSalary,
            Salaries toSalary,
            GridFilter filter = null)
        {
            var entityType = typeof(EmployeeCard);
            var entities = new List<IAggregateRoot>();
            CrudController.UpdateFilter(filter, entityType);
            IQueryable<IEntity> queryable = ServiceFactory.ORMService.AllWithVertualDeleted<EmployeeCard>();
            var filteredPrimaryCards = (IEnumerable<EmployeeCard>)(DataSourceResult.GetDataSourceResult(queryable, entityType, 10, 0, false, null, filter).Data);


            foreach (var filteredPrimaryCard in filteredPrimaryCards)
            {
                float fromSalaryValue = 0;
                switch (fromSalary)
                {
                    case Salaries.PrimarySalary:
                        fromSalaryValue = filteredPrimaryCard.FinancialCard.Salary;
                        break;
                    case Salaries.InsuranceSalary:
                        fromSalaryValue = filteredPrimaryCard.FinancialCard.InsuranceSalary;
                        break;
                    case Salaries.BenefitSalary:
                        fromSalaryValue = filteredPrimaryCard.FinancialCard.BenefitSalary;
                        break;
                    case Salaries.TempSalary1:
                        fromSalaryValue = filteredPrimaryCard.FinancialCard.TempSalary1;
                        break;
                    case Salaries.TempSalary2:
                        fromSalaryValue = filteredPrimaryCard.FinancialCard.TempSalary2;
                        break;
                }

                switch (toSalary)
                {
                    case Salaries.PrimarySalary:
                        filteredPrimaryCard.FinancialCard.Salary = fromSalaryValue;
                        break;
                    case Salaries.InsuranceSalary:
                        filteredPrimaryCard.FinancialCard.InsuranceSalary = fromSalaryValue;
                        break;
                    case Salaries.BenefitSalary:
                        filteredPrimaryCard.FinancialCard.BenefitSalary = fromSalaryValue;
                        break;
                    case Salaries.TempSalary1:
                        filteredPrimaryCard.FinancialCard.TempSalary1 = fromSalaryValue;
                        break;
                    case Salaries.TempSalary2:
                        filteredPrimaryCard.FinancialCard.TempSalary2 = fromSalaryValue;
                        break;
                }
                entities.Add(filteredPrimaryCard);
            }

            var message = Helpers.GlobalResource.DoneMessage;
            var info = PayrollSystemLocalizationHelper.GetResource(PayrollSystemLocalizationHelper.ApplyCopySalaries);
            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, null, OperationType.Update, info);

            return Json(new
            {
                Success = true,
                Msg = message,
            });
        }



    }
}
