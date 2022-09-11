using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Personnel.Enums
{
    public enum EmployeeStatus
    {
        //New,
        Resigned,
        /// <summary>
        /// على رأس عمله
        /// </summary>
        InPosition,
        /// <summary>
        /// ليس على رأس عمله
        /// </summary>
        NotInPosition
    }
}
