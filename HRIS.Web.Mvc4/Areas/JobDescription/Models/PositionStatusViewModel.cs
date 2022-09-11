using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using FluentNHibernate.Utils;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using  Project.Web.Mvc4.Areas.Grades.Models;
using  Project.Web.Mvc4.Factories;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Extensions;
using  Project.Web.Mvc4.Models.GridModel;
using  Project.Web.Mvc4.Helpers.Resource;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.JobDescription.Entities;
using  Project.Web.Mvc4.Areas.JobDescription.Helpers;
using Souccar.Core.Extensions;
using Souccar.Core.Fasterflect;
using Souccar.Core.Utilities;
using Souccar.Infrastructure.Core;
using HRIS.Domain.JobDescription.RootEntities;
using Souccar.Domain.DomainModel;

using Souccar.Domain.Validation;
using  Project.Web.Mvc4.Helpers;
namespace Project.Web.Mvc4.Areas.JobDescription.Models
{
    
    public class PositionStatusViewModel : ViewModel
    {
        //
        // GET: /JobDescription/PositionStatusViewModel/

       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(PositionStatusViewModel).FullName;
            model.ActionListHandler = "initializeJobDesActionList";
            model.Views[0].ViewHandler = "PositionStatusViewHandler";
           model.ToolbarCommands.RemoveAt(0);
           
        }

    }
}
