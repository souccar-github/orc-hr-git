using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Configurations
{
    public class TravelLicenceOptionSpecification : Validates<TravelLicenceOption>
    {
        public TravelLicenceOptionSpecification()
        {
            IsDefaultForType();

            Check(x => x.KiloPrice, y => typeof(TravelLicenceOption).GetProperty("KiloPrice").GetTitle()).Required().GreaterThan(0);
            Check(x => x.FuelPrice, y => typeof(TravelLicenceOption).GetProperty("FuelPrice").GetTitle()).Required().GreaterThan(0);
            Check(x => x.DayBorder, y => typeof(TravelLicenceOption).GetProperty("DayBorder").GetTitle()).Required().GreaterThan(0).And.LessThanEqualTo(24);
            Check(x => x.HalfDayBorder, y => typeof(TravelLicenceOption).GetProperty("HalfDayBorder").GetTitle()).Required().GreaterThan(0).And.LessThan(x => x.DayBorder);
            Check(x => x.FoodExternalPercentage, y => typeof(TravelLicenceOption).GetProperty("FoodExternalPercentage").GetTitle()).Required().GreaterThan(0).And.LessThanEqualTo(100);
            Check(x => x.FoodInternalPercentage, y => typeof(TravelLicenceOption).GetProperty("FoodInternalPercentage").GetTitle()).Required().GreaterThan(0).And.LessThanEqualTo(100);
            Check(x => x.RestExternalPercentage, y => typeof(TravelLicenceOption).GetProperty("RestExternalPercentage").GetTitle()).Required().GreaterThan(0).And.LessThanEqualTo(100);
            Check(x => x.RestInternalPercentage, y => typeof(TravelLicenceOption).GetProperty("RestInternalPercentage").GetTitle()).Required().GreaterThan(0).And.LessThanEqualTo(100);
            Check(x => x.AddedValuePercentage, y => typeof(TravelLicenceOption).GetProperty("AddedValuePercentage").GetTitle()).Required().GreaterThan(0).And.LessThanEqualTo(100);
            Check(x => x.ExternalOtherExpense, y => typeof(TravelLicenceOption).GetProperty("ExternalOtherExpense").GetTitle()).Required().GreaterThan(0).And.LessThanEqualTo(100); 
            Check(x => x.InternalOtherExpense, y => typeof(TravelLicenceOption).GetProperty("InternalOtherExpense").GetTitle()).Required().GreaterThan(0).And.LessThanEqualTo(100);
            Check(x => x.MinisterSalaryCeil, y => typeof(TravelLicenceOption).GetProperty("MinisterSalaryCeil").GetTitle()).Required().GreaterThan(0);
            Check(x => x.EmployeeSalaryCeil, y => typeof(TravelLicenceOption).GetProperty("EmployeeSalaryCeil").GetTitle()).Required().GreaterThan(0);
            Check(x => x.InternalTransferenceWeightValue, y => typeof(TravelLicenceOption).GetProperty("InternalTransferenceWeightValue").GetTitle()).Required().GreaterThan(0);
            Check(x => x.ExternalTransferenceWeightValue, y => typeof(TravelLicenceOption).GetProperty("ExternalTransferenceWeightValue").GetTitle()).Required().GreaterThan(0);
            Check(x => x.CarConsumeIn20Liter, y => typeof(TravelLicenceOption).GetProperty("CarConsumeIn20Liter").GetTitle()).Required().GreaterThan(0);
            Check(x => x.Round, y => typeof(TravelLicenceOption).GetProperty("Round").GetTitle()).Required();
        }
    }
}
