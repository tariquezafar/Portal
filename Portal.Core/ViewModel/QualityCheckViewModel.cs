using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core
{
public class QualityCheckViewModel
    {
        public long QualityCheckId { get; set; }
        public string QualityCheckNo { get; set; }
        public string QualityCheckDate { get; set; }
        public long GateInId { get; set; }
        public string GateInNo { get; set; }
        public string GateInDate { get; set; }         
        public long POId { get; set; }
        public string PONo { get; set; }
        public string PODate { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorCode { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }

        public int NoOfPackets { get; set; }
        public string Remarks { get; set; }
        public string RejectRemarks { get; set; }
        public int FinYearId { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public string Remakrs { get; set; }
        public string RejectRemakrs { get; set; }
        public int Inspectedby { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
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
        public int QualityCheckSequence { get; set; }
        public string CreatedByUserName { get; set; }
        public string ModifiedByUserName { get; set; }
    }
    public class QualityCheckProductDetailViewModel
    {
        public int SequenceNo { get; set; }
        public long QualityCheckDetailId { get; set; }
        public long QualityCheckID { get; set; }
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
        public decimal QCQuantity { get; set; }
        public string RejectRemakrs { get; set; }

    }


    public class QualityCheckSupportingDocumentViewModel
    {
        public int DocumentSequenceNo { get; set; }
        public long QualityCheckDocId { get; set; }
        public long QualityCheckId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentTypeDesc { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }

    }
}
