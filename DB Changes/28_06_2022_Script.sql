USE [DMS]
GO
/****** Object:  StoredProcedure [dbo].[proc_GetSINRequisitionProducts]    Script Date: 27-06-2022 21:54:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--[proc_GetSINRequisitionProducts] 3,2018
ALTER proc [dbo].[proc_GetSINRequisitionProducts] 
(              
@RequisitionId bigint,
@FinYear int     
)              
as              
begin  
declare @StartDate as date;
declare @EndDate as date; 
declare @CompanyID as int; 
declare @CompanyBranchId as bigint; 
declare @LocationId as int;

select @StartDate=StartDate,@EndDate=EndDate from financialyear
where finyearid= @FinYear  
select @CompanyID=CompanyId,@CompanyBranchId=CompanyBranchId 
, @LocationId=ISNULL(LocationId,0)
from StoreRequisition where RequisitionId=@RequisitionId    
          
set nocount on;              
select 
ROW_NUMBER() OVER(ORDER BY E.ProductId ASC) AS SNo,
0 SINProductDetailId,   
E.ProductId,         
P.ProductName,          
P.ProductCode,          
isnull(P.ProductShortDesc,'') as ProductShortDesc,
U.UOMName,
E.Quantity,
ISNULL(E.IssuedQuantity,0) IssuedQuantity,
E.Quantity-ISNULL(E.IssuedQuantity,0) BalanceQuantity,
0 as IssueQuantity,
---- IndentQuantity  used for Raise Indent discuss with Hari 
isnull(E.IndentQuantity,0) IndentQuantity,
------- used for available stock  added 12 Oct 2017 

--------Start Code Dheeraj Get Product QTY.


dbo.udf_GetProductBalanceQTY(@FinYear,@CompanyId , 
       @CompanyBranchId,E.ProductId , @StartDate,
       @EndDate,@LocationId )  as AvailableStock

---------End Code---------------

---------Commented Below Code By Dheeraj As Discussino Hari Sir
---------Call GLobal Function Get Product Quantity
---------Start Comment Code---


--(select isnull((select pos.OpeningQty from dbo.ProductOpeningStock pos   
-- where pos.FinYearid=@FinYear and 
--  pos.CompanyId=(select CompanyId from StoreRequisition where RequisitionId=@RequisitionId ) and    
-- pos.CompanyBranchId=(select CompanyBranchId from StoreRequisition where RequisitionId=@RequisitionId ) 
-- and pos.ProductId=E.ProductId),0)  
-- +  
-- Isnull((SELECT sum(pts.TrnQty) from    
-- ProductTrnStock PTS  where pts.TrnType IN ('MRN','STR','FAB','JWM','FG','SR','PHYStkIN','ISS') AND    
-- pts.CompanyId=(select CompanyId from StoreRequisition where RequisitionId=@RequisitionId )and    
-- pts.ToCompanyBranchId=(select CompanyBranchId from StoreRequisition where RequisitionId=@RequisitionId )
-- and pts.ProductId=E.ProductId  
-- and pts.TrnDate between  @StartDate and @EndDate),0)  
-- +  
-- Isnull((SELECT sum(pts.TrnQty) from  ProductTrnStock PTS    
-- where pts.TrnType IN ('DC','STN','ISS','JW','PR','PHYStkOut') AND    
-- pts.CompanyId=(select CompanyId from StoreRequisition where RequisitionId=@RequisitionId ) and    
-- pts.FromCompanyBranchId=(select CompanyBranchId from StoreRequisition where RequisitionId=@RequisitionId ) 
-- and pts.ProductId=E.ProductId  
-- and pts.TrnDate between   @StartDate and @EndDate) ,0)
--  )AvailableStock



---------END Comment Code---
---------Commented Below Code By Dheeraj As Discussino Hari Sir
---------Call GLobal Function Get Product Quantity


------- end here
from [StoreRequisitionProductDetail] E        
inner join [Product] P on E.ProductId=P.ProductId         
left join uom u on P.UOMId=u.UOMId
where  E.RequisitionId=@RequisitionId and E.Quantity-ISNULL(E.IssuedQuantity,0)>0 order by P.ProductName asc
   
set nocount off;              
end 

go

ALTER proc [dbo].[proc_AddEditWorkOrder]        
(        
@WorkOrderId bigint,        
@WorkOrderDate date,  
@TargetFromDate date,    
@TargetToDate date,      
@CompanyId int,   
@CompanyBranchId int,    
@Remarks1 VARCHAR(2000)='',      
@Remarks2 VARCHAR(2000)='',      
@CreatedBy int,  
@SOId bigint,
@SONo varchar(100)='',   
@WorkOrderStatus varchar(20), 
@LocationId int,
@WorkOrderProductDetail udt_WorkOrderProductDetail readonly,        
@status varchar(50) out,        
@message varchar(500) out,        
@RetWorkOrderId bigint out        
)        
as        
begin        
BEGIN TRY        
BEGIN TRANSACTION        

declare @temp_ProductEntry as table    
(    
 RowId int,    
 AssemblyId bigint,    
 Quantity decimal(18,2)    
);   

declare @RowCount int=0;    
declare @RowNum int; 

declare @temp_Product as table    
(    
 RowId int,    
 ProductId bigint,    
 Quantity decimal(18,2)    
);  
declare @RowCountProduct int=0;    
declare @RowNumProduct int;   

declare @AssemblyProductId bigint;    
declare @AssemblyQuantity decimal(18,2);    
declare @AssemblyType varchar(50);
DECLARE @AssemblyCode varchar(50);
declare @CompanyBranchCode varchar(50);
declare @CompanyBranchCodeNew varchar(50);
declare @QuantityCounter decimal(18,2); 
----------For insert Data Requisition Table-------------------------
DECLARE @RequisitionNo AS VARCHAR(50);        
DECLARE @MaxRequisitionNo as int; 
DECLARE @StateCode as varchar(10);  
DECLARE @RetRequisitionId  as bigint;
DECLARE @FinYearCode AS VARCHAR(20); 
DECLARE @FinYearStartDate as date;        
DECLARE @FinYearEndDate as date;
---------End Code---------------------------------------------------   



declare @AssemblySerialNo as varchar(50);
declare @MaxAssemblySerialNo as int;


declare @BOMProductId bigint;    
declare @BOMQuantity decimal(18,2);    
        
DECLARE @CompanyCode as varchar(10);    
DECLARE @CompanyStateId as int;
DECLARE @FinYearID as int;

select @FinYearID= FinYearID from FinancialYear FY
where CONVERT(date, getdate() ) between FY.StartDate and FY.EndDate             
        
SELECT @CompanyCode=CompanyCode from Company where CompanyId=@CompanyId        
SELECT @CompanyBranchCode=isnull(CompanyBranchCode,'VE') from ComapnyBranch where CompanyBranchId=@CompanyBranchId

SELECT @CompanyBranchCodeNew=isnull(CompanyBranchCode,'VE') from ComapnyBranch where CompanyBranchId=@CompanyBranchId

IF @WorkOrderId=0  -- INSERT START        
BEGIN        
        
DECLARE @WorkOrderNo AS VARCHAR(50);        
DECLARE @MaxWorkOrderNo as int;        
        
SELECT @MaxWorkOrderNo=MAX(WorkOrderSequence) FROM WorkOrder WHERE COMPANYID=@CompanyId and CompanyBranchId=@CompanyBranchId
IF ISNULL(@MaxWorkOrderNo,0)<>0        
BEGIN        
 SET @MaxWorkOrderNo=@MaxWorkOrderNo+1;        
END        
ELSE        
BEGIN        
 SET @MaxWorkOrderNo=1;        
END        
    set @WorkOrderNo=@CompanyBranchCodeNew + '/WO/'  + FORMAT(@MaxWorkOrderNo,'000#');        
     
        
		INSERT INTO WorkOrder(WorkOrderNo,WorkOrderDate,SOId,SONo,TargetFromDate,TargetToDate,
		CompanyId,CompanyBranchId,        
		Remarks1,Remarks2,CreatedBy,CreatedDate,WorkOrderStatus, WorkOrderSequence,LocationId)        
		VALUES(@WorkOrderNo,@WorkOrderDate,@SOId,@SONo, @TargetFromDate,@TargetToDate,
		@CompanyId,@CompanyBranchId,@Remarks1,@Remarks2,@CreatedBy,GETDATE(),@WorkOrderStatus,@MaxWorkOrderNo, @LocationId)        
        
		set @RetWorkOrderId=SCOPE_IDENTITY();         
        
		       

		insert into WorkOrderBOMDetail(WorkOrderId,AssemblyId,ProductId,BOMQty,ProcessType,ScrapPercentage,PurchasePrice,TotalValue)
		select @RetWorkOrderId,wpd.ProductId as AssemblyId,pb.ProductId,wpd.Quantity*pb.BOMQty as BOMQty,
		isnull(pb.ProcessType,'') ProcessType ,isnull(pb.ScrapPercentage,0) ScrapPercentage 
		,p.PurchasePrice,(wpd.Quantity*pb.BOMQty*p.PurchasePrice) as TotalValue
		from @WorkOrderProductDetail WPD
		inner join ProductBOM PB on wpd.ProductId=pb.assemblyId
		inner join Product P on PB.ProductId= P.Productid
		where pb.Status=1

		insert into WorkOrderProductDetail(WorkOrderId,ProductId,Quantity,BOMCost,TotalBOMCost)        
		select @RetWorkOrderId,ProductId,Quantity,
		(Select Sum(isnull(TotalValue,0))/isnull(wopd.Quantity,0) from WorkOrderBOMDetail bom where bom.AssemblyId = wopd.ProductId )  BOMCost,
        (Select Sum(isnull(TotalValue,0))/isnull(wopd.Quantity,0) from WorkOrderBOMDetail bom where bom.AssemblyId = wopd.ProductId ) * wopd.Quantity as TotalBOMCost
		from @WorkOrderProductDetail wopd 

if @WorkOrderStatus='FINAL'
BEGIN
      
insert into @temp_ProductEntry(RowId,AssemblyId,Quantity)    
select row_number() over (order by  ProductId), ProductId,Sum(Quantity)    
from @WorkOrderProductDetail                  
group by ProductId  

set @RowCount=(select Max(RowId) from @temp_ProductEntry);    
set @RowNum=1; 

while (@RowNum<=@RowCount)    
begin    
      
 select @AssemblyProductId=AssemblyId,@AssemblyQuantity=Quantity,
 @AssemblyType=p.AssemblyType,@AssemblyCode=p.ProductCode
 from @temp_ProductEntry tpe inner join Product P on tpe.AssemblyId=p.Productid
 where tpe.RowId=@RowNum;    

set @AssemblyQuantity=isnull(@AssemblyQuantity,0);

IF @AssemblyType='SA'
BEGIN
        
		select @CompanyBranchCode=left(City,1) from ComapnyBranch 
		where CompanyBranchId=@CompanyBranchId	

		SELECT @MaxAssemblySerialNo =MAX(ProductSerialSequence) FROM 
		WorkOrder WO INNER JOIN WorkOrderProductSerialDetail WPSD ON WO.WorkOrderId=WPSD.WorkOrderId
		WHERE WO.COMPANYID=@CompanyId and WO.CompanyBranchId=@CompanyBranchId
		AND WPSD.ProductId=@AssemblyProductId
		AND YEAR(WO.WorkOrderDate)=YEAR(@WorkOrderDate);
		

		set @QuantityCounter=1;    
	 	
		while  @QuantityCounter<=@AssemblyQuantity    
		begin    
			IF ISNULL(@MaxAssemblySerialNo,0)<>0        
			BEGIN        
			 SET @MaxAssemblySerialNo=@MaxAssemblySerialNo+1;        
			END        
			ELSE        
			BEGIN        
			 SET @MaxAssemblySerialNo=1;        
			END        
		
			set @AssemblySerialNo='M16VECB' +  format(month(@WorkOrderDate),'0#') + @AssemblyCode + right(cast(year(@WorkOrderDate) as varchar),2) + FORMAT(@MaxAssemblySerialNo,'000#');        


   
     
   insert into WorkOrderProductSerialDetail(WorkOrderId,ProductId,ProductSerialNo,ProductSerialStatus,ProductSerialRemarks,ProductSerialSequence)            
   values(@RetWorkOrderId,@AssemblyProductId,@AssemblySerialNo,'NEW','',@MaxAssemblySerialNo)    
       
   set @QuantityCounter=@QuantityCounter+1;    
  end    
END
 set @RowNum=@RowNum+1;    
end    

END


------------------Inerst data in StoreRequisition table and StoreRequisitionProductDetail Table By Dheeraj Kumar Date---18/12/2017---------------------------

if @WorkOrderStatus='FINAL'

     begin

				
			IF @WorkOrderDate<@FinYearStartDate OR @WorkOrderDate>@FinYearEndDate         
			 BEGIN        
			      SET @message='Work Order Date must be within selected financial year.';        
					  set @status='FAIL';        
					  set @RetWorkOrderId=0;        
					  RAISERROR(@message,16,1);        
			 END 
							select @StateCode=st.StateCode from dbo.ComapnyBranch cb inner join [state] st       
							on cb.StateId=st.StateId where cb.CompanyBranchId=@CompanyBranchId     
        
							SELECT @FinYearStartDate=StartDate,@FinYearEndDate=EndDate,@FinYearCode=FinYearCode        
							FROM FinancialYear WHERE FinYearId=@FinYearID
							

							SELECT @MaxRequisitionNo=MAX(RequisitionSequence) FROM StoreRequisition WHERE COMPANYID=@CompanyId and CompanyBranchId=@CompanyBranchId     
							IF ISNULL(@MaxRequisitionNo,0)<>0        
							BEGIN      
							 SET @MaxRequisitionNo=@MaxRequisitionNo+1;        
							END        
							ELSE        
							BEGIN        
							 SET @MaxRequisitionNo=1;        
							END        
							set @RequisitionNo=@CompanyBranchCodeNew + '/SR/'+ RIGHT(@FinYearCode,5) + '/' +  FORMAT(@MaxRequisitionNo,'000#');        
				
				
----------------------------Insert Data StoreRequisition Table ------------------------------------------------------------------------------------------------

					INSERT INTO StoreRequisition(RequisitionNo,RequisitionDate,FinYearId,RequisitionType,CompanyId,
					CompanyBranchId,RequisitionByUserId,CustomerId,CustomerBranchId,LocationId,Remarks1,Remarks2,CreatedBy,
					CreatedDate,RequisitionStatus,ApprovalStatus,ApprovedBy,ApprovedDate,RequisitionSequence,WorkOrderId,WorkOrderNo) 
					VALUES(@RequisitionNo,@WorkOrderDate,@FinYearID,'PO',@CompanyId,
					@CompanyBranchId,@LocationId,0,0,0,@Remarks1,@Remarks2,@CreatedBy,
					GETDATE(),@WorkOrderStatus,'Approved',@CreatedBy,GETDATE(),@MaxRequisitionNo,@RetWorkOrderId,@WorkOrderNo)

----------------------------End Code -------------------------------------------------------------------------------------------------------------------------
				    set @RetRequisitionId=SCOPE_IDENTITY();

---------------------------Insert Data StoreRequisitionProductDetail Table --------------------------------------------------------------------------------------
								insert into @temp_Product(RowId,ProductId,Quantity)    
								select row_number() over (order by  PB.ProductId),PB.ProductId,Sum(wpd.Quantity*pb.BOMQty) as BOMQty   
								from @WorkOrderProductDetail WPD
								inner join ProductBOM PB on WPD.ProductId=pb.assemblyId 
								where pb.Status=1               
								group by PB.ProductId 
								set @RowCountProduct=(select Max(RowId) from @temp_Product);  
								set @RowNumProduct=1; 
						   				
						while (@RowNumProduct<=@RowCountProduct)  
						begin  						   						

						--select @rowID=RowId from @temp_Product where RowId=@RowNumProduct

						 insert into StoreRequisitionProductDetail(RequisitionId,ProductId,ProductShortDesc,Quantity,IssuedQuantity)        				
				         select @RetRequisitionId,ProductId,'Work Order',Quantity,0	 
				         from @temp_Product WPD

						 set @RowNumProduct=@RowCountProduct+1;

						 end

				
end
-----------------------------------------End Code--------------------------------------------------------------------------------------------
        
SET @message='';        
set @status='SUCCESS';        
        
        
         
END        
ELSE  -- MODIFY START        
BEGIN        
 update WorkOrder set     
 WorkOrderDate=@WorkOrderDate,    
 TargetFromDate=@TargetFromDate,    
 TargetToDate=@TargetToDate,        
 Remarks1=@Remarks1,    
 Remarks2=@Remarks2,       
 ModifiedBy=@CreatedBy,      
 WorkOrderStatus=@WorkOrderStatus,
 SOId=@SOId,
 SONo=@SONo,
 CompanyBranchId=@CompanyBranchId,       
 ModifiedDate=getdate() ,
 LocationId=@LocationId
 where WorkOrderId=@WorkOrderId
        
 delete from WorkOrderProductDetail where WorkOrderId=@WorkOrderId        
         
     

delete from WorkOrderBOMDetail where WorkOrderId=@WorkOrderId           
insert into WorkOrderBOMDetail(WorkOrderId,AssemblyId,ProductId,BOMQty,ProcessType,ScrapPercentage,PurchasePrice,TotalValue)
select @WorkOrderId,wpd.ProductId as AssemblyId,pb.ProductId,wpd.Quantity*pb.BOMQty as BOMQty,
isnull(pb.ProcessType,'') ProcessType ,isnull(pb.ScrapPercentage,0) ScrapPercentage ,p.PurchasePrice,(wpd.Quantity*pb.BOMQty*p.PurchasePrice) as TotalValue
from @WorkOrderProductDetail WPD
inner join ProductBOM PB on wpd.ProductId=pb.assemblyId
inner join Product P on PB.ProductId= P.Productid
where pb.Status=1       

insert into WorkOrderProductDetail(WorkOrderId,ProductId,Quantity,BOMCost, TotalBOMCost)        
select @WorkOrderId,ProductId,Quantity,
(Select Sum(isnull(TotalValue,0))/isnull(wopd.Quantity,0) from WorkOrderBOMDetail bom where bom.AssemblyId = wopd.ProductId )  BOMCost,
(Select Sum(isnull(TotalValue,0))/isnull(wopd.Quantity,0) from WorkOrderBOMDetail bom where bom.AssemblyId = wopd.ProductId ) * wopd.Quantity as TotalBOMCost
from @WorkOrderProductDetail  wopd

DELETE FROM WorkOrderProductSerialDetail where WorkOrderId=@WorkOrderId ;
if @WorkOrderStatus='FINAL'
BEGIN
      
insert into @temp_ProductEntry(RowId,AssemblyId,Quantity)    
select row_number() over (order by  ProductId), ProductId,Sum(Quantity)    
from @WorkOrderProductDetail                  
group by ProductId  

set @RowCount=(select Max(RowId) from @temp_ProductEntry);    
set @RowNum=1; 

while (@RowNum<=@RowCount)    
begin    
      
 select @AssemblyProductId=AssemblyId,@AssemblyQuantity=Quantity,
 @AssemblyType=p.AssemblyType,@AssemblyCode=p.ProductCode
 from @temp_ProductEntry tpe inner join Product P on tpe.AssemblyId=p.Productid
 where tpe.RowId=@RowNum;    

set @AssemblyQuantity=isnull(@AssemblyQuantity,0);

IF @AssemblyType='SA'
BEGIN
        
		select @CompanyBranchCode=CompanyBranchCode from ComapnyBranch 
		where CompanyBranchId=@CompanyBranchId

		SELECT @MaxAssemblySerialNo =MAX(ProductSerialSequence) FROM 
		WorkOrder WO INNER JOIN WorkOrderProductSerialDetail WPSD ON WO.WorkOrderId=WPSD.WorkOrderId
		WHERE WO.COMPANYID=@CompanyId and WO.CompanyBranchId=@CompanyBranchId
		AND WPSD.ProductId=@AssemblyProductId
		AND YEAR(WO.WorkOrderDate)=YEAR(@WorkOrderDate);
		

		set @QuantityCounter=1;    
	 	
		while  @QuantityCounter<=@AssemblyQuantity    
		begin    
			IF ISNULL(@MaxAssemblySerialNo,0)<>0        
			BEGIN        
			 SET @MaxAssemblySerialNo=@MaxAssemblySerialNo+1;        
			END        
			ELSE        
			BEGIN        
			 SET @MaxAssemblySerialNo=1;        
			END        
		
			set @AssemblySerialNo='M16VECB' +  format(month(@WorkOrderDate),'0#') + @AssemblyCode + right(cast(year(@WorkOrderDate) as varchar),2) + FORMAT(@MaxAssemblySerialNo,'000#');        


   
     
   insert into WorkOrderProductSerialDetail(WorkOrderId,ProductId,ProductSerialNo,ProductSerialStatus,ProductSerialRemarks,ProductSerialSequence)            
   values(@WorkOrderId,@AssemblyProductId,@AssemblySerialNo,'NEW','',@MaxAssemblySerialNo)    
       
   set @QuantityCounter=@QuantityCounter+1;    
  end    
END
 set @RowNum=@RowNum+1;    
end    

END
--------------------Inerst data in StoreRequisition table and StoreRequisitionProductDetail Table By Dheeraj Kumar Date---18/12/2017-------------------------

if @WorkOrderStatus='FINAL'

     begin

	        declare @WorkNo as varchar(100);
			IF @WorkOrderDate<@FinYearStartDate OR @WorkOrderDate>@FinYearEndDate         
			 BEGIN        
			      SET @message='Work Order Date must be within selected financial year.';        
					  set @status='FAIL';        
					  set @RetWorkOrderId=0;        
					  RAISERROR(@message,16,1);        
			 END 

				
					    	select @StateCode=st.StateCode from dbo.ComapnyBranch cb inner join [state] st       
							on cb.StateId=st.StateId where cb.CompanyBranchId=@CompanyBranchId     
        
							SELECT @FinYearStartDate=StartDate,@FinYearEndDate=EndDate,@FinYearCode=FinYearCode        
							FROM FinancialYear WHERE FinYearId=@FinYearID

							SELECT @MaxRequisitionNo=MAX(RequisitionSequence) FROM StoreRequisition WHERE COMPANYID=@CompanyId and CompanyBranchId=@CompanyBranchId      
							IF ISNULL(@MaxRequisitionNo,0)<>0        
							BEGIN      
							 SET @MaxRequisitionNo=@MaxRequisitionNo+1;        
							END        
							ELSE        
							BEGIN        
							 SET @MaxRequisitionNo=1;        
							END        
							set @RequisitionNo=@CompanyBranchCodeNew + '/SR/'+ RIGHT(@FinYearCode,5) + '/' +  FORMAT(@MaxRequisitionNo,'000#');       
				       

				            select @WorkNo=WorkOrderNo from WorkOrder where WorkOrderId=@WorkOrderId
						
---------------------------------Insert Data StoreRequisition Table ---------------------------------------------------------------------------------------

							INSERT INTO StoreRequisition(RequisitionNo,RequisitionDate,FinYearId,RequisitionType,CompanyId,
							CompanyBranchId,RequisitionByUserId,CustomerId,CustomerBranchId,LocationId,Remarks1,Remarks2,CreatedBy,
							CreatedDate,RequisitionStatus,ApprovalStatus,ApprovedBy,ApprovedDate,RequisitionSequence,WorkOrderId,WorkOrderNo) 
							VALUES(@RequisitionNo,@WorkOrderDate,@FinYearID,'PO',@CompanyId,
							@CompanyBranchId,@LocationId,0,0,0,@Remarks1,@Remarks2,@CreatedBy,
							GETDATE(),@WorkOrderStatus,'Approved',@CreatedBy,GETDATE(),@MaxRequisitionNo,@WorkOrderId,@WorkNo)

---------------------------End Code ------------------------------------------------------------------------------------------------------------------
				           set @RetRequisitionId=SCOPE_IDENTITY();   

---------------------------Insert Data StoreRequisitionProductDetail Table -------------------------------------------------------------

					            insert into @temp_Product(RowId,ProductId,Quantity)    
								select row_number() over (order by  PB.ProductId),PB.ProductId,Sum(wpd.Quantity*pb.BOMQty) as BOMQty   
								from @WorkOrderProductDetail WPD
								inner join ProductBOM PB on WPD.ProductId=pb.assemblyId 
								where pb.Status=1               
								group by PB.ProductId 
								set @RowCountProduct=(select Max(RowId) from @temp_Product);  
								set @RowNumProduct=1; 
						   				
						while (@RowNumProduct<=@RowCountProduct)  
						begin  						   						

------------------------select @rowID=RowId from @temp_Product where RowId=@RowNumProduct--------------------------------------

						 insert into StoreRequisitionProductDetail(RequisitionId,ProductId,ProductShortDesc,Quantity,IssuedQuantity)        				
				         select @RetRequisitionId,ProductId,'Work Order',Quantity,0	 
				         from @temp_Product WPD

						 set @RowNumProduct=@RowCountProduct+1;

						 end


			

end
-------------------------End Code------------------------------------------------------------------------------------------------    
         
         
 SET @message='';        
 set @status='SUCCESS';        
 set @RetWorkOrderId=@WorkOrderId;         
         
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
set @RetWorkOrderId=0;        
END CATCH;        
end  

go





