#region

using System;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.Helpers;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Recruitment.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Personnel.Enums;

#endregion

namespace HRIS.Domain.Recruitment.Entities
{
    public class RChild : ChildBase
    {
        [UserInterfaceParameter(Order = 25, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildInfo)]
        public virtual string FatherName
        {
            get
            {
                if (Applicant == null)
                    return "";
                if (Applicant.Gender == Gender.Male)
                    return Applicant.FullName;
                return Spouse == null ? "" : string.Format("{0} {1}", Spouse.FirstName, Spouse.LastName);
            }
        }
        [UserInterfaceParameter(Order = 27, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildInfo)]
        public virtual string MatherName
        {
            get
            {
                if (Applicant == null)
                    return "";
                if (Applicant.Gender == Gender.Female)
                    return Applicant.FullName;
                return Spouse == null ? "" : string.Format("{0} {1}", Spouse.FirstName, Spouse.LastName);
            }
        }

        [UserInterfaceParameter(Order = 30, IsReference = true, ReferenceReadUrl = "Recruitment/Reference/ReadSpouseForChild", Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildInfo)]
        public virtual RSpouse RSpouse { get; set; }

        public virtual Applicant Applicant { get; set; }
    }
}