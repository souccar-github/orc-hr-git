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
using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Core;
#endregion
namespace HRIS.Mapping.Personnel.Configurations
{
    public sealed class HealthInsuranceTypesMap : ClassMap<HealthInsuranceTypes>
    {
        public HealthInsuranceTypesMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.InsuranceType);
            Map(x => x.HospitalPercentage);
            Map(x => x.ClinicsPercentage);
            Map(x => x.XRayPercentage);
            Map(x => x.LaboratoryPercentage);
            Map(x => x.TeethPercentage);
            Map(x => x.EyesPercentage);
            Map(x => x.WithSpouse);
            Map(x => x.WithChildren);
            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);

        }
    }
}