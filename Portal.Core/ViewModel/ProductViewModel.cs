using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class ProductViewModel:IModel
    {
        public long Productid { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public string ProductFullDesc { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public int ProductMainGroupId { get; set; }
        public string ProductMainGroupName { get; set; }
        public int ProductSubGroupId { get; set; }
        public string ProductSubGroupName { get; set; }
        public string AssemblyType { get; set; }
        public int UOMId { get; set; }
        public string UOMName { get; set; }
        public int PurchaseUOMId { get; set; }
        public string PurchaseUOMName { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal LocalTaxRate { get; set; }
        public decimal CentralTaxRate { get; set; }
        public decimal OtherTaxRate { get; set; }
        public bool IsSerializedProduct { get; set; }

        public bool IsThirdPartyProduct { get; set; }
        public bool OnlineProduct { get; set; }


        public string BrandName { get; set; }
        public decimal ReOrderQty { get; set; }
        public decimal MinOrderQty { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public string ModifiedDate { get; set; }
        public bool Product_Status { get; set; }
        public int CompanyId { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string ProductPicPath { get; set; }
        public string ProductPicName { get; set; }

        public decimal CGST_Perc { get; set; }
        public decimal SGST_Perc { get; set; }
        public decimal IGST_Perc { get; set; }
        public string HSN_Code { get; set; }
        public bool GST_Exempt { get; set; }

        public int SizeId { get; set; }
        public string SizeDesc { get; set; }
        public string SizeCode { get; set; } 
        public string Length { get; set; }
        public int ManufacturerId { get; set; }
        public string ManufacturerName { get; set; }
        public string ManufacturerCode { get; set; }
        public string ColourCode { get; set; }

        public bool IsNonGST { get; set; }
        public bool IsWarrantyProduct { get; set; }
        public int WarrantyInMonth { get; set; }
        public string WarrantyStartDate { get; set; }
        public string WarrantyEndDate { get; set; }
        public bool IsNilRated { get; set; }
        public bool IsZeroRated { get; set; }
        public string  RackNo { get; set; }
        public string ModelName { get; set; }
        public string CC { get; set; }
        public long HSNID { get; set; }
        public long VendorId { get; set; }
        public string VehicleType { get; set; }
        public string VendorName { get; set; }
        public string VendorCode { get; set; }
        public string LocalName { get; set; }
        public string Compatibility { get; set; }
        public long CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }

        public string ProductMainGroupCode { get; set; }
        public string ProductSubGroupCode { get; set; }

        public string ManufactureCode { get; set; }
        public decimal MRP { get; set; }
    }

    public class ProductConsumeCountViewModel
    {
        public int ProductConsumeTodayCount { get; set; }
        public int ProductConsumeMtdCount { get; set; }
        public int ProductConsumeYtdCount { get; set; }
    }
    public class PendingMRNCountViewModel
    {
        public int JobWorkCount { get; set; }
        public int PendingJobWorkMRN { get; set; }
        public int QCPendingForMRN { get; set; }
    }
    public class InventoryDashboardOthersCountViewModel
    {
        public int GateInCount { get; set; }
        public int PendingRequistionforSIN { get; set; }
        public int PhysicalAsOnDate { get; set; }
    }
    public class InventoryDashboardProductPriceViewModel
    {
        public decimal RawMaterialTotalPrice { get; set; }
        public decimal ConsumableTotalPrice { get; set; }
        public decimal FinishedGoodTotalPrice { get; set; }
    }
}
