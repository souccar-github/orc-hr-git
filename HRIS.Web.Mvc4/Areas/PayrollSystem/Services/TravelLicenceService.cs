//using System.Linq;
//using HRIS.Domain.PayrollSystem.Enums;
//using HRIS.Domain.PayrollSystem.RootEntities;
//using Souccar.Core.Utilities;
//using  Project.Web.Mvc4.Extensions;
//using Souccar.Infrastructure.Extenstions;
//using HRIS.Domain.PayrollSystem.Configurations;

//namespace Project.Web.Mvc4.Areas.PayrollSystem.Services
//{
//    public static class TravelLicenceService
//    {
//        public static void CalculateExternalTravelLicence(ExternalTravelLicence externalTravelLicence)
//        {
//            var travelLicenceOption = typeof(TravelLicenceOption).GetAll<TravelLicenceOption>().First();
//            var generalOption = typeof(GeneralOption).GetAll<GeneralOption>().First();
//            var travelCategory = typeof(TravelCategory).GetAll<TravelCategory>().First(x => x.TravelCategoryCountries.Any(y => y.Country.Id == externalTravelLicence.Country.Id));

//            externalTravelLicence.Salary = externalTravelLicence.PrimaryCard == null ? 0 : externalTravelLicence.PrimaryCard.Salary;
//            externalTravelLicence.BenefitSalary = externalTravelLicence.PrimaryCard == null ? 0 : externalTravelLicence.PrimaryCard.BenefitSalary;

//            #region حساب تعويض الانتقال

//            // حساب تعويض الانتقال الابتدائي
//            var initialTransferenceBenefitValue =
//                (externalTravelLicence.BenefitSalary * externalTravelLicence.ActualTransferenceDays *
//                 travelLicenceOption.ExternalTransferenceWeightValue) / generalOption.TotalMonthDays;

//            // حساب حسم المبيت
//            var restDeductionValue = 0.0;
//            if (externalTravelLicence.WithRest)
//            {
//                restDeductionValue = (((travelLicenceOption.RestExternalPercentage / 100) * initialTransferenceBenefitValue)
//                                      / externalTravelLicence.ActualTransferenceDays)
//                                     * externalTravelLicence.TotalRestDays;
//            }

//            // حساب حسم الطعام
//            var foodDeductionValue = 0.0;
//            if (externalTravelLicence.WithFood)
//            {
//                foodDeductionValue = (((travelLicenceOption.FoodExternalPercentage / 100) * initialTransferenceBenefitValue)
//                                      / externalTravelLicence.ActualTransferenceDays)
//                                     * externalTravelLicence.TotalFoodDays;
//            }
//            externalTravelLicence.TransferenceBenefitValue = initialTransferenceBenefitValue - restDeductionValue - foodDeductionValue;
//            externalTravelLicence.TransferenceBenefitValue = RoundUtility.Round(externalTravelLicence.TransferenceBenefitValue, RoundDirection.Normal, RoundSite.AfterComma, 2);
//            #endregion

//            #region حساب تعويض الاغتراب

//            float salary;
//            if (externalTravelLicence.IsMinister)
//            {
//                salary = (float)(externalTravelLicence.BenefitSalary > travelLicenceOption.MinisterSalaryCeil
//                    ? travelLicenceOption.MinisterSalaryCeil
//                    : externalTravelLicence.BenefitSalary);
//            }
//            else
//            {
//                salary = (float)(externalTravelLicence.BenefitSalary > travelLicenceOption.EmployeeSalaryCeil
//                    ? travelLicenceOption.EmployeeSalaryCeil
//                    : externalTravelLicence.BenefitSalary);
//            }



//            // حساب تعويض الاغتراب الابتدائي
//            var initialTravelBenefitValue = (salary * externalTravelLicence.ActualTravelDays * travelCategory.ValueRate) / generalOption.TotalMonthDays;

//            // حساب حسم المبيت
//            restDeductionValue = 0.0;
//            if (externalTravelLicence.WithRest)
//            {
//                restDeductionValue = (((travelLicenceOption.RestExternalPercentage / 100) * initialTravelBenefitValue)
//                                      / externalTravelLicence.ActualTravelDays)
//                                     * externalTravelLicence.TotalRestDays;
//            }

//            // حساب حسم الطعام
//            foodDeductionValue = 0.0;
//            if (externalTravelLicence.WithFood)
//            {
//                foodDeductionValue = (((travelLicenceOption.FoodExternalPercentage / 100) * initialTravelBenefitValue)
//                                      / externalTravelLicence.ActualTravelDays)
//                                     * externalTravelLicence.TotalFoodDays;
//            }

//            externalTravelLicence.TravelBenefitValue = initialTravelBenefitValue - restDeductionValue - foodDeductionValue;
//            externalTravelLicence.TravelBenefitValue = RoundUtility.Round(externalTravelLicence.TravelBenefitValue, RoundDirection.Normal, RoundSite.AfterComma, 2);
//            // حساب تعويض التهيوء
//            externalTravelLicence.PreparationBenefitValue = (salary * travelCategory.ValueRate) / generalOption.TotalMonthDays;
//            externalTravelLicence.PreparationBenefitValue = RoundUtility.Round(externalTravelLicence.PreparationBenefitValue, RoundDirection.Normal, RoundSite.AfterComma, 2);
//            // حساب تعويض نفقات نثرية
//            externalTravelLicence.OtherExpenseBenefitValue =
//                ((externalTravelLicence.TransferenceBenefitValue + externalTravelLicence.TravelBenefitValue +
//                externalTravelLicence.PreparationBenefitValue) * travelLicenceOption.ExternalOtherExpense) / 100;
//            externalTravelLicence.OtherExpenseBenefitValue = RoundUtility.Round(externalTravelLicence.OtherExpenseBenefitValue, RoundDirection.Normal, RoundSite.AfterComma, 2);
//            // حساب تعويض النسبة المضافة
//            externalTravelLicence.AddedValueBenefitValue =
//                ((externalTravelLicence.TravelBenefitValue + externalTravelLicence.PreparationBenefitValue) * travelLicenceOption.AddedValuePercentage) / 100;
//            externalTravelLicence.AddedValueBenefitValue = RoundUtility.Round(externalTravelLicence.AddedValueBenefitValue, RoundDirection.Normal, RoundSite.AfterComma, 2);
//            #endregion


//            if (!externalTravelLicence.WithBenefit)
//            {
//                externalTravelLicence.TransferenceBenefitValue = 0;
//                externalTravelLicence.TravelBenefitValue = 0;
//                externalTravelLicence.PreparationBenefitValue = 0;
//                externalTravelLicence.OtherExpenseBenefitValue = 0;
//                externalTravelLicence.AddedValueBenefitValue = 0;
//            }

//            externalTravelLicence.FinalBenefitValues = RoundUtility.PreDefinedRoundValue(travelLicenceOption.Round,
//                 (externalTravelLicence.TransferenceBenefitValue + externalTravelLicence.TravelBenefitValue + externalTravelLicence.PreparationBenefitValue +
//                       externalTravelLicence.OtherExpenseBenefitValue + externalTravelLicence.AddedValueBenefitValue + externalTravelLicence.LeavingFee) - externalTravelLicence.AdvanceValue);

            
//        }

//        public static void CalculateInternalTravelLicence(InternalTravelLicence internalTravelLicence)
//        {
//            var travelLicenceOption = typeof(TravelLicenceOption).GetAll<TravelLicenceOption>().First();
//            var generalOption = typeof(GeneralOption).GetAll<GeneralOption>().First();
//            float transportationBenefitValue;
//            var restDeductionValue = 0.0;
//            var foodDeductionValue = 0.0;

//            internalTravelLicence.Salary = internalTravelLicence.PrimaryCard == null ? 0 : internalTravelLicence.PrimaryCard.Salary;
//            internalTravelLicence.BenefitSalary = internalTravelLicence.PrimaryCard == null ? 0 : internalTravelLicence.PrimaryCard.BenefitSalary;


//            #region حساب تعويض الانتقال
//            // حساب تعويض الانتقال الابتدائي
//            var initialTransferenceBenefitValue =
//                (internalTravelLicence.BenefitSalary * internalTravelLicence.ActualTransferenceDays *
//                 travelLicenceOption.InternalTransferenceWeightValue)
//                / generalOption.TotalMonthDays;

//            // حساب حسم المبيت
//            if (internalTravelLicence.WithRest)
//            {
//                restDeductionValue = (((travelLicenceOption.RestInternalPercentage / 100) * initialTransferenceBenefitValue)
//                                      / internalTravelLicence.ActualTransferenceDays)
//                                     * internalTravelLicence.TotalRestDays;
//            }

//            // حساب حسم الطعام
//            if (internalTravelLicence.WithFood)
//            {
//                foodDeductionValue = (((travelLicenceOption.FoodInternalPercentage / 100) * initialTransferenceBenefitValue)
//                                      / internalTravelLicence.ActualTransferenceDays)
//                                     * internalTravelLicence.TotalFoodDays;
//            }

//            internalTravelLicence.TransferenceBenefitValue = (float)(initialTransferenceBenefitValue - restDeductionValue - foodDeductionValue);
//            internalTravelLicence.TransferenceBenefitValue =
//                (float)RoundUtility.Round(internalTravelLicence.TransferenceBenefitValue, RoundDirection.Normal, RoundSite.AfterComma, 2);
//            // تعويض أجور النقل
//            if (internalTravelLicence.WithSpecificTransportationBenefitValue == false)
//            {
//                if (internalTravelLicence.GoingTransportationType == TransportationType.CompanyCar && internalTravelLicence.ReturnTransportationType == TransportationType.CompanyCar)
//                {
//                    transportationBenefitValue = 0;
//                }
//                else if (internalTravelLicence.GoingTransportationType == TransportationType.PersonalCar && internalTravelLicence.ReturnTransportationType == TransportationType.PersonalCar)
//                {
//                    transportationBenefitValue = (float)((internalTravelLicence.Distance * 2) * travelLicenceOption.KiloPrice);
//                }
//                else if ((internalTravelLicence.GoingTransportationType == TransportationType.CompanyCar && internalTravelLicence.ReturnTransportationType == TransportationType.PersonalCar) ||
//                    internalTravelLicence.GoingTransportationType == TransportationType.PersonalCar && internalTravelLicence.ReturnTransportationType == TransportationType.CompanyCar)
//                {
//                    transportationBenefitValue = (float)(internalTravelLicence.Distance * travelLicenceOption.KiloPrice);
//                }
//                else
//                {
//                    transportationBenefitValue = ((internalTravelLicence.Distance * 2 * 20) / travelLicenceOption.CarConsumeIn20Liter) * travelLicenceOption.FuelPrice;
//                }
//            }
//            else
//            {
//                transportationBenefitValue = internalTravelLicence.TransportationBenefitValue;
//            }

//            // حساب تعويض نفقات نثرية
//            var otherExpenseBenefitValue = transportationBenefitValue * travelLicenceOption.InternalOtherExpense / 100;

//            //  التعويضات
//            internalTravelLicence.TransportationBenefitValue = (float)RoundUtility.Round(transportationBenefitValue, RoundDirection.Normal, RoundSite.AfterComma, 2);
//            internalTravelLicence.OtherExpenseBenefitValue = (float)RoundUtility.Round(otherExpenseBenefitValue, RoundDirection.Normal, RoundSite.AfterComma, 2);

//            internalTravelLicence.FinalBenefitValues = (float)(RoundUtility.PreDefinedRoundValue(travelLicenceOption.Round,
//                (internalTravelLicence.TransferenceBenefitValue + internalTravelLicence.TransportationBenefitValue + internalTravelLicence.OtherExpenseBenefitValue) - internalTravelLicence.AdvanceValue));

//            #endregion

            
//        }

//        //internal static void AcceptExternalTravelLicence(ExternalTravelLicence externalTravelLicence)
//        //{
//        //    externalTravelLicence.Status = Status.Accepted;
//        //    externalTravelLicence.Save();
//        //}

//        //internal static void AcceptInternalTravelLicence(InternalTravelLicence internalTravelLicence)
//        //{
//        //    internalTravelLicence.Status = Status.Accepted;
//        //    internalTravelLicence.Save();
//        //}


//    }
//}