using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.PMS.Entities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.PMS.RootEntities;

namespace HRIS.Domain.PMS.Configurations
{
    [Command(CommandsNames.OverwriteAppraisalTemplateSetting, Order = 1)]

    [Module(ModulesNames.PMS)]
    [Order(2)]
    public class AppraisalTemplateSetting : Entity, IConfigurationRoot
    {
        public AppraisalTemplateSetting()
        {
            AppraisalTemplatePositions = new List<TemplateAppraisalPositions>();
        }

        [UserInterfaceParameter(Order = 1, IsReference = true)]
        public virtual AppraisalTemplate DefaultTemplate { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual string Name { get; set; }

        [UserInterfaceParameter(Order = 3)]
        public virtual DateTime CreationDate { get; set; }
        
        public virtual IList<TemplateAppraisalPositions> AppraisalTemplatePositions { get; set; }

        public virtual void AddAppraisalTemplatePosition(TemplateAppraisalPositions templatePosition)
        {
            templatePosition.AppraisalTemplateSetting = this;
            this.AppraisalTemplatePositions.Add(templatePosition);
        }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown
        {
            get { return Name; }
        }
    }
}
