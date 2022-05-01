using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
 public  class PurchaseInvoiceImportViewModel
    {
        public long InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }


        public string CustomerInvoiceNo { get; set; }
        public string CustomerInvoiceDate { get; set; }
        public string CustomStn { get; set; }
        public string CHA { get; set; }
        public string BENo { get; set; }
        public string BEDate { get; set; }
        public string CC { get; set; }
        public string Type { get; set; }
        public long POId { get; set; }
        public string PONo { get; set; }
        public int ConsigneeId { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeCode { get; set; }

        public string ShippingAddress { get; set; }
        public string ShippingCity { get; set; }
        public int ShippingStateId { get; set; }
        public int ShippingCountryId { get; set; }
        public string ShippingPinCode { get; set; }
        public string ConsigneeGSTNo { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorCode { get; set; }

        public string BillingAddress { get; set; }
        public int CountryId { get; set; }
        public string PinCode { get; set; }
        public string PANNo { get; set; }
        public string GSTNo { get; set; }
        public string ExciseNo { get; set; }
        public string LocalIGMNo { get; set; }
        public string LocalIGMDate { get; set; }
        public string GatewayIGMNo { get; set; }
        public string GatewayIGMDate { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfReporing { get; set; }
        public int CountryOfOrigin { get; set; }
        public int CountryOfConsignee { get; set; }
        public string BLNo { get; set; }
        public string BLDate { get; set; }
        public string HBLNo { get; set; }
        public string HBLDate { get; set; }
        public decimal NoOfPkgs { get; set; }
        public int NoOfPkgsUnit { get; set; }
        public decimal GrossWt { get; set; }
        public int GrossWtUnit { get; set; }
        public string Mark { get; set; }
        public string SupplierInvoiceNo { get; set; }
        public string SupplierInvoiceDate { get; set; }
        public decimal InvoiceValue { get; set; }
        public string InvoiceValueCurrency { get; set; }
        public string TOI { get; set; }
        public decimal Freight { get; set; }
        public string FreightCurrency { get; set; }
        public decimal Insurence { get; set; }
        public string InsurenceCurrency { get; set; }
        public string SVBLoadingASS { get; set; }
        public string SVBLoadingDty { get; set; }
        public string CustHouseNo { get; set; }
        public decimal HSSLoadRate { get; set; }
        public decimal HSSAmount { get; set; }
        public decimal MiscCharges { get; set; }
        public decimal DiscountPercRate { get; set; }
        public decimal DiscountAmount { get; set; }
        public string EDD { get; set; }
        public string ThirdParty { get; set; }
        public decimal XBEDutyFGInt { get; set; }
        public string BuyerSellarRelted { get; set; }
        public string PaymentMethod { get; set; }
        public decimal ExchangeRate { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public string Remarks { get; set; }
        public int FinYearId { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public string InvoiceStatus { get; set; }
        public string ApprovalStatus { get; set; }
        public int ApprovedBy { get; set; }
        public decimal ApprovedDate { get; set; }
        public string RejectionStatus { get; set; }
        public int RejectedBy { get; set; }
        public string RejectedDate { get; set; }
        public string RejectedReason { get; set; }
        public string CancelStatus { get; set; }
        public int CancelBy { get; set; }
        public string CancelDate { get; set; }
        public string CancelReason { get; set; }
        public decimal EducationalCessOnCVDPerc { get; set; }
        public decimal EducationalCessOnCVDAmt { get; set; }
        public decimal SecHigherEduCessOnCVDPerc { get; set; }
        public decimal SecHigherEduCessOnCVDAmt { get; set; }
        public decimal CustomSecHigherEduCessPerc { get; set; }
        public decimal CustomSecHigherEduCessAmt { get; set; }
        public decimal CustomEducationalCessPerc { get; set; }
        public decimal CustomEducationalCessAmt { get; set; }
        public int InvoiceSequence { get; set; }

        public decimal BasicValue { get; set; }
        public decimal TotalValue { get; set; }
        public decimal RoundOfValue { get; set; }
        public decimal GrossValue { get; set; }
        public string AdCode { get; set; }

        public string ExchangeRatecurrencyName { get; set; }
        public decimal ForeigncurrencyValue { get; set; }
        public string ForeigncurrencyName { get; set; }
    }
    public  class PurchaseInvoiceImportProductDetailViewModel
    {
        public int SequenceNo { get; set; }
        public long InvoiceProductDetailId { get; set; }
        public long InvoiceId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public string Currency { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public decimal AcceptQuantity { get; set; }
        public decimal RejectQuantity { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal IGST_Perc { get; set; }
        public decimal IGST_Amount { get; set; }
        public string HSN_Code { get; set; }
        public decimal Total { get; set; }
        public string RITC { get; set; }
        public decimal AssValue { get; set; }
        public decimal CTH { get; set; }
        public string CETH { get; set; }
        public string CNotn { get; set; }
        public string ENotn { get; set; }
        public string RSP { get; set; }
        public decimal CustomDtyRate { get; set; }
        public decimal LoadProv { get; set; }
        public decimal BCDAmtRs { get; set; }
        public decimal CVDAmtRs { get; set; }
        public decimal CustomDutyPerc { get; set; }
        public decimal CustomDutyRate { get; set; }
        public decimal ExciseDutyPerc { get; set; }
        public decimal ExciseDutyRate { get; set; }
        public decimal IGST { get; set; }
        public decimal GSTCess { get; set; }
        public string UOMName { get; set; }
        public decimal GSTCessPerc { get; set; }
        public decimal GSTCessAmt { get; set; }
        public string Cnsno { get; set; }
        public string Ensno { get; set; }

        //-------------Dheeraj Declare Properties for Child Table
        public decimal EducationalCessOnCVDPerc { get; set; }
        public decimal EducationalCessOnCVDAmt { get; set; }
        public decimal SecHigherEduCessOnCVDPerc { get; set; }
        public decimal SecHigherEduCessOnCVDAmt { get; set; }
        public decimal CustomSecHigherEduCessPerc { get; set; }
        public decimal CustomSecHigherEduCessAmt { get; set; }
        public decimal CustomEducationalCessPerc { get; set; }
        public decimal CustomEducationalCessAmt { get; set; }

    }

    public class PurchaseInvoiceImportTermsDetailViewModel
    {
        public long InvoiceTermDetailId { get; set; }
        public long InvoiceId { get; set; }
        public string TermDesc { get; set; }
        public int TermSequence { get; set; }
        public string message { get; set; }
        public string status { get; set; }
    }
}
