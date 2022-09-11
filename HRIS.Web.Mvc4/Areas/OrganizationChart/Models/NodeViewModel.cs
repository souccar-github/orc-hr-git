using Project.Web.Mvc4.Areas.OrganizationChart.Models;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.OrganizationChart.RootEntities;
using System.Web;

namespace Project.Web.Mvc4.Areas.OrganizationChart.Models
{
    public class NodeViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type,
           RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof (NodeViewModel).FullName;
            model.ActionListHandler = "Node_ActionListHandler";
            model.Views[0].ViewHandler = "NodeViewHandler";
            model.ToolbarCommands.RemoveAt(0);
        }
        public override void BeforeRead(RequestInformation requestInformation)
        {
            PreventDefault = true;
        }
        public override void AfterRead(RequestInformation requestInformation, DataSourceResult result, int pageSize = 10, int skip = 0)
        {
            var beforeFilter = ((IQueryable<Node>)result.Data).ToList();
            var validData = beforeFilter.Where(x => !x.IsHistorical).ToList();
            result.Data = validData.Skip<Node>(skip).Take<Node>(pageSize).AsQueryable();
            result.Total = validData.Count();
        }


    }
}