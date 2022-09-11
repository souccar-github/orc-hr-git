using System;
using System.Linq;
using HRIS.Domain.PayrollSystem.RootEntities;
using  Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;

namespace Project.Web.Mvc4.Areas.PayrollSystem.EventHandlers
{
    public class TravelCategoryEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(TravelCategoryEventHandlers).FullName;
            model.SchemaFields.SingleOrDefault(x => x.Name == "Number").Editable = false;
        }

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var travelCategory = (TravelCategory)entity;

            if (!typeof(TravelCategory).GetAll<TravelCategory>().Any())
            {
                travelCategory.Number = 1;
            }
            else
            {
                travelCategory.Number = typeof(TravelCategory).GetAll<TravelCategory>().Max(x => x.Number) + 1;
            }
        }
    }
}