using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.JobDesc.ValueObjects;
using HRIS.Domain.OrgChart.ValueObjects;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.PositionAgg;
using Service;
using UI.Areas.Services.DTO.ViewModels;

namespace UI.Areas.Services.DTO.Adapters
{
    public static class DelegationAdapter
    {
        public static void UpdatePositionFulfillmentsFromViewModel(DelegationViewModel delegationViewModel, Position originalPosition)
        {

            foreach (var item in delegationViewModel.AssignedEmployees)
            {
                var positionFulfillment = new PositionFulfillment
                                              {
                                                  Employee = new Employee { Id = item.Employee.Id },
                                                  Type = PositionFulfillmentType.Delegated
                                              };
                item.Roles.ForEach(x => positionFulfillment.AddRole(new Role { Id = x.Id }));
                item.Authorities.ForEach(x => positionFulfillment.AddAuthority(new Authority { Id = x.Id }));
                originalPosition.AddPositionFulfillment(positionFulfillment);
            }
        }

        public static IEnumerable<EmployeeViewModel> GetDuplicatedDelegatedEmployees(Position origenalPosition, DelegationViewModel delegationViewModel)
        {
            var duplicatedDelegatedEmployees = delegationViewModel.AssignedEmployees.Where(x => origenalPosition.PositionFulfillments.Any(y => y.Employee.Id == x.Employee.Id))
                                                            .Select(z => z.Employee);

            return duplicatedDelegatedEmployees;
        }
    }
}