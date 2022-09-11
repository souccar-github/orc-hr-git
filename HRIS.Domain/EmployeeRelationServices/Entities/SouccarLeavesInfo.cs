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

namespace HRIS.Domain.EmployeeRelationServices.Entities
{

    public class SouccarLeavesInfo : Entity, IAggregateRoot
    {

        public virtual int LeaveId { get; set; }

        public virtual int EmployeeId { get; set; }

        public virtual DateTime? FromHour { get; set; }

        public virtual DateTime? ToHour { get; set; }

        //public virtual float Duration { get; set; }

        public virtual int Type { get; set; }

        public virtual int ProcessType { get; set; }

         public virtual int Sync { get; set; }
        

    }
}