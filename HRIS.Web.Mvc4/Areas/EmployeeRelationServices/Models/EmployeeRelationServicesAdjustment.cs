using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Recruitment.RootEntities;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Models.Navigation;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Global.Constant;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Models;
using Project.Web.Mvc4.Helpers;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class EmployeeRelationServicesAdjustment: ModelAdjustment
    {
        private static Dictionary<string, ViewModel> parent = new Dictionary<string, ViewModel>();
        public override ViewModel AdjustGridModel(string type)
        {
            if (parent.Count == 0)
            {
                parent.Add("ChangeableHoliday", new ChangeableHolidayViewModel());
                parent.Add("AssigningEmployeeToPosition", new AssigningEmployeeToPositionViewModel());
                parent.Add("Assignment", new AssignmentViewModel());
                parent.Add("EmployeeDisciplinary", new EmployeeDisciplinaryViewModel());
                parent.Add("EmployeeCard", new EmployeeCardViewModel());
                parent.Add("EmployeePromotion", new EmployeePromotionViewModel());
                parent.Add("EmployeeResignation", new EmployeeResignationViewModel());
                parent.Add("EmployeeReward", new EmployeeRewardViewModel());
                parent.Add("EmployeeTermination", new EmployeeTerminationViewModel());
                parent.Add("EmployeeTransfer", new EmployeeTransferViewModel());
                parent.Add("Employee", new EmployeeViewModel());
                parent.Add("ExitInterview", new ExitInterviewViewModel());
               // parent.Add("EndWorkflow", new EndWorkflowviewViewModel());
                parent.Add("ExitInterviewAnswer", new ExitInterviewAnswerViewModel());
                parent.Add("FinancialPromotion", new FinancialPromotionViewModel());
                parent.Add("InterviewAnswers", new InterviewAnswersViewModel());
                parent.Add("LeaveRequest", new LeaveRequestViewModel());
                parent.Add("InfractionForm", new InfractionFormEventHandlers());
                parent.Add("InfractionSlice", new InfractionSliceViewModel());
                parent.Add("LeaveSetting", new LeaveSettingViewModel());
                parent.Add("LeaveTemplateDetail", new LeaveTemplateDetailViewModel());
                parent.Add("LeaveTemplateMaster", new LeaveTemplateMasterViewModel());
                parent.Add("PublicHoliday", new PublicHolidayViewModel());
                parent.Add("Recycle", new RecycleViewModel());
                parent.Add("RecycledLeave", new RecycledLeaveViewModel());
                parent.Add("RewardSetting", new RewardSettingViewModel());
                parent.Add("BalanceSlice", new BalanceSlicesViewModel());
                parent.Add("PaidSlice", new PaidSlicesViewModel());
                parent.Add("ContractorLeaveSetting", new ContractorLeaveSettingViewModel());
                parent.Add("FixedHoliday", new FixedHolidayViewModel());
                parent.Add("DisciplinarySetting", new DisciplinarySettingViewModel());
                parent.Add("EndingSecondaryPositionEmployee", new EndingSecondaryPositionEmployeeViewModel());
                parent.Add("GeneralEmployeeRelationSetting", new GeneralEmployeeRelationSettingViewModel());
                parent.Add("EmployeeTransferRequest", new EmployeeTransferRequestViewModel());
                parent.Add("EmployeeLoanRequest", new EmployeeLoanRequestViewModel());
                parent.Add("EmployeeInfraction", new EmployeeInfractionViewModel());
            }
            try
            {
                return parent[type];
            }
            catch
            {
                return new ViewModel();
            }
        }

        public override void AdjustModule(Module module)
        {

            var aggregate = module.Aggregates.SingleOrDefault(x => x.TypeFullName == typeof(EmployeeCard).FullName);
            var details = aggregate.Details.SingleOrDefault(x => x.DetailId == "ExitInterviews");
            details.Details = DetailFactory.Create(typeof(ExitInterview));
            aggregate.Details.SingleOrDefault(x => x.TypeFullName == (typeof(EmployeeResignation).FullName))
               .Details = DetailFactory.Create(typeof(EmployeeResignation));

            aggregate.Details.SingleOrDefault(x => x.TypeFullName == (typeof(LeaveRequest).FullName))
               .Details = DetailFactory.Create(typeof(LeaveRequest));
            module.Services.Add(new Service()
            {
                Controller = "EmployeeRelationServices/Service",
                Action = "EmployeeLeaveRequest",
                Title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeLeaveRequest),
                ServiceId = "EmployeeLeaveRequest",
                SecurityId = "EmployeeLeaveRequest"
            });
            
            module.Services.Add(new Service()
            {
                Controller = "EmployeeRelationServices/Service",
                Action = "MissionRequest",
                Title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionRequest),
                ServiceId = "MissionRequest",
                SecurityId = "MissionRequest"
            });
            module.Services.Add(new Service()
            {
                Controller = "EmployeeRelationServices/Service",
                Action = "EntranceExitRequest",
                Title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EntranceExitRequest),
                ServiceId = "EntranceExitRequest",
                SecurityId = "EntranceExitRequest"
            });
            module.Services.Add(new Service()
            {
                Controller = "EmployeeRelationServices/Service",
                Action = "EmployeeAdvanceRequest",
                Title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeAdvanceRequest),
                ServiceId = "EmployeeAdvanceRequest",
                SecurityId = "EmployeeAdvanceRequest"
            });
            module.Services.Add(new Service()
            {
                Controller = "EmployeeRelationServices/Service",
                Action = "EmployeeCustodiesRequest",
                Title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeCustodiesRequest),
                ServiceId = "EmployeeCustodiesRequest",
                SecurityId = "EmployeeCustodiesRequest"
            });
            module.Services.Add(new Service()
            {
                Controller = "EmployeeRelationServices/Service",
                Action = "EmployeeLoanRequest",
                Title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeLoanRequest),
                ServiceId = "EmployeeLoanRequest",
                SecurityId = "EmployeeLoanRequest"
            });
            module.Services.Add(new Service()
            {
                Controller = "EmployeeRelationServices/Service",
                Action = "EmployeeTransferRequest",
                Title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeTransferRequest),
                ServiceId = "EmployeeTransferRequest",
                SecurityId = "EmployeeTransferRequest"
            });
            module.Services.Add(new Service()
            {
                Controller = "EmployeeRelationServices/Service",
                Action = "PromotionRequest",
                Title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PromotionRequest),
                ServiceId = "PromotionRequest",
                SecurityId = "PromotionRequest"
            });
            module.Services.Add(new Service()
            {
                Controller = "EmployeeRelationServices/Service",
                Action = "FinancialPromotionRequest",
                Title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.FinancialPromotionRequest),
                ServiceId = "FinancialPromotionRequest",
                SecurityId = "FinancialPromotionRequest"
            });
            module.Services.Add(new Service()
            {
                Controller = "EmployeeRelationServices/Service",
                Action = "RewardRequest",
                Title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.RewardRequest),
                ServiceId = "RewardRequest",
                SecurityId = "RewardRequest"
            });
            module.Services.Add(new Service()
            {
                Controller = "EmployeeRelationServices/Service",
                Action = "DisciplinaryRequest",
                Title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.DisciplinaryRequest),
                ServiceId = "DisciplinaryRequest",
                SecurityId = "DisciplinaryRequest"
            });

            module.Services.Add(new Service()
            {
                Controller = "EmployeeRelationServices/Service",
                Action = "ResignationRequest",
                Title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.ResignationRequest),
                ServiceId = "ResignationRequest",
                SecurityId = "ResignationRequest"
            });
            module.Services.Add(new Service()
            {
                Controller = "EmployeeRelationServices/Service",
                Action = "TerminationRequest",
                Title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TerminationRequest),
                ServiceId = "TerminationRequest",
                SecurityId = "TerminationRequest"
            });

            module.Services.Add(new Service()
            {
                Controller = "EmployeeRelationServices/Service",
                Action = "ExitInterview",
                Title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.ExitInterview),
                ServiceId = "ExitInterview",
                SecurityId = "ExitInterview"
            });

            module.Services.Add(new Service()
            {
                Controller = "EmployeeRelationServices/Service",
                Action = "EndWorkflow",
                Title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EndWorkflow),
                ServiceId = "EndWorkflow",
                SecurityId = "EndWorkflow"
            });












            module.Aggregates = module.Aggregates.Where(x => x.TypeFullName != typeof(Applicant).FullName).ToList();

         

            var employeeCardDetails = new List<string>()
                {
                    "Assignments","EmployeeTransfers","EndingSecondaryPositions","EmployeeDisciplinarys","EmployeeTerminations","ExitInterviews","EmployeeResignations",
                    "EmployeePromotions","FinancialPromotions","EmployeeRewards","LeaveRequests","RecycledLeaves","ServiceWorkflows"
                };
            module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(EmployeeCard).FullName))
                .Details = module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(EmployeeCard).FullName))
                    .Details.Where(x => employeeCardDetails.Contains(x.DetailId))
                    .ToList();
          

            

            module.Dashboards.Add(new Dashboard()
            {
                Title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeRelationServicesDashboard),
                Controller = "EmployeeRelationServices/Dashboard",
                Action = "EmployeeRelationServicesDashboard",
                DashboardId = "EmployeeRelationServicesDashboard",
                SecurityId = "EmployeeRelationServicesDashboard"
            });

            module.Dashboards.Add(new Dashboard()
            {
                Title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AdministrativeLevelDashboardForEmployeeRelationsServices),
                Controller = "EmployeeRelationServices/Dashboard",
                Action = "AdministrativeLevelDashboardForEmployeeRelationsServices",
                DashboardId = "AdministrativeLevelDashboardForEmployeeRelationsServices",
                SecurityId = "AdministrativeLevelDashboardForEmployeeRelationsServices"
            });

        }
    }

    
}