using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class SLViewModel
    {
        public long SLId { get; set; }
        public string SLCode { get; set; }
        public string SLHead { get; set; }
        public string RefCode { get; set; }
        public int SLTypeId { get; set; }
        public string  SLTypeName { get; set; }
        public int CostCenterId { get; set; }
        public string CostCenterName { get; set; }
        public int SubCostCenterId { get; set; }
        public int CompanyId { get; set; }
        public bool SL_Status { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedName { get; set; }
        public string ModifiedDate { get; set; }
        public string message { get; set; }
        public bool status { get; set; } 
        public int PostingGLId { get; set; }
        public string GLHead { get; set; }

        public long CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }

    }

    public class SLDetailViewModel
    {
        public long SLDetailId { get; set; }
        public int GLId { get; set; }
        public long SLId { get; set; }
        public string SLCode { get; set; }
        public string SLHead { get; set; }
        public int SLTypeId { get; set; }
        public string SLTypeName { get; set; }
        public int CompanyId { get; set; }
        public int FinYearId { get; set; }

      
        public decimal OpeningBalanceDebit { get; set; }
        public decimal OpeningBalanceCredit { get; set; }
        public decimal OpeningBalance { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedName { get; set; }
        public string ModifiedDate { get; set; }
        public bool SLDetailStatus { get; set; }
        public long CompanyBranchId { get; set; }
    }
}
