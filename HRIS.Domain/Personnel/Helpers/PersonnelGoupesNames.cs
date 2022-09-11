using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Personnel.Helpers
{
    public static class PersonnelGoupesNames
    {
        public const string ResourceGroupName = "PersonnelGoupesNames";

        #region Employee groups
        public const string PersonalInformation = "PersonalInformation";
        public const string FamilyInformation = "FamilyInformation";
        public const string BasicDetails = "BasicDetails";
        public const string CardBasicDetails = "CardBasicDetails";
        public const string EmployeeHealthInsurance = "EmployeeHealthInsurance";
        public const string SocialSecurity = "SocialSecurity";
        public const string FinanceDetails = "FinanceDetails";
        public const string AttendanceDetails = "AttendanceDetails";
        public const string Eligibility = "Eligibility";
        public const string EmployeePensionPlan = "EmployeePensionPlan";

        public const string Family = "Family";
        public const string Qualification = "Qualification";

        public const string Nationality = "Nationality";
        public const string General = "General";
        public const string ContactInformation = "ContactInformation";
        public const string Additional = "Additional";

        public const string Department = "Department";
        public const string JobDescription = "JobDescription";

        //spouse
        public const string SpouseInfo = "SpouseInfo";
        public const string MarriageInfo = "MarriageInfo";
        public const string JobInfo = "JobInfo";

        //child
        public const string ChildInfo = "ChildInfo";
        public const string ChildBenefitInfo = "ChildBenefitInfo";

        //exp
        public const string ReferenceInfo = "ReferenceInfo";

        //JobRelatedInfo
        public const string ChildrenOfMartyrs = "ChildrenOfMartyrs";

        public const string Leaves = "Leaves";
        public const string SalaryPackages = "SalaryPackages";
        public const string AdvanceDetails = "AdvanceDetails";
        public const string MissionDetails = "MissionDetails";
        public const string DeductionsDetails = "DeductionsDetails";
        public const string BenefitsDetails = "BenefitsDetails";
        public const string CourseDetails = "CourseDetails";
        public const string TrainingCenterDetails = "TrainingCenterDetails";



        public static string GetResourceKey(string key)
        {
            return string.Format("{0}_{1}", ResourceGroupName, key);
        }
        #endregion
    }
}
