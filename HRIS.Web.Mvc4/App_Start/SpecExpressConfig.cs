using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Validation.MessageStore;
using HRIS.Validation.Specification.Personnel.RootEntities;
using SpecExpress;
using System.Reflection;

namespace Project.Web.Mvc4.App_Start
{
    public static class SpecExpressConfig
    {
        public static void Register()
        {
            ValidationCatalog.Scan(x => x.AddAssembly(Assembly.GetAssembly(typeof(EmployeeSpecification))));
            ValidationCatalog.Configuration.DefaultMessageStore = new DefaultValidationMessagesStor();
            //ValidationCatalog.Configure(c => c.AddMessageStore(new CustomMessagesMessageStore(), "CustomMessagesMessageStore"));
        }
    }
}