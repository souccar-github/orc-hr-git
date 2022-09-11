using HRIS.Domain.OrgChart.ValueObjects;
using HRIS.Domain.Personnel.Entities;
using UI.Areas.OrganizationChart.DTO.ViewModels;

namespace UI.Areas.OrganizationChart.DTO.Adapters
{
    public static class PositionFulfillmentAdapter
    {
        public static PositionFulfillment Adapte(PositionFulfillmentViewModel positionFulfillmentViewModel)
        {
            var p = new PositionFulfillment
            {
                Id = positionFulfillmentViewModel.Id,
                FromDate = positionFulfillmentViewModel.FromDate,

                Weight = positionFulfillmentViewModel.Weight,
                Type = positionFulfillmentViewModel.Type,
                Employee =
                    new Employee()
                    {
                        Id = positionFulfillmentViewModel.EmployeeId,
                        FirstName = positionFulfillmentViewModel.EmployeeFirstName,
                        LastName = positionFulfillmentViewModel.EmployeeLastName
                    }
            };
            if (positionFulfillmentViewModel.ExpireDate.HasValue)
                p.MakeHistory(positionFulfillmentViewModel.ExpireDate.Value);
            return p;
        }

        public static void Adapte(PositionFulfillmentViewModel positionFulfillmentViewModel, PositionFulfillment positionFulfillment)
        {

            positionFulfillment.Id = positionFulfillmentViewModel.Id;
            //positionFulfillment.FromDate = positionFulfillmentViewModel.FromDate;

            positionFulfillment.Weight = positionFulfillmentViewModel.Weight;
            //positionFulfillment.Type = positionFulfillmentViewModel.Type;
            //positionFulfillment.Employee =
            //    new Employee()
            //        {
            //            Id = positionFulfillmentViewModel.EmployeeId,
            //            FirstName = positionFulfillmentViewModel.EmployeeFirstName,
            //            LastName = positionFulfillmentViewModel.EmployeeLastName
            //        };
           
            //if (positionFulfillmentViewModel.ExpireDate.HasValue)
            //    positionFulfillment.MakeHistory(positionFulfillmentViewModel.ExpireDate.Value);
            
        }

        public static PositionFulfillmentViewModel Adapte(PositionFulfillment positionFulfillment)
        {
            var p = new PositionFulfillmentViewModel()
                       {
                           Id = positionFulfillment.Id,
                           FromDate = positionFulfillment.FromDate,
                           Weight = positionFulfillment.Weight,
                           Type = positionFulfillment.Type,
                           TypeText = positionFulfillment.Type.ToString(),
                           EmployeeId = positionFulfillment.Employee.Id,
                           EmployeeFirstName = positionFulfillment.Employee.FirstName,
                           EmployeeLastName = positionFulfillment.Employee.LastName,
                       };
            if (positionFulfillment.ExpireDate.HasValue)
                p.ExpireDate = positionFulfillment.ExpireDate.Value;
            return p;

        }

    }
}