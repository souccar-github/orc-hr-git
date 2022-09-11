using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.AttendanceSystem.Configurations;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.EventHandlers
{
    public class NonAttendanceSliceViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(NonAttendanceSliceViewModel).FullName;
        }
        
    }
}
