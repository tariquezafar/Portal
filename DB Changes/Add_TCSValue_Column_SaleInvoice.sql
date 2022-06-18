IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'TCSValue'
          AND Object_ID = Object_ID(N'dbo.SaleInvoice'))
begin
alter table SaleInvoice
add TCSValue decimal(18,2)  constraint DF_SaleInvoice_TCSValue default 0
end

go



if exists (select * from sys.objects where type='p' and name ='proc_AddEditSaleInvoice')
begin
		drop proc proc_AddEditSaleInvoice
end
go
create proc [dbo].[proc_AddEditSaleInvoice]              
(             
@InvoiceId bigint,             
@InvoiceDate date,   
@InvoiceType varchar(100)='',            
@SOId bigint,            
@SONo varchar(100)='',   
@CompanyBranchId int,  
@CurrencyCode varchar(50)='INR',             
@CustomerId int,              
@CustomerName varchar(100)='',              
@ConsigneeId int,              
@ConsigneeName varchar(100)='',              
@ContactPerson VARCHAR(100)='',            
@BillingAddress varchar(100)='',                
@City varchar(50)='',                
@StateId int,                
@CountryId int,                
@PinCode varchar(20)='',                
@CSTNo varchar(20)='',                
@TINNo varchar(20)='',                
@PANNo varchar(20)='',                
@GSTNo varchar(20)='',                
@ExciseNo varchar(20)='',                
@Email varchar(100)='',                
@MobileNo varchar(20)='',                
@ContactNo varchar(20)='',                
@Fax varchar(20)='',     
@ApprovalStatus varchar(20),  
@SaleType varchar(50),              
            
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
          
@RefNo varchar(50)='',              
@RefDate date,              
@BasicValue decimal(18,2),                
@TotalValue decimal(18,2),                
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

@RtoRegsValue decimal(18,2)=0,                  
@RtoRegsCGST_Amt decimal(18,2)=0,                  
@RtoRegsSGST_Amt decimal(18,2)=0,                  
@RtoRegsIGST_Amt decimal(18,2)=0,                  
@RtoRegsCGST_Perc decimal(18,2)=0,                  
@RtoRegsSGST_Perc decimal(18,2)=0,                  
@RtoRegsIGST_Perc decimal(18,2)=0,                  
@VehicleInsuranceValue decimal(18,2)=0,

           
@PayToBookId INT=0,            
@Remarks varchar(2000)='',            
@FinYearId int,              
@CompanyId int,              
@CreatedBy int,              
@ReverseChargeApplicable bit=0,
@ReverseChargeAmount decimal(18,2) =0,
@RoundOfValue decimal(18,2)=0,
@GrossValue decimal(18,2)=0,
@TransportName varchar(500)='',
@VehicleNo varchar(500)='',
@BiltyNo varchar(500)='',
@BiltyDate varchar(500),

@AdharcardNo varchar(50)='',
@Pancard varchar(50)='',
@IdtypeName varchar(50)='',
@IdtypeValue varchar(50)='',
@HypothecationBy varchar(50)='',
@EwayBillNo varchar(100)='',
@SaleEmpId bigint=0,

@SaleInvoiceProductDetail udt_SaleInvoiceProductDetail readonly,              
@SaleInvoiceTaxDetail udt_SaleInvoiceTaxDetail readonly,              
@SaleInvoiceTermDetail udt_SaleInvoiceTermDetail readonly,    
@SaleInvoiceProductSerialDetail udt_SaleInvoiceProductSerialDetail readonly,  
@SaleInvoiceSupportingDocument udt_SaleInvoiceSupportingDocument readonly,
@SaleInvoiceType varchar(50),
@status varchar(50) out,              
@message varchar(500) out,              
@RetInvoiceId bigint out              
)              
as              
begin              
BEGIN TRY              
BEGIN TRANSACTION              
declare @RetWarrantyID as bigint; 
DECLARE @FinYearStartDate as date;              
DECLARE @FinYearEndDate as date;              
DECLARE @FinYearCode AS VARCHAR(20);              
DECLARE @CompanyCode as varchar(10);   
DECLARE @CompanyBranchCode as varchar(10);            
DECLARE @CompanyStateId as int;        
DECLARE @StateCode as varchar(10);      

declare @temp_ProductEntry as table    
(    
 RowId int,    
 ProductId bigint,    
 Quantity decimal(18,2)    
);  

DECLARE @PackingListTypeId int=0;
DECLARE @SerialRequisitionId bigint;
DECLARE @SerialRetRequisitionId bigint;
declare @temp_ProductSerials as table    
(    
 RowId int,    
 refserial1 varchar(100), 
 PackingListTypeId int    
);
declare @temp_CheckSerials as table    
(    
 RowId int,    
 refserial1 varchar(100) 
);
declare @RowCount int=0;    
declare @RowNum int; 

declare @ProductBOMRowCount int=0;    
declare @ProductBOMRowNum int; 

declare @temp_ProductWarranty as table    
(    
 RowId int,    
 Productid bigint,    
 Quantity decimal(18,2) ,
 IsWarrantyProduct varchar(50),
 WarrantyPeriodMonth int 
);   
declare @WarrantyrowID bigint=0;
declare @WarrantyRowCount int=0;    
declare @WarrantyRowNum int;
declare @WarrantyProductid as bigint;       

declare @ChallanProductId bigint;    
declare @ChallanQuantity decimal(18,2);    
declare @StockQuantity decimal(18,2);                  
declare @ProductSerialCount int=0;    
declare @ProductSerialNo varchar(50)='';
declare @MaxRowCheckSerialCount int=0;    
declare @RowCheckSerialNum int=1; 
declare @TCSApplicable  bit, @TCSValue decimal(18,2)=0 , @TotalValueSum decimal(18, 2)
-------------------Product Serials Check------------------------- 
insert into @temp_CheckSerials(RowId,refserial1)
select row_number() over (order by  refserial1),refserial1  from @SaleInvoiceProductSerialDetail 
select @MaxRowCheckSerialCount = max(RowId) from @temp_CheckSerials

while (@RowCheckSerialNum<=@MaxRowCheckSerialCount)    
begin  
			    
	select @ProductSerialCount =ISNULL( COUNT(psd.RefSerial1),0)
	from SaleInvoiceProductSerialDetail psd 
	inner join SaleInvoice si on psd.InvoiceId=si.InvoiceId				
	where si.CompanyBranchId = @CompanyBranchId and isnull(psd.SaleReturnCancel,'') not in ('SaleReturn','CancelSI')
	and  psd.RefSerial1 in(select RefSerial1 from @temp_CheckSerials tmp where tmp.RowId=@RowCheckSerialNum) 
					

	IF @ProductSerialCount>0  and @InvoiceId=0        
		BEGIN 
			select @ProductSerialNo=RefSerial1+' '+@ProductSerialNo from @temp_CheckSerials tmp where tmp.RowId=@RowCheckSerialNum
			SET @message= @ProductSerialNo +' has already been used in Other Sale Invoice';              
			set @status='FAIL';              
			set @RetInvoiceId=0;              
			RAISERROR(@message,16,1);
					             
		END  
		set @RowCheckSerialNum=@RowCheckSerialNum+1; 
END
--select * from SaleInvoice

-------------------End Product Serials Check-----------------------------
SELECT @CompanyCode=c.CompanyCode,@CompanyStateId=Isnull(c.State,0) from Company c where c.CompanyId=@CompanyId   

SELECT @CompanyBranchCode=CB.CompanyBranchCode from ComapnyBranch CB where CB.CompanyBranchId=@CompanyBranchId             
  
select @StateCode=st.StateCode from dbo.ComapnyBranch cb inner join [state] st     
on cb.StateId=st.StateId where cb.CompanyBranchId=@CompanyBranchId   
  
if Isnull(@StateCode,'')=''    
begin    
 set @StateCode='DL';    
end    
    
                
SELECT @FinYearStartDate=StartDate,@FinYearEndDate=EndDate,@FinYearCode=FinYearCode   
FROM FinancialYear WHERE FinYearId=@FinYearId    
 
        
IF @InvoiceDate<@FinYearStartDate OR @InvoiceDate>@FinYearEndDate               
 BEGIN              
  SET @message='Invoice Date must be within selected financial year.';              
  set @status='FAIL';              
  set @RetInvoiceId=0;              
  RAISERROR(@message,16,1);              
 END              
              
              
IF @InvoiceId=0  -- INSERT START              
BEGIN              
      
DECLARE @InvoiceNo AS VARCHAR(50);              
DECLARE @MaxInvoiceNo as int;              
              
SELECT @MaxInvoiceNo=MAX(InvoiceSequence) FROM SaleInvoice WHERE COMPANYID=@CompanyId and FinYearId=@FinYearid   and CompanyBranchId=@CompanyBranchId        
      
IF ISNULL(@MaxInvoiceNo,0)<>0              
BEGIN              
 SET @MaxInvoiceNo=@MaxInvoiceNo+1;              
END              
ELSE              
BEGIN              
 SET @MaxInvoiceNo=1;              
END              
        
IF (@InvoiceType='TAX')          
begin          
set @InvoiceNo=@CompanyBranchCode + '/TI/' + RIGHT(@FinYearCode,5) + '/' +  FORMAT(@MaxInvoiceNo,'000#');                
end          
else          
begin          
set @InvoiceType='RETAIL';          
set @InvoiceNo=@CompanyBranchCode + '/RI/'+ RIGHT(@FinYearCode,5) + '/' +  FORMAT(@MaxInvoiceNo,'000#');                
end           
        
INSERT INTO SaleInvoice(InvoiceNo,InvoiceDate,InvoiceType,SOId,SONo,CompanyBranchId,CurrencyCode,
CustomerId,CustomerName,ConsigneeId,ConsigneeName, ContactPerson,            
BillingAddress,City,StateId,CountryId,PinCode,CSTNo,TINNo,PANNo,GSTNo,ExciseNo,            
Email,MobileNo,ContactNo,Fax,ApprovalStatus ,           
ShippingContactPerson,ShippingBillingAddress,ShippingCity,ShippingStateId,ShippingCountryId,ShippingPinCode,            
ShippingTINNo,ShippingEmail,ShippingMobileNo,ShippingContactNo,ShippingFax,            
RefNo,RefDate,BasicValue,TotalValue,RoundOfValue,GrossValue,  
FreightValue,FreightCGST_Perc,FreightCGST_Amt,
FreightSGST_Perc,FreightSGST_Amt,FreightIGST_Perc,FreightIGST_Amt,
LoadingValue,LoadingCGST_Perc,LoadingCGST_Amt,
LoadingSGST_Perc,LoadingSGST_Amt,LoadingIGST_Perc,LoadingIGST_Amt,
InsuranceValue,InsuranceCGST_Perc,InsuranceCGST_Amt,
InsuranceSGST_Perc,InsuranceSGST_Amt,InsuranceIGST_Perc,InsuranceIGST_Amt
,PayToBookId,Remarks,             
FinYearId,CompanyId,CreatedBy,CreatedDate,InvoiceSequence,
ReverseChargeApplicable,ReverseChargeAmount,SaleType,TransportName,VehicleNo,BiltyNo,BiltyDate,
AdharcardNo,Pancard,IdtypeName,IdtypeValue,HypothecationBy,RtoRegsValue,RtoRegsCGST_Amt,RtoRegsSGST_Amt,
RtoRegsIGST_Amt,RtoRegsCGST_Perc,RtoRegsSGST_Perc,RtoRegsIGST_Perc,VehicleInsuranceValue,EwayBillNo,SaleEmpId,SaleInvoiceType) 
             
VALUES(@InvoiceNo,@InvoiceDate,@InvoiceType, @SOId,@SONo,@CompanyBranchId,@CurrencyCode,
@CustomerId,@CustomerName,@ConsigneeId,@ConsigneeName,@ContactPerson,@BillingAddress,
@City,@StateId,@CountryId,@PinCode,@CSTNo,@TINNo,@PANNo,@GSTNo,@ExciseNo,            
@Email,@MobileNo,@ContactNo,@Fax,@ApprovalStatus,            
@ShippingContactPerson,@ShippingBillingAddress,@ShippingCity,@ShippingStateId,@ShippingCountryId,            
@ShippingPinCode,@ShippingTINNo,@ShippingEmail,@ShippingMobileNo,@ShippingContactNo,@ShippingFax,            
@RefNo,@RefDate,                
@BasicValue,@TotalValue,@RoundOfValue,@GrossValue,
@FreightValue,@FreightCGST_Perc,@FreightCGST_Amt,
@FreightSGST_Perc,@FreightSGST_Amt,@FreightIGST_Perc,@FreightIGST_Amt,
@LoadingValue,@LoadingCGST_Perc,@LoadingCGST_Amt,
@LoadingSGST_Perc,@LoadingSGST_Amt,@LoadingIGST_Perc,@LoadingIGST_Amt,
@InsuranceValue,@InsuranceCGST_Perc,@InsuranceCGST_Amt,
@InsuranceSGST_Perc,@InsuranceSGST_Amt,@InsuranceIGST_Perc,@InsuranceIGST_Amt ,
@PayToBookId,@Remarks, @FinYearId,@CompanyId,@CreatedBy,GETDATE(),@MaxInvoiceNo,
@ReverseChargeApplicable,@ReverseChargeAmount,@SaleType,@TransportName,@VehicleNo,@BiltyNo,@BiltyDate,
@AdharcardNo,@Pancard,@IdtypeName,@IdtypeValue,@HypothecationBy,
@RtoRegsValue,@RtoRegsCGST_Amt,@RtoRegsSGST_Amt,@RtoRegsIGST_Amt,
@RtoRegsCGST_Perc,@RtoRegsSGST_Perc,@RtoRegsIGST_Perc,@VehicleInsuranceValue,@EwayBillNo,@SaleEmpId, @SaleInvoiceType)              
              
set @RetInvoiceId=SCOPE_IDENTITY();               
              
insert into SaleInvoiceProductDetail(InvoiceId,ProductId,ProductShortDesc,Price,Quantity,DiscountPercentage,              
DiscountAmount,TaxId,TaxName,TaxPercentage,TaxAmount,TotalPrice,  
SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,  
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,  
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3 ,
CGST_Perc,CGST_Amount,SGST_Perc,    
SGST_Amount,IGST_Perc,IGST_Amount,HSN_Code)              
select @RetInvoiceId,ProductId,ProductShortDesc,Price,Quantity,DiscountPercentage,              
DiscountAmount,TaxId,TaxName,TaxPercentage,TaxAmount,TotalPrice,  
SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,  
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,  
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3 ,
CGST_Perc,CGST_Amount,SGST_Perc,    
SGST_Amount,IGST_Perc,IGST_Amount,HSN_Code                             
from @SaleInvoiceProductDetail              
            
            
insert into SaleInvoiceTaxDetail(InvoiceId,TaxId,TaxName,TaxPercentage,TaxAmount,  
SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,  
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,  
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3 )              
select @RetInvoiceId,TaxId,TaxName,TaxPercentage,TaxAmount,  
SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,  
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,  
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3             
from @SaleInvoiceTaxDetail             
            
insert into SaleInvoiceTermsDetail(InvoiceId,TermDesc,TermSequence)              
select @RetInvoiceId,TermDesc,TermSequence            
from @SaleInvoiceTermDetail


insert into SaleInvoiceSupportingDocument(InvoiceId,DocumentTypeId,DocumentName,DocumentPath,
LabourCode,[Description],LabourRate,DiscountPerc,DiscountAmount,CGST_Perc,CGST_Amount,SGST_Perc,SGST_Amount,
IGST_Perc,IGST_Amount,TotalAmount,ProductId)          
select @RetInvoiceId,DocumentTypeId,DocumentName, DocumentPath ,
LabourCode,Description,LabourRate,DiscountPerc,DiscountAmount,CGST_Perc,CGST_Amount,SGST_Perc,SGST_Amount,
IGST_Perc,IGST_Amount,TotalAmount,ProductId   
from @SaleInvoiceSupportingDocument                
     
insert into  SaleInvoiceProductSerialDetail(InvoiceId,ProductId,RefSerial1,RefSerial2,RefSerial3,RefSerial4,PackingListTypeID)  
select @RetInvoiceId, ProductId,RefSerial1,RefSerial2,RefSerial3,RefSerial4,PackingListTypeID from @SaleInvoiceProductSerialDetail    
  
-------------------Update Stock IN ProductTrnStock---------------------------------------------------
  if(@ApprovalStatus='Final')
  begin

  
  select @TCSApplicable= ISNULL(IsTCSApplicable,0)
from  [dbo].[CustomerTCSDetail] 
where CustomerID=@Customerid and FinYEarId=@FinYearId  
if(@TCSApplicable is null or @TCSApplicable=0 )
begin

select  @TotalValueSum=  
--*
(isnull( sum( E.TotalValue) ,0)+ @TotalValue) 
from SaleInvoice E where E.CustomerId=@Customerid and FinYearId=@FinYearId and ApprovalStatus='Final'
print @TotalValueSum
IF(@TotalValueSum>5000000)
BEGIN
  SET @TCSValue=(@TotalValueSum-5000000)*(0.001)
  INSERT INTO [dbo].[CustomerTCSDetail]
  (CustomerID,FinYEarId,IsTCSApplicable,TCSApplicableDate) VALUES (@Customerid,@FinYearId,1, @InvoiceDate)
END
end
else
begin

select @TCSValue= TotalValue*(0.001) from SaleInvoice SI where SI.InvoiceId=@RetInvoiceId
end

update [dbo].[SaleInvoice] set TCSValue=@TCSValue where  InvoiceId=@RetInvoiceId
  

  --Update ChasisSerialMapping---

  select RefSerial1 into #temp_SaleInvoiceProductSerialDetail from @SaleInvoiceProductSerialDetail

  UPDATE ChasisSerialMapping
 SET ChasisSerialNo = TempSIPSD.RefSerial1,ProductSerialStatus='SOLD'
 FROM ChasisSerialMapping CSM
INNER JOIN #temp_SaleInvoiceProductSerialDetail TempSIPSD ON CSM.ChasisSerialNo = TempSIPSD.RefSerial1
where CSM.CompanyBranchId=@CompanyBranchId

drop table #temp_SaleInvoiceProductSerialDetail

--End Update ChasisSerialMapping---





insert into @temp_ProductEntry(RowId,ProductId,Quantity)    
select row_number() over (order by  ProductId), ProductId,Sum(Quantity)    
from @SaleInvoiceProductDetail                  
group by ProductId    

set @RowCount=(select Max(RowId) from @temp_ProductEntry);    
set @RowNum=1;    


set @StockQuantity=0;    
while (@RowNum<=@RowCount)    
begin    
		 set @StockQuantity=0;    
     
		 select @ChallanProductId=ProductId,@ChallanQuantity=Quantity     
		 from @temp_ProductEntry where RowId=@RowNum;  
		 
		set @StockQuantity=dbo.udf_GetProductBalanceQTY(@FinYearid,@CompanyId , 
							@CompanyBranchId,@ChallanProductId , @FinYearStartDate ,
							@FinYearEndDate )  
   
     --commented Below code  On Date 18-Apr-2018 By Dheeraj And Hari Sir
	 --Created New GLobal Function Get Product Balance

		 --set @StockQuantity=isnull((select pos.OpeningQty from dbo.ProductOpeningStock pos     
		 --where pos.FinYearid=@FinYearid and  pos.CompanyId=@CompanyId and      
		 --pos.CompanyBranchId=@CompanyBranchId and pos.ProductId=@ChallanProductId),0)    
		 --+    
		 --Isnull((SELECT sum(pts.TrnQty) from      
		 --ProductTrnStock PTS  where pts.TrnType IN ('MRN','STR','FAB','FG','JWM','SR') AND      
		 --pts.CompanyId=@CompanyId and      
		 --pts.ToCompanyBranchId=@CompanyBranchId and pts.ProductId=@ChallanProductId    
		 --and pts.TrnDate between  @FinYearStartDate and @FinYearEndDate),0)    
		 --+    
		 --Isnull((SELECT sum(pts.TrnQty) from  ProductTrnStock PTS      
		 --where pts.TrnType IN ('DC','STN','ISS','PR','JW') AND      
		 --pts.CompanyId=@CompanyId and      
		 --pts.FromCompanyBranchId=@CompanyBranchId and pts.ProductId=@ChallanProductId    
		 --and pts.TrnDate between  @FinYearStartDate and @FinYearEndDate),0)    
    
		 IF @StockQuantity<@ChallanQuantity    
		 BEGIN                  
		  SET @message='Stock is not Sufficient in selected Location';                  
		  set @status='FAIL';                  
		  set @RetInvoiceId=0;                  
		  RAISERROR(@message,16,1);                  
		 END                  
         
		 insert into ProductTrnStock(ProductId,CompanyId,TrnType,TrnDate,TrnQty,TrnReferenceNo,TrnReferenceDate,FromCompanyBranchId,ToCompanyBranchId)            
		 values(@ChallanProductId,@CompanyId,'DC',@InvoiceDate,-1*@ChallanQuantity,@RetInvoiceId,@InvoiceDate,@CompanyBranchId,0)    
		 set @RowNum=@RowNum+1;  
  
end 

end
----------------------------------END CODE---------------------------------------------------
---Create Store requisition Based on Packing Material BOM------------------------------------
IF @ApprovalStatus='Final'
BEGIN

insert into @temp_ProductSerials(RowId,refserial1,PackingListTypeId)    
select row_number() over (order by  refserial1),refserial1,PackingListTypeID    
from @SaleInvoiceProductSerialDetail                  
group by refserial1,PackingListTypeId    

set @ProductBOMRowCount=(select Max(RowId) from @temp_ProductSerials);    
set @ProductBOMRowNum=1;    

    
while (@ProductBOMRowNum<=@ProductBOMRowCount)    
begin      
  
 
 select @PackingListTypeId=PackingListTypeId from @temp_ProductSerials    
 where RowId=@ProductBOMRowNum;

 if(@PackingListTypeId<>0)
 BEGIN
 exec proc_GetPMBProductsByPackingListTypeId @RetInvoiceId,@PackingListTypeId,@invoiceDate,
 @CompanyBranchId,@FinYearId,@CompanyId,@CreatedBy     --,@SerialRequisitionId OUTPUT

 --update SaleInvoiceProductSerialDetail set RequisitionId = @SerialRetRequisitionId 
 --where InvoiceId = @InvoiceId and PackingListTypeID = @PackingListTypeId

 END
     
 set @ProductBOMRowNum=@ProductBOMRowNum+1;    
end  
END
------------End Create Store requisition Based on Packing Material BOM-----   

-----------Insert Data in Warranty Table And WarrantyProductDetail Table If Product Is Warranty True--------------
 IF @ApprovalStatus='Final'
 begin
                     
        INSERT INTO Warranty(WarrantyDate,InvoicePackingListId,InvoicePackingListNo,
		InvoicePackingListDate,InvoiceId,InvoiceNo,CompanyBranchId,DispatchDate)                  
		VALUES(Getdate(), 0,'','', @RetInvoiceId,@InvoiceNo,@CompanyBranchId,@InvoiceDate)  



		set @RetWarrantyID=SCOPE_IDENTITY();  

	    insert into @temp_ProductWarranty(RowId,Productid,Quantity)  
		select row_number() over (order by  ProductId),ProductId,Quantity  
		from @SaleInvoiceProductDetail               
					    
		set @WarrantyRowCount=(select Max(RowId) from @temp_ProductWarranty);  
		set @WarrantyRowNum=1; 
						   				
		while (@WarrantyRowNum<=@WarrantyRowCount) 
	
		begin						   						

		select @WarrantyrowID=RowId from @temp_ProductWarranty where RowId=@WarrantyRowNum
		select @WarrantyProductid=Productid from @temp_ProductWarranty where RowId=@WarrantyRowNum

		if(@WarrantyProductid in (Select AssemblyId from ProductBOM))
		begin
		insert into WarrantyProductDetail(WarrantyID,ProductId,WarrantyPeriodMonth,Quantity,WarrantyStartDate,WarrantyEndDate)
		select @RetWarrantyID,pb.ProductId,P.WarrantyInMonth,sipd.Quantity*pb.BOMQty as BOMQty,@InvoiceDate,DATEADD (month , p.WarrantyInMonth , @InvoiceDate )		
		from @SaleInvoiceProductDetail sipd
		inner join ProductBOM PB on sipd.ProductId=pb.assemblyId
		inner join Product P on PB.ProductId= P.Productid
		where pb.Status=1 and p.IsWarrantyProduct=1
		 
		end


	else
			begin 
			insert into WarrantyProductDetail(WarrantyID,ProductId,WarrantyPeriodMonth,Quantity,WarrantyStartDate,WarrantyEndDate)
		    select @RetWarrantyID,P.ProductId,P.WarrantyInMonth,sipd.Quantity BOMQty,@InvoiceDate,DATEADD (month , p.WarrantyInMonth , @InvoiceDate )		
			from @SaleInvoiceProductDetail sipd			
			inner join Product P on sipd.ProductId= P.Productid
			where p.IsWarrantyProduct=1
	end			
	     set @WarrantyRowNum=@WarrantyRowNum+1; 
		end						       		
 end
-------------------End Code---------------------------------------------------------------------------------------
           


--Sale Voucher Posting Code  
--Below code Uncomented By Hari Sir And Dheeraj date 23/Sep/2017 For Posting In FA
--  IF @ApprovalStatus='Final'
--BEGIN
--exec proc_AddEditSaleVoucher @RetInvoiceId,@CompanyId,@FinYearId,@CompanyBranchId,@CreatedBy,@ApprovalStatus,@SaleInvoiceProductDetail,@SaleInvoiceTaxDetail  
--END
--End of Sale voucher Posting Code              
              
              
              
SET @message='';              
set @status='SUCCESS';              
      
              
               
END              
ELSE  -- MODIFY START              
BEGIN              
            
 update SaleInvoice set InvoiceDate=@InvoiceDate,              
 SOId=@SOId,  
 SONo=@SONo,      
 CompanyBranchId=@CompanyBranchId,  
 CurrencyCode=@CurrencyCode,         
 CustomerId=@CustomerId,              
 CustomerName=@CustomerName,      
 ConsigneeId=@ConsigneeId,              
 ConsigneeName=@ConsigneeName,                      
 ContactPerson=@ContactPerson,     
 BillingAddress=@BillingAddress,                
 City=@City,                
 StateId=@StateId,                
 CountryId=@CountryId,                
 PinCode=@PinCode,                
 CSTNo=@CSTNo,                
 TINNo=@TINNo,            
 PANNo=@PANNo,                
 GSTNo=@GSTNo,                
 ExciseNo=@ExciseNo,                
 Email=@Email,            
 MobileNo=@MobileNo,            
 ContactNo=@ContactNo,            
 Fax=@Fax,      
 ApprovalStatus=@ApprovalStatus,          
   
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
 RefNo=@RefNo,                
 RefDate=@RefDate,                
 BasicValue=@BasicValue,                
 TotalValue=@TotalValue, 
 RoundOfValue=@RoundOfValue,
 GrossValue=@GrossValue,               
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
 PayToBookId=@PayToBookId,            
 Remarks=@Remarks,              
 ModifiedBy=@CreatedBy,              
 ModifiedDate=getdate() ,
 ReverseChargeApplicable=@ReverseChargeApplicable,
 ReverseChargeAmount=@ReverseChargeAmount , 
 SaleType=@SaleType ,
 TransportName=@TransportName,
 VehicleNo=@VehicleNo,
 BiltyNo=@BiltyNo,
 BiltyDate=@BiltyDate,

 RtoRegsValue=@RtoRegsValue,
 RtoRegsCGST_Amt=@RtoRegsCGST_Amt,
 RtoRegsSGST_Amt=@RtoRegsSGST_Amt,
 RtoRegsIGST_Amt=@RtoRegsIGST_Amt,
 RtoRegsCGST_Perc=@RtoRegsCGST_Perc,
 RtoRegsSGST_Perc=@RtoRegsSGST_Perc,
 RtoRegsIGST_Perc=@RtoRegsIGST_Perc,
 VehicleInsuranceValue=@VehicleInsuranceValue,
 
 AdharcardNo=@AdharcardNo,
 Pancard=@Pancard,
 IdtypeName=@IdtypeName,
 IdtypeValue=@IdtypeValue,
 HypothecationBy=@HypothecationBy,
 EwayBillNo=@EwayBillNo,
 SaleEmpId=@SaleEmpId
 
        
 where InvoiceId=@InvoiceId             
              
 delete from SaleInvoiceProductDetail where InvoiceId=@InvoiceId              
               
 insert into SaleInvoiceProductDetail(InvoiceId,ProductId,ProductShortDesc,Price,Quantity,DiscountPercentage,              
 DiscountAmount,TaxId,TaxName,TaxPercentage,TaxAmount,TotalPrice,SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,  
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,  
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3,
CGST_Perc,CGST_Amount,SGST_Perc,    
SGST_Amount,IGST_Perc,IGST_Amount,HSN_Code)              
 select @InvoiceId,ProductId,ProductShortDesc,Price,Quantity,DiscountPercentage,              
 DiscountAmount,TaxId,TaxName,TaxPercentage,TaxAmount,TotalPrice ,SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,  
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,  
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3              ,
CGST_Perc,CGST_Amount,SGST_Perc,    
SGST_Amount,IGST_Perc,IGST_Amount,HSN_Code             
 from @SaleInvoiceProductDetail              
              
 delete from SaleInvoiceTaxDetail where InvoiceId=@InvoiceId               
 insert into SaleInvoiceTaxDetail(InvoiceId,TaxId,TaxName,TaxPercentage,TaxAmount,SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,  
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,  
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3)              
 select @InvoiceId,TaxId,TaxName,TaxPercentage,TaxAmount ,SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,  
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,  
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3           
 from @SaleInvoiceTaxDetail             
              
delete from SaleInvoiceTermsDetail where InvoiceId=@InvoiceId              
insert into SaleInvoiceTermsDetail(InvoiceId,TermDesc,TermSequence)              
select @InvoiceId,TermDesc,TermSequence            
from @SaleInvoiceTermDetail         
   
delete from SaleInvoiceProductSerialDetail where InvoiceId=@InvoiceId   
insert into  SaleInvoiceProductSerialDetail(InvoiceId,ProductId,RefSerial1,RefSerial2,RefSerial3,RefSerial4,PackingListTypeID)  
select @InvoiceId, ProductId,RefSerial1,RefSerial2,RefSerial3,RefSerial4,PackingListTypeID from @SaleInvoiceProductSerialDetail 

delete from SaleInvoiceSupportingDocument where InvoiceId=@InvoiceId      
insert into SaleInvoiceSupportingDocument(InvoiceId,DocumentTypeId,DocumentName,DocumentPath,
LabourCode,Description,LabourRate,DiscountPerc,DiscountAmount,CGST_Perc,CGST_Amount,SGST_Perc,SGST_Amount,
IGST_Perc,IGST_Amount,TotalAmount,ProductId)          
select @InvoiceId,DocumentTypeId,DocumentName, DocumentPath ,
LabourCode,Description,LabourRate,DiscountPerc,DiscountAmount,CGST_Perc,CGST_Amount,SGST_Perc,SGST_Amount,
IGST_Perc,IGST_Amount,TotalAmount,ProductId   
from @SaleInvoiceSupportingDocument   

 
 
    
-------------------Update Stock IN ProductTrnStock---------------------------------------------------
  if(@ApprovalStatus='Final')
  begin

   ----TCS value calculation
  select @TCSApplicable= ISNULL(IsTCSApplicable,0)
from  [dbo].[CustomerTCSDetail] 
where CustomerID=@Customerid and FinYEarId=@FinYearId  
if( @TCSApplicable is null OR  @TCSApplicable=0)
begin

select  @TotalValueSum=  
--*
isnull( sum( E.TotalValue) ,0)
from SaleInvoice E where E.CustomerId=@Customerid and FinYearId=@FinYearId and ApprovalStatus='Final'
IF(@TotalValueSum>5000000)
BEGIN
  SET @TCSValue=(@TotalValueSum-5000000)*(0.001)
  INSERT INTO [dbo].[CustomerTCSDetail]
  (CustomerID,FinYEarId,IsTCSApplicable,TCSApplicableDate) VALUES (@Customerid,@FinYearId,1, @InvoiceDate)
END
end
else
begin

select @TCSValue= TotalValue*(0.001) from SaleInvoice SI where SI.InvoiceId=@InvoiceId
end

update [dbo].[SaleInvoice] set TCSValue=@TCSValue where  InvoiceId=@InvoiceId
    --Update ChasisSerialMapping---

  select RefSerial1 into #temp_SaleInvoiceProductSerialDetailUpdate from @SaleInvoiceProductSerialDetail

  UPDATE ChasisSerialMapping
 SET ChasisSerialNo = TempSIPSD.RefSerial1,ProductSerialStatus='SOLD'
 FROM ChasisSerialMapping CSM
INNER JOIN #temp_SaleInvoiceProductSerialDetailUpdate TempSIPSD ON CSM.ChasisSerialNo = TempSIPSD.RefSerial1
where CSM.CompanyBranchId=@CompanyBranchId
drop table #temp_SaleInvoiceProductSerialDetailUpdate

--End Update ChasisSerialMapping---

insert into @temp_ProductEntry(RowId,ProductId,Quantity)    
select row_number() over (order by  ProductId), ProductId,Sum(Quantity)    
from @SaleInvoiceProductDetail                  
group by ProductId    

set @RowCount=(select Max(RowId) from @temp_ProductEntry);    
set @RowNum=1;    


set @StockQuantity=0;    
while (@RowNum<=@RowCount)    
begin    
 set @StockQuantity=0;    
     
 select @ChallanProductId=ProductId,@ChallanQuantity=Quantity     
 from @temp_ProductEntry where RowId=@RowNum;    

 set @StockQuantity=dbo.udf_GetProductBalanceQTY(@FinYearid,@CompanyId , 
							@CompanyBranchId,@ChallanProductId , @FinYearStartDate ,
							@FinYearEndDate )  
   
     --commented Below code  On Date 18-Apr-2018 By Dheeraj And Hari Sir
	 --Created New GLobal Function Get Product Balance
   
     
					 --set @StockQuantity=isnull((select pos.OpeningQty from dbo.ProductOpeningStock pos     
					 --where pos.FinYearid=@FinYearid and  pos.CompanyId=@CompanyId and      
					 --pos.CompanyBranchId=@CompanyBranchId and pos.ProductId=@ChallanProductId),0)    
					 --+    
					 --Isnull((SELECT sum(pts.TrnQty) from      
					 --ProductTrnStock PTS where pts.TrnType IN ('MRN','STR','FAB','FG','JWM','SR') AND      
					 --pts.CompanyId=@CompanyId and      
					 --pts.ToCompanyBranchId=@CompanyBranchId and pts.ProductId=@ChallanProductId    
					 --and pts.TrnDate between  @FinYearStartDate and @FinYearEndDate),0)    
					 --+    
					 --Isnull((SELECT sum(pts.TrnQty) from  ProductTrnStock PTS      
					 --where pts.TrnType IN ('DC','STN','ISS','JW','PR') AND      
					 --pts.CompanyId=@CompanyId and      
					 --pts.FromCompanyBranchId=@CompanyBranchId and pts.ProductId=@ChallanProductId    
					 --and pts.TrnDate between  @FinYearStartDate and @FinYearEndDate),0)    
    
 IF @StockQuantity<@ChallanQuantity    
 BEGIN                  
  SET @message='Stock is not Sufficient in selected Location';                  
  set @status='FAIL';                  
  set @RetInvoiceId=0;                  
  RAISERROR(@message,16,1);                  
 END                  
         
 insert into ProductTrnStock(ProductId,CompanyId,TrnType,TrnDate,TrnQty,TrnReferenceNo,TrnReferenceDate,FromCompanyBranchId,ToCompanyBranchId)            
 values(@ChallanProductId,@CompanyId,'DC',@InvoiceDate,-1*@ChallanQuantity,@InvoiceId,@InvoiceDate,@CompanyBranchId,0)    
 set @RowNum=@RowNum+1;  
  
end 

end
----------------------------------END CODE---------------------------------------------------
            

---Create Store requisition Based on Packing Material BOM------------
IF @ApprovalStatus='Final'
BEGIN

insert into @temp_ProductSerials(RowId,refserial1,PackingListTypeId)    
select row_number() over (order by  refserial1),refserial1,PackingListTypeID    
from @SaleInvoiceProductSerialDetail                  
group by refserial1,PackingListTypeId    

set @ProductBOMRowCount=(select Max(RowId) from @temp_ProductSerials);    
set @ProductBOMRowNum=1;    

    
while (@ProductBOMRowNum<=@ProductBOMRowCount)    
begin      
  
 
 select @PackingListTypeId=PackingListTypeId from @temp_ProductSerials    
 where RowId=@ProductBOMRowNum;

 if(@PackingListTypeId<>0)
 BEGIN
 exec proc_GetPMBProductsByPackingListTypeId @RetInvoiceId,@PackingListTypeId,@invoiceDate,
 @CompanyBranchId,@FinYearId,@CompanyId,@CreatedBy     --,@SerialRequisitionId OUTPUT

 --update SaleInvoiceProductSerialDetail set RequisitionId = @SerialRetRequisitionId 
 --where InvoiceId = @InvoiceId and PackingListTypeID = @PackingListTypeId

 END
     
 set @ProductBOMRowNum=@ProductBOMRowNum+1;    
end  
END
------------End Create Store requisition Based on Packing Material BOM-----   
           
-----------Insert Data in Warranty Table And WarrantyProductDetail Table If Product Is Warranty True--------------
 IF @ApprovalStatus='Final'
 begin

 declare @InvoiceNoForWarrenty varchar(20)
 select @InvoiceNoForWarrenty=InvoiceNo from SaleInvoice where InvoiceId=@InvoiceId
                     
        INSERT INTO Warranty(WarrantyDate,InvoicePackingListId,InvoicePackingListNo,InvoicePackingListDate,InvoiceId,InvoiceNo,CompanyBranchId,DispatchDate)                  
		VALUES(Getdate(), 0,'','', @InvoiceId,@InvoiceNoForWarrenty,@CompanyBranchId,@InvoiceDate)  



		set @RetWarrantyID=SCOPE_IDENTITY();  

	    insert into @temp_ProductWarranty(RowId,Productid,Quantity)  
		select row_number() over (order by  ProductId),ProductId,Quantity  
		from @SaleInvoiceProductDetail               
					    
		set @WarrantyRowCount=(select Max(RowId) from @temp_ProductWarranty);  
		set @WarrantyRowNum=1; 
						   				
		while (@WarrantyRowNum<=@WarrantyRowCount)  
	
		begin						   						

		select @WarrantyrowID=RowId from @temp_ProductWarranty where RowId=@WarrantyRowNum
		select @WarrantyProductid=Productid from @temp_ProductWarranty where RowId=@WarrantyRowNum

		if(@WarrantyProductid in (Select AssemblyId from ProductBOM))
		begin
		insert into WarrantyProductDetail(WarrantyID,ProductId,WarrantyPeriodMonth,Quantity,WarrantyStartDate,WarrantyEndDate)
		select @RetWarrantyID,pb.ProductId,P.WarrantyInMonth,sipd.Quantity*pb.BOMQty as BOMQty	,@InvoiceDate,DATEADD (month , p.WarrantyInMonth , @InvoiceDate )	
		from @SaleInvoiceProductDetail sipd
		inner join ProductBOM PB on sipd.ProductId=pb.assemblyId
		inner join Product P on PB.ProductId= P.Productid
		where pb.Status=1 and p.IsWarrantyProduct=1
		 
		end

		
	else
			begin 

			
			insert into WarrantyProductDetail(WarrantyID,ProductId,WarrantyPeriodMonth,Quantity,WarrantyStartDate,WarrantyEndDate)
		    select @RetWarrantyID,P.ProductId,P.WarrantyInMonth,sipd.Quantity BOMQty,@InvoiceDate,DATEADD (month , p.WarrantyInMonth , @InvoiceDate )		
			from @SaleInvoiceProductDetail sipd			
			inner join Product P on sipd.ProductId= P.Productid
			where p.IsWarrantyProduct=1
	end			
	       set @WarrantyRowNum=@WarrantyRowNum+1; 
		end				
		

        

		
 end
-------------------End Code---------------------------------------------------------------------------------------
           




--Sale Voucher Posting Code
--Below code Uncomented By Hari Sir And Dheeraj date 23/Sep/2017 For Posting In FA

--IF @ApprovalStatus='Final'
--BEGIN
--	exec proc_AddEditSaleVoucher @InvoiceId,@CompanyId,@FinYearId,@CompanyBranchId,@CreatedBy,@ApprovalStatus,@SaleInvoiceProductDetail,@SaleInvoiceTaxDetail               
--END  

--End of Sale voucher Posting Code          
 SET @message='';              
 set @status='SUCCESS';              
 set @RetInvoiceId=@InvoiceId;               
               
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
set @RetInvoiceId=0;              
END CATCH;              
end 

go

if exists (select * from sys.objects where type='p' and name='proc_GetSaleInvoiceDetail' )
begin
		drop proc proc_GetSaleInvoiceDetail
end
go
create proc [dbo].[proc_GetSaleInvoiceDetail] --proc_GetSaleInvoiceDetail 8           
(                  
@InvoiceId int                
)                  
as                  
begin                  
set nocount on;   


select E.InvoiceId,              
E.InvoiceNo,                  
replace(convert(varchar, E.InvoiceDate,106),' ','-') InvoiceDate,      
E.InvoiceType,
E.CustomerId,               
E.CustomerName,      
isnull(E.ConsigneeId,0) ConsigneeId,               
isnull(E.ConsigneeName,'') as ConsigneeName,      
E.SOId,      
E.SONo,  
isnull(E.CurrencyCode,'INR') AS CurrencyCode,                   
ISNULL(E.CompanyBranchId,0) CompanyBranchId ,     
case when SO.SODate is null or SO.SODate < '1900-12-01' then '' else replace(convert(varchar, SO.SODate,106),' ','-') end SODate,              
C.CustomerCode, 

ISNULL(jc.JobCardNo,'') as JobCardNo,
case when jc.JobCardDate is null or jc.JobCardDate < '1900-12-01' then '' else replace(convert(varchar, jc.JobCardDate,106),' ','-') end JobCardDate,
isnull(jc.ModelName,'') as ModelName,
isnull(jc.EngineNo,'') as EngineNo,
isnull(jc.RegNo,'')    as RegNo,
isnull(jc.KMSCovered,'') as KMSCovered,

CN.CustomerCode as ConsigneeCode,
CN.MobileNo  as ConsigneeMobileNo,

C.MobileNo as CustomerMobileNo,
c.CSTNo CustomerCSTNo, 
c.GSTNo,  
    
E.ContactPerson,      
E.BillingAddress,                
E.City,                
E.StateId,            
billState.StateName,    
billState.StateCode,
E.CountryId,            
billCountry.CountryName,    
E.PinCode, 
E.ApprovalStatus,               
E.CSTNo,            
E.TINNo,            
E.PANNo, 
           

          
E.ExciseNo,         
E.Email,      
E.MobileNo,      
E.ContactNo,      
E.Fax,      
E.ShippingContactPerson,      
E.ShippingBillingAddress,                
E.ShippingCity,                
ISNULL(E.ShippingStateId,0) ShippingStateId,            
isnull(shipState.StateName,'') ShippingStateName ,    
isnull(shipState.StateCode,'') ShippingStateCode ,    
ISNULL(E.ShippingCountryId,0) ShippingCountryId ,            
isnull(shipCountry.CountryName,'') ShippingCountryName ,    
E.ShippingPinCode,
               
CN.GSTNo as ShippingTINNo,            
E.ShippingEmail,      
E.ShippingMobileNo,      
E.ShippingContactNo,      
E.ShippingFax,      
isnull(E.SaleType,'') as SaleType,  

isnull(E.CancelReason,'') as  CancelReason,  
E.RefNo,        
case when E.RefDate is null or E.RefDate < '1900-12-01' then '' else replace(convert(varchar, E.RefDate,106),' ','-') end RefDate,              
E.BasicValue,        
E.TotalValue,
isnull(E.GrossValue,0) GrossValue,
isnull(E.RoundOfValue,0) RoundOfValue,        
ISNULL(E.LoadingValue,0) LoadingValue,        
ISNULL(E.LoadingCGST_Perc,0) LoadingCGST_Perc,        
ISNULL(E.LoadingCGST_Amt,0) LoadingCGST_Amt,
ISNULL(E.LoadingSGST_Perc,0) LoadingSGST_Perc,        
ISNULL(E.LoadingSGST_Amt,0) LoadingSGST_Amt,
ISNULL(E.LoadingIGST_Perc,0) LoadingIGST_Perc,        
ISNULL(E.LoadingIGST_Amt,0) LoadingIGST_Amt,        
ISNULL(E.FreightValue,0) FreightValue ,      
ISNULL(E.FreightCGST_Perc,0) FreightCGST_Perc,        
ISNULL(E.FreightCGST_Amt,0) FreightCGST_Amt,
ISNULL(E.FreightSGST_Perc,0) FreightSGST_Perc,        
ISNULL(E.FreightSGST_Amt,0) FreightSGST_Amt,
ISNULL(E.FreightIGST_Perc,0) FreightIGST_Perc,        
ISNULL(E.FreightIGST_Amt,0) FreightIGST_Amt,        
ISNULL(E.InsuranceValue,0) InsuranceValue ,      
ISNULL(E.InsuranceCGST_Perc,0) InsuranceCGST_Perc,        
ISNULL(E.InsuranceCGST_Amt,0) InsuranceCGST_Amt,
ISNULL(E.InsuranceSGST_Perc,0) InsuranceSGST_Perc,        
ISNULL(E.InsuranceSGST_Amt,0) InsuranceSGST_Amt,
ISNULL(E.InsuranceIGST_Perc,0) InsuranceIGST_Perc,        
ISNULL(E.InsuranceIGST_Amt,0) InsuranceIGST_Amt,        
ISNULL(E.PayToBookId,0) PayToBookId,      
isnull(book.BookName,'') PayToBookName,        
isnull(book.BankBranch,'') PayToBookBranch,      
isnull(book.IFSC,'') IFSC,      
isnull(book.BankAccountNo,'') BankAccountNo,      
E.Remarks,      
E.FinYearId,      
E.CompanyId,   
Com.CompanyName,    
cb.ContactPersonName  ContactPersonName,  
cb.ContactNo as CompanyPhone,    
cb.Email  as CompanyEmail,    
cb.Fax  as CompanyFax,    
com.Website as CompanyWebsite,    
cb.PrimaryAddress as CompanyAddress,    
cb.City as CompanyCity,   
CompState.StateName as CompanyStateName,    
CompState.StateCode as CompanyStateCode,    
CompCountry.CountryName CompanyCountryName ,    
cb.PinCode CompanyZipCode,     
com.CompanyDesc CompanyDesc,     
com.TINNo CompanyTINNo, 
isnull(cb.PANNo,'') CompanyPANNo,
isnull(cb.GSTNo,'') CBGSTNo,
Isnull(cb.MobileNo,'') CBMobileNo,
Isnull(cb.ContactNo,'') CBContactNo,
Isnull(cb.Fax,'') CBFaxNo,
isnull(dc.ChallanNo,'') ChallanNo,
case when dc.ChallanDate is null then '' else replace(convert(varchar, dc.ChallanDate,106),' ','-') end ChallanDate,
isnull(dc.DispatchRefNo,'') DispatchRefNo,
case when dc.DispatchRefDate is null then '' else replace(convert(varchar, dc.DispatchRefDate,106),' ','-') end DispatchRefDate,
isnull(dc.LRNo,'') LRNo,
case when dc.LRDate is null then '' else replace(convert(varchar, dc.LRDate,106),' ','-') end LRDate,
isnull(dc.TransportVia,'') TransportVia ,    
case when dc.ChallanDate is null then '' else replace(convert(varchar, dc.ChallanDate,106),' ','-') end DeliveryChallanDate,
E.CreatedBy,              
cu.FullName CreatedByName,                
replace(convert(varchar, E.CreatedDate,106),' ','-') CreatedDate,                  
E.ModifiedBy,                   
isnull(mu.FullName,'') ModifiedByName,                
case when E.ModifiedDate is null then '' else replace(convert(varchar, E.ModifiedDate,106),' ','-') end ModifiedDate      ,
cast(isnull(E.ReverseChargeApplicable,0) as bit) as ReverseChargeApplicable,
ISNULL(E.ReverseChargeAmount,0) AS ReverseChargeAmount,
finyear.FinYearCode,
isnull(E.TransportName,'') as TransportName,isnull(E.VehicleNo,'') as VehicleNo,isnull(E.BiltyNo,'') as BiltyNo,
case when E.BiltyDate is null or E.BiltyDate < '1900-12-01' then '' else replace(convert(varchar, E.BiltyDate,106),' ','-') end BiltyDate,
isnull(E.AdharcardNo,'') as AdharcardNo,
isnull(E.Pancard,'') as Pancard,
isnull(E.IdtypeName,'') as IdtypeName,
isnull(E.IdtypeValue,'') as IdtypeValue,

isnull(E.RtoRegsValue,0) as RtoRegsValue ,
isnull(E.RtoRegsCGST_Amt,0) as RtoRegsCGST_Amt ,    
isnull(E.RtoRegsSGST_Amt,0) as RtoRegsSGST_Amt,    
isnull(E.RtoRegsIGST_Amt,0) as RtoRegsIGST_Amt,
isnull(E.RtoRegsCGST_Perc,0) as RtoRegsCGST_Perc,    
isnull(E.RtoRegsSGST_Perc,0) as RtoRegsSGST_Perc,   
isnull(E.RtoRegsIGST_Perc,0) as RtoRegsIGST_Perc,    
isnull(E.VehicleInsuranceValue,0)  as VehicleInsuranceValue ,
isnull(CB.BranchType,'') as BranchType,
isnull(E.HypothecationBy,'') as HypothecationBy,
isnull(E.EwayBillNo,'') as EwayBillNo,
isnull(E.SaleEmpId,0) as SaleEmpId,
isnull(Emp.FirstName,'') as SaleEmpName,
E.TCSValue as TCSValue

from [SaleInvoice] E            
inner join Customer C on E.CustomerId=C.CustomerId      
left join Customer CN on E.ConsigneeId=CN.CustomerId
inner join [State] billState on E.StateId=billState.StateId    
inner join [Country] billCountry on E.CountryId=billCountry.CountryId    
inner join [State] shipState on E.ShippingStateId=shipState.StateId    
inner join [Country] shipCountry on E.ShippingCountryId=shipCountry.CountryId    
inner join [User] cu on E.createdBy=cu.UserId                
inner join Company Com on E.CompanyId=Com.CompanyId    
inner join FinancialYear finyear on e.FinYearId=finyear.FinYearId
left join ComapnyBranch CB on E.CompanyBranchId=CB.CompanyBranchId   
inner join [State] compState on CB.StateId=compState.StateId    
inner join [Country] compCountry on CB.CountryId=compCountry.CountryId  
left join [User] mu on E.ModifiedBy=mu.Userid                
left join SO SO on E.SOId=SO.SOId 

left join JobCard jc on E.SOId=jc.[JobCardID] 
     
LEFT JOIN BOOK book on E.PayToBookId=book.BookId      
left join DeliveryChallan dc on E.InvoiceId=dc.InvoiceId
left join employee Emp on Emp.EmployeeId=E.SaleEmpId
where  E.InvoiceId=@InvoiceId        



          
set nocount off;                  
end              








