GO
ALTER TABLE MRN ADD LocationID INT

GO
-- =============================================
-- Author		:		<Author Name>
-- Create date	:		<Created Date>
-- Description	:		<Purpose of the SP>
-- ==============================================
ALTER proc [dbo].[proc_AddEditMRNQC]            
(           
@MRNId bigint,           
@MRNDate date,          
@GRNo varchar(50),        
@GRDate date,         
@QualityCheckId bigint,          
@QualityCheckNo varchar(100)='', 
@QualityCheckDate date,            
@VendorId int,            
@VendorName varchar(100)='',            
@ContactPerson VARCHAR(100)='', 
@LocationID INT = NULL,
@ShippingContactPerson VARCHAR(100)='',        
@ShippingBillingAddress varchar(100)='',            
@ShippingCity varchar(50)='',            
@ShippingStateId int,            
@ShippingCountryId int,            
@ShippingPinCode varchar(20)='',            
@ShippingTINNo varchar(20)='',            
@ShippingEmail varchar(100)='',            
@ShippingMobileNo varchar(20)='',            
@ShippingContactNo varchar(20)='',            
@ShippingFax varchar(20)='',            
@CompanyBranchId int=0,                
@DispatchRefNo varchar(50)='',            
@DispatchRefDate date,         
        
@LRNO varchar(50)='',        
@LRDate date,          
@ApprovalStatus varchar(20),    
        
@TransportVia varchar(50)='',        
@NoOfPackets int,        
@Remarks1 varchar(2000)='',        
@Remarks2 varchar(2000)='',        
@FinYearId int,            
@CompanyId int,            
@CreatedBy int,            
@MRNProductDetail udt_MRNProductDetail readonly,  
@MRNSupportingDocument udt_MRNSupportingDocument readonly,           
@status varchar(50) out,            
@message varchar(500) out,            
@RetMRNId bigint out            
)            
as   
-- -------------------------------------------------------- History -------------------------------------------------------------------------------- 
--    Date		Story					Developer			Description  
-- ----------- ----------------------- ------------------- ----------------------------------------------------------------------------------------- 
-- 26-May-2022							DHEERAJ KUMAR		Insert locationID column value.
-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
begin            
BEGIN TRY            
BEGIN TRANSACTION            
            
DECLARE @FinYearStartDate as date;            
DECLARE @FinYearEndDate as date;            
DECLARE @FinYearCode AS VARCHAR(20);            
DECLARE @CompanyCode as varchar(10); 


declare @RowCount int=0;    
declare @RowNum int;   
DECLARE @ProductId as bigint;

DECLARE @Quantity as decimal(18,2);


declare @temp_ProductDetailTable as table    
(    
 RowId int,    
 ProductId bigint,    
 Quantity decimal(18,2) 
);              
DECLARE @CompanyBranchCode as varchar(10);       
SELECT @CompanyCode=CompanyCode from Company where CompanyId=@CompanyId            
SELECT @CompanyBranchCode=CB.CompanyBranchCode from ComapnyBranch CB 
where CB.CompanyBranchId=@CompanyBranchId             
SELECT @FinYearStartDate=StartDate,@FinYearEndDate=EndDate,@FinYearCode=FinYearCode            
FROM FinancialYear WHERE FinYearId=@FinYearId            
            
IF @MRNDate<@FinYearStartDate OR @MRNDate>@FinYearEndDate             
 BEGIN            
  SET @message='MRN Date must be within selected financial year.';            
  set @status='FAIL';            
  set @RetMRNId=0;            
  RAISERROR(@message,16,1);            
 END            
            
            
IF @MRNId=0  -- INSERT START            
BEGIN            
            
DECLARE @MRNNo AS VARCHAR(50);            
DECLARE @MaxMRNNo as int;            
            
SELECT @MaxMRNNo=MAX(MRNSequence) FROM MRN WHERE COMPANYID=@CompanyId and FinYearId=@FinYearid and CompanyBranchId=@CompanyBranchId           
IF ISNULL(@MaxMRNNo,0)<>0            
BEGIN            
 SET @MaxMRNNo=@MaxMRNNo+1;            
END            
ELSE            
BEGIN            
 SET @MaxMRNNo=1;            
END            
            
set @MRNNo=@CompanyBranchCode + '/MR/' + RIGHT(@FinYearCode,5) + '/' +  FORMAT(@MaxMRNNo,'000#');            
         
INSERT INTO MRN(MRNNo,MRNDate,GRNo,GRDate,QualityCheckId,QualityCheckNo,QualityCheckDate,VendorId,VendorName,        
ContactPerson, ShippingContactPerson,ShippingBillingAddress,ShippingCity,ShippingStateId,ShippingCountryId,        
ShippingPinCode,ShippingTINNo,ShippingEmail,ShippingMobileNo,ShippingContactNo,ShippingFax,  CompanyBranchId,      
DispatchRefNo,DispatchRefDate,LRNO,LRDate,TransportVia,NoOfPackets,Remarks1,Remarks2,        
FinYearId,CompanyId,CreatedBy,CreatedDate,MRNSequence,ApprovalStatus, LocationID)            
VALUES(@MRNNo,@MRNDate,@GRNo,@GRDate,@QualityCheckId,@QualityCheckNo,@QualityCheckDate,@VendorId,@VendorName,        
@ContactPerson,@ShippingContactPerson,@ShippingBillingAddress,@ShippingCity,@ShippingStateId,@ShippingCountryId,        
@ShippingPinCode,@ShippingTINNo,@ShippingEmail,@ShippingMobileNo,@ShippingContactNo,@ShippingFax,  @CompanyBranchId,      
@DispatchRefNo,@DispatchRefDate,@LRNO,@LRDate, @TransportVia,@NoOfPackets,@Remarks1,@Remarks2,@FinYearId,@CompanyId,@CreatedBy,GETDATE(),@MaxMRNNo,@ApprovalStatus,@LocationID)            
            
set @RetMRNId=SCOPE_IDENTITY();             
            
insert into MRNProductDetail(MRNId,ProductId,ProductShortDesc,Price,Quantity,ReceivedQuantity,AcceptQuantity)            
select @RetMRNId,ProductId,ProductShortDesc,Price,Quantity,Quantity,Quantity from @MRNProductDetail 


insert into MRNSupportingDocument(MrnId,DocumentTypeId,DocumentName,DocumentPath)        
select @RetMRNId,DocumentTypeId,DocumentName, DocumentPath  
from @MRNSupportingDocument            
      
--------------------------Update ProductTrnStock table-------------------------------------------------------------

			if(@ApprovalStatus='Final')

			Begin
      
			insert into ProductTrnStock(ProductId,CompanyId,TrnType,TrnDate,TrnQty,TrnReferenceNo,TrnReferenceDate,FromCompanyBranchId,ToCompanyBranchId)          
			select ProductId,@CompanyId,'MRN',@MRNDate,Quantity,@RetMRNId,@MRNDate,0,@CompanyBranchId      
			from @MRNProductDetail   

			end

-----------------------------------End Code----------------------------------------------------------------------------

--------------------------For Update QualityCheckProductDetail Table Column ReceivedQty,AcceptQty, RejectQty Quantity----------------------------------------------------------

               if(@ApprovalStatus='Final')

			   begin
            
				insert into @temp_ProductDetailTable(RowId,ProductId,Quantity)    
				select row_number() over (order by  ProductId), ProductId,ISNULL(Quantity,0) Quantity   
				from @MRNProductDetail				   
    
				set @RowCount=(select Max(RowId) from @temp_ProductDetailTable);    
				set @RowNum=1;    

				while (@RowNum<=@RowCount) 
				   
				begin    
     
						 select @ProductId=ProductId,@Quantity=Quantity						
						 from @temp_ProductDetailTable  
						 where RowId=@RowNum ;    
        
						 UPDATE QualityCheckProductDetail set
						 MrnQTY = ISNULL(MrnQTY,0) + @Quantity						
						 where ProductId=@ProductId and QualityCheckID = @QualityCheckId 
   
				 set @RowNum=@RowNum+1;    
				 end
				 End
---------------------------------------------END CODE---------------------------------------------------------------------
      
SET @message='';            
set @status='SUCCESS';            
END            
ELSE  -- MODIFY START        
BEGIN            
          
 update MRN set MRNDate=@MRNDate,          
 GRNo=@GRNo,        
 GRDate=@GRDate,          
 QualityCheckId=@QualityCheckId,           
 QualityCheckNo=@QualityCheckNo,
 QualityCheckDate=@QualityCheckDate,            
 VendorId=@VendorId,            
 VendorName=@VendorName,         
 ContactPerson=@ContactPerson,         
 ShippingContactPerson=@ShippingContactPerson,        
 ShippingBillingAddress=@ShippingBillingAddress,            
 ShippingCity=@ShippingCity,            
 ShippingStateId=@ShippingStateId,            
 ShippingCountryId=@ShippingCountryId,            
 ShippingPinCode=@ShippingPinCode,            
 ShippingTINNo=@ShippingTINNo,            
 ShippingEmail=@ShippingEmail,        
 ShippingMobileNo=@ShippingMobileNo,        
 ShippingContactNo=@ShippingContactNo,        
 ShippingFax=@ShippingFax,        
 CompanyBranchId=@CompanyBranchId,      
 DispatchRefNo=@DispatchRefNo,            
 DispatchRefDate=@DispatchRefDate,            
 Remarks1=@Remarks1,        
 Remarks2=@Remarks2,        
 ModifiedBy=@CreatedBy,            
 ModifiedDate=getdate(),    
 ApprovalStatus=@ApprovalStatus ,
 LocationID = @LocationID
 where MRNId=@MRNId            
            
 delete from MRNProductDetail where MRNId=@MRNId            
             
 insert into MRNProductDetail(MRNId,ProductId,ProductShortDesc,Price,Quantity,ReceivedQuantity,AcceptQuantity)            
select @MRNId,ProductId,ProductShortDesc,Price,Quantity,Quantity,Quantity from @MRNProductDetail 
 
 
delete from MRNSupportingDocument where MrnId=@MRNId    
insert into MRNSupportingDocument(MrnId,DocumentTypeId,DocumentName,DocumentPath)        
select @MRNId,DocumentTypeId,DocumentName, DocumentPath  
from @MRNSupportingDocument         
        
--------------------------Update ProductTrnStock table-------------------------------------------------------------

			if(@ApprovalStatus='Final')

			Begin
      
			insert into ProductTrnStock(ProductId,CompanyId,TrnType,TrnDate,TrnQty,TrnReferenceNo,TrnReferenceDate,FromCompanyBranchId,ToCompanyBranchId)          
			select ProductId,@CompanyId,'MRN',@MRNDate,Quantity,@MRNId,@MRNDate,0,@CompanyBranchId      
			from @MRNProductDetail   

			end

-----------------------------------End Code----------------------------------------------------------------------------

--------------------------For Update POProductDetail Table Column ReceivedQty,AcceptQty, RejectQty Quantity----------------------------------------------------------

   
               if(@ApprovalStatus='Final')

			   begin
            
				insert into @temp_ProductDetailTable(RowId,ProductId,Quantity)    
				select row_number() over (order by  ProductId), ProductId,ISNULL(Quantity,0) Quantity   
				from @MRNProductDetail				   
    
				set @RowCount=(select Max(RowId) from @temp_ProductDetailTable);    
				set @RowNum=1;    

				while (@RowNum<=@RowCount) 
				   
				begin    
     
						 select @ProductId=ProductId,@Quantity=Quantity						
						 from @temp_ProductDetailTable  
						 where RowId=@RowNum ;    
        
						 UPDATE QualityCheckProductDetail set
						 MrnQTY = ISNULL(MrnQTY,0) + @Quantity						
						 where ProductId=@ProductId and QualityCheckID = @QualityCheckId 
   
				 set @RowNum=@RowNum+1;    
				 end
				 End
---------------------------------------------END CODE---------------------------------------------------------------------
               
             
 SET @message='';            
 set @status='SUCCESS';            
 set @RetMRNId=@MRNId;             
             
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
set @RetMRNId=0;            
END CATCH;            
end 



GO
-- =============================================
-- Author		:		<Author Name>
-- Create date	:		<Created Date>
-- Description	:		<Purpose of the SP>
-- ==============================================
--proc_GetMRNQCDetail 50081     
ALTER proc [dbo].[proc_GetMRNQCDetail]          
(                    
@MRNId int                  
)                    
as   
-- -------------------------------------------------------- History -------------------------------------------------------------------------------- 
--    Date		Story					Developer			Description  
-- ----------- ----------------------- ------------------- ----------------------------------------------------------------------------------------- 
-- 26-May-2022							DHEERAJ KUMAR		Get Location Details
-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
begin                    
set nocount on;                  
select E.MRNId,                
E.MRNNo,     
E.GRNo,    
replace(convert(varchar,E.GRDate,106),' ','-') GRDate,                   
replace(convert(varchar, E.MRNDate,106),' ','-') MRNDate,        
E.VendorId,                 
E.VendorName,     
V.VendorCode,
isnull(V.MobileNo,'') as VendorMobileNo,   
isnull(V.Email,'') as VendorEmail,
isnull(V.Address,'') as VendorAddress,
isnull(V.City,'') as VendorCity,
isnull(V.PinCode,'') as VendorPinCode, 
isnull(VS.StateName,'') as VendorStateName,  
   
ISNULL(E.QualityCheckId,0) QualityCheckId,        
ISNULL(E.QualityCheckNo,'') QualityCheckNo,
ISNULL(replace(convert(varchar, E.QualityCheckDate,106),' ','-'),'') QualityCheckDate,
E.ApprovalStatus,                               
E.ContactPerson,      
E.ShippingContactPerson,      
E.ShippingBillingAddress,                
E.ShippingCity,                
ISNULL(E.ShippingStateId,0) ShippingStateId,            
isnull(shipState.StateName,'') StateName ,      
ISNULL(E.ShippingCountryId,0) ShippingCountryId ,            
isnull(shipCountry.CountryName,'') CountryName ,      
E.ShippingPinCode,               
E.ShippingTINNo,            
E.ShippingEmail,      
E.ShippingMobileNo,      
E.ShippingContactNo,      
E.ShippingFax,      
E.CompanyBranchId,   
comBranch.BranchName as CompanyBranchName,  
comBranch.PrimaryAddress as CompanyBranchAddress,  
ComBranch.City as CompanyBranchCity,  
ComBranch.StateId as CompanyBranchStateId,  
ComBranchState.StateName as CompanyBranchStateName,  
ComBranch.PinCode as CompanyBranchPinCode,  
ComBranch.CSTNo as CompanyBranchCSTNo,  
ComBranch.TINNo as CompanyBranchTINNo,
ComBranch.GSTNo as CompanyBranchGSTNo,  
E.DispatchRefNo,        
case when E.DispatchRefDate is null or E.DispatchRefDate < '1900-12-01' then '' else replace(convert(varchar, E.DispatchRefDate,106),' ','-') end DispatchRefDate,              
E.LRNo,    
case when E.LRDate is null or E.LRDate < '1900-12-01' then '' else replace(convert(varchar, E.LRDate,106),' ','-') end LRDate,              
E.TransportVia,    
E.NoOfPackets,       
E.Remarks1,      
E.Remarks2,        
E.FinYearId,        
E.CompanyId,        
Com.CompanyName, 
isnull(Com.TanNo,'') as CompanyGSTNo,    
com.ContactPersonName,    
com.Phone as CompanyPhone,      
com.Email as CompanyEmail,      
com.Fax as CompanyFax,      
com.Website as CompanyWebsite,      
com.Address as CompanyAddress,      
com.City as CompanyCity,      
CompState.StateName as CompanyStateName,      
CompCountry.CountryName CompanyCountryName ,      
com.ZipCode CompanyZipCode,       
com.CompanyDesc CompanyDesc,       
com.TINNo CompanyTINNo,      
    
E.CreatedBy,                
cu.FullName CreatedByName,                  
replace(convert(varchar, E.CreatedDate,106),' ','-') CreatedDate,                    
E.ModifiedBy,                     
isnull(mu.FullName,'') ModifiedByName,                  
case when E.ModifiedDate is null then '' else replace(convert(varchar, E.ModifiedDate,106),' ','-') end ModifiedDate    ,
ISNULL(E.LocationID, 0) AS LocationID   
from [MRN] E           
inner join Vendor V on E.VendorId=V.VendorId     
inner join [User] cu on E.createdBy=cu.UserId         
inner join Company Com on E.CompanyId=Com.CompanyId      
inner join [State] compState on Com.State=compState.StateId      
inner join [Country] compCountry on Com.CountryId=compCountry.CountryId    
inner join ComapnyBranch ComBranch on E.CompanyBranchId=ComBranch.CompanyBranchId  
inner join [State] ComBranchState on ComBranch.StateId=ComBranchState.StateId  
left join [State] shipState on E.ShippingStateId=shipState.StateId   
left join [State] VS on E.ShippingStateId=VS.StateId    
left join [Country] shipCountry on E.ShippingCountryId=shipCountry.CountryId 
left join [User] mu on E.ModifiedBy=mu.Userid                      
where  E.MRNId=@MRNId    
set nocount off;                    
end                


-- =============================================
-- Author		:		<Author Name>
-- Create date	:		<Created Date>
-- Description	:		<Purpose of the SP>
-- ==============================================
GO
-- proc_GetQCMRNs '','','','','2017-04-01','2018-03-31',1 ,'0'  
ALTER proc [dbo].[proc_GetQCMRNs]    
(                
@MRNNo varchar(50),               
@VendorName varchar(50),               
@DispatchRefNo varchar(50),   
@QCNo varchar(50),            
@FromDate date,             
@ToDate date,            
@CompanyId int ,
@ApprovalStatus varchar(20),
@companyBranch varchar(10),   
@LocationID INT = 0 
)                
as    
-- -------------------------------------------------------- History -------------------------------------------------------------------------------- 
--    Date		Story					Developer			Description  
-- ----------- ----------------------- ------------------- ----------------------------------------------------------------------------------------- 
-- 26-May-2022							DHEERAJ KUMAR		Get Location Details
-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
begin                
set nocount on;                
                
declare @strSql as nvarchar(4000);             
set @strSql='select E.MRNId,            
E.MRNNo,                
replace(convert(varchar, E.MRNDate,106),'' '',''-'') MRNDate,                
E.VendorId,            
E.VendorName,      
V.VendorCode,                 
E.ShippingCity,   
E.ApprovalStatus, 
E.QualityCheckId,    
E.QualityCheckNo,
replace(convert(varchar, E.QualityCheckDate,106),'' '',''-'') QualityCheckDate,    
isnull(St.StateName,'''') StateName,           
E.DispatchRefNo,      
case when E.DispatchRefDate is null or DispatchRefDate < ''1900-12-01'' then '''' else replace(convert(varchar, E.DispatchRefDate,106),'' '',''-'') end DispatchRefDate,            
E.CreatedBy,            
cu.FullName CreatedByName,              
replace(convert(varchar, E.CreatedDate,106),'' '',''-'') CreatedDate,                
E.ModifiedBy,                 
isnull(mu.FullName,'''') ModifiedByName,              
case when E.ModifiedDate is null then '''' else replace(convert(varchar, E.ModifiedDate,106),'' '',''-'') end ModifiedDate,
isnull(cb.BranchName,'''') as BranchName ,
ISNULL(L.LocationName , '''') AS LocationName
from [MRN] E     
inner join [Vendor] V on E.VendorId=V.VendorId         
inner join [User] cu on E.createdBy=cu.UserId              
left join [State] St on E.ShippingStateId=St.StateId 
left join [User] mu on E.ModifiedBy=mu.Userid    
left join ComapnyBranch CB on CB.CompanyBranchId=E.CompanyBranchId  
LEFT JOIN Location AS L ON E.LocationID = L.LocationID
where  E.CompanyId='+ cast(@CompanyId as varchar) + ' and  E.QualityCheckId is not null and E.QualityCheckNo is not null ';                
set @strSql=@strSql + ' and  E.MRNDate BETWEEN '''+ cast(@FromDate as varchar) + ''' AND  '''+ cast(@ToDate as varchar) + ''' ';     

if(@companyBranch<>'0')  
begin
set @strSql=@strSql + ' and isnull(E.CompanyBranchId,0) = ' + @companyBranch +'';  
end           
      
if @VendorName<>''                
begin                
set @strSql=@strSql + ' and  E.VendorName like ''%' + @VendorName + '%'' ';                
end                
if @QCNo<>''                
begin                
set @strSql=@strSql + ' and  E.QualityCheckNo like ''%' + @QCNo + '%'' ';                
end        
if @MRNNo<>''                
begin                
set @strSql=@strSql + ' and E.MRNNo like ''%' + @MRNNo + '%''';                
end        
if @DispatchRefNo<>''                
begin                
set @strSql=@strSql + ' and E.DispatchRefNo like ''%' + @DispatchRefNo + '%''';                
end   

if @ApprovalStatus<>'0'                  
begin                    
set @strSql=@strSql + ' and E.ApprovalStatus =''' + @ApprovalStatus + '''';                    
end  

if @LocationID <> 0                  
begin                    
set @strSql=@strSql + ' and L.LocationID =''' + CAST(@LocationID AS VARCHAR) + '''';                   
end  

set @strSql=@strSql + ' order by E.MRNNo desc ';                
--print @strsql
exec sp_executesql @strSql 
print @strSql               
               
set nocount off;                
end 

GO

ALTER TABLE ComplaintServiceProductDetail ADD Quantity DECIMAL(18, 2)

GO

ALTER TABLE ComplaintService ADD InvoiceDate DATE


GO

DROP PROC proc_AddEditComplaintService

DROP TYPE [dbo].[udt_ComplaintServiceProductDetail]
GO

CREATE TYPE [dbo].[udt_ComplaintServiceProductDetail] AS TABLE(
	[ComplaintProductDetailID] [bigint] NOT NULL,
	[ComplaintId] [bigint] NULL,
	[ProductId] [bigint] NULL,
	[Remarks] [varchar](500) NULL,
	[Quantity] DECIMAL(18,2) NULL
)
GO

GO
-- =============================================
-- Author		:		<Author Name>
-- Create date	:		<Created Date>
-- Description	:		<Purpose of the SP>
-- ==============================================
CREATE proc [dbo].[proc_AddEditComplaintService]        
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
CustomerAddress,Status,BranchID,ComplaintSequence, EmployeeID, DealerID,InvoiceDate)        
VALUES(@ComplaintNo,@ComplaintDate,@InvoiceNo,@EnquiryType,@ComplaintMode,@ComplaintDescription,@CustomerName,@CustomerMobile,@CustomerEmail,
@CustomerAddress,@ActiveStatus,@CompanyBranchId,@MaxComplaintNo, @EmployeeID, @DealerID, @InvoiceDate)        

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
InvoiceDate = @InvoiceDate
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







