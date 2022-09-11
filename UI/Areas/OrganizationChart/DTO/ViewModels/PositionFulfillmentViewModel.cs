using System;
using HRIS.Domain.OrgChart.ValueObjects;

namespace UI.Areas.OrganizationChart.DTO.ViewModels
{
    public class PositionFulfillmentViewModel
    {
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public decimal Weight { get; set; }
        public PositionFulfillmentType Type { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public int EmployeeId { get; set; }
        public string TypeText { get; set; }

    }
}