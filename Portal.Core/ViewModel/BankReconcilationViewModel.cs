using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
public class BankReconcilationViewModel
    {
        public long BankRecoID { get; set; }
        public string BankRecoNo { get; set; }
        public string BankRecoDate { get; set; }
        public long BankBookId { get; set; }
        public string BankBookName { get; set; }
        public string BankBranch { get; set; }
        public string BankRecoFromDate { get; set; }
        public string BankRecoToDate { get; set; }
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
        public string BankRecoStatus { get; set; }
        public int BankRecoSequence { get; set; }

        public decimal BookClosingBalance { get; set; }
        public string BookClosingRemarks { get; set; }
        public decimal StatementClosingBalance { get; set; }
        public string StatementClosingRemarks { get; set; }
    }

    public class BankReconcilationDetailViewModel
    {
        public int BankRecoDetailId { get; set; }
        public long BankRecoID { get; set; }
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string ChequeNumber { get; set; }
        public string ChequeRefNo { get; set; }
        public string ChequeRefDate { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public int CompanyId { get; set; }
        public string BankRecoNarration { get; set; }
        public decimal Amount { get; set; }

        public string TrnType { get; set; }
    }


    public class BankReconcilationSummaryViewModel
    {
        
        public long BankRecoSummaryId { get; set; }
        public long BankRecoID { get; set; }

        public int BookId { get; set; }
        public string AsOnDate { get; set; }
        public decimal BookClosingBalance { get; set; }
        public decimal CheqeusIssuedButNotPresentedInBankAmt { get; set; }
        public decimal ChequesReceivedButNotInBankAmt { get; set; }
        public decimal ChequeReceivedInBankButNotInBooksAmt { get; set; }
        public decimal ChequeDebitedPaidByBankButNotInBookAmt { get; set; }
        public decimal BankStatementClosingBalanceAmt { get; set; }
        public decimal ClosingBalAsPerBankStatementAmt { get; set; }
        public decimal ClosingBalOfBankAsPerBankReco { get; set; }
    }
}
