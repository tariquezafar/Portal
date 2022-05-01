using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class AssemblingProcessViewModel
    {      
        public long AssemblingProcessId { get; set; }
        public string AssemblingProcessNo { get; set; }
        public string AssemblingProcessDate { get; set; }
        public long WorkOrderId { get; set; }
        public string WorkOrderNo { get; set; }
        
        public long PaintProcessId { get; set; }
        public string PaintProcessNo { get; set; }
        public int CompanyId { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public string AssemblingProcessStatus { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal PaintProcessQuantity { get; set; }
        public string message { get; set; }
        public string status { get; set; }

        public string CancelReason { get; set; }

    }
    public class AssemblingProcessProductViewModel
    {
        
        public long AssemblingProcessDetailId { get; set; }
        public int SequenceNo { get; set; }
        public int AssemblingProcessId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public string UOMName { get; set; }
        public decimal WorkorderQuantity { get; set; }
        public decimal TotalRecivedAssembledQuantity { get; set; }
        public decimal TotalPaintQuantity { get; set; }
        public decimal TotalAssembledQuantity { get; set; }
        public decimal TotalRecivedFinishedGoodQuantity { get; set; }
        public decimal PendingQuantity { get; set; }
        public decimal RecivedQuantity { get; set; }
        public decimal Quantity { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string IsThirdPartyProduct { get; set; }

        

    }
    public class AssemblingProcessChasisSerialViewModel
    {
        public long AssemblingProcessChasisId { get; set; }
        public int AssemblingProcessId { get; set; }
        public int ProductId { get; set; }
        public int MatchProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public string ChasisSerialNo { get; set; }
        public string PaintFlag { get; set; }
        public string AssembledFlag { get; set; }
        public string MotorNo { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }

}
