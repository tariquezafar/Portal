using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
  public class GLViewModel
    {
        public int GLId { get; set; }
        public string GLCode { get; set; }
        public string GLHead { get; set; }
        public string GLType { get; set; }
        public int GLSubGroupId { get; set; }
        public string GLSubGroupName { get; set; }
        public int GLMainGroupId { get; set; }
        public string GLMainGroupName { get; set; }
        public int SLTypeId { get; set; }
        public string SLTypeName { get; set; }
        public bool IsBookGL { get; set; }
        public bool IsBranchGL { get; set; }
        public bool IsDebtorGL { get; set; }
        public bool IsCreditorGL { get; set; }
        public bool IsTaxGL { get; set; }
        public bool IsPostGL { get; set; }

        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public bool GLStatus { get; set; }

        public int FinYearId { get; set; }
        public decimal OpeningBalanceDebit { get; set; }
        public decimal OpeningBalanceCredit { get; set; }
        public decimal OpeningBalance { get; set; }
        public long CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }

        public string FinyearDesc { get; set; }
    }
    public  class GLDetailViewModel
    {
        public long GLDetailId { get; set; }
        public int GLId { get; set; }
        public int CompanyId { get; set; }
        public int FinYearId { get; set; }
        public long CompanyBranchId { get; set; }
        public decimal OpeningBalanceDebit { get; set; }
        public decimal OpeningBalanceCredit { get; set; }
        public decimal OpeningBalance { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public bool GLDetailStatus { get; set; }
    }
}
