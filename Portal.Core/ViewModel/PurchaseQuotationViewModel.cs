using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
  public class PurchaseQuotationViewModel
    {
        public long QuotationId { get; set; }
        public string QuotationNo { get; set; }
        public string QuotationDate { get; set; }
        public int RequisitionId { get; set; }
        public string RequisitionNo { get; set; }
        public string RequisitionDate { get; set; }
        public int CompanyBranchId { get; set; }
        public string CurrencyCode { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorCode { get; set; }
        public int DeliveryDays { get; set; }
        public string DeliveryAt { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public decimal BasicValue { get; set; }
        public decimal TotalValue { get; set; }
        public decimal FreightValue { get; set; }
        public decimal LoadingValue { get; set; }
        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }
        public int FinYearId { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedByUser { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByUser { get; set; }
        public string ModifiedDate { get; set; }
        public bool QuotationRevisedStatus { get; set; }
        public string ApprovalStatus { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedDate { get; set; }
        public string RejectionStatus { get; set; }
        public int RejectedBy { get; set; }
        public string RejectedByUser { get; set; }
        public string RejectedDate { get; set; }
        public string RejectedReason { get; set; }
        public int QuotationSequence { get; set; }
        public decimal InsuranceValue { get; set; }
        public decimal FreightCGST_Amt { get; set; }
        public decimal FreightSGST_Amt { get; set; }
        public decimal FreightIGST_Amt { get; set; }
        public decimal LoadingCGST_Amt { get; set; }
        public decimal LoadingSGST_Amt { get; set; }
        public decimal LoadingIGST_Amt { get; set; }
        public decimal InsuranceCGST_Amt { get; set; }
        public decimal InsuranceSGST_Amt { get; set; }
        public decimal InsuranceIGST_Amt { get; set; }
        public decimal FreightCGST_Perc { get; set; }
        public decimal FreightSGST_Perc { get; set; }
        public decimal FreightIGST_Perc { get; set; }
        public decimal LoadingCGST_Perc { get; set; }
        public decimal LoadingSGST_Perc { get; set; }
        public decimal LoadingIGST_Perc { get; set; }
        public decimal InsuranceCGST_Perc { get; set; }
        public decimal InsuranceSGST_Perc { get; set; }
        public decimal InsuranceIGST_Perc { get; set; }
        public string POStatus { get; set; }

        public string companyBranch { get; set; }
    }

    public class PurchaseQuotationProductViewModel
    {
        public int SequenceNo { get; set; }
        public long QuotationProductDetailId { get; set; }
        public long QuotationId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public decimal Price { get; set; }
        public  string UOMName { get; set; }
        public decimal Quantity { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal CGST_Perc { get; set; }
        public decimal CGST_Amount { get; set; }
        public decimal SGST_Perc { get; set; }
        public decimal SGST_Amount { get; set; }
        public decimal IGST_Perc { get; set; }
        public decimal IGST_Amount { get; set; }
        public string HSN_Code { get; set; }
    }
}
