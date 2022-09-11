using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.Attachment.Enums;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Training.RootEntities
{
    [Module(ModulesNames.Training)]
    [Order(1)]
    public class CourseName: Entity,IAggregateRoot
    {
        public virtual string ArabicName { get; set; }
        public virtual string EnglishName { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown
        {
            get
            {
                return ArabicName;//EnglishName + "( " + ArabicName + " )";
            }
        }
    }
}
