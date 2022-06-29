alter table WorkOrder
add LocationId int
go


-- select  dbo.udf_GetProductBalanceQTY(10034)

ALTER Function [dbo].[udf_GetProductBalanceQTY](
 @FinYearid int,
 @CompanyId int, 
 @CompanyBranchId int ,
 @ProductId bigint,
 @FinYearStartDate date,
 @FinYearEndDate date,
 @LocationId int =1
 ) 
RETURNS  decimal(10,2)    
Begin

--This function use in this sp [proc_GetProductAvailableStock]
--This function use in this sp [proc_AddEditSaleInvoice]
--This function use in this sp [proc_GetProductAvailableStockBranchWise]
--This function use in this sp [proc_CancelMRNPO]


 Declare @totalQuantity decimal(10,2)  

 if(@CompanyBranchId>0)
 begin
   set @totalQuantity=isnull((select pos.OpeningQty from dbo.ProductOpeningStock pos     
		 where pos.FinYearid=@FinYearid and  pos.CompanyId=@CompanyId and      
		 pos.CompanyBranchId=@CompanyBranchId and pos.ProductId=@ProductId
		 and LocationId=@LocationId
		 ),0)    
		 +    
		 Isnull((SELECT sum(pts.TrnQty) from      
		 ProductTrnStock PTS  
		 where pts.CompanyId=@CompanyId 
		 and pts.ToCompanyBranchId=@CompanyBranchId 
		 and pts.ProductId=@ProductId 
		 and pts.TrnDate between  @FinYearStartDate and @FinYearEndDate),0)    
		 +    
		 Isnull((SELECT sum(pts.TrnQty) from  ProductTrnStock PTS      
		 where pts.CompanyId=@CompanyId 
		 and pts.FromCompanyBranchId=@CompanyBranchId 
		 and pts.ProductId=@ProductId    
		 and pts.TrnDate 
		 between  @FinYearStartDate and @FinYearEndDate),0) 

		 -
		 Isnull((SELECT sum(pts.TrnQty) from  ProductTrnStock PTS      
		 where pts.CompanyId=@CompanyId 
		 and pts.FromCompanyBranchId<>@CompanyBranchId 
		 and pts.ToCompanyBranchId=@CompanyBranchId 
		 and pts.ProductId=@ProductId    
		 and pts.trntype = 'STN' 
		 and pts.TrnDate between  @FinYearStartDate and @FinYearEndDate),0) 
		 




 end
 else
 begin
         set @totalQuantity=isnull((select sum(pos.OpeningQty) from dbo.ProductOpeningStock pos     
		 where pos.FinYearid=@FinYearid and  pos.CompanyId=@CompanyId and      
		  pos.ProductId=@ProductId),0)    
		 +    
		 Isnull((SELECT sum(pts.TrnQty) from      
		 ProductTrnStock PTS  
		 where pts.CompanyId=@CompanyId  
		 and pts.ProductId=@ProductId    
		 and pts.TrnDate 
		 between  @FinYearStartDate and @FinYearEndDate),0)    
		 
 end
  
  return  Isnull(@totalQuantity,0)   
End

go

ALTER proc [dbo].[proc_GetWorkOrderDetail] --proc_GetWorkOrderDetail 3                
(                      
@WorkOrderId int                    
)                      
as                      
begin                      
set nocount on;                    
select E.WorkOrderId,                  
E.WorkOrderNo,                      
replace(convert(varchar, E.WorkOrderDate,106),' ','-') WorkOrderDate,     
replace(convert(varchar, E.TargetFromDate,106),' ','-') TargetFromDate,     
replace(convert(varchar, E.TargetToDate,106),' ','-') TargetToDate,  
isnull(E.SOId,0) as SOId ,
ISNULL(E.SONo,'') as SONo,
case when so.SODate is null then '' else replace(convert(varchar, so.SODate,106),' ','-') end SODate ,    
E.CompanyBranchId,  
CB.BranchName,
CB.GSTNo as CompanyBranchGSTNo,
CB.MobileNo as CompanyBranchMobileNo,
CB.Fax as CompanyBranchFax,
Com.CompanyName,    
com.Phone as CompanyPhone,    
com.Email as CompanyEmail,    
com.Fax as CompanyFax,    
com.Website as CompanyWebsite,    
com.Address as CompanyAddress,    
com.City as CompanyCity,
com.CompanyDesc CompanyDesc, 
com.TanNo CompanyGSTNo,
E.Remarks1,  
E.Remarks2,         
E.CompanyId,          
E.CreatedBy,                  
cu.FullName CreatedByName,                    
replace(convert(varchar, E.CreatedDate,106),' ','-') CreatedDate,                      
E.ModifiedBy,                       
isnull(mu.FullName,'') ModifiedByName,                    
case when E.ModifiedDate is null then '' else replace(convert(varchar, E.ModifiedDate,106),' ','-') end ModifiedDate        ,
E.WorkOrderStatus,
isnull(E.LocationId,0) as LocationId 
from [WorkOrder] E                
inner join [User] cu on E.createdBy=cu.UserId 
left join SO so on E.SOId=so.SOId                    
left join [User] mu on E.ModifiedBy=mu.Userid  
Left join [ComapnyBranch] CB on E.CompanyBranchId=CB.CompanyBranchId  
inner join [Company] Com on E.CompanyId=Com.CompanyId                
where  E.WorkOrderId=@WorkOrderId
              
set nocount off;                      
end 



        
		


