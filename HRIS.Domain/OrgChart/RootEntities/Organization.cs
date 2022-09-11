#region

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using HRIS.Domain.OrganizationChart.Helpers;
using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;
//using Resources.Areas.OrganizationChart.Entities.Organization;

#endregion

namespace HRIS.Domain.OrganizationChart.RootEntities
{
   // [Module(ModulesNames.OrganizationChart)]
    public class Organization : Entity, IAggregateRoot
    {
        public Organization()
        {
            
        }


        #region General

        public virtual string Name { get; set; }
        
        public virtual Country Location { get; set; }

        #region New For PS

        public virtual OrganizationSize Size { get; set; }
        public virtual int NumberOfEmployees { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Fax { get; set; }
        public virtual string Mobile { get; set; }
        public virtual string Address { get; set; }
        public virtual string POBox { get; set; }
        public virtual string WebSite { get; set; }
        public virtual string Facebook { get; set; }



        #endregion

        #endregion

        
    }
}