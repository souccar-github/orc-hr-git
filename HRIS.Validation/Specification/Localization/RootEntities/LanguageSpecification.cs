using Souccar.Domain.Localization;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Validation.Specification.Localization.RootEntities
{
    public class LanguageSpecification : Validates<Language>
    {
        public LanguageSpecification()
        {
            IsDefaultForType();


            #region Indexes

            Check(x => x.Name).Required();

            Check(x => x.LanguageCulture).Required();

            #endregion Indexes

        }
    }
}
