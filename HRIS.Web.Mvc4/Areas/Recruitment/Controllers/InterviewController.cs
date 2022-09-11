using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.Recruitment.RootEntities;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Domain.Security;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using FluentNHibernate.Conventions;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.PMS.RootEntities;
using HRIS.Domain.Recruitment.Entities.Evaluations;
using HRIS.Domain.Recruitment.Enums;
using HRIS.Domain.Workflow;
using Project.Web.Mvc4.Areas.Appraisals;
using Project.Web.Mvc4.Areas.Recruitment.Models.ApplicantsEvaluation;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.Resource;
using Souccar.Domain.Workflow.Entities;
using Souccar.Domain.Workflow.Enums;
using Project.Web.Mvc4.ProjectModels;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Notification;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.Personnel.Configurations;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.Enums;
using Project.Web.Mvc4.Areas.JobDescription.Helpers;
using Project.Web.Mvc4.Areas.Security.Helpers;
using AutoMapper;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Recruitment.Entities;
using Souccar.Infrastructure.Extenstions;
using Souccar.NHibernate;
using HRIS.Domain.PayrollSystem.RootEntities;

namespace Project.Web.Mvc4.Areas.Recruitment.Controllers
{
    public class InterviewController : Controller
    {
        //
        // GET: /Recruitment/Interview/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ApplicantsEvaluation()
        {
            return PartialView("../Services/ApplicantsEvaluation");
        }

        public ActionResult GetApplicants()
        {
            var applicantsInfo = new List<ApplicantInfoViewModel>();
            var currentUser = UserExtensions.CurrentUser;
            var interviews = ServiceFactory.ORMService.All<Interview>()
                .Where(x => x.WorkflowItem.Approvals.Any(y => y.User.Id == currentUser.Id)).ToList();

            foreach (var interview in interviews)
            {
                var user = GetApproval(interview?.WorkflowItem);
                if (user != null)
                {
                    if (user.Id == currentUser.Id)
                    {
                        var position = GetPosition(interview.JobApplication);
                        var positionCode = position != null ? position.Code : "";
                        applicantsInfo.Add(new ApplicantInfoViewModel()
                        {
                            FullName = interview?.JobApplication.FullName,
                            InterviewId = interview.Id,
                            WorkflowId = interview.InterviewAppraisalSetting.WorkflowSetting.Id,
                            WorkflowItemId = interview.WorkflowItem.Id,
                            Position = $"{position?.JobDescription.Name}{positionCode}"
                        });
                    }
                }
            }

            return Json(new { Applicants = applicantsInfo }, JsonRequestBehavior.AllowGet);
        }

        private Position GetPosition(JobApplication jobApplication)
        {
            if (jobApplication.Position != null)
            {
                return jobApplication.Position;
            }
            else if (jobApplication.RecruitmentRequest != null &&
                     jobApplication.RecruitmentRequest.RequestedPosition != null)
            {
                return jobApplication.RecruitmentRequest.RequestedPosition;
            }

            return null;

        }

        public ActionResult GenerateWorkflowItemsToInterview(int id)
        {
            try
            {
                var workflowItem = new WorkflowItem()
                {
                    Date = DateTime.Now,
                    Creator = UserExtensions.CurrentUser,
                    Status = WorkflowStatus.Pending,
                    Type = WorkflowType.InterviewEvaluation,
                    StepCount = 0
                };

                var interview = ServiceFactory.ORMService.GetById<Interview>(id);
                var workflowSettingApprovals = interview?.InterviewAppraisalSetting?.WorkflowSetting.SettingApprovals;
                if (workflowSettingApprovals != null)
                {
                    foreach (var workflowSettingApproval in workflowSettingApprovals)
                    {
                        workflowItem.Approvals.Add(new WorkflowApproval()
                        {
                            Date = DateTime.Now,
                            Order = workflowSettingApproval.Order,
                            Status = WorkflowStepStatus.Pending,
                            User = workflowSettingApproval.Position.User()
                        });
                    }
                }

                workflowItem.Save();

                return Json(new { Success = true, WorkflowItemId = workflowItem.Id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult InitialApplicantEvaluationInfo(int id)
        {
            var interview = ServiceFactory.ORMService.GetById<Interview>(id);
            if (interview != null)
            {
                //var employee = EmployeeExtensions.CurrentEmployee;
                //var position = employee?.PrimaryPosition();
                var appraisalPhaseSetting = interview.InterviewAppraisalSetting;
                var appraisalTemplate = interview.InterviewAppraisalTemplate;
                var position = GetPosition(interview.JobApplication);
                var result = new EvaluationViewModel
                {
                    FullName = interview.JobApplication.FullName,
                    Position = interview.JobApplication.RecruitmentRequest!=null ? interview.JobApplication.RecruitmentRequest.PositionCode:"",
                    InterviewId = interview.Id,
                    TotalMark = interview.FinalMark,
                    TemplateId = appraisalTemplate?.Id,
                    WorkflowId = interview.InterviewAppraisalSetting?.WorkflowSetting?.Id,
                    PhaseSettingId = interview.InterviewAppraisalSetting?.Id,
                    Step = appraisalPhaseSetting.MarkStep,
                    MinMark = appraisalPhaseSetting.FromMark,
                    MaxMark = appraisalPhaseSetting.ToMark,
                    StrongLimit = appraisalPhaseSetting.SkillThreshold,
                    WeaknessLimit = appraisalPhaseSetting.GapThreshold,
                    CustomSections = GetCustomSections(appraisalTemplate?.Id),
                    ShowRejectButton = false,
                    WorkflowItemId = interview.WorkflowItem.Id,
                    Note = ""
                };
                return Json(new
                {
                    Success = true,
                    Result = result

                }, JsonRequestBehavior.AllowGet);
            }


            return Json(new
            {
                Success = false,
                Message = GlobalResource.FailMessage
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetCustomSectionsInformation(int interviewId)
        {
            var interview = ServiceFactory.ORMService.GetById<Interview>(interviewId);
            var customSections = GetCustomSectionViewModel(interview);
            return Json(customSections, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Accept(EvaluationViewModel evaluationViewModel, IList<Dictionary<string, object>> checkedItems)
        {
            if (evaluationViewModel.InterviewId != null)
            {
                var interview = ServiceFactory.ORMService.GetById<Interview>(evaluationViewModel.InterviewId.Value);
                var workflowItem = ServiceFactory.ORMService.GetById<WorkflowItem>(evaluationViewModel.WorkflowItemId);
                var currentUser = UserExtensions.CurrentUser;

                float result = UpdateResultFromEvaluationViewModel(interview, evaluationViewModel);

                var approval = workflowItem.Approvals.FirstOrDefault(x => x.User.Id == currentUser.Id);
                approval.Status = WorkflowStepStatus.Accept;
                approval.IsSeen = true;
                approval.Description = evaluationViewModel.Note;
                approval.Date = DateTime.Now;

                var checkIfThereIsAnotherApprovalPending = workflowItem.Approvals.Any(x =>
                    x.Status == WorkflowStepStatus.Pending && x.User != currentUser);
                if (!checkIfThereIsAnotherApprovalPending)
                {
                    workflowItem.Status = WorkflowStatus.Completed;
                }

                ServiceFactory.ORMService.Save(workflowItem, currentUser);

                var evaluator = ServiceFactory.ORMService.All<Evaluator>().FirstOrDefault(x =>
                    x.User.Id == UserExtensions.CurrentUser.Id && x.Interview.Id == evaluationViewModel.InterviewId);

                foreach (var interviewCustomSection in evaluator.InterviewCustomSections)
                {
                    var customSection =
                        evaluationViewModel.CustomSections.FirstOrDefault(
                            x => x.Id == interviewCustomSection.AppraisalSection.Id);
                    interviewCustomSection.Weight = customSection.SectionWeight;
                    foreach (var interviewCustomSectionItem in interviewCustomSection.InterviewCustomSectionItems)
                    {
                        var customSectionItem = evaluationViewModel.CustomSections.SelectMany(x => x.AppraisalItems).FirstOrDefault(
                            x => x.Id == interviewCustomSectionItem.AppraisalSectionItem.Id);

                        interviewCustomSectionItem.Rate = (float) Math.Round(customSectionItem.Rate, 1);
                        interviewCustomSectionItem.Weight = customSectionItem.Weight;
                        interviewCustomSectionItem.CanNotEvaluate = customSectionItem.CanNotEvaluate;
                        interviewCustomSectionItem.Description = customSectionItem.Note;
                        interviewCustomSectionItem.Save();
                    }

                    interviewCustomSection.Save();
                }

                evaluator.Mark = evaluator.EvaluationValue();
                evaluator.Save();

                interview.CalculateFinalMark();
                interview.Save();

                if (checkIfThereIsAnotherApprovalPending)
                {
                    var body = $"{RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.YouHaveAnInterviewEvaluationFor)} {interview.JobApplication?.FullName}";
                    var title = $"{RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.YouHaveAnInterviewEvaluationFor)} {interview.JobApplication?.FullName}";
                    var destinationTabName = NavigationTabName.Strategic;
                    var destinationModuleName = ModulesNames.Recruitment;
                    var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
                        ModulesNames.ResourceGroupName + "_" + ModulesNames.Recruitment);
                    var destinationControllerName = "Recruitment/Home";
                    var destinationActionName = "ApplicantsEvaluation";
                    var destinationEntityId = "ApplicantsEvaluation";
                    var destinationEntityTitle = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.ApplicantsEvaluation);
                    var destinationEntityOperationType = OperationType.Nothing;
                    IDictionary<string, int> destinationData = new Dictionary<string, int>();
                    destinationData.Add("WorkflowId", evaluationViewModel.WorkflowItemId);
                    destinationData.Add("InterviewId", evaluationViewModel.InterviewId.Value);

                    SendNotification(title, body, destinationTabName, destinationModuleName,
                        destinationLocalizationModuleName,
                        destinationControllerName, destinationActionName, destinationEntityId, destinationEntityTitle,
                        destinationEntityOperationType, destinationData, workflowItem, currentUser);
                }

                return Json(new { Success = false, Result = result }, JsonRequestBehavior.AllowGet);
            }


            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Reject()
        {
            return View();
        }

        public ActionResult Pending(EvaluationViewModel evaluationViewModel)
        {
            if (evaluationViewModel.InterviewId != null)
            {
                var interview = ServiceFactory.ORMService.GetById<Interview>(evaluationViewModel.InterviewId.Value);
                var workflowItem = ServiceFactory.ORMService.GetById<WorkflowItem>(evaluationViewModel.WorkflowItemId);
                var currentUser = UserExtensions.CurrentUser;

                var approval = workflowItem.Approvals.FirstOrDefault(x => x.User.Id == currentUser.Id);
                approval.Status = WorkflowStepStatus.Pending;
                approval.IsSeen = true;
                approval.Description = evaluationViewModel.Note;
                approval.Date = DateTime.Now;

                ServiceFactory.ORMService.Save(workflowItem, currentUser);

                var evaluator = ServiceFactory.ORMService.All<Evaluator>().FirstOrDefault(x =>
                    x.User.Id == UserExtensions.CurrentUser.Id && x.Interview.Id == evaluationViewModel.InterviewId);

                foreach (var interviewCustomSection in evaluator.InterviewCustomSections)
                {
                    var customSection =
                        evaluationViewModel.CustomSections.FirstOrDefault(
                            x => x.Id == interviewCustomSection.AppraisalSection.Id);
                    interviewCustomSection.Weight = customSection.SectionWeight;
                    foreach (var interviewCustomSectionItem in interviewCustomSection.InterviewCustomSectionItems)
                    {
                        var customSectionItem = evaluationViewModel.CustomSections.SelectMany(x => x.AppraisalItems).FirstOrDefault(
                            x => x.Id == interviewCustomSectionItem.AppraisalSectionItem.Id);

                        interviewCustomSectionItem.Rate = (float)Math.Round(customSectionItem.Rate,1);
                        interviewCustomSectionItem.Weight = customSectionItem.Weight;
                        interviewCustomSectionItem.CanNotEvaluate = customSectionItem.CanNotEvaluate;
                        interviewCustomSectionItem.Description = customSectionItem.Note;
                        interviewCustomSectionItem.Save();
                    }

                    interviewCustomSection.UpdateValue();
                    interviewCustomSection.Save();
                }


                return Json(new { Success = false, Result = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.PendingApproved) }, JsonRequestBehavior.AllowGet);
            }


            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEvaluationInformation(EvaluationViewModel evaluationViewModel)
        {
            List<DevelopmentViewModel> result = null;
            result = DevelopmentViewModel.CreateInstance(evaluationViewModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckStatementOfJobApplicationStatus(int id)
        {
            var interview = ServiceFactory.ORMService.GetById<Interview>(id);
            if (interview != null)
            {
                #region Check if application status is Initiate

                var jobApplication = interview.JobApplication;
                if (jobApplication != null && jobApplication.ApplicationStatus != ApplicationStatus.Initiated)
                {
                    return Json(new
                    {
                        Success = false,
                        Message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper
                            .ApplicationStatusMustBeInitiated)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                #region Check if the interview evaluation is complete

                var workflowItem = interview.WorkflowItem;
                if (workflowItem != null && workflowItem.Status != WorkflowStatus.Completed)
                {
                    return Json(new
                    {
                        Success = false,
                        Message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper
                            .InterviewEvaluationMustBeCompleted)
                    }, JsonRequestBehavior.AllowGet);
                }

                #endregion

                var laseUser = interview.Evaluators.LastOrDefault().User;

                var appraisalTemplate = interview.InterviewAppraisalTemplate;
                var appraisalPhaseSetting = interview.InterviewAppraisalSetting;

                var evaluationViewModel = new EvaluationViewModel
                {
                    FullName = interview.JobApplication.FullName,
                    Position = GetPosition(interview.JobApplication)?.Code,
                    InterviewId = interview.Id,
                    TemplateId = appraisalTemplate?.Id,
                    WorkflowId = interview.InterviewAppraisalSetting?.WorkflowSetting?.Id,
                    PhaseSettingId = interview.InterviewAppraisalSetting?.Id,
                    Step = appraisalPhaseSetting.MarkStep,
                    MinMark = appraisalPhaseSetting.FromMark,
                    MaxMark = appraisalPhaseSetting.ToMark,
                    StrongLimit = appraisalPhaseSetting.SkillThreshold,
                    WeaknessLimit = appraisalPhaseSetting.GapThreshold,
                    CustomSections = GetCustomSectionViewModel(interview, laseUser),
                    ShowRejectButton = false,
                    WorkflowItemId = interview.WorkflowItem.Id,
                    Note = ""
                };

                var result = DevelopmentViewModel.CreateInstance(evaluationViewModel);

                return Json(new { Success = true, DevelopmentViewModel = result, FinalMark = interview.FinalMark, ViewModel = evaluationViewModel }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveApplicationStatus(ApplicationStatus status, int id)
        {
            var interview = ServiceFactory.ORMService.GetById<Interview>(id);
            if (interview != null)
            {
                
                var jobApplication = interview.JobApplication;
                jobApplication.ApplicationStatus = status;

                ServiceFactory.ORMService.Save(jobApplication, UserExtensions.CurrentUser);
                if (status == ApplicationStatus.Accepted)
                {
                    var recruitmentRequest = jobApplication.RecruitmentRequest;
                    if (recruitmentRequest != null)
                    {
                        recruitmentRequest.RequestStatus = RequestStatus.Finished;
                        ServiceFactory.ORMService.Save(recruitmentRequest, UserExtensions.CurrentUser);
                    }

                    var newEmployee = CreateEmployee(jobApplication);
                    CreateEmployeeEducations(newEmployee, jobApplication);
                    CreateEmployeeExperiences(newEmployee, jobApplication);
                    CreateEmployeeLanguages(newEmployee, jobApplication);
                    CreateEmployeeCertifications(newEmployee, jobApplication);
                    CreateEmployeeSkills(newEmployee, jobApplication);
                    CreateEmployeeTrainingCourses(newEmployee, jobApplication);
                    CreateEmployeeMilitaryService(newEmployee, jobApplication);
                    ServiceFactory.ORMService.Save(newEmployee, UserExtensions.CurrentUser);
                }

                return Json(new { Success = true, Message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.ApplicationStatusSuccessfullyChanged) }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false, Message = GlobalResource.ExceptionMessage }, JsonRequestBehavior.AllowGet);
        }

        private static void CreateEmployeeEducations(Employee employee, JobApplication jobApplication)
        {
            Mapper.CreateMap<RecruitmentEducation, Education>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.Employee, y => y.Ignore());

            foreach (var recruitmentEducation in jobApplication.Educations)
            {
                var education = new Education();
                Mapper.Map<RecruitmentEducation, Education>(recruitmentEducation, education);
                education.Employee = employee;
                employee.AddEducation(education);
            }
        }

        private static void CreateEmployeeLanguages(Employee employee, JobApplication jobApplication)
        {
            Mapper.CreateMap<TrainingCourseLanguage, Language>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.Employee, y => y.Ignore());

            foreach (var lang in jobApplication.Languages)
            {
                var language = new Language();
                Mapper.Map<TrainingCourseLanguage, Language>(lang, language);
                language.Employee = employee;
                employee.AddLanguage(language);
            }
        }

        private static void CreateEmployeeCertifications(Employee employee, JobApplication jobApplication)
        {
            Mapper.CreateMap<ProfessionalCertification, Certification>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.Employee, y => y.Ignore());

            foreach (var professionalCertification in jobApplication.ProfessionalCertifications)
            {
                var certification = new Certification();
                Mapper.Map<ProfessionalCertification, Certification>(professionalCertification, certification);
                certification.Employee = employee;
                employee.AddCertification(certification);
            }
        }

        private static void CreateEmployeeSkills(Employee employee, JobApplication jobApplication)
        {
            Mapper.CreateMap<PersonalSkill, Skill>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.Employee, y => y.Ignore());

            foreach (var personalSkill in jobApplication.PersonalSkills)
            {
                var skill = new Skill();
                Mapper.Map<PersonalSkill, Skill>(personalSkill, skill);
                skill.Employee = employee;
                employee.AddSkill(skill);
            }
        }

        private static void CreateEmployeeExperiences(Employee employee, JobApplication jobApplication)
        {
            Mapper.CreateMap<WorkingExperience, Experience>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.Employee, y => y.Ignore());
            var experiences = new List<Experience>();

            foreach (var workingExperience in jobApplication.WorkingExperiences)
            {
                var experience = new Experience();
                Mapper.Map<WorkingExperience, Experience>(workingExperience, experience);
                experience.Employee = employee;
                employee.AddExperience(experience);
            }

        }

        private static void CreateEmployeeTrainingCourses(Employee employee, JobApplication jobApplication)
        {
            var trainingCourses = new List<TrainingCourse>();

            foreach (var trainingCourse in jobApplication.TrainingCourses)
            {
                var training = new HRIS.Domain.Personnel.Entities.Training()
                {
                    Status = trainingCourse.Status,
                    //CertificateIssuanceDate = trainingCourse.AttendanceCertificateIssuanceDate.Value,
                    //CourseDuration = int.TryParse(trainingCourse.CourseDuration,out ) ,
                    CourseName = trainingCourse.CourseName,
                    Notes = trainingCourse.Description,
                    Specialize = trainingCourse.CompetencyName,
                    //TrainingCenter = trainingCourse.TrainingCenter,
                    TrainingCenterLocation = trainingCourse.TrainingLocation,
                    Employee = employee
                };

                if (trainingCourse.AttendanceCertificateIssuanceDate != null)
                    training.CertificateIssuanceDate = trainingCourse.AttendanceCertificateIssuanceDate.Value;

                int duration = 0;
                if (int.TryParse(trainingCourse.CourseDuration, out duration))
                    training.CourseDurationPerHour = duration;


                employee.AddTraining(training);
            }

        }

        private static void CreateEmployeeMilitaryService(Employee employee, JobApplication jobApplication)
        {
            Mapper.CreateMap<RecruitmentMilitaryService, MilitaryService>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.Employee, y => y.Ignore());
            var militaryServices = new List<MilitaryService>();

            foreach (var recruitmentMilitaryServices in jobApplication.RecruitmentMilitaryServices)
            {
                var militaryService = new MilitaryService();
                Mapper.Map<RecruitmentMilitaryService, MilitaryService>(recruitmentMilitaryServices, militaryService);
                militaryService.Employee = employee;
                
                employee.AddMilitaryService(militaryService);
            }

        }

        private Employee CreateEmployee(JobApplication jobApplication)
        {
            string commandName = ServiceFactory.LocalizationService.GetResource(CommandsNames.ResourceGroupName + "_" + CommandsNames.CreateEmployee);
            var employee = new Employee()
            {
                FirstName = jobApplication.FirstName,
                LastName = jobApplication.LastName,
                FatherName = jobApplication.FatherName,
                MotherName = jobApplication.MotherName,
                DateOfBirth = jobApplication.DateOfBirth,
                PlaceOfBirth = jobApplication.PlaceOfBirth,
                Nationality = jobApplication.Nationality,
                CivilRecordPlaceAndNumber = jobApplication.CivilRecordPlaceAndNumber,
                IdentificationNo = jobApplication.IdentificationNo,
                CountryOfBirth = jobApplication.CountryOfBirth,
                PersonalRecordSource = jobApplication.PersonalRecordSource,
                Gender = jobApplication.Gender,
                Religion = jobApplication.Religion,
                FirstNameL2 = jobApplication.FirstNameL2,
                LastNameL2 = jobApplication.LastNameL2,
                FatherNameL2 = jobApplication.FatherNameL2,
                MotherNameL2 = jobApplication.MotherNameL2,
                PlaceOfBirthL2 = jobApplication.PlaceOfBirthL2,
                DisabilityExist = jobApplication.DisabilityExist,
                DisabilityType = jobApplication.DisabilityExist ? jobApplication.DisabilityType : null,
                BloodType = jobApplication.BloodType,
                OtherNationalityExist = jobApplication.OtherNationalityExist,
                OtherNationality = jobApplication.OtherNationalityExist ? jobApplication.OtherNationality : null,
                MaritalStatus = jobApplication.MaritalStatus,
                MilitaryStatus = jobApplication.MilitaryStatus,
                Address = jobApplication.Address,
                POBox = jobApplication.POBox,
                Mobile = jobApplication.Mobile,
                Phone = jobApplication.Phone,
                WebSite = jobApplication.WebSite,
                Email = jobApplication.Email,
                Facebook = jobApplication.Facebook,
                Twitter = jobApplication.Twitter
            };

            //ServiceFactory.ORMService.Save(employee, UserExtensions.CurrentUser);
            var financialCard = new FinancialCard
            {
                Employee = employee,
                SalaryDeservableType = SalaryDeservableType.Nothing
            };
            var empCard = new EmployeeCard { CardStatus = EmployeeCardStatus.New, FinancialCard = financialCard, Employee = employee, ProbationPeriodEndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0) };
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { employee, empCard,financialCard }, UserExtensions.CurrentUser);

            if (ServiceFactory.ORMService.All<EmployeeCodeSetting>().Any())
            {
                var employeeCodeSetting = ServiceFactory.ORMService.All<EmployeeCodeSetting>().First();
                employee.Code = JobDescriptionHelper.GetCode(employeeCodeSetting, employee);
            }

            if (employee.Gender == 0)
            {
                employee.MilitaryStatus = MilitaryStatus.Nothing;
            }

            if (!employee.OtherNationalityExist)
            {

                employee.OtherNationality = null;
            }
            UserHelper.CreateUser(employee, employee.Username, UserHelper.DefaultPassword, false, false,commandName);
            var user = UserHelper.GetUserByUsername(employee.Username);
            employee.User = user;
            employee.Save();

            return employee;
        }

        #region Method Helper

        public IList<CustomSectionViewModel> GetCustomSections(int? templateId)
        {
            var customSections = new List<CustomSectionViewModel>();
            if (templateId != null)
            {
                var template = ServiceFactory.ORMService.GetById<AppraisalTemplate>(templateId.Value);

                foreach (var customSection in template.TemplateSectionWeights)
                {
                    customSections.Add(new CustomSectionViewModel()
                    {
                        Name = customSection.AppraisalSection.Name,
                        SectionWeight = customSection.Weight,
                        Description = customSection.AppraisalSection.Description,
                        Id = customSection.AppraisalSection.Id,
                        AppraisalItems = customSection.AppraisalSection.Items.Select(x => new AppraisalSectionItemViewModel()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            CanNotEvaluate = false,
                            Weight = x.Weight,                           
                            Kpis = x.Kpis.Select(y => new KpiViewModel()
                            { Description = y.Description, Value = y.Value, Weight = y.Weight }).ToList()
                        }).ToList()

                    });
                }
            }

            return customSections;
        }

        private User GetApproval(WorkflowItem workflow)
        {
            if (workflow.Approvals.IsEmpty())
                return null;
            if (workflow.Approvals.Any(x => x.Status == WorkflowStepStatus.Reject))
                return null;
            var temp = workflow.Approvals.Where(x => x.Status == WorkflowStepStatus.Pending).OrderBy(x => x.Order).FirstOrDefault();
            return temp != null ? temp.User : null;
        }

        private IList<CustomSectionViewModel> GetCustomSectionViewModel(Interview interview)
        {
            var customSections = new List<CustomSectionViewModel>();

            User user = null;
            var currentApproval = interview.WorkflowItem.Approvals.FirstOrDefault(x => x.User == UserExtensions.CurrentUser);
            if (currentApproval != null && currentApproval.IsSeen)
            {
                user = UserExtensions.CurrentUser;
            }
            else
            {
                var previousApproval = interview.WorkflowItem.Approvals.LastOrDefault(x => x.Status == WorkflowStepStatus.Accept);
                if (previousApproval != null)
                {
                    user = previousApproval.User;
                }
                else
                {
                    user = UserExtensions.CurrentUser;
                }
            }

            if (user != null)
            {
                customSections = GetCustomSectionViewModel(interview, user);

            }

            return customSections;
        }

        private List<CustomSectionViewModel> GetCustomSectionViewModel(Interview interview, User user)
        {
            var customSections = new List<CustomSectionViewModel>();
            var evaluator = ServiceFactory.ORMService.All<Evaluator>().FirstOrDefault(x =>
                x.User.Id == user.Id && x.Interview.Id == interview.Id);

            if (evaluator != null)
            {

                foreach (var interviewCustomSection in evaluator.InterviewCustomSections)
                {
                    customSections.Add(new CustomSectionViewModel()
                    {
                        Name = interviewCustomSection.AppraisalSection.Name,
                        SectionWeight = interviewCustomSection.Weight,
                        Description = interviewCustomSection.AppraisalSection.Description,
                        Id = interviewCustomSection.AppraisalSection.Id,
                        AppraisalItems = interviewCustomSection.InterviewCustomSectionItems.Select(x =>
                            new AppraisalSectionItemViewModel()
                            {
                                Id = x.AppraisalSectionItem.Id,
                                Name = x.AppraisalSectionItem.Name,
                                CanNotEvaluate = x.CanNotEvaluate,
                                Weight = x.Weight,
                                Rate = (float)Math.Round(x.Rate,1),
                                Note = user.Username == UserExtensions.CurrentUser.Username ? x.Description : "",
                                Kpis = x.AppraisalSectionItem.Kpis.Select(y => new KpiViewModel()
                                { Description = y.Description, Value = y.Value, Weight = y.Weight }).ToList()
                            }).ToList()
                    });
                }
            }

            return customSections;
        }

        private float UpdateResultFromEvaluationViewModel(Interview interview, EvaluationViewModel evaluationViewModel)
        {
            var appraisalTemplate = interview.InterviewAppraisalTemplate;
            float value = 0;
            foreach (var custom in evaluationViewModel.CustomSections)
            {
                var templateSectionWeight = appraisalTemplate.TemplateSectionWeights.FirstOrDefault(x => x.AppraisalSection.Id == custom.Id);
                if (templateSectionWeight != null)
                    value += custom.AppraisalItems.Sum(x => x.Weight * x.Rate / 100) * templateSectionWeight.Weight;
            }
            value /= 100;

            return (float)Math.Round(value,2);
        }

        private void SendNotification(string title, string body, string destinationTabName,
         string destinationModuleName, string destinationLocalizationModuleName, string destinationControllerName,
         string destinationActionName, string destinationEntityId, string destinationEntityTitle,
         OperationType destinationEntityOperationType, IDictionary<string, int> destinationData, WorkflowItem workflowItem, User user)
        {

            var _notification = ServiceFactory.ORMService.All<Notify>().OrderByDescending(x => x.Date)
                .FirstOrDefault(x => x.DestinationData["WorkflowId"] == workflowItem.Id);
            if (_notification != null)
            {
                foreach (var receiver in _notification.Receivers)
                    receiver.IsRead = true;
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { _notification },
                    UserExtensions.CurrentUser);
            }

            var nextUser = GetApproval(workflowItem);
            if (nextUser != null)
            {
                var notify = new Notify()
                {
                    Sender = user,
                    Body = body,
                    Subject = title,
                    Type = NotificationType.Request,
                    DestinationTabName = destinationTabName,
                    DestinationModuleName = destinationModuleName,
                    DestinationLocalizationModuleName = destinationLocalizationModuleName,
                    DestinationControllerName = destinationControllerName,
                    DestinationActionName = destinationActionName,
                    DestinationEntityId = destinationEntityId,
                    DestinationEntityTitle = destinationEntityTitle,
                    DestinationEntityOperationType = destinationEntityOperationType,
                    DestinationData = destinationData
                };
                notify.AddNotifyReceiver(new NotifyReceiver()
                {
                    Date = DateTime.Now,
                    Receiver = nextUser
                });

                ServiceFactory.ORMService.Save(notify, user);
            }


        }


        #endregion


    }

}
