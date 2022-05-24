
GO
ALTER TABLE Vendor ADD IsTCS BIT
GO
ALTER TABLE ComplaintService Add EmployeeID INT
GO
ALTER TABLE DocumentType ADD ModuleType VARCHAR(100)
GO
ALTER TABLE ComplaintService Add DealerID INT


GO
-- =============================================
-- Author		:		Dheeraj Kumar
-- Create date	:		14 May, 2022
-- Description	:		This sp is used to get employee list.
-- =============================================
-- EXECUTE [dbo].[usp_GetEmployeeList] 3
CREATE PROCEDURE [dbo].[usp_GetEmployeeList] 	
	@DepartmentId INT
AS
-- -------------------------------------------------------- History -------------------------------------------------------------------------------- 
--    Date		Story					Developer			Description  
-- ----------- ----------------------- ------------------- ----------------------------------------------------------------------------------------- 
-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
BEGIN 
	SELECT EMP.EmployeeId, CONCAT(EMP.FirstName, ' ', EMP.LastName) AS EmployeeName FROM Employee AS EMP 
	INNER JOIN Designation AS D ON EMP.DesignationId = D.DesignationId
	INNER JOIN Department AS DEP ON D.DepartmentId = DEP.DepartmentId
	WHERE DEP.DepartmentId = @DepartmentId
END

GO

GO
-- =============================================
-- Author		:		<Author Name>
-- Create date	:		<Created Date>
-- Description	:		<Purpose of the SP>
-- ==============================================
-- proc_GetProductAutoCompleteList 'H'   
ALTER proc [dbo].[proc_GetProductAutoCompleteList] 
(              
@SearchTerm NVARCHAR(20)  
)              
AS   
-- -------------------------------------------------------- History -------------------------------------------------------------------------------- 
--    Date			Story					Developer			Description  
-- ----------- ----------------------- ------------------- ----------------------------------------------------------------------------------------- 
-- 14-May-2022								Dheeraj Kumar		Get All product in this SP (As discuss with Prabhaker)
-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
BEGIN              
SET NOCOUNT ON; 

		SELECT P.Productid, P.ProductName, P.ProductCode, ISNULL(P.IsWarrantyProduct , 0 ) AS IsWarrantyProduct ,
		NULL AS WarrantyStartDate, NULL AS WarrantyEndDate
		FROM Product AS P WHERE P.ProductName LIKE  '%'+@SearchTerm+'%' 

SET NOCOUNT OFF;              
END 

--SELECTp.Productid,
--p.ProductName,p.ProductCode,
--replace(convert(varchar, wpd.WarrantyStartDate,106),' ','-') WarrantyStartDate,
--replace(convert(varchar, wpd.WarrantyEndDate,106),' ','-') WarrantyEndDate
--from [dbo].[WarrantyProductDetail] wpd
--inner join Product p on wpd.Productid=p.Productid
--where p.ProductName like '%'+@SearchTerm+'%' 
--and CONVERT(date, GETDATE())  between  wpd.WarrantyStartDate and wpd.WarrantyEndDate   


GO
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
@ComplaintServiceProductDetail udt_ComplaintServiceProductDetail readonly,  
@status varchar(50) out,        
@message varchar(500) out,        
@RetComplaintId bigint out  
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
CustomerAddress,Status,BranchID,ComplaintSequence, EmployeeID, DealerID)        
VALUES(@ComplaintNo,@ComplaintDate,@InvoiceNo,@EnquiryType,@ComplaintMode,@ComplaintDescription,@CustomerName,@CustomerMobile,@CustomerEmail,
@CustomerAddress,@ActiveStatus,@CompanyBranchId,@MaxComplaintNo, @EmployeeID, @DealerID)        

set @RetComplaintId=SCOPE_IDENTITY();         

insert into ComplaintServiceProductDetail(ComplaintId,ProductId,Remarks)        
select @RetComplaintId,ProductId,Remarks
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
DealerID = @DealerID
where ComplaintId=@ComplaintId  

    

delete from ComplaintServiceProductDetail where ComplaintId=@ComplaintId  

insert into ComplaintServiceProductDetail(ComplaintId,ProductId,Remarks)        
select @ComplaintId,ProductId,Remarks
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






GO
-- =============================================
-- Author		:		<Author Name>
-- Create date	:		<Created Date>
-- Description	:		<Purpose of the SP>
-- ==============================================
-- EXECUTE [dbo].[proc_GetComplaintServiceDetail] 1  
ALTER  proc [dbo].[proc_GetComplaintServiceDetail]            
(                      
@ComplaintId int                    
)                      
as 
-- -------------------------------------------------------- History -------------------------------------------------------------------------------- 
--    Date			Story					Developer			Description  
-- ----------- ----------------------- ------------------- -----------------------------------------------------------------------------------------
-- 14-May-2022								Dheeraj Kumar		Get EmployeeId Column Value.
-- 21-May-2022								Dheeraj Kumar		Insert DealerID Column Value.
-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
begin                      
set nocount on;                    
select E.ComplaintId,                
E.ComplaintNo,
E.ComplaintDate,
E.InvoiceNo,                    
E.EnquiryType,    
E.BranchId,
E.ComplaintMode,
E.ComplaintDescription,
E.CustomerName,
E.CustomerMobile,
E.CustomerEmail,
E.CustomerAddress,
E.Status, 
E.EmployeeID,
ISNULL(E.DealerID, 0 ) AS DealerID
from [ComplaintService] E   
inner join ComapnyBranch  cb on E.BranchId=cb.CompanyBranchId 
where E.ComplaintId=@ComplaintId
set nocount off;                      
end 



GO
-- =============================================
-- Author		:		<Author Name>
-- Create date	:		<Created Date>
-- Description	:		<Purpose of the SP>
-- ==============================================
-- proc_GetDocumentTypeDetail 1  
ALTER proc [dbo].[proc_GetDocumentTypeDetail] --proc_GetCustomerTypeDetail 2
(  
@DocumentTypeId int
)  
AS  
-- -------------------------------------------------------- History -------------------------------------------------------------------------------- 
--    Date		Story					Developer			Description  
-- ----------- ----------------------- ------------------- ----------------------------------------------------------------------------------------- 
-- 16-May-2022							DHEERAJ KUMAR		Return one or more property ModuleType
-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
BEGIN  
SET NOCOUNT ON;  
	SELECT d.DocumentTypeId,  d.DocumentTypeDesc, d.Status,ISNULL(d.CompanyBranchId,0) AS CompanyBranchId, d.ModuleType AS ModuleType
	FROM DocumentType AS d WHERE d.DocumentTypeId=@DocumentTypeId
SET NOCOUNT OFF;  
END






GO

ALTER proc [dbo].[proc_GetDocumentTypes]  --proc_GetDocumentTypes '',1,'',0,'Admin'
(
@DocumentTypeDesc varchar(100),  
@CompanyId int, 
@Status varchar(50),
@CompanyBranchId  int =0,
@ModuleType  varchar(50)
)  
as  
begin  
set nocount on;  
  
declare @strSql as nvarchar(1000); 
  
set @strSql='select d.DocumentTypeId,  
d.DocumentTypeDesc, 
d.CompanyId,
d.Status,
isnull(d.CompanyBranchId,0)  CompanyBranchId,
isnull(cb.BranchName,'''')  CompanyBranchName,
d.ModuleType 
from DocumentType d
left join [ComapnyBranch] cb on d.CompanyBranchId = cb.CompanyBranchId
 where  d.CompanyId='+ cast(@CompanyId as varchar) + ' ';   
 
if @DocumentTypeDesc<>''  
begin  
set @strSql=@strSql + ' and d.DocumentTypeDesc like ''%' + @DocumentTypeDesc + '%''';  
end   
if @Status<>''
begin  
set @strSql=@strSql + ' and d.Status = ' + @Status + '';  
end 
if @CompanyBranchId<>0                  
begin                  
set @strSql=@strSql + ' and d.CompanyBranchId =''' + cast(@CompanyBranchId as varchar) + '''';                  
end 
if (@ModuleType<>'' AND @ModuleType<>'0')
begin  
set @strSql=@strSql + ' and d.ModuleType = ''' + @ModuleType + '''';  
end 
set @strSql=@strSql + ' order by d.DocumentTypeDesc ';  
exec sp_executesql @strSql   
set nocount off;  
end 

















GO
-- =============================================
-- Author		:		<Author Name>
-- Create date	:		<Created Date>
-- Description	:		<Purpose of the SP>
-- ==============================================
-- EXECUTE [dbo].[proc_GetComplaintService] '','0','0' , '', '', '0', 0, 0, 0
ALTER proc [dbo].[proc_GetComplaintService] 
( 
@ComplaintNo varchar(50)='',                
@EnquiryType varchar(50)='',
@ComplaintMode varchar(50)='',
@CustomerMobile varchar(50)='',                
@CustomerName varchar(50)='', 
@Status varchar(50),
@CompanyBranchId  int =0  ,
@ServiceEngineerId  int =0    , 
@DealerId  int =0     
)                    
as   
-- -------------------------------------------------------- History -------------------------------------------------------------------------------- 
--    Date			Story					Developer			Description  
-- ----------- ----------------------- ------------------- -----------------------------------------------------------------------------------------
-- 21-May-2022								Dheeraj Kumar		Filter records by employee / dealer id
-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
begin                    
set nocount on;                    
                    
declare @strSql as nvarchar(4000);                 
set @strSql='select E.ComplaintId,                
E.ComplaintNo,                     
E.EnquiryType, 
E.ComplaintMode,
E.CustomerMobile,
E.CustomerName , E.Status as Status,
isnull(E.BranchId,0)  CompanyBranchId,
isnull(cb.BranchName,'''')  CompanyBranchName ,
C.CustomerName AS DealerName,
CONCAT(Emp.FirstName, '' '', Emp.LastName ) AS EmployeeName
from [ComplaintService] E 
left join [ComapnyBranch] cb on E.BranchId = cb.CompanyBranchId
LEFT JOIN Employee AS Emp ON E.EmployeeID = Emp.EmployeeID
LEFT JOIN Customer AS C ON E.DealerID = C.CustomerID
where 1=1'          

          
if @ComplaintNo<>''                    
begin                    
set @strSql=@strSql + ' and E.ComplaintNo like ''%' + @ComplaintNo + '%''';                    
end 
     
if @EnquiryType<>'0'                    
begin                    
set @strSql=@strSql +' and E.EnquiryType like ''%' + @EnquiryType + '%''';                    
end                 
   
if @ComplaintMode <> '0'                    
begin                    
set @strSql=@strSql + ' and E.ComplaintMode = '''+@ComplaintMode + '''';                    
end                 
   
if @CustomerMobile<>''                    
begin                    
set @strSql=@strSql + ' and E.CustomerMobile = ''' + @CustomerMobile + '''';                    
end

if @CustomerName<>''                    
begin                    
set @strSql=@strSql + ' and E.CustomerName LIKE ''%' + @CustomerName + '%''';                    
end

if @CompanyBranchId<>0                  
begin                  
set @strSql=@strSql + ' and E.BranchId =' + cast(@CompanyBranchId as varchar) + '';                  
end  
            
if @Status <> ''                    
begin   
set @strSql=@strSql + ' and  E.Status = ''' + convert(varchar(50),@Status) + '''';                                     
end 
 

 if @ServiceEngineerId <> 0                  
begin                  
set @strSql=@strSql + ' and E.EmployeeID =''' + cast(@ServiceEngineerId as varchar) + '''';                  
end 

if @DealerId <> 0                  
begin                  
set @strSql=@strSql + ' and E.DealerID =''' + cast(@DealerId as varchar) + '''';                  
end 
 
          
set @strSql=@strSql + ' order by E.ComplaintNo Desc '

PRINT @strSql
--select @strSql
exec sp_executesql @strSql    

                    
set nocount off;                    
end 


