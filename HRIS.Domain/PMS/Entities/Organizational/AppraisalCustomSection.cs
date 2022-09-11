
using HRIS.Domain.PMS.RootEntities;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.PMS.Entities.Organizational
{
    public class AppraisalCustomSection : Entity,IAggregateRoot
    {
        public AppraisalCustomSection()
        {
            AppraisalCustomSectionItems=new List<AppraisalCustomSectionItem>();
        }
        public virtual AppraisalSection Section { get; set; }
        public virtual float Weight { get; set; }
        public virtual float Rate { get; set; }

        public virtual void UpdateValue()
        {
            Rate = AppraisalCustomSectionItems.Sum(x => x.Weight*x.Rate/100);
        }
        public virtual IList<AppraisalCustomSectionItem> AppraisalCustomSectionItems { get; set; }
        public virtual void AddAppraisalCustomSectionItem(AppraisalCustomSectionItem appraisalCustomSectionItem)
        {
            appraisalCustomSectionItem.Section = this;
            AppraisalCustomSectionItems.Add(appraisalCustomSectionItem);
        }
        public virtual Appraisal Appraisal { get; set; }
    }
}
