using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Domain.Validation;
using System.Web.Mvc;
using Souccar.Web.Mvc.JsonNet;
using Project.Web.Mvc4.Models.GridModel;
using Project.Web.Mvc4.Models.MasterDetailModels.DetailGridModels;

namespace Project.Web.Mvc4.Models
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public  class ViewModel
    {
        public ViewModel()
        {
            PreventDefault = false;
        }

        public virtual void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            

        }
        public virtual void CustomizeDetailGridModelForMasterDetail(GridViewModel model, Type type, RequestInformation requestInformation)
        {


        }
        public bool PreventDefault { get; set; }

       
    
      
        public virtual void AfterRead(RequestInformation requestInformation, DataSourceResult result, int pageSize = 10, int skip = 0)
        {
        }

        public virtual void BeforeRead(RequestInformation requestInformation)
        {
        }

        public virtual void AfterInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
        }
        public virtual void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
        }
        public virtual void AfterUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, string customInformation = null)
        {
        }
        public virtual void AfterUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, string customInformation = null, IList<DetailData> Details = null)
        {
        }
        public virtual void BeforeUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, string customInformation = null)
        {
        }

        public virtual ActionResult BeforeCreate(RequestInformation requestInformation, string customInformation = null)
        {
            var msg = "";
            
            return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new  { Data = true, message = msg });
        }

        public virtual void AfterDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
        }
        public virtual void BeforeDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
        }

        public virtual void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
        }
        public virtual void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null, IList<DetailData> Details = null)
        {
        }
        public virtual void BeforeValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, CrudOperationType operationType, string customInformation = null)
        {
        }

        public virtual void AfterRead()
        {
        }

        public virtual void BeforeRead()
        {
        }

        public virtual void AfterInsert(Entity entity)
        {
        }
        public virtual void BeforeInsert(Entity entity)
        {
        }
        public virtual void AfterUpdate(Entity entity)
        {
        }
        public virtual void BeforeUpdate(Entity entity)
        {
        }

        public virtual ActionResult BeforeCreate()
        {
            var msg = "";

            return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { Data = true, message = msg });
        }

        public virtual void AfterDelete(Entity entity)
        {
        }
        public virtual void BeforeDelete(Entity entity)
        {
        }

        public virtual void AfterValidation(Entity entity, IList<ValidationResult> validationResults)
        {
        }
        public virtual void BeforeValidation(Entity entity)
        {
        }
    }
}