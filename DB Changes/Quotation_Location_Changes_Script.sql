use dms

alter table Quotation
add LocationId int

go

ALTER proc [dbo].[proc_AddEditQuotation]        
(        
@QuotationId bigint,        
@QuotationDate date,    
@CompanyBranchId int,    
@CurrencyCode varchar(50)='INR',        
@CustomerId int,        
@CustomerName varchar(100)='',        
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
@RefNo varchar(50)='',        
@RefDate date,        
@BasicValue decimal(18,2),    
@TotalValue decimal(18,2),      
@FreightValue decimal(18,2)=0,                  
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
@RtoRegsSGST_Amt decimal(18,2),                  
@RtoRegsIGST_Amt decimal(18,2)=0,                  
@RtoRegsCGST_Perc decimal(18,2)=0,                  
@RtoRegsSGST_Perc decimal(18,2)=0,                  
@RtoRegsIGST_Perc decimal(18,2)=0,                  
@VehicleInsuranceValue decimal(18,2)=0,                  



@ReverseChargeApplicable bit=0,  
@ReverseChargeAmount decimal(18,2) =0, 


         
@Remarks1 VARCHAR(2000),      
@Remarks2 VARCHAR(2000),      
      
@FinYearId int,        
@CompanyId int,        
@CreatedBy int,     
@ApprovalStatus varchar(20),   
@LocationId int ,
@QuotationProductDetail udt_QuotationProductDetail readonly,        
@QuotationTaxDetail udt_QuotationTaxDetail readonly,        
@QuotationTermDetail udt_QuotationTermDetail readonly,   
@QuotationSupportingDocument udt_QuotationSupportingDocument readonly,        
@status varchar(50) out,        
@message varchar(500) out,        
@RetQuotationId bigint out        
)        
as        
begin        
BEGIN TRY        
BEGIN TRANSACTION        
        
DECLARE @FinYearStartDate as date;        
DECLARE @FinYearEndDate as date;        
DECLARE @FinYearCode AS VARCHAR(20);        
DECLARE @CompanyCode as varchar(10);    
DECLARE @CompanyStateId as int;                  
DECLARE @StateCode as varchar(10);    
DECLARE @CompanyBranchCode as varchar(10);     
        
SELECT @CompanyCode=CompanyCode from Company where CompanyId=@CompanyId  
SELECT @CompanyBranchCode=CB.CompanyBranchCode from ComapnyBranch CB 
where CB.CompanyBranchId=@CompanyBranchId       
    
select @StateCode=st.StateCode from dbo.ComapnyBranch cb inner join [state] st       
on cb.StateId=st.StateId where cb.CompanyBranchId=@CompanyBranchId      
      
if Isnull(@StateCode,'')=''      
begin      
 set @StateCode='DL';      
end      
      
    
SELECT @FinYearStartDate=StartDate,@FinYearEndDate=EndDate,@FinYearCode=FinYearCode        
FROM FinancialYear WHERE FinYearId=@FinYearId        
        
IF @QuotationDate<@FinYearStartDate OR @QuotationDate>@FinYearEndDate         
 BEGIN        
  SET @message='Quotation Date must be within selected financial year.';        
  set @status='FAIL';        
  set @RetQuotationId=0;        
  RAISERROR(@message,16,1);        
 END        
        
        
IF @QuotationId=0  -- INSERT START        
BEGIN        
        
DECLARE @QuotationNo AS VARCHAR(50);        
DECLARE @MaxQuotationNo as int;        
        
SELECT @MaxQuotationNo=MAX(QuotationSequence) FROM Quotation WHERE COMPANYID=@CompanyId and FinYearId=@FinYearid and CompanyBranchId=@CompanyBranchId        
IF ISNULL(@MaxQuotationNo,0)<>0        
BEGIN        
 SET @MaxQuotationNo=@MaxQuotationNo+1;        
END        
ELSE        
BEGIN        
 SET @MaxQuotationNo=1;        
END        
    set @QuotationNo=@CompanyBranchCode + '/QU/'+ RIGHT(@FinYearCode,5) + '/' +  FORMAT(@MaxQuotationNo,'000#');        
--set @QuotationNo=@CompanyCode + '/QU/' + RIGHT(@FinYearCode,5) + '/' +  FORMAT(@MaxQuotationNo,'000#');        
        
INSERT INTO Quotation(QuotationNo,QuotationDate,CompanyBranchId,CurrencyCode,CustomerId,CustomerName,BillingAddress,City,        
StateId,CountryId,PinCode,CSTNo,TINNo,PANNo,GSTNo,ExciseNo,RefNo,RefDate,BasicValue,TotalValue,
FreightValue,FreightCGST_Perc,FreightCGST_Amt,  
FreightSGST_Perc,FreightSGST_Amt,FreightIGST_Perc,FreightIGST_Amt,  
LoadingValue,LoadingCGST_Perc,LoadingCGST_Amt,  
LoadingSGST_Perc,LoadingSGST_Amt,LoadingIGST_Perc,LoadingIGST_Amt,  
InsuranceValue,InsuranceCGST_Perc,InsuranceCGST_Amt,  
InsuranceSGST_Perc,InsuranceSGST_Amt,InsuranceIGST_Perc,InsuranceIGST_Amt, 
Remarks1,Remarks2,FinYearId,CompanyId,ApprovalStatus, CreatedBy,CreatedDate,QuotationSequence,ReverseChargeApplicable,ReverseChargeAmount,
 RtoRegsValue,RtoRegsCGST_Amt,RtoRegsSGST_Amt,RtoRegsIGST_Amt,RtoRegsCGST_Perc,RtoRegsSGST_Perc,RtoRegsIGST_Perc,VehicleInsuranceValue,LocationId)        
VALUES(@QuotationNo,@QuotationDate,@CompanyBranchId,@CurrencyCode,@CustomerId,@CustomerName,@BillingAddress,        
@City,@StateId,@CountryId,@PinCode,@CSTNo,@TINNo,@PANNo,@GSTNo,@ExciseNo,@RefNo,@RefDate,        
@BasicValue,@TotalValue,
@FreightValue,@FreightCGST_Perc,@FreightCGST_Amt,  
@FreightSGST_Perc,@FreightSGST_Amt,@FreightIGST_Perc,@FreightIGST_Amt,  
@LoadingValue,@LoadingCGST_Perc,@LoadingCGST_Amt,  
@LoadingSGST_Perc,@LoadingSGST_Amt,@LoadingIGST_Perc,@LoadingIGST_Amt,  
@InsuranceValue,@InsuranceCGST_Perc,@InsuranceCGST_Amt,  
@InsuranceSGST_Perc,@InsuranceSGST_Amt,@InsuranceIGST_Perc,@InsuranceIGST_Amt , 
@Remarks1,@Remarks2,@FinYearId,@CompanyId,@ApprovalStatus,@CreatedBy,GETDATE(),@MaxQuotationNo,@ReverseChargeApplicable,@ReverseChargeAmount,
@RtoRegsValue,@RtoRegsCGST_Amt,@RtoRegsSGST_Amt,@RtoRegsIGST_Amt,@RtoRegsCGST_Perc,@RtoRegsSGST_Perc,@RtoRegsIGST_Perc,@VehicleInsuranceValue, @LocationId )        
        
set @RetQuotationId=SCOPE_IDENTITY();         
        
insert into QuotationProductDetail(QuotationId,ProductId,ProductShortDesc,Price,Quantity,DiscountPercentage,        
DiscountAmount,TaxPercentage,TaxAmount,TotalPrice,TaxId,TaxName,    
SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,    
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,    
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3 ,  
CGST_Perc,CGST_Amount,SGST_Perc,      
SGST_Amount,IGST_Perc,IGST_Amount,HSN_Code )        
select @RetQuotationId,ProductId,ProductShortDesc,Price,Quantity,DiscountPercentage,        
DiscountAmount,TaxPercentage,TaxAmount,TotalPrice,TaxId,Taxname,    
SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,    
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,    
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3   ,  
CGST_Perc,CGST_Amount,SGST_Perc,      
SGST_Amount,IGST_Perc,IGST_Amount,HSN_Code  
 from @QuotationProductDetail        
      
      
insert into QuotationTaxDetail(QuotationId,TaxId,TaxName,TaxPercentage,TaxAmount,    
SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,    
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,    
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3 )        
select @RetQuotationId,TaxId,TaxName,TaxPercentage,TaxAmount,SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,    
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,    
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3       
from @QuotationTaxDetail       
      
insert into QuotationTermsDetail(QuotationId,TermDesc,TermSequence)        
select @RetQuotationId,TermDesc,TermSequence      
from @QuotationTermDetail       
   
insert into QuotationSupportingDocument(QuotationId,DocumentTypeId,DocumentName,DocumentPath)          
select @RetQuotationId,DocumentTypeId,DocumentName, DocumentPath    
from @QuotationSupportingDocument           
        
        
SET @message='';        
set @status='SUCCESS';        
        
        
         
END        
ELSE  -- MODIFY START        
BEGIN        
 update Quotation set     
 QuotationDate=@QuotationDate,    
 CompanyBranchId=@CompanyBranchId,    
 CurrencyCode=@CurrencyCode,        
 CustomerId=@CustomerId,        
 CustomerName=@CustomerName,        
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
  
 RtoRegsValue=@RtoRegsValue,
 RtoRegsCGST_Amt=@RtoRegsCGST_Amt,
 RtoRegsSGST_Amt=@RtoRegsSGST_Amt,
 RtoRegsIGST_Amt=@RtoRegsIGST_Amt,
 RtoRegsCGST_Perc=@RtoRegsCGST_Perc,
 RtoRegsSGST_Perc=@RtoRegsSGST_Perc,
 RtoRegsIGST_Perc=@RtoRegsIGST_Perc,
 VehicleInsuranceValue=@VehicleInsuranceValue,
   
 Remarks1=@Remarks1,    
 Remarks2=@Remarks2,       
 ModifiedBy=@CreatedBy,      
 ApprovalStatus=@ApprovalStatus,      
 ModifiedDate=getdate()  ,
 ReverseChargeApplicable=@ReverseChargeApplicable,  
 ReverseChargeAmount=@ReverseChargeAmount ,
 LocationId=@LocationId
 where QuotationId=@QuotationId        
        
 delete from QuotationProductDetail where QuotationId=@QuotationId        
 insert into QuotationProductDetail(QuotationId,ProductId,ProductShortDesc,Price,Quantity,DiscountPercentage,        
 DiscountAmount,TaxPercentage,TaxAmount,TotalPrice,TaxId,TaxName,SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,    
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,    
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3,  
CGST_Perc,CGST_Amount,SGST_Perc,      
SGST_Amount,IGST_Perc,IGST_Amount,HSN_Code)        
 select @QuotationId,ProductId,ProductShortDesc,Price,Quantity,DiscountPercentage,        
 DiscountAmount,TaxPercentage,TaxAmount,TotalPrice,TaxId,TaxName,    
 SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,    
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,    
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3 ,  
CGST_Perc,CGST_Amount,SGST_Perc,      
SGST_Amount,IGST_Perc,IGST_Amount,HSN_Code from @QuotationProductDetail        
        
 delete from QuotationTaxDetail where QuotationId=@QuotationId         
 insert into QuotationTaxDetail(QuotationId,TaxId,TaxName,TaxPercentage,TaxAmount,    
 SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,    
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,    
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3  )        
 select @QuotationId,TaxId,TaxName,TaxPercentage,TaxAmount,SurchargeName_1,SurchargePercentage_1,SurchargeAmount_1 ,    
SurchargeName_2,SurchargePercentage_2,SurchargeAmount_2 ,    
SurchargeName_3,SurchargePercentage_3,SurchargeAmount_3 from @QuotationTaxDetail       
        
delete from QuotationTermsDetail where QuotationId=@QuotationId         
insert into QuotationTermsDetail(QuotationId,TermDesc,TermSequence)        
select @QuotationId,TermDesc,TermSequence      
from @QuotationTermDetail       
         
  
delete from QuotationSupportingDocument where QuotationId=@QuotationId      
insert into QuotationSupportingDocument(QuotationId,DocumentTypeId,DocumentName,DocumentPath)          
select @QuotationId,DocumentTypeId,DocumentName, DocumentPath    
from @QuotationSupportingDocument            
         
         
         
 SET @message='';        
 set @status='SUCCESS';        
 set @RetQuotationId=@QuotationId;         
         
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
set @RetQuotationId=0;        
END CATCH;        
end  
go
ALTER proc [dbo].[proc_GetQuotationDetail] --proc_GetQuotationDetail 4                
(                      
@QuotationId int                    
)                      
as                      
begin                      
set nocount on;                    
select E.QuotationId,                  
E.QuotationNo,                      
replace(convert(varchar, E.QuotationDate,106),' ','-') QuotationDate,
isnull(E.CompanyBranchId,0) as CompanyBranchId , 
CASE WHEN E.QuotationNo LIKE '%/R%' THEN 'Revised Quotation' ELSE 'Quotation' END as QuotationType,      
E.CurrencyCode,       
E.CustomerId,                   
E.CustomerName,              
C.CustomerCode,          
C.ContactPersonName,        
C.MobileNo,        
C.ContactNo,        
C.Fax,        
C.Email,
C.GSTNo CustomerGSTNo,    
E.BillingAddress,                  
E.City,    
E.ApprovalStatus,                
E.StateId,              
CustState.StateName,
CustState.StateCode,        
E.CountryId,              
CustCountry.CountryName,        
E.PinCode,                 
E.CSTNo,              
E.TINNo,              
E.PANNo,              
E.GSTNo,              
E.ExciseNo,            
E.RefNo,     
    
case when E.RefDate is null or RefDate < '1900-12-01' then '' else replace(convert(varchar, E.RefDate,106),' ','-') end RefDate,                
E.BasicValue,          
E.TotalValue,   

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

ISNULL(E.RtoRegsValue,0) RtoRegsValue ,        
ISNULL(E.RtoRegsCGST_Amt,0) RtoRegsCGST_Amt,          
ISNULL(E.RtoRegsSGST_Amt,0) RtoRegsSGST_Amt,  
ISNULL(E.RtoRegsIGST_Amt,0) RtoRegsIGST_Amt,          
ISNULL(E.RtoRegsCGST_Perc,0) RtoRegsCGST_Perc,  
ISNULL(E.RtoRegsSGST_Perc,0) RtoRegsSGST_Perc,          
ISNULL(E.RtoRegsIGST_Perc,0) RtoRegsIGST_Perc,
ISNULL(E.VehicleInsuranceValue,0) VehicleInsuranceValue,

  
E.Remarks1,  
E.Remarks2,         
E.FinYearId,          
E.CompanyId,          
Com.CompanyName,        
cb.ContactPersonName,      
cb.ContactNo as CompanyPhone,        
cb.Email as CompanyEmail,        
cb.Fax as CompanyFax,        
com.Website as CompanyWebsite,        
cb.PrimaryAddress as CompanyAddress,        
cb.City as CompanyCity,        
cb.TINNo CompanyTINNo,     
cb.PinCode CompanyZipCode, 
isnull(cb.PANNo,'') CompanyPANNo,  
isnull(cb.GSTNo,'') CompanyGSTNo,  

   
       
com.CompanyDesc CompanyDesc, 
CompState.StateName as CompanyStateName,        
CompCountry.CountryName CompanyCountryName ,        
       

SaleEmp.EmployeeCode,        
SaleEmp.FirstName,        
SaleEmp.LastName,        
SaleEmp.Email as SaleEmpEmail,        
SaleEmp.MobileNo as SaleEmpMobile,        
EmpDesig.DesignationName as SaleEmpDesignation,        
Isnull(cb.MobileNo,'') CompayMobileNo,  
Isnull(cb.ContactNo,'') CompayContactNo,  
Isnull(cb.Fax,'') CompayOfficeNo,
isnull(CB.BranchType,'') as BranchType,  

  
E.CreatedBy,                  
cu.FullName CreatedByName,                    
replace(convert(varchar, E.CreatedDate,106),' ','-') CreatedDate,                      
E.ModifiedBy,                       
isnull(mu.FullName,'') ModifiedByName,                    
case when E.ModifiedDate is null then '' else replace(convert(varchar, E.ModifiedDate,106),' ','-') end ModifiedDate        ,
cast(isnull(E.ReverseChargeApplicable,0) as bit) as ReverseChargeApplicable,  
ISNULL(E.ReverseChargeAmount,0) AS ReverseChargeAmount,   
CustState.StateCode BillingStateCode,  
compState.StateCode CompanyStateCode,
isnull(E.LocationId,0) LocationId
from [Quotation] E                
inner join Customer C on E.CustomerId=C.CustomerId          
inner join [State] CustState on C.StateId=CustState.StateId        
inner join [Country] CustCountry on C.CountryId=CustCountry.CountryId        
inner join Company Com on E.CompanyId=Com.CompanyId        
inner join ComapnyBranch CB on E.CompanyBranchId=CB.CompanyBranchId        
inner join [State] compState on Cb.StateId=compState.StateId        
inner join [Country] compCountry on cb.CountryId=compCountry.CountryId     
left join Employee SaleEmp on C.EmployeeId=SaleEmp.EmployeeId        
left join Designation EmpDesig on SaleEmp.DesignationId=EmpDesig.DesignationId        
inner join [User] cu on E.createdBy=cu.UserId                    
left join [User] mu on E.ModifiedBy=mu.Userid                    
where  E.QuotationId=@QuotationId                
              
set nocount off;                      
end 

GO

--proc_GetQuotations '','','','2021-04-01','2022-03-31',1 ,'','',1 ,3                    
ALTER proc [dbo].[proc_GetQuotations] 
(						
@QuotationNo varchar(50),                   
@CustomerName varchar(50),                   
@RefNo varchar(50),                 
@FromDate date,                 
@ToDate date,                
@CompanyId int,    
@DisplayType varchar(50)='',
@ApprovalStatus varchar(50)='',
@CompanyBranchId int=0      ,
@CustomerId int =0,
@LocationId int=0
)                    
as                    
begin                    
set nocount on;                    
                    
declare @strSql as nvarchar(4000);                 
set @strSql='select E.QuotationId,                
E.QuotationNo,                    
replace(convert(varchar, E.QuotationDate,106),'' '',''-'') QuotationDate,                    
E.CustomerId,        
E.CustomerName,             
Cust.CustomerCode,            
E.City,                
St.StateName,               
E.RefNo,    
E.ApprovalStatus,        
case when E.RefDate is null or RefDate < ''1900-12-01'' then '''' else replace(convert(varchar, E.RefDate,106),'' '',''-'') end RefDate,                
E.BasicValue,          
E.TotalValue,          
ISNULL(E.QuotationRevisedStatus,0)  QuotationRevisedStatus,      
E.CreatedBy,                
cu.FullName CreatedByName,                  
replace(convert(varchar, E.CreatedDate,106),'' '',''-'') CreatedDate,                    
E.ModifiedBy,                     
isnull(mu.FullName,'''') ModifiedByName,                  
case when E.ModifiedDate is null then '''' else replace(convert(varchar, E.ModifiedDate,106),'' '',''-'') end ModifiedDate,
isnull(E.CompanyBranchId,0)  CompanyBranchId,
isnull(cb.BranchName,'''')  CompanyBranchName,
isnull(lc.LocationName,'''') LocationName,
isnull(E.LocationId,0) as LocationId
from [Quotation] E              
inner join [State] St on E.StateId=St.StateId               
inner join [User] cu on E.createdBy=cu.UserId                  
left join [User] mu on E.ModifiedBy=mu.Userid                  
inner join customer cust on E.CustomerId=Cust.CustomerId  
left join [ComapnyBranch] cb on E.CompanyBranchId = cb.CompanyBranchId
left join [Location] lc on isnull(E.LocationId,0)=isnull(lc.LocationId,0)     
where  E.CompanyId='+ cast(@CompanyId as varchar) + ' ';                    
set @strSql=@strSql + ' and  E.QuotationDate BETWEEN '''+ cast(@FromDate as varchar) + ''' AND  '''+ cast(@ToDate as varchar) + ''' ';                    
          
if @CustomerName<>''                    
begin                    
set @strSql=@strSql + ' and  E.CustomerName like ''%' + @CustomerName + '%'' ';                    
end                    
          
if @QuotationNo<>''                    
begin                    
set @strSql=@strSql + ' and E.QuotationNo like ''%' + @QuotationNo + '%''';                    
end                 
          
if @RefNo<>''                    
begin                    
set @strSql=@strSql + ' and E.RefNo like ''%' + @RefNo + '%''';                    
end                 
if @DisplayType<>''    
begin                    
 set @strSql=@strSql + ' and E.QuotationId not in (Select QuotationId from SO) and isnull(E.QuotationRevisedStatus,0)=0 ';                    
end                 
 
 if @ApprovalStatus<>'0'                    
begin   
set @strSql=@strSql + ' and  E.ApprovalStatus = ''' + @ApprovalStatus + ''' ';                                     
end 
if @CompanyBranchId<>0                  
begin                  
set @strSql=@strSql + ' and E.CompanyBranchId =''' + cast(@CompanyBranchId as varchar) + '''';                  
end  

if @CustomerId<>0
begin
set @strSql=@strSql+' and CU.FK_CustomerId =''' + cast(@CustomerId as varchar) + '''';   	
end

if @LocationId<>0
begin
set @strSql=@strSql+' and isnull(E.LocationId,0) =''' + cast(@LocationId as varchar) + '''';   	
end
       
set @strSql=@strSql + ' order by E.QuotationDate Desc ';   
print @strSql
exec sp_executesql @strSql
 -- select @strSql                  
set nocount off;                    
end 