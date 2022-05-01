using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class CarryForwardChasissViewModel
    {      
        public long CarryForwardID { get; set; }
        public string CarryForwardNo { get; set; }
        public string CarryForwardDate { get; set; }      
        public long CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
        public int CompanyId { get; set; }               
        public int CreatedBy { get; set; }

        public int PrevChasisMonth { get; set; }
        public string ChasisMonth { get; set; }
        public int PrevChasisYear { get; set; }
        public int CarryForwardMonth { get; set; }
        public string CarryMonth { get; set; }
        public int CarryForwardYear { get; set; }

        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }      
      
        public string ApprovalStatus { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    public class CarryForwardChasisDetailViewModel
    {        
        public long CarryForwardDetailID { get; set; }
        public long CarryForwardID { get; set; }
        public long ChasisSerialDetailID { get; set; }
        public int SequenceNo { get; set; }
        public int ChasisModelID { get; set; }       
        public string MotorNo { get; set; }
        public string ChasisSerialNo { get; set; }
        public int CarryForwardMonth { get; set; }
        public string CarryMonth { get; set; }
        public int CarryForwardYear { get; set; }
        public string PrintChasisFlag { get; set; }
        public string FabricatedFlag { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        
    }
  
}
