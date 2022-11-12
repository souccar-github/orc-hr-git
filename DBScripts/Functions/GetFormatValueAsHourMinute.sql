CREATE FUNCTION [dbo].[GetFormatValueAsHourMinute](
@NumberOfHours REAL
)
RETURNS VARCHAR(5)  
AS    
BEGIN  
DECLARE @NumberOfSeconds INT 
SET @NumberOfSeconds = @NumberOfHours * 60    

RETURN LEFT(CONVERT(VARCHAR, DATEADD(MINUTE, @NumberOfSeconds, 0), 108), 5) 

END 
