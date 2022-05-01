using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
 public class InventoryDashboardViewModel
    {
        
    }
    public class ReorderPointProductCountViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public string ProductFullDesc { get; set; }


        public int ReorderQty{ get; set; }
        public int AvailableStock { get; set; }
        
    }
    public class ProductQuantityCountViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public int OpeningQty { get; set; }
        public int PurchaseQty { get; set; }
        public int SaleQty { get; set; }
        public int STNQty { get; set; }
        public int STRQty { get; set; }
        public int ClosingQty { get; set; }
        


    }

    public class SINProductQuantityCountViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }      
        public int Qty { get; set; }      

    }



}
