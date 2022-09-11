#region

using System;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.Helpers;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Recruitment.RootEntities;
using Souccar.Domain.DomainModel;
using Souccar.Core.CustomAttribute;

#endregion

namespace HRIS.Domain.Recruitment.Entities
{
    public class RJobRelatedInfo : JobRelatedInfoBase
    {
        public virtual Applicant Applicant { get; set; }
    }
}