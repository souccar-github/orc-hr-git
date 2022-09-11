using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Security.Configuration
{
    [Module(ModulesNames.Security)]
    [Order(2)]
    public class FullAuthorityRole : Entity, IConfigurationRoot
    {

        #region Basic Info

        [UserInterfaceParameter(IsReference = true, Order = 1)]
        public virtual Role Role { get; set; }

        [UserInterfaceParameter(IsReference = true, Order = 10)]
        public virtual string Notes { get; set; }

        #endregion

    }
}
