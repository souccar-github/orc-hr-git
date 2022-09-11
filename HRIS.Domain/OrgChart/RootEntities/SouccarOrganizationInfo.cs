#region

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using HRIS.Domain.JobDesc.RootEntities;
using HRIS.Domain.Objectives.RootEntities;
using HRIS.Domain.OrgChart.Indexes;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;
using Souccar.Core.CustomAttribute;

#endregion

namespace HRIS.Domain.OrgChart.RootEntities
{
    public class SouccarOrganizationInfo : Entity, IAggregateRoot
    {

        public SouccarOrganizationInfo()
        {
            
        }


        public virtual int NodeId { get; set; }

        public virtual string NodeName { get; set; }

        public virtual int? ParentId { get; set; }

        public virtual int ProcessType { get; set; }

        public virtual int Sync { get; set; }

        

        
    }
}