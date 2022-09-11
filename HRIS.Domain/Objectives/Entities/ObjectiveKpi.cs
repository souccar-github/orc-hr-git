#region

using System.ComponentModel.DataAnnotations;
using HRIS.Domain.Objectives.Indexes;
using HRIS.Domain.Objectives.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.JobDescription.Entities;

#endregion

namespace HRIS.Domain.Objectives.Entities
{
    public class ObjectiveKpi : AbstractKpi
    {
        public virtual Objective Objective { get; set; }
    }
}