using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.JobDescription.Entities;

using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Validation.MessageKeys;
using  Project.Web.Mvc4.Controllers;
using  Project.Web.Mvc4.Extensions;
using  Project.Web.Mvc4.Factories;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.PayrollSystem.Entities;


namespace Project.Web.Mvc4.Areas.PayrollSystem.Controllers
{
    public class FilterController : Controller
    {
        #region Shared

        public GridViewModel GetTypeGridModel(Type type, string readDataUrl)
        {
            var gridModel = GridViewModelFactory.Create(type, null);
            gridModel.Views[0].ReadUrl = readDataUrl;
            gridModel.ToolbarCommands = new List<ToolbarCommand>
            {
                new ToolbarCommand
                {
                    Additional = false,
                    Name = "GenerateButton",
                    ClassName = "grid-action-button GenerateButtonKey",
                    Handler = "",
                    ImageClass = "",
                    Text =
                        ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.GenerateCards))
                }
            };
            return gridModel; //Json(gridModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadTypeData(Type type, IQueryable<IEntity> allData, int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null)
        {
            //todo Mhd Alsadi: لا داعي للميثودات التي من خلالها يتم قراءة الداتا وتمريرها لهذه الميثود لان هذه الميثود قادرة على قراءة الداتا
            //var test = CrudController.GetAll(type);
            CrudController.UpdateFilter(filter, type);

            var dataSourse = DataSourceResult.GetDataSourceResult(allData, type, pageSize, skip, serverPaging, sort, filter);
            var data = type.ToDynamicData(dataSourse.Data);
            return Json(new { Data = data, TotalCount = dataSourse.Total });
        }

        #endregion

        #region FilterBy Primary Card

        [HttpPost]
        public ActionResult GetPrimaryCardGridModel()
        {
            var gridModel = GetTypeGridModel(typeof(EmployeeCard), "PayrollSystem/Filter/ReadPrimaryCardData");
            var displayColumnsList = new List<string> { "Employee","FatherName", "SalaryDeservableType", "StartWorkingDate", "ContractType", "EmployeeType", "Salary", "BenefitSalary", "TempSalary1", "TempSalary2" };
            gridModel.Views[0].Columns = gridModel.Views[0].Columns.Where(x => displayColumnsList.Contains(x.FieldName)).ToList();
            return Json(gridModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadPrimaryCardData(int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null, IEnumerable<GridGroup> group = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null)
        {
            IQueryable<IEntity> queryable = ServiceFactory.ORMService.AllWithVertualDeleted<EmployeeCard>(); //ServiceFactory.ORMService.All<EmployeeCard>().Where(x => x.SalaryDeservableType != SalaryDeservableType.Nothing);
            return ReadTypeData(typeof(EmployeeCard), queryable, pageSize, skip, serverPaging, sort, filter);
        }

        #endregion

        #region FilterBy Employee

        [HttpPost]
        public ActionResult GetEmployeeGridModel()
        {
            var gridModel = GetTypeGridModel(typeof(Employee), "PayrollSystem/Filter/ReadEmployeeData");
            //gridModel.Views[0].Columns.Remove(gridModel.Views[0].Columns.First(x => x.FieldName == "PhotoPath"));
            return Json(gridModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadEmployeeData(int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null, IEnumerable<GridGroup> group = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null)
        {
            IQueryable<IEntity> queryable = ServiceFactory.ORMService.AllWithVertualDeleted<Employee>();//ServiceFactory.ORMService.All<Employee>();
            return ReadTypeData(typeof(Employee), queryable, pageSize, skip, serverPaging, sort, filter);
        }

        #endregion

        #region FilterBy Grade

        [HttpPost]
        public ActionResult GetGradeGridModel()
        {
            var gridModel = GetTypeGridModel(typeof(HRIS.Domain.Grades.RootEntities.Grade), "PayrollSystem/Filter/ReadGradeData");
            return Json(gridModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadGradeData(int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null, IEnumerable<GridGroup> group = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null)
        {
            IQueryable<IEntity> queryable = ServiceFactory.ORMService.AllWithVertualDeleted<HRIS.Domain.Grades.RootEntities.Grade>(); //ServiceFactory.ORMService.All<Domain.OrganizationChart.RootEntities.Grade>();
            return ReadTypeData(typeof(HRIS.Domain.Grades.RootEntities.Grade), queryable, pageSize, skip, serverPaging, sort, filter);
        }

        #endregion

        #region FilterBy JobTitle

        [HttpPost]
        public ActionResult GetJobTitleGridModel()
        {
            var gridModel = GetTypeGridModel(typeof(JobTitle), "PayrollSystem/Filter/ReadJobTitleData");
            return Json(gridModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadJobTitleData(int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null, IEnumerable<GridGroup> group = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null)
        {
            IQueryable<IEntity> queryable = ServiceFactory.ORMService.AllWithVertualDeleted<JobTitle>();
            return ReadTypeData(typeof(JobTitle), queryable, pageSize, skip, serverPaging, sort, filter);
        }

        #endregion

        #region FilterBy JobDescription

        [HttpPost]
        public ActionResult GetJobDescriptionGridModel()
        {
            var gridModel = GetTypeGridModel(typeof(HRIS.Domain.JobDescription.RootEntities.JobDescription), "PayrollSystem/Filter/ReadJobDescriptionData");
            return Json(gridModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadJobDescriptionData(int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null, IEnumerable<GridGroup> group = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null)
        {
            IQueryable<IEntity> queryable = ServiceFactory.ORMService.AllWithVertualDeleted<HRIS.Domain.JobDescription.RootEntities.JobDescription>();
            return ReadTypeData(typeof(HRIS.Domain.JobDescription.RootEntities.JobDescription), queryable, pageSize, skip, serverPaging, sort, filter);
        }

        #endregion

        #region FilterBy Position

        [HttpPost]
        public ActionResult GetPositionGridModel()
        {
            var gridModel = GetTypeGridModel(typeof(Position), "PayrollSystem/Filter/ReadPositionData");
            return Json(gridModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadPositionData(int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null, IEnumerable<GridGroup> group = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null)
        {
            IQueryable<IEntity> queryable = ServiceFactory.ORMService.AllWithVertualDeleted<Position>();
            return ReadTypeData(typeof(Position), queryable, pageSize, skip, serverPaging, sort, filter);
        }

        #endregion

        #region FilterBy Node

        [HttpPost]
        public ActionResult GetNodeGridModel()
        {
            var gridModel = GetTypeGridModel(typeof(Node), "PayrollSystem/Filter/ReadNodeData");
            return Json(gridModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadNodeData(int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null, IEnumerable<GridGroup> group = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null)
        {
            IQueryable<IEntity> queryable = ServiceFactory.ORMService.AllWithVertualDeleted<Node>();
            return ReadTypeData(typeof(Node), queryable, pageSize, skip, serverPaging, sort, filter);
        }

        #endregion

        #region FilterBy MajorType

        [HttpPost]
        public ActionResult GetMajorTypeGridModel()
        {
            var gridModel = GetTypeGridModel(typeof(MajorType), "PayrollSystem/Filter/ReadMajorTypeData");
            return Json(gridModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadMajorTypeData(int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null, IEnumerable<GridGroup> group = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null)
        {
            IQueryable<IEntity> queryable = ServiceFactory.ORMService.AllWithVertualDeleted<MajorType>();
            return ReadTypeData(typeof(MajorType), queryable, pageSize, skip, serverPaging, sort, filter);
        }

        #endregion

        #region FilterBy Major

        [HttpPost]
        public ActionResult GetMajorGridModel()
        {
            var gridModel = GetTypeGridModel(typeof(Major), "PayrollSystem/Filter/ReadMajorData");
            return Json(gridModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadMajorData(int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null, IEnumerable<GridGroup> group = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null)
        {
            IQueryable<IEntity> queryable = ServiceFactory.ORMService.AllWithVertualDeleted<Major>();
            return ReadTypeData(typeof(Major), queryable, pageSize, skip, serverPaging, sort, filter);
        }

        #endregion


        public static IQueryable<EmployeeCard> GetRelatedPrimaryCards(Type type, IQueryable<IEntity> allData, GridFilter filter = null)
        {
            IQueryable<EmployeeCard> queryablePrimaryCards;
            CrudController.UpdateFilter(filter, type);
            var filteredData = DataSourceResult.GetDataSourceResult(allData, type, 10, 0, false, null, filter);

            if (type == typeof(EmployeeCard))
            {
                queryablePrimaryCards = (IQueryable<EmployeeCard>)filteredData.Data;
            }
            else if (type == typeof(Employee))
            {
                var employees = (IQueryable<Employee>)filteredData.Data;
                var employeeCards = typeof(EmployeeCard).GetAll<EmployeeCard>().ToList();
                var empCards = new List<EmployeeCard>();
                foreach (Employee emp in employees)
                {
                    var empCard = employeeCards.SingleOrDefault(x => x.Employee.Id == emp.Id);
                    if (empCard!= null)
                    {
                        empCards.Add(empCard);
                    }
                }
              
                queryablePrimaryCards = (IQueryable<EmployeeCard>)empCards.AsQueryable<EmployeeCard>(); ;
            }
            else if (type == typeof(HRIS.Domain.Grades.RootEntities.Grade))
            {
                var grades = (IQueryable<HRIS.Domain.Grades.RootEntities.Grade>)filteredData.Data;
                //todo Mhd Alsaadi: ايجاد حل للاكسبشن - تم التحويل الى ليست لكي لا يظهر الخطا الناتج عن عدم دعم انهايبرنيت للكومبلكس كويري 
                queryablePrimaryCards =
                    typeof(EmployeeCard).GetAll<EmployeeCard>().Where(x => x.Employee.Positions.SingleOrDefault(y => y.IsPrimary) != null)
                    .ToList()//todo Mhd Alsaadi: ايجاد حل للاكسبشن - تم التحويل الى ليست لكي لا يظهر الخطا الناتج عن عدم دعم انهايبرنيت للكومبلكس كويري 
                    .Where(x => grades.ToList().Any(y => y.Id == x.Employee.Positions.First(q => q.IsPrimary).Position.JobDescription.JobTitle.Grade.Id))
                    .AsQueryable();
            }
            else if (type == typeof(JobTitle))
            {
                var jobTitles = (IQueryable<JobTitle>)filteredData.Data;

                queryablePrimaryCards =
                    typeof(EmployeeCard).GetAll<EmployeeCard>().Where(x => x.Employee.Positions.SingleOrDefault(y => y.IsPrimary) != null)
                    .ToList()//todo Mhd Alsaadi: ايجاد حل للاكسبشن - تم التحويل الى ليست لكي لا يظهر الخطا الناتج عن عدم دعم انهايبرنيت للكومبلكس كويري 
                    .Where(x => jobTitles.ToList().Any(y => y.Id == x.Employee.Positions.First(q => q.IsPrimary).Position.JobDescription.JobTitle.Id))
                    .AsQueryable();
            }
            else if (type == typeof(HRIS.Domain.JobDescription.RootEntities.JobDescription))
            {
                var jobDescriptions = (IQueryable<HRIS.Domain.JobDescription.RootEntities.JobDescription>)filteredData.Data;

                queryablePrimaryCards =
                    typeof(EmployeeCard).GetAll<EmployeeCard>().Where(x => x.Employee.Positions.SingleOrDefault(y => y.IsPrimary) != null)
                    .ToList()//todo Mhd Alsaadi: ايجاد حل للاكسبشن - تم التحويل الى ليست لكي لا يظهر الخطا الناتج عن عدم دعم انهايبرنيت للكومبلكس كويري 
                    .Where(x => jobDescriptions.ToList().Any(y => y.Id == x.Employee.Positions.First(q => q.IsPrimary).Position.JobDescription.Id))
                    .AsQueryable();
            }
            else if (type == typeof(Position))
            {
                var positions = (IQueryable<Position>)filteredData.Data;

                queryablePrimaryCards =
                    typeof(EmployeeCard).GetAll<EmployeeCard>().Where(x => x.Employee.Positions.SingleOrDefault(y => y.IsPrimary) != null)
                    .ToList()//todo Mhd Alsaadi: ايجاد حل للاكسبشن - تم التحويل الى ليست لكي لا يظهر الخطا الناتج عن عدم دعم انهايبرنيت للكومبلكس كويري 
                    .Where(x => positions.ToList().Any(y => y.Id == x.Employee.Positions.First(q => q.IsPrimary).Position.Id))
                    .AsQueryable();
            }
            else if (type == typeof(Node))
            {
                var nodes = (IQueryable<Node>)filteredData.Data;

                queryablePrimaryCards =
                    typeof(EmployeeCard).GetAll<EmployeeCard>().Where(x => x.Employee.Positions.SingleOrDefault(y => y.IsPrimary) != null)
                    .ToList()//todo Mhd Alsaadi: ايجاد حل للاكسبشن - تم التحويل الى ليست لكي لا يظهر الخطا الناتج عن عدم دعم انهايبرنيت للكومبلكس كويري 
                    .Where(x => nodes.ToList().Any(y => y.Id == x.Employee.Positions.First(q => q.IsPrimary).Position.JobDescription.Node.Id))
                    .AsQueryable();
            }
            else if (type == typeof(MajorType))
            {
                var majorTypes = (IQueryable<MajorType>)filteredData.Data;
                // todo Mhd Alsadi: GradeEducation سيتم مناقشة الشرط التالي ولكن بعد الانتهاء بشكل كامل من

                queryablePrimaryCards =
                    typeof(EmployeeCard).GetAll<EmployeeCard>().Where(x => x.Employee.Positions.SingleOrDefault(y => y.IsPrimary) != null)
                    .ToList()//todo Mhd Alsaadi: ايجاد حل للاكسبشن - تم التحويل الى ليست لكي لا يظهر الخطا الناتج عن عدم دعم انهايبرنيت للكومبلكس كويري 
                    .Where(x => majorTypes.ToList().Any(y => x.Employee.Positions.First(q => q.IsPrimary).Position.JobDescription.JobTitle.Grade
                        .GradeByEducation.GradeByEducationQualifications.Any(v => v.MajorType.Id == y.Id)))
                    .AsQueryable();
            }
            else if (type == typeof(Major))
            {
                var majors = (IQueryable<Major>)filteredData.Data;
                // todo Mhd Alsadi: GradeEducation سيتم مناقشة الشرط التالي ولكن بعد الانتهاء بشكل كامل من

                queryablePrimaryCards =
                    typeof(EmployeeCard).GetAll<EmployeeCard>().Where(x => x.Employee.Positions.SingleOrDefault(y => y.IsPrimary) != null)
                    .ToList()//todo Mhd Alsaadi: ايجاد حل للاكسبشن - تم التحويل الى ليست لكي لا يظهر الخطا الناتج عن عدم دعم انهايبرنيت للكومبلكس كويري 
                    .Where(x => majors.ToList().Any(y => x.Employee.Positions.First(q => q.IsPrimary).Position.JobDescription.JobTitle.Grade
                        .GradeByEducation.GradeByEducationQualifications.Any(v => v.Major.Id == y.Id)))
                    .AsQueryable();
            }
            else
            {
                throw new Exception("This type is not supported");
            }

            return queryablePrimaryCards;
        }
        #region Get the PrimaryCard wih the same benefit and the same duction that is update it
        public static IQueryable<EmployeeCard> GetRelatedPrimaryCardsWithBenefit(IQueryable<EmployeeCard> queryablePrimaryCards, BenefitCard benefitCard)
        {
            var beforeQueryablePrimaryCard = queryablePrimaryCards.Count();

            queryablePrimaryCards.ToList();

            bool addOneQurb = false;

            List<EmployeeCard> listOfQueryablePrimaryCardsWithBenefit = new List<EmployeeCard>();

            foreach (var queryablePrimaryCard in queryablePrimaryCards)
            {
                var queryablePrimaryCardBenefitCards = queryablePrimaryCard.PrimaryEmployeeBenefits.ToList();
                foreach (var queryablePrimaryCardBenefitCard in queryablePrimaryCardBenefitCards)
                {
                    if (queryablePrimaryCardBenefitCard.BenefitCard.Id == benefitCard.Id & addOneQurb == false)
                    {
                        listOfQueryablePrimaryCardsWithBenefit.Add(queryablePrimaryCard);
                        addOneQurb = true;
                    }
                }
                addOneQurb = false;
            }
            queryablePrimaryCards = listOfQueryablePrimaryCardsWithBenefit.AsQueryable();
            var afterQueryablePrimaryCards = listOfQueryablePrimaryCardsWithBenefit.Count(); ;
            return queryablePrimaryCards;
        }


        public static IQueryable<EmployeeCard> GetRelatedPrimaryCardsWithdeDuction(IQueryable<EmployeeCard> queryablePrimaryCards, DeductionCard deductionCard)
        {
            var beforeQueryablePrimaryCard = queryablePrimaryCards.Count();

            queryablePrimaryCards.ToList();

            bool addOneQurb = false;

            List<EmployeeCard> listOfQueryablePrimaryCardsWithDeductions = new List<EmployeeCard>();

            foreach (var queryablePrimaryCard in queryablePrimaryCards)
            {
                var queryablePrimaryCardDeductionCards = queryablePrimaryCard.PrimaryEmployeeDeductions.ToList();
                foreach (var queryablePrimaryCardDeductionCard in queryablePrimaryCardDeductionCards)
                {
                    if (queryablePrimaryCardDeductionCard.DeductionCard.Id == deductionCard.Id & addOneQurb == false)
                    {
                        listOfQueryablePrimaryCardsWithDeductions.Add(queryablePrimaryCard);
                        addOneQurb = true;
                    }
                }
                addOneQurb = false;
            }
            queryablePrimaryCards = listOfQueryablePrimaryCardsWithDeductions.AsQueryable();
            var afterQueryablePrimaryCards = listOfQueryablePrimaryCardsWithDeductions.Count(); ;
            return queryablePrimaryCards;
        }

        #endregion
    }
}
