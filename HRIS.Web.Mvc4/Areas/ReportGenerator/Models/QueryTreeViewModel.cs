using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Domain.DomainModel;
using Project.Web.Mvc4.Models.GridModel;
using Project.Web.Mvc4.Models;

namespace project.Web.Mvc4.Areas.ReportGenerator.Models
{
    public class QueryTreeViewModel:ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            //add
            model.ViewModelTypeFullName = typeof(QueryTreeViewModel).FullName;
            model.Views[0].EditHandler = "QueryTreeEditHandler"; 
        }

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity,
            string customInformation = null)
        {
            this.PreventDefault = true;
        }

    }
}