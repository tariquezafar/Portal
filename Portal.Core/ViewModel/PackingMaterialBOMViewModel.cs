using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class PackingMaterialBOMViewModel
    {
        public long PMBId { get; set; }
        public string PMBNo { get; set; }
        public string PMBDate { get; set; }
        public int CompanyId { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
        public int PackingListTypeId { get; set; }
        public string PackingListTypeName { get; set; }
        public long ProductSubGroupid { get; set; }
        public string ProductSubGroupName { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public string status { get; set; }

    }
    public class PackingMaterialBOMProductViewModel
    {
        public long PMBProductDetailId { get; set; }
        public int SequenceNo { get; set; }
        public int PMBId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public string UOMName { get; set; }
        public decimal Quantity { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        
    }
   
}
