using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
     
    public class VendorPaymentViewModel
    {
        public long PaymentTrnId { get; set; }
        public string PaymentNo { get; set; }
        public string PaymentDate { get; set; }
        public int  VendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public long  InvoiceId { get; set; }
        public string InvoiceNo { get; set; } 
        public string InvoiceDate { get; set; }
        public int  BookId { get; set; } 
        public string BookType { get; set; }
        public string BookName { get; set; }
        public string BookCode { get; set; }
        public string BankBranch { get; set; }
        public string BankIFSCCode { get; set; }
        public  int  PaymentModeId { get; set; }
        public string PaymentModeName { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public decimal Amount { get; set; }
        public string ValueDate { get; set; }
        public string Remarks { get; set; }
        public int FinYearId { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; } 
        public string VendorPaymentSequence { get; set; }
        public bool VendorPayment_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }
    }




















}
