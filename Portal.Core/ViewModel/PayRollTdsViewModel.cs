using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class PayRollTdsViewModel
    {
        public int TdsSlaBid { get; set; }
        public int Companyid { get; set; }
        public int CompanyBranchid { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
        public string Category { get; set; }
        public decimal TDSPerc { get; set; }
        public decimal CessPerc { get; set; }
        public decimal SurcharegePerc { get; set; }
        public decimal SurchargePerc2 { get; set; }
        public decimal SurchargePerc3 { get; set; }
        public decimal YearlyDiscount { get; set; }
        public decimal MonthlyDiscount { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedbyUserName { get; set; }
        public string ModifiedDate { get; set; }







    }
}
