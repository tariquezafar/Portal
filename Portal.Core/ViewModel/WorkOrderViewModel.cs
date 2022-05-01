using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class WorkOrderViewModel
    {


        public long WorkOrderId { get; set; }
        public string WorkOrderNo { get; set; }
        public string WorkOrderDate { get; set; }
        
        public string SODate { get; set; }
        public long SOId { get; set; }
        public string SONo { get; set; }
        public string CancelStatus { get; set; }
        public int CancelBy { get; set; }
        public string CancelByUserName { get; set; }
        public string CancelDate { get; set; }
        public string CancelReason { get; set; }
        public string TargetFromDate { get; set; }
        public string TargetToDate { get; set; }
        public int CompanyId { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }

        public string AssemblyType { get; set; }

        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public string WorkOrderStatus { get; set; }

        public decimal Quantity { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string  CustomerName { get; set; }
        public string ProductName { get; set; }

        public decimal WOQTY { get; set; }
        public decimal FinishedGoodQTY { get; set; }

    }
    public class WorkOrderProductViewModel
    {
        public long WorkOrderProductDetailId { get; set; }
        public int SequenceNo { get; set; }
        public int WorkOrderId { get; set; }
        public int ProductId { get; set; }
        public int AdjProDuctID { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public string AssemblyType { get; set; }
        public string UOMName { get; set; }
        public string NewProduct { get; set; }
        public decimal Quantity { get; set; }
        public string IsSerializedProduct { get; set; }
        public decimal WorkorderQuantity { get; set; }
        public decimal TotalRecivedFabQuantity { get; set; }
        public decimal TotalPaintQuantity { get; set; }
        public decimal PendingQuantity { get; set; }
        public decimal RecivedQuantity { get; set; }
        public decimal RepQTY { get; set; }
        public string message { get; set; }
        public string status { get; set; }

        public string IsThirdPartyProduct { get; set; }

    }
   
}
