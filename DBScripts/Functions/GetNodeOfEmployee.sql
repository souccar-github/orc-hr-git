create function GetNodeOfEmployee    
(    
   @employeeId int  
)    
returns int    
as    
begin  
 DECLARE @positionTmp table (positionId int, nodeId int, isPrimary bit, datePos Date)  
  insert @positionTmp  
select assign.Position_Id, nodeTable.Id, assign.IsPrimary, assign.CreationDate from   
AssigningEmployeeToPosition assign inner join   
Position pos on assign.Position_Id = pos.Id inner join   
JobDescription jobDesc on pos.JobDescription_id = jobDesc.Id inner join   
Node nodeTable on jobDesc.Node_id = nodeTable.Id inner join   
Employee emp on assign.Employee_id = emp.Id   
where emp.Id = @employeeId  
  
IF (select count(1) from @positionTmp) > 1  
   return (select top(1) nodeId from @positionTmp where isPrimary = 0 order by 1 desc);  
ELSE  
BEGIN  
   IF  (select count(1) from @positionTmp) = 1  
       return (select top(1) nodeId from @positionTmp where isPrimary = 1 order by 1 desc);  
   ELSE  
      return null;  
END;  
      return null;  
end 