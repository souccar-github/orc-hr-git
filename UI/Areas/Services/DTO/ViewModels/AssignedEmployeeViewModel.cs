using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.Personnel.Entities;

namespace UI.Areas.Services.DTO.ViewModels
{
    public class AssignedEmployeeViewModel
    {
        public AssignedEmployeeViewModel()
        {
            Roles = new List<RoleViewModel>();
            Authorities=new List<AuthorityViewModel>();
            Employee = new EmployeeViewModel();
        }

        public EmployeeViewModel Employee { get; set; }
        public List<RoleViewModel> Roles { get; set; }
        public List<AuthorityViewModel> Authorities { get; set; }
    }
}