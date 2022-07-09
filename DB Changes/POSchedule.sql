Create Table POProductSchedule (
PoProductScheduleId INT IDENTITY(1,1) PRIMARY KEY,
POId bigint,
CompanyBranchId int,
LocationName varchar(50),
ProductId bigint ,
ProductName varchar(100),
ProductCode varchar(50),
UOMName varchar(50),
Quantity decimal(18, 2),
DeliveryDate Date,
SchQuantity decimal(18, 2),
ConDeliveryDate date
);

--------------------------------------------------------------------------------------------------------------------------------------------
CREATE TYPE [dbo].[udt_POProductSchedule] AS TABLE (
PoProductScheduleId INT,
POId bigint,
CompanyBranchId int,
LocationName varchar(50),
ProductId bigint ,
ProductName varchar(100),
ProductCode varchar(50),
UOMName varchar(50),
Quantity decimal(18, 2),
DeliveryDate Date,
SchQuantity decimal(18, 2),
ConDeliveryDate date
);

GO
-----------------------------------------------------------------------------------------------------------------------------------------------------------
GO
/****** Object:  StoredProcedure [dbo].[proc_AddEditPO]    Script Date: 7/9/2022 3:44:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
      
ALTER proc [dbo].[proc_AddEditPO]      
(      
@POId bigint,      
@PODate date,
@IndentId int=0,   
@IndentNo varchar(50)='',
@QuotationId int =0,
@QuotationNo varchar(50)='',   
@CurrencyCode varchar(50)='INR',       
@VendorId int,      
@VendorName varchar(100)='',      
@BillingAddress varchar(100)='',      
@ShippingAddress varchar(150)='',      
@City varchar(50)='',      
@StateId int=0,      
@CountryId int=0,  
@CompanyBranchId int=0,    
@PinCode varchar(20)='',      
@CSTNo varchar(20)='',      
@TINNo varchar(20)='',      
@PANNo varchar(20)='',      
@GSTNo varchar(20)='',      
@ExciseNo varchar(20)='',     
@ApprovalStatus varchar(20),     
@RefNo varchar(50)='',      
@RefDate date,      
@BasicValue decimal(18,2),      
@TotalValue decimal(18,2),  

@POType varchar(20), 
@CurrencyConversionRate decimal(18,2),

@ConsigneeId int,                
@ConsigneeName varchar(100)='',                                
@ShippingCity varchar(50)='',                  
@ShippingStateId int,                  
@ShippingCountryId int,                  
@ShippingPinCode varchar(20)='',                  
@ConsigneeGSTNo varchar(20)='', 
                 
@FreightValue decimal(18,2),                  
@FreightCGST_Perc decimal(18,2)=0,                  
@FreightCGST_Amt decimal(18,2)=0,                  
@FreightSGST_Perc decimal(18,2)=0,                  
@FreightSGST_Amt decimal(18,2)=0,                  
@FreightIGST_Perc decimal(18,2)=0,                  
@FreightIGST_Amt decimal(18,2)=0,                  
  
@LoadingValue decimal(18,2),                  
@LoadingCGST_Perc decimal(18,2)=0,                  
@LoadingCGST_Amt decimal(18,2)=0,                  
@LoadingSGST_Perc decimal(18,2)=0,                  
@LoadingSGST_Amt decimal(18,2)=0,                  
@LoadingIGST_Perc decimal(18,2)=0,                  
@LoadingIGST_Amt decimal(18,2)=0,                  
@InsuranceValue decimal(18,2),                  
@InsuranceCGST_Perc decimal(18,2)=0,                  
@InsuranceCGST_Amt decimal(18,2)=0,                  
@InsuranceSGST_Perc decimal(18,2)=0,                  
@InsuranceSGST_Amt decimal(18,2)=0,                  
@InsuranceIGST_Perc decimal(18,2)=0,                  
@InsuranceIGST_Amt decimal(18,2)=0,   
@ReverseChargeApplicable bit=0,  
@ReverseChargeAmount decimal(18,2) =0, 
@ExpectedDeliveryDate date ,     
@Remarks1 varchar(2000)='',      
@Remarks2 varchar(2000)='',      
@FinYearId int,      
@CompanyId int,      
@CreatedBy int,      
@POProductDetail udt_POProductDetail readonly,
@POProductSchedule udt_POProductSchedule readonly,
@POTaxDetail udt_POTaxDetail readonly,        
@POTermDetail udt_POTermDetail readonly,        
@POSupportingDocument udt_POSupportingDocument readonly,        
@status varchar(50) out,      
@message varchar(500) out,      
@RetPOId bigint out      
)      
as      
begin      
BEGIN TRY      
BEGIN TRANSACTION      
      
DECLARE @FinYearStartDate as date;      
DECLARE @FinYearEndDate as date;      
DECLARE @FinYearCode AS VARCHAR(20);      
DECLARE @CompanyCode as varchar(10); 
DECLARE @CompanyBranchCode as varchar(10);      
      
SELECT @CompanyCode=CompanyCode from Company where CompanyId=@CompanyId      
SELECT @CompanyBranchCode=CB.CompanyBranchCode from ComapnyBranch CB where CB.CompanyBranchId=@CompanyBranchId 
      
SELECT @FinYearStartDate=StartDate,@FinYearEndDate=EndDate,@FinYearCode=FinYearCode      
FROM FinancialYear WHERE FinYearId=@FinYearId      
      
IF @PODate<@FinYearStartDate OR @PODate>@FinYearEndDate       
 BEGIN      
  SET @message='Purchase Order (PO) Date must be within selected financial year.';      
  set @status='FAIL';      
  set @RetPOId=0;      
  RAISERROR(@message,16,1);      
 END      
      
      
IF @POId=0  -- INSERT START      
BEGIN      
      
DECLARE @PONo AS VARCHAR(50);      
DECLARE @MaxPONo as int;      
      
SELECT @MaxPONo=MAX(POSequence) FROM PO WHERE COMPANYID=@CompanyId and FinYearId=@FinYearid  and CompanyBranchId=@CompanyBranchId    
IF ISNULL(@MaxPONo,0)<>0      
BEGIN      
 SET @MaxPONo=@MaxPONo+1;      
END      
ELSE      
BEGIN      
 SET @MaxPONo=1;      
END      
      
set @PONo=@CompanyBranchCode + '/PO/' + RIGHT(@FinYearCode,5) + '/' +  FORMAT(@MaxPONo,'000#');      
      
INSERT INTO PO(PONo,PODate,IndentId,IndentNo,QuotationId,QuotationNo,CurrencyCode,VendorId,VendorName,BillingAddress,ShippingAddress,City,      
StateId,CountryId,PinCode,CSTNo,TINNo,PANNo,GSTNo,ExciseNo,POStatus,RefNo,RefDate,BasicValue,TotalValue,
ConsigneeId,ConsigneeName,ShippingCity,ShippingStateId,ShippingCountryId,ShippingPinCode,ConsigneeGSTNo,
FreightValue,FreightCGST_Perc,FreightCGST_Amt,  
FreightSGST_Perc,FreightSGST_Amt,FreightIGST_Perc,FreightIGST_Amt,  
LoadingValue,LoadingCGST_Perc,LoadingCGST_Amt,  
LoadingSGST_Perc,LoadingSGST_Amt,LoadingIGST_Perc,LoadingIGST_Amt,  
InsuranceValue,InsuranceCGST_Perc,InsuranceCGST_Amt,  
InsuranceSGST_Perc,InsuranceSGST_Amt,InsuranceIGST_Perc,InsuranceIGST_Amt 
,ExpectedDeliveryDate,Remarks1,Remarks2,FinYearId,CompanyBranchId,CompanyId,CreatedBy,CreatedDate,POSequence,ReverseChargeApplicable,
ReverseChargeAmount,POType,CurrencyConversionRate)      
VALUES(@PONo,@PODate,@IndentId,@IndentNo,@QuotationId,@QuotationNo,@CurrencyCode,@VendorId,@VendorName,@BillingAddress,@ShippingAddress,      
@City,@StateId,@CountryId,@PinCode,@CSTNo,@TINNo,@PANNo,@GSTNo,@ExciseNo,@ApprovalStatus,@RefNo,@RefDate,      
@BasicValue,@TotalValue,@ConsigneeId ,@ConsigneeName,@ShippingCity,@ShippingStateId,@ShippingCountryId ,@ShippingPinCode  , @ConsigneeGSTNo ,
@FreightValue,@FreightCGST_Perc,@FreightCGST_Amt,  
@FreightSGST_Perc,@FreightSGST_Amt,@FreightIGST_Perc,@FreightIGST_Amt,  
@LoadingValue,@LoadingCGST_Perc,@LoadingCGST_Amt,  
@LoadingSGST_Perc,@LoadingSGST_Amt,@LoadingIGST_Perc,@LoadingIGST_Amt,  
@InsuranceValue,@InsuranceCGST_Perc,@InsuranceCGST_Amt,  
@InsuranceSGST_Perc,@InsuranceSGST_Amt,@InsuranceIGST_Perc,@InsuranceIGST_Amt ,@ExpectedDeliveryDate,
@Remarks1,@Remarks2,@FinYearId,@CompanyBranchId,@CompanyId,@CreatedBy,GETDATE(),@MaxPONo,@ReverseChargeApplicable,@ReverseChargeAmount,
@POType,@CurrencyConversionRate)      
      
set @RetPOId=SCOPE_IDENTITY();       
      
insert into POProductDetail(POId,ProductId,ProductShortDesc,Price,Quantity,DiscountPercentage,      
DiscountAmount,TaxId,TaxName,TaxPercentage,TaxAmount,TotalPrice,  
SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,  
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,  
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3 ,
CGST_Perc,CGST_Amount,SGST_Perc,      
SGST_Amount,IGST_Perc,IGST_Amount,HSN_Code,ExpectedDeliveryDate)      
select @RetPOId,ProductId,ProductShortDesc,Price,Quantity,DiscountPercentage,      
DiscountAmount,TaxId,TaxName,TaxPercentage,TaxAmount,TotalPrice,  
SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,  
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,  
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3 ,
CGST_Perc,CGST_Amount,SGST_Perc,      
SGST_Amount,IGST_Perc,IGST_Amount,HSN_Code,ExpectedDeliveryDate       
from @POProductDetail

insert into POProductSchedule(POId,CompanyBranchId,LocationName,ProductId,ProductName,ProductCode,UOMName,Quantity,      
DeliveryDate,SchQuantity,ConDeliveryDate)
select @RetPOId,CompanyBranchId,LocationName,ProductId,ProductName,ProductCode,UOMName,Quantity, 
DeliveryDate,SchQuantity,ConDeliveryDate       
from @POProductSchedule
      
insert into POTaxDetail(POId,TaxId,TaxName,TaxPercentage,TaxAmount,SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,  
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,  
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3 )        
select @RetPOId,TaxId,TaxName,TaxPercentage,TaxAmount,  
SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,  
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,  
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3       
from @POTaxDetail       
      
insert into POTermsDetail(POId,TermDesc,TermSequence)        
select @RetPOId,TermDesc,TermSequence      
from @POTermDetail     
  
insert into POSupportingDocument(POId,DocumentTypeId,DocumentName,DocumentPath)        
select @RetPOId,DocumentTypeId,DocumentName,    DocumentPath  
from @POSupportingDocument       
      
       
      
SET @message='';      
set @status='SUCCESS';      
      
       
END      
ELSE  -- MODIFY START      
BEGIN      
 update PO set   
 CurrencyCode=@CurrencyCode,  
 PODate=@PODate,
 IndentId=@IndentId,
 IndentNo=@IndentNo,
 QuotationId=@QuotationId,
 QuotationNo=@QuotationNo,      
 VendorId=@VendorId,      
 VendorName=@VendorName,      
 BillingAddress=@BillingAddress,      
 ShippingAddress=@ShippingAddress,   
 ConsigneeId=@ConsigneeId,                
 ConsigneeName=@ConsigneeName,                                  
 ShippingCity =@ShippingCity ,               
 ShippingStateId =@ShippingStateId,                  
 ShippingCountryId=@ShippingCountryId,                
 ShippingPinCode=@ShippingPinCode ,             
 ConsigneeGSTNo=@ConsigneeGSTNo ,   
 City=@City,      
 StateId=@StateId,      
 CountryId=@CountryId,      
 PinCode=@PinCode,      
 CSTNo=@CSTNo,      
 TINNo=@TINNo,      
 PANNo=@PANNo,      
 GSTNo=@GSTNo,      
 ExciseNo=@ExciseNo,    
 POStatus=@ApprovalStatus,      
 RefNo=@RefNo,      
 RefDate=@RefDate,      
 BasicValue=@BasicValue,      
 TotalValue=@TotalValue,      
 FreightValue=@FreightValue,              
 FreightCGST_Perc=@FreightCGST_Perc,              
 FreightCGST_Amt=@FreightCGST_Amt,              
 FreightSGST_Perc=@FreightSGST_Perc,              
 FreightSGST_Amt=@FreightSGST_Amt,              
 FreightIGST_Perc=@FreightIGST_Perc,              
 FreightIGST_Amt=@FreightIGST_Amt,              
 LoadingValue=@LoadingValue,              
 LoadingCGST_Perc=@LoadingCGST_Perc,              
 LoadingCGST_Amt=@LoadingCGST_Amt,              
 LoadingSGST_Perc=@LoadingSGST_Perc,              
 LoadingSGST_Amt=@LoadingSGST_Amt,              
 LoadingIGST_Perc=@LoadingIGST_Perc,              
 LoadingIGST_Amt=@LoadingIGST_Amt,              
 InsuranceValue=@InsuranceValue,              
 InsuranceCGST_Perc=@InsuranceCGST_Perc,              
 InsuranceCGST_Amt=@InsuranceCGST_Amt,              
 InsuranceSGST_Perc=@InsuranceSGST_Perc,              
 InsuranceSGST_Amt=@InsuranceSGST_Amt,              
 InsuranceIGST_Perc=@InsuranceIGST_Perc,              
 InsuranceIGST_Amt=@InsuranceIGST_Amt, 
 ExpectedDeliveryDate=@ExpectedDeliveryDate,
 CompanyBranchId=@CompanyBranchId,   
 Remarks1=@Remarks1,      
 Remarks2=@Remarks2,      
 ModifiedBy=@CreatedBy,      
 ModifiedDate=getdate() ,
 ReverseChargeApplicable=@ReverseChargeApplicable,  
 ReverseChargeAmount=@ReverseChargeAmount,
 POType=@POType,
 CurrencyConversionRate=@CurrencyConversionRate      
 where POId=@POId      
      
 delete from POProductDetail where POId=@POId      
       
 insert into POProductDetail(POId,ProductId,ProductShortDesc,Price,Quantity,DiscountPercentage,      
 DiscountAmount,TaxId,TaxName,TaxPercentage,TaxAmount,TotalPrice,SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,  
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,  
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3 ,
CGST_Perc,CGST_Amount,SGST_Perc,      
SGST_Amount,IGST_Perc,IGST_Amount,HSN_Code,ExpectedDeliveryDate)      
 select @POId,ProductId,ProductShortDesc,Price,Quantity,DiscountPercentage,      
 DiscountAmount,TaxId,TaxName,TaxPercentage,TaxAmount,TotalPrice,  
 SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,  
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,  
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3 ,
CGST_Perc,CGST_Amount,SGST_Perc,      
SGST_Amount,IGST_Perc,IGST_Amount,HSN_Code,ExpectedDeliveryDate      
 from @POProductDetail 
 
 delete from POProductSchedule where POId=@POId
 insert into POProductSchedule(POId,CompanyBranchId,LocationName,ProductId,ProductName,ProductCode,UOMName,Quantity,      
DeliveryDate,SchQuantity,ConDeliveryDate)
select @POId,CompanyBranchId,LocationName,ProductId,ProductName,ProductCode,UOMName,Quantity, 
DeliveryDate,SchQuantity,ConDeliveryDate       
from @POProductSchedule

      
 delete from POTaxDetail where POId=@POId        
 insert into POTaxDetail(POId,TaxId,TaxName,TaxPercentage,TaxAmount,SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,  
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,  
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3 )        
 select @POId,TaxId,TaxName,TaxPercentage,TaxAmount,SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,  
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,  
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3       
 from @POTaxDetail       
        
 delete from POTermsDetail where POId=@POId         
 insert into POTermsDetail(POId,TermDesc,TermSequence)        
 select @POId,TermDesc,TermSequence      
 from @POTermDetail       
  
 delete from POSupportingDocument where POId=@POId    
insert into POSupportingDocument(POId,DocumentTypeId,DocumentName,DocumentPath)        
select @POId,DocumentTypeId,DocumentName,    DocumentPath  
from @POSupportingDocument  
  
      
 SET @message='';      
 set @status='SUCCESS';      
 set @RetPOId=@POId;       
       
END      
  ----Added Start Section for Purchase Indent to Update the PurchaseIndentProductTable
declare @temp_ProductEntry as table    
(    
 RowId int,    
 ProductId bigint,    
 Quantity decimal(18,2)
); 
declare @RowCount int=0;    
declare @RowNum int;          
declare @IndentProductId bigint; 
Declare @PurchaseOrderId as Int;
Declare @PurchaseIndentQuantity as int;
Declare @POStatus varchar(50);

 if @IndentId<>0 and @IndentId Is Not NULL
   Begin
    select @POStatus= POStatus from PO where IndentId=@IndentId and POId=(Case when @POId<>0 then @POId Else @RetPOId End)
     if @POStatus='Draft' or @POStatus='Final'
      Begin

   insert into @temp_ProductEntry(RowId,ProductId ,Quantity)    
select row_number() over (order by  ProductId), ProductId,Sum(Quantity)
from @POProductDetail
group by ProductId 
    
set @RowCount=(select Max(RowId) from @temp_ProductEntry);    
set @RowNum=1;    

while (@RowNum<=@RowCount)    
begin      
     
 select @IndentProductId=ProductId ,@PurchaseIndentQuantity=Quantity     
 from @temp_ProductEntry  
 where RowId=@RowNum ;    
     
if @PurchaseIndentQuantity>0  
begin     
 
 UPDATE PurchaseIndentProductDetail
 Set POQuantity=isnull(POQuantity,0) + isnull(@PurchaseIndentQuantity,0)
 where ProductId=@IndentProductId and IndentId=@IndentId
 end     
 set @RowNum=@RowNum+1;    
 end     
End
    End
----End Section------------------------------------         
COMMIT TRANSACTION      
END TRY      
BEGIN CATCH      
IF @@TRANCOUNT > 0      
BEGIN      
 ROLLBACK TRANSACTION;      
END      
set @status ='FAIL';      
set @message = ERROR_MESSAGE();      
set @RetPOId=0;      
END CATCH;      
end 

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------


GO
/****** Object:  StoredProcedure [dbo].[proc_GetPOSchedules]    Script Date: 7/9/2022 10:21:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[proc_GetPOSchedules] --  proc_GetPOSchedules 31   
(              
@POId bigint    
)              
as              
begin              
set nocount on;              
select 
ROW_NUMBER() OVER(ORDER BY PoProductScheduleId ASC) AS SequenceNo,
PoProductScheduleId,
CompanyBranchId,LocationName,ProductId,ProductName,ProductCode,UOMName,Quantity,
replace(convert(varchar, DeliveryDate,106), '','-') AS DeliveryDate,
SchQuantity,
replace(convert(varchar, ConDeliveryDate,106), '','-') AS ConDeliveryDate
from POProductSchedule        
where  POId=@POId 
set nocount off;              
end 


---------------------------------------------------------------------------------------------------------------------------------------------------------------------------


GO
/****** Object:  StoredProcedure [dbo].[proc_GetComplaintServiceDetail]    Script Date: 7/9/2022 1:03:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO-- EXECUTE [dbo].[proc_GetComplaintServiceDetail] 7ALTER  proc [dbo].[proc_GetComplaintServiceDetail]            (                      @ComplaintId int                    )                      as -- -------------------------------------------------------- History -------------------------------------------------------------------------------- --    Date			Story					Developer			Description  -- ----------- ----------------------- ------------------- ------------------------------------------------------------------------------------------- 14-May-2022								Dheeraj Kumar		Get EmployeeId Column Value.-- 21-May-2022								Dheeraj Kumar		Insert DealerID Column Value.-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////begin                      set nocount on;                    select E.ComplaintId,                E.ComplaintNo,replace(convert(varchar, E.ComplaintDate,106),'','-') ComplaintDate,E.InvoiceNo,                    E.EnquiryType,    E.BranchId,E.ComplaintMode,E.ComplaintDescription,E.CustomerName,E.CustomerMobile,E.CustomerEmail,E.CustomerAddress,E.Status, E.EmployeeID,ISNULL(E.ComplaintStatus,0) AS ComplaintStatus,ISNULL(E.DealerID, 0 ) AS DealerID,CASE WHEN E.InvoiceDate IS NULL OR E.InvoiceDate < '1900-12-01' THEN '' ELSE REPLACE(CONVERT(VARCHAR, E.InvoiceDate,106),' ','-') END InvoiceDate      from [ComplaintService] E   inner join ComapnyBranch  cb on E.BranchId=cb.CompanyBranchId where E.ComplaintId=@ComplaintIdset nocount off;                      end 

--------------------------------------------------------------------------------------------------------------------------------------------------

