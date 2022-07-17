using Portal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
    public class ComplaintServiceViewModel:IModel
    {
        public int ComplaintId { get; set; }
        public string EnquiryType { get; set; }
        public string ComplaintMode { get; set; }
        public string ComplaintDescription { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public string Status { get; set; }
        public int CustomerId { get; set; }
        public bool ComplaintService_Status { get; set; }
        public long Productid { get; set; }
        public string ComplaintNo { get; set; }
        public int ComplaintSequence { get; set; }
        public int BranchID { get; set; }
        public string CompanyBranchName { get; set; }
        public int CompanyId { get; set; }
        public string ComplaintDate { get; set; }

        public string InvoiceDate { get; set; }
        public string InvoiceNo { get; set; }
        public int? UserId { get; set; }

        public int? DealerID { get; set; }


        public string DealerName { get; set; }
        public string EmployeeName { get; set; }
        public int ComplaintStatus { get; set; }
        public string Remarks { get; set; }
        public string ComplaintStatusCon { get; set; }

    }

    
    public class ComplaintServiceProductDetailViewModel
    {
        public int SequenceNo { get; set; }
        public long MappingId { get; set; }
        public int CustomerId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public long ComplaintProductDetailID { get; set; }
        public long ComplaintId { get; set; }
        public long ProductId { get; set; }
        public string Remarks { get; set; }
        public string WarrantyStartDate { get; set; }
        public string WarrantyEndDate { get; set; }

        public int Quantity { get; set; }

    }

    public class ComplaintServiceSupportingDocumentViewModel
    {
        public int DocumentSequenceNo { get; set; }
        public int ComplaintDocId { get; set; }
        public long ComplaintId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentTypeDesc { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }


}
