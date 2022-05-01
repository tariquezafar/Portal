using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class PaintProcessViewModel
    {      
        public long PaintProcessId { get; set; }
        public string PaintProcessNo { get; set; }
        public string PaintProcessDate { get; set; }
        public string WorkOrderDate { get; set; }
        
        public long WorkOrderId { get; set; }
        public string WorkOrderNo { get; set; }       
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
        public string PaintProcessStatus { get; set; }
        public decimal PaintProcessQuantity { get; set; }
        public decimal Quantity { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public decimal TotalQuantity { get; set; }

        public int FinYearId { get; set; }
        public string CancelStatus { get; set; }
        public int CancelBy { get; set; }
        public string CancelDate { get; set; }
        public string CancelReason { get; set; }

    }
    public class PaintProcessProductViewModel
    {
        
        public long PaintProcessDetailId { get; set; }
        public int SequenceNo { get; set; }
        public int PaintProcessId { get; set; }
        public int AdjProDuctID { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public string NewProduct { get; set; }
        public string UOMName { get; set; }
        public decimal WorkorderQuantity { get; set; }
        public decimal TotalRecivedAssembledQuantity { get; set; }
        public decimal TotalPaintQuantity { get; set; }
        public decimal PendingQuantity { get; set; }
        public decimal RecivedQuantity { get; set; }
        public decimal Quantity { get; set; }
        public decimal RepQTY { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string IsThirdPartyProduct { get; set; }        
    }

    public class ProductSerialDetailViewModel
    {
        public string ChasisSerialNo { get; set; }
        public string MotorNo { get; set; }
        public int ProductId { get; set; }
    }

    public class PaintProcessChasisSerialViewModel
    {
        public long PaintProcessChasisID { get; set; }
        public int PaintProcessDetailId { get; set; }       
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public string ChasisSerialNo { get; set; }
        public string PaintFlag { get; set; }
        public string MotorNo { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }

}
