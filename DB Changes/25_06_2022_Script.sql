if exists (select * from sys.objects where type='p' and name ='proc_AddEditMRNQC')
begin
		DROP PROC proc_AddEditMRNQC
end
GO
-- =============================================
-- Author		:		<Author Name>
-- Create date	:		<Created Date>
-- Description	:		<Purpose of the SP>
-- ==============================================
cREATE proc [dbo].[proc_AddEditMRNQC]            
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
 
 DECLARE @InvoiceId AS BIGINT;
DECLARE @InvoiceNo VARCHAR(50);
DECLARE @POId AS BIGINT;
DECLARE @PONo VARCHAR(50);
SELECT @POId=QC.POID, @PONo=ISNULL(PO.PONo,''),
@InvoiceId=ISNULL(PI.InvoiceId,0) , @InvoiceNo=ISNULL(PI.InvoiceNo,'')
FROM QualityCheck QC 
INNER JOIN PO PO  ON QC.POID=PO.POId
LEFT JOIN PurchaseInvoice PI ON QC.POID=PI.POId
WHERE QC.QualityCheckId=@QualityCheckId
--SELECT @POId AS POID, @PONo AS PONo,@InvoiceId AS InvoiceId , @InvoiceNo AS InvoiceNo
  --SELECT * FROM   QualityCheck

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
         
INSERT INTO MRN(MRNNo,MRNDate,GRNo,GRDate,InvoiceId,InvoiceNo,POId,PONo,QualityCheckId,QualityCheckNo,QualityCheckDate,VendorId,VendorName,        
ContactPerson, ShippingContactPerson,ShippingBillingAddress,ShippingCity,ShippingStateId,ShippingCountryId,        
ShippingPinCode,ShippingTINNo,ShippingEmail,ShippingMobileNo,ShippingContactNo,ShippingFax,  CompanyBranchId,      
DispatchRefNo,DispatchRefDate,LRNO,LRDate,TransportVia,NoOfPackets,Remarks1,Remarks2,        
FinYearId,CompanyId,CreatedBy,CreatedDate,MRNSequence,ApprovalStatus, LocationID)            
VALUES(@MRNNo,@MRNDate,@GRNo,@GRDate,@InvoiceId,@InvoiceNo,@POId,@PONo, @QualityCheckId,@QualityCheckNo,@QualityCheckDate,@VendorId,@VendorName,        
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
InvoiceId	= @InvoiceId,
InvoiceNo	= @InvoiceNo,
POId		= @POId,
PONo		= @PONo,
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

if exists (select * from sys.objects where type='p' and name ='proc_GetPIs')
begin
		DROP PROC proc_GetPIs
end

GO

--  proc_GetPIs '','','','2018-01-01','2019-04-30',1,'Final','Popup','','0','','',''   
    
cREATE proc [dbo].[proc_GetPIs]  
(              
@InvoiceNo varchar(50),             
@VendorName varchar(50),             
@RefNo varchar(50),           
@FromDate date,           
@ToDate date,          
@CompanyId int,  
@ApprovalStatus varchar(20)='',    
@DisplayType varchar(50)='',
@VendorCode varchar(50)='',
@PurchaseType varchar(50)='',
@CreatedBy varchar(10),
@poNo varchar(25),
@companyBranch varchar(10),
@MRNNo varchar(50)
)              
as              
begin              
set nocount on;              
              
declare @strSql as nvarchar(4000);           
set @strSql='select PI.InvoiceId,          
PI.InvoiceNo, 
PI.PONo,                 
replace(convert(varchar, PI.InvoiceDate,106),'' '',''-'') InvoiceDate,              
PI.VendorName,           
PI.City,      
PI.VendorId,    
V.VendorCode,      
St.StateName,         
PI.RefNo,  
PI.ApprovalStatus,    
ISNULL(PI.InvoiceRevisedStatus,0)  InvoiceRevisedStatus,     
case when PI.RefDate is null or RefDate < ''1900-12-01'' then '''' else replace(convert(varchar, PI.RefDate,106),'' '',''-'') end RefDate,          
PI.BasicValue,    
PI.TotalValue,
isnull(PI.PurchaseType,'''') as PurchaseType,    
PI.CreatedBy,          
cu.FullName CreatedByName,            
replace(convert(varchar, PI.CreatedDate,106),'' '',''-'') CreatedDate,              
PI.ModifiedBy,               
isnull(mu.FullName,'''') ModifiedByName,            
case when PI.ModifiedDate is null then '''' else replace(convert(varchar, PI.ModifiedDate,106),'' '',''-'') end ModifiedDate,
isnull(cb.BranchName,'''') as BranchName  ,
ISNULL(MR.MRNNo,'''') AS MRNNo,
case when MR.MRNDate is null then '''' else replace(convert(varchar, MR.MRNDate,106),'' '',''-'') end MRNDate,
isnull(MR.MRNId,0) as MRNId
from [PurchaseInvoice] PI       
left join [Vendor] V on PI.VendorId=V.VendorId     
inner join [State] St on PI.StateId=St.StateId         
inner join [User] cu on PI.createdBy=cu.UserId            
left join [User] mu on PI.ModifiedBy=mu.Userid  
left join ComapnyBranch CB on CB.CompanyBranchId=PI.CompanyBranchId    
LEFT JOIN MRN MR ON ISNULL(MR.POId,0)=ISNULL(PI.POId,0)
where  PI.CompanyId='+ cast(@CompanyId as varchar) + ' ';              
set @strSql=@strSql + ' and  PI.InvoiceDate BETWEEN '''+ cast(@FromDate as varchar) + ''' AND  '''+ cast(@ToDate as varchar) + ''' '; 

if(@companyBranch='')
set @companyBranch='0'

if(@companyBranch<>'0')  
begin
set @strSql=@strSql + ' and isnull(PI.CompanyBranchId,0) = ' + @companyBranch +'';  
end               
    
if @VendorName<>''              
begin              
set @strSql=@strSql + ' and PI.VendorName like ''%' + @VendorName + '%'' ';              
end              
if @VendorCode<>''              
begin              
set @strSql=@strSql + ' and V.VendorCode=''' + @VendorCode + ''' ';              
end
if @CreatedBy<>''                
begin                
set @strSql=@strSql + ' and cu.FullName like ''%' + @CreatedBy + '%''';                
end                  
if @InvoiceNo<>''              
begin              
set @strSql=@strSql + ' and PI.InvoiceNo like ''%' + @InvoiceNo + '%''';              
end           
   if @poNo<>''              
begin              
set @strSql=@strSql + ' and PI.PONo like ''%' + @poNo + '%''';              
end   
if @RefNo<>''              
begin              
set @strSql=@strSql + ' and PI.RefNo like ''%' + @RefNo + '%''';              
end 
begin              
set @strSql=@strSql + ' and isnull( MR.MRNNo,'''') like ''%' + @MRNNo + '%''';              
end   

if @DisplayType<>''    
begin                    
 set @strSql=@strSql + ' and PI.InvoiceId not in (Select isnull(InvoiceId,0) as InvoiceId from MRN where ApprovalStatus <> ''Cancelled'' )  ';                    
end             
    
 if @ApprovalStatus<>'0'                  
begin                    
set @strSql=@strSql + ' and PI.ApprovalStatus =''' + @ApprovalStatus + '''';                    
end 

 if @PurchaseType<>'0'                  
begin                    
set @strSql=@strSql + ' and PI.PurchaseType =''' + @PurchaseType + '''';    
                
end 
set @strSql=@strSql+' Order by PI.CreatedDate desc ';

--set @strSql=@strSql + ' and PI.InvoiceId in (Select mrn.InvoiceId from MRN mrn  where mrn.ApprovalStatus=''Cancelled'') ';
--set @strSql=@strSql + ' order by PI.InvoiceNo Desc ';              
exec sp_executesql @strSql   
--select @strSql         
              
set nocount off;              
end    

