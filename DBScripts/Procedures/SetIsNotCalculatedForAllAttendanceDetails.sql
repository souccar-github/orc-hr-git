CREATE PROC SetIsNotCalculatedForAllAttendanceDetails( 
     @minDate Date,
     @maxDate Date
)
AS
BEGIN 
  
  SET NOCOUNT ON;

  DECLARE @attendanceRecordId INT = (
          SELECT TOP(1) Id FROM AttendanceRecord WITH (NOLOCK) 
		  WHERE AttendanceMonthStatus = 3
		  ORDER BY [Date] DESC
	  );

   SELECT dailyAdujstmentDetails.Id AS recordId
      INTO #tmpIds
	  FROM 
      AttendanceRecord records  WITH (NOLOCK) INNER JOIN 
      AttendanceDailyAdjustment dailyAdujstments WITH (NOLOCK)  
	     ON  records.Id =  dailyAdujstments.AttendanceRecord_id  INNER JOIN  
	  AttendanceDailyAdjustmentDetail dailyAdujstmentDetails WITH (NOLOCK) 
	     ON  dailyAdujstments.Id =  dailyAdujstmentDetails.AttendanceDailyAdjustment_id    
   WHERE records.Id = @attendanceRecordId AND 
         (dailyAdujstmentDetails.Date >= @minDate OR 
		 dailyAdujstmentDetails.Date <= @maxDate)

  UPDATE AttendanceDailyAdjustmentDetail 
  SET IsCalculated = 0
  WHERE Id IN (SELECT recordId FROM #tmpIds)

  DELETE FROM #tmpIds

    INSERT INTO #tmpIds
    SELECT monthlyAdujstmentDetails.Id AS recordId
	  FROM 
      AttendanceRecord records  WITH (NOLOCK) INNER JOIN 
      AttendanceMonthlyAdjustment monthlyAdujstments WITH (NOLOCK)  
	     ON  records.Id =  monthlyAdujstments.AttendanceRecord_id  INNER JOIN  
	  AttendanceMonthlyAdjustmentDetail monthlyAdujstmentDetails WITH (NOLOCK) 
	     ON  monthlyAdujstments.Id =  monthlyAdujstmentDetails.AttendanceMonthlyAdjustment_id    
   WHERE records.Id = @attendanceRecordId AND 
         (monthlyAdujstmentDetails.Date >= @minDate OR 
		 monthlyAdujstmentDetails.Date <= @maxDate)

  UPDATE AttendanceMonthlyAdjustmentDetail 
  SET IsCalculated = 0
  WHERE Id IN (SELECT recordId FROM #tmpIds)

  
  DELETE FROM #tmpIds

    INSERT INTO #tmpIds
    SELECT withoutAdujstmentDetails.Id AS recordId
	  FROM 
      AttendanceRecord records  WITH (NOLOCK) INNER JOIN 
      AttendanceWithoutAdjustment withoutAdujstments WITH (NOLOCK)  
	     ON  records.Id =  withoutAdujstments.AttendanceRecord_id  INNER JOIN  
	  AttendanceWithoutAdjustmentDetail withoutAdujstmentDetails WITH (NOLOCK) 
	     ON  withoutAdujstments.Id =  withoutAdujstmentDetails.AttendanceWithoutAdjustment_id    
   WHERE records.Id = @attendanceRecordId AND 
         (withoutAdujstmentDetails.Date >= @minDate OR 
		 withoutAdujstmentDetails.Date <= @maxDate)

  UPDATE AttendanceWithoutAdjustmentDetail 
  SET IsCalculated = 0
  WHERE Id IN (SELECT recordId FROM #tmpIds)

  SET NOCOUNT OFF;
END