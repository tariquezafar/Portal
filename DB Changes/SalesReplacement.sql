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
-- =============================================-- Author		:		<Author Name>-- Create date	:		<Created Date>-- Description	:		<Purpose of the SP>-- ==============================================-- EXECUTE [dbo].[proc_GetComplaintServiceDetail] 7ALTER  proc [dbo].[proc_GetComplaintServiceDetail]            (                      @ComplaintId int                    )                      as -- -------------------------------------------------------- History -------------------------------------------------------------------------------- --    Date			Story					Developer			Description  -- ----------- ----------------------- ------------------- ------------------------------------------------------------------------------------------- 14-May-2022								Dheeraj Kumar		Get EmployeeId Column Value.-- 21-May-2022								Dheeraj Kumar		Insert DealerID Column Value.-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////begin                      set nocount on;                    select E.ComplaintId,                E.ComplaintNo,E.ComplaintDate,E.InvoiceNo,                    E.EnquiryType,    E.BranchId,E.ComplaintMode,E.ComplaintDescription,E.CustomerName,E.CustomerMobile,E.CustomerEmail,E.CustomerAddress,E.Status, E.EmployeeID,ISNULL(E.ComplaintStatus,0) AS ComplaintStatus,ISNULL(E.DealerID, 0 ) AS DealerID,CASE WHEN E.InvoiceDate IS NULL OR E.InvoiceDate < '1900-12-01' THEN '' ELSE REPLACE(CONVERT(VARCHAR, E.InvoiceDate,106),' ','-') END InvoiceDate      from [ComplaintService] E   inner join ComapnyBranch  cb on E.BranchId=cb.CompanyBranchId where E.ComplaintId=@ComplaintIdset nocount off;                      end 

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
-- =============================================-- Author		:		<Author Name>-- Create date	:		<Created Date>-- Description	:		<Purpose of the SP>-- ==============================================-- EXECUTE [dbo].[proc_GetComplaintService] '','0','0' , '', '', '', 0, 0, 0,1ALTER proc [dbo].[proc_GetComplaintService] ( @ComplaintNo varchar(50)='',                @EnquiryType varchar(50)='',@ComplaintMode varchar(50)='',@CustomerMobile varchar(50)='',                @CustomerName varchar(50)='', @Status varchar(50),@CompanyBranchId  int =0  ,@ServiceEngineerId  int =0    , @DealerId  int =0  ,@ComplaintStatus int = 0)                    as   -- -------------------------------------------------------- History -------------------------------------------------------------------------------- --    Date			Story					Developer			Description  -- ----------- ----------------------- ------------------- ------------------------------------------------------------------------------------------- 21-May-2022								Dheeraj Kumar		Filter records by employee / dealer id-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////begin                    set nocount on;                     declare @strSql as nvarchar(4000);                 set @strSql='select E.ComplaintId,                E.ComplaintNo,                     E.EnquiryType, E.ComplaintMode,E.CustomerMobile,E.CustomerName , E.Status as Status,isnull(E.BranchId,0)  CompanyBranchId,isnull(cb.BranchName,'''')  CompanyBranchName ,C.CustomerName AS DealerName,CONCAT(Emp.FirstName, '' '', Emp.LastName ) AS EmployeeName,ISNULL(E.ComplaintStatus,0) AS ComplaintStatusfrom [ComplaintService] E left join [ComapnyBranch] cb on E.BranchId = cb.CompanyBranchIdLEFT JOIN Employee AS Emp ON E.EmployeeID = Emp.EmployeeIDLEFT JOIN Customer AS C ON E.DealerID = C.CustomerIDwhere 1=1'                    if @ComplaintNo<>''                    begin                    set @strSql=@strSql + ' and E.ComplaintNo like ''%' + @ComplaintNo + '%''';                    end      if @EnquiryType<>'0'                    begin                    set @strSql=@strSql +' and E.EnquiryType like ''%' + @EnquiryType + '%''';                    end                    if @ComplaintMode <> '0'                    begin                    set @strSql=@strSql + ' and E.ComplaintMode = '''+@ComplaintMode + '''';                    end                    if @CustomerMobile<>''                    begin                    set @strSql=@strSql + ' and E.CustomerMobile = ''' + @CustomerMobile + '''';                    endif @CustomerName<>''                    begin                    set @strSql=@strSql + ' and E.CustomerName LIKE ''%' + @CustomerName + '%''';                    endif @CompanyBranchId<>0                  begin                  set @strSql=@strSql + ' and E.BranchId =' + cast(@CompanyBranchId as varchar) + '';                  end              if @Status <> ''                    begin   set @strSql=@strSql + ' and  E.Status = ''' + convert(varchar(50),@Status) + '''';                                     end   if @ServiceEngineerId <> 0                  begin                  set @strSql=@strSql + ' and E.EmployeeID =''' + cast(@ServiceEngineerId as varchar) + '''';                  end if @DealerId <> 0                  begin                  set @strSql=@strSql + ' and E.DealerID =''' + cast(@DealerId as varchar) + '''';                  end if @ComplaintStatus <> 0                  begin                  set @strSql=@strSql + ' and ISNULL(E.ComplaintStatus,0) =''' +cast(@ComplaintStatus as varchar) + '''';                 end            set @strSql=@strSql + ' order by E.ComplaintNo Desc '--PRINT @strSql--select @strSqlexec sp_executesql @strSql                        set nocount off;                    end 


