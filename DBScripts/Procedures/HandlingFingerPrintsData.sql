
CREATE PROC HandlingFingerPrintsData ( 
    @insertSource bit
)
AS
BEGIN 
  SET NOCOUNT OFF;
  --GET general attendance ATTENDANCE
  DECLARE @generalAttendanceId INT = 
    (SELECT TOP(1) AttendanceForm_id FROM GeneralSettings   
     WITH (NOLOCK) WHERE IsVertualDeleted = 0) 
  
  -- Get 4 transfered finger prints to every employee with transfered flag 
  -- to handle the first new fingerprints in the same day
  ;WITH TransferedFingerPrints AS
   (
   SELECT *,
         ROW_NUMBER() OVER (PARTITION BY Employee_id ORDER BY LogDateTime DESC) AS rn
   FROM FingerprintTransferredData
   	WHERE  IsTransfered = 1 and 
		   isOld = 0
   )
   SELECT LogDateTime, Employee_id, LogType, IsTransfered
   INTO #FingerprintsData
   FROM TransferedFingerPrints
   WHERE rn between 1 and 4

   -- Get the new finger print
   Insert into #FingerprintsData
   SELECT LogDateTime, Employee_id, LogType, IsTransfered
   FROM FingerprintTransferredData
   	WHERE  IsTransfered = 0 and 
		   isOld = 0 and IsLogTypeIgnored = 1

  -- Select the distinct data based on date
  SELECT DISTINCT  
    ftd.Employee_id AS EmployeeId,   
    DATEADD(dd, 0, DATEDIFF(dd, 0, ftd.LogDateTime)) AS TodayDate
  INTO #DistinctFingerprintsData   
  FROM #FingerprintsData ftd 

  -- Get only the days with employee from  temp finger prints table
  -- Set the form id form employee card 
  SELECT   
    ftd.EmployeeId AS EmployeeId,   
    empCard.Id AS EmployeeCardId,   
    ftd.TodayDate AS TodayDate, 
    empCard.AttendanceForm_id AS FormId,
    0 AS recurrenceIndex,
    NULL AS workshopId
  INTO #EMPLOYEE_RECURRENCES   
  FROM #DistinctFingerprintsData ftd INNER JOIN   
       EmployeeCard empCard WITH (NOLOCK) on ftd.EmployeeId = empCard.Employee_id 

  --Set the form id from grade for all employees' form is null
  --if we have employees without assigned a form to their cards
  UPDATE #EMPLOYEE_RECURRENCES SET FormId =   
    (select grade.AttendanceForm_id FROM     
            AssigningEmployeeToPosition assign WITH (NOLOCK) INNER JOIN     
            Position pos  WITH (NOLOCK) on assign.Position_Id = pos.Id INNER JOIN      
            JobDescription jobDesc WITH (NOLOCK) on pos.JobDescription_id = jobDesc.Id INNER JOIN      
            JobTitle title WITH (NOLOCK)on jobDesc.JobTitle_id = title.Id INNER JOIN    
            Grade grade WITH (NOLOCK) on title.Grade_id = grade.Id   
      WHERE assign.Employee_id = EmployeeCardId AND assign.IsPrimary = 1)  
   WHERE FormId IS NULL 

  --Set the general form id for all employees' form is null
  --if we have employees without assigned the form to their cards or grades
  UPDATE #EMPLOYEE_RECURRENCES SET FormId = @generalAttendanceId  
  WHERE FormId IS NULL   

  --get the recurrences for all days depends on form 
  UPDATE #EMPLOYEE_RECURRENCES SET recurrenceIndex =   
     (SELECT dbo.GetRecurrenceIndexByDate(EmployeeCardId, FormId, TodayDate) + 1) 

   

  -- Get the temporary workshops from EmployeeTemporaryWorkshop
  UPDATE #EMPLOYEE_RECURRENCES SET workshopId =   
        (SELECT EmpTemWorkshop.Workshop_id FROM   
         EmployeeTemporaryWorkshop EmpTemWorkshop WITH (NOLOCK)  
         WHERE EmpTemWorkshop.EmployeeCard_id = EmployeeCardId and  
         EmpTemWorkshop.FromDate <= TodayDate and   
         EmpTemWorkshop.ToDate >= TodayDate)  
  
  
  -- Get the temporary workshops from TemporaryWorkshop
  UPDATE #EMPLOYEE_RECURRENCES SET workshopId =   
         (SELECT AlternativeWorkshop_id FROM 
		  TemporaryWorkshop tmWorkShop WITH (NOLOCK)   
          WHERE tmWorkShop.Workshop_id = workshopId and   
          tmWorkShop.FromDate <= TodayDate and   
          tmWorkShop.ToDate >= TodayDate)   
   WHERE workshopId IS NULL  

   -- if we dont have any temporary workshop 
   -- Get the workshops with taking care about the recurrence number
   UPDATE #EMPLOYEE_RECURRENCES SET workshopId =   
          (SELECT Workshop_id FROM WorkshopRecurrence 
		   WHERE AttendanceForm_id = FormId AND
		    RecurrenceOrder = recurrenceIndex)  
   WHERE workshopId IS NULL  
  

   --Get the shift start and end times based on employee's workshop
   --handle also the night shift
   SELECT 
       tmp.EmployeeId as EmployeeId,
       tmp.EmployeeCardId as EmployeeCardId, 
       tmp.TodayDate as DateOfDay,
	   tmp.FormId AS FormId,
	   tmp.workshopId AS WorkshopId,

       DATEADD(DAY, DATEDIFF(day, 0, tmp.TodayDate),
       CONVERT(char(8), shifts.ShiftRangeStartTime, 108))
       as ShiftRangeStartTime,

       CASE WHEN
       DATEADD(DAY, DATEDIFF(DAY, 0, tmp.TodayDate),
       CONVERT(char(8), shifts.ShiftRangeStartTime, 108))> 
       DATEADD(DAY, DATEDIFF(DAY, 0, tmp.TodayDate),
       CONVERT(char(8), shifts.ShiftRangeEndTime, 108))
       THEN DATEADD(DAY, DATEDIFF(DAY, -1, tmp.TodayDate),
       CONVERT(char(8), shifts.ShiftRangeEndTime, 108))
       ELSE
       DATEADD(DAY, DATEDIFF(DAY, 0, tmp.TodayDate),
       CONVERT(char(8), shifts.ShiftRangeEndTime, 108))
       END
	   AS  ShiftRangeEndTime


    INTO #shifts
   FROM 
      Workshop shop WITH (NOLOCK) INNER JOIN 
      #EMPLOYEE_RECURRENCES tmp on shop.Id = tmp.workshopId INNER JOIN 
      NormalShift shifts WITH (NOLOCK) on shop.Id = shifts.Workshop_id
   WHERE shifts.IsVertualDeleted = 0 AND
         shop.IsVertualDeleted = 0

   --declare a finger prints table with shifts range to determine log type
   Declare @fingerPrintsTmp TABLE(
               EmployeeId int, 
               EmployeeCardId int, 
               DateOfDay datetime,
			   FormId int,
			   WorkshopId int,
			   LogDateTime datetime,
			   ShiftRangeStartTime datetime,
			   ShiftRangeEndTime datetime,
			   DATERANGE nvarchar(200),
			   LogType bit,
			   IsTransfered bit,
			   OldLogType bit)
   INSERT INTO @fingerPrintsTmp
   SELECT  
       shiftsTmpTable.EmployeeId, 
       shiftsTmpTable.EmployeeCardId, 
       shiftsTmpTable.DateOfDay,
	   shiftsTmpTable.FormId,
	   shiftsTmpTable.WorkshopId,
	   ftd.LogDateTime, 

       CASE 
	   WHEN ftd.LogDateTime > shiftsTmpTable.ShiftRangeStartTime and
            ftd.LogDateTime < shiftsTmpTable.ShiftRangeEndTime
       THEN 
	        shiftsTmpTable.ShiftRangeStartTime
       ELSE 
            DATEADD(day, DATEDIFF(day, 1, shiftsTmpTable.ShiftRangeStartTime),
                    convert(char(8), shiftsTmpTable.ShiftRangeStartTime, 108))
       END 
	   AS ShiftRangeStartTime, 

       CASE 
	   WHEN
            ftd.LogDateTime > shiftsTmpTable.ShiftRangeStartTime and
            ftd.LogDateTime < shiftsTmpTable.ShiftRangeEndTime
       then 
	        shiftsTmpTable.ShiftRangeEndTime 
       else 
            DATEADD(day, DATEDIFF(day, 1, shiftsTmpTable.ShiftRangeEndTime),
                    convert(char(8), shiftsTmpTable.ShiftRangeEndTime, 108))
        end 
		AS ShiftRangeEndTime,
		'' AS DATERANGE,
		0 LogType,
		IsTransfered as IsTransfered,
		LogType as OldLogType
     FROM #FingerprintsData ftd INNER JOIN 
          EmployeeCard empCard WITH (NOLOCK) ON ftd.Employee_id = empCard.Employee_id INNER JOIN  
          #shifts shiftsTmpTable ON 
   	       empCard.Id = shiftsTmpTable.EmployeeCardId AND 
           DATEADD(dd, 0, DATEDIFF(dd, 0, ftd.LogDateTime)) = shiftsTmpTable.DateOfDay
    WHERE empCard.IsVertualDeleted = 0  
  --for night shift we have two dates so I make the distinct data as
  --a mix from start range date with end one
  UPDATE @fingerPrintsTmp 
  SET DATERANGE = 
  LEFT(CONVERT(VARCHAR, ShiftRangeStartTime, 120), 10) + 'TO' +
  LEFT(CONVERT(VARCHAR, ShiftRangeEndTime, 120), 10)
  
  --get the row number depends on employee and
  --the mix from start range date with end one
  SELECT *, ROW_NUMBER() OVER (
    PARTITION BY EmployeeCardId, DATERANGE
	ORDER BY LogDateTime
    ) AS RowNumber
  INTO #fingerPrintsTmpWithRowNumber
  FROM @fingerPrintsTmp

  --set the log type 
  -- if RowNumber % 2 is an odd it will be entrance else exit
  -- 1 for entrance and 0 for exit
  -- but in the logtype enum we should replace them 
  UPDATE #fingerPrintsTmpWithRowNumber
  SET LogType = RowNumber % 2

  -- start updating the tables
  
  -- insert into entrance exit records
  INSERT INTO EntranceExitRecord
    (IsVertualDeleted, LogDateTime, LogTime,
	LogDate, LogType, ErrorType, InsertSource, Employee_id, IsChecked)
    SELECT 0 AS IsVertualDeleted,
         fingerPrintsData.LogDateTime AS LogDateTime,
		 DATEADD(DAY, DATEDIFF(DAY, 0, '2000-01-01'),
         CONVERT(CHAR(8), fingerPrintsData.LogDateTime, 108)) AS LogTime,
         fingerPrintsData.DateOfDay AS LogDate, 
		 (CASE 
		      WHEN fingerPrintsData.LogType = 1 
			  THEN 0 
			  ELSE 1 END) AS LogType,-- in the enum the 0 is entrance
		 0 AS ErrorType, -- NONE
		 @insertSource AS InsertSource, 
         fingerPrintsData.EmployeeId AS Employee_id,
		 0 AS IsChecked
	     FROM #fingerPrintsTmpWithRowNumber fingerPrintsData 
	WHERE IsTransfered = 0 

  --set the date for night shift to be equal to entrance date 
  --cause the entrance date and exit will be in different dates
  UPDATE #fingerPrintsTmpWithRowNumber
  SET DateOfDay = (SELECT TOP(1) DateOfDay 
                   FROM #fingerPrintsTmpWithRowNumber dataoffingerprints
				   WHERE dataoffingerprints.DATERANGE = #fingerPrintsTmpWithRowNumber.DATERANGE AND
				         dataoffingerprints.EmployeeId = #fingerPrintsTmpWithRowNumber.EmployeeId
				   ORDER BY dataoffingerprints.LogDateTime ASC)

  --Insert new records for distinct finger prints
  -- partition by employee and date
  Insert into DailyEnternaceExitRecord
(AbsenseType, AbsentHoursValue, [Date], Day, Employee_id, HasMission, HasVacation, HolidayOvertimeHoursValue
,InsertSource, IsCalculated, IsClosed, LateHoursValue , LateType, MissionValue
, Node_id, Note, OvertimeHoursValue, RequiredWorkHours, Status,WorkHoursValue,
 VacationValue, IsVertualDeleted, RequiredWorkHoursFormatedValue, WorkHoursFormatedValue, AbsentHoursFormatedValue,
 LatenessHoursFormatedValue, OvertimeHoursFormatedValue, HolidayOvertimeFormatedValue, 
 MissionFormatedValue, VacationFormatedValue)

  SELECT 0, 0, details.TodayDate, 
       DATEPART(WEEKDAY,details.TodayDate), 
       details.EmployeeId, 0, 0, 0, 2, 0, 0, 0, 0, 0,
       (SELECT dbo.GetNodeOfEmployee(details.EmployeeId)), '', 0,
       0, 0, 0,0, 0, '', '', '', '', '', '','', ''
  FROM #DistinctFingerprintsData details
  WHERE details.TodayDate not in
         (SELECT [Date] FROM DailyEnternaceExitRecord dailytmp
          WHERE dailytmp.Employee_id = details.EmployeeId) 
  
  UPDATE DailyEnternaceExitRecord
  SET LoginDate = DATEADD(dd, 0, DATEDIFF(dd, 0, #fingerPrintsTmpWithRowNumber.LogDateTime)),
      LoginDateTime = #fingerPrintsTmpWithRowNumber.LogDateTime,
	  LoginTime = DATEADD(DAY, DATEDIFF(DAY, 0, '2000-01-01'),
           CONVERT(CHAR(8), #fingerPrintsTmpWithRowNumber.LogDateTime, 108))
  FROM DailyEnternaceExitRecord INNER JOIN
       #fingerPrintsTmpWithRowNumber ON 
	   DailyEnternaceExitRecord.Employee_id = #fingerPrintsTmpWithRowNumber.EmployeeId AND
	   #fingerPrintsTmpWithRowNumber.DateOfDay = DailyEnternaceExitRecord.[Date]
	   WHERE #fingerPrintsTmpWithRowNumber.RowNumber = 1 AND #fingerPrintsTmpWithRowNumber.LogType = 1

   UPDATE DailyEnternaceExitRecord
  SET LogoutDate = DATEADD(dd, 0, DATEDIFF(dd, 0, #fingerPrintsTmpWithRowNumber.LogDateTime)),
      LogoutDateTime = #fingerPrintsTmpWithRowNumber.LogDateTime,
	  LogoutTime = DATEADD(DAY, DATEDIFF(DAY, 0, '2000-01-01'),
           CONVERT(CHAR(8), #fingerPrintsTmpWithRowNumber.LogDateTime, 108))
  FROM DailyEnternaceExitRecord INNER JOIN
       #fingerPrintsTmpWithRowNumber ON 
	   DailyEnternaceExitRecord.Employee_id = #fingerPrintsTmpWithRowNumber.EmployeeId AND
	   #fingerPrintsTmpWithRowNumber.DateOfDay = DailyEnternaceExitRecord.[Date]
	   WHERE #fingerPrintsTmpWithRowNumber.RowNumber = 2 AND #fingerPrintsTmpWithRowNumber.LogType = 0

	     UPDATE DailyEnternaceExitRecord
  SET SecondLoginDate = DATEADD(dd, 0, DATEDIFF(dd, 0, #fingerPrintsTmpWithRowNumber.LogDateTime)),
      SecondLoginDateTime = #fingerPrintsTmpWithRowNumber.LogDateTime,
	  SecondLoginTime = DATEADD(DAY, DATEDIFF(DAY, 0, '2000-01-01'),
           CONVERT(CHAR(8), #fingerPrintsTmpWithRowNumber.LogDateTime, 108))
  FROM DailyEnternaceExitRecord INNER JOIN
       #fingerPrintsTmpWithRowNumber ON 
	   DailyEnternaceExitRecord.Employee_id = #fingerPrintsTmpWithRowNumber.EmployeeId AND
	   #fingerPrintsTmpWithRowNumber.DateOfDay = DailyEnternaceExitRecord.[Date]
	   WHERE #fingerPrintsTmpWithRowNumber.RowNumber = 3 AND #fingerPrintsTmpWithRowNumber.LogType = 1

	   	     UPDATE DailyEnternaceExitRecord
  SET SecondLogoutDate = DATEADD(dd, 0, DATEDIFF(dd, 0, #fingerPrintsTmpWithRowNumber.LogDateTime)),
      SecondLogoutDateTime = #fingerPrintsTmpWithRowNumber.LogDateTime,
	  SecondLogoutTime = DATEADD(DAY, DATEDIFF(DAY, 0, '2000-01-01'),
           CONVERT(CHAR(8), #fingerPrintsTmpWithRowNumber.LogDateTime, 108))
  FROM DailyEnternaceExitRecord INNER JOIN
       #fingerPrintsTmpWithRowNumber ON 
	   DailyEnternaceExitRecord.Employee_id = #fingerPrintsTmpWithRowNumber.EmployeeId AND
	   #fingerPrintsTmpWithRowNumber.DateOfDay = DailyEnternaceExitRecord.[Date]
	   WHERE #fingerPrintsTmpWithRowNumber.RowNumber = 4 AND #fingerPrintsTmpWithRowNumber.LogType = 0

	   	     UPDATE DailyEnternaceExitRecord
  SET ThirdLoginDate = DATEADD(dd, 0, DATEDIFF(dd, 0, #fingerPrintsTmpWithRowNumber.LogDateTime)),
      ThirdLoginDateTime = #fingerPrintsTmpWithRowNumber.LogDateTime,
	  ThirdLoginTime = DATEADD(DAY, DATEDIFF(DAY, 0, '2000-01-01'),
           CONVERT(CHAR(8), #fingerPrintsTmpWithRowNumber.LogDateTime, 108))
  FROM DailyEnternaceExitRecord INNER JOIN
       #fingerPrintsTmpWithRowNumber ON 
	   DailyEnternaceExitRecord.Employee_id = #fingerPrintsTmpWithRowNumber.EmployeeId AND
	   #fingerPrintsTmpWithRowNumber.DateOfDay = DailyEnternaceExitRecord.[Date]
	   WHERE #fingerPrintsTmpWithRowNumber.RowNumber = 5 AND #fingerPrintsTmpWithRowNumber.LogType = 1

	   	     UPDATE DailyEnternaceExitRecord
  SET ThirdLogoutDate = DATEADD(dd, 0, DATEDIFF(dd, 0, #fingerPrintsTmpWithRowNumber.LogDateTime)),
      ThirdLogoutDateTime = #fingerPrintsTmpWithRowNumber.LogDateTime,
	  ThirdLogoutTime = DATEADD(DAY, DATEDIFF(DAY, 0, '2000-01-01'),
           CONVERT(CHAR(8), #fingerPrintsTmpWithRowNumber.LogDateTime, 108))
  FROM DailyEnternaceExitRecord INNER JOIN
       #fingerPrintsTmpWithRowNumber ON 
	   DailyEnternaceExitRecord.Employee_id = #fingerPrintsTmpWithRowNumber.EmployeeId AND
	   #fingerPrintsTmpWithRowNumber.DateOfDay = DailyEnternaceExitRecord.[Date]
	   WHERE #fingerPrintsTmpWithRowNumber.RowNumber = 6 AND #fingerPrintsTmpWithRowNumber.LogType = 0

      
	    --update the transfered finger prints to old one
  UPDATE FingerprintTransferredData 
  SET IsOld = 1
  WHERE IsTransfered = 1

  --update the log type of finger prints if it is not correct
  --set IsTransfered as true for all current data
  UPDATE FingerprintTransferredData
  SET LogType = CASE
                WHEN #fingerPrintsTmpWithRowNumber.LogType = 1
				THEN 0 ELSE 1 END,
      IsTransfered = 1
  FROM FingerprintTransferredData INNER JOIN
       #fingerPrintsTmpWithRowNumber ON 
	   FingerprintTransferredData.Employee_id = #fingerPrintsTmpWithRowNumber.EmployeeId AND
	   #fingerPrintsTmpWithRowNumber.LogDateTime = FingerprintTransferredData.[LogDateTime] 
    WHERE FingerprintTransferredData.IsTransfered = 0
	
	DECLARE @maxDate date = (select Max(TodayDate) from #EMPLOYEE_RECURRENCES);  
    DECLARE @minDate date = (select Min(TodayDate) from #EMPLOYEE_RECURRENCES); 
	--DROP TEMP TABELS
    DROP TABLE #EMPLOYEE_RECURRENCES
    DROP TABLE #shifts
    DROP TABLE #FingerprintsData
    DROP TABLE #DistinctFingerprintsData 
    DROP TABLE #fingerPrintsTmpWithRowNumber

	EXEC SetIsNotCalculatedForAllAttendanceDetails @minDate = @minDate, @maxDate = @maxDate

  SET NOCOUNT ON;
END
