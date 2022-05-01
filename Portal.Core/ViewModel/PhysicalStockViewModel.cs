using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
    public  class PhysicalStockViewModel
    {
        public long PhysicalStockID { get; set; }
        public string PhysicalStockNo { get; set; }
        public string PhysicalStockDate { get; set; }
        public string PhysicalAsOnDate { get; set; }
        public string ApprovalStatus { get; set; }
        public int CompanyId { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
        public int PhysicalStockSequence { get; set; }
    }

    public class PhysicalStockProductDetailViewModel
    {
        public long Productid { get; set; }
        public long PhysicalStockDetailID { get; set; }
        public long PhysicalStockID { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public string ProductFullDesc { get; set; }
        public int CompanyId { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public int ProductMainGroupId { get; set; }
        public string ProductMainGroupName { get; set; }
        public int ProductSubGroupId { get; set; }
        public string ProductSubGroupName { get; set; }
        public string AssemblyType { get; set; }
        public int UOMId { get; set; }
        public string UOMName { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string ApprovalStatus { get; set; }
        public decimal PhysicalQTY { get; set; }
        public decimal SystemQTY { get; set; }
        public decimal DiffQTY { get; set; }
        public bool Status { get; set; }
        public int TransferTo { get; set; }

        public string TransferProductName { get; set; }
    }
}
