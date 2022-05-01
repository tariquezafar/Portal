using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class STNViewModel
    {
        public long STNId { get; set; }
        public string STNNo { get; set; }
        public string STNDate { get; set; }
        public string GRNo { get; set; }
        public string GRDate { get; set; }
        public long InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ContactPerson { get; set; }
        public long FromWarehouseId { get; set; }
        public string FormLocationName { get; set; }  
        public string FormPrimaryAddress { get; set; } 
        public string FormCity { get; set; } 
        public string FormPinCode { get; set; } 
        public string FormCSTNo { get; set; }
        public string FormTINNo { get; set; } 
        public long ToWarehouseId { get; set; } 
        public string ToPrimaryAddress { get; set; }
        public string ToCity { get; set; }
        public string ToPinCode { get; set; }
        public string ToCSTNo { get; set; }
        public string ToTINNo { get; set; } 
        public string ToLocationName { get; set; }
        public string DispatchRefNo { get; set; }
        public string DispatchRefDate { get; set; }
        public string LRNo { get; set; }
        public string LRDate { get; set; }
        public string TransportVia { get; set; }
        public int NoOfPackets { get; set; }
        public decimal BasicValue { get; set; }
        public decimal TotalValue { get; set; }
        public decimal FreightValue { get; set; }
        public decimal LoadingValue { get; set; }
        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }
        public int FinYearId { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public string STNStatus { get; set; }
        public string ApprovalStatus { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedDate { get; set; }
        public string RejectionStatus { get; set; }
        public int RejectedBy { get; set; }
        public string RejectedDate { get; set; }
        public string RejectedReason { get; set; }
        public int STNSequence { get; set; }
    }
    public class STNProductDetailViewModel
    {
        public long STNProductDetailId { get; set; }
        public long STNId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public decimal Price { get; set; }
        public string UOMName { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string IsSerializedProduct { get; set; }

        public int SequenceNo { get; set; }
    }


    public class STNSupportingDocumentViewModel
    {
        public int DocumentSequenceNo { get; set; }
        public long STNDocId { get; set; }
        public int STNId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentTypeDesc { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }

    public class STNChasisProductSerialDetailViewModel
    {
        public long SequenceNo { get; set; }

        public string ProductName { get; set; }
        public string RefSerial1 { get; set; }

      
        public int ProductId { get; set; }

        public decimal Price { get; set; }
        public int Status { get; set; }
        public int StnId { get; set; }
        public int STNProductDetailId { get; set; }

        public string Remarks { get; set; }

        public string IsSerializedProduct { get; set; }
    }

    public class STNCountViewModel
    {
        public string STNTodayCount { get; set; }
        public string STNMtdCount { get; set; }
        public string STNYtdCount { get; set; }
        public string STNCount_Head { get; set; }


    }

}
