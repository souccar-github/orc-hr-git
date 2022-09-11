#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 30/04/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference

using DevExpress.XtraCharts.Native;
using FluentNHibernate.Data;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.Personnel.RootEntities;
using  Project.Web.Mvc4.Areas.JobDescription.Helpers;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using Entity = Souccar.Domain.DomainModel.Entity;

#endregion
namespace Project.Web.Mvc4.Areas.Personnel.Models
{
    public class EmployeeCardViewModel : ViewModel
    {
public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(EmployeeCardViewModel).FullName;
       //     model.ToolbarCommands.RemoveAt(0);
            model.ActionListHandler = "initializeEmployeeCardActionList";
            model.ToolbarCommands.RemoveAt(0);
        }
        
    }
}