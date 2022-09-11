using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.OrganizationChart.Helpers
{
    public static class OrgChartGoupesNames
    {
        public const string ResourceGroupName = "OrgChartGoupesNames";

        #region Employee groups
        
        public const string General = "General";
        public const string ContactInformation = "ContactInformation";
        public const string Additional = "Additional";

        


        public static string GetResourceKey(string key)
        {
            return string.Format("{0}_{1}", ResourceGroupName, key);
        }
        #endregion
    }
}
