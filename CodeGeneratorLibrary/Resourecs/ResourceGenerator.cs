using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
//using HRIS.Domain.ProjectManagment.RootEntities;
using Souccar.Reflector;
using Souccar.Domain.Extensions;

namespace Souccar.CodeGenerator.Resourecs
{
    public class ResourceGenerator : IResourceGenerator
    {
        private readonly string _filePath = string.Empty;

        public ResourceGenerator(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new Exception("file path must be not empty");

            _filePath = filePath;

            if (!Directory.Exists(_filePath))
                Directory.CreateDirectory(_filePath);
        }

       private string SeparateCapitalLetters(string value)
       {
           return Regex.Replace(value, @"(?<a>[a-z])(?<b>[A-Z0-9])", @"${a} ${b}");
       }

       //public void Generate(Assembly assembly)
       //{
       //    Generate(new CultureInfo("en-US"), assembly);
       //}

       //public void Generate(CultureInfo cultureInfo, Assembly assembly)
       //{
       //    foreach (var aggregate in assembly.GetAggregateClasses())
       //    {
       //        if (aggregate.Key != typeof(Project))
       //            Generate(cultureInfo, ClassTreeFactory.Create(aggregate.Key));
       //    }
       //}
       // public void GenerateAndCompare(CultureInfo cultureInfo, Assembly assembly, Hashtable table)
       // {
       //     foreach (var aggregate in assembly.GetAggregateClasses())
       //     {
       //         if (aggregate.Key != typeof(Project))
       //             GenerateAndCompare(cultureInfo, ClassTreeFactory.Create(aggregate.Key),table);
       //     }
        //}

        public void Generate(CultureInfo cultureInfo , ClassTree classTree)
        {
            string language = string.Empty;
            if (!cultureInfo.Name.StartsWith("en"))
            {
                language = "." + cultureInfo.Name.Substring(0, 2);
            }

            var @namespace = classTree.Type.Namespace;
            var path = _filePath + @"\" + string.Join(@"\", @namespace.Split('.'));
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            using (var resXResourceWriter = new ResXResourceWriter(path + @"/" + classTree.Name + "Resource" + language + ".resx"))
            {
                resXResourceWriter.AddResource("ObjectName", SeparateCapitalLetters(classTree.Name));

                foreach (var simpleProperty in classTree.SimpleProperties)
                {
                    resXResourceWriter.AddResource(simpleProperty.Name, SeparateCapitalLetters(simpleProperty.Name));
                }

                foreach (var referencesProperty in classTree.ReferencesProperties)
                {
                    resXResourceWriter.AddResource(referencesProperty.Name, SeparateCapitalLetters(referencesProperty.Name));
                    Generate(referencesProperty.ClassTree);
                }

                foreach (var referencedByProperties in classTree.ReferencedByProperties)
                {
                    resXResourceWriter.AddResource(referencedByProperties.Name, SeparateCapitalLetters(referencedByProperties.Name));
                    Generate(referencedByProperties.ClassTree);
                }

                resXResourceWriter.Generate();
            }
        }

        public void GenerateAndCompare(CultureInfo cultureInfo, ClassTree classTree,Hashtable table)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
            if (table.ContainsKey(classTree.Type))
            {
                return;
            }
            table[classTree.Type] = classTree.Type;


            string language = string.Empty;
            if (!cultureInfo.Name.StartsWith("en"))
            {
                language = "." + cultureInfo.Name.Substring(0, 2);
            }

            var @namespace = classTree.Type.Namespace;
            var path = _filePath + @"\" + string.Join(@"\", @namespace.Split('.'));
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            using (var resXResourceWriter = new ResXResourceWriter(path + @"/" + classTree.Name + "Resource" + language + ".resx"))
            {
                var title = classTree.Type.GetLocalizedName();
                if (string.IsNullOrEmpty(title)||title==classTree.Name)
                {
                    title = SeparateCapitalLetters(classTree.Name);
                }
                
                resXResourceWriter.AddResource("ObjectName", title);

                foreach (var simpleProperty in classTree.SimpleProperties)
                {
                    if (!classTree.Type.TryGetPropertyLocalizedName(simpleProperty.Name,out title))
                    {
                        title = SeparateCapitalLetters(simpleProperty.Name);
                    }
                    resXResourceWriter.AddResource(simpleProperty.Name, title);
                }

                foreach (var referencesProperty in classTree.ReferencesProperties)
                {
                    if (!classTree.Type.TryGetPropertyLocalizedName(referencesProperty.Name, out title))
                    {
                        title = SeparateCapitalLetters(referencesProperty.Name);
                    }
                    resXResourceWriter.AddResource(referencesProperty.Name, title);
                    GenerateAndCompare(cultureInfo, referencesProperty.ClassTree,table);
                }

                foreach (var referencedByProperties in classTree.ReferencedByProperties)
                {
                    if (!classTree.Type.TryGetPropertyLocalizedName(referencedByProperties.Name, out title))
                    {
                        title = SeparateCapitalLetters(referencedByProperties.Name);
                    }
                    resXResourceWriter.AddResource(referencedByProperties.Name, title);
                    GenerateAndCompare(cultureInfo,referencedByProperties.ClassTree,table);
                }

                resXResourceWriter.Generate();
                resXResourceWriter.Close();
            }
        }

        public void Generate(ClassTree classTree)
        {
            Generate(new CultureInfo("en-US"), classTree);
        }
        public void GenerateArabice(ClassTree classTree)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("ar-SY");
            Generate(System.Threading.Thread.CurrentThread.CurrentCulture, classTree);
        }
        public void GenerateEnglish(ClassTree classTree)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Generate(System.Threading.Thread.CurrentThread.CurrentCulture, classTree);
        }
    }
}
