using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class ChasisSerialPlanViewModel
    {      
        public long ChasisSerialPlanID { get; set; }
        public string ChasisSerialPlanNo { get; set; }
        public string ChasisSerialPlanDate { get; set; }
        public int  ChasisMonth { get; set; }
        public int ChasisYear { get; set; }
        public long CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
        public int CompanyId { get; set; }
        
        public int TotalChasisSerialNo { get; set; }      
        public int LastIncreamentNo { get; set; }      
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public string ApprovalStatus { get; set; }
        public string Month { get; set; }
        public string Consumed { get; set; }        
        public string message { get; set; }
        public string status { get; set; }

    }
    public class ChasisSerialPlanProductViewModel
    {
        
        public long ChasisModelID { get; set; }
        public long ProductSubGroupId { get; set; }
        public string ChasisModelName { get; set; }
        public string ChasisModelCode { get; set; }
        public string MotorModelCode { get; set; }
        public int QtyProduced { get; set; }
        public int CarryForwardQTY { get; set; }
        public int LastIncreamentNo { get; set; }
        public string ProductSubGroupName { get; set; }

        public string CarryForwardTrueORNot { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        
    }
    public class ChasisSerialPlanDetailViewModel
    {
        public long ChasisSerialDetailID { get; set; }
        public long ChasisSerialPlanID { get; set; }
        public int ChasisModelID { get; set; }
        public string ChasisSerialNo { get; set; }
        public string MotorNo { get; set; }
        public string IncrementalNo { get; set; }
    }
    public class ChasisSerialModelDetailViewModel
    {
        public long ChasisSerialModelDetailID { get; set; }
        public long ChasisSerialPlanID { get; set; }
        public int ChasisModelID { get; set; }
        public decimal QtyProduced { get; set; }
        public int LastIncreamentNo { get; set; }
    }

    public class ChasisNoDetailedModelDetailViewModel
    {
        public string InvoiceNo { get; set; }
        public string partyname { get; set; }
        public string ShippingBillingAddress { get; set; }
        public string ProductShortDesc { get; set; }
        public string DespatchedDate { get; set; }
        public string chasisno { get; set; }
        public string FinishedGoodProcessDate { get; set; }
        

    }

    public class ChasisNoTrackingDetailedViewModel
    {
        public string Process { get; set; }
        public string ProcessNo { get; set; }
        public string ProcessDate { get; set; }
        public string CustomerName { get; set; }
        public string ProcessMonth { get; set; }
        public string ProcessYear { get; set; }
        public string WorkOrderNo { get; set; }
        public string WorkOrderDate { get; set; }
        public string ProductName { get; set; }
        public string ProcessStatus { get; set; }
        public string Createdby { get; set; }
        public string CompanyBranchName { get; set; }
       


    }





}
