using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.Recruitment.RootEntities;
using  Project.Web.Mvc4.Helpers.Resource;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;

namespace Project.Web.Mvc4.Areas.Recruitment.Models
{
    public class AdvertisementViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            //add
            model.ViewModelTypeFullName = typeof(AdvertisementViewModel).FullName;
            model.Views[0].EditHandler = "AdvertisementEditHandler";
            model.Views[0].Columns.SingleOrDefault(x => x.FieldName == "RecruitmentType").ReadUrl = "Recruitment/Reference/ReadRecruitmentType";

            #region // Actions List //

            //model.ActionList.Commands.Add(new ActionListCommand() { GroupId = 1, Order = 2, HandlerName = "RecruitmentCancellation", 
            //    Name = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.RecruitmentCancellation), ShowCommand = true });

            #endregion
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<Souccar.Domain.Validation.ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var advertisement = (Advertisement)entity;

            if ((advertisement.EndingDate - advertisement.StartingDate).Days < 15)
            {
                validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                {
                    Message =RecruitmentLocalizationHelper.GetResource( RecruitmentLocalizationHelper.MsgDifferenceBetweenStartDateEndDateShouldBeGreaterThanFifteen),
                    Property = null
                });

                return;
            }
        }
        

    }
}