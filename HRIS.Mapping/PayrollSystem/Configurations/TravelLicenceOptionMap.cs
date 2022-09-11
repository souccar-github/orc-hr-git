using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.RootEntities;

namespace HRIS.Mapping.PayrollSystem.Configurations
{
    public class TravelLicenceOptionMap : ClassMap<TravelLicenceOption>
    {
        public TravelLicenceOptionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.KiloPrice);
            Map(x => x.FuelPrice);
            Map(x => x.HalfDayBorder);
            Map(x => x.DayBorder);
            Map(x => x.FoodExternalPercentage);
            Map(x => x.FoodInternalPercentage);
            Map(x => x.RestExternalPercentage);
            Map(x => x.RestInternalPercentage);
            Map(x => x.AddedValuePercentage);
            Map(x => x.ExternalOtherExpense);
            Map(x => x.InternalOtherExpense);
            Map(x => x.MinisterSalaryCeil);
            Map(x => x.EmployeeSalaryCeil);
            Map(x => x.ExternalTransferenceWeightValue);
            Map(x => x.InternalTransferenceWeightValue);
            Map(x => x.CarConsumeIn20Liter);
            Map(x => x.Round);
            //Map(x => x.AuditState);
            //Map(x => x.Status);
        }
    }
}