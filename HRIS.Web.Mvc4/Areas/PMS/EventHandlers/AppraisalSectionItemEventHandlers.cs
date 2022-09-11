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
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.RootEntities;
using HRIS.Validation.MessageKeys;
using  Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Souccar.NHibernate;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;


namespace Project.Web.Mvc4.Areas.PMS.EventHandlers
{
    public class AppraisalSectionItemEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(AppraisalSectionItemEventHandlers).FullName;

            //Aggregates
            model.Views.FirstOrDefault().ServerAggregates = false;
            model.Views.FirstOrDefault().ServerPaging = false;
            model.Views.FirstOrDefault().Columns.SingleOrDefault(x => x.FieldName == "Weight").AddGlobalAggregate(AggregatesType.Max);
            model.Views.FirstOrDefault().Columns.SingleOrDefault(x => x.FieldName == "Weight").AddGlobalAggregate(AggregatesType.Sum);
            model.Views.FirstOrDefault().Columns.SingleOrDefault(x => x.FieldName == "Weight").FooterTemplate = "Max: #:max# , Sum: #:sum#";
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var masterRowId = requestInformation.NavigationInfo.Previous[0].RowId;
            var appraisalSectionItem = (AppraisalSectionItem)entity;
            var appraisalSection = (AppraisalSection)typeof(AppraisalSection).GetById(masterRowId);
            var sumWeight = appraisalSection.Items.Where(x => x.Id != entity.Id).Sum(x => x.Weight);
            sumWeight += appraisalSectionItem.Weight;

            if (sumWeight > 100)
            {
                validationResults.Add(new ValidationResult
                {
                    Property = typeof(AppraisalSectionItem).GetProperty("Weight"),
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPmsModule.GetFullKey(CustomMessageKeysPmsModule.TotalSumWeight))
                });
            }
        }
    }
}
