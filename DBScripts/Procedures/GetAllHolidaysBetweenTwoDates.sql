--exec GetAllHolidaysBetweenTwoDates  @MinDate = '2022-09-28', @MaxDate = '2022-10-28';

CREATE PROCEDURE GetAllHolidaysBetweenTwoDates(

  @MinDate DATE,
   @MaxDate DATE
)
AS  
begin  	
SET NOCOUNT OFF;	
select @MinDate StartDate, @MaxDate EndDate into #rangeDaysTmp;
WITH #tmpCounter AS
(
    SELECT TOP(100) v.number
    FROM master.dbo.spt_values as v
    WHERE v.type = 'P'
    ORDER BY v.number ASC
)
SELECT  
    [DATE] = CAST(DATEADD(DAY,y.number,T.StartDate) AS DATE)
	into #allDaysTmp
FROM #rangeDaysTmp as t
    INNER JOIN #tmpCounter as y
    ON t.EndDate > = DATEADD(DAY,y.number,T.StartDate)
;


SELECT  Date
into #allHolidaysDays
FROM    #allDaysTmp
WHERE  DATEPART(weekday,Date) in (select DayOfWeek from PublicHoliday (nolock))  


select StartDate, EndDate into #holidaysDaysTmp  from ChangeableHoliday changeableHolidays;
insert into #holidaysDaysTmp select
 CONVERT(Datetime,CONVERT(varchar(10), YEAR(GETDATE())) 
 + CONVERT(varchar(10), FixedHoliday.Month) 
 + CONVERT(varchar(10), FixedHoliday.Day)) as StartDate,
  CONVERT(Datetime,CONVERT(varchar(10), YEAR(GETDATE())) 
 + CONVERT(varchar(10), FixedHoliday.Month) 
 + CONVERT(varchar(10), FixedHoliday.Day)) as EndDate  from  FixedHoliday;


WITH #tmpCounter AS
(
    SELECT TOP(100) v.number
    FROM master.dbo.spt_values as v
    WHERE v.type = 'P'
    ORDER BY v.number ASC
)
insert into #allHolidaysDays
SELECT  
    [DATE] = CAST(DATEADD(DAY,y.number,T.StartDate) AS DATE)
FROM #holidaysDaysTmp as t
    INNER JOIN #tmpCounter as y
    ON t.EndDate > = DATEADD(DAY,y.number,T.StartDate)
	where CAST(DATEADD(DAY,y.number,T.StartDate) AS DATE) >= @MinDate
	and  CAST(DATEADD(DAY,y.number,T.StartDate) AS DATE) <= @MaxDate
;

drop table #holidaysDaysTmp;
drop table #rangeDaysTmp;
drop table #allDaysTmp;
SET NOCOUNT ON; 
select * from #allHolidaysDays;
drop table #allHolidaysDays;
end