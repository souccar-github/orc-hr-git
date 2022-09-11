using System;
using HRIS.Domain.PayrollSystem.Enums;

namespace HRIS.Domain.EmployeeRelationServices.DTO
{
    public class EmployeeServiceHistoryDTO
    {
        public int Years { get; set; }
        public int Months { get; set; }
        public int Days { get; set; }
    }
}
