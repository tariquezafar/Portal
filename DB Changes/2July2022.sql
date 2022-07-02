
GO

/****** Object:  UserDefinedTableType [dbo].[udt_POProductDetail]    Script Date: 7/2/2022 8:14:15 PM ******/
CREATE TYPE [dbo].[udt_POProductDetail] AS TABLE(
	[POProductDetailId] [bigint] NOT NULL,
	[POId] [bigint] NULL,
	[ProductId] [bigint] NULL,
	[ProductShortDesc] [varchar](250) NULL,
	[Price] [decimal](18, 2) NULL,
	[Quantity] [decimal](18, 2) NULL,
	[DiscountPercentage] [decimal](18, 2) NULL,
	[DiscountAmount] [decimal](18, 2) NULL,
	[TaxId] [bigint] NULL,
	[TaxName] [varchar](250) NULL,
	[TaxPercentage] [decimal](18, 2) NULL,
	[TaxAmount] [decimal](18, 2) NULL,
	[SurchargeName_1] [varchar](50) NULL,
	[SurchargePercentage_1] [decimal](18, 2) NULL,
	[SurchargeAmount_1] [decimal](18, 2) NULL,
	[SurchargeName_2] [varchar](50) NULL,
	[SurchargePercentage_2] [decimal](18, 2) NULL,
	[SurchargeAmount_2] [decimal](18, 2) NULL,
	[SurchargeName_3] [varchar](50) NULL,
	[SurchargePercentage_3] [decimal](18, 2) NULL,
	[SurchargeAmount_3] [decimal](18, 2) NULL,
	[CGST_Perc] [decimal](18, 2) NULL,
	[CGST_Amount] [decimal](18, 2) NULL,
	[SGST_Perc] [decimal](18, 2) NULL,
	[SGST_Amount] [decimal](18, 2) NULL,
	[IGST_Perc] [decimal](18, 2) NULL,
	[IGST_Amount] [decimal](18, 2) NULL,
	[HSN_Code] [varchar](50) NULL,
	[TotalPrice] [decimal](18, 2) NULL,
	[ExpectedDeliveryDate] [date]
)
GO

-------------------------------------------------------------------------------------------------------------------------------------------------------------

GO
/****** Object:  StoredProcedure [dbo].[proc_GetPOProducts]    Script Date: 7/2/2022 8:01:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER proc [dbo].[proc_GetPOProducts] --  proc_GetPOProducts 30136   
(              
@POId bigint    
)              
as              
begin              
set nocount on;              
select 
ROW_NUMBER() OVER(ORDER BY P.ProductName ASC) AS SNo,
PO.POProductDetailId,          
PO.ProductId,         
P.ProductName,          
P.ProductCode,          
PO.ProductShortDesc,
isnull(U.UOMName,'') UOMName,
PO.Price,
PO.Quantity,
ISNULL(PO.DiscountPercentage,0)   DiscountPercentage,
ISNULL(PO.DiscountAmount,0)   DiscountAmount,
ISNULL(PO.TaxId,0) TaxId,
ISNULL(PO.TaxName,'') TaxName,
ISNULL(PO.TaxPercentage,0)   TaxPercentage,
ISNULL(PO.TaxAmount,0)   TaxAmount,
PO.TotalPrice,
ISNULL(PO.SurchargeName_1,'') SurchargeName_1,
ISNULL(PO.SurchargePercentage_1,0) SurchargePercentage_1,
ISNULL(PO.SurchargeAmount_1,0) SurchargeAmount_1,

ISNULL(PO.SurchargeName_2,'') SurchargeName_2,
ISNULL(PO.SurchargePercentage_2,0) SurchargePercentage_2,
ISNULL(PO.SurchargeAmount_2,0) SurchargeAmount_2,

ISNULL(PO.SurchargeName_3,'') SurchargeName_3,
ISNULL(PO.SurchargePercentage_3,0) SurchargePercentage_3,
ISNULL(PO.SurchargeAmount_3,0) SurchargeAmount_3,
ISNULL(PO.CGST_Perc,0) CGST_Perc,      
ISNULL(PO.CGST_Amount,0) CGST_Amount,      
ISNULL(PO.SGST_Perc,0) SGST_Perc,      
ISNULL(PO.SGST_Amount,0) SGST_Amount,      
ISNULL(PO.IGST_Perc,0) IGST_Perc,      
ISNULL(PO.IGST_Amount,0) IGST_Amount,  
isnull(p.BrandName,'') as BrandName  ,
isnull(PO.HSN_Code,'') as HSN_Code ,
replace(convert(varchar, PO.ExpectedDeliveryDate,106), '','-') AS ExpectedDeliveryDate

from [POProductDetail] PO        
inner join [Product] P on PO.ProductId=P.ProductId         
left join uom u on P.PurchaseUOMId=u.UOMId
where  PO.POId=@POId 
set nocount off;              
end 



----------------------------------------------------------------------------------------------------------------------------------------------------------------------------


GO
/****** Object:  StoredProcedure [dbo].[proc_AddEditPO]    Script Date: 7/2/2022 8:11:55 PM ******/
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










