using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Personnel.Enums;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class RecycledLeaveViewModel : ViewModel
    {

        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(RecycledLeaveViewModel).FullName;


        }

        public override System.Web.Mvc.ActionResult BeforeCreate(RequestInformation requestInformation, string customInformation = null)
        {
            var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);
            if (employeeCard.CardStatus != EmployeeCardStatus.OnHeadOfHisWork)

                return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new
                {
                    Data = false,
                    message =
                    EmployeeRelationServicesLocalizationHelper.GetResource(
                        employeeCard.CardStatus == EmployeeCardStatus.New ?
                        EmployeeRelationServicesLocalizationHelper.TheEmployeeWhoYouHaveSelectedIsNew :
                    EmployeeRelationServicesLocalizationHelper.TheEmployeeWhoYouHaveSelectedIsResignedOrTerminated)
                });
            else
                return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { Data = true, message = "" });
        }
        public override void AfterValidation(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, IDictionary<string, object> originalState, IList<Souccar.Domain.Validation.ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Souccar.Domain.DomainModel.Entity parententity = null)
        {
            var recycledleave = (RecycledLeave)entity;
            var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);

            if (recycledleave.LeaveSetting.RoundPercentage == 0)
            {
                validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouCanNotRecycleThisLeaveBecauseRoundPercentageIsZero),
                    Property = null
                });

                return;
            }
            if (employeeCard.StartWorkingDate.Value.Year > recycledleave.Year)
            {
                validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TheEmployeeStartWorkingDateIsGraterThanYear),
                    Property = null

                });

                return;
            }
            var recycledleaveCount = employeeCard.RecycledLeaves.Where(x => x.LeaveSetting == recycledleave.LeaveSetting).Count(x => x.Year == recycledleave.Year);
            if (operationType == CrudOperationType.Insert)
            {
                if (recycledleaveCount > 0)
                {
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgCannotAddMoreRecycleForThisLeaveSettingInYear),
                        Property = null

                    });

                    return;
                }

            }
            if (operationType == CrudOperationType.Update)
            {
                var yearbeforeupdate = (int)originalState["Year"];
                IDictionary<string, object> IDictionaryLeaveSettingbefore = (IDictionary<string, object>)originalState["LeaveSetting"];
                var LeaveSettingbeforeid = (int)IDictionaryLeaveSettingbefore["Id"];
                var LeaveSettingbefore = ServiceFactory.ORMService.GetById<LeaveSetting>(LeaveSettingbeforeid);
               
                if (yearbeforeupdate != recycledleave.Year || LeaveSettingbefore != recycledleave.LeaveSetting)
                {
                    recycledleaveCount = employeeCard.RecycledLeaves.Where(x => x.LeaveSetting == LeaveSettingbefore).Count(x => x.Year == yearbeforeupdate);
                    if (recycledleaveCount > 0)
                    {
                        validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                        {
                            Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgCannotAddMoreRecycleForThisLeaveSettingInYear),
                            Property = null

                        });

                        return;
                    }

                }

            }
            
        }

    }
}