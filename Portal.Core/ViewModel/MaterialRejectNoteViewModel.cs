using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
    public class MaterialRejectNoteViewModel
    {
        public long MaterialReceiveNoteId { get; set; }
        public string MaterialReceiveNoteNo { get; set; }
        public string MaterialReceiveNoteDate { get; set; }
        public long QualityCheckId { get; set; }
        public string QualityCheckNo { get; set; }

        public string QualityCheckDate { get; set; }
        public long GateInId { get; set; }
        public string GateInNo { get; set; }
        public long POID { get; set; }
        public string PONo { get; set; }
        public string PoDate { get; set; }
        public int CompanyBranchId { get; set; }
        public int CompanyId { get; set; }
        public int FinYearId { get; set; }
        public string Remarks { get; set; }
        public string RejectRemarks { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string ApprovalStatus { get; set; }
        public long MaterialReceiveNoteSequenceId { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }

        public string GateInDate { get; set; }
        public string CompanyBranchName { get; set; }
      
        public string CreatedByUserName { get; set; }
        public string ModifiedByUserName { get; set; }


    }

    public class MaterialRejectNoteProductDetailViewModel
    {
        public long MaterialRejectNoteProductDetailId { get; set; }
        public long MaterialReceiveNoteId { get; set; }
        public long ProductId { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public decimal AcceptQuantity { get; set; }
        public decimal RejectQuantity { get; set; }
        public string RejectMarks { get; set; }
    }


    }
