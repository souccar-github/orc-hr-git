using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Infrastructure.Core;
using Souccar.Core.Extensions;

namespace Project.Web.Mvc4.Helpers.Resource
{
    public class PersonnelLocalizationHelper
    {
        public const string ResourceGroupName = "PersonnelLocalizationHelper";

        public const string MilitaryServiceGenderValidationMessage = "MilitaryServiceGenderValidationMessage";

        //JobRelatedInfo
        public const string EndWorkflow = "EndWorkflow"; 
        public const string MsgWorkSideRequired = "MsgWorkSideRequired";
        public const string MsgWorkSideAgreementNumberRequired = "MsgWorkSideAgreementNumberRequired";
        public const string MsgWorkSideAgreementDateRequired = "MsgWorkSideAgreementDateRequired";
        public const string MsgKinshipTypeRequired = "MsgKinshipTypeRequired";
        public const string MsgDocumentNumberRequired = "MsgDocumentNumberRequired";
        public const string MsgDocumentDateRequired = "MsgDocumentDateRequired";
        public const string MsgIssuedByRequired = "MsgIssuedByRequired";
        public const string MsgDateOfExemptionMustBeGreaterThanDateOfBirth = "MsgDateOfExemptionMustBeGreaterThanDateOfBirth";
        public const string MsgDateOfDelayMustBeGreaterThanDateOfBirth = "MsgDateOfDelayMustBeGreaterThanDateOfBirth";
        public const string MsgMilitiryServiceDocIssuanceMustBeGreaterThanDateOfBirth = "MsgMilitiryServiceDocIssuanceMustBeGreaterThanDateOfBirth";
        public const string MsgReserveStartDateMustBeGreaterThanDateOfBirth = "MsgReserveStartDateMustBeGreaterThanDateOfBirth";
        public const string MsgServiceEndDateMustBeGreaterThanDateOfBirth = "MsgServiceEndDateMustBeGreaterThanDateOfBirth";
        public const string MsgServiceStartDateMustBeGreaterThanDateOfBirth = "MsgServiceStartDateMustBeGreaterThanDateOfBirth";
        public const string MsgMilitiryServiceDocIssuanceMustBeGreaterThanServiceStartDate = "MsgMilitiryServiceDocIssuanceMustBeGreaterThanServiceStartDate";
        public const string MsgMilitiryServiceDocIssuanceMustBeLessThanCurrentDate = "MsgMilitiryServiceDocIssuanceMustBeLessThanCurrentDate";

        public const string MsgHoldDateMustBeGreaterThanDateOfBirth = "MsgHoldDateMustBeGreaterThanDateOfBirth";
        public const string MsgServiceEndDateMustBeGreaterThanServiceStartDate="MsgServiceEndDateMustBeGreaterThanServiceStartDate";
        public const string MsgReserveStartDateMustBeGreaterThanServiceEndDate = "MsgReserveStartDateMustBeGreaterThanServiceEndDate";
        public const string MsHoldDateMustBeGreaterThanServiceEndDate = "MsHoldDateMustBeGreaterThanServiceEndDate";
        public const string MsYouCanNotAddDuplicateHoldDate = "MsYouCanNotAddDuplicateHoldDate";
        public const string MsYouCanNotAddDuplicateReserveStartDate = "MsYouCanNotAddDuplicateReserveStartDate";
        public const string MsYouCanNotAddDuplicateDateOfExemption = "MsYouCanNotAddDuplicateDateOfExemption";
        public const string MsYouCanNotAddDuplicateDateOfDelay = "MsYouCanNotAddDuplicateDateOfDelay";
        public const string MsYouCanNotAddDuplicateServiceStartDate = "MsYouCanNotAddDuplicateServiceStartDate";
        public const string DateOfBirthForTheChildMustBeGreaterThanMarriageDate = "DateOfBirthForTheChildMustBeGreaterThanMarriageDate";
        public const string DateOfBirthForTheChildMustBeGreaterThanDateOfBirthForTheSpouse = "DateOfBirthForTheChildMustBeGreaterThanDateOfBirthForTheSpouse";
        public const string DateOfBirthForTheChildMustBeGreaterThanDateOfBirthForTheEmployee = "DateOfBirthForTheChildMustBeGreaterThanDateOfBirthForTheEmployee";
        public const string AdvanceAmountMustBeLessThanOrEqualDeservableAdvanceAmount = "AdvanceAmountMustBeLessThanOrEqualDeservableAdvanceAmount";
        public const string TheAdvanceDateMustBeBetweenFromDateAndToDateDefindingInAdvanceSetting = "TheAdvanceDateMustBeBetweenFromDateAndToDateDefindingInAdvanceSetting";
        public const string ThisEmployeeHasApprovedNotTransferAdvancesThisMonth = "ThisEmployeeHasApprovedNotTransferAdvancesThisMonth";
        public const string AdvanceAmountMustBeGreaterThanZero = "AdvanceAmountMustBeGreaterThanZero";
        //Dashboard
        public const string ParentNodes = "ParentNodes";
        public const string SelectParentNodes = "SelectParentNodes";
        public const string ChildNodes = "ChildNodes";
        public const string SelectChildNodes = "SelectChildNodes";
        public const string MalesAndFemalesPercentage = "MalesAndFemalesPercentage";
        public const string PercentageOfMales = "PercentageOfMales";
        public const string PercentageOfFemales = "PercentageOfFemales";
        public const string BloodGroupPercentage = "BloodGroupPercentage";
        public const string NumberOfMales = "NumberOfMales";
        public const string NumberOfFemales = "NumberOfFemales";
        public const string ReligionsPercentage = "ReligionsPercentage";
        public const string Muslim = "Muslim";
        public const string Christian = "Christian";
        public const string Jewish = "Jewish";
        public const string OtherReligions = "OtherReligions";
        public const string SocialStatusPercentage = "SocialStatusPercentage";
        public const string Single = "Single";
        public const string Married = "Married";
        public const string Divorced = "Divorced";
        public const string Widow = "Widow";
        public const string Engaged = "Engaged";
        public const string DisabilityPercentage = "DisabilityPercentage";
        public const string Nationality = "Nationality";
        public const string NationalityType = "NationalityType";
        public const string BasicNationality = "BasicNationality";
        public const string OtherNationality = "OtherNationality";

        public static string GetResource(string key)
        {
            var result = ServiceFactory.LocalizationService.GetResource(ResourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }

    }
}