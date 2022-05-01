using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class TempSLHeadClosingBalanceTransferViewModel
    {

        public long SLDetailId { get; set; }
        public int GLId { get; set; }
        public long SLId { get; set; }
        public string SLCode { get; set; }
        public string SLHead { get; set; }
        public string GLCode { get; set; }
        public string GLHead { get; set; }
        public int SLTypeId { get; set; }
        public string SLTypeName { get; set; }
        public int CompanyId { get; set; }
        public int FinYearId { get; set; }
        public decimal ClosingBalanceDebit { get; set; }
        public decimal ClosingBalanceCredit { get; set; }
        public decimal ClosingBalance { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedName { get; set; }
        public string ModifiedDate { get; set; }
        public bool SLDetailStatus { get; set; }
       

    }


    public class SLTransferClosingBalanceViewModel
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
