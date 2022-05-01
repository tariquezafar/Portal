using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class PurchaseIndentViewModel
    {
        public long IndentId { get; set; }
        public string IndentNo { get; set; }
        public string IndentDate { get; set; }
        public int FinYearId { get; set; }
        public string IndentType { get; set; }
        public int CompanyId { get; set; }
        public int CompanyBranchId { get; set; }
        public int LocationId { get; set; }
        public int IndentByUserId { get; set; }
        public string FullName { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public int CustomerBranchId { get; set; }
        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public string IndentStatus { get; set; }
        public string ApprovalStatus { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedByUserName { get; set; }
        public string ApprovedDate { get; set; }
        public string RejectionStatus { get; set; }
        public int RejectedBy { get; set; }
        public string RejectedByUserName { get; set; }
        public string RejectedDate { get; set; }
        public string RejectedReason { get; set; }
        public int IndentSequence { get; set; }
        public long RequisitionId { get; set; }
        public string RequisitionNo { get; set; }

        public string CompanyBranchName { get; set; }
    }
    public class PurchaseIndentProductDetailViewModel
    {
        public int SequenceNo { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string UOMName { get; set; }
        public long IndentProductDetailId { get; set; }
        public long IndentId { get; set; }
        public long ProductId { get; set; }
        public string ProductShortDesc { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
         
        public decimal CGST_Perc { get; set; }
        public decimal CGST_Amount { get; set; }
        public decimal SGST_Perc { get; set; }
        public decimal SGST_Amount { get; set; }
        public decimal IGST_Perc { get; set; }
        public decimal IGST_Amount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string HSN_Code { get; set; }
        

    }
}

