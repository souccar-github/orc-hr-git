using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infrastructure.Localization;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Services;

namespace UI.Areas.Services.DTO.ViewModels
{
    public class DelegationViewModel
    {
        public DelegationViewModel()
        {
            Roles = new List<RoleViewModel>();
            Authorities = new List<AuthorityViewModel>();
            AssignedEmployees = new List<AssignedEmployeeViewModel>();
            Delegation = new Delegation();
        }

        public Delegation Delegation { get; set; }
        public List<RoleViewModel> Roles { get; set; }
        public List<AuthorityViewModel> Authorities { get; set; }
        public List<AssignedEmployeeViewModel> AssignedEmployees { get; set; }

        [LocalizationDisplayName("EmployeeId", typeof(Resources.Areas.Services.Delegation.DTO.DelegationViewModel))]
        public int EmployeeId { get; set; }

        [LocalizationDisplayName("PositionId", typeof(Resources.Areas.Services.Delegation.DTO.DelegationViewModel))]
        public int PositionId { get; set; }
    }
}