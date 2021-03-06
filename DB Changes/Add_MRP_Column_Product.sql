
alter table Product
add MRP decimal(18,2)
GO
ALTER proc [dbo].[proc_GetProductDetail] --proc_GetProductDetail  6    
(          
@ProductId bigint        
)          
as          
begin          
set nocount on;          
select p.Productid,    
p.ProductName,    
p.ProductCode,    
p.ProductShortDesc,    
p.ProductFullDesc,    
p.CompanyId,    
p.ProductTypeId,    
pt.ProductTypeName,    
p.ProductMainGroupId,    
pmg.ProductMainGroupName,    
p.ProductSubGroupId,    
psg.ProductSubGroupName,    
p.AssemblyType,    
p.UOMId,    
uom.UOMName,    
isnull(p.PurchaseUOMId,0) PurchaseUOMId,
isnull(purchaseUom.UOMName,'') as PurchaseUOMName,
p.PurchasePrice,    
p.SalePrice,    
p.IsSerializedProduct,   
isnull(p.IsWarrantyProduct,0) as IsWarrantyProduct,
isnull(p.WarrantyInMonth,0) as WarrantyInMonth,    
p.BrandName,    
p.ReOrderQty,  
isnull(p.CGST_Perc,'0') as  CGST_Perc,
isnull(p.SGST_Perc,'0') as  SGST_Perc,
isnull(p.IGST_Perc,'0') as  IGST_Perc,
isnull(p.HSN_Code,'') as  HSN_Code,
isnull(p.ColourCode,'') as  ColourCode,
isnull(p.GST_Exempt,0) as GST_Exempt,
isnull(p.IsNonGST,0) as IsNonGST,
ISNULL(p.IsZeroRated,0) as IsZeroRated,
ISNULL(p.IsNilRated,0) as IsNilRated,

ISNULL(p.IsThirdPartyProduct,0) as IsThirdPartyProduct,
ISNULL(p.IsOnline,0) as OnlineProduct,
ISNULL(p.HSNID,0) as HSNID,

ISNULL(p.ModelName,'') as ModelName,
ISNULL(p.CC,'') as CC,

p.MinOrderQty,    
p.CreatedBy,          
cu.FullName CreatedByName,        
replace(convert(varchar, p.CreatedDate,106),' ','-') CreatedDate,          
p.ModifiedBy,          
isnull(mu.FullName,'') ModifiedByName,        
case when p.ModifiedDate is null then '' else replace(convert(varchar, p.ModifiedDate,106),' ','-') end ModifiedDate,          
p.Status      ,  
isnull(p.UserPicPath,'') as UserPicPath,  
isnull(p.UserPicName,'') as UserPicName  ,
isnull(p.SizeId,0) SizeId,isnull(siz.SizeCode,'0') SizeCode,isnull(siz.SizeDesc,'') SizeDesc,
isnull(p.Length,'') [Length],
isnull(p.ManufacturerId,0) ManufacturerId,isnull(man.ManufacturerCode,'0') ManufacturerCode,isnull(man.ManufacturerName,'') ManufacturerName,
Isnull(RackNo,'') RackNo,
ISNULL(v.VendorName,'') as VendorName,
ISNULL(v.VendorCode,'') as VendorCode,
ISNULL(P.VendorId,0) as VendorId,
ISNULL(P.VehicleType,'') as VehicleType,
ISNULL(P.LocalName,'') as LocalName,
ISNULL(P.Compatibility,'') as Compatibility,
ISNULL(P.CompanyBranchId,0) as CompanyBranchId,
ISNULL(cb.BranchName,'') as CompanyBranchName,
isnull(p.MRP,0) as MRP
from Product p     
left join Vendor v on p.VendorId=v.VendorId
left join ProductType pt on p.ProductTypeId=pt.ProductTypeId    
left join ProductMainGroup pmg on p.ProductMainGroupId=pmg.ProductMainGroupId    
left join ProductSubGroup psg on p.ProductSubGroupId=psg.ProductSubGroupId    
left join UOM uom on p.UOMId=uom.UOMId    
left join [User] cu on p.createdBy=cu.UserId        
left join [User] mu on p.ModifiedBy=mu.Userid
left join Size siz on p.SizeId=siz.SizeId
left join Manufacturer man on p.ManufacturerId=man.ManufacturerId
left join UOM purchaseUom on p.PurchaseUOMId=purchaseUom.UOMId
left join ComapnyBranch  cb on p.CompanyBranchId=cb.CompanyBranchId
where P.ProductId=@Productid    
    
set nocount off;          
end



