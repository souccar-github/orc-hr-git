using System;
using System.Linq;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class ContractorLeaveSettingViewModel : ViewModel
    {

       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(ContractorLeaveSettingViewModel).FullName;
            model.Views[0].Columns.Single(x => x.FieldName == "DueBalance").Hidden = true;
            model.SchemaFields.Single(x => x.Name == "DueBalance").Editable = false;
            model.Views[0].Columns.Single(x => x.FieldName == "PastDueOfEmploymentPeriod").Hidden = true;
            model.SchemaFields.Single(x => x.Name == "PastDueOfEmploymentPeriod").Editable = false;
            model.Views[0].Columns.Single(x => x.FieldName == "IsContinuous").Hidden = true;
            model.SchemaFields.Single(x => x.Name == "IsContinuous").Editable = false;
            model.Views[0].Columns.Single(x => x.FieldName == "NumberOfIntervalDays").Hidden = true;
            model.SchemaFields.Single(x => x.Name == "NumberOfIntervalDays").Editable = false;
        }
    }
}