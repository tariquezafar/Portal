using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class PurchaseReturnViewModel
    {
         
        public long PurchaseReturnId { get; set; }
        public string PurchaseReturnNo { get; set; }
        public string PurchaseReturnDate { get; set; }

        public string ReturnType { get; set; }
        public string InvoiceStatus { get; set; }
        
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public int VendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string ContactPerson { get; set; }
        public string StateName { get; set; }
        public string ShippingContactPerson { get; set; }
        public string BillingAddress { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string ShippingStateName { get; set; }
        public int CountryId { get; set; }
        public string ShippingCountryName { get; set; }
        public string PinCode { get; set; }
        public string GSTNo { get; set; }
        public string Fax { get; set; } 
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
        public string CompanyBranchAddress { get; set; }
        public string CompanyBranchCity { get; set; }
        public int CompanyBranchStateId { get; set; }
        public string CompanyBranchStateName { get; set; }
        public string CompanyBranchPinCode { get; set; }
        public string CompanyBranchCSTNo { get; set; }
        public string CompanyBranchTINNo { get; set; }

        public string RefNo { get; set; }
        public string RefDate { get; set; }


        public string LRNo { get; set; }
        public string LRDate { get; set; }
        public string TransportVia { get; set; }
        public int NoOfPackets { get; set; } 

        public decimal BasicValue { get; set; }
        public decimal LoadingValue { get; set; }
        public decimal FreightValue { get; set; }
        public decimal TotalValue { get; set; }

 
        public string Remarks { get; set; }
        public string Remarks2 { get; set; }
        public int FinYearId { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public string ChallanStatus { get; set; }
        

        public int ApprovedBy { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovedByUserName { get; set; }
        public string ApprovedDate { get; set; }
        public string RejectionStatus { get; set; }
        public int RejectedBy { get; set; }
        public string RejectedByUserName { get; set; }
        public string RejectedDate { get; set; }
        public string RejectedReason { get; set; }

        public int ReturnSequence { get; set; }
        public string message { get; set; }
        public string status { get; set; }

        public int ConsigneeId { get; set; }
        public string ConsigneeCode { get; set; }
        public string ConsigneeName { get; set; }
        public bool ReverseChargeApplicable { get; set; }
        public decimal ReverseChargeAmount { get; set; }
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
        public decimal RoundOfValue { get; set; }
        public decimal GrossValue { get; set; }

        public string companyBranch  { get; set; } 

    }
    public class PurchaseReturnProductViewModel
    {
        public int SequenceNo { get; set; }
        public long PurchaseReturnProductDetailId { get; set; }
        public int PurchaseReturnId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public string UOMName { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public long TaxId { get; set; }
        public string TaxName { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalPrice { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public decimal CGST_Perc { get; set; }
        public decimal CGST_Amount { get; set; }
        public decimal SGST_Perc { get; set; }
        public decimal SGST_Amount { get; set; }
        public decimal IGST_Perc { get; set; }
        public decimal IGST_Amount { get; set; }
        public string HSN_Code { get; set; }

    }
  
}
