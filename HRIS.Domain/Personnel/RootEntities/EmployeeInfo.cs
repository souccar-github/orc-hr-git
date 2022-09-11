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

namespace HRIS.Domain.Personnel.RootEntities
{
    public class EmployeeInfo : Entity, IAggregateRoot
    {

        public virtual int EmployeeId { get; set; }

        public virtual int? NodeId { get; set; }

        public virtual string EmployeeNumber { get; set; }

        public virtual string CardNumber { get; set; }

        public virtual string FirstName { get; set; }
        
        public virtual string LastName { get; set; }
        
        public virtual string FatherName { get; set; }

        public virtual string FirstNameL2 { get; set; }
       
        public virtual string LastNameL2 { get; set; }
       
        public virtual string FatherNameL2 { get; set; }

        public virtual int Gender { get; set; }

        public virtual int Status { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

         public virtual int ProcessType { get; set; }

         public virtual int Sync { get; set; }
        

    }
}