using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.Personnel.RootEntities;
using Project.Web.Mvc4.Areas.Personnel.Models;
using NHibernate.Hql.Ast.ANTLR;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Models;
using Project.Web.Mvc4.Models.Navigation;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Recruitment.RootEntities;
using HRIS.Domain.Personnel.Configurations;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Models;

using Project.Web.Mvc4.Helpers;

public class PersonnelAdjustment : ModelAdjustment
{

    private static Dictionary<string, ViewModel> parent = new Dictionary<string, ViewModel>();
    public override void AdjustModule(Module module)
    {
        module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(EmployeeCard).FullName))
                .Details.SingleOrDefault(x => x.TypeFullName == (typeof(EmployeeLoan).FullName)).Details = DetailFactory.Create(typeof(EmployeeLoan));

        if (module.ModuleId.Equals(ModulesNames.Personnel))
        {
            var employeeDetails = new List<string>()
                {
                    "Children","Spouse","Dependents","Experiences","Educations","Trainings","Skills","Languages",
                    "Certifications","MilitaryService","Passports","DrivingLicense","Convictions","Residencies","Positions","Attachments","LogOfPositions"
                };
            module.Aggregates = module.Aggregates.Where(x => x.TypeFullName != typeof(Applicant).FullName).ToList();
            module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(Employee).FullName))
                .Details = module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(Employee).FullName))
                    .Details.Where(x => employeeDetails.Contains(x.DetailId))
                    .ToList();

            var employeeCardDetails = new List<string>()
                {
                    "EmployeeCustodies","PrimaryEmployeeBenefits","PrimaryEmployeeDeductions","EmployeeLoans","BankingInformations","TemporaryWorkshops","EmployeeAdvances"
                };

            module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(EmployeeCard).FullName))
                .Details = module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(EmployeeCard).FullName))
                    .Details.Where(x => employeeCardDetails.Contains(x.DetailId))
                    .ToList();

            var employee = module.Aggregates.SingleOrDefault(x => x.TypeFullName == typeof(Employee).FullName);

            var spouse = employee.Details.SingleOrDefault(x => x.TypeFullName ==
                   typeof(Spouse).FullName);
            spouse.Details = DetailFactory.Create(typeof(Spouse));

            var spouseAttachment = spouse.Details.SingleOrDefault(x => x.TypeFullName ==
                typeof(SpouseAttachment).FullName);
            spouseAttachment.Details = DetailFactory.Create(typeof(SpouseAttachment));

            var certification = employee.Details.SingleOrDefault(x => x.TypeFullName ==
            typeof(Certification).FullName);
            certification.Details = DetailFactory.Create(typeof(Certification));

            var certificationAttachment = certification.Details.SingleOrDefault(x => x.TypeFullName ==
                typeof(CertificationAttachment).FullName);
            certificationAttachment.Details = DetailFactory.Create(typeof(CertificationAttachment));

            var child = employee.Details.SingleOrDefault(x => x.TypeFullName ==
            typeof(Child).FullName);
            child.Details = DetailFactory.Create(typeof(Child));

            var childAttachment = child.Details.SingleOrDefault(x => x.TypeFullName ==
                typeof(ChildAttachment).FullName);
            childAttachment.Details = DetailFactory.Create(typeof(ChildAttachment));

            var conviction = employee.Details.SingleOrDefault(x => x.TypeFullName ==
            typeof(Conviction).FullName);
            conviction.Details = DetailFactory.Create(typeof(Conviction));

            var convictionAttachment = conviction.Details.SingleOrDefault(x => x.TypeFullName ==
                typeof(ConvictionAttachment).FullName);
            convictionAttachment.Details = DetailFactory.Create(typeof(ConvictionAttachment));

            var dependent = employee.Details.SingleOrDefault(x => x.TypeFullName ==
            typeof(Dependent).FullName);
            dependent.Details = DetailFactory.Create(typeof(Dependent));

            var dependentAttachment = dependent.Details.SingleOrDefault(x => x.TypeFullName ==
                typeof(DependentAttachment).FullName);
            dependentAttachment.Details = DetailFactory.Create(typeof(DependentAttachment));

            var drivingLicense = employee.Details.SingleOrDefault(x => x.TypeFullName ==
            typeof(DrivingLicense).FullName);
            drivingLicense.Details = DetailFactory.Create(typeof(DrivingLicense));

            var drivingLicenseAttachment = drivingLicense.Details.SingleOrDefault(x => x.TypeFullName ==
                typeof(DrivingLicenseAttachment).FullName);
            drivingLicenseAttachment.Details = DetailFactory.Create(typeof(DrivingLicenseAttachment));

            var education = employee.Details.SingleOrDefault(x => x.TypeFullName ==
            typeof(Education).FullName);
            education.Details = DetailFactory.Create(typeof(Education));

            var educationAttachment = education.Details.SingleOrDefault(x => x.TypeFullName ==
                typeof(EducationAttachment).FullName);
            educationAttachment.Details = DetailFactory.Create(typeof(EducationAttachment));

            var experience = employee.Details.SingleOrDefault(x => x.TypeFullName ==
            typeof(Experience).FullName);
            experience.Details = DetailFactory.Create(typeof(Experience));

            var experienceAttachment = experience.Details.SingleOrDefault(x => x.TypeFullName ==
                typeof(ExperienceAttachment).FullName);
            experienceAttachment.Details = DetailFactory.Create(typeof(ExperienceAttachment));

            var language = employee.Details.SingleOrDefault(x => x.TypeFullName ==
            typeof(Language).FullName);
            language.Details = DetailFactory.Create(typeof(Language));

            var languageAttachment = language.Details.SingleOrDefault(x => x.TypeFullName ==
                typeof(LanguageAttachment).FullName);
            languageAttachment.Details = DetailFactory.Create(typeof(LanguageAttachment));

            var militaryService = employee.Details.SingleOrDefault(x => x.TypeFullName ==
            typeof(MilitaryService).FullName);
            militaryService.Details = DetailFactory.Create(typeof(MilitaryService));

            var militaryServiceAttachment = militaryService.Details.SingleOrDefault(x => x.TypeFullName ==
                typeof(MilitaryServiceAttachment).FullName);
            militaryServiceAttachment.Details = DetailFactory.Create(typeof(MilitaryServiceAttachment));

            var passport = employee.Details.SingleOrDefault(x => x.TypeFullName ==
            typeof(Passport).FullName);
            passport.Details = DetailFactory.Create(typeof(Passport));

            var passportAttachment = passport.Details.SingleOrDefault(x => x.TypeFullName ==
                typeof(PassportAttachment).FullName);
            passportAttachment.Details = DetailFactory.Create(typeof(PassportAttachment));

            var residency = employee.Details.SingleOrDefault(x => x.TypeFullName ==
            typeof(Residency).FullName);
            residency.Details = DetailFactory.Create(typeof(Residency));

            var residencyAttachment = residency.Details.SingleOrDefault(x => x.TypeFullName ==
                typeof(ResidencyAttachment).FullName);
            residencyAttachment.Details = DetailFactory.Create(typeof(ResidencyAttachment));

            var skill = employee.Details.SingleOrDefault(x => x.TypeFullName ==
            typeof(Skill).FullName);
            skill.Details = DetailFactory.Create(typeof(Skill));

            var skillAttachment = skill.Details.SingleOrDefault(x => x.TypeFullName ==
                typeof(SkillAttachment).FullName);
            skillAttachment.Details = DetailFactory.Create(typeof(SkillAttachment));

            var training = employee.Details.SingleOrDefault(x => x.TypeFullName ==
            typeof(Training).FullName);
            training.Details = DetailFactory.Create(typeof(Training));

            var trainingAttachment = training.Details.SingleOrDefault(x => x.TypeFullName ==
                typeof(TrainingAttachment).FullName);
            trainingAttachment.Details = DetailFactory.Create(typeof(TrainingAttachment));
        }

        module.Dashboards.Add(new Dashboard()
        {
            Title = GlobalResource.Dashboard,
            Controller = "Personnel/Dashboard",
            Action = "PersonnelDashboard",
            DashboardId = "PersonnelDashboard",
            SecurityId = "PersonnelDashboard"
        });
   


    }


    public override ViewModel AdjustGridModel(string type)
    {
      
        if (parent.Count == 0)
        {
            parent.Add("Attachment", new AttachmentViewModel());
            parent.Add("MilitaryService", new MilitaryServiceViewModel());
            parent.Add("Passport", new PassportViewModel());
            parent.Add("Residency", new ResidencieViewModel());
            parent.Add("Spouse", new SpouseViewModel());
            parent.Add("Language", new LanguageViewModel());
            parent.Add("Certification", new CertificationViewModel());
            parent.Add("Conviction", new ConvictionViewModel());
            parent.Add("HealthInsuranceTypes", new DefineHealthInsuranceViewModel());
            parent.Add("Dependent", new DependentViewModel());
            parent.Add("DrivingLicense", new DrivingLicenseViewModel());
            parent.Add("EmployeeCustodie", new EmployeeCustodieViewModel());
            parent.Add("Experience", new ExperienceViewModel());
            parent.Add("JobRelatedInfo", new JobRelatedInfoViewModel());
            parent.Add("Skill", new SkillViewModel());
            parent.Add("Child", new ChildViewModel());
            parent.Add("PositionsLogOfEmployee", new LogOfPositionsViewModel());
            parent.Add("EmployeeCodeSetting", new EmployeeCodeSettingViewModel());
            parent.Add("Employee", new Project.Web.Mvc4.Areas.Personnel.Models.EmployeeViewModel());

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
}