using Portal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
    public class DispatchViewModel
    {
        public int DispatchID { get; set; }
        public string DispatchDate { get; set; }
        public string DispatchNo { get; set; }
        public int DispatchPlanID { get; set; }
        public string DispatchPlanNo { get; set; }
        public string DispatchPlanDate { get; set; }
        public int CompanyBranchID { get; set; }
        public int CreatedBy { get; set; }
        public string BranchName { get; set; }
        public string ApprovalStatus { get; set; }

    }

    
    public class DispatchProductDetailViewModel
    {
        public int SOId { get; set; }
        public string SONo { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string CustomerName { get; set; }
        public string City { get; set; }
        public int? ProductId { get; set; }     
        public decimal Quantity { get; set; }
        public decimal Priority { get; set; }

    }

}
