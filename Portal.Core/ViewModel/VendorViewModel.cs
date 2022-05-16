using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class VendorViewModel
    {  
        public int VendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string ContactPersonName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string ContactNo { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }
        public string PinCode { get; set; }
        public string CSTNo { get; set; }
        public string TINNo { get; set; }
        public string PANNo { get; set; }
        public string GSTNo { get; set; } 
        public string ExciseNo { get; set; }
        public int CompanyId { get; set; }
        public decimal CreditLimit { get; set; }
        public int CreditDays { get; set; }
        public decimal AnnualTurnover { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public bool Vendor_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public bool GST_Exempt { get; set; }
        public bool IsComposition { get; set; }
        public Int64 CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }

        public string StateCode { get; set; }


    }

    public class VendorProductViewModel
    {
        public long MappingId { get; set; }
        public int VendorId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
}
