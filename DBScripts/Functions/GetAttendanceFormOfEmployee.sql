create function GetAttendanceFormOfEmployee    
(    
   @employeeId int,
   @generalAttendanceId int
)    
returns int    
as    
begin  
 DECLARE @gradeAttendanceFormId int; 
  set @gradeAttendanceFormId =   
(select grade.AttendanceForm_id from   
AssigningEmployeeToPosition assign inner join   
Position pos on assign.Position_Id = pos.Id inner join   
JobDescription jobDesc on pos.JobDescription_id = jobDesc.Id inner join   
JobTitle title on jobDesc.JobTitle_id = title.Id inner join 
Grade grade on title.Grade_id = grade.Id 
where assign.Employee_id = @employeeId and assign.IsPrimary = 1);

DECLARE @attendanceFormId int; 
  set @attendanceFormId =   
(select empcard.AttendanceForm_id from   
Employee emp inner join   
EmployeeCard empcard on emp.Id = empcard.Employee_id
where emp.Id = @employeeId);
  
IF @attendanceFormId is not null 
   return (@attendanceFormId);  
ELSE  
BEGIN  
   IF  @gradeAttendanceFormId is not null 
   return (@gradeAttendanceFormId);   
   ELSE  
      return @generalAttendanceId;  
END;  
      return null;  
end 