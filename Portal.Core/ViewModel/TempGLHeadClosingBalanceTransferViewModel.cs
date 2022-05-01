using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class TempGLHeadClosingBalanceTransferViewModel
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
        public decimal ClosingBalanceDebit { get; set; }
        public decimal ClosingBalanceCredit { get; set; }
        public decimal ClosingBalance { get; set; }

    }


    public class GLTransferClosingBalanceViewModel
    {
       
        public long GLTransferId { get; set; }
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
