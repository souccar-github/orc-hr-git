using Project.Web.Mvc4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Web.Mvc4.Models.GridModel;

namespace Project.Web.Mvc4.Areas.Personnel.Models
{
    public class LoanPaymentViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ActionList.Commands.RemoveAt(2);
            model.ActionList.Commands.RemoveAt(1);
            model.ToolbarCommands.RemoveAt(0);
            model.Views[0].ViewHandler = "onViewLoanPayment";
        }
    }
}