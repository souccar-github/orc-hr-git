#region

using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Recruitment.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.Personnel.Helpers;

#endregion

namespace HRIS.Domain.Recruitment.Entities
{
    public class RSpouse : SpouseBase
    {
        public virtual Applicant Applicant { get; set; }
    }
}