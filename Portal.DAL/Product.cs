//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Portal.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public long Productid { get; set; }
        public string ProductName { get; set; }
        public string LocalName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public string ProductFullDesc { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> ProductTypeId { get; set; }
        public Nullable<int> ProductMainGroupId { get; set; }
        public Nullable<int> ProductSubGroupId { get; set; }
        public string AssemblyType { get; set; }
        public Nullable<int> UOMId { get; set; }
        public Nullable<decimal> PurchasePrice { get; set; }
        public Nullable<decimal> SalePrice { get; set; }
        public Nullable<decimal> LocalTaxRate { get; set; }
        public Nullable<decimal> CentralTaxRate { get; set; }
        public Nullable<decimal> OtherTaxRate { get; set; }
        public Nullable<bool> IsSerializedProduct { get; set; }
        public string BrandName { get; set; }
        public Nullable<decimal> ReOrderQty { get; set; }
        public Nullable<decimal> MinOrderQty { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> Status { get; set; }
        public string UserPicPath { get; set; }
        public string UserPicName { get; set; }
        public Nullable<decimal> CGST_Perc { get; set; }
        public Nullable<decimal> SGST_Perc { get; set; }
        public Nullable<decimal> IGST_Perc { get; set; }
        public Nullable<long> HSNID { get; set; }
        public string HSN_Code { get; set; }
        public Nullable<bool> GST_Exempt { get; set; }
        public Nullable<int> SizeId { get; set; }
        public string Length { get; set; }
        public Nullable<int> ManufacturerId { get; set; }
        public Nullable<int> PurchaseUOMId { get; set; }
        public string ColourCode { get; set; }
        public Nullable<bool> IsNonGST { get; set; }
        public Nullable<bool> IsWarrantyProduct { get; set; }
        public Nullable<int> WarrantyInMonth { get; set; }
        public Nullable<bool> IsNilRated { get; set; }
        public Nullable<bool> IsZeroRated { get; set; }
        public Nullable<bool> IsThirdPartyProduct { get; set; }
        public string RackNo { get; set; }
        public string ModelName { get; set; }
        public string CC { get; set; }
        public Nullable<long> ProductSequence { get; set; }
        public Nullable<long> VendorId { get; set; }
        public string VehicleType { get; set; }
        public string Compatibility { get; set; }
        public Nullable<long> CompanyBranchId { get; set; }
        public Nullable<bool> IsOnline { get; set; }
        public Nullable<decimal> MRP { get; set; }
    }
}
