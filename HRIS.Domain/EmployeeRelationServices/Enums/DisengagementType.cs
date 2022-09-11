using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.EmployeeRelationServices.Enums
{
    public enum DisengagementType
    {
        NotCreatedYet,
        DisengageFromSecondaryPosition,
        DisengageFromPromotion,
        DisengageFromTransfer,
        Resignation,
        Termination
    }
}
