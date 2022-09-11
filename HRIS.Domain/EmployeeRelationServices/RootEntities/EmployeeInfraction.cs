#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 05/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;

using HRIS.Domain.JobDescription.Entities;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Domain.Security;
using Souccar.Infrastructure.Core;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.Global.Enums;
#endregion

namespace HRIS.Domain.EmployeeRelationServices.RootEntities
{
    /// <summary>
    /// مخالفة موظف
    /// </summary>
    [Module(ModulesNames.EmployeeRelationServices)]
    [Order(30)]
    public class EmployeeInfraction : Entity, IAggregateRoot
    {
        string pName;
        public EmployeeInfraction()
        {
            
            CreationDate = DateTime.Now;
            InfractionStatus = Status.Approved;
          

        }
        [UserInterfaceParameter(Order = 1,IsReference =true)]
        public virtual Employee Employee { get; set; }
        [UserInterfaceParameter(Order = 2, IsReference = true)]
        public virtual InfractionForm Infraction { get; set; }
        [UserInterfaceParameter(Order = 3)]
        public virtual string PenaltyName {
            get; set;
    
        
        }
        [UserInterfaceParameter(Order = 3,IsNonEditable =true)]
        public virtual Status InfractionStatus { get; set; }
        [UserInterfaceParameter(Order = 3, IsNonEditable = true)]
        public virtual DateTime CreationDate { get; set; }
        private int GetEmployeeInfractionCount()
        {
            var employeeInfractionCount = ServiceFactory.ORMService.All<EmployeeInfraction>().Where(x => x.Employee == Employee).Count();

           
            return employeeInfractionCount;
        }
    }
}
