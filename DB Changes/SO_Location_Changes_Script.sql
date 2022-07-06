alter table SO
add LocationId int

go
----[dbo].[proc_AddEditSO]     

ALTER proc [dbo].[proc_AddEditSO]          
(         
@SOId bigint,         
@SODate date,         
@QuotationId bigint,        
@QuotationNo varchar(100)='',   
@CompanyBranchId int,  
@CurrencyCode varchar(50)='INR',          
@CustomerId int,          
@CustomerName varchar(100)='',          
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
@ConsigneeId int,          
@ConsigneeName varchar(100)='',      
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
@LoadingValue decimal(18,2),          
@PayToBookId INT=0,      
@Remarks1 VARCHAR(2000),      
@Remarks2 VARCHAR(2000), 

@ReverseChargeApplicable bit=0,
@ReverseChargeAmount decimal(18,2),

               
@FreightCGST_Perc decimal(18,2)=0,                
@FreightCGST_Amt decimal(18,2)=0,                
@FreightSGST_Perc decimal(18,2)=0,                
@FreightSGST_Amt decimal(18,2)=0,                
@FreightIGST_Perc decimal(18,2)=0,                
@FreightIGST_Amt decimal(18,2)=0,                


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
@RtoRegsSGST_Amt decimal(18,2),                  
@RtoRegsIGST_Amt decimal(18,2)=0,                  
@RtoRegsCGST_Perc decimal(18,2)=0,                  
@RtoRegsSGST_Perc decimal(18,2)=0,                  
@RtoRegsIGST_Perc decimal(18,2)=0,                  
@VehicleInsuranceValue decimal(18,2)=0,  
     
@FinYearId int,          
@CompanyId int,     
@ApprovalStatus varchar(20),         
@CreatedBy int,

@AdharcardNo varchar(50)='',
@Pancard varchar(50)='',
@IdtypeName varchar(50)='',
@IdtypeValue varchar(50)='',
@HypothecationBy varchar(50)='', 
@LocationId int,          
@SOProductDetail udt_SOProductDetail readonly,          
@SOTaxDetail udt_SOTaxDetail readonly,          
@SOTermDetail udt_SOTermDetail readonly,  
@SOSupportingDocument udt_SOSupportingDocument readonly,         
@status varchar(50) out,          
@message varchar(500) out,          
@RetSOId bigint out          
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
DECLARE @CompanyStateId as int;                
DECLARE @StateCode as varchar(10);        
          
SELECT @CompanyCode=CompanyCode from Company where CompanyId=@CompanyId    

SELECT @CompanyBranchCode=CB.CompanyBranchCode from ComapnyBranch CB where CB.CompanyBranchId=@CompanyBranchId        
         
select @StateCode=st.StateCode from dbo.ComapnyBranch cb inner join [state] st     
on cb.StateId=st.StateId where cb.CompanyBranchId=@CompanyBranchId    
    
if Isnull(@StateCode,'')=''    
begin    
 set @StateCode='DL';    
end    
    
  
SELECT @FinYearStartDate=StartDate,@FinYearEndDate=EndDate,@FinYearCode=FinYearCode          
FROM FinancialYear WHERE FinYearId=@FinYearId          
          
IF @SODate<@FinYearStartDate OR @SODate>@FinYearEndDate           
 BEGIN          
  SET @message='SO Date must be within selected financial year.';          
  set @status='FAIL';          
  set @RetSOId=0;          
  RAISERROR(@message,16,1);          
 END          
          
          
IF @SOId=0  -- INSERT START          
BEGIN          
          
DECLARE @SONo AS VARCHAR(50);          
DECLARE @MaxSONo as int;          
          
SELECT @MaxSONo=MAX(SOSequence) FROM SO WHERE COMPANYID=@CompanyId and FinYearId=@FinYearid  and CompanyBranchId=@CompanyBranchId        
IF ISNULL(@MaxSONo,0)<>0          
BEGIN          
 SET @MaxSONo=@MaxSONo+1;          
END          
ELSE          
BEGIN          
 SET @MaxSONo=1;          
END          
          
set @SONo=@CompanyBranchCode + '/PI/'+ RIGHT(@FinYearCode,5) + '/' +  FORMAT(@MaxSONo,'000#');          
          
INSERT INTO SO(SONo,SODate,CompanyBranchId,CurrencyCode,QuotationId,QuotationNo,CustomerId,CustomerName,ContactPerson,      
BillingAddress,City,StateId,CountryId,PinCode,CSTNo,TINNo,PANNo,GSTNo,ExciseNo,      
Email,MobileNo,ContactNo,Fax,ConsigneeId,ConsigneeName,ApprovalStatus,     
ShippingContactPerson,ShippingBillingAddress,ShippingCity,ShippingStateId,ShippingCountryId,ShippingPinCode,      
ShippingTINNo,ShippingEmail,ShippingMobileNo,ShippingContactNo,ShippingFax,      
RefNo,RefDate,BasicValue,TotalValue,    FreightValue,LoadingValue,PayToBookId,Remarks1,Remarks2,      
FinYearId,CompanyId,CreatedBy,CreatedDate,SOSequence,ReverseChargeApplicable,ReverseChargeAmount,
InsuranceValue,FreightCGST_Amt,FreightSGST_Amt,FreightIGST_Amt,LoadingCGST_Amt,LoadingSGST_Amt,LoadingIGST_Amt,
InsuranceCGST_Amt,InsuranceSGST_Amt,InsuranceIGST_Amt,FreightCGST_Perc,FreightSGST_Perc,FreightIGST_Perc,
LoadingCGST_Perc,LoadingSGST_Perc,LoadingIGST_Perc,InsuranceCGST_Perc,InsuranceSGST_Perc,InsuranceIGST_Perc,
AdharcardNo,Pancard,IdtypeName,IdtypeValue,HypothecationBy,RtoRegsValue,RtoRegsCGST_Amt,
RtoRegsSGST_Amt,RtoRegsIGST_Amt,RtoRegsCGST_Perc,RtoRegsSGST_Perc,RtoRegsIGST_Perc,
VehicleInsuranceValue,LocationId)   
       
VALUES(@SONo,@SODate,@CompanyBranchId,@CurrencyCode,@QuotationId,@QuotationNo,@CustomerId,@CustomerName,      
@ContactPerson,@BillingAddress,@City,@StateId,@CountryId,@PinCode,@CSTNo,@TINNo,@PANNo,@GSTNo,@ExciseNo,      
@Email,@MobileNo,@ContactNo,@Fax,@ConsigneeId,@ConsigneeName,@ApprovalStatus,      
@ShippingContactPerson,@ShippingBillingAddress,@ShippingCity,@ShippingStateId,@ShippingCountryId,      
@ShippingPinCode,@ShippingTINNo,@ShippingEmail,@ShippingMobileNo,@ShippingContactNo,@ShippingFax,      
@RefNo,@RefDate,          
@BasicValue,@TotalValue,@FreightValue,@LoadingValue ,@PayToBookId,@Remarks1,@Remarks2,@FinYearId,@CompanyId,@CreatedBy,GETDATE(),@MaxSONo,
@ReverseChargeApplicable,@ReverseChargeAmount,
@InsuranceValue,@FreightCGST_Amt,@FreightSGST_Amt,@FreightIGST_Amt,@LoadingCGST_Amt,@LoadingSGST_Amt,@LoadingIGST_Amt,
@InsuranceCGST_Amt,@InsuranceSGST_Amt,@InsuranceIGST_Amt,@FreightCGST_Perc,@FreightSGST_Perc,@FreightIGST_Perc,
@LoadingCGST_Perc,@LoadingSGST_Perc,@LoadingIGST_Perc,@InsuranceCGST_Perc,@InsuranceSGST_Perc,@InsuranceIGST_Perc,
@AdharcardNo,@Pancard,@IdtypeName,@IdtypeValue,@HypothecationBy,
@RtoRegsValue,@RtoRegsCGST_Amt,@RtoRegsSGST_Amt,@RtoRegsIGST_Amt,@RtoRegsCGST_Perc,
@RtoRegsSGST_Perc,@RtoRegsIGST_Perc,@VehicleInsuranceValue,@LocationId)          
          
set @RetSOId=SCOPE_IDENTITY();           
          
insert into SOProductDetail(SOId,ProductId,ProductShortDesc,Price,Quantity,DiscountPercentage,          
DiscountAmount,TaxId,TaxName,TaxPercentage,TaxAmount,TotalPrice,  
SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,  
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,  
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3,
CGST_Perc,CGST_Amount,SGST_Perc,SGST_Amount,IGST_Perc,IGST_Amount,HSN_Code)          
select @RetSOId,ProductId,ProductShortDesc,Price,Quantity,DiscountPercentage,          
DiscountAmount,TaxId,TaxName,TaxPercentage,TaxAmount,TotalPrice,SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,  
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,  
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3 ,
CGST_Perc,CGST_Amount,SGST_Perc,SGST_Amount,IGST_Perc,IGST_Amount,HSN_Code
from @SOProductDetail          
        
        
insert into SOTaxDetail(SOId,TaxId,TaxName,TaxPercentage,TaxAmount,SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,  
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3 )          
select @RetSOId,TaxId,TaxName,TaxPercentage,TaxAmount,SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,  
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,  
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3         
from @SOTaxDetail         
        
insert into SOTermsDetail(SOId,TermDesc,TermSequence)          
select @RetSOId,TermDesc,TermSequence        
from @SOTermDetail  

insert into SOSupportingDocument(SOId,DocumentTypeId,DocumentName,DocumentPath)          
select @RetSOId,DocumentTypeId,DocumentName, DocumentPath    
from @SOSupportingDocument        
          
          
          
SET @message='';          
set @status='SUCCESS';          
          
          
           
END          
ELSE  -- MODIFY START          
BEGIN          
        
 update SO set SODate=@SODate,  
 CompanyBranchId=@CompanyBranchId, 
 LocationId=@LocationId,
 CurrencyCode=@CurrencyCode,  
 QuotationId=@QuotationId,          
 QuotationNo=@QuotationNo,          
 CustomerId=@CustomerId,          
 CustomerName=@CustomerName,       
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
ConsigneeId=@ConsigneeId,
ConsigneeName=@ConsigneeName,       
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
 FreightValue=@FreightValue,      
 LoadingValue=@LoadingValue,      
 PayToBookId=@PayToBookId,      
 Remarks1=@Remarks1,      
 Remarks2=@Remarks2,      
 ModifiedBy=@CreatedBy,          
 ModifiedDate=getdate(),
 ReverseChargeApplicable=@ReverseChargeApplicable,
 ReverseChargeAmount=@ReverseChargeAmount,
 InsuranceValue=@InsuranceValue,
 FreightCGST_Amt=@FreightCGST_Amt,
 FreightSGST_Amt=@FreightSGST_Amt,
 FreightIGST_Amt=@FreightIGST_Amt,
 LoadingCGST_Amt=@LoadingCGST_Amt,
 LoadingSGST_Amt=@LoadingSGST_Amt,
 LoadingIGST_Amt=@LoadingIGST_Amt,
 InsuranceCGST_Amt=@InsuranceCGST_Amt,
 InsuranceSGST_Amt=@InsuranceSGST_Amt,
 InsuranceIGST_Amt=@InsuranceIGST_Amt,
 FreightCGST_Perc=@FreightCGST_Perc,
 FreightSGST_Perc=@FreightSGST_Perc,
 FreightIGST_Perc=@FreightIGST_Perc,
 LoadingCGST_Perc=@LoadingCGST_Perc,
 LoadingSGST_Perc=@LoadingSGST_Perc,
 LoadingIGST_Perc=@LoadingIGST_Perc,
 InsuranceCGST_Perc=@InsuranceCGST_Perc,
 InsuranceSGST_Perc=@InsuranceSGST_Perc,
 InsuranceIGST_Perc=@InsuranceIGST_Perc   ,


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
 HypothecationBy=@HypothecationBy
        
where SOId=@SOId      
          
 delete from SOProductDetail where SOId=@SOId          
           
 insert into SOProductDetail(SOId,ProductId,ProductShortDesc,Price,Quantity,DiscountPercentage,DiscountAmount,TaxId,TaxName,TaxPercentage,TaxAmount,TotalPrice,SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,  
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,  
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3 ,CGST_Perc,CGST_Amount,SGST_Perc,SGST_Amount,IGST_Perc,IGST_Amount,HSN_Code)          
 select @SOId,ProductId,ProductShortDesc,Price,Quantity,DiscountPercentage,          
 DiscountAmount,TaxId,TaxName,TaxPercentage,TaxAmount,TotalPrice,  
 SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,  
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,  
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3 ,
CGST_Perc,CGST_Amount,SGST_Perc,SGST_Amount,IGST_Perc,IGST_Amount,HSN_Code           
 from @SOProductDetail          
          
 delete from SOTaxDetail where SOId=@SOId           
 insert into SOTaxDetail(SOId,TaxId,TaxName,TaxPercentage,TaxAmount)          
 select @SOId,TaxId,TaxName,TaxPercentage,TaxAmount        
 from @SOTaxDetail         
          
delete from SOTermsDetail where SOId=@SOId           
insert into SOTermsDetail(SOId,TermDesc,TermSequence)          
select @SOId,TermDesc,TermSequence        
from @SOTermDetail         
           
delete from SOSupportingDocument where SOId=@SOId      
insert into SOSupportingDocument(SOId,DocumentTypeId,DocumentName,DocumentPath)          
select @SOId,DocumentTypeId,DocumentName, DocumentPath    
from @SOSupportingDocument 
 
           
 SET @message='';          
 set @status='SUCCESS';          
 set @RetSOId=@SOId;           
           
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
set @RetSOId=0;          
END CATCH;          
end 
GO
GO
ALTER proc [dbo].[proc_GetSOs] --  proc_GetSOs '0','','','2022-01-04','2023-03-31',1,'0','0','',0 ,0   
(                
@SONo varchar(50),               
@CustomerName varchar(50),               
@RefNo varchar(50),             
@FromDate date,             
@ToDate date,            
@CompanyId int  ,  
@ApprovalStatus varchar(20)='',  
@DisplayType varchar(20)='',
@CreatedBy varchar(10),
@CompanyBranchId int=0,
@DashboardList varchar(25)='',
@CustomerId int =0,
@LocationId int =0
)                
as                
begin                
set nocount on;                
                
declare @strSql as nvarchar(4000);             
set @strSql='select E.SOId,            
E.SONo,                
replace(convert(varchar, E.SODate,106),'' '',''-'') SODate,                
E.CustomerId,            
E.CustomerName,      
C.CustomerCode,
E.TINNo CustomerGSTNo, 
isnull(C.City,'''') as City ,
isnull(S.StateName,'''') as StateName ,
E.ConsigneeId,       
E.ConsigneeName,
CN.CustomerCode ConsigneeCode,         
E.ShippingTINNo ConsigneeGSTNo,
E.QuotationNo,      
E.RefNo,      
case when E.RefDate is null or RefDate < ''1900-12-01'' then '''' else replace(convert(varchar, E.RefDate,106),'' '',''-'') end RefDate,  
E.BasicValue-((dbo.udf_GetTotalSOCGSTAmount(E.SOId) +dbo.udf_GetTotalSOSGSTAmount(E.SOId)+dbo.udf_GetTotalSOIGSTAmount(E.SOId))- dbo.udf_GetTotalSODiscountAmount(E.SOId)) as BasicAmt,            
E.BasicValue,      
E.TotalValue,       
E.CreatedBy, 
E.ApprovalStatus,          
cu.FullName CreatedByName,              
replace(convert(varchar, E.CreatedDate,106),'' '',''-'') CreatedDate,                
E.ModifiedBy,                 
isnull(mu.FullName,'''') ModifiedByName,              
case when E.ModifiedDate is null then '''' else replace(convert(varchar, E.ModifiedDate,106),'' '',''-'') end ModifiedDate,
isnull(E.CompanyBranchId,0)  CompanyBranchId,
isnull(cb.BranchName,'''')  CompanyBranchName,
isnull(E.HypothecationBy,'''') as HypothecationBy ,
isnull(E.LocationId,0) as LocationId,
ISNULL(LC.LocationName,'''') AS LocationName
from [SO] E      
inner join Customer C on E.CustomerId=C.CustomerId 
inner join Customer CN on E.ConsigneeId=CN.CustomerId
inner join State S on E.StateId=S.StateId
inner join [User] cu on E.createdBy=cu.UserId              
left join [User] mu on E.ModifiedBy=mu.Userid  
left join [ComapnyBranch] cb on E.CompanyBranchId = cb.CompanyBranchId
left join [Location] LC on ISNULL(E.LocationId,0)=ISNULL(LC.LocationId,0)
where  E.CompanyId='+ cast(@CompanyId as varchar) + ' ';                
set @strSql=@strSql + ' and  E.SODate BETWEEN '''+ cast(@FromDate as varchar) + ''' AND  '''+ cast(@ToDate as varchar) + ''' ';                
      
if @CustomerName<>''                
begin                
set @strSql=@strSql + ' and  E.CustomerName like ''%' + @CustomerName + '%'' ';                
end                
      
if @SONo<>''                
begin                
set @strSql=@strSql + ' and E.SONo like ''%' + @SONo + '%''';                
end        
if @RefNo<>''                
begin                
set @strSql=@strSql + ' and E.RefNo like ''%' + @RefNo + '%''';                
end     
if @DisplayType<>''  
begin                  
 set @strSql=@strSql + ' and E.SOId not in (Select SOId from SaleInvoice) ';                  
end 
if @CreatedBy<>''                
begin                
set @strSql=@strSql + ' and cu.FullName like ''%' + @CreatedBy + '%''';                
end

if @ApprovalStatus<>'0'                      
begin     
set @strSql=@strSql + ' and  E.ApprovalStatus = ''' + @ApprovalStatus + ''' ';                                       
end 
if @CompanyBranchId<>0                  
begin                  
set @strSql=@strSql + ' and E.CompanyBranchId =''' + cast(@CompanyBranchId as varchar) + '''';                  
end  

if @DashboardList<>''                  
begin                  
set @strSql=@strSql +' and E.SOId not in (Select SOId from WorkOrder) ';                  
end 

if @CustomerId<>0
begin
set @strSql=@strSql+' and CU.FK_CustomerId =''' + cast(@CustomerId as varchar) + '''';   	
end

if @LocationId<>0
begin
set @strSql=@strSql+' and E.LocationId =''' + cast(@LocationId as varchar) + '''';   	
end
                
set @strSql=@strSql + ' order by E.SONo Desc ';                
exec sp_executesql @strSql  
--select @strSql              
             
set nocount off;                
end            