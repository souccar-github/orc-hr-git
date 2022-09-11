#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Personnel.Entities
{
    public sealed class SpouseMap : ClassMap<Spouse>
    {
        public SpouseMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            #region spouse Info
            Map(x => x.IdentificationNo).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.FirstName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.LastName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.FatherName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.MatherName).Length(GlobalConstant.SimpleStringMaxLength);
           
            Map(x => x.DateOfBirth);
            References(x => x.PlaceOfBirth);
           
            References(x => x.Nationality);

            Map(x => x.ResidencyNo).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.ResidencyExpiryDate).Nullable();

            Map(x => x.PassportNo).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.PassportExpiryDate).Nullable();

            Map(x => x.HasChildBenefit);

            Map(x => x.Note).Length(GlobalConstant.MultiLinesStringMaxLength);
            #endregion
            
            #region marraige info
            Map(x => x.Order).Column("SpouseOrder");
            Map(x => x.MarriageDate);
            Map(x => x.IsDeath);
            Map(x => x.DeathDate);
            Map(x => x.IsDivorce);
            Map(x => x.DivorceDate);

            #endregion 
            Map(x => x.FirstContactNumber).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.SecondContactNumber).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Email).Nullable().Length(GlobalConstant.SimpleStringMaxLength);

            Map(x => x.HasJob);
            References(x => x.JobTitle);

         
           
            Map(x => x.DateOfFamilyBenefitActivation);
            Map(x => x.CompanyName).Nullable().Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.WorkAddress).Nullable().Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.WorkEmail).Nullable().Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.WorkPhone).Nullable().Length(GlobalConstant.SimpleStringMaxLength);

            References(x => x.Employee);
            HasMany(x => x.Attachments).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}