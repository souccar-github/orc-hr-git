using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Souccar.Domain.Localization;
using Souccar.Reflector;
using Souccar.Domain.Extensions;
using Souccar.Core.Extensions;
namespace Souccar.CodeGenerator.Resourecs
{
    public class ResourceFactory
    {
        public void Create(Language language,Assembly assembly)
        {
            var table = new Hashtable();
            foreach (var aggregate in assembly.GetAggregates())
            {
               Create(language,ClassTreeFactory.Create(aggregate.Key),table);
            }
        }

        public void Create(Language language, ClassTree classTree, Hashtable table)
        {
            var cultureInfo = new CultureInfo(language.LanguageCulture.ToString());
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
            if (table.ContainsKey(classTree.Type))
            {
                return;
            }
            table[classTree.Type] = classTree.Type;

            var group = new ResourceGroup() { Name = classTree.Type.FullName };

            var title = classTree.Type.GetLocalizedName();
            if (string.IsNullOrEmpty(title) || title == classTree.Name)
            {
                title = classTree.Name.ToCapitalLetters();
            }
            var localeStringResource = new LocaleStringResource();
            localeStringResource.ResourceName = classTree.Type.FullName;
            localeStringResource.ResourceValue = title;
            localeStringResource.ResourceGroup = group;
            localeStringResource.Language = language;

            language.LocaleStringResources.Add(localeStringResource);

            foreach (var simpleProperty in classTree.SimpleProperties)
            {
                if (!classTree.Type.TryGetPropertyLocalizedName(simpleProperty.Name, out title))
                {
                    title = simpleProperty.Name.ToCapitalLetters();
                }
                localeStringResource = new LocaleStringResource();
                localeStringResource.ResourceName = string.Format("{0}.{1}", classTree.Type.FullName, simpleProperty.Name);
                localeStringResource.ResourceValue = title;
                localeStringResource.ResourceGroup = group;
                localeStringResource.Language = language;

                language.LocaleStringResources.Add(localeStringResource);
                //dbContext = (IDbContext)_repository.GetPropertyValue("DbContext");
                //using (dbContext.BeginTransaction())
                //{
                //    dbContext.CommitTransaction();
                //}
            }

            foreach (var referencesProperty in classTree.ReferencesProperties)
            {
                if (!classTree.Type.TryGetPropertyLocalizedName(referencesProperty.Name, out title))
                {
                    title = referencesProperty.Name.ToCapitalLetters();
                }
                localeStringResource = new LocaleStringResource();
                localeStringResource.ResourceName = string.Format("{0}.{1}", classTree.Type.FullName, referencesProperty.Name);
                localeStringResource.ResourceValue = title;
                localeStringResource.ResourceGroup = group;
                localeStringResource.Language = language;
                language.LocaleStringResources.Add(localeStringResource);
                //dbContext = (IDbContext)_repository.GetPropertyValue("DbContext");
                //using (dbContext.BeginTransaction())
                //{
                //    dbContext.CommitTransaction();
                //}

                Create(language, referencesProperty.ClassTree, table);
            }

            foreach (var referencedByProperties in classTree.ReferencedByProperties)
            {
                if (!classTree.Type.TryGetPropertyLocalizedName(referencedByProperties.Name, out title))
                {
                    title = referencedByProperties.Name.ToCapitalLetters();
                }

                localeStringResource = new LocaleStringResource();
                localeStringResource.ResourceName = string.Format("{0}.{1}", classTree.Type.FullName, referencedByProperties.Name);
                localeStringResource.ResourceValue = title;
                localeStringResource.ResourceGroup = group;
                localeStringResource.Language = language;
                language.LocaleStringResources.Add(localeStringResource);
                //dbContext = (IDbContext)_repository.GetPropertyValue("DbContext");
                //using (dbContext.BeginTransaction())
                //{
                //    dbContext.CommitTransaction();
                //}
                Create(language, referencedByProperties.ClassTree, table);
            }

        }
    }
}
