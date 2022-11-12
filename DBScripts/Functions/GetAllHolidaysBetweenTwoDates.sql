CREATE FUNCTION GetAllHolidaysBetweenTwoDates (  
      
  @MinDate DATE,  
   @MaxDate DATE  
)  
RETURNS @holidays TABLE (  
        HolidayDate Date  
    )  
AS  
begin     
declare @rangeDaysTmp TABLE(StartDate Date, EndDate Date);  
declare @allDaysTmp TABLE(DateOfDay Date);  
declare @tmpCounter TABLE(number int);  
insert into @rangeDaysTmp select @MinDate StartDate, @MaxDate EndDate;  
insert into @tmpCounter    
    SELECT TOP(100) v.number  
    FROM master.dbo.spt_values as v  
    WHERE v.type = 'P'  
    ORDER BY v.number ASC;  
  
insert into @allDaysTmp   
SELECT    
    [DATE] = CAST(DATEADD(DAY,y.number,T.StartDate) AS DATE)  
FROM @rangeDaysTmp as t  
    INNER JOIN @tmpCounter as y  
    ON t.EndDate > = DATEADD(DAY,y.number,T.StartDate);  
  
  
insert into @holidays   
SELECT DateOfDay as HolidayDate  
FROM    @allDaysTmp  
WHERE  DATEPART(weekday,DateOfDay) in (select DayOfWeek from PublicHoliday (nolock));  
  
delete from @rangeDaysTmp;  
insert into @rangeDaysTmp  
select StartDate, EndDate  from ChangeableHoliday changeableHolidays;  
insert into @rangeDaysTmp select  
 CONVERT(Datetime,CONVERT(varchar(10), YEAR(GETDATE()))   
 + CONVERT(varchar(10), FixedHoliday.Month)   
 + CONVERT(varchar(10), FixedHoliday.Day)) as StartDate,  
  CONVERT(Datetime,CONVERT(varchar(10), YEAR(GETDATE()))   
 + CONVERT(varchar(10), FixedHoliday.Month)   
 + CONVERT(varchar(10), FixedHoliday.Day)) as EndDate  from  FixedHoliday;  
  
insert into @holidays  
SELECT    
    [DATE] = CAST(DATEADD(DAY,y.number,T.StartDate) AS DATE)  
FROM @rangeDaysTmp as t  
    INNER JOIN @tmpCounter as y  
    ON t.EndDate > = DATEADD(DAY,y.number,T.StartDate)  
 where CAST(DATEADD(DAY,y.number,T.StartDate) AS DATE) >= @MinDate  
 and  CAST(DATEADD(DAY,y.number,T.StartDate) AS DATE) <= @MaxDate  
;  
return;  
end  