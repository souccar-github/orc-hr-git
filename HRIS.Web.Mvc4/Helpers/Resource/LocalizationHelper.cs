using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Core;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Localization;
namespace Project.Web.Mvc4.Helpers
{
    //After Apply Master Detail Feature

    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class LocalizationHelper 
    {
        public static bool IsRtl
        {
            get
            {
                var lan = ServiceFactory.ORMService.All<Language>().FirstOrDefault(x => x.IsActive);
                return lan != null ? lan.Rtl : false;
            }
        }

        public static string GetResource(string resourceGroupName, string key)
        {
            var result = ServiceFactory.LocalizationService.GetResource(resourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }

        public static void SetActiveLanguage(int langId)
        {

            var oldLans = ServiceFactory.ORMService.All<Language>().Where(x => x.IsActive);
            var lan = ServiceFactory.ORMService.GetById<Language>(langId);
            var entities = new List<IAggregateRoot>();
            foreach (var l in oldLans)
            {
                l.IsActive = false;
                entities.Add(l);
            }

            if (lan != null)
            {
                lan.IsActive = true;
                entities.Add(lan);
            }
            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
        }

        public const string ResourceGroupName = "LocalizationHelper";
        public const string Total = "Total";
        public const string EmployeeName = "EmployeeName";
        public const string Save = "Save";
        public const string HomePageTitle = "HomePageTitle";
        public const string HomePageDescription = "HomePageDescription";
        public const string Module = "Module";
        public const string ChangePassword = "ChangePassword";
        public const string Apply = "Apply";
        public const string Option = "Option";
        public const string Options = "Options";
        public const string Role = "Role";
        public const string Manager = "Manager";
        
        public const string Accept = "Accept";
        public const string Reject = "Reject";
        public const string Pending = "Pending";
        public const string TheFileExtensionIsInvalid = "TheFileExtensionIsInvalid";
        

        #region Security
        public const string Roles = "Roles";
        public const string SelectedRole = "SelectedRole";
        public const string AddUserToRole = "AddUserToRole";
        public const string RestPassword = "RestPassword";
        public const string NewPassword = "NewPassword";
        public const string ConfirmPassword = "ConfirmPassword";
        #endregion
        public const string ManageFieldSecurity = "ManageFieldSecurity";
        public const string ShowFields = "ShowFields";
        public const string HiddenFields = "HiddenFields";
        #region navigation
        public const string RoleManagement = "RoleManagement";
        public const string Dashboard = "Dashboard";
        public const string Aggregate = "Aggregate";
        public const string AggregatesFields = "AggregatesFields";
        public const string Details = "Details";
        public const string DetailsFields = "DetailsFields";
        public const string ActionList = "ActionList";
        public const string Index = "Index";
        public const string Service = "Service";
        public const string Report = "Report";
        public const string Configuration = "Configuration";
        public const string ConfigurationFields = "ConfigurationsFields";
        public const string AvailableModules = "AvailableModules";
        public const string AssignedModules = "AssignedModules";
        public const string AvailableDashboards = "AvailableDashboards";
        public const string AssignedDashboards = "AssignedDashboards";
        public const string AvailableAggregates = "AvailableAggregates";
        public const string AssignedAggregates = "AssignedAggregates";
        public const string AvailableDetails = "AvailableDetails";
        public const string AssignedDetails = "AssignedDetails";
        public const string AvailableActionLists = "AvailableActionLists";
        public const string AssignedActionLists = "AssignedActionLists";
        public const string AvailableIndexs = "AvailableIndexs";
        public const string AssignedIndexs = "AssignedIndexs";
        public const string AvailableServices = "AvailableServices";
        public const string AssignedServices = "AssignedServices";
        public const string AvailableReports = "AvailableReports";
        public const string AssignedReports = "AssignedReports";
        public const string AvailableConfigurations = "AvailableConfigurations";
        public const string AssignedConfigurations = "AssignedConfigurations";

        public const string Editable = "Editable";
        public const string Insertable = "Insertable";
        public const string Deleteable = "Deleteable";
        public const string EditableAll = "EditableAll";
        public const string InsertableAll = "InsertableAll";
        public const string DeleteableAll = "DeleteableAll";
        public const string UnEditable = "UnEditable";
        public const string UnInsertable = "UnInsertable";
        public const string UnDeleteable = "UnDeleteable";
        public const string UnEditableAll = "UnEditableAll";
        public const string UnInsertableAll = "UnInsertableAll";
        public const string UnDeleteableAll = "UnDeleteableAll";

        #endregion

        public const string Owner = "Owner";
        public const string Actually = "Actually";
        public const string ActuallyStartDate = "ActuallyStartDate";
        public const string ActuallyEndDate = "ActuallyEndDate";
        public const string ActionPlan = "ActionPlan";

        public const string Error = "Error";
        public const string Warning = "Warning";
        public const string Information = "Information";
        public const string SectionValue="SectionValue";
        public const string Add = "Add";
        public const string Remove = "Remove";

        public const string Name = "Name";
        public const string General = "General";
        public const string Admin = "Admin";
        public const string Planning = "Planning";
        public const string PercentageOfCompletion = "PercentageOfCompletion";
        public const string ExpectedResult = "ExpectedResult";
        public const string Description = "Description";
        public const string JobDescription = "JobDescription";
        public const string EmploymentStatus = "EmploymentStatus";
        public const string EmployeeStatus = "EmployeeStatus";
        public const string Date = "Date";
        public const string LeaveRequesTypeTitle = "LeaveRequesTypeTitle";
        public const string JobTitle = "JobTitle";
        public const string AdditionalType = "AdditionalType";
        public const string Type = "Type";
        public const string Position = "Position";
        public const string Order = "Order";
        public const string Fixed = "Fixed";
        public const string Custom = "Custom";
        public const string Grade = "Grade";
        public const string OrgLevel = "OrgLevel";
        public const string Item = "Item";
        public const string Items = "Items";
        public const string Value = "Value";
        public const string Title = "Title";
        public const string Weight = "Weight";
        public const string Kpi = "Kpi";
        public const string Kpis = "Kpis";
        public const string FullName = "FullName";
        public const string TripleName = "TripleName";
        public const string StartDate = "StartDate";
        public const string EndDate = "EndDate";
        public const string AddNotificationTitle = "AddNotificationTitle";
        public const string DeleteNotificationTitle = "DeleteNotificationTitle";
        public const string UpdateNotificationTitle = "UpdateNotificationTitle";

        public const string Age = "Age";
        public const string Code = "Code";
        public const string Phase = "Phase";
        public const string PositionCode = "PositionCode";

        public const string Node = "Node";
        public const string EnglishMark = "EnglishMark";
        public const string TestDate = "TestDate";
        public const string IsChecked = "IsChecked";
        public const string Document = "Document";
        public const string MessageDetailsValidationError = "MessageDetailsValidationError";
     





        public const string Ok = "Ok";
        public const string Edit = "Edit";
        public const string Next = "Next";
        public const string Previous = "Previous";
        public const string Cancel = "Cancel";
        public const string Yes = "Yes";
        public const string No = "No";
        public const string Min = "Min";
        public const string Max = "Max";
        public const string Update = "Update";
        public const string Delete = "Delete";
        public const string More = "More";
        public const string Help = "Help";
        public const string Logout = "Logout";
        public const string Settings = "Settings";
        public const string Exit = "Exit";
        public const string Print = "Print";
        public const string Fail = "Fail";
        public const string Done = "Done";
        public const string Pair = "Pair";
        public const string Success = "Success";
        public const string FailMessage = "FailMessage";
        public const string CantTermintatePreparationPeriodThisEmployeeBecauseHisCardStatusIsNotNew = "CantTermintatePreparationPeriodThisEmployeeBecauseHisCardStatusIsNotNew";
        public const string YouCanNotDoingThisProcessBecauseTheEmployeeHasPendingWorkflowsPleaseCheckHisWorkflows = "YouCanNotDoingThisProcessBecauseTheEmployeeHasPendingWorkflowsPleaseCheckHisWorkflows";
        public const string DoneMessage = "DoneMessage";
        public const string SuccessMessage = "SuccessMessage";
        public const string AlreadyexistMessage = "AlreadyexistMessage";
        public const string TheFirstSalaryIsOutOfRangeOfTheSalaryLimits = "TheFirstSalaryIsOutOfRangeOfTheSalaryLimits";
        public const string PleaseLoginByUserName = "PleaseLoginByUserName";
        public const string OrderAlreadyexistMessage = "OrderAlreadyexistMessage";
        public const string NumberAlreadyexistMessage = "NumberAlreadyexistMessage";
        public const string ReleaseDateMustBeGreaterThanDateOfBirth = "ReleaseDateMustBeGreaterThanDateOfBirth";
        public const string StartDateMustBeGreaterThanDateOfBirth = "StartDateMustBeGreaterThanDateOfBirth";
        public const string IssuanceDateMustBeGreaterThanDateOfBirth = "IssuanceDateMustBeGreaterThanDateOfBirth";
        public const string DeleteConfirmationMessage = "AreYouSureYouWantToDelete?";
        public const string CannotDeleteBecauseItHasChildrenMessage = "YouCan'tDeleteThisBecauseItHasChildren...";
        public const string CannotDeleteBecauseItRelatedToJobDescription = "CannotDeleteBecauseItRelatedToJobDescription";
        public const string RequiredMessage = "RequiredMessage";
        public const string InvalidDateMessage = "InvalidDateMessage";
        public const string InvalidTimeMessage = "InvalidTimeMessage";
        public const string InvalidFileSizeMessage = "InvalidFileSizeMessage";
        public const string InalidWeightSumMessage = "InalidWeightSumMessage";
        public const string LessThanMessage = "LessThanMessage";
        public const string LessThanEqMessage = "LessThanEqMessage";
        public const string GreaterThanMessage = "GreaterThanMessage";
        public const string PeriodErrorMessage = "PeriodErrorMessage";
        public const string GreaterThanEqMessage = "GreaterThanEqMessage";
        public const string InvalidExtensionMessage = "InvalidExtensionMessage";
        public const string ExceptionMessage = "ExceptionMessage";
        public const string Select = "Select";
        public const string Calc = "Calc";
        public const string Browse = "Browse";
        public const string Clear = "Clear";
        public const string FilterBy = "FilterBy";
        public const string Template = "Template";
        public const string Section = "Section";
        public const string SectionWeight = "SectionWeight";

        public const string Loading = "Loading";
        public const string Finish = "Finish";
        public const string Appraisal = "Appraisal";
        public const string App = "App";
        public const string Approval = "Approval";
        public const string Tracking = "Tracking";
        public const string SetAsDefaultLanguage = "SetAsDefaultLanguage";



        public const string Copyright = "Copyright";
        public const string Maestro = "Maestro";
       

        public const string DefaultDetailsGroupTitle = "DefaultDetailsGroupTitle";

        public const string PendingCustodiesRequest = "PendingCustodiesRequest";
        
        public const string ViewsTitle = "ViewsTitle";
        public const string DefaultViewTitle = "DefaultViewTitle";
        public const string SimpleViewTitle = "SimpleViewTitle";

        public const string PercentageOfSalaryIncrease = "PercentageOfSalaryIncrease";

        public const string Mark = "Mark";
        public const string MarkValue = "MarkValue";
        public const string Mass = "Mass";
        public const string Employee = "Employee";
        public const string EmployeeInfo = "EmployeeInfo";
        public const string PhaseInfo = "PhaseInfo";
        public const string MarkBelowExpected = "MarkBelowExpected";
        public const string MarkNeedTraining = "MarkNeedTraining";
        public const string MarkExpected = "MarkExpected";
        public const string MarkUpExpected = "MarkUpExpected";
        public const string MarkDistinct = "MarkDistinct";
        public const string MarkNeedIntenseIncentive = "MarkNeedIntenseIncentive";
        public const string MarkNeedIncentive = "MarkNeedIncentive";
        public const string AlreadyExistsMessage = "AlreadyExistsMessage";
        public const string AddItem = "AddItem";
        public const string CompetencySectionDescriptionIsRequired = "CompetencySectionDescriptionIsRequired";
        public const string ObjectiveSectionDescriptionIsRequired = "ObjectiveSectionDescriptionIsRequired";
        public const string JobDescriptionSectionDescriptionIsRequired = "JobDescriptionSectionDescriptionIsRequired";
        public const string CustomSectionDescriptionIsRequired = "CustomSectionDescriptionIsRequired";
        public const string JobDescriptionInformation = "JobDescriptionInformation";
        public const string ObjectiveInformation = "ObjectiveInformation";
        public const string MsgActualStartDateMustBeLessThanActualEndDate = "MsgActualStartDateMustBeLessThanActualEndDate";
        public const string RoleName = "RoleName";
        public const string RoleWeight = "RoleWeight";
        public const string DevelopmentWindow = "DevelopmentWindow";
        public const string SelectEmployee = "SelectEmployee";
        public const string WorkflowTree = "WorkflowTree";
        public const string CourseName = "CourseName";
        public const string Specialize = "Specialize";
        public const string Priority = "Priority";
        public const string NumberOfEmployees = "NumberOfEmployees";
        public const string NumberOfSession = "NumberOfSession";
        public const string Duration = "Duration";
        public const string Status = "Status";
        public const string Number = "Number";
        public const string AddSelected = "AddSelected";
        public const string DeleteSelected = "DeleteSelected";
        public const string SuggestedCourses = "SuggestedCourses";
        public const string PlannedCourses = "PlannedCourses";
        public const string CourseCancellation = "CourseCancellation";
        public const string CostsInformation = "CostsInformation";
        public const string AllEmployees = "AllEmployees";
        public const string CandidateEmployees = "CandidateEmployees";
        public const string AttendanceEmployees = "AttendanceEmployees";
        public const string CourseInformation = "CourseInformation";
        public const string AddCoursesFromNeed = "AddCoursesFromNeed";
        public const string FromDate = "FromDate";
        public const string ToDate = "ToDate";
        public const string Subject = "Subject";
        public const string Notes = "Notes";
        public const string Body = "Body";
        public const string NotificationType = "NotificationType";
        public const string Close = "Close";
        public const string ApproachName = "ApproachName";
        public const string SaveResult = "SaveResult";
        public const string Delegate = "Delegate";
        public const string Survey = "Survey";
        public const string Authorities = "Authorities";
        public const string Delegation = "Delegation";
        public const string DelegationHistory = "DelegationHistory";
        public const string SaveAndClose = "SaveAndClose";
        public const string CalculateAndClose = "CalculateAndClose";
        public const string PendingDisciplinaryRequest = "PendingDisciplinaryRequest";
        public const string PendingRewardRequest = "PendingRewardRequest";
        public const string PendingTerminationRequest = "PendingTerminationRequest";
        public const string PendingPromotionRequest = "PendingPromotionRequest";
        public const string PendingLeaveRequest = "PendingLeaveRequest";
        public const string PendingMissionRequest = "PendingMissionRequest";
        public const string PendingTransferRequest = "PendingTransferRequest"; 
        public const string PendingLoanRequest = "PendingLoanRequest";
        public const string PendingEntranceExitRequest = "PendingEntranceExitRequest"; 
        public const string PendingFinancialPromotionRequest = "PendingFinancialPromotionRequest";
        public const string PendingResignationRequest = "PendingResignationRequest";
        public const string InterviewQuestions = "InterviewQuestions";
        public const string BasicInfo = "BasicInfo";
        public const string AutoGenerateAttendanceRecords = "AutoGenerateAttendanceRecords";
        public const string YouCanNotAddMoreThenOneCodeSetting = "YouCanNotAddMoreThenOneCodeSetting";
        public const string YouMustAddEmployeeCodeSetting = "YouMustAddEmployeeCodeSetting";
        public const string BiometricInteraction = "BiometricInteraction";
        public const string ORC = "ORC";
        public const string OrderingParentFailerMessage = "YouCan'tParentNodeToChildNode";
        public const string OrderingFailerMessage = "YouCan'tAddOrUpdateToThisNodeTypeBecauseOfParentNodeType";  
        public const string NodeTypeMustBeMoreThanSelectedParentType = "NodeTypeMustBeMoreThanSelectedParentType";
        public const string UpdateReportsValues = "UpdateReportsValues";
        public const string GenerateValues = "GenerateValues";
        public const string GenerateReportsValues = "GenerateReportsValues";
        public const string ImportValues = "ImportValues";
        public const string ExportValues = "ExportValues";
        public const string Entrance = "Entrance";
        public const string OrganizationRequest = "OrganizationRequest";
        public const string YouCannotAddSpouseToSingleEmployee = "YouCannotAddSpouseToSingleEmployee";
        public const string YouCannotAddChildToSingleEmployee = "YouCannotAddChildToSingleEmployee";
        public const string FirstNameInEmployeeInformationDiffersFromResidenceInformation = "FirstNameInEmployeeInformationDiffersFromResidenceInformation";
        public const string LastNameInEmployeeInformationDiffersFromResidenceInformation = "LastNameInEmployeeInformationDiffersFromResidenceInformation";
        public const string FatherNameInEmployeeInformationDiffersFromResidenceInformation = "FatherNameInEmployeeInformationDiffersFromResidenceInformation";
        public const string MotherNameInEmployeeInformationDiffersFromResidenceInformation = "MotherNameInEmployeeInformationDiffersFromResidenceInformation";
        public const string FirstNameInEmployeeInformationDiffersFromPassportInformation = "FirstNameInEmployeeInformationDiffersFromPassportInformation";
        public const string LastNameInEmployeeInformationDiffersFromPassportInformation = "LastNameInEmployeeInformationDiffersFromPassportInformation";
        public const string FatherNameInEmployeeInformationDiffersFromPassportInformation = "FatherNameInEmployeeInformationDiffersFromPassportInformation";
        public const string MotherNameInEmployeeInformationDiffersFromPassportInformation = "MotherNameInEmployeeInformationDiffersFromPassportInformation";
        public const string FirstNameL2InEmployeeInformationDiffersFromPassportInformation = "FirstNameL2InEmployeeInformationDiffersFromPassportInformation";
        public const string LastNameL2InEmployeeInformationDiffersFromPassportInformation = "LastNameL2InEmployeeInformationDiffersFromPassportInformation";
        public const string AppraisalResult = "AppraisalResult";
        public const string PendingAppraisal = "PendingAppraisal";
        public const string All = "All";
        public const string MaritalStatusCannotBeSingle = "MaritalStatusCannotBeSingle";
        public const string DutyDateMustBeGreaterThanStartWorkingDate = "DutyDateMustBeGreaterThanStartWorkingDate";
        public const string CustodyStartDateMustBeGreaterThanStartWorkingDate = "CustodyStartDateMustBeGreaterThanStartWorkingDate";
        public const string CustodyStartDateMustBeGreaterThanPurchaseDate = "CustodyStartDateMustBeGreaterThanPurchaseDate";
        public const string DateOfIssuanceMustBeGreaterThanDateOfBirth = "DateOfIssuanceMustBeGreaterThanDateOfBirth";
        public const string ExpirationDateMustBeGreaterThanDateOfIssuance = "ExpirationDateMustBeGreaterThanDateOfIssuance";
        public const string FromDateMustBeGreaterThanStartWorkingDate = "FromDateMustBeGreaterThanStartWorkingDate";
        public const string ToDateMustBeGreaterThanFromDate = "ToDateMustBeGreaterThanFromDate";
        public const string OrderInFamilyAlreadyExists = "OrderInFamilyAlreadyExists";
        public const string Confirm = "Confirm";
        public const string AreYouSureYouWantToAcceptEntranceExitRecord = "AreYouSureYouWantToAcceptEntranceExitRecord";
        public const string AreYouSureYouWantToAcceptPenalty = "AreYouSureYouWantToAcceptPenalty";
        public const string ThisMemberIsAlreadyExist = "ThisMemberIsAlreadyExist";
        public const string StartSlice = "StartSlice";
        public const string EndSlice = "EndSlice";
        public const string InfractionForm = "InfractionForm";
        public const string PercentageOrder = "PercentageOrder";
        public const string Percentage = "Percentage";
        public const string ExportToCvs = "ExportToCvs";
        public const string TheMinSalaryGreaterThanMaxSalary = "TheMinSalaryGreaterThanMaxSalary";
        public const string AreYouSureYouWantToDeleteEntranceExitRecord = "AreYouSureYouWantToDeleteEntranceExitRecord";
        public const string AddNonAttendanceSlices = "AddNonAttendanceSlices";
        public const string AddNonAttendanceSlicePercentages = "AddNonAttendanceSlicePercentages";
        public const string AreYouSureYouWantToGenerateEntranceExitRecordErrors = "AreYouSureYouWantToGenerateEntranceExitRecordErrors";
        public const string OutOfShiftRange = "OutOfShiftRange";
        public const string YouCanNotAddMoreThenOneGeneralSetting = "YouCanNotAddMoreThenOneGeneralSetting";
        public const string ThereIsAnyStepMinSalaryLessThanGradeMinSalary = "ThereIsAnyStepMinSalaryLessThanGradeMinSalary";
        public const string ThereIsAnyStepMaxSalaryGreaterThanGradeMaxSalary = "ThereIsAnyStepMaxSalaryGreaterThanGradeMaxSalary";
        public const string GenerateBasicReports = "GenerateBasicReports";
        public const string DeleteFilteredReports = "DeleteFilteredReports";
        public const string GenerateBasicReportsConfirm = "GenerateBasicReportsConfirm";
        public const string EndDateMustBeLessThanNowDate = "EndDateMustBeLessThanNowDate";
        public const string MustBeTheDefaultLanguage = "MustBeTheDefaultLanguage";
        public const string CouldNotGenerateValuesForReports = "CouldNotGenerateValuesForReports";
        public const string FailedToDeploy = "FailedToDeploy";
        public const string FailedToGenerateValuesForSomeReports = "FailedToGenerateValuesForSomeReports";
        public const string SelectingFileIsRequired = "SelectingFileIsRequired";
        public const string InvalidFileExtension = "InvalidFileExtension";
        public const string Approved = "Approved";
        public const string WaitingApproved = "WaitingApproved";
        public const string Year = "Year";
        public const string ImportEntranceExitRecords = "ImportEntranceExitRecords";
        public const string SelectXLSXFileToImportOrDragAndDropFilesToTheBelowRectangle = "SelectXLSXFileToImportOrDragAndDropFilesToTheBelowRectangle";
        public const string SelectFiles = "SelectFiles";
        
        public const string Info = "Info";
        public const string Filter = "Filter";
        public const string ClearFilter = "ClearFilter";
        public const string IsTrue = "IsTrue";
        public const string IsFalse = "IsFalse";
        public const string And = "And";
        public const string Or = "Or";

        public const string Equals = "Equals";
        public const string NotEquals = "NotEquals";
        public const string StartsWith = "StartsWith";
        public const string Contains = "Contains";
        public const string EndsWith = "EndsWith";

        public const string GreaterThanEqualTo = "GreaterThanEqualTo";
        public const string GreaterThan = "GreaterThan";
        public const string LessThanEqualTo = "LessThanEqualTo";
        public const string LessThan = "LessThan";

        public const string Display = "Display";
        public const string Empty = "Empty";
        public const string Page = "Page";
        public const string Of = "Of";
        public const string ItemsPerPage = "ItemsPerPage";
        public const string First = "First";
        public const string Last = "Last";
        public const string Refresh = "Refresh";
        public const string PendingAdvanceRequest = "PendingAdvanceRequest";
        public const string OrganizationChartTree = "OrganizationChartTree";
        public const string ForceClose = "ForceClose";
        public const string TheEmployeeHasNotPendingWorkflows = "TheEmployeeHasNotPendingWorkflows"; 
        public static string GetResource(string key)
        {
            var result = ServiceFactory.LocalizationService.GetResource(ResourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }

        public static string GetResourceForEnum(Enum e)
        {
            var key = string.Format("{0}.{1}", e.GetType().FullName, e.ToString());
            var result = ServiceFactory.LocalizationService.GetResource(key);
            return string.IsNullOrEmpty(result) ? e.ToString() : result;
        }

    }

    public class GlobalResource
    {
        public static string OrganizationChartTree
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.OrganizationChartTree); }
        }
        public static string ForceClose
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ForceClose); }
        }
        public static string ClearFilter
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ClearFilter); }
        }
        public static string Total
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Total); }
        }
        
        public static string Page
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Page); }
        }
        public static string Of
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Of); }
        }
        public static string ItemsPerPage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ItemsPerPage); }
        }
        public static string First
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.First); }
        }
        public static string Last
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Last); }
        }
        public static string Refresh
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Refresh); }
        }
        public static string Info
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Info); }
        }
        public static string Filter
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Filter); }
        }
        public static string IsTrue
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.IsTrue); }
        }
        public static string IsFalse
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.IsFalse); }
        }
        public static string And
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.And); }
        }
        public static string Or
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Or); }
        }
        public static string Equals
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Equals); }
        }
        public static string NotEquals
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.NotEquals); }
        }
        public static string StartsWith
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.StartsWith); }
        }
        public static string Contains
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Contains); }
        }
        public static string EndsWith
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.EndsWith); }
        }

        public static string GreaterThanEqualTo
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.GreaterThanEqualTo); }
        }
        public static string Entrance
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Entrance); }
        }
        public static string GreaterThan
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.GreaterThan); }
        }

        public static string LessThanEqualTo
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.LessThanEqualTo); }
        }
        public static string LessThan
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.LessThan); }
        }

        public static string Display
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Display); }
        }
        public static string Empty
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Empty); }
        }
        public static string Year
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Year); }
        }
        public static string Approved
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Approved); }
        }
        public static string WaitingApproved
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.WaitingApproved); }
        }
        public static string TheFileExtensionIsInvalid
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.TheFileExtensionIsInvalid); }
        }
        public static string InvalidFileExtension
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.InvalidFileExtension); }
        }
        public static string SelectingFileIsRequired
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.SelectingFileIsRequired); }
        }
        public static string ExportToCvs
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ExportToCvs); }
        }
        public static string EndDateMustBeLessThanNowDate
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.EndDateMustBeLessThanNowDate); }
        }
        public static string Manager
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Manager); }
        }
        public static string TheMinSalaryGreaterThanMaxSalary
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.TheMinSalaryGreaterThanMaxSalary); }
        }
        public static string SelectedRole
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.SelectedRole); }
        }

        public static string Admin
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Admin); }
        }
        public static string OrderInFamilyAlreadyExists
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.OrderInFamilyAlreadyExists); }
        }

        public static string HomePageTitle
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.HomePageTitle); }
        }
        public static string HomePageDescription
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.HomePageDescription); }
        }
        public static string General
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.General); }
        }
        public static string Name
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Name); }
        }
        public static string AlreadyExistsMessage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AlreadyExistsMessage); }
        }
        
        
        public static string FullName
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.FullName); }
        }
        public static string TripleName
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.TripleName); }
        }
        public static string AddNotificationTitle
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AddNotificationTitle); }
        }
        public static string DeleteNotificationTitle
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.DeleteNotificationTitle); }
        }
        public static string UpdateNotificationTitle
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.UpdateNotificationTitle); }
        }
        public static string StartDate
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.StartDate); }
        }
        public static string EndDate
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.EndDate); }
        }
        public static string EnglishMark
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.EnglishMark); }
        }
        public static string TestDate
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.TestDate); }
        }
        public static string IsChecked
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.IsChecked); }
        }
        public static string MessageDetailsValidationError
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.MessageDetailsValidationError); }
        }
        public static string Document
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Document); }
        }
        #region navigation
        public static string Dashboard
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Dashboard); }
        }
        public static string Aggregate
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Aggregate); }
        }

        public static string ManageFieldSecurity
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ManageFieldSecurity); }
        }
        public static string ShowFields
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ShowFields); }
        }
        public static string HiddenFields
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.HiddenFields); }
        }
        public static string AggregatesFields
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AggregatesFields); }
        }
        public static string Details
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Details); }
        }
        public static string DetailsFields
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.DetailsFields); }
        }
        public static string ActionList
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ActionList); }
        }
        public static string Index
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Index); }
        }
        public static string Service
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Service); }
        }
        public static string Report
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Report); }
        }
        public static string Configuration
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Configuration); }
        }
        public static string ConfigurationsFields
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ConfigurationFields); }
        }
        public static string AvailableModules
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AvailableModules); }
        }
        public static string AssignedModules
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AssignedModules); }
        }
        public static string AvailableDashboards
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AvailableDashboards); }
        }
        public static string AssignedDashboards
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AssignedDashboards); }
        }
        public static string AvailableAggregates
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AvailableAggregates); }
        }
        public static string AssignedAggregates
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AssignedAggregates); }
        }
        public static string AvailableDetails
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AvailableDetails); }
        }
        public static string AssignedDetails
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AssignedDetails); }
        }
        public static string AvailableActionLists
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AvailableActionLists); }
        }
        public static string AssignedActionLists
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AssignedActionLists); }
        }
        public static string AvailableIndexs
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AvailableIndexs); }
        }
        public static string AssignedIndexs
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AssignedIndexs); }
        }
        public static string AvailableServices
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AvailableServices); }
        }
        public static string AssignedServices
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AssignedServices); }
        }
        public static string AvailableReports
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AvailableReports); }
        }
        public static string AssignedReports
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AssignedReports); }
        }
        public static string AvailableConfigurations
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AvailableConfigurations); }
        }
        public static string AssignedConfigurations
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AssignedConfigurations); }
        }

        public static string RoleManagement
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.RoleManagement); }
        }

        public static string Editable
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Editable); }
        }
        public static string Insertable
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Insertable); }
        }
        public static string Deleteable
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Deleteable); }
        }
        public static string EditableAll
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.EditableAll); }
        }
        public static string InsertableAll
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.InsertableAll); }
        }
        public static string DeleteableAll
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.DeleteableAll); }
        }
        public static string UnEditable
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.UnEditable); }
        }
        public static string UnInsertable
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.UnInsertable); }
        }
        public static string UnDeleteable
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.UnDeleteable); }
        }
        public static string UnEditableAll
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.UnEditableAll); }
        }
        public static string UnInsertableAll
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.UnInsertableAll); }
        }
        public static string UnDeleteableAll
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.UnDeleteableAll); }
        }
        #endregion
        public static string Node
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Node); }
        }

        public static string Age
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Age); }
        }
        public static string Code
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Code); }
        }
        public static string Phase
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Phase); }
        }
        public static string PositionCode
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.PositionCode); }
        }
        public static string Value
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Value); }
        }
        public static string Description
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Description); }
        }
        public static string PercentageOfCompletion
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.PercentageOfCompletion); }
        }
        public static string ExpectedResult
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ExpectedResult); }
        }
        public static string Planning
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Planning); }
        }
        public static string JobDescription
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.JobDescription); }
        }

        public static string EmploymentStatus
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.EmploymentStatus); }
        }
        public static string EmployeeName
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.EmployeeName); }
        }
        public static string EmployeeStatus
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.EmployeeStatus); }
        }
        public static string Date
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Date); }
        }
        public static string LeaveRequesTypeTitle
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.LeaveRequesTypeTitle); }
        }




        public static string JobTitle
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.JobTitle); }
        }
        public static string Grade
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Grade); }
        }
        public static string Calc
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Calc); }
        }
        public static string Type
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Type); }
        }
        public static string AdditionalType
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AdditionalType); }
        }
        public static string OrgLevel
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.OrgLevel); }
        }
        public static string Position
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Position); }
        }
        public static string Order
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Order); }
        }
        public static string Fixed
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Fixed); }
        }
        public static string Custom
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Custom); }
        }
        public static string Item
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Item); }
        }
        public static string Items
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Items); }
        }
        public static string Title
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Title); }
        }
        public static string Weight
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Weight); }
        }
        public static string Kpi
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Kpi); }
        }
        public static string Kpis
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Kpis); }
        }

        public static string Save
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Save); }
        }
        public static string Module
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Module); }
        }
        public static string Role
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Role); }
        }
        public static string ChangePassword
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ChangePassword); }
        }

        public static string Apply
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Apply); }
        }
        public static string Accept
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Accept); }
        }
        public static string Reject
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Reject); }
        }
        public static string Pending
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Pending); }
        }
        public static string Owner
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Owner); }
        }
        public static string ActionPlan
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ActionPlan); }
        }
        public static string Actually
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Actually); }
        }
        public static string ActuallyEndDate
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ActuallyEndDate); }
        }
        public static string ActuallyStartDate
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ActuallyStartDate); }
        }
        public static string Option
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Option); }
        }

        public static string Options
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Options); }
        }

        public static string Cancel
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Cancel); }
        }
        public static string ImportEntranceExitRecords
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ImportEntranceExitRecords); }
        }
        public static string SelectXLSXFileToImportOrDragAndDropFilesToTheBelowRectangle
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.SelectXLSXFileToImportOrDragAndDropFilesToTheBelowRectangle); }
        }
        public static string SelectFiles
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.SelectFiles); }
        }
        
        public static string Yes
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Yes); }
        }
        public static string No
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.No); }
        }
        public static string Min
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Min); }
        }
        public static string Max
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Max); }
        }
        public static string Update
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Update); }
        }
        public static string Add
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Add); }
        }
        public static string Edit
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Edit); }
        }
        public static string Next
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Next); }
        }
        public static string Previous
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Previous); }
        }
        public static string Remove
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Remove); }
        }
        public static string Delete
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Delete); }
        }
        public static string More
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.More); }
        }
        public static string Help
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Help); }
        }
        public static string Logout
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Logout); }
        }
        public static string Settings
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Settings); }
        }
        public static string Exit
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Exit); }
        }
        public static string Print
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Print); }
        }
        public static string Fail
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Fail); }
        }
        public static string Done
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Done); }
        }
        public static string Success
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Success); }
        }
        public static string Pair
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Pair); }
        }
        public static string FailMessage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.FailMessage); }
        }
        public static string CantTermintatePreparationPeriodThisEmployeeBecauseHisCardStatusIsNotNew
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.CantTermintatePreparationPeriodThisEmployeeBecauseHisCardStatusIsNotNew); }
        }
        public static string TheEmployeeHasNotPendingWorkflows
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.TheEmployeeHasNotPendingWorkflows); }
        }
        public static string YouCanNotDoingThisProcessBecauseTheEmployeeHasPendingWorkflowsPleaseCheckHisWorkflows
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.YouCanNotDoingThisProcessBecauseTheEmployeeHasPendingWorkflowsPleaseCheckHisWorkflows); }
        }
        public static string DoneMessage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.DoneMessage); }
        }
        public static string SuccessMessage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.SuccessMessage); }
        }

        public static string RequiredMessage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.RequiredMessage); }
        }

        public static string DeleteConfirmationMessage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.DeleteConfirmationMessage); }
        }

        public static string CannotDeleteBecauseItHasChildrenMessage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.CannotDeleteBecauseItHasChildrenMessage); }
        }

        public static string CannotDeleteBecauseItRelatedToJobDescription
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.CannotDeleteBecauseItRelatedToJobDescription); }
        }
        public static string AlreadyexistMessage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AlreadyexistMessage); }
        }
        public static string TheFirstSalaryIsOutOfRangeOfTheSalaryLimits
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.TheFirstSalaryIsOutOfRangeOfTheSalaryLimits); }
        }
        public static string PleaseLoginByUserName
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.PleaseLoginByUserName); }
        }
        public static string OrderAlreadyexistMessage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.OrderAlreadyexistMessage); }
        }
        public static string NumberAlreadyexistMessage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.NumberAlreadyexistMessage); }
        }
        public static string ReleaseDateMustBeGreaterThanDateOfBirth
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ReleaseDateMustBeGreaterThanDateOfBirth); }
        }
        public static string StartDateMustBeGreaterThanDateOfBirth
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.StartDateMustBeGreaterThanDateOfBirth); }
        }
        public static string IssuanceDateMustBeGreaterThanDateOfBirth
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.IssuanceDateMustBeGreaterThanDateOfBirth); }
        }
        public static string InvalidDateMessage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.InvalidDateMessage); }
        }
        public static string InvalidTimeMessage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.InvalidTimeMessage); }
        }
        public static string InalidWeightSumMessage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.InalidWeightSumMessage); }
        }

        public static string InvalidFileSizeMessage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.InvalidFileSizeMessage); }
        }

        public static string LessThanMessage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.LessThanMessage); }
        }

        public static string LessThanEqMessage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.LessThanEqMessage); }
        }

        public static string GreaterThanMessage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.GreaterThanMessage); }
        }

        public static string GreaterThanEqMessage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.GreaterThanEqMessage); }
        }
        public static string PeriodErrorMessage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.PeriodErrorMessage); }
        }
        public static string ExceptionMessage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ExceptionMessage); }
        }

        public static string InvalidExtensionMessage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.InvalidExtensionMessage); }
        }


        public static string Loading
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Loading); }
        }
        public static string Finish
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Finish); }
        }


        public static string Error
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Error); }
        }
        public static string Warning
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Warning); }
        }
        public static string Information
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Information); }
        }

        public static string Ok
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Ok); }
        }
        public static string SetAsDefaultLanguage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.SetAsDefaultLanguage); }
        }
        public static string Copyright
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Copyright); }
        }
        public static string Maestro
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Maestro); }
        }
        public static string Select
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Select); }
        }
        public static string Browse
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Browse); }
        }
        public static string Clear
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Clear); }
        }
        public static string FilterBy
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.FilterBy); }
        }
        public static string Template
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Template); }
        }
        public static string Section
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Section); }
        }
        public static string SectionWeight
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.SectionWeight); }
        }
        public static string SectionValue
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.SectionValue); }
        }
        public static string Appraisal
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Appraisal); }
        }
        public static string App
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.App); }
        }
        public static string Approval
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Approval); }
        }
        public static string Tracking
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Tracking); }
        }
        

        public static string ViewsTitle
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ViewsTitle); }
        }

        public static string DefaultViewTitle
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.DefaultViewTitle); }
        }

        public static string SimpleViewTitle
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.SimpleViewTitle); }
        }


        public static string DefaultDetailsGroupTitle
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.DefaultDetailsGroupTitle); }
        }
        public static string PendingCustodiesRequest
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.PendingCustodiesRequest); }
        }
        
        public static string PercentageOfSalaryIncrease
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.PercentageOfSalaryIncrease); }
        }

        #region PMS_Appraisal_and_Incentive_Phase_Setting

        public static string Mark
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Mark); }
        }

        public static string Mass
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Mass); }
        }

        public static string PhaseInfo
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.PhaseInfo); }
        }
        public static string EmployeeInfo
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.EmployeeInfo); }
        }
        public static string Employee
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Employee); }
        }

        public static string MarkBelowExpected
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.MarkBelowExpected); }
        }

        public static string MarkNeedTraining
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.MarkNeedTraining); }
        }

        public static string MarkExpected
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.MarkExpected); }
        }

        public static string MarkUpExpected
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.MarkUpExpected); }
        }

        public static string MarkDistinct
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.MarkDistinct); }
        }

        public static string MarkNeedIntenseIncentive
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.MarkNeedIntenseIncentive); }
        }

        public static string MarkNeedIncentive
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.MarkNeedIncentive); }
        }

        #endregion

        public static string AddItem
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AddItem); }
        }

        public static string CompetencySectionDescriptionIsRequired
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.CompetencySectionDescriptionIsRequired); }
        }
        public static string ObjectiveSectionDescriptionIsRequired
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ObjectiveSectionDescriptionIsRequired); }
        }

        public static string JobDescriptionSectionDescriptionIsRequired
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.JobDescriptionSectionDescriptionIsRequired); }
        }

        public static string CustomSectionDescriptionIsRequired
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.CustomSectionDescriptionIsRequired); }
        }
        public static string JobDescriptionInformation
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.JobDescriptionInformation); }
        }
        public static string ObjectiveInformation
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ObjectiveInformation); }
        }
        public static string MsgActualStartDateMustBeLessThanActualEndDate
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.MsgActualStartDateMustBeLessThanActualEndDate); }
        }
        public static string Roles
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Roles); }
        }
        public static string RoleName
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.RoleName); }
        }
        public static string RoleWeight
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.RoleWeight); }
        }
        public static string DevelopmentWindow
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.DevelopmentWindow); }
        }
        public static string SelectEmployee
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.SelectEmployee); }
        }
        public static string WorkflowTree
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.WorkflowTree); }
        }
        public static string CourseName
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.CourseName); }
        }
        public static string Specialize
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Specialize); }
        }
        public static string Priority
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Priority); }
        }
        public static string NumberOfEmployees
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.NumberOfEmployees); }
        }
        public static string NumberOfSession
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.NumberOfSession); }
        }
        public static string Duration
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Duration); }
        }
        public static string Status
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Status); }
        }
        public static string Number
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Number); }
        }
        public static string AddSelected
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AddSelected); }
        }
        public static string DeleteSelected
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.DeleteSelected); }
        }
        public static string SuggestedCourses
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.SuggestedCourses); }
        }
        public static string PlannedCourses
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.PlannedCourses); }
        }
        public static string CourseCancellation
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.CourseCancellation); }
        }
        public static string CostsInformation
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.CostsInformation); }
        }
        public static string AllEmployees
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AllEmployees); }
        }
        public static string CandidateEmployees
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.CandidateEmployees); }
        }
        public static string AttendanceEmployees
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AttendanceEmployees); }
        }
        public static string CourseInformation
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.CourseInformation); }
        }
        public static string AddCoursesFromNeed
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AddCoursesFromNeed); }
        }

        public static string FromDate
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.FromDate); }
        }
        public static string ToDate
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ToDate); }
        }
        public static string Notes
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Notes); }
        }
        
        public static string Subject
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Subject); }
        }
        public static string Body
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Body); }
        }

        public static string NotificationType
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.NotificationType); }
        }
        public static string OrderingParentFailerMessage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.OrderingParentFailerMessage); }
        }
        public static string OrderingFailerMessage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.OrderingFailerMessage); }
        }


        public static string Close
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Close); }
        }
        public static string ApproachName
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ApproachName); }
        }
        public static string SaveResult
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.SaveResult); }
        }
        public static string Delegate
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Delegate); }
        }
        public static string Survey
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Survey); }
        }
        public static string Authorities
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Authorities); }
        }

        public static string Delegation
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Delegation); }
        }

        public static string DelegationHistory
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.DelegationHistory); }
        }

        public static string SaveAndClose
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.SaveAndClose); }
        }

        public static string CalculateAndClose
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.CalculateAndClose); }
        }
        public static string PendingDisciplinaryRequest
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.PendingDisciplinaryRequest); }
        }
        public static string PendingRewardRequest
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.PendingRewardRequest); }
        }
        public static string PendingTerminationRequest
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.PendingTerminationRequest); }
        }
        public static string PendingPromotionRequest
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.PendingPromotionRequest); }
        }
        public static string PendingLeaveRequest
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.PendingLeaveRequest); }
        }

        public static string PendingMissionRequest
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.PendingMissionRequest); }
        }
        public static string PendingEntranceExitRequest
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.PendingEntranceExitRequest); }
        }
        public static string PendingTransferRequest
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.PendingTransferRequest); }
        }
        public static string PendingLoanRequest
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.PendingLoanRequest); }
        }
        public static string PendingFinancialPromotionRequest
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.PendingFinancialPromotionRequest); }
        }
        public static string PendingResignationRequest
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.PendingResignationRequest); }
        }
        public static string InterviewQuestions
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.InterviewQuestions); }
        }
        public static string BasicInfo
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.BasicInfo); }
        }
        public static string YouCanNotAddMoreThenOneCodeSetting
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.YouCanNotAddMoreThenOneCodeSetting); }
        }
        public static string YouMustAddEmployeeCodeSetting
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.YouMustAddEmployeeCodeSetting); }
        }

        public static string BiometricInteraction
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.BiometricInteraction); }
        }

        public static string AutoGenerateAttendanceRecords
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AutoGenerateAttendanceRecords); }
        }

        public static string ORC
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ORC); }
        }
        public static string NodeTypeMustBeMoreThanSelectedParentType
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.NodeTypeMustBeMoreThanSelectedParentType); }
        }
        public static string GenerateValues
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.GenerateValues); }
        }
        public static string GenerateReportsValues
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.GenerateReportsValues); }
        }
        public static string UpdateReportsValues
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.UpdateReportsValues); }
        }
        public static string ExportValues
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ExportValues); }
        }
        public static string ImportValues
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ImportValues); }
        }
        public static string OrganizationRequest
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.OrganizationRequest); }
        }

        public static string YouCannotAddSpouseToSingleEmployee
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.YouCannotAddSpouseToSingleEmployee); }
        }

        public static string YouCannotAddChildToSingleEmployee
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.YouCannotAddChildToSingleEmployee); }
        }
        public static string DutyDateMustBeGreaterThanStartWorkingDate
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.DutyDateMustBeGreaterThanStartWorkingDate); }
        }
        public static string ToDateMustBeGreaterThanFromDate
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ToDateMustBeGreaterThanFromDate); }
        }
        public static string FromDateMustBeGreaterThanStartWorkingDate
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.FromDateMustBeGreaterThanStartWorkingDate); }
        }
        
        public static string DateOfIssuanceMustBeGreaterThanDateOfBirth
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.DateOfIssuanceMustBeGreaterThanDateOfBirth); }
        }
        public static string ExpirationDateMustBeGreaterThanDateOfIssuance
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ExpirationDateMustBeGreaterThanDateOfIssuance); }
        }
        public static string CustodyStartDateMustBeGreaterThanStartWorkingDate
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.CustodyStartDateMustBeGreaterThanStartWorkingDate); }
        }
        public static string CustodyStartDateMustBeGreaterThanPurchaseDate
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.CustodyStartDateMustBeGreaterThanPurchaseDate); }
        }
        public static string FirstNameInEmployeeInformationDiffersFromResidenceInformation
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.FirstNameInEmployeeInformationDiffersFromResidenceInformation); }
        }
        public static string LastNameInEmployeeInformationDiffersFromResidenceInformation
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.LastNameInEmployeeInformationDiffersFromResidenceInformation); }
        }
        public static string FatherNameInEmployeeInformationDiffersFromResidenceInformation
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.FatherNameInEmployeeInformationDiffersFromResidenceInformation); }
        }
        public static string MotherNameInEmployeeInformationDiffersFromResidenceInformation
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.MotherNameInEmployeeInformationDiffersFromResidenceInformation); }
        }
        public static string FirstNameInEmployeeInformationDiffersFromPassportInformation
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.FirstNameInEmployeeInformationDiffersFromPassportInformation); }
        }
        public static string LastNameInEmployeeInformationDiffersFromPassportInformation
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.LastNameInEmployeeInformationDiffersFromPassportInformation); }
        }
        public static string FatherNameInEmployeeInformationDiffersFromPassportInformation
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.FatherNameInEmployeeInformationDiffersFromPassportInformation); }
        }
        public static string MotherNameInEmployeeInformationDiffersFromPassportInformation
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.MotherNameInEmployeeInformationDiffersFromPassportInformation); }
        }
        public static string FirstNameL2InEmployeeInformationDiffersFromPassportInformation
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.FirstNameL2InEmployeeInformationDiffersFromPassportInformation); }
        }
        public static string LastNameL2InEmployeeInformationDiffersFromPassportInformation
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.LastNameL2InEmployeeInformationDiffersFromPassportInformation); }
        }
        public static string AppraisalResult
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AppraisalResult); }
        }
        public static string PendingAppraisal
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.PendingAppraisal); }
        }
        
        public static string All
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.All); }
        }
        public static string MaritalStatusCannotBeSingle
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.MaritalStatusCannotBeSingle); }
        }
        public static string Confirm
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Confirm); }
        }
        public static string AreYouSureYouWantToAcceptEntranceExitRecord
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AreYouSureYouWantToAcceptEntranceExitRecord); }
        }
        public static string AreYouSureYouWantToAcceptPenalty
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AreYouSureYouWantToAcceptPenalty); }
        }
        public static string ThisMemberIsAlreadyExist
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ThisMemberIsAlreadyExist); }
        }
        public static string StartSlice
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.StartSlice); }
        }
        public static string EndSlice
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.EndSlice); }
        }
        public static string InfractionForm
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.InfractionForm); }
        }
        public static string PercentageOrder
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.PercentageOrder); }
        }
        public static string Percentage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.Percentage); }
        }
        public static string AreYouSureYouWantToDeleteEntranceExitRecord
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AreYouSureYouWantToDeleteEntranceExitRecord); }
        }
        public static string AddNonAttendanceSlices
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AddNonAttendanceSlices); }
        }
        public static string AddNonAttendanceSlicePercentages
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AddNonAttendanceSlicePercentages); }
        }
        public static string AreYouSureYouWantToGenerateEntranceExitRecordErrors
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.AreYouSureYouWantToGenerateEntranceExitRecordErrors); }
        }
        public static string OutOfShiftRange
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.OutOfShiftRange); }
        }
        public static string YouCanNotAddMoreThenOneGeneralSetting
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.YouCanNotAddMoreThenOneGeneralSetting); }
        }
        public static string ThereIsAnyStepMinSalaryLessThanGradeMinSalary
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ThereIsAnyStepMinSalaryLessThanGradeMinSalary); }
        }
        public static string ThereIsAnyStepMaxSalaryGreaterThanGradeMaxSalary
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.ThereIsAnyStepMaxSalaryGreaterThanGradeMaxSalary); }
        }
        public static string GenerateBasicReports
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.GenerateBasicReports); }
        }
        public static string DeleteFilteredReports
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.DeleteFilteredReports); }
        }
        public static string GenerateBasicReportsConfirm
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.GenerateBasicReportsConfirm); }
        }
        public static string MustBeTheDefaultLanguage
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.MustBeTheDefaultLanguage); }
        }
        public static string CouldNotGenerateValuesForReports
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.CouldNotGenerateValuesForReports); }
        }
        public static string FailedToDeploy
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.FailedToDeploy); }
        }
        public static string FailedToGenerateValuesForSomeReports
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.FailedToGenerateValuesForSomeReports); }
        }
        public static string PendingAdvanceRequest
        {
            get { return LocalizationHelper.GetResource(LocalizationHelper.PendingAdvanceRequest); }
        }
        
    }
}