
GO
/****** Object:  StoredProcedure [dbo].[proc_GetDispatchs]    Script Date: 6/29/2022 6:14:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- proc_GetDispatchs '','','2021-04-01','2022-03-31',1,0
  
ALTER proc [dbo].[proc_GetDispatchs]
(                      
@DispatchNo varchar(50),                     
@CustomerName varchar(50),                    
@FromDate date, 
@ToDate date,
@CompanyBranchId int=0,                                                            
@CustomerId int =0
)                      
as                      
begin                      
set nocount on;                      
                      
declare @strSql as nvarchar(4000);                   
set @strSql='select E.DispatchPlanID,                  
E.DispatchPlanNo,           
replace(convert(varchar, E.DispatchPlanDate,106),'' '',''-'') DispatchPlanDate,                          
E.CustomerId,            
c.CustomerCode,   
CB.BranchName,            
c.CustomerName,               
c.City,                   
St.StateName,                                                                   
E.CreatedBy,                                              
replace(convert(varchar, E.CreatedDate,106),'' '',''-'') CreatedDate,                      
E.ModifiedBy,                                         
case when E.ModifiedDate is null then '''' else replace(convert(varchar, E.ModifiedDate,106),'' '',''-'') end ModifiedDate,
isnull(E.CompanyBranchId,0)  CompanyBranchId,
isnull(cb.BranchName,'''')  CompanyBranchName
from [DispatchPlan] E                
inner join [Customer] c on E.CustomerId = c.CustomerId                       
inner join [State] St on c.StateId=St.StateId  
inner join [ComapnyBranch] cb on E.CompanyBranchId = cb.CompanyBranchId                   
where  1=1 ';                      
set @strSql=@strSql + ' and  E.DispatchPlanDate BETWEEN '''+ cast(@FromDate as varchar) + ''' AND  '''+ cast(@ToDate as varchar) + ''' ';                      
            
if @CustomerName<>''                      
begin                      
set @strSql=@strSql + ' and  c.CustomerName like ''%' + @CustomerName + '%'' ';                      
end   
                               
if @DispatchNo<>''                      
begin                      
set @strSql=@strSql + ' and E.DispatchPlanNo like ''%' + @DispatchNo + '%''';                      
end                            
                        
set @strSql=@strSql + ' order by E.DispatchPlanDate Desc, E.DispatchPlanNo Desc ';         
--print @strSql
exec sp_executesql @strSql 
--select @strSql                     
                      
set nocount off;                      
end   


--------------------------------------------------------------------------------------------------------------------------------------------------


GO
/****** Object:  StoredProcedure [dbo].[proc_AddEditReturn]    Script Date: 6/29/2022 11:19:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[proc_AddEditReturn]        
(        
@ReturnedID bigint,        
@ReturnedDate date,      
@InvoiceID bigint, 
@InvoiceNo varchar(50) = null,
@CompanyId int,   
@CompanyBranchId int,    
@FinYearId int,    
@CreatedBy int,     
@ApprovalStatus varchar(20), 
@Warranty varchar(50),   
@ReturnedProductDetail udt_ReturnedProductDetail readonly,  
 

@status varchar(50) out,        
@message varchar(500) out,        
@RetReturnId bigint out        
)        
as        
begin        
BEGIN TRY        
BEGIN TRANSACTION        
DECLARE @FinYearStartDate as date;              
DECLARE @FinYearEndDate as date;              
DECLARE @FinYearCode AS VARCHAR(20); 

declare @temp_ProductEntry as table    
(    
 RowId int,    
 ProductId bigint,    
 Quantity decimal(18,2)   
); 


declare @ChallanProductId bigint;    
declare @ChallanQuantity decimal(18,2);    
 

declare @RowCount int=0;    
declare @RowNum int; 
declare @JobWorkProductId bigint;    
declare @jobWorkQuantity decimal(18,2);    
declare @StockQuantity decimal(18,2); 
   
   
        
DECLARE @CompanyCode as varchar(10);    
DECLARE @CompanyBranchCode as varchar(10);   
DECLARE @CompanyStateId as int;

SELECT @FinYearStartDate=StartDate,@FinYearEndDate=EndDate,@FinYearCode=FinYearCode              
FROM FinancialYear WHERE FinYearId=@FinYearId                 

IF @ReturnedDate<@FinYearStartDate OR @ReturnedDate>@FinYearEndDate               
 BEGIN              
  SET @message='Return Date must be within selected financial year.';              
  set @status='FAIL';              
  set @RetReturnId=0;              
  RAISERROR(@message,16,1);              
 END    
         
SELECT @CompanyCode=CompanyCode from Company where CompanyId=@CompanyId        
SELECT @CompanyBranchCode=CB.CompanyBranchCode from ComapnyBranch CB 
where CB.CompanyBranchId=@CompanyBranchId  

IF @ReturnedID=0  -- INSERT START        
BEGIN        
        
DECLARE @ReturnNo AS VARCHAR(50);        
DECLARE @MaxReturnNo as int;        
        
SELECT @MaxReturnNo=MAX(ReturnedSequence) FROM [Return] WHERE COMPANYID=@CompanyId and CompanyBranchId=@CompanyBranchId
IF ISNULL(@MaxReturnNo,0)<>0        
BEGIN        
 SET @MaxReturnNo=@MaxReturnNo+1;        
END        
ELSE        
BEGIN        
 SET @MaxReturnNo=1;        
END        
set @ReturnNo=@CompanyBranchCode + '/RP/'+ RIGHT(@FinYearCode,5) +'/'+ FORMAT(@MaxReturnNo,'000#');        

        
INSERT INTO [Return](ReturnedNo,ReturnedDate,InvoiceID,InvoiceNo,FinYearId,CompanyId,CompanyBranchId,CreatedBy,
CreatedDate,ApprovalStatus,ReturnedSequence,Warranty)        
VALUES(@ReturnNo,@ReturnedDate,@InvoiceID,@InvoiceNo,@FinYearId,@CompanyId,
@CompanyBranchId,@CreatedBy,getDate(),@ApprovalStatus,@MaxReturnNo,@Warranty)        
set @RetReturnId=SCOPE_IDENTITY();         

insert into ReturnedProductDetail(ReturnedID,ProductId,Quantity,ReplacedQTY,ReturnedQty,Status,Remarks)        
select @RetReturnId,ProductId,Quantity,ReplacedQTY,ReturnedQty,Status,Remarks
from @ReturnedProductDetail 

 -------------------Update Stock IN ProductTrnStock---------------------------------------------------
  if(@ApprovalStatus='Final')
  begin
insert into @temp_ProductEntry(RowId,ProductId,Quantity)    
select row_number() over (order by  ProductId), ProductId,ReplacedQTY    
from @ReturnedProductDetail                  
--group by ProductId    

set @RowCount=(select Max(RowId) from @temp_ProductEntry);    
set @RowNum=1;    


set @StockQuantity=0;    
while (@RowNum<=@RowCount)    
begin    


 set @StockQuantity=0;    
     
 select @ChallanProductId=ProductId,@ChallanQuantity=Quantity     
 from @temp_ProductEntry where RowId=@RowNum
 ;    
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
  set @RetReturnId=0;                  
  RAISERROR(@message,16,1);                  
 END                  
         
 
 
 insert into ProductTrnStock(ProductId,CompanyId,TrnType,TrnDate,TrnQty,TrnReferenceNo,TrnReferenceDate,FromCompanyBranchId,ToCompanyBranchId)          
 select @ChallanProductId,@CompanyId,'RP',@ReturnedDate,-1*@ChallanQuantity,@RetReturnId,@ReturnedDate,@CompanyBranchId,0      
 from @temp_ProductEntry  where RowId=@RowNum 
 
   
 
  
   set @RowNum=@RowNum+1; 
end 

end
------------------------------------END CODE---------------------------------------------------
 
        
SET @message='';        
set @status='SUCCESS';        
         
END        
ELSE  -- MODIFY START        
BEGIN        
update [Return] set     
ReturnedDate=@ReturnedDate,
InvoiceID=@InvoiceID,
InvoiceNo=@InvoiceNo,
CompanyBranchId=@CompanyBranchId,

ModifiedBy=@CreatedBy,
ModifiedDate=GETDATE(),
ApprovalStatus=@ApprovalStatus,
Warranty=@Warranty
where ReturnedID=@ReturnedID       

delete from ReturnedProductDetail where ReturnedID=@ReturnedID
         
insert into ReturnedProductDetail(ReturnedID,ProductId,Quantity,ReplacedQTY,ReturnedQty,Status,Remarks)        
select @ReturnedID,ProductId,Quantity,ReplacedQTY,ReturnedQty,Status,Remarks
from @ReturnedProductDetail 

-------------------Update Stock IN ProductTrnStock---------------------------------------------------
  if(@ApprovalStatus='Final')
  begin
insert into @temp_ProductEntry(RowId,ProductId,Quantity)    
select row_number() over (order by  ProductId), ProductId,ReplacedQTY    
from @ReturnedProductDetail                  
--group by ProductId    

set @RowCount=(select Max(RowId) from @temp_ProductEntry);    
set @RowNum=1;    


set @StockQuantity=0;    
while (@RowNum<=@RowCount)    
begin    


 set @StockQuantity=0;    
     
 select @ChallanProductId=ProductId,@ChallanQuantity=Quantity     
 from @temp_ProductEntry where RowId=@RowNum
 ;    
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
  set @RetReturnId=0;                  
  RAISERROR(@message,16,1);                  
 END                  
         
 
 
 insert into ProductTrnStock(ProductId,CompanyId,TrnType,TrnDate,TrnQty,TrnReferenceNo,TrnReferenceDate,FromCompanyBranchId,ToCompanyBranchId)          
 select @ChallanProductId,@CompanyId,'RP',@ReturnedDate,-1*@ChallanQuantity,@ReturnedID,@ReturnedDate,@CompanyBranchId,0      
 from @temp_ProductEntry  where RowId=@RowNum 
 
   
 
  
   set @RowNum=@RowNum+1; 
end 

end
------------------------------------END CODE---------------------------------------------------

 
 SET @message='';        
 set @status='SUCCESS';        
 set @RetReturnId=@ReturnedID;         
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
set @RetReturnId=0;        
END CATCH;        
end  



