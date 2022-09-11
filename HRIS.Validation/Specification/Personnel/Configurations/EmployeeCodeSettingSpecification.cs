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
    public class EmployeeCodeSettingSpecification : Validates<EmployeeCodeSetting>
    {
        public EmployeeCodeSettingSpecification()
        {
            IsDefaultForType();

            #region Primitive Types
            Check(x => x.FixedPrefix).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.FixedSuffix).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.CustomPrefix).Required();
            Check(x => x.CustomPrefixLength).Required().Between(GlobalConstant.MinimumCustomPrefixLength, GlobalConstant.MaximumCustomPrefixLength);
            Check(x => x.CustomPrefixStartingPosition).Optional().Between(GlobalConstant.MinimumCustomPrefixStartingPosition, GlobalConstant.MaximumCustomPrefixStartingPosition);
            Check(x => x.CustomSuffix).Required();
            Check(x => x.CustomSuffixLength).Required().Between(GlobalConstant.MinimumCustomSuffixLengthn, GlobalConstant.MaximumCustomSuffixLength);
            Check(x => x.CustomSuffixStartingPosition).Optional().Between(GlobalConstant.MinimumCustomSuffixStartingPosition, GlobalConstant.MaximumCustomSuffixStartingPosition);
            Check(x => x.SeparatorSymbol).Required();
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            #endregion

        }
    }
}
