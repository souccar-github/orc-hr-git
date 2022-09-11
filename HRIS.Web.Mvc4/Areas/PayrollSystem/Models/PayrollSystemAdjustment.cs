using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Recruitment.RootEntities;
using HRIS.Validation.MessageKeys;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Models.Navigation;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Areas.PayrollSystem.EventHandlers;
using Project.Web.Mvc4.Areas.Personnel.Models;

namespace Project.Web.Mvc4.Areas.PayrollSystem.Models
{
    public class PayrollSystemAdjustment: ModelAdjustment
    {
        private static Dictionary<string, ViewModel> parent = new Dictionary<string, ViewModel>();
        public override void AdjustModule(Module module)
        {
            //var employeeAggregate = module.Aggregates.Single(x => x.TypeFullName == typeof(Employee).FullName);

       
        
            module.Aggregates = module.Aggregates.Where(x => x.TypeFullName != typeof(Applicant).FullName).ToList();

            var monthAggregate = module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(Month).FullName));
            if (monthAggregate != null)
            {
                var detail = monthAggregate.Details.SingleOrDefault(x => x.TypeFullName == typeof(MonthlyCard).FullName);
                if (detail != null)
                {
                    detail.Details = DetailFactory.Create(typeof(MonthlyCard));
                  
                }
              
               
            }
            var primaryCardAggregate = module.Aggregates.SingleOrDefault(x => x.TypeFullName == typeof(EmployeeCard).FullName);
            if (primaryCardAggregate != null)
            {
                var detail = primaryCardAggregate.Details.SingleOrDefault(x => x.TypeFullName == typeof(EmployeeLoan).FullName);
                if (detail != null)
                {
                    detail.Details = DetailFactory.Create(typeof(EmployeeLoan));
                }
            }


            var employeeAggregate = module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(Employee).FullName));
            if (employeeAggregate != null)
            {
                var detailChild = employeeAggregate.Details.Single(x => x.TypeFullName == typeof(Child).FullName);
                var detailSpouse = employeeAggregate.Details.Single(x => x.TypeFullName == typeof(Spouse).FullName);
                employeeAggregate.Order = 25;
                employeeAggregate.Title = ServiceFactory.LocalizationService
                    .GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.EmployeeFamilyDeserve));

                employeeAggregate.Details = new List<Detail>
                {
                    detailChild, 
                    detailSpouse
                };
            }

            var BenefitCard=module.Configurations.SingleOrDefault(x => x.TypeFullName == (typeof(BenefitCard).FullName));
            if (BenefitCard != null)
            {
                var detail = BenefitCard.Details.SingleOrDefault(x => x.TypeFullName == (typeof(CrossDeductionWithBenefit).FullName));
                if (detail != null)

                    detail.Details = DetailFactory.Create(typeof(CrossDeductionWithBenefit)).ToList();
            }

            //var CrossDeductionWithBenefitDetails = DetailFactory.Create(typeof(CrossDeductionWithBenefit));
            //module.Configurations.SingleOrDefault(x => x.TypeFullName == typeof(BenefitCard).FullName).Details
            //    .SingleOrDefault(x => x.TypeFullName == typeof(CrossDeductionWithBenefit).FullName).Details = CrossDeductionWithBenefitDetails.ToList();

            module.Services.Add(new Service
            {
                Controller = "PayrollSystem/BenefitDeductionService",
                Action = "AddBenefitToEmployees",
                Title = ServiceFactory.LocalizationService
                    .GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.AddBenefitToEmployees)),
                ServiceId = "AddBenefitToEmployees",
                SecurityId = "AddBenefitToEmployees"
            });

            module.Services.Add(new Service
            {
                Controller = "PayrollSystem/BenefitDeductionService",
                Action = "AddDeductionToEmployees",
                Title = ServiceFactory.LocalizationService
                    .GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.AddDeductionToEmployees)),
                ServiceId = "AddDeductionToEmployees",
                SecurityId = "AddDeductionToEmployees"
            });

            module.Services.Add(new Service
            {
                Controller = "PayrollSystem/CopySalariesService",
                Action = "CopySalaries",
                Title = ServiceFactory.LocalizationService
                    .GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.CopySalariesServiceTitle)),
                ServiceId = "CopySalariesService",
                SecurityId = "CopySalariesService"
            });
        }

        public override ViewModel AdjustGridModel(string type)
        {
            if (parent.Count == 0)
            {
                parent.Add("BenefitCard", new BenefitCardEventHandlers());
                parent.Add("FinancialCard", new FinancialCardEventHandler());
                parent.Add("CrossDeductionWithBenefit", new CrossDeductionWithBenefitEventHandlers());
                parent.Add("CrossDeductionWithDeduction", new CrossDeductionWithDeductionEventHandlers());
                parent.Add("DeductionCard", new DeductionCardEventHandlers());
                parent.Add("GeneralOption", new GeneralOptionEventHandlers());
                parent.Add("LoanPayment", new LoanPaymentEventHandlers());
                parent.Add("Month", new MonthEventHandlers());
                parent.Add("MonthlyEmployeeBenefit", new MonthlyEmployeeBenefitEventHandlers());
                parent.Add("MonthlyEmployeeDeduction", new MonthlyEmployeeDeductionEventHandlers());
                parent.Add("SalaryIncreaseOrdinanceEmployee", new SalaryIncreaseOrdinanceEmployeeEventHandlers());
                parent.Add("SalaryIncreaseOrdinance", new SalaryIncreaseOrdinanceEventHandlers());
                parent.Add("TaxSlice", new TaxSliceEventHandlers());
                parent.Add("MonthlyCard", new MonthlyCardEventHandlers());
                parent.Add("TemporaryWorkshop", new TemporaryWorkshopViewModel());
                parent.Add("PrimaryEmployeeBenefit", new PrimaryEmployeeBenefitViewModel());
                parent.Add("PrimaryEmployeeDeduction", new PrimaryEmployeeDeductionViewModel());
                parent.Add("EmployeeLoan", new EmployeeLoanViewModel());
                parent.Add("EmployeeAdvance", new EmployeeAdvanceViewModel());
                parent.Add("BankingInformation", new BankingInformationEventHandlers());
                parent.Add("MonthlyEmployeeAdvance", new MonthlyEmployeeAdvanceViewModel());
            }
            try
            {
                return parent[type];
            }
            catch
            {

                return new PayrollSystemViewModel();
            }
 

        }
    }
}