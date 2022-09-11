using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.Validation;

namespace Project.Web.Mvc4.Areas.ProjectManagement.Models
{
    public class TRoleViewModel : ViewModel
    {
        //
        // GET: /ProjectManagement/TRoleViewModel/

        //public TRoleViewModel()
        //{
        //    Items = new List<TRoleItemViewModel>();
        //}
        
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(TaskViewModel).FullName;
        }

        //public override void AfterValidation(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, IDictionary<string, object> originalState, IList<Souccar.Domain.Validation.ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null)
        //{
        //    var temp = new JavaScriptSerializer();
        //    var taskViewModel = temp.Deserialize<TRoleViewModel>(customInformation);
        //    if (taskViewModel.Items.Sum(x => x.Weight) != 100)
        //    {
        //        validationResults.Add(new ValidationResult() { Message = GlobalResource.InalidWeightSumMessage, Property = null });
        //    }
        //}

        //public virtual IList<TRoleItemViewModel> Items { get; set; }
        //public class TRoleItemViewModel
        //{
        //    public int Id { get; set; }
        //    public virtual string Name { get; set; }
        //    public virtual float Weight { get; set; }

        //}
    }
}
