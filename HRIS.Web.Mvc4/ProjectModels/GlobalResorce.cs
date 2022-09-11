using HRIS.Domain.Global.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Mvc4.ProjectModels
{
    public class GlobalResorce
    {
        //
        // GET: /GlobalResorce/
        public static string GetCommandResourceGroupName()

        {

            return CommandsNames.ResourceGroupName;
        }

        public static string GetModulesResourceGroupName()

        {

            return ModulesNames.ResourceGroupName;
        }

        public static int GetValidationSimpleStringMaxLength()
        {
            return HRIS.Validation.GlobalConstant.SimpleStringMaxLength;
        }

        
              public static int GetValidationMultiLinesStringMaxLength()
        {
            return HRIS.Validation.GlobalConstant.MultiLinesStringMaxLength;
        }
    }

    
}
