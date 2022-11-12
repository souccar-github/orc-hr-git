CREATE PROCEDURE UpdateDailyRecordsFromAttendance(
@attendanceRecordId int
)
AS
Begin
SET NOCOUNT ON
select dailyRecords.Date as [Date], dailyRecords.Employee_id as employeeId, 
EmpCard.Id as EmpCardId, dailyRecords.Id as dailyRecordId, details.Id as detailId,
details.AttendanceWithoutAdjustment_id as attendanceRecordId into #recordsTmp
from DailyEnternaceExitRecord dailyRecords with (nolock) 
inner join EmployeeCard EmpCard with  (nolock)
on dailyRecords.Employee_id = EmpCard.Employee_id
inner join AttendanceWithoutAdjustment attendance with  (nolock)
on EmpCard.Id = attendance.EmployeeAttendanceCard_id
inner join AttendanceWithoutAdjustmentDetail details with  (nolock)
on attendance.Id = details.AttendanceWithoutAdjustment_id
and details.Date  = dailyRecords.Date
where dailyRecords.IsVertualDeleted = 0 and
EmpCard.IsVertualDeleted = 0 and
attendance.IsVertualDeleted = 0 and
details.IsVertualDeleted = 0
and attendance.AttendanceRecord_id = @attendanceRecordId

select detail.Date as [Date], attendance.EmployeeAttendanceCard_id as empCardId, 
detail.Id as dailyRecordId, detail.Id as detailId,
detail.AttendanceWithoutAdjustment_id as attendanceRecordId,
detail.HasMission as HasMission,
detail.HasVacation as HasVacation,
Round(detail.NonAttendanceHoursValue,2) as AbsentHoursValue,
Round(detail.LatenessHoursValue, 2) as LateHoursValue,
Round(detail.MissionValue, 2) as MissionValue,
 Round(detail.VacationValue, 2) as VacationValue,
 Round(detail.HolidayOvertimeValue, 2) as HolidayOvertimeHoursValue,
Round(detail.ActualWorkValue, 2) as WorkHoursValue,
 Round(detail.RequiredWorkHoursValue, 2) as RequiredWorkHours,
 1 as IsCalculated,
         Round(
		 detail.OvertimeOrderValue + 
		 detail.NormalOvertimeValue +
		 detail.ParticularOvertimeValue + 
		 detail.ExpectedOvertimeValue +
		 detail.HolidayOvertimeValue, 2) as OvertimeHoursValue
,
CASE
    WHEN detail.LatenessHoursValue > 0 then 3
    ELSE 0 
END as LateType,
CASE
    WHEN (detail.ActualWorkValue <= 0 or detail.RequiredWorkHoursValue - detail.ActualWorkValue > 0)
	and detail.IsOffDay = 0 and detail.IsHoliday = 1  then 3
    ELSE 0
END as AbsenseType,
 detail.DayOfWeek as [day],
CASE
    WHEN detail.IsHoliday = 1 or detail.IsOffDay = 1 THEN 4  
    WHEN detail.HasVacation = 1 and detail.VacationValue >= detail.RequiredWorkHoursValue THEN 2
    WHEN detail.HasMission = 1 and detail.MissionValue >= detail.RequiredWorkHoursValue THEN 3
    WHEN detail.LatenessHoursValue > 0 then 1
    WHEN detail.ActualWorkValue <= 0 and detail.IsOffDay = 0 
	and detail.IsHoliday = 0 and detail.HasVacation = 0 
	and detail.HasMission = 0  then 0
	ELSE 1
END as [status],
detail.RequiredWorkHoursFormatedValue as [RequiredWorkHoursFormatedValue],
detail.ActualWorkFormatedValue as [WorkHoursFormatedValue],
detail.NonAttendanceHoursFormatedValue as [AbsentHoursFormatedValue],
detail.LatenessHoursFormatedValue as [LatenessHoursFormatedValue],
detail.OvertimeOrderFormatedValue as [OvertimeHoursFormatedValue],
detail.HolidayOvertimeFormatedValue as [HolidayOvertimeFormatedValue],
detail.MissionFormatedValue as [MissionFormatedValue],
detail.VacationFormatedValue as [VacationFormatedValue]
 into #detailsTmp
from AttendanceWithoutAdjustment attendance with  (nolock)
inner join AttendanceWithoutAdjustmentDetail detail with  (nolock)
on attendance.Id = detail.AttendanceWithoutAdjustment_id
where
attendance.IsVertualDeleted = 0 and
detail.IsVertualDeleted = 0
and attendance.AttendanceRecord_id = @attendanceRecordId
and detail.Date not in (select Date from #recordsTmp where 
 #recordsTmp.EmpCardId = attendance.EmployeeAttendanceCard_id)


---
update DailyEnternaceExitRecord set 
DailyEnternaceExitRecord.HasMission = detail.HasMission,
DailyEnternaceExitRecord.HasVacation = detail.HasVacation,
DailyEnternaceExitRecord.AbsentHoursValue = Round(detail.NonAttendanceHoursValue,2),
DailyEnternaceExitRecord.LateHoursValue = Round(detail.LatenessHoursValue, 2),
DailyEnternaceExitRecord.MissionValue = Round(detail.MissionValue, 2),
DailyEnternaceExitRecord.VacationValue = Round(detail.VacationValue, 2),
DailyEnternaceExitRecord.HolidayOvertimeHoursValue = Round(detail.HolidayOvertimeValue, 2),
DailyEnternaceExitRecord.WorkHoursValue = Round(detail.ActualWorkValue, 2),
DailyEnternaceExitRecord.RequiredWorkHours = Round(detail.RequiredWorkHoursValue, 2),
DailyEnternaceExitRecord.IsCalculated = 1,
DailyEnternaceExitRecord.OvertimeHoursValue = 
         Round(
		 detail.OvertimeOrderValue + 
		 detail.NormalOvertimeValue +
		 detail.ParticularOvertimeValue + 
		 detail.ExpectedOvertimeValue +
		 detail.HolidayOvertimeValue, 2)
,
DailyEnternaceExitRecord.LateType = 
CASE
    WHEN detail.LatenessHoursValue > 0 then 3
    ELSE 0
END ,
DailyEnternaceExitRecord.AbsenseType = 
CASE
    WHEN (detail.ActualWorkValue <= 0 or detail.RequiredWorkHoursValue - detail.ActualWorkValue > 0)
	and detail.IsOffDay = 0 and detail.IsHoliday = 1  then 3
    ELSE 0
END,
DailyEnternaceExitRecord.Day = detail.DayOfWeek,
DailyEnternaceExitRecord.Status = 
CASE
    WHEN detail.IsHoliday = 1 or detail.IsOffDay = 1 THEN 4  
    WHEN detail.HasVacation = 1 and detail.VacationValue >= detail.RequiredWorkHoursValue THEN 2
    WHEN detail.HasMission = 1 and detail.MissionValue >= detail.RequiredWorkHoursValue THEN 3
    WHEN detail.LatenessHoursValue > 0 then 1
    WHEN detail.ActualWorkValue <= 0 and detail.IsOffDay = 0 
	and detail.IsHoliday = 0 and detail.HasVacation = 0 
	and detail.HasMission = 0  then 0
    WHEN detail.ActualWorkValue > 0 then 1
	else 1
END,
DailyEnternaceExitRecord.RequiredWorkHoursFormatedValue =  detail.RequiredWorkHoursFormatedValue,
DailyEnternaceExitRecord.WorkHoursFormatedValue =  detail.ActualWorkFormatedValue,
DailyEnternaceExitRecord.AbsentHoursFormatedValue =  detail.NonAttendanceHoursFormatedValue,
DailyEnternaceExitRecord.LatenessHoursFormatedValue =  detail.LatenessHoursFormatedValue,
DailyEnternaceExitRecord.OvertimeHoursFormatedValue =  detail.OvertimeOrderFormatedValue,
DailyEnternaceExitRecord.HolidayOvertimeFormatedValue =  detail.HolidayOvertimeFormatedValue,
DailyEnternaceExitRecord.MissionFormatedValue =  detail.MissionFormatedValue,
DailyEnternaceExitRecord.VacationFormatedValue =  detail.VacationFormatedValue
FROM
    DailyEnternaceExitRecord DailyEntrExitRecords
INNER JOIN
    #recordsTmp tmp on DailyEntrExitRecords.Id = tmp.dailyRecordId
	
INNER JOIN
    AttendanceWithoutAdjustmentDetail detail on tmp.detailId = detail.Id


Insert into DailyEnternaceExitRecord
(AbsenseType, AbsentHoursValue, Date, Day, Employee_id, HasMission, HasVacation, HolidayOvertimeHoursValue
,InsertSource, IsCalculated, IsClosed, LateHoursValue , LateType, MissionValue
, Node_id, Note, OvertimeHoursValue, RequiredWorkHours, Status,WorkHoursValue,
 VacationValue, IsVertualDeleted, RequiredWorkHoursFormatedValue, WorkHoursFormatedValue, AbsentHoursFormatedValue,
 LatenessHoursFormatedValue, OvertimeHoursFormatedValue, HolidayOvertimeFormatedValue, 
 MissionFormatedValue, VacationFormatedValue)

select details.AbsenseType, details.AbsentHoursValue, details.Date, details.day, empCard.Employee_id,
details.HasMission, details.HasVacation, details.HolidayOvertimeHoursValue,
0, 1, 0, details.LateHoursValue, details.LateType, details.MissionValue,
(select dbo.GetNodeOfEmployee(empCard.Employee_id)), '', details.OvertimeHoursValue,
details.RequiredWorkHours, details.status, details.WorkHoursValue,
 details.VacationValue, 0, RequiredWorkHoursFormatedValue, WorkHoursFormatedValue, AbsentHoursFormatedValue,
 LatenessHoursFormatedValue, OvertimeHoursFormatedValue, HolidayOvertimeFormatedValue, 
 MissionFormatedValue, VacationFormatedValue
from #detailsTmp details inner join 
EmployeeCard empCard on details.empCardId = empCard.Id 
 

 drop table #recordsTmp

 drop table #detailsTmp;
 SET NOCOUNT OFF
 End
GO