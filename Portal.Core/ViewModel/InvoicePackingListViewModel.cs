using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class InvoicePackingListViewModel
    {
         
        public long InvoicePackingListId { get; set; }
        public string InvoicePackingListNo { get; set; }
        public string InvoicePackingListDate { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public int CustomerId { get; set; }
        public int CompanyBranchId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public long PackingListTypeID { get; set; }
        public string PackingListTypeName { get; set; }
        public string Remarks { get; set; }
        public int FinYearId { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public string InvoicePackingListStatus { get; set; }
        

        public int ApprovedBy { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovedByUserName { get; set; }
        public string ApprovedDate { get; set; }
        public string RejectionStatus { get; set; }
        public int RejectedBy { get; set; }
        public string RejectedByUserName { get; set; }
        public string RejectedDate { get; set; }
        public string RejectedReason { get; set; }

        public int InvoicePackingListSequence { get; set; }
        public string message { get; set; }
        public string status { get; set; }

        public string CompanyBranchName { get; set; }
    }
    public class InvoicePackingListProductDetailViewModel
    {
        public int SequenceNo { get; set; }
        public long InvoicePackingListProductDetailId { get; set; }
        public int InvoicePackingListId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public string UOMName { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
    
        public decimal TotalPrice { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string PackingProductType { get; set; }
        public string IsWarrantyProduct { get; set; }
        public string WarrantyInMonth { get; set; }

    }
  
}
