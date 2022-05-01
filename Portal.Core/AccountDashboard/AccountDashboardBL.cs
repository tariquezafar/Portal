using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Core.ViewModel;
using Portal.DAL;
using Portal.Common;
using System.Reflection;
using System.Data;

namespace Portal.Core
{
    public class AccountDashboardBL
    {
        
        public AccountDashboardBL()
        {
            
        }
        public List<BookBalanceViewModel> GetBookBalanceList(int companyId,int finYearId,int companyBranchId,out string bookList,out string balanceList)
        {
            
            List<BookBalanceViewModel> bookBalanceList = new List<BookBalanceViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtbookBalance = sqlDbInterface.GetDashboard_BookBalance(companyId,finYearId,companyBranchId);
                if (dtbookBalance != null && dtbookBalance.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtbookBalance.Rows)
                    {
                        bookBalanceList.Add(new BookBalanceViewModel
                        {
                            BookId = Convert.ToInt32(dr["BookId"]),
                            BookName = Convert.ToString(dr["BookName"]),
                            GLId = Convert.ToInt32(dr["GLId"]),
                            BookBalance = Convert.ToDecimal(dr["BookBalance"]),
                            BookType = Convert.ToString(dr["BookType"])
                        });
                    }
                }
                var bookNames = (from temp in bookBalanceList
                                 select temp.BookName).ToList();

                var balanceTotal = (from temp in bookBalanceList
                                    select temp.BookBalance).ToList();

                bookList ="'"+ string.Join("','", bookNames) + "'";


                balanceList = string.Join(",", balanceTotal);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bookBalanceList;
        }

        public List<MonthWiseCashBankTrnSummaryViewModel> GetMonthWiseBankCashTrnList(int companyId, int finYearId,int companyBranchId, out string monthList, out string bankPaymentList,out string bankReceiptList,out string cashPaymentList,out string cashReceiptList)
        {
            List<MonthWiseCashBankTrnSummaryViewModel> trnList = new List<MonthWiseCashBankTrnSummaryViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtbookBalance = sqlDbInterface.GetDashboard_MonthWiseBankCashTransactionSummary(companyId, finYearId, companyBranchId);
                if (dtbookBalance != null && dtbookBalance.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtbookBalance.Rows)
                    {
                        trnList.Add(new MonthWiseCashBankTrnSummaryViewModel
                        {
                            MonthId = Convert.ToInt16(dr["MonthId"]),
                            MonthNo = Convert.ToInt16(dr["MonthNo"]),
                            MonthShortName = Convert.ToString(dr["MonthShortName"]),
                            BankPayment = Convert.ToDecimal(dr["BankPayment"]),
                            BankReceipt = Convert.ToDecimal(dr["BankReceipt"]),
                            CashPayment = Convert.ToDecimal(dr["CashPayment"]),
                            CashReceipt = Convert.ToDecimal(dr["CashReceipt"])
                        });
                    }
                }
                var monthNames = (from temp in trnList
                                 select temp.MonthShortName).ToList();

                var bankPaymentTrans = (from temp in trnList
                                        select temp.BankPayment).ToList();
                var bankReceiptTrans = (from temp in trnList
                                        select temp.BankReceipt).ToList();
                var cashPaymentTrans = (from temp in trnList
                                        select temp.CashPayment).ToList();
                var cashReceiptTrans = (from temp in trnList
                                        select temp.CashReceipt).ToList();

                monthList = "'" + string.Join("','", monthNames) + "'";
                bankPaymentList = string.Join(",", bankPaymentTrans);
                bankReceiptList = string.Join(",", bankReceiptTrans);
                cashPaymentList = string.Join(",", cashPaymentTrans);
                cashReceiptList = string.Join(",", cashReceiptTrans);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return trnList;
        }









    }
}








