using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class ProductPurchasePIViewModel
    {
        public long InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }

        public string PONo { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorCode { get; set; }
        public string BillingAddress { get; set; }

        public string ConsigneeName { get; set; }
        public string ShippingAddress { get; set; }

        public string ShippingCity { get; set; }

        public string ConsigneeGSTNo { get; set; }



        public string City { get; set; }
        public string StateName { get; set; }
        public string GSTNO { get; set; }    
        public decimal NetAmount { get; set; }
        public decimal CGST_Amount { get; set; }
        public decimal SGST_Amount { get; set; }
        public decimal IGST_Amount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
              
        public string message { get; set; }
        public string status { get; set; }
        public string companyBranch { get; set; }

    }

}
