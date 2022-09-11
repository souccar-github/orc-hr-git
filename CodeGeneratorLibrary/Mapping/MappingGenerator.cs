using System;
using System.IO;
using System.Text;
using Souccar.Reflector;

namespace Souccar.CodeGenerator.Mapping
{
    public class MappingGenerator : IMappingGenerator
    {
        private readonly string _filePath = string.Empty;
        private static readonly string MappingTemplateDirectoryPath = Environment.CurrentDirectory + @"\MappingTemplates";
        private readonly string _mappingTempateFileSyntax = string.Empty;
        private readonly string _mapTempateFileSyntax = string.Empty;
        private readonly string _referenceTempateFileSyntax = string.Empty;
        private readonly string _hasManyTempateFileSyntax = string.Empty;

        public MappingGenerator(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new Exception("file path must be not empty");

            _filePath = filePath;

            if (!Directory.Exists(_filePath))
                Directory.CreateDirectory(_filePath);

            _mappingTempateFileSyntax = ReadFile(MappingTemplateDirectoryPath+@"\MappingTempate.txt");
            _mapTempateFileSyntax = ReadFile(MappingTemplateDirectoryPath + @"\MapTempate.txt");
            _referenceTempateFileSyntax = ReadFile(MappingTemplateDirectoryPath + @"\ReferenceTemplate.txt");
            _hasManyTempateFileSyntax = ReadFile(MappingTemplateDirectoryPath + @"\HasManyTempate.txt");
           
        }

        public void Generate(ClassTree classTree)
        {
            var result = ReadClassTreePropertyNames(classTree);
            using (var stream = new StreamWriter(_filePath + @"\" + classTree.Name + "Map.cs"))
                stream.Write(result);
        }

        private static string ReadFile(string filePath)
        {
            using (var stream = new StreamReader(filePath))
            {
                return  stream.ReadToEnd();
            }
        }

        private string ReadClassTreePropertyNames(ClassTree classTree)
        {
            var separateMappingFileStrings = string.Empty;
            var mappingTempateFileStrings = new StringBuilder();
            
            mappingTempateFileStrings.Append(_mappingTempateFileSyntax.Replace("$$ObjectName$$", classTree.Name));

            foreach (var simpleProperties in classTree.SimpleProperties)
            {
                if (simpleProperties.Name != "Id")
                {
                    separateMappingFileStrings += _mapTempateFileSyntax.Replace("$$PropertyName$$", simpleProperties.Name);
                    separateMappingFileStrings += Environment.NewLine;
                }
            }

            mappingTempateFileStrings.Replace("$$Mappings$$", separateMappingFileStrings);
            separateMappingFileStrings = string.Empty;

            foreach (var referencesProperties in classTree.ReferencesProperties)
            {
                separateMappingFileStrings += _referenceTempateFileSyntax.Replace("$$PropertyName$$", referencesProperties.Name);
                separateMappingFileStrings += Environment.NewLine;
                Generate(referencesProperties.ClassTree);
            }

            mappingTempateFileStrings.Replace("$$References$$", separateMappingFileStrings);
            separateMappingFileStrings = string.Empty;

            foreach (var referencedByProperties in classTree.ReferencedByProperties)
            {
                separateMappingFileStrings += _hasManyTempateFileSyntax.Replace("$$PropertyName$$", referencedByProperties.Name);
                separateMappingFileStrings += Environment.NewLine;
                Generate(referencedByProperties.ClassTree);
            }

            mappingTempateFileStrings.Replace("$$HasManies$$", separateMappingFileStrings);

            return mappingTempateFileStrings.ToString();
        }

    }
}
