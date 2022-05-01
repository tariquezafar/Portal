using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
 public class POViewModel
    {
        public long POId { get; set; }
        public string PONo { get; set; }
        public string PODate { get; set; }
        public string CurrencyCode { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorCode { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }

        public int CompanyBranchId { get; set; }
        public string CountryName { get; set; }
        public string PinCode { get; set; }
        public string CSTNo { get; set; }
        public string TINNo { get; set; }
        public string PANNo { get; set; }
        public string GSTNo { get; set; }
        public string ExciseNo { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public decimal BasicValue { get; set; }
        public decimal TotalValue { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal FreightValue { get; set; }
        public decimal LoadingValue { get; set; }
        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }
        public int FinYearId { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        
        public string ApprovedByName { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public bool PORevisedStatus { get; set; }
        public string ApprovalStatus { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedByUserName { get; set; }
        public string ApprovedDate { get; set; }
        public string RejectionStatus { get; set; }
        public int RejectedBy { get; set; }
        public string RejectedByUserName { get; set; }
        public string RejectedDate { get; set; }
        public string RejectedReason { get; set; }
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

        public string ShippingCity { get; set; }
        public int ShippingStateId { get; set; }
        public string ShippingStateName { get; set; }
        public int ShippingCountryId { get; set; }
        public string ShippingCountryName { get; set; }
        public string ShippingPinCode { get; set; }

        public string ConsigneeGSTNo { get; set; }


        public string ExpectedDeliveryDate { get; set; }
        public long IndentId { get; set; }
        public string IndentNo { get; set; }
        public string IndentDate { get; set; }
        public long QuotationId { get; set; }
        public string QuotationNo { get; set; }
        public string QuotationDate { get; set; }
        public decimal CGST_Amount { get; set; }
        public decimal SGST_Amount { get; set; }
        public decimal IGST_Amount { get; set; }
        public int CancelBy { get; set; }
        public string CancelByUserName { get; set; }
        public string CancelDate { get; set; }
        public string CancelReason { get; set; }
        public string POStatus { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string CompanyBranch { get; set; }

        public decimal CurrencyConversionRate { get; set; }
        public string POType { get; set; }
    }
    public class POProductViewModel
    {
        public int SequenceNo { get; set; }
        public long POProductDetailId { get; set; }
        public int POId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public string UOMName { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public decimal PendingQuantity { get; set; }
        public decimal TotalRecQuantity { get; set; }
        public decimal AcceptQuantity { get; set; }
        public decimal RejectQuantity { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public long TaxId { get; set; }
        public string TaxName { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalPrice { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string SurchargeName_1 { get; set; }
        public decimal SurchargePercentage_1 { get; set; }
        public decimal SurchargeAmount_1 { get; set; }
        public string SurchargeName_2 { get; set; }
        public decimal SurchargePercentage_2 { get; set; }
        public decimal SurchargeAmount_2 { get; set; }
        public string SurchargeName_3 { get; set; }
        public decimal SurchargePercentage_3 { get; set; }
        public decimal SurchargeAmount_3 { get; set; }

        public decimal CGST_Perc { get; set; }
        public decimal CGST_Amount { get; set; }
        public decimal SGST_Perc { get; set; }
        public decimal SGST_Amount { get; set; }
        public decimal IGST_Perc { get; set; }
        public decimal IGST_Amount { get; set; }
        public string HSN_Code { get; set; }

       

    }
    public class POTaxViewModel
    {
        public int TaxSequenceNo { get; set; }
        public long POTaxDetailId { get; set; }
        public int POId { get; set; }
        public long TaxId { get; set; }
        public string TaxName { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal TaxAmount { get; set; }
        public string message { get; set; }
        public string status { get; set; }

        public string SurchargeName_1 { get; set; }
        public decimal SurchargePercentage_1 { get; set; }
        public decimal SurchargeAmount_1 { get; set; }
        public string SurchargeName_2 { get; set; }
        public decimal SurchargePercentage_2 { get; set; }
        public decimal SurchargeAmount_2 { get; set; }
        public string SurchargeName_3 { get; set; }
        public decimal SurchargePercentage_3 { get; set; }
        public decimal SurchargeAmount_3 { get; set; }

    }
    public class POTermViewModel
    {
        public long POTermDetailId { get; set; }
        public int POId { get; set; }
        public string TermDesc { get; set; }
        public int TermSequence { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    public class POSupportingDocumentViewModel
    {
        public int DocumentSequenceNo { get; set; }
        public long PODocId { get; set; }
        public int POId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentTypeDesc { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
}
