#region

using System;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Objectives.RootEntities;

using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.OrganizationChart.RootEntities;

#endregion

namespace HRIS.Domain.Objectives.Entities
{
    public class SharedWith : Entity
    {
        [UserInterfaceParameter(Order = 10 ,IsReference = true)]
        public virtual Node Department { get; set; }

        [UserInterfaceParameter(Order = 21, IsReference = true, CascadeFrom = "Department", ReferenceReadUrl = "Objectives/Reference/ReadPositionsByNode")]
        public virtual Position Position { get; set; }

        [UserInterfaceParameter(Order = 32)]
        public virtual float Percentage { get; set; }


        [UserInterfaceParameter(Order = 40)]
        public virtual string Description { get; set; }

        [UserInterfaceParameter(Order = 43)]
        public virtual Objective Objective { get; set; }
    }
}