#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 26/02/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Global.Entities;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.Helpers;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
namespace HRIS.Domain.Personnel.Configurations
{
    [Module(ModulesNames.Personnel)]
    [Order(2)]
    public class EmployeeCodeSetting : CodeSetting, IConfigurationRoot
    {     
    }
}
