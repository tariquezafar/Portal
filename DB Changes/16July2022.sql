GO

ALTER TABLE ComplaintService ADD UserId int 
ALTER TABLE ComplaintService DROP COLUMN  EmployeeId 

---------------------------------------------------------------------------------------------------------------------------------------------------------------------

GO
/****** Object:  StoredProcedure [dbo].[usp_GetEmployeeList]    Script Date: 7/16/2022 12:10:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[usp_GetEmployeeList] --104	
	@RoleId INT
AS
-- -------------------------------------------------------- History -------------------------------------------------------------------------------- 
--    Date		Story					Developer			Description  
-- ----------- ----------------------- ------------------- ----------------------------------------------------------------------------------------- 
-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
BEGIN 
	--SELECT EMP.EmployeeId, CONCAT(EMP.FirstName, ' ', EMP.LastName) AS EmployeeName FROM Employee AS EMP 
	--INNER JOIN Designation AS D ON EMP.DesignationId = D.DesignationId
	--INNER JOIN Department AS DEP ON D.DepartmentId = DEP.DepartmentId
	--WHERE DEP.DepartmentId = @DepartmentId
	SELECT UserId,FullName FROM [USER]
	WHERE RoleId = @RoleId
END

------------------------------------------------------------------------------------------------------------------------------------------------------------------------

GO
/****** Object:  StoredProcedure [dbo].[proc_GetComplaintServiceDetail]    Script Date: 7/16/2022 12:44:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

----------------------------------------------------------------------------------------------------------------------------------------------------------------------

GO
/****** Object:  StoredProcedure [dbo].[proc_AddEditComplaintService]    Script Date: 7/16/2022 12:52:54 PM ******/
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
@UserId INT = NULL,  
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
CustomerAddress,Status,BranchID,ComplaintSequence, UserId, DealerID,InvoiceDate,ComplaintStatus,Remarks)        
VALUES(@ComplaintNo,@ComplaintDate,@InvoiceNo,@EnquiryType,@ComplaintMode,@ComplaintDescription,@CustomerName,@CustomerMobile,@CustomerEmail,
@CustomerAddress,@ActiveStatus,@CompanyBranchId,@MaxComplaintNo, @UserId, @DealerID, @InvoiceDate,@ComplaintStatus,@Remarks)        

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
UserId = @UserId,
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

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

GO
/****** Object:  StoredProcedure [dbo].[proc_GetComplaintService]    Script Date: 7/16/2022 12:50:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
GO
/****** Object:  StoredProcedure [dbo].[proc_GetPOSchedules]    Script Date: 7/16/2022 4:01:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[proc_ExportPOSchedules] --  proc_ExportPOSchedules 31   
(              
@POId bigint    
)              
as              
begin              
set nocount on;              
select 
p.PONo,
replace(convert(varchar, p.PODate,106), '','-') AS PODate,
p.VendorName ,cb.BranchName,ps.LocationName,ProductName,ProductCode,UOMName,ps.Quantity,
replace(convert(varchar, ps.DeliveryDate,106), '','-') AS DeliveryDate,
ps.SchQuantity,
replace(convert(varchar, ps.ConDeliveryDate,106), '','-') AS ConDeliveryDate
from POProductSchedule ps  
Left join PO p on  ps.POId = p.POId
LEFT JOIN [ComapnyBranch] cb on ps.CompanyBranchId = cb.CompanyBranchId
where  ps.POId=@POId 
set nocount off;              
end 












