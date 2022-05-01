using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
 public   class VendorFormViewModel
    {
        public long VendorFormTrnId { get; set; }
        public int VendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public long InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public int FormTypeId { get; set; }
        public string FormTypeDesc { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public decimal Amount { get; set; }
        public string ValueDate { get; set; }
        public string Remarks { get; set; }
        public string FormStatus { get; set; }
        public int FinYearId { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public string CustomerPaymentSequence { get; set; }
        public bool CustomerForm_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }
    }
}
