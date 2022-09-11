#region

using System;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Recruitment.RootEntities;
using Souccar.Domain.DomainModel;
using Souccar.Core.CustomAttribute;

#endregion

namespace HRIS.Domain.Recruitment.Entities
{
    public class REducation : EducationBase
    {

        //public virtual bool IsCertificateModified { get; set; }

        //public virtual bool EditedDocument
        
        public virtual Applicant Applicant { get; set; }
    }
}