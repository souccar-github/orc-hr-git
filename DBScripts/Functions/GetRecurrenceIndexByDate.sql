create function GetRecurrenceIndexByDate      
(      
   @employeeCardId int,  
   @attendanceFormId int,  
   @date Date  
)      
returns int      
as      
begin    
 declare @monthNumber int = (SELECT MONTH(@date));    
 declare @yearNumber int = (SELECT Year(@date));    
 declare @dayNumber int = (SELECT Day(@date));   
 if @monthNumber = 1  
 begin   
    set @monthNumber = 12;  
    set @yearNumber = @yearNumber - 1;  
 end  
 else   
 begin  
    set @monthNumber = @monthNumber - 1;  
 end  
   
declare @lastRecurrenceTmp TABLE(lastRecurrenceDate Date, lastRecurenceIndex int);  
declare @lastMonthDate Date = (CONVERT(varchar(10),@yearNumber) + '-' +  
CONVERT(varchar(10), @monthNumber) + '-' +CONVERT(varchar(10),  1));
 set @lastMonthDate = DATEADD(day, -1, @lastMonthDate);
 declare @lastDate date =  (CONVERT(varchar(10),Year(@date)) + '-' +  
CONVERT(varchar(10), MONTH(@date)) + '-' +CONVERT(varchar(10),  @dayNumber));  

 insert into @lastRecurrenceTmp  
 select top(1) details.Date as lastRecurrenceDate, details.RecurrenceIndex as lastRecurenceIndex  
   from AttendanceWithoutAdjustmentDetail details with (nolock) inner join   
   AttendanceWithoutAdjustment attendance with (nolock) on details.AttendanceWithoutAdjustment_id = attendance.Id  
   where Year(details.Date) = Year(@lastMonthDate) and   
         MONTH(details.Date) = month(@lastMonthDate) and  
   attendance.EmployeeAttendanceCard_id = @employeeCardId  
   order by 1 desc; 

          insert into @lastRecurrenceTmp  
 select top(1) details.Date as lastRecurrenceDate, details.RecurrenceIndex as lastRecurenceIndex  
   from AttendanceMonthlyAdjustmentDetail details with (nolock) inner join   
   AttendanceMonthlyAdjustment attendance with (nolock) on details.AttendanceMonthlyAdjustment_id = attendance.Id  
   where Year(details.Date) = Year(@lastMonthDate) and   
         MONTH(details.Date) = month(@lastMonthDate) and  
   attendance.EmployeeAttendanceCard_id = @employeeCardId  
   order by 1 desc;  

       insert into @lastRecurrenceTmp  
 select top(1) details.Date as lastRecurrenceDate, details.RecurrenceIndex as lastRecurenceIndex  
   from AttendanceDailyAdjustmentDetail details with (nolock) inner join   
   AttendanceDailyAdjustment attendance with (nolock) on details.AttendanceDailyAdjustment_id = attendance.Id  
   where Year(details.Date) = Year(@lastMonthDate) and   
         MONTH(details.Date) = month(@lastMonthDate) and 
   attendance.EmployeeAttendanceCard_id = @employeeCardId  
   order by 1 desc;  

 declare @lastRecurenceIndex int = (select top(1) lastRecurenceIndex from  @lastRecurrenceTmp);  
 declare @lastRecurrenceDate Date = (select top(1) lastRecurrenceDate from  @lastRecurrenceTmp);  
  
 if @lastRecurenceIndex is not null  
 begin  
 declare @countOfRecurrences int = (select count(1) from   
 WorkshopRecurrence recurrences with (nolock)  
 where recurrences.AttendanceForm_id = @attendanceFormId)   
   
 declare @lastRecurrence int = (select top(1) recurrences.RecurrenceOrder from   
 WorkshopRecurrence recurrences with (nolock)   
 where recurrences.AttendanceForm_id = @attendanceFormId  
 order by 1 desc)   
  
 --if @lastRecurenceIndex + 1 > @countOfRecurrences   
 --return 0  
  
-- declare @firstDayInMonth date = (CONVERT(varchar(10),Year(@date)) + '-' +  
--CONVERT(varchar(10), MONTH(@date)) + '-01');  
declare @rangeDaysTmp TABLE(StartDate Date, EndDate Date);  
insert into @rangeDaysTmp select @lastRecurrenceDate StartDate, @date EndDate;  
declare @tmpCounter TABLE(number int);  
insert into @tmpCounter    
    SELECT TOP(100) v.number  
    FROM master.dbo.spt_values as v  
    WHERE v.type = 'P'  
    ORDER BY v.number ASC;  
  
return (SELECT  top (1)  
 (y.number + 1 + @lastRecurenceIndex) % @countOfRecurrences as recurrence  
FROM @rangeDaysTmp as t  
    INNER JOIN @tmpCounter as y  
    ON t.EndDate > = DATEADD(DAY,y.number,T.StartDate)  
 order by y.number desc);  
   
 end  
 else   
 return 0;  
   
 return 0;  
end 