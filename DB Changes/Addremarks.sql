

ALTER TABLE ComplaintService ADD Remarks VARCHAR(500)


------------------------------------------------------------------------------------------------------

GO
/****** Object:  StoredProcedure [dbo].[proc_GetReturnCILists]    Script Date: 6/19/2022 2:59:08 PM ******/
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
set @strSql=@strSql + ' and CS.ComplaintNo like ''%' + @ComplaintInvoiceNo + '%''';                    
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

-------------------------------------------------------------------------------------------------------------------


GO
/****** Object:  StoredProcedure [dbo].[proc_AddEditComplaintService]    Script Date: 6/19/2022 4:13:27 PM ******/
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
@ComplaintSupportingDocument udt_ComplaintSupportingDocument readonly,
@status varchar(50) out,        
@message varchar(500) out,        
@RetComplaintId bigint out,
@ComplaintStatus int,
@Remarks varchar(500)
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
CustomerAddress,Status,BranchID,ComplaintSequence, EmployeeID, DealerID,InvoiceDate,ComplaintStatus,Remarks)        
VALUES(@ComplaintNo,@ComplaintDate,@InvoiceNo,@EnquiryType,@ComplaintMode,@ComplaintDescription,@CustomerName,@CustomerMobile,@CustomerEmail,
@CustomerAddress,@ActiveStatus,@CompanyBranchId,@MaxComplaintNo, @EmployeeID, @DealerID, @InvoiceDate,@ComplaintStatus,@Remarks)        

set @RetComplaintId=SCOPE_IDENTITY();         

insert into ComplaintServiceProductDetail(ComplaintId,ProductId,Remarks,Quantity)        
select @RetComplaintId,ProductId,Remarks, Quantity
from @ComplaintServiceProductDetail 

insert into ComplaintSupportingDocument(ComplaintId,DocumentTypeId,DocumentName,DocumentPath)          
select @RetComplaintId,DocumentTypeId,DocumentName,DocumentPath    
from @ComplaintSupportingDocument

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
InvoiceDate = @InvoiceDate,ComplaintStatus=@ComplaintStatus,Remarks = @Remarks
where ComplaintId=@ComplaintId  

    

delete from ComplaintServiceProductDetail where ComplaintId=@ComplaintId  

insert into ComplaintServiceProductDetail(ComplaintId,ProductId,Remarks,Quantity)        
select @ComplaintId,ProductId,Remarks,Quantity
from @ComplaintServiceProductDetail 

delete from ComplaintSupportingDocument where ComplaintId=@ComplaintId      
insert into ComplaintSupportingDocument(ComplaintId,DocumentTypeId,DocumentName,DocumentPath)          
select @ComplaintId,DocumentTypeId,DocumentName,DocumentPath    
from @ComplaintSupportingDocument

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







