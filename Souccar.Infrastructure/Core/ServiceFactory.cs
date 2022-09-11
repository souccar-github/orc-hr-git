using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Souccar.Core.Services;
using Souccar.Infrastructure.Services.Sys;

namespace Souccar.Infrastructure.Core
{
    /// <summary>
    /// Author:Yaseen Alrefaee
    /// </summary>
    public class ServiceFactory
    {
        public static readonly ILocalizationService LocalizationService = new NHibernateLocalizationService();
        public static readonly ILanguageService LanguageService = new NHibernateLanguageService();
        public static readonly IValidationService ValidationService = new ValidationService();
        public static readonly ISecurityService SecurityService = new SecurityService();
        public static readonly IORMService ORMService = new NHibernateORMService();
    }
}
