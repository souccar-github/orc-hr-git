#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 26/02/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.Helpers;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
namespace HRIS.Domain.Personnel.Configurations
{
    [Module(ModulesNames.Personnel)]
    [Order(3)]
    public class HealthInsuranceTypes : Entity, IConfigurationRoot
    {
        [UserInterfaceParameter(Order = 2)]
        public virtual string InsuranceType { get; set; }
        [UserInterfaceParameter(Order = 3)]
        public virtual float HospitalPercentage { get; set; }
        [UserInterfaceParameter(Order = 4)]
        public virtual float ClinicsPercentage { get; set; }
        [UserInterfaceParameter(Order = 5)]
        public virtual float XRayPercentage { get; set; }
        [UserInterfaceParameter(Order = 6)]
        public virtual float LaboratoryPercentage { get; set; }
        [UserInterfaceParameter(Order = 7)]
        public virtual float TeethPercentage { get; set; }
        [UserInterfaceParameter(Order = 8)]
        public virtual float EyesPercentage { get; set; }
        [UserInterfaceParameter(Order = 9)]
        public virtual bool WithSpouse { get; set; }
        [UserInterfaceParameter(Order = 10)]
        public virtual bool WithChildren { get; set; }
        [UserInterfaceParameter(Order = 11)]
        public virtual string Description { get; set; }
        [UserInterfaceParameter(Order = 12, IsHidden = true)]
        public virtual string NameForDropdown { get { return InsuranceType; } }
    }
}
