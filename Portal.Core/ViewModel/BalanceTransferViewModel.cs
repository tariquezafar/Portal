using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class BalanceTransferViewModel
    {
     
        public long ProductId { get; set; }
       
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public long ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public long ProductMainGroupId { get; set; }
        public string ProductMainGroupName { get; set; }
        public long ProductSubGroupId { get; set; }
        
        public string ProductSubGroupName { get; set; }
        public string AssemblyType { get; set; }

        public int CompanyId { get; set; }
        public int UOMId { get; set; }
        public string UOMName { get; set; }
        public string TrnType { get; set; }
        public string TrnTypeName { get; set; }
        public string TrnDate { get; set; }
        
        public decimal TrnQty { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public int CompanyBranchId { get; set; }
        public string BranchName { get; set; }
        
        public decimal OpeningQty { get; set; }
        public decimal PurchaseQty { get; set; }
        public decimal SaleQty  { get; set; }

        public decimal StockInQty { get; set; }
        public decimal StockOutQty { get; set; }
        public decimal ClosingQty{ get; set; }


        public string message { get; set; }
        public string status { get; set; }
        public string  ProcessType { get; set; }

    }


    public class TransferClosingBalanceViewModel
    {
       
        public long TransferId { get; set; }
        public int FromFinYearID { get; set; }
        public int ToFinYearID { get; set; }
        public long CompanyBranchId { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }

        public string BranchName { get; set; }
        public string UserName { get; set; }


    }

}
