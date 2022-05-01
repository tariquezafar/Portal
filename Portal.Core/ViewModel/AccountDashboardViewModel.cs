using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
 public   class AccountDashboardViewModel
    {
        public List<BookBalanceViewModel> bookBalanceList { get; set; }
    }
    public class BookBalanceViewModel
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public int GLId { get; set; }
        public decimal BookBalance { get; set; }
        public string BookType { get; set; }
    }
    public class MonthWiseCashBankTrnSummaryViewModel
    {
        public int MonthId { get; set; }
        public int MonthNo { get; set; }
        public string MonthShortName { get; set; }
        public decimal BankPayment { get; set; }
        public decimal BankReceipt { get; set; }
        public decimal CashPayment { get; set; }
        public decimal CashReceipt { get; set; }

    }
}
