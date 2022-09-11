#region


using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.Personnel.Helpers;
using System.Collections.Generic;

#endregion

namespace HRIS.Domain.Personnel.Entities
{
    public class Spouse:SpouseBase
    {
        public Spouse()
        {
            Attachments = new List<SpouseAttachment>();
        }
        public virtual Employee Employee { get; set; }
        [UserInterfaceParameter( Order = 1)]
        public virtual IList<SpouseAttachment> Attachments { get;  set; }

        public virtual void AddAttachment(SpouseAttachment attachment)
        {
            attachment.Spouse = this;
            Attachments.Add(attachment);
        }

    }
}