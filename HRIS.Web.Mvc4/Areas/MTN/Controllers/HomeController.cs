using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.MTN.RootEntities;
using HRIS.Domain.MTN.Entities;
using HRIS.Domain.Recruitment.Enums;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Models;
using LinqToExcel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.PersistenceSupport;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Souccar.NHibernate;
using HRIS.Domain.Recruitment.RootEntities;
using HRIS.Domain.Recruitment.Indexes;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Extenstions;
using  Project.Web.Mvc4.Extensions;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Domain.Security;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;



namespace Project.Web.Mvc4.Areas.MTN.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(RequestInformation.Navigation.Step moduleInfo)
        {
            if (TempData["Module"] == null)
                return RedirectToAction("Welcome", "Module", new { area = "", id = ModulesNames.MTN });

            return View();
        }

        public void SaveMtnProject(int userId, string name)
        {
            var entities = new List<IAggregateRoot>();

            var user = ServiceFactory.ORMService.GetById<User>(userId);
            
            var users = new List<User>();
            users.Add(user);

            var workflowItem = MtnWorkflowHelper.InitWithSetting(users, "");
            entities.Add(workflowItem);

            var mtnProjectStepHelper = new MtnProjectStepHelper() { Entity = "JobDescription", WorkflowItem = workflowItem };

            var mtnStep = new MtnStep();
            mtnStep.IsStage = false;
            mtnProjectStepHelper.MtnStep = mtnStep;
            mtnStep.ProjectSteps.Add(mtnProjectStepHelper);
            mtnStep.StepOrder = 1;

            var workflowItem2 = MtnWorkflowHelper.InitWithSetting(users, "");
            entities.Add(workflowItem2);

            var mtnProjectStepHelper2 = new MtnProjectStepHelper() { Entity = "position", WorkflowItem = workflowItem2 };

            var mtnStep2 = new MtnStep();
            mtnStep2.IsStage = false;
            mtnProjectStepHelper2.MtnStep = mtnStep2;
            mtnStep2.ProjectSteps.Add(mtnProjectStepHelper2);
            mtnStep2.StepOrder = 2;

            var mtnproject = new MTNProject();
            mtnproject.Name = "test";
            mtnproject.WorkflowOrder = 1;
            mtnproject.AddMtnStep(mtnStep);
            mtnproject.AddMtnStep(mtnStep2);

            entities.Add(mtnproject);

          
            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
        
        }


     




    }
}
