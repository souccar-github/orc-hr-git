using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.RootEntities;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Domain.DomainModel;

using System.IO;

namespace Project.Web.Mvc4.Areas.Personnel.Models
{
    public class AttachmentViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(AttachmentViewModel).FullName;
        }


    }
}