#region

using System;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.Helpers;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Domain.DomainModel;
using Souccar.Core.CustomAttribute;

#endregion

namespace HRIS.Domain.Personnel.Entities
{
    public class JobRelatedInfo :JobRelatedInfoBase
    {
       

        public virtual Employee Employee { get; set; }
    }
}