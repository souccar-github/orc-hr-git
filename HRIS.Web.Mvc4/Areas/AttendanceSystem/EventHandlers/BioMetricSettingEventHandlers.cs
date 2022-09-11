using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Infrastructure.Core;
using HRIS.Validation.MessageKeys;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.EventHandlers
{
    public class BioMetricSettingEventHandlers:ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ToolbarCommands.Add(
              new ToolbarCommand
              {
                  Additional = false,
                  ClassName = "grid-action-button SyncSupportedBioMetricDevicesButton",
                  Handler = "",
                  ImageClass = "",
                  Name = "SyncSupportedBioMetricDevicesButton",
                  Text =ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(CustomMessageKeysAttendanceSystemModule.SyncDevicesButton))
              });

            model.Views[0].AfterRequestEnd = "BioMetricSettingAfterRequestEnd";
            model.ViewModelTypeFullName = typeof(BioMetricSettingEventHandlers).FullName;
        }
    }
}