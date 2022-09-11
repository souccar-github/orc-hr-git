//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//project manager:
//supervisor:
//author: Ammar Alziebak
//description:
//start date:
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.RootEntities;
using HRIS.Domain.Workflow;
using Project.Web.Mvc4.Areas.PMS.EventHandlers;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using HRIS.Domain.PMS.Configurations;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;

namespace Project.Web.Mvc4.Areas.PMS.Models
{
    public partial class PmsViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {

            #region TemplateSectionWeight






            if (type == typeof(TemplateAppraisalPositions))
            {
                //Show Windows with Two Columns
                model.Views[0].ShowTwoColumns = false;

                //Hidden field from grid
                model.SchemaFields.SingleOrDefault(x => x.Name == "WorkflowApplyFlag").Editable = false;

                //model.Views[0].EditHandler = "getTemplateAppraisalPositionsWindow";

            }
            if (type == typeof(AppraisalPhaseWorkflow))
            {

                model.ActionListHandler = "initializeAppraisalPhaseWoekflowActionList";
                model.ToolbarCommands.RemoveAt(0);
                model.Views[0].ViewHandler = "AppraisalPhaseWorkflowViewHandler";



                ////Show Windows with Two Columns
                //model.Views[0].ShowTwoColumns = false;

                ////Hidden field from grid
                //model.SchemaFields.SingleOrDefault(x => x.Name == "WorkflowApplyFlag").Editable = false;

                //model.Views[0].EditHandler = "getAppraisalPhaseWorkflowWindow";
            }
            //if (type == typeof(AppraisalPhaseConfiguration))
            //{
            //    ////Action List Command
            //    //model.ActionList.Commands.Add(new ActionListCommand() { GroupId = 1, Order = 1, HandlerName = "AppraisalPhaseConfigurationWorkflow", Name = "Appraisal Phase Configuration Workflow", ShowCommand = true });
            //    //model.ActionList.Commands.Add(new ActionListCommand() { GroupId = 1, Order = 2, HandlerName = "AppraisalPhaseConfigurationApprovals", Name = "Appraisal Phase Configuration Approvals", ShowCommand = true });

            //    ////Call Event Handlers
            //    //model.ViewModelTypeFullName = typeof(AppraisalPhaseConfigurationEventHandlers).FullName;
            //}

            if (type == typeof(AppraisalTemplateSetting))
            {
                requestInformation.NavigationInfo.Next.Clear();

                //Action List Command
                //model.ActionList.Commands.Add(new ActionListCommand()
                //{
                //    GroupId = 1,
                //    Order = 1,
                //    HandlerName = "OverwriteAppraisalTemplateSetting",
                //    Name = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.OverwriteAppraisalTemplateSetting),
                //    ShowCommand = true
                //});
            }

            //if (type==typeof(AppraisalPhaseSetting))
            //{
            //    model.Views[0].EditHandler = "AppraisalPhaseSettingEditHandler";
            //    model.Views[0].ViewHandler = "AppraisalPhaseSettingViewHandler";
            //}
            #endregion

        }

    }
}
