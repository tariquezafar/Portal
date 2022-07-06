using Portal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
    public class DispatchPlanViewModel
    {
        public int DispatchPlanID { get; set; }
        public string DispatchPlanDate { get; set; }
        public string DispatchPlanNo { get; set; }
        public int CustomerID { get; set; }
        public int CompanyBranchID { get; set; }
        public int CreatedBy { get; set; }

        public string CustomerName { get; set; }
        public string BranchName { get; set; }

        public string ApprovalStatus { get; set; }


    }

    
    public class DispatchPlanProductDetailViewModel
    {
        public int SOId { get; set; }
        public int? ProductId { get; set; }     
        public decimal Quantity { get; set; }

        public decimal Priority { get; set; }

    }

}
