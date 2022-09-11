using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Souccar.Core.Extensions;
using System.Web;

namespace Project.Web.Mvc4.Helpers.Resource
{
    public class AttendanceLocalizationHelper
    {
        public const string ResourceGroupName = "AttendanceLocalizationHelper";
        public const string BioMetricInteractionSupportedDevicesTitle = "BioMetricInteractionSupportedDevicesTitle";
        public const string AttendanceCardGenerated = "AttendanceCardGenerated";
        public const string AutoGenerateAttendanceRecordNoteIsRequired = "AutoGenerateAttendanceRecordNoteIsRequired";
        public const string AutoGenerateAttendanceRecords = "AutoGenerateAttendanceRecords";
        public const string EntranceTimeGreaterThanExitTime = "EntranceTimeGreaterThanExitTime";
        public const string MultipleEntrance = "MultipleEntrance";
        public const string OutOfShiftRange = "OutOfShiftRange";
        public const string MultipleExit = "MultipleExit";
        public const string ExitWithoutEntrance = "ExitWithoutEntrance";
        public const string EntranceWithoutExit = "EntranceWithoutExit";
        public const string AutoGenerateAttendanceRecordToDateMustBeGreaterThanEqualToFromDate = "AutoGenerateAttendanceRecordToDateMustBeGreaterThanEqualToFromDate";
        public const string BioMetricInteraction = "BioMetricInteraction";
        public const string BioMetricInteractionClearDataFromBioMetricTitle = "BioMetricInteractionClearDataFromBioMetricTitle";
        public const string AttendanceDetails = "AttendanceDetails";
        public const string BioMetricInteractionTransferDataFromBioMetricTitle = "BioMetricInteractionTransferDataFromBioMetricTitle";
        public const string BioMetricInteractionIgnoreFingerPrintTypeTitle = "BioMetricInteractionIgnoreFingerPrintTypeTitle";
        public const string CalculateTitle = "CalculateTitle";
        public const string CannotCalculateLockedOrCreatedAttendanceRecords = "CannotCalculateLockedOrCreatedAttendanceRecords";
        public const string CannotGenerateLockedAttendanceRecord = "CannotGenerateLockedAttendanceRecord";
        //public const string CheckAttendanceRecordStabilityFaild = "CheckAttendanceRecordStabilityFaild";
        public const string ChooseBioMetricActionsDeviceBeforeExecuteMessage = "ChooseBioMetricActionsDeviceBeforeExecuteMessage";
        public const string ConflictSlices = "ConflictSlices";
        public const string EmployeeDoesNotHasEmployeeAttendanceCard = "EmployeeDoesNotHasEmployeeAttendanceCard";
        public const string EntryTimeCannotBeLessThanShiftRangeStartTime = "EntryTimeCannotBeLessThanShiftRangeStartTime";
        public const string ExitTimeCannotBeLessThanRestRangeEndTime = "ExitTimeCannotBeLessThanRestRangeEndTime";
        public const string FromDateCannotBeGreaterThanToDate = "FromDateCannotBeGreaterThanToDate";
        public const string GenerateTitle = "GenerateTitle";
        public const string InfractionSliceConflictWithOtherInfractionSlicesInThisInfractionForm = "InfractionSliceConflictWithOtherInfractionSlicesInThisInfractionForm";
        public const string LockTitle = "LockTitle";
        public const string NonAttendanceSliceConflictWithOtherNonAttendanceSlicesInThisNonAttendanceForm = "NonAttendanceSliceConflictWithOtherNonAttendanceSlicesInThisNonAttendanceForm";
        public const string NormalShiftConflictWithOtherNormalShiftsInThisWorkshop = "NormalShiftConflictWithOtherNormalShiftsInThisWorkshop";
        public const string OnlyCalculatedAttendanceRecordCanBeLocked = "OnlyCalculatedAttendanceRecordCanBeLocked";
        public const string YouCantAcceptPenaltiesBecauseTheMonthStatusIsNotCalculated = "YouCantAcceptPenaltiesBecauseTheMonthStatusIsNotCalculated";
        public const string ThereIsNoPenalty = "ThereIsNoPenalty";
        public const string AccordingToTheInfractionForm = "AccordingToTheInfractionForm";
        public const string FromAttendanceRecordThatIsName = "FromAttendanceRecordThatIsName";
        public const string WithNonAttendanceValue = "WithNonAttendanceValue";
        public const string And = "And";
        public const string Minutes = "Minutes";
        public const string ConnectedFailed = "ConnectedFailed";
        public const string ConnectedSuccess = "ConnectedSuccess";
        public const string TheDeviceIsNotConnected = "TheDeviceIsNotConnected";
        public const string Recurrences = "Recurrences";
        public const string OvertimeSliceConflictWithOtherOvertimeSlicesInThisOvertimeForm = "OvertimeSliceConflictWithOtherOvertimeSlicesInThisOvertimeForm";
        public const string ParticularOvertimeShiftConflictWithOtherParticularOvertimeShiftsInThisWorkshop = "ParticularOvertimeShiftConflictWithOtherParticularOvertimeShiftsInThisWorkshop";
        public const string ParticularOvertimeShiftEndTimeCannotBeLessThanStartTime = "ParticularOvertimeShiftEndTimeCannotBeLessThanStartTime";
        public const string ParticularOvertimeShiftMustBeInAnyNormalShiftRanges = "ParticularOvertimeShiftMustBeInAnyNormalShiftRanges";
        public const string PerformBioMetricInteractionButtonTitle = "PerformBioMetricInteractionButtonTitle";
        public const string RestRangeEndTimeCannotBeLessThanRestRangeStartTime = "RestRangeEndTimeCannotBeLessThanRestRangeStartTime";
        public const string RestRangeStartTimeCannotBeLessThanEntryTime = "RestRangeStartTimeCannotBeLessThanEntryTime";
        public const string SelectBioMetricDeviceBeforeExecuteMessage = "SelectBioMetricDeviceBeforeExecuteMessage";
        public const string SetValuesToAllFieldsBeforeAutoGeneratingRecords = "SetValuesToAllFieldsBeforeAutoGeneratingRecords";
        public const string ShiftRangeEndTimeCannotBeLessThanExitTime = "ShiftRangeEndTimeCannotBeLessThanExitTime";
        public const string SomeEmployeesDoNotHaveWorkshopRecurrence = "SomeEmployeesDoNotHaveWorkshopRecurrence";
        public const string SyncDevicesButton = "SyncDevicesButton";
        public const string TemporaryWorkshopConflictWithOtherTemporaryWorkshopsInThisWorkshop = "TemporaryWorkshopConflictWithOtherTemporaryWorkshopsInThisWorkshop";
        public const string MsgSorryThisRecurrenceOrderIsAlreadyExist = "MsgSorryThisRecurrenceOrderIsAlreadyExist";
        public const string GeneratedFromAttendanceSystem = "GeneratedFromAttendanceSystem";
        public const string IgnorePeriodMinutes = "IgnorePeriodMinutes";
        public const string IgnorePeriodMinutesMustBeBetween1And60 = "IgnorePeriodMinutesMustBeBetween1And60";
        public const string EmployeeNameIsNotValidInTheRowWhichNumberIs = "EmployeeNameIsNotValidInTheRowWhichNumberIs";
        public const string LogDateIsNotValidInTheRowWhichNumberIs = "LogDateIsNotValidInTheRowWhichNumberIs";
        public const string LogTimeIsNotValidInTheRowWhichNumberIs = "LogTimeIsNotValidInTheRowWhichNumberIs";
        public const string LogTypeIsNotValidInTheRowWhichNumberIs = "LogTypeIsNotValidInTheRowWhichNumberIs";
        public const string NoteIsNotValidInTheRowWhichNumberIs = "NoteIsNotValidInTheRowWhichNumberIs";
        public const string Done = "Done";
        public const string ImportEntranceExitRecordsFromExcel = "ImportEntranceExitRecordsFromExcel";

        
        public const string ParticularOvertimeShiftMustBelongToTheRangeFromTheMinimumEntryTimeToEntryTimeOrTheRangeFromExitTimeToTheMaximumExitTime = "ParticularOvertimeShiftMustBelongToTheRangeFromTheMinimumEntryTimeToEntryTimeOrTheRangeFromExitTimeToTheMaximumExitTime";
        
        public static string GetResource(string key)
        {
            var result = ServiceFactory.LocalizationService.GetResource(ResourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }
    }
}