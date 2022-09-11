using HRIS.Domain.Global.Enums;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Global.Entities
{
    public class CodeSetting:Entity
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual string FixedPrefix { get; set; }
        [UserInterfaceParameter(Order = 2)]
        public virtual string FixedSuffix { get; set; }
        [UserInterfaceParameter(Order = 3)]
        public virtual CodeSettingDynamicPart CustomPrefix { get; set; }
        [UserInterfaceParameter(Order = 4)]
        public virtual int CustomPrefixLength { get; set; }
        [UserInterfaceParameter(Order = 5)]
        public virtual int CustomPrefixStartingPosition { get; set; }
        [UserInterfaceParameter(Order = 6)]
        public virtual CodeSettingDynamicPart CustomSuffix { get; set; }
        [UserInterfaceParameter(Order = 7)]
        public virtual int CustomSuffixLength { get; set; }
        [UserInterfaceParameter(Order = 8)]
        public virtual int CustomSuffixStartingPosition { get; set; }
        [UserInterfaceParameter(Order = 9)]
        public virtual SeparatorSymbol SeparatorSymbol { get; set; }
        [UserInterfaceParameter(Order = 10)]
        public virtual string Description { get; set; }
    }
}
