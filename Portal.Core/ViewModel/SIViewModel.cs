using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class SaleInvoiceViewModel
    {

        public string ChallanNo { get; set; }
        public long InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceType { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
        public string CurrencyCode { get; set; }
        public int SOId { get; set; }
        public string SONo { get; set; }
        public string SODate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerCode { get; set; }

        public string SaleType { get; set; }
        public string CustomerName { get; set; }
        public string ContactPerson { get; set; }
        public string BillingAddress { get; set; }
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
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string ContactNo { get; set; }
        public string Fax { get; set; }

        public string ShippingContactPerson { get; set; }
        public string ShippingBillingAddress { get; set; }
        public string ShippingCity { get; set; }
        public int ShippingStateId { get; set; }
        public string ShippingStateName { get; set; }
        public int ShippingCountryId { get; set; }
        public string ShippingCountryName { get; set; }
        public string ShippingPinCode { get; set; }
        public string ShippingTINNo { get; set; }
        public string ShippingEmail { get; set; }
        public string ShippingMobileNo { get; set; }
        public string ShippingContactNo { get; set; }
        public string ShippingFax { get; set; }

        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public decimal BasicValue { get; set; }
        public decimal BasicAmt { get; set; }
        public decimal LoadingValue { get; set; }
        public decimal FreightValue { get; set; }
        public decimal CGST_Amount { get; set; }
        public decimal SGST_Amount { get; set; }
        public decimal IGST_Amount { get; set; }
        public decimal TotalValue { get; set; }
        public int PayToBookId { get; set; }
        public string PayToBookName { get; set; }
        public string PayToBookBranch { get; set; }

        public string Remarks { get; set; }
        public int FinYearId { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }

     

        
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public bool InvoiceRevisedStatus { get; set; }
        public bool InvoiceStatus { get; set; }
        public string ApprovalStatus { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedByUserName { get; set; }
        public string ApprovedDate { get; set; }
        public string RejectionStatus { get; set; }
        public int RejectedBy { get; set; }
        public string RejectedByUserName { get; set; }
        public string RejectedDate { get; set; }
        public string RejectedReason { get; set; } 
        public string CancelStatus { get; set; }
        public int CancelBy { get; set; }
        public string CancelByUserName { get; set; }
        public string CancelDate { get; set; }
        public string CancelReason { get; set; } 
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
        public decimal TotalQuantity { get; set; }
        public decimal RoundOfValue { get; set; }
        public decimal GrossValue { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; }

        public string TransportName { get; set; }
        public string VehicleNo { get; set; }
        public string BiltyNo { get; set; }
        public string BiltyDate { get; set; }

        public string AdharcardNo { get; set; }
        public string Pancard { get; set; }
        public string IdtypeName { get; set; }
        public string IdtypeValue { get; set; }
        public string BranchType { get; set; }

        public decimal RtoRegsValue { get; set; }
        public decimal RtoRegsCGST_Amt { get; set; }
        public decimal RtoRegsSGST_Amt { get; set; }
        public decimal RtoRegsIGST_Amt { get; set; }
        public decimal RtoRegsCGST_Perc { get; set; }
        public decimal RtoRegsSGST_Perc { get; set; }
        public decimal RtoRegsIGST_Perc { get; set; }
        public decimal VehicleInsuranceValue { get; set; }

        public string HypothecationBy { get; set; }
        public string EwayBillNo { get; set; }
        public int SaleEmpId { get; set; }
        public string SaleEmployeeName { get; set; }

        public string SaleInvoiceType { get; set; }
    }
    public class SaleInvoiceProductViewModel
    {
        public long InvoiceProductDetailId { get; set; }
        public int SequenceNo { get; set; }
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public string UOMName { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxPercentage { get; set; }
        public long TaxId { get; set; }
        public string TaxName { get; set; }

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
        public string IsSerializedProduct { get; set; }

        public string IsThirdPartyProduct { get; set; }

        public string Remarks { get; set; }
        public string WarrantyStartDate { get; set; }
        public string WarrantyEndDate { get; set; }
    }
    public class SaleInvoiceTaxViewModel
    {
        public long InvoiceTaxDetailId { get; set; }
        public int InvoiceId { get; set; }
        public int TaxSequenceNo { get; set; }
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
    public class SaleInvoiceTermViewModel
    {
        public long InvoiceTermDetailId { get; set; }
        public int InvoiceId { get; set; }
        public string TermDesc { get; set; }
        public int TermSequence { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }

    public class SaleSummaryRegisterViewModel
    {


        public long InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceType { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public decimal BasicValue { get; set; }
        public decimal LoadingValue { get; set; }
        public decimal FreightValue { get; set; }
        public decimal BasicAmt { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal AmountPending { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string message { get; set; }
        public string status { get; set; }

        public string CompanyBranchName { get; set; }

    }

    public class SaleInvoiceProductSerialDetailViewModel
    {
        public long InvoiceSerialId { get; set; }
        public long InvoiceId { get; set; }
        public long ProductId { get; set; }
        public int PackingListTypeID { get; set; }
        public string ProductName { get; set; }
        public string RefSerial1 { get; set; }
        public string RefSerial2 { get; set; }
        public string RefSerial3 { get; set; }
        public string RefSerial4 { get; set; }
        public string PackingListTypeName { get; set; }

        public string MotorNo { get; set; }
        public string ControllerNo { get; set; }
        public string BatterySerialNo { get; set; }
        public string ChargerNo { get; set; }

    }

    public class SISupportingDocumentViewModel
    {
        public int DocumentSequenceNo { get; set; }
        public long SaleInvoiceDocId { get; set; }
        public int InvoiceId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentTypeDesc { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public string message { get; set; }
        public decimal LabourRate { get; set; }
        public string LabourCode { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public decimal DiscountPerc { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal CGST_Perc { get; set; }
        public decimal CGST_Amount { get; set; }
        public decimal SGST_Perc { get; set; }
        public decimal SGST_Amount { get; set; }
        public decimal IGST_Perc { get; set; }
        public decimal IGST_Amount { get; set; }

        public decimal TotalAmount { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
     

    }

    public class GSTR1ViewModel
    {
        public int RecipientCount { get; set; }
        public int InvoiceCount { get; set; }
        public decimal TotalInvoiceValue { get; set; }
        public decimal TotalTaxableValue { get; set; }
        public string ConsigneGSTNo { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public decimal InvoiceValue { get; set; }
        public string PlaceOfSupply { get; set; }
        public string ReverseCharge { get; set; }
        public string InvoiceType { get; set; }
        public string EcommerceGSTNo { get; set; }
        public decimal Rate { get; set; }
        public decimal TaxableValue { get; set; }
        public decimal CessValue { get; set; }
    }

    public class SaleInvoiceChasisProductSerialDetailViewModel
    {
        public long SequenceNo { get; set; }
         
        public string ProductName { get; set; }
        public string RefSerial1 { get; set; }
        public int InvoiceSerialId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; }
    }

    public class SaleRetrunProductSerialDetailViewModel
    {
        public long InvoiceSaleReturnProSerialId { get; set; }
        public long InvoiceSerialId { get; set; }
        public long ProductId { get; set; }
        public string RefSerial1 { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public bool serialStatus { get; set; }
    }


    public class SaleInvoiceCountViewModel
    {
        public string TodaySaleCount { get; set; }
        public string TodaySaleAmount { get; set; }
        public string MTDSaleCount { get; set; }
        public string MTASaleAmount { get; set; }
        public string YTDSaleCount { get; set; }
        public string YTDSaleAmount { get; set; }


    }

    public class SaleQutationCountViewModel
    {
        public string TodaySaleQutationCount { get; set; }
        public string TodaySaleQutationAmount { get; set; }
        public string MTDSaleQutationCount { get; set; }
        public string MTASaleQutationAmount { get; set; }
        public string YTDSaleQutationCount { get; set; }
        public string YTDSaleQutationAmount { get; set; }
    }
    public class SaleOrderCountViewModel
    {
        public string TodaySaleOrderCount { get; set; }
        public string TodaySaleOrderAmount { get; set; }
        public string MTDSaleOrderCount { get; set; }
        public string MTDSaleOrderAmount { get; set; }
        public string YTDSaleOrderCount { get; set; }
        public string YTDSaleOrderAmount { get; set; }
    }
}

