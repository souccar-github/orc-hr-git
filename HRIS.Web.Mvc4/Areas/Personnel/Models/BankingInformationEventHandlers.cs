using System;
using  Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;


namespace Project.Web.Mvc4.Areas.Personnel.Models
{
    public class BankingInformationEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            
            model.ViewModelTypeFullName = typeof(BankingInformationEventHandlers).FullName;
            model.Views[0].EditHandler = "BankingInfoEditHandler";
        }

     
    }
}