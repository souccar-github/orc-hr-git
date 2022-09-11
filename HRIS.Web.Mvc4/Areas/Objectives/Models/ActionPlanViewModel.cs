using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.Objectives.Enums;
using HRIS.Domain.Objectives.RootEntities;
using  Project.Web.Mvc4.Factories;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using HRIS.Domain.Objectives.Entities;
using Souccar.Domain.Validation;
using Souccar.Domain.DomainModel;
using  Project.Web.Mvc4.Extensions;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;

namespace Project.Web.Mvc4.Areas.Objectives.Models
{
    public class ActionPlanViewModel:ViewModel
    {

        //public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults,
        //    CrudOperationType operationType, string customInformation = null)
        //{
        //    var objective = (HRIS.Domain.Objectives.RootEntities.Objective)
        //                                    typeof(HRIS.Domain.Objectives.RootEntities.Objective)
        //                                    .GetById(requestInformation.NavigationInfo.Previous[0].RowId);

        //    //Unique Number.
        //    var action = objective.ActionPlans.SingleOrDefault(x => x.Number == ((ActionPlan) entity).Number);
        //    if (action != null && action.Id != entity.Id)
        //        validationResults.Add(new ValidationResult() { Message = "Number already exist.", Property = typeof(ActionPlan).GetProperty("Number") });
        //    //Action planned dates <=> Objective planned dates.
        //    var currentActionPlan = (ActionPlan) entity;
        //    if (currentActionPlan.PlannedStartingDate < objective.PlannedStartingDate)
        //          validationResults.Add(new ValidationResult() { Message = "Planned starting date should to be between the objective planned dates", Property = typeof(ActionPlan).GetProperty("PlannedStartingDate") });
        //    if (currentActionPlan.PlannedClosingDate > objective.PlannedClosingDate)
        //        validationResults.Add(new ValidationResult() { Message = "Planned closing date should to be between the objective planned dates", Property = typeof(ActionPlan).GetProperty("PlannedClosingDate") });
        //}

        //public override void AfterInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        //{
        //    var actionPlan = (ActionPlan)entity;
        //    //Address the (created at) case.
        //    var isTrackPhase = actionPlan.Objective.ActionPlans.Any(y =>
        //                                                             y.Status == ActionPlanStatus.InProgress ||
        //                                                             y.Status==ActionPlanStatus.Accepted ||
        //                                                             y.Status == ActionPlanStatus.Closed);
        //    actionPlan.CreatedAt = isTrackPhase ? ActionPlanFlag.Track : ActionPlanFlag.Phase;
        //    actionPlan.Status = isTrackPhase ? ActionPlanStatus.InProgress : ActionPlanStatus.Pending;
        //    actionPlan.Save();
        //}

        //public override void BeforeDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        //{
        //    var actionPlan = (ActionPlan)entity;
        //    //Be at the first instructions.
        //    //Prevent if evaluating right now.
        //    if (WorkflowService.PreventedActionPlanOperation(actionPlan))
        //    {
        //        PreventDefault = true;
        //        throw new Exception("You can't delete action plan evaluating right now.");
        //    }
        //}

        //public override void BeforeUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, string customInformation = null)
        //{
        //    var actionPlan = (ActionPlan)entity;
        //    //Be at the first instructions.
        //    //Prevent if evaluating right now.
        //    if (WorkflowService.PreventedActionPlanOperation(actionPlan))
        //    {
        //        PreventDefault = true;
        //        throw new Exception("You can't update action plan evaluating right now.");
        //    }
        //}

        //public static void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        //{
        //    #region Hidden Fields

        //    model.SchemaFields.SingleOrDefault(x => x.Name == "ActualStartingDate").Editable = false;

        //    model.SchemaFields.SingleOrDefault(x => x.Name == "ActualClosingDate").Editable = false;

        //    model.SchemaFields.SingleOrDefault(x => x.Name == "CreatedAt").Editable = false;

        //    model.SchemaFields.SingleOrDefault(x => x.Name == "Status").Editable = false;

        //    #endregion

        //    #region Added Fields

        //    GridViewModelFactory.AddRefField(model, "Owner");

        //    #endregion

        //    #region Add to action list

        //    //Add to action list commands
        //    model.ActionList.Commands.Add(new ActionListCommand() { GroupId = 1, Order = 1, HandlerName = "createTrackingWindow", Name = "Track", ShowCommand = true });
        //    model.ActionList.Commands.Add(new ActionListCommand() { GroupId = 1, Order = 2, HandlerName = "closeActionPlan", Name = "Close", ShowCommand = true });

        //    #endregion


        //}



    }
}