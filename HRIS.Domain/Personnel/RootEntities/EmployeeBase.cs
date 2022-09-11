using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.Helpers;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Personnel.RootEntities
{
    public class EmployeeBase : Entity, IAggregateRoot
    {
        public override string ToString()
        {
            return string.Format("Employee: {0}", NameForDropdown);
        }

        #region PersonalInformation
        [UserInterfaceParameter(IsImageColumn = true, ImageColumnPath = "Content/EmployeesPhoto/", DefaultImageName = "placeholder.jpg", Order = 1, Width = 60, IsHidden = false)]
        public virtual string PhotoPath
        {
            get { return PhotoId; }
        }

        /// <summary>
        ///   Name Details In First Language
        /// </summary>
        /// 
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 10)]
        public virtual string FirstName { get; set; }
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 20)]
        public virtual string LastName { get; set; }
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 30)]
        public virtual string FatherName { get; set; }
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 40)]
        public virtual string MotherName { get; set; }
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 45)]
        public virtual string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string TripleName
        {
            get { return string.Format("{0} {1} {2}", FirstName, FatherName, LastName); }
        }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 46)]
        public virtual City PlaceOfBirth { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 47)]
        public virtual DateTime DateOfBirth { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 48)]
        public virtual int Age
        {
            get
            {
                var now = DateTime.Today;
                var age = now.Year - DateOfBirth.Year;
                if (now < DateOfBirth.AddYears(age))
                    age--;
                return age;
            }
        }


        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 49)]
        public virtual string IdentificationNo { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 51, IsNonEditable = true)]
        public virtual string Code { get; set; }


        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 52)]
        public virtual Country CountryOfBirth { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 55)]
        public virtual string PersonalRecordSource { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 57)]
        public virtual string CivilRecordPlaceAndNumber { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 60)]
        public virtual Gender Gender { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 62)]
        public virtual Religion Religion { get; set; }

        /// <summary>
        ///   Name Details In Second Language
        /// </summary>        
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 70)]
        public virtual string FirstNameL2 { get; set; }
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 80)]
        public virtual string LastNameL2 { get; set; }
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 90)]
        public virtual string FatherNameL2 { get; set; }
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 100)]
        public virtual string MotherNameL2 { get; set; }
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 103)]
        public virtual string FullNameL2
        { get { return FirstNameL2 + " " + LastNameL2; } }
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 105)]
        public virtual string PlaceOfBirthL2 { get; set; }



        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 150)]
        public virtual bool DisabilityExist { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 160)]
        public virtual DisabilityType DisabilityType { get; set; }
        //public virtual string DisabilityDescription { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 165)]
        public virtual BloodType BloodType { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 170)]
        public virtual Nationality Nationality { get; set; }


        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 185)]
        public virtual bool OtherNationalityExist { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 190)]
        public virtual Nationality OtherNationality { get; set; }

     
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 230)]
        public virtual MilitaryStatus MilitaryStatus { get; set; }

        #endregion

        #region ContactInformation
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.ContactInformation, Order = 280)]
        public virtual string Address { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.ContactInformation, Order = 290)]
        public virtual string POBox { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.ContactInformation, Order = 300)]
        public virtual string Mobile { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.ContactInformation, Order = 310)]
        public virtual string Phone { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.ContactInformation, Order = 320)]
        public virtual string WebSite { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.ContactInformation, Order = 330, Width = 200)]
        public virtual string Email { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.ContactInformation, Order = 350)]
        public virtual string Facebook { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.ContactInformation, Order = 360)]
        public virtual string Twitter { get; set; }

        #endregion

        #region MaritalStatus

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.FamilyInformation, Order = 260)]
        public virtual MaritalStatus MaritalStatus { get; set; }
        #endregion
       
        #region General
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string PhotoId { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return FullName; } }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.General, IsHidden = true, Order = 380)]
        public virtual EmployeeStatus Status { get; set; }

        #endregion
    }
}
