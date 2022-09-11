using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Souccar.Core.Services;
using Souccar.Domain.Localization;
using Souccar.Domain.PersistenceSupport;
using Souccar.NHibernate;

namespace Souccar.Infrastructure.Services.Sys
{
    public class NHibernateLanguageService : ILanguageService
    {
        private readonly IRepository<Language> _repository;

        public NHibernateLanguageService()
        {
            _repository = new NHibernateRepository<Language>();
        }

        #region ILanguageService Members

        //public Language GetLangugeByCulture(CultureInfo cultureInfo)
        //{
        //    return _repository.GetAll().Where(x => x.LanguageCulture == cultureInfo.Name).SingleOrDefault();
        //}

        //public Language GetDefaultLanguage()
        //{
        //    return GetLangugeByCulture(new CultureInfo("en-US"));
        //}

        public IList<Language> GetAllLanguages()
        {
            return _repository.GetAll().ToList();
        }

        #endregion

    }
}