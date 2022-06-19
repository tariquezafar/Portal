GO
ALTER TABLE ComplaintService ADD ComplaintStatus INT
GO

------------------------------------------------------------------------------------------------------------------------------

GO
/****** Object:  StoredProcedure [dbo].[proc_AddEditComplaintService]    Script Date: 6/17/2022 7:43:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author		:		<Author Name>
-- Create date	:		<Created Date>
-- Description	:		<Purpose of the SP>
-- ==============================================
ALTER proc [dbo].[proc_AddEditComplaintService]        
(        
@ComplaintId bigint, 
@ComplaintDate date,       
@EnquiryType varchar(50),  
@InvoiceNo varchar(50) = NULL,
@ComplaintMode varchar(50),    
@ComplaintDescription varchar(500), 
@CustomerName varchar(50),
@CustomerMobile varchar(50),
@CustomerAddress varchar(100) = NULL,   
@CompanyBranchId int, 
@ActiveStatus bit,  
@EmployeeID INT = NULL,  
@CustomerEmail varchar(50) = NULL,
@DealerID INT = NULL,
@InvoiceDate DATE = NULL,
@ComplaintServiceProductDetail udt_ComplaintServiceProductDetail readonly,  
@status varchar(50) out,        
@message varchar(500) out,        
@RetComplaintId bigint out,
@ComplaintStatus int
)        
AS
-- -------------------------------------------------------- History -------------------------------------------------------------------------------- 
--    Date			Story					Developer			Description  
-- ----------- ----------------------- ------------------- -----------------------------------------------------------------------------------------
-- 14-May-2022								Dheeraj Kumar		Insert EmployeeId Column Value.
-- 21-May-2022								Dheeraj Kumar		Insert DealerID Column Value.
-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
BEGIN        
BEGIN TRY        
BEGIN TRANSACTION        
	
	DECLARE @FinYearStartDate AS DATE, @FinYearEndDate AS DATE, @FinYearCode AS VARCHAR(20), @RowCount INT = 0, @RowNum INT,
	@JobWorkProductId BIGINT, @jobWorkQuantity DECIMAL(18,2), @StockQuantity DECIMAL(18,2), @CompanyCode VARCHAR(10), @CompanyBranchCode VARCHAR(10),
	@CompanyStateId INT

	DECLARE @temp_ProductEntry AS TABLE
	(    
		RowId INT,    
		ProductId BIGINT,    
		Quantity DECIMAL(18,2)    
	)

	SELECT @CompanyBranchCode=CB.CompanyBranchCode FROM ComapnyBranch CB WHERE CB.CompanyBranchId=@CompanyBranchId  

IF @ComplaintId=0  -- INSERT START        
BEGIN        
        
DECLARE @ComplaintNo AS VARCHAR(50);        
DECLARE @MaxComplaintNo as int;        
 --ComplaintId=@ComplaintId        
SELECT @MaxComplaintNo=MAX(ComplaintSequence) FROM ComplaintService WHERE  BranchID=@CompanyBranchId
IF ISNULL(@MaxComplaintNo,0)<>0        
BEGIN        
 SET @MaxComplaintNo=@MaxComplaintNo+1;        
END        
ELSE        
BEGIN        
 SET @MaxComplaintNo=1;        
END        
set @ComplaintNo=@CompanyBranchCode + '/CS/'+ FORMAT(@MaxComplaintNo,'000#');        

        
INSERT INTO ComplaintService(ComplaintNo,ComplaintDate,InvoiceNo,EnquiryType,ComplaintMode,ComplaintDescription,CustomerName,CustomerMobile,CustomerEmail,
CustomerAddress,Status,BranchID,ComplaintSequence, EmployeeID, DealerID,InvoiceDate,ComplaintStatus)        
VALUES(@ComplaintNo,@ComplaintDate,@InvoiceNo,@EnquiryType,@ComplaintMode,@ComplaintDescription,@CustomerName,@CustomerMobile,@CustomerEmail,
@CustomerAddress,@ActiveStatus,@CompanyBranchId,@MaxComplaintNo, @EmployeeID, @DealerID, @InvoiceDate,@ComplaintStatus)        

set @RetComplaintId=SCOPE_IDENTITY();         

insert into ComplaintServiceProductDetail(ComplaintId,ProductId,Remarks,Quantity)        
select @RetComplaintId,ProductId,Remarks, Quantity
from @ComplaintServiceProductDetail 


SET @message='';        
set @status='SUCCESS';        
         
END        
ELSE  -- MODIFY START        
BEGIN    
update ComplaintService set EnquiryType=@EnquiryType,
ComplaintMode=@ComplaintMode,ComplaintDate=@ComplaintDate,InvoiceNo=@InvoiceNo,
ComplaintDescription=@ComplaintDescription,CustomerName=@CustomerName,
CustomerMobile=@CustomerMobile, CustomerEmail=@CustomerEmail,CustomerAddress=@CustomerAddress,
BranchID=@CompanyBranchId,Status=@ActiveStatus,
EmployeeID = @EmployeeID,
DealerID = @DealerID,
InvoiceDate = @InvoiceDate,ComplaintStatus=@ComplaintStatus
where ComplaintId=@ComplaintId  

    

delete from ComplaintServiceProductDetail where ComplaintId=@ComplaintId  

insert into ComplaintServiceProductDetail(ComplaintId,ProductId,Remarks,Quantity)        
select @ComplaintId,ProductId,Remarks,Quantity
from @ComplaintServiceProductDetail 

 SET @message='';        
 set @status='SUCCESS';        
 set @RetComplaintId=@ComplaintId;         
END       
         
COMMIT TRANSACTION        
END TRY        
BEGIN CATCH        
IF @@TRANCOUNT > 0        
BEGIN        
 ROLLBACK TRANSACTION;        
END        
set @status ='FAIL';        
set @message = ERROR_MESSAGE();        
set @RetComplaintId=0;        
END CATCH;        
end  



------------------------------------------------------------------------------------------------------------------------------

GO
/****** Object:  StoredProcedure [dbo].[proc_GetComplaintServiceDetail]    Script Date: 6/17/2022 8:34:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================

--------------------------------------------------------------------------------------------------------------------------------------------------

GO
/****** Object:  StoredProcedure [dbo].[proc_GetReturnCILists]    Script Date: 6/18/2022 12:11:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--select * from Warranty
--  proc_GetReturnCILists '','',1
            
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
where CS.ComplaintStatus = 2 ';                    
                    
          
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
print @strSql
                    
set nocount off;                    
end 


--------------------------------------------------------------------------------------------------------------------------------------------------

GO
/****** Object:  StoredProcedure [dbo].[proc_GetComplaintService]    Script Date: 6/17/2022 8:00:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================

