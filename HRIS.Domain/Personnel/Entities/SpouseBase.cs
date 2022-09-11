using HRIS.Domain.Personnel.Helpers;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Personnel.Entities
{
    public class SpouseBase: Entity, IAggregateRoot
    {
       
        #region SpouseInfo
        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.SpouseInfo, Order = 5)]
        public virtual string IdentificationNo { get; set; }

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.SpouseInfo, Order = 10)]
        public virtual string FirstName { get; set; }

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.SpouseInfo, Order = 20)]
        public virtual string LastName { get; set; }

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.SpouseInfo, Order = 30)]
        public virtual string FatherName { get; set; }

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.SpouseInfo, Order = 35)]
        public virtual string MatherName { get; set; }

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.SpouseInfo, Order = 40)]
        public virtual DateTime? DateOfBirth { get; set; }

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.SpouseInfo, Order = 50)]
        public virtual Country PlaceOfBirth { get; set; }

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.SpouseInfo, Order = 60)]
        public virtual Nationality Nationality { get; set; }

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.SpouseInfo, Order = 70)]
        public virtual string ResidencyNo { get; set; }

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.SpouseInfo, Order = 80)]
        public virtual DateTime? ResidencyExpiryDate { get; set; }

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.SpouseInfo, Order = 90)]
        public virtual string PassportNo { get; set; }

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.SpouseInfo, Order = 100)]
        public virtual DateTime? PassportExpiryDate { get; set; }

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.SpouseInfo, Order = 110)]
        public virtual string FirstContactNumber { get; set; }

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.SpouseInfo, Order = 120)]
        public virtual string SecondContactNumber { get; set; }

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.SpouseInfo, Order = 130)]
        public virtual string Email { get; set; }

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.SpouseInfo, Order = 135,IsHidden=true)]
        public virtual bool HasChildBenefit { get; set; }

        [UserInterfaceParameter(IsReference = true, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.SpouseInfo, Order = 136, IsHidden = true)]
        public virtual DateTime? DateOfFamilyBenefitActivation { get; set; }


        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.SpouseInfo, Order = 140)]
        public virtual string Note { get; set; }
        #endregion 

        #region MarriageInfo
        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.MarriageInfo, Order = 145)]
        public virtual int Order { get; set; }

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.MarriageInfo, Order = 150)]
        public virtual DateTime MarriageDate { get; set; }

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.MarriageInfo, Order = 155)]
        public virtual bool IsDivorce { get; set; }

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.MarriageInfo, Order = 160)]
        public virtual DateTime? DivorceDate { get; set; }

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.MarriageInfo, Order = 170)]
        public virtual bool IsDeath { get; set; }

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.MarriageInfo, Order = 180)]
        public virtual DateTime? DeathDate { get; set; }

        #endregion

        #region JobInfo

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.JobInfo, Order = 170)]
        public virtual bool HasJob { get; set; }

      

    
        [UserInterfaceParameter(IsReference = true, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.JobInfo, Order = 190)]
        public virtual GlobalJobTitle JobTitle { get; set; }

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.JobInfo, Order = 200)]
        public virtual string CompanyName { get; set; }

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.JobInfo, Order = 210)]
        public virtual string WorkAddress { get; set; }

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.JobInfo, Order = 220)]
        public virtual string WorkPhone { get; set; }

        [UserInterfaceParameter(Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.JobInfo, Order = 230)]
        public virtual string WorkEmail { get; set; }

        #endregion
        [UserInterfaceParameter(Order = 200, IsHidden = true)]
        public virtual string NameForDropdown { get { return string.Format("{0} {1}", FirstName, LastName); } }
    }
}
