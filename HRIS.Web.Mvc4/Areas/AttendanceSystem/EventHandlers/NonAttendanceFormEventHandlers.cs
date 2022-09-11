using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using System.Web.Script.Serialization;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using Souccar.Domain.Validation;
using Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.AttendanceSystem.Configurations;
using HRIS.Domain.AttendanceSystem.Helpers;
using HRIS.Validation.MessageKeys;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.EventHandlers
{
    public class NonAttendanceFormEventHandlers : ViewModel
    {

        public int Id { get; set; }
        public virtual IList<SectionSliceItemViewModel> Items { get; set; }

        public NonAttendanceFormEventHandlers()
        {
            Items = new List<SectionSliceItemViewModel>();
        }

        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            requestInformation.NavigationInfo.Next.Clear();

            //Show Windows with Two Columns
            model.Views[0].ShowTwoColumns = true;

            model.Views[0].EditHandler = "NonAttendanceFormEditHandler";
            model.Views[0].ViewHandler = "NonAttendanceFormViewHandler";
            //Call Event Handlers
            model.ViewModelTypeFullName = typeof(NonAttendanceFormEventHandlers).FullName;
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var NonAttendanceForm=(NonAttendanceForm)entity;
            var temp = new JavaScriptSerializer();
            var sectionViewModel = temp.Deserialize<NonAttendanceFormEventHandlers>(customInformation);


            NonAttendanceForm NonAttendanceFormExist = null;

            NonAttendanceFormExist = ServiceFactory.ORMService.All<NonAttendanceForm>().Where(a => a.Number == NonAttendanceForm.Number && a.Id != NonAttendanceForm.Id).FirstOrDefault();
            if (NonAttendanceFormExist != null)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                              .GetFullKey(CustomMessageKeysAttendanceSystemModule.ThereAreAnotherNonAttendanceTemplateWithThisNumber)),
                    Property = null
                });
                return;
            }

            var NonAttendanceSliceCount = originalState == null ? sectionViewModel.Items.Count(x => x != null && x.Id == 0) : sectionViewModel.Items.Count(x => x != null );

            if (NonAttendanceSliceCount == 0)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                            .GetFullKey(
                                CustomMessageKeysAttendanceSystemModule
                                    .YouMustAddOneNonAttendanceSliceAtLeast)),
                    Property = null
                 
                });
            }
    
        }

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var temp = new JavaScriptSerializer();
            var sectionViewModel = temp.Deserialize<NonAttendanceFormEventHandlers>(customInformation);
            var section = (NonAttendanceForm)entity;
            foreach (var item in sectionViewModel.Items.Where(x => x != null && x.Id == 0))
            {
                var infractionForm = ServiceFactory.ORMService.GetById<InfractionForm>(item.InfractionForm.Id);
                var newItem = new NonAttendanceSlice() { StartSlice = item.StartSlice, InfractionForm = infractionForm, EndSlice = item.EndSlice, Value = item.Value };
                foreach (var Percentage in item.Percentages)
                {
                    var newPercentage = new NonAttendanceSlicePercentage() { Percentage = Percentage.Percentage, PercentageOrder = Percentage.PercentageOrder };
                    newItem.AddNonAttendanceSlicePercentage(newPercentage);
                }
                section.AddNonAttendanceSlice(newItem);

            }
            section.Save();
            PreventDefault = true;
        }
        public override void BeforeUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, string customInformation = null)
        {
            var temp = new JavaScriptSerializer();
            var sectionViewModel = temp.Deserialize<NonAttendanceFormEventHandlers>(customInformation);
            var section = (NonAttendanceForm)entity;
            foreach (var item in sectionViewModel.Items.Where(x => x != null && x.Id == 0))
            {

                var infractionForm = ServiceFactory.ORMService.GetById<InfractionForm>(item.InfractionForm.Id);
                var newItem = new NonAttendanceSlice() { StartSlice = item.StartSlice, InfractionForm = infractionForm, EndSlice = item.EndSlice, Value = item.Value };
                foreach (var Percentage in item.Percentages)
                {
                    var newPercentage = new NonAttendanceSlicePercentage() { Percentage = Percentage.Percentage, PercentageOrder = Percentage.PercentageOrder };
                    newItem.AddNonAttendanceSlicePercentage(newPercentage);
                }
                section.AddNonAttendanceSlice(newItem);

            }
            var removeIndexs = new List<int>();
            var index = 0;
            foreach (var item in section.NonAttendanceSlices)
            {
                if (item.Id == 0)
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
                section.NonAttendanceSlices.RemoveAt(removeIndexs[i]);
            }
            section.Save();
            PreventDefault = true;
        }

        public override void BeforeDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var nonAttendanceForm = (NonAttendanceForm)entity;
            var nonAttendanceSlices = nonAttendanceForm.NonAttendanceSlices;
            if (nonAttendanceSlices != null)
            {
                foreach (var nonAttendanceSlice in nonAttendanceSlices)
                {
                    nonAttendanceSlice.IsVertualDeleted = true;
                    var NonAttendanceSlicePercentage = nonAttendanceSlice.NonAttendanceSlicePercentages;
                    if (NonAttendanceSlicePercentage != null)
                    {
                        foreach (var nonAttendanceSlicePercentage in NonAttendanceSlicePercentage)
                        {
                            nonAttendanceSlicePercentage.IsVertualDeleted = true;
                        }
                    }
                }
            }
            PreventDefault = true;
            nonAttendanceForm.IsVertualDeleted = true;
            nonAttendanceForm.Save();
        }

        private void updateSection(NonAttendanceForm section, NonAttendanceSlice item, SectionSliceItemViewModel itemViewModel)
        {
            item.StartSlice = itemViewModel.StartSlice;
            item.EndSlice = itemViewModel.EndSlice;
            item.Value = itemViewModel.Value;
            item.InfractionForm = ServiceFactory.ORMService.GetById<InfractionForm>(itemViewModel.InfractionForm.Id);
            item.NonAttendanceSlicePercentages.Clear();
            foreach (var percentage in itemViewModel.Percentages)
            {
                var newPercentage = new NonAttendanceSlicePercentage() {PercentageOrder = percentage.PercentageOrder, Percentage = percentage.Percentage };
                item.AddNonAttendanceSlicePercentage(newPercentage);
            }
        } 

    }



    public class SectionSliceItemViewModel
    {
        public SectionSliceItemViewModel()
        {
            Percentages = new List<SectionPercentagesItemViewModel>();
       }
        public int Id { get; set; }
        public virtual int EndSlice { get; set; }
        public virtual InfractionForm InfractionForm { get; set; }
        public virtual NonAttendanceForm NonAttendanceForm { get; set; }
        public virtual IList<SectionPercentagesItemViewModel> Percentages { get; set; }
        public virtual int StartSlice { get; set; }
        public virtual int Value { get; set; }
    }

    public class SectionPercentagesItemViewModel
    {
        public int Id { get; set; }
        public virtual double Percentage { get; set; }
        public virtual int PercentageOrder { get; set; }
    }

}