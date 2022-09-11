#region

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Entities;
using Infrastructure.Validation;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.Indexes;
using Service;
using StructureMap;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.Infrastructure;
using UI.Areas.Personnel.Controllers.EntitiesRoots;
using UI.Areas.Personnel.Helpers;
using UI.Areas.Services.DTO.ViewModels;
using UI.Helpers.Cache;
using UI.Helpers.Controllers;
using UI.Helpers.Security;
using Telerik.Web.Mvc.UI;
using Telerik.Web.Mvc.Extensions;

#endregion

namespace UI.Areas.Services.Controllers.EntitiesRoots
{
    public  class EmployeeOnlineController : EmployeeAggregateController
    {

        public ActionResult Index()
        {
            GetNationalties();

            return View("Index");
        }
        //[GridAction]
        public ActionResult ReadEmployees()
        {
            return this.Json(refreshGridModel());
        }

        [HttpPost]
       // [GridAction]
        public ActionResult UpdateEmployee(string id)
        {
           
            
            var employee = Service.GetById(int.Parse(id));

           
            if (TryUpdateModel(employee))
            {
                employee.Nationality = new Nationality { Id = this.ValueOf<int>("Nationalties") };
                Service.Update(employee);

            }


            return this.Json(refreshGridModel());

        }

        [HttpPost]
       // [GridAction]
        public ActionResult DeleteEmployee(string id)
        {
            var employee = Service.GetById(int.Parse(id));

            if (TryUpdateModel(employee))
            {
                Service.Delete(employee);

            }

            return this.Json(refreshGridModel());
        }
       

        public IList<Employee> GetEmployeesList(IList<Employee> employees)
        {
            return employees.Select(a => new Employee
            {
                Id = a.Id,
                LastName = a.LastName,
                FirstName = a.FirstName,
                MiddleName = a.MiddleName,
                FatherName = a.FatherName,
                MotherName = a.MotherName,
                DateOfBirth = a.DateOfBirth,
                Nationality = new Nationality() { Id = a.Nationality.Id, Name = a.Nationality.Name }
            }).ToList();
        }

        private void GetNationalties()
        {
            ViewData["nationalties"] = CacheProvider.Get(PersonnelCacheKeys.Nationality.ToString(),
                                                                    () => new EntityService<Nationality>().GetList());
        }
        private GridModel refreshGridModel()
        {
            var currentPage = this.ValueOf<int>(GridUrlParameters.CurrentPage);
            var pageSize = this.ValueOf<int>(GridUrlParameters.PageSize);
            var orderBy = this.ValueOf<string>(GridUrlParameters.OrderBy);
            var filter = this.ValueOf<string>(GridUrlParameters.Filter);
            var query = Service.GetAll();
            var model = query.ToGridModel(currentPage, pageSize, orderBy, null, filter);
            model.Data = GetEmployeesList((IList<Employee>)model.Data.AsQueryable().ToIList());
            return model;
        }
    }
}