using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
    public class VoucherViewModel
    {
        public long VoucherId { get; set; }
        public string VoucherNo { get; set; }
        public int VoucherNoSequence { get; set; }
        public string VoucherDate { get; set; }
        public string VoucherType { get; set; }
        public string VoucherMode { get; set; }
        public decimal VoucherAmount { get; set; }
        public int PayeeSLTypeId { get; set; }
        public int BookId { get; set; }
        public int CompanyId { get; set; }
        public int FinYearId { get; set; }
        public long ContraVoucherId { get; set; }
        public string ContraVoucherNo { get; set; }
        public int ContraBookId { get; set; }
        public int ContraCompanyId { get; set; }
        public string VoucherStatus { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedName { get; set; }
        public string ModifiedDate { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public string ApprovedDate { get; set; }
        public int CancelledBy { get; set; }
        public string CancelledByName { get; set; }
        public string CancelledDate { get; set; }
        public string CancelReason { get; set; }
        public string message { get; set; }
        public bool status { get; set; }

        public long CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
    }
    public class VoucherDetailViewModel
    {
        public long VoucherDetailId { get; set; }
        public long VoucherId { get; set; }
        
        public int SequenceNo { get; set; }
        public string EntryMode { get; set; }
        public int GLId { get; set; }
        public string GLCode { get; set; }
        public string GLHead { get; set; }
        public int SLTypeId { get; set; }
        public string SLTypeName { get; set; }
        public long SLId { get; set; }
        public string SLCode { get; set; }
        public string SLHead { get; set; }
        public string Narration { get; set; }
        public int PaymentModeId { get; set; }
        public string PaymentModeName { get; set; }
        public string ChequeRefNo { get; set; }
        public string ChequeRefDate { get; set; }
        public decimal Amount { get; set; }
        public int CostCenterId { get; set; }
        public string CostCenterName { get; set; }
        public string ValueDate { get; set; }
        public string DrawnOnBank { get; set; }
        public string PO_SONo { get; set; }
        public string BillNo { get; set; }
        public string BillDate { get; set; }
        public int PayeeId { get; set; }
        public string PayeeName { get; set; }
        public bool AutoEntry { get; set; }
    }

    public class VoucherSupportingDocumentViewModel
    {
        public int DocumentSequenceNo { get; set; }
        public long VoucherDocId { get; set; }
        public int VoucherId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentTypeDesc { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
}
