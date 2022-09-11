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
using HRIS.Domain.Personnel.Configurations;
using HRIS.Domain.Personnel.RootEntities;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
namespace HRIS.Validation.Specification.Personnel.Configurations
{
    public class HealthInsuranceTypesSpecification : Validates<HealthInsuranceTypes>
    {
        public HealthInsuranceTypesSpecification()
        {
            IsDefaultForType();

            #region Primitive Types
            Check(x => x.InsuranceType).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.HospitalPercentage).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);
            Check(x => x.ClinicsPercentage).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);
            Check(x => x.XRayPercentage).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);
            Check(x => x.LaboratoryPercentage).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);
            Check(x => x.TeethPercentage).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);
            Check(x => x.EyesPercentage).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);
            //Check(x => x.WithSpouse).Required();
            //Check(x => x.WithChildren).Required();
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            #endregion

        }
    }
}
