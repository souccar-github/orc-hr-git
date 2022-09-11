#region

using System.ComponentModel.DataAnnotations;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.JobDescription.Entities
{
    public class RoleKpi : AbstractKpi
    {     
        public virtual Role Role { get; set; }
    }
}