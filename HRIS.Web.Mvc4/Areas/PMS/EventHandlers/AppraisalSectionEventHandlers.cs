using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.Entities.Organizational;
using HRIS.Domain.PMS.RootEntities;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Helpers.Resource;
using Souccar.Infrastructure.Extenstions;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Helpers.DomainExtensions;

namespace Project.Web.Mvc4.Areas.PMS.EventHandlers
{
    public class AppraisalSectionEventHandlers : ViewModel
    {
        public int Id { get; set; }
        public virtual IList<SectionItemViewModel> Items { get; set; }

        public AppraisalSectionEventHandlers()
        {
            Items = new List<SectionItemViewModel>();
        }

        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            requestInformation.NavigationInfo.Next.Clear();

            //Show Windows with Two Columns
            model.Views[0].ShowTwoColumns = true;

            model.Views[0].EditHandler = "AppraisalSectionEditHandler";
            model.Views[0].ViewHandler = "AppraisalSectionItemKpiViewHandler";
            //Call Event Handlers
            model.ViewModelTypeFullName = typeof(AppraisalSectionEventHandlers).FullName;
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var temp = new JavaScriptSerializer();
            var sectionViewModel = temp.Deserialize<AppraisalSectionEventHandlers>(customInformation);
            if (sectionViewModel.Items.Sum(x => x.Weight) != 100)
            {
                validationResults.Add(new ValidationResult() { Message = GlobalResource.InalidWeightSumMessage, Property = null });
            }
            if (sectionViewModel.Items.Any(x => x.Name == ""))
            {
                validationResults.Add(new ValidationResult() { Message = IncentiveLocalizationHelper.GetResource(IncentiveLocalizationHelper.InvalidItemsName), Property = null });//todo
            }
            var appraisalSection = (entity as AppraisalSection);
            var savedAppraisalSection = ServiceFactory.ORMService.All<AppraisalSection>().Where(x => x.Name == appraisalSection.Name).FirstOrDefault();
               
            if (savedAppraisalSection !=null && savedAppraisalSection.Id != entity.Id)
            {
                var prop = typeof(AppraisalSection).GetProperty("Name");
                validationResults.Add(new ValidationResult() { Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.AlreadyexistMessage), Property = prop });
            }
        }

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity,string customInformation = null)
        {
            var temp = new JavaScriptSerializer();
            var start = DateTime.Now;
            var sectionViewModel = temp.Deserialize<AppraisalSectionEventHandlers>(customInformation);
            var section = (AppraisalSection)entity;
            foreach (var item in sectionViewModel.Items)
            {
                var newItem = new AppraisalSectionItem() { Name = item.Name, Description = item.Description,Weight = item.Weight};
                foreach (var kpi in item.Kpis)
                {
                    var newKpi = new AppraisalSectionItemKpi() { Description = kpi.Description, Value = kpi.Value, Weight = kpi.Weight };
                    newItem.AddKpi(newKpi);
                }
                section.AddSectionItem(newItem);

            }
          //  section.Save();
            var entities = new List<IAggregateRoot>();
            entities.Add(section);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AddSection);

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, section, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() {  });

            PreventDefault = true;
        }
        public override void BeforeUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, string customInformation = null)
        {
            var temp = new JavaScriptSerializer();
            var start = DateTime.Now;
            var sectionViewModel = temp.Deserialize<AppraisalSectionEventHandlers>(customInformation);
            var section = (AppraisalSection)entity;
            foreach (var item in sectionViewModel.Items.Where(x => x != null && x.Id == 0))
            {
                var newItem = new AppraisalSectionItem() { Name = item.Name, Description = item.Description,Weight = item.Weight};
                foreach (var kpi in item.Kpis)
                {
                    var newKpi = new AppraisalSectionItemKpi() { Description = kpi.Description, Value = kpi.Value, Weight = kpi.Weight };
                    newItem.AddKpi(newKpi);
                }
                section.AddSectionItem(newItem);

            }
            var removeIndexs = new List<int>();
            var index = 0;
            foreach (var item in section.Items)
            {
                if (item.Id==0)
                {
                    continue;
                }
                var itemViewModel = sectionViewModel.Items.SingleOrDefault(x => x.Id == item.Id);
                if (itemViewModel == null)
                    removeIndexs.Add(index);
                else
                    updateSection(section, item, itemViewModel);
                index++;
            }
            for (var i = removeIndexs.Count - 1; i >= 0; i--)
            {
                section.Items.RemoveAt(removeIndexs[i]);
            }
          //  section.Save();
            var entities = new List<IAggregateRoot>();
            entities.Add(section);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.UpdateSection);

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, section, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { });

            PreventDefault = true;
        }

        private void updateSection(AppraisalSection section, AppraisalSectionItem item, SectionItemViewModel itemViewModel)
        {
            item.Description = itemViewModel.Description;
            item.Name = itemViewModel.Name;
            item.Weight = itemViewModel.Weight;
            item.Kpis.Clear();
            foreach (var kpi in itemViewModel.Kpis)
            {
                var newKpi = new AppraisalSectionItemKpi() { Description = kpi.Description, Value = kpi.Value, Weight = kpi.Weight };
                item.AddKpi(newKpi);
            }
        }

    }

    public class SectionItemViewModel
    {
        public SectionItemViewModel()
        {
            Kpis = new List<SectionItemKpiViewModel>();
        }
        public int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual float Weight { get; set; }
        public virtual IList<SectionItemKpiViewModel> Kpis { get; set; }

    }

    public class SectionItemKpiViewModel
    {
        public int Id { get; set; }
        public virtual float Weight { get; set; }
        public virtual int Value { get; set; }
        public virtual string Description { get; set; }
    }
}