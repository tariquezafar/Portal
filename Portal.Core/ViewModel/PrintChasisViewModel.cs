using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class PrintChasisViewModel
    {      
        public long PrintID { get; set; }
        public string PrintNo { get; set; }
        public string PrintDate { get; set; }      
        public long CompanyBranchId { get; set; }

        public string CompanyBranchName { get; set; }
        public int CompanyId { get; set; }               
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }      
        public string Consumed { get; set; }    
        
        public string ApprovalStatus { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    public class PrintChasisDetailViewModel
    {        
        public long PrintDetailID { get; set; }
        public long ChasisSerialDetailID { get; set; }
        public int SequenceNo { get; set; }
        public long PrintChasisSerialID { get; set; }       
        public string ChasisSerialNo { get; set; }
        public string MotorNo { get; set; }
        public string PrintStatus { get; set; }
        public string ProductSubGroupName { get; set; }
        public string ChasisModelCode { get; set; }
        public string MotorModelCode { get; set; }
        public string PrintChasisFlag { get; set; }
        public string FabricatedFlag { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        
    }
   
}
