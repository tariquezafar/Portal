using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
    public  class BankStatementViewModel
    {
        public long BankStatementID { get; set; }
        public string BankStatementNo { get; set; }
        public string BankStatementDate { get; set; }
        public long BankBookId { get; set; }
        public string BankBookName { get; set; }
        public string BankBranch { get; set; }
        public string BankStatementFromDate { get; set; }
        public string BankStatementToDate { get; set; }
        public string Remarks { get; set; }
        public int CompanyId { get; set; }
        public int CompanyBranchId { get; set; }

        public string CompanyBranchName { get; set; }
        public int CreatedBy { get; set; }

        public string CreatedByName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public string ModifiedDate { get; set; }
        public string BankStatementStatus { get; set; }
        public int BankStatementSequence { get; set; }
    }

    public class BankStatementDetailViewModel
    {
        public int SequenceNo { get; set; }
        public int BankStatementDetailId { get; set; }
        public string TransactionDate { get; set; }
        public string ChequeNumber { get; set; }
        public decimal Withdrawal { get; set; }
        public decimal Deposit { get; set; }
        public decimal Balance { get; set; }
        public string Narration { get; set; }
    }
}
