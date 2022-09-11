using HRIS.Domain.Global.Entities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using HRIS.Domain.Global.Enums;

namespace Project.Web.Mvc4.Areas.JobDescription.Helpers
{
    public class JobDescriptionHelper
    {
        public static List<KeyValuePair<Employee, double>> MatchJobDesWithEmployees(HRIS.Domain.JobDescription.RootEntities.JobDescription jobDescription, int skillWeight, int educationWeight, int experianceWeight, int languageWeight, int certificateWeight, int competenceWeight)
        {

            var result = new List<KeyValuePair<Employee, double>>(); 
            foreach (var emp in ServiceFactory.ORMService.All<Employee>())
            {
                var mark= MatchJobDesWithEmployee(jobDescription, emp, skillWeight, educationWeight, experianceWeight, languageWeight, certificateWeight, competenceWeight);
                result.Add(new KeyValuePair<Employee,double>(emp,mark));
            }
            return result;
        }
        public static double MatchJobDesWithEmployee(HRIS.Domain.JobDescription.RootEntities.JobDescription jobDescription, Employee employee, int skillWeight, int educationWeight, int experianceWeight, int languageWeight, int certificateWeight, int competenceWeight)
        {
            var skillMark =(jobDescription.Skills.Count !=0)?(double)(jobDescription.Skills.Select(x => x.Type).Concat(employee.Skills.Select(x => x.SkillType)).Count()) / jobDescription.Skills.Count:0;
            var educationMark = (jobDescription.Educations.Count !=0)?(double)(jobDescription.Educations.Select(x => x.Major).Concat(employee.Educations.Select(x => x.Major)).Count()) / jobDescription.Educations.Count:0;
            var experienceMark = (jobDescription.Experiences.Count !=0)?(double)(jobDescription.Experiences.Select(x => x.Industry).Concat(employee.Experiences.Select(x => x.Industry)).Count()) / jobDescription.Experiences.Count:0;
            var languageMark = (jobDescription.Languages.Count != 0) ? (double)(jobDescription.Languages.Select(x => x.LanguageName).Concat(employee.Languages.Select(x => x.LanguageName)).Count()) / jobDescription.Languages.Count : 0;
            //var languageMark = (double)(jobDescription..Select(x => x.CompetenceSpecification).Concat(employee.Comp.Select(x => x.LanguageName)).Count()) / jobDescription.Languages.Count;
            //var languageMark = (double)(jobDescription.Languages.Select(x => x.LanguageName).Concat(employee.Languages.Select(x => x.LanguageName)).Count()) / jobDescription.Languages.Count; 

            var result = skillMark * skillWeight;
            result += educationMark * educationWeight;
            result += experienceMark * experianceWeight;
            result += languageMark * languageWeight;
            return result ;
        }
        public static string GetCode(CodeSetting setting, Employee employee)
        {
            return GetPositionCode(setting, employee.PrimaryPosition(), employee.Id);
        }
        public static string GetCode(CodeSetting setting, Position position)
        {
            return GetPositionCode(setting, position, position.Id);
        }
        public static string GetPositionCode(CodeSetting setting, Position position, int number)
        {
            var separatorSymbol = "";
            if (setting.SeparatorSymbol == SeparatorSymbol.dot)
                separatorSymbol = ".";
            if (setting.SeparatorSymbol == SeparatorSymbol.dash)
                separatorSymbol = "-";
            else if (setting.SeparatorSymbol != SeparatorSymbol.dot & setting.SeparatorSymbol != SeparatorSymbol.dash)
                separatorSymbol = setting.SeparatorSymbol.ToString();

            var prePostion = "";
            var sufPostion = "";
            var preJt = "";
            var sufJt = "";
            if (position == null)
            {
                prePostion = "--";
                sufPostion = "--";
                preJt = "--";
                sufJt = "--";
            }
            else
            {
                //Pre Node Name
                if (position.JobDescription != null &&
                    position.JobDescription.Node != null &&
                    setting.CustomPrefixStartingPosition == 0 &&
                    setting.CustomPrefixLength <= position.JobDescription.Node.Code.Length)

                    prePostion = position.JobDescription.Node.Code.Substring(setting.CustomPrefixStartingPosition, setting.CustomPrefixLength);

                else if (position.JobDescription != null &&
                        position.JobDescription.Node != null && 
                        setting.CustomPrefixStartingPosition != 0 &&
                        setting.CustomPrefixStartingPosition + setting.CustomPrefixLength < position.JobDescription.Node.Code.Length)
                                
                        prePostion = position.JobDescription.Node.Code.Substring(setting.CustomPrefixStartingPosition, setting.CustomPrefixLength);

                else

                    prePostion = position.JobDescription.Node.Code;

                //Suf Node Name
                if (position.JobDescription != null &&
                    position.JobDescription.Node != null &&
                    setting.CustomSuffixStartingPosition == 0 &&
                    setting.CustomSuffixLength <= position.JobDescription.Node.Code.Length)

                    sufPostion = position.JobDescription.Node.Code.Substring(setting.CustomSuffixStartingPosition, setting.CustomSuffixLength);

                else if (position.JobDescription != null &&
                        position.JobDescription.Node != null &&
                        setting.CustomSuffixStartingPosition != 0 &&
                        setting.CustomSuffixStartingPosition + setting.CustomSuffixLength < position.JobDescription.Node.Code.Length)
                    
                        sufPostion = position.JobDescription.Node.Code.Substring(setting.CustomSuffixStartingPosition, setting.CustomSuffixLength);

                else

                    sufPostion = position.JobDescription.Node.Code;

                //Pre Job Titel
                if (position.JobDescription != null &&
                    position.JobDescription.JobTitle != null &&
                    setting.CustomPrefixStartingPosition == 0 &&
                    setting.CustomPrefixLength <= position.JobDescription.JobTitle.Name.Length)
                    preJt = position.JobDescription.JobTitle.Name.Substring(setting.CustomPrefixStartingPosition, setting.CustomPrefixLength);

                else if (position.JobDescription != null &&
                    position.JobDescription.JobTitle != null &&
                    setting.CustomPrefixStartingPosition != 0 &&
                    setting.CustomPrefixStartingPosition + setting.CustomPrefixLength < position.JobDescription.JobTitle.Name.Length)
                    preJt = position.JobDescription.JobTitle.Name.Substring(setting.CustomPrefixStartingPosition, setting.CustomPrefixLength);

                else
                    preJt = position.JobDescription.JobTitle.Name;
                
                //Suf Job Titel
                if (position.JobDescription != null &&
                    position.JobDescription.JobTitle != null &&
                    setting.CustomSuffixStartingPosition == 0 &&
                    setting.CustomSuffixLength <= position.JobDescription.JobTitle.Name.Length)

                    sufJt = position.JobDescription.JobTitle.Name.Substring(setting.CustomSuffixStartingPosition, setting.CustomSuffixLength);

                else if (position.JobDescription != null &&
                        position.JobDescription.JobTitle != null &&
                        setting.CustomSuffixStartingPosition != 0 &&
                        setting.CustomSuffixStartingPosition + setting.CustomSuffixLength < position.JobDescription.JobTitle.Name.Length)

                    sufJt = position.JobDescription.JobTitle.Name.Substring(setting.CustomSuffixStartingPosition, setting.CustomSuffixLength);

                else

                    sufJt = position.JobDescription.JobTitle.Name;
             
            }

            //if (position != null)
                return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
                    setting.FixedPrefix,
                    separatorSymbol,
                    (setting.CustomPrefix == 0) ? prePostion : preJt,
                    separatorSymbol,
                    number,
                    separatorSymbol,
                    (setting.CustomSuffix == 0) ? sufPostion : sufJt,
                    separatorSymbol,
                    setting.FixedSuffix);

            //return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
            //        setting.FixedPrefix,
            //        separatorSymbol,
            //        "--",
            //        separatorSymbol,
            //        number,
            //        separatorSymbol,
            //        "--",
            //        separatorSymbol,
            //        setting.FixedSuffix);
        }

    }
}