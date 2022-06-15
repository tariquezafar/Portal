USE [Demo]
GO
/****** Object:  StoredProcedure [dbo].[proc_GetProductCILists] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  proc_GetProductCILists 1,1
            
ALTER PROC [dbo].[proc_GetProductCILists] 
(                    
@ComplaintId varchar(50),                   
@CompanyBranchId int=0     
)                    
as                    
begin                    
set nocount on;  

select CS.ComplaintId, P.Productid,P.ProductName,P.ProductCode,ISNULL(P.WarrantyInMonth,0) AS WarrantyInMonth,ISNULL(CSP.Quantity,0) AS Quantity ,CSP.Remarks,CS.Status from ComplaintServiceProductDetail CSP
Left join ComplaintService CS ON CSP.ComplaintId = CS.ComplaintId
Inner join Product P on CSP.ProductId = P.Productid
where CSP.ComplaintId = @ComplaintId AND CS.BranchID = @CompanyBranchId
                    
                    
set nocount off;                    
end 


