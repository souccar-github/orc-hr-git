#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.JobDesc.Entities;
using HRIS.Domain.Payroll.Entities;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Domain.Recruitment.Enums;
using HRIS.Domain.Recruitment.Helpers;
using HRIS.Domain.Recruitment.Indexes;
using HRIS.Domain.Recruitment.RootEntities;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Personnel.Helpers;
//using MaritalStatus = HRIS.Domain.Personnel.Indexes.MaritalStatus;
using Souccar.Infrastructure.Core;
using Souccar.NHibernate;
using LinqToExcel.Attributes;

#endregion
namespace HRIS.Domain.Recruitment.Configurations
{
     [Module(ModulesNames.Recruitment)]
      [Order(4)]
    public class RoundSetting : Entity, IConfigurationRoot
    {


       
         public RoundSetting() { }



        [UserInterfaceParameter(Order = 1)]
        public virtual int MonthCountForRoundToYear { set; get; }




        [UserInterfaceParameter(Order = 2)]
        public virtual int DaysCountForRoundtoMonth { set; get; }



    }
}
