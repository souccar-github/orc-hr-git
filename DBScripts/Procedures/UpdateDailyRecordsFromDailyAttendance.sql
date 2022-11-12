CREATE PROCEDURE UpdateDailyRecordsFromDailyAttendance(
@attendanceRecordId int
)
AS
Begin
SET NOCOUNT ON
select dailyRecords.Date as [Date], dailyRecords.Employee_id as employeeId, 
EmpCard.Id as EmpCardId, dailyRecords.Id as dailyRecordId, details.Id as detailId,
details.AttendanceDailyAdjustment_id as attendanceRecordId into #recordsTmp
from DailyEnternaceExitRecord dailyRecords with (nolock) 
inner join EmployeeCard EmpCard with  (nolock)
on dailyRecords.Employee_id = EmpCard.Employee_id
inner join AttendanceDailyAdjustment attendance with  (nolock)
on EmpCard.Id = attendance.EmployeeAttendanceCard_id
inner join AttendanceDailyAdjustmentDetail details with  (nolock)
on attendance.Id = details.AttendanceDailyAdjustment_id
and details.Date  = dailyRecords.Date
where dailyRecords.IsVertualDeleted = 0 and
EmpCard.IsVertualDeleted = 0 and
attendance.IsVertualDeleted = 0 and
details.IsVertualDeleted = 0
and attendance.AttendanceRecord_id = @attendanceRecordId

select detail.Date as [Date], attendance.EmployeeAttendanceCard_id as empCardId, 
detail.Id as dailyRecordId, detail.Id as detailId,
detail.AttendanceDailyAdjustment_id as attendanceRecordId,
detail.HasMission as HasMission,
detail.HasVacation as HasVacation,
 CASE
  WHEN(detail.ActualWorkHoursValue < (detail.WorkHoursValue - detail.VacationValue - detail.MissionValue))
  then  Round((detail.WorkHoursValue - detail.VacationValue - detail.MissionValue) - detail.ActualWorkHoursValue , 2)
  ELSE 0 
  END as AbsentHoursValue
,
0 as LateHoursValue,
Round(detail.MissionValue, 2) as MissionValue,
 Round(detail.VacationValue, 2) as VacationValue,
 Round(detail.HolidayOvertimeValue, 2) as HolidayOvertimeHoursValue,
Round(detail.ActualWorkHoursValue, 2) as WorkHoursValue,
 Round(detail.WorkHoursValue - detail.VacationValue - detail.MissionValue, 2) as RequiredWorkHours,
 1 as IsCalculated,
 CASE
  WHEN(detail.ActualWorkHoursValue > (detail.WorkHoursValue - detail.VacationValue - detail.MissionValue))
  then  Round(detail.ActualWorkHoursValue - (detail.WorkHoursValue - detail.VacationValue - detail.MissionValue), 2)
  ELSE 0 
  END as OvertimeHoursValue
,
0 as LateType,
CASE
    WHEN (detail.ActualWorkHoursValue <= 0 or 
	 (detail.WorkHoursValue - detail.VacationValue - detail.MissionValue) - detail.ActualWorkHoursValue > 0)
	and detail.IsOffDay = 0 and detail.IsHoliday = 0  then 3
    ELSE 0
END as AbsenseType,
 detail.DayOfWeek as [day],
CASE
    WHEN detail.IsHoliday = 1 or detail.IsOffDay = 1 THEN 4  
    WHEN detail.HasVacation = 1 and detail.VacationValue >= 
	     (detail.WorkHoursValue - detail.VacationValue - detail.MissionValue) THEN 2
    WHEN detail.HasMission = 1 and detail.MissionValue >= 
	     (detail.WorkHoursValue - detail.VacationValue - detail.MissionValue) THEN 3
    WHEN detail.ActualWorkHoursValue <= 0 and detail.IsOffDay = 0 
	and detail.IsHoliday = 0 and detail.HasVacation = 0 
	and detail.HasMission = 0  then 0
	ELSE 1
END as [status],
(SELECT [dbo].[GetFormatValueAsHourMinute](
Round(detail.WorkHoursValue - detail.VacationValue - detail.MissionValue,2))) as [RequiredWorkHoursFormatedValue],
detail.ActualWorkHoursValueFormatedValue as [WorkHoursFormatedValue],
 CASE
  WHEN(detail.ActualWorkHoursValue < (detail.WorkHoursValue - detail.VacationValue - detail.MissionValue))
  then  (SELECT [dbo].[GetFormatValueAsHourMinute](
  Round((detail.WorkHoursValue - detail.VacationValue - detail.MissionValue) - detail.ActualWorkHoursValue , 2)))
  ELSE '00:00'
  END as [AbsentHoursFormatedValue]
,
--detail.LatenessHoursFormatedValue as [LatenessHoursFormatedValue],
 CASE
  WHEN(detail.ActualWorkHoursValue > (detail.WorkHoursValue - detail.VacationValue - detail.MissionValue))
  then   (SELECT  [dbo].[GetFormatValueAsHourMinute](
  Round(detail.ActualWorkHoursValue - (detail.WorkHoursValue - detail.VacationValue - detail.MissionValue), 2)))
  ELSE '00:00'
  END  as [OvertimeHoursFormatedValue],
detail.HolidayOvertimeValueFormatedValue as [HolidayOvertimeFormatedValue],
detail.MissionValueFormatedValue as [MissionFormatedValue],
detail.VacationValueFormatedValue as [VacationFormatedValue]
 into #detailsTmp
from AttendanceDailyAdjustment attendance with  (nolock)
inner join AttendanceDailyAdjustmentDetail detail with  (nolock)
on attendance.Id = detail.AttendanceDailyAdjustment_id
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
DailyEnternaceExitRecord.AbsentHoursValue =
CASE
  WHEN(detail.ActualWorkHoursValue < (detail.WorkHoursValue - detail.VacationValue - detail.MissionValue))
  then  Round((detail.WorkHoursValue - detail.VacationValue - detail.MissionValue) - detail.ActualWorkHoursValue , 2)
  ELSE 0 
  END,
DailyEnternaceExitRecord.LateHoursValue = 0,
DailyEnternaceExitRecord.MissionValue = Round(detail.MissionValue, 2),
DailyEnternaceExitRecord.VacationValue = Round(detail.VacationValue, 2),
DailyEnternaceExitRecord.HolidayOvertimeHoursValue = Round(detail.HolidayOvertimeValue, 2),
DailyEnternaceExitRecord.WorkHoursValue = Round(detail.ActualWorkHoursValue, 2),
DailyEnternaceExitRecord.RequiredWorkHours = Round(
                 detail.WorkHoursValue - detail.VacationValue - detail.MissionValue, 2),
DailyEnternaceExitRecord.IsCalculated = 1,
DailyEnternaceExitRecord.OvertimeHoursValue = 
 CASE
  WHEN(detail.ActualWorkHoursValue > (detail.WorkHoursValue - detail.VacationValue - detail.MissionValue))
  then  Round(detail.ActualWorkHoursValue - (detail.WorkHoursValue - detail.VacationValue - detail.MissionValue), 2)
  ELSE 0 
  END
,
DailyEnternaceExitRecord.LateType = 0,
DailyEnternaceExitRecord.AbsenseType = 
CASE
    WHEN (detail.ActualWorkHoursValue <= 0 or 
	(detail.WorkHoursValue - detail.VacationValue - detail.MissionValue) - detail.ActualWorkHoursValue > 0)
	and detail.IsOffDay = 0 and detail.IsHoliday = 1  then 3
    ELSE 0
END,
DailyEnternaceExitRecord.Day = detail.DayOfWeek,
DailyEnternaceExitRecord.Status = 
CASE
    WHEN detail.IsHoliday = 1 or detail.IsOffDay = 1 THEN 4  
    WHEN detail.HasVacation = 1 and detail.VacationValue >= 
	   (detail.WorkHoursValue - detail.VacationValue - detail.MissionValue) THEN 2
    WHEN detail.HasMission = 1 and detail.MissionValue >= 
	   (detail.WorkHoursValue - detail.VacationValue - detail.MissionValue) THEN 3
    WHEN detail.ActualWorkHoursValue <= 0 and detail.IsOffDay = 0 
	and detail.IsHoliday = 0 and detail.HasVacation = 0 
	and detail.HasMission = 0  then 0
    WHEN detail.ActualWorkHoursValue > 0 then 1
	else 1
END,
DailyEnternaceExitRecord.RequiredWorkHoursFormatedValue =  
(SELECT [dbo].[GetFormatValueAsHourMinute](
Round(detail.WorkHoursValue - detail.VacationValue - detail.MissionValue,2))),
DailyEnternaceExitRecord.WorkHoursFormatedValue =  detail.ActualWorkHoursValueFormatedValue,
DailyEnternaceExitRecord.AbsentHoursFormatedValue = 
  CASE
  WHEN(detail.ActualWorkHoursValue < (detail.WorkHoursValue - detail.VacationValue - detail.MissionValue))
  then   (SELECT  [dbo].[GetFormatValueAsHourMinute](
  Round((detail.WorkHoursValue - detail.VacationValue - detail.MissionValue) - detail.ActualWorkHoursValue, 2)))
  ELSE '00:00'
  END,
--DailyEnternaceExitRecord.LatenessHoursFormatedValue =  detail.LatenessHoursFormatedValue,
DailyEnternaceExitRecord.OvertimeHoursFormatedValue =  CASE
  WHEN(detail.ActualWorkHoursValue > (detail.WorkHoursValue - detail.VacationValue - detail.MissionValue))
  then   (SELECT  [dbo].[GetFormatValueAsHourMinute](
  Round(detail.ActualWorkHoursValue - (detail.WorkHoursValue - detail.VacationValue - detail.MissionValue), 2)))
  ELSE '00:00'
  END,
DailyEnternaceExitRecord.HolidayOvertimeFormatedValue =  detail.HolidayOvertimeValueFormatedValue,
DailyEnternaceExitRecord.MissionFormatedValue =  detail.MissionValueFormatedValue,
DailyEnternaceExitRecord.VacationFormatedValue =  detail.VacationValueFormatedValue
FROM
    DailyEnternaceExitRecord DailyEntrExitRecords
INNER JOIN
    #recordsTmp tmp on DailyEntrExitRecords.Id = tmp.dailyRecordId
	
INNER JOIN
    AttendanceDailyAdjustmentDetail detail on tmp.detailId = detail.Id


Insert into DailyEnternaceExitRecord
(AbsenseType, AbsentHoursValue, Date, Day, Employee_id, HasMission, HasVacation, HolidayOvertimeHoursValue
,InsertSource, IsCalculated, IsClosed, LateHoursValue , LateType, MissionValue
, Node_id, Note, OvertimeHoursValue, RequiredWorkHours, Status,WorkHoursValue,
 VacationValue, IsVertualDeleted
 , RequiredWorkHoursFormatedValue, WorkHoursFormatedValue, AbsentHoursFormatedValue,
  OvertimeHoursFormatedValue, HolidayOvertimeFormatedValue, 
 MissionFormatedValue, VacationFormatedValue
 )

select details.AbsenseType, details.AbsentHoursValue, details.Date, details.day, empCard.Employee_id,
details.HasMission, details.HasVacation, details.HolidayOvertimeHoursValue,
0, 1, 0, details.LateHoursValue, details.LateType, details.MissionValue,
(select dbo.GetNodeOfEmployee(empCard.Employee_id)), '', details.OvertimeHoursValue,
details.RequiredWorkHours, details.status, details.WorkHoursValue,
 details.VacationValue, 0
 , RequiredWorkHoursFormatedValue, WorkHoursFormatedValue, AbsentHoursFormatedValue,
  OvertimeHoursFormatedValue, HolidayOvertimeFormatedValue, 
 MissionFormatedValue, VacationFormatedValue
from #detailsTmp details inner join 
EmployeeCard empCard on details.empCardId = empCard.Id 
 

 drop table #recordsTmp

 drop table #detailsTmp;
 SET NOCOUNT OFF
 End
GO