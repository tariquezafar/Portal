IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'LocationId'
          AND Object_ID = Object_ID(N'dbo.ProductOpeningStock'))
begin
alter table ProductOpeningStock
add LocationId int constraint FK_ProductOpeningStock_LocationId foreign key references [dbo].[Location](LocationId)
end

go

--proc_GetProductOpenings

ALTER proc [dbo].[proc_GetProductOpenings] --proc_GetProductOpenings 'e',1,'2008'
(        
@ProductName varchar(50),        
@CompanyId int,        
@FinYearId int,
@companyBranchId int
)        
as        
begin        
set nocount on;        
        
declare @strSql as nvarchar(4000);        
      
        
set @strSql='select po.OpeningTrnId,  
po.ProductId,
p.ProductName,  
p.ProductCode,  
p.ProductShortDesc,  
po.FinYearId,
f.FinYearDesc,
po.CompanyBranchId,
CB.BranchName,
po.OpeningQty,  
po.CreatedBy,        
cu.FullName CreatedByName,      
replace(convert(varchar, po.CreatedDate,106),'' '',''-'') CreatedDate,        
po.ModifiedBy,        
isnull(mu.FullName,'''') ModifiedByName,      
case when po.ModifiedDate is null then '''' else replace(convert(varchar, po.ModifiedDate,106),'' '',''-'') end ModifiedDate ,
isnull( LC.LocationName,'''') as LocationName
from  ProductOpeningStock po inner join  Product p on po.productid=p.productid  
left join FinancialYear f on po.FinYearid=f.FinYearid
left join ComapnyBranch CB on po.CompanyBranchId=CB.CompanyBranchId
left join [User] cu on p.createdBy=cu.UserId      
left join [User] mu on p.ModifiedBy=mu.Userid  
left join [Location] LC on isnull(po.LocationId,0)= isnull(LC.LocationId,0)
where po.CompanyId='+ cast(@CompanyId as varchar) +' ';        
        
if @ProductName<>''        
begin        
set @strSql=@strSql + ' and p.ProductName like ''%' + @ProductName + '%''';        
end        
      
if @FinYearId<>0        
begin        
set @strSql=@strSql + ' and po.FinYearId = ' + cast(@FinYearId as varchar) + '';        
end  
if @companyBranchId<>0        
begin        
set @strSql=@strSql + ' and po.CompanyBranchId = ' + cast(@companyBranchId as varchar) + '';        
end          

set @strSql=@strSql + ' order by p.Productname Asc';   
print @strSql       
exec sp_executesql @strSql        
        
set nocount off;        
end

go
--proc_GetProductOpeningDetail
ALTER proc [dbo].[proc_GetProductOpeningDetail] --proc_GetProductDetail   
(        
@OpeningTrnId bigint      
)        
as        
begin        
set nocount on;        
select po.OpeningTrnId,  
po.ProductId,
p.ProductName,  
p.ProductCode,  
p.ProductShortDesc,  
po.FinYearId,
f.FinYearDesc,
po.CompanyBranchId,
CB.BranchName,
po.OpeningQty,  
po.CreatedBy,        
cu.FullName CreatedByName,      
replace(convert(varchar, po.CreatedDate,106),' ','-') CreatedDate,        
po.ModifiedBy,        
isnull(mu.FullName,'') ModifiedByName,      
case when po.ModifiedDate is null then '' else replace(convert(varchar, po.ModifiedDate,106),' ','-') end ModifiedDate, 
ISNULL(LC.LocationName,'') AS LocationName,
ISNULL(LC.LocationId,0) as LocationId
from  ProductOpeningStock po inner join  Product p on po.productid=p.productid  
inner join FinancialYear f on po.FinYearid=f.FinYearid
inner join ComapnyBranch CB on po.CompanyBranchId=CB.CompanyBranchId
inner join [User] cu on p.createdBy=cu.UserId      
left join [User] mu on p.ModifiedBy=mu.Userid   
left join [Location] LC ON  ISNULL( LC.LocationId,0)=ISNULL( PO.LocationId,0)
where PO.OpeningTrnId=@OpeningTrnId  
  
set nocount off;        
end
