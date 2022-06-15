USE [Demo]
GO
/****** Object:  StoredProcedure [dbo].[proc_GetReturnSILists]    Script Date: 6/11/2022 11:07:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--select * from Warranty
--  proc_GetReturnCILists '','',0
            
ALTER PROC [dbo].[proc_GetReturnCILists] 
(                    
@ComplaintInvoiceNo varchar(50),                   
@CustomerMobileNo varchar(50),
@CompanyBranchId int=0     
)                    
as                    
begin                    
set nocount on;                    
                    
declare @strSql as nvarchar(4000);                 
set @strSql='select CS.ComplaintId,                            
CS.CustomerMobile,
replace(convert(varchar, CS.ComplaintDate,106),'' '',''-'') AS ComplaintDate,
CS.EnquiryType,
CS.ComplaintNo
from [ComplaintService] CS         
where 1=1 ';                    
                    
          
if @ComplaintInvoiceNo<>''                    
begin                    
set @strSql=@strSql + ' and CS.ComplaintId like ''%' + @ComplaintInvoiceNo + '%''';                    
end   
if @CustomerMobileNo<>''                    
begin                    
set @strSql=@strSql + ' and CS.CustomerMobile like ''%' + @CustomerMobileNo + '%''';                    
end               
          
if @CompanyBranchId<>0                  
begin                  
set @strSql=@strSql + ' and CS.BranchID =''' + cast(@CompanyBranchId as varchar) + '''';                  
end 
          
set @strSql=@strSql + ' order by CS.ComplaintId Desc ';                    
exec sp_executesql @strSql   
--print @strSql
                    
set nocount off;                    
end 


