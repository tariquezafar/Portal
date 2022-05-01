using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class WarrantyViewModel
    {
        public long WarrantyID { get; set; }       
        public string WarrantyDate { get; set; }       
        public int InvoicePackingListId { get; set; }
        public string InvoicePackingListNo { get; set; }
        public string InvoicePackingListDate { get; set; }
        public string InvoiceNo { get; set; }
        public int InvoiceId { get; set; }       
        public string DispatchDate { get; set; }
        public int FinYearId { get; set; }

    }
    public class WarrantyProductDetailViewModel
    {
        public int SequenceNo { get; set; }
        public long WarrantyDetailID { get; set; }
        public long WarrantyID { get; set; }
        public long Productid { get; set; }
        public int WarrantyPeriodMonth { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public string ProductHSNCode { get; set; }
        public string UOMName { get; set; }
        public string WarrantyStartDate { get; set; }
        public decimal Quantity { get; set; }
        public string WarrantyEndDate { get; set; }
       

    }

    public partial class ReturnViewModel
    {
       
        public long ReturnedID { get; set; }
        public long WarrantyID { get; set; }
        
        public string ReturnedNo { get; set; }
        public string ReturnedDate { get; set; }
        public long InvoiceID { get; set; }
        public string InvoiceNo { get; set; }
        public int FinYearId { get; set; }
        public int CompanyId { get; set; }
        public string InvoicePackingListNo { get; set; }
        public string CompanyBranchName { get; set; }
        public long CompanyBranchId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedName { get; set; }
        public string ModifiedDate { get; set; }
        public string ApprovalStatus { get; set; }
        public string Warranty { get; set; }
    }
    public partial class ReturnedProductDetailViewModel
    {
        public int SequenceNo { get; set; }
        public long ReturnedDetailID {get;set;}
        public long ReturnedID { get; set; }
        public long ProductId { get; set; }
        public decimal ReplacedQTY { get; set; }
        public decimal ReturnedQty { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public int WarrantyPeriodMonth { get; set; }
        public string WarrantyStartDate { get; set; }
        public decimal Quantity { get; set; }
        public string WarrantyEndDate { get; set; }

        public string ProductName { get; set; }
        public string ProductCode { get; set; }

    }

}
