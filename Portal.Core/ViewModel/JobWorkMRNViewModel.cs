using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class JobWorkMRNViewModel
    {      
        public long JobWorkMRNId { get; set; }
        public string JobWorkMRNNo { get; set; }
        public string JobWorkMRNDate { get; set; }
        public string JobWorkMRNTime { get; set; }        
        public long JobWorkId { get; set; }
        public string JobWorkNo { get; set; }            
        public int CompanyId { get; set; }
        public int CompanyBranchId { get; set; }
        public int FinYearId { get; set; }
        public string CompanyBranchName { get; set; }
        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }
        public string Remarks3 { get; set; }
        public string Remarks4 { get; set; }
        public string Remarks5 { get; set; }
        public string Remarks6 { get; set; }
        public string Remarks7 { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public string JobWorkMRNStatus { get; set; }
        
        public string message { get; set; }
        public string status { get; set; }

    }
    public class JobWorkMRNProductViewModel
    {        
        public long JobWorkMRNProductDetailId { get; set; }
        public int SequenceNo { get; set; }
        public int JobWorkMRNId { get; set; }
        public long JobWorkId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public string ProductHSNCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Weight { get; set; }
        public int UomId { get; set; }
        public string UOMName { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        
    }

 

}
