using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core
{
public class GateInViewModel
    {
        public long GateInId { get; set; }
        public string GateInNo { get; set; }
        public string GateInDate { get; set; }
        public string GRNo { get; set; }
        public string GRDate { get; set; }
        public long InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public long POId { get; set; }
        public string PONo { get; set; }
        public string PODate { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorCode { get; set; }
        public int CompanyBranchId { get; set; }
        public string ContactPerson { get; set; }
        public string ShippingContactPerson { get; set; }
        public string ShippingBillingAddress { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingStateName { get; set; }
        public int ShippingStateId { get; set; }
        public int ShippingCountryId { get; set; }
        public string ShippingPinCode { get; set; }
        public string ShippingTINNo { get; set; }
        public string ShippingEmail { get; set; }
        public string ShippingMobileNo { get; set; }
        public string ShippingContactNo { get; set; }
        public string ShippingFax { get; set; }
        public string DispatchRefNo { get; set; }
        public string DispatchRefDate { get; set; }
        public string LRNo { get; set; }
        public string LRDate { get; set; }
        public string TransportVia { get; set; }
        public int NoOfPackets { get; set; }
        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }
        public int FinYearId { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string GateInStatus { get; set; }
        public string ApprovalStatus { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedDate { get; set; }
        public int CancelBy { get; set; }
        public string CancelDate { get; set; }
        public string CancelReason { get; set; }
        public string RejectionStatus { get; set; }
        public int RejectedBy { get; set; }
        public string RejectedDate { get; set; }
        public string RejectedReason { get; set; }
        public int GateInSequence { get; set; }
        public string CreatedByUserName { get; set; }
        public string ModifiedByUserName { get; set; }
        public string CompanyBranchName { get; set; }
    }
public class GateInProductDetailViewModel
    {
        public int SequenceNo { get; set; }
        public long GateInProductDetailId { get; set; }
        public long GateInId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public decimal AcceptQuantity { get; set; }
        public decimal RejectQuantity { get; set; }
        public decimal TotalRecQuantity { get; set; }
        public decimal PendingQuantity { get; set; }
        public string UOMName { get; set; }
        public string Remarks { get; set; }

        public decimal QCRecQTY { get; set; }
        public decimal QCAcceptQTY { get; set; }
        public decimal QCRejectQTY { get; set; }


    }


    public class GateInSupportingDocumentViewModel
    {
        public int DocumentSequenceNo { get; set; }
        public long GateInDocId { get; set; }
        public long GateInId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentTypeDesc { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }

    }
}
