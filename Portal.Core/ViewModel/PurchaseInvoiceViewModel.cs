using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
    public class PurchaseInvoiceViewModel
    {
        public long InvoiceId { get; set; }
        public string InvoiceNo { get; set; }


        public long SaleInvoiceInvoiceId { get; set; }
        public string SaleInvoiceInvoiceNo { get; set; }

        public string InvoiceDate { get; set; }
        public string PODate { get; set; }
        public int POId { get; set; }
        public string PONo { get; set; }
        public string CurrencyCode { get; set; }
        public string PurchaseType { get; set; }
        public int CompanyBranchId { get; set; }
        public int SICompanyBranchId { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorCode { get; set; }
        public bool GST_Exempt { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }
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
        public decimal FreightValue { get; set; }
        public decimal LoadingValue { get; set; }
        public string Remarks { get; set; }
        public int FinYearId { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public bool InvoiceRevisedStatus { get; set; }
        public string InvoiceStatus { get; set; }
        public string ApprovalStatus { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedDate { get; set; }
        public string RejectionStatus { get; set; }
        public int RejectedBy { get; set; }
        public string RejectedDate { get; set; }
        public string RejectedReason { get; set; }
        public string CancelStatus { get; set; }
        public int CancelBy { get; set; }
        public string CancelDate { get; set; }
        public string CancelReason { get; set; }
        public int InvoiceSequence { get; set; }

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
        public decimal TotalQuantity { get; set; }
        public string ShippingCity { get; set; }
        public int ShippingStateId { get; set; }
        public string ShippingStateName { get; set; }
        public int ShippingCountryId { get; set; }
        public string ShippingCountryName { get; set; }
        public string ShippingPinCode { get; set; }

        public decimal CGST_Amount { get; set; }
        public decimal SGST_Amount { get; set; }
        public decimal IGST_Amount { get; set; }
        public string ConsigneeGSTNo { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public decimal RoundOfValue { get; set; }
        public decimal GrossValue { get; set; }

        public string companyBranch { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }

        public string MRNNO { get; set; }

        public string MRNDate { get; set; }

        public long MRNId { get; set; }


    }
    public class PurchaseInvoiceProductDetailViewModel
    {
        public int SequenceNo { get; set; }
        public long InvoiceProductDetailId { get; set; }
        public long InvoiceId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public string UOMName { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }

        public decimal ReceivedQuantity{ get; set; }
        public decimal AcceptQuantity{ get; set; }
        public decimal RejectQuantity{ get; set; }

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

    public class PurchaseInvoiceTaxDetailViewModel
    {
        public int TaxSequenceNo { get; set; }
        public long InvoiceTaxDetailId { get; set; }
        public long InvoiceId { get; set; }
        public long TaxId { get; set; }
        public string TaxName { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal TaxAmount { get; set; }
        public bool CFormApplicable { get; set; }
        public string CFormStatus { get; set; }
        public string CFormNo { get; set; }
        public string CFormDate { get; set; }
        public string CFormRemarks { get; set; }
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
    public class PurchaseInvoiceTermsDetailViewModel
    {
        public long InvoiceTermDetailId { get; set; }
        public long InvoiceId { get; set; }
        public string TermDesc { get; set; }
        public int TermSequence { get; set; }
        public string message { get; set; }
        public string status { get; set; }
    }


    public class PurchaseInvoiceChasisDetailViewModel
    {
        public long InvoiceSerialId { get; set; }
        public long InvoiceId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ChasisSerialNo { get; set; }
    }

    public class PurchaseSummaryRegisterViewModel
    {  
        public long InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceType { get; set; }
        public int VendorId { get; set; }
        public string  VendorName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public decimal BasicValue { get; set; }
        public decimal LoadingValue { get; set; }
        public decimal FreightValue { get; set; }

        public decimal TotalValue { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal AmountPending { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string companyBranch { get; set; }
        

    }

    public class PISupportingDocumentViewModel
    {
        public int DocumentSequenceNo { get; set; }
        public long InvoiceDocId { get; set; }
        public int InvoiceId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentTypeDesc { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }




}
