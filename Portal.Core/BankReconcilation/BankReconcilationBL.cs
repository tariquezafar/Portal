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

    public class BankReconcilationBL
    {
        DBInterface dbInterface;
        public BankReconcilationBL()
        {
            dbInterface = new DBInterface();
        }

        public ResponseOut AddEditBankReconcilation(BankReconcilationViewModel bankReconcilationViewModel, List<BankReconcilationDetailViewModel> bankReconcilationDetailList, BankReconcilationSummaryViewModel bankReconcilationSummaryViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                BankReconcilation bankReconcilation = new BankReconcilation
                {
                    BankRecoID = bankReconcilationViewModel.BankRecoID,
                    BankRecoDate = Convert.ToDateTime(bankReconcilationViewModel.BankRecoDate),
                    BankBookId = Convert.ToInt32(bankReconcilationViewModel.BankBookId),
                    BankRecoFromDate = Convert.ToDateTime(bankReconcilationViewModel.BankRecoFromDate),
                    BankRecoToDate = Convert.ToDateTime(bankReconcilationViewModel.BankRecoToDate),
                    BookClosingBalance = bankReconcilationViewModel.BookClosingBalance,
                    BookClosingRemarks = bankReconcilationViewModel.BookClosingRemarks,
                    StatementClosingBalance = bankReconcilationViewModel.StatementClosingBalance,
                    StatementClosingRemarks = bankReconcilationViewModel.StatementClosingRemarks,
                    Remarks = bankReconcilationViewModel.Remarks,
                    CompanyId = Convert.ToInt32(bankReconcilationViewModel.CompanyId),
                    CompanyBranchId = Convert.ToInt32(bankReconcilationViewModel.CompanyBranchId),
                    CreatedBy = bankReconcilationViewModel.CreatedBy,
                    BankRecoStatus = bankReconcilationViewModel.BankRecoStatus

                };
                List<BankReconcilationDetail> bankReconcilationDetails = new List<BankReconcilationDetail>();
                if (bankReconcilationDetailList != null && bankReconcilationDetailList.Count > 0)
                {
                    foreach (BankReconcilationDetailViewModel item in bankReconcilationDetailList)
                    {

                        bankReconcilationDetails.Add(new BankReconcilationDetail
                        {
                            BankRecoDetailId = Convert.ToInt32(item.BankRecoDetailId),
                            ChequeNumber = item.ChequeNumber,
                            BankRecoNarration = item.BankRecoNarration,
                            RefNo = item.RefNo,
                            RefDate = Convert.ToDateTime(item.RefDate),
                            Amount = Convert.ToDecimal(item.Amount),
                            TrnType = item.TrnType
                        });
                    }
                }
                BankReconcilationSummary bankReconcilationSummary = new BankReconcilationSummary
                {
                    BookId= bankReconcilationSummaryViewModel.BookId,
                    AsOnDate= Convert.ToDateTime(bankReconcilationSummaryViewModel.AsOnDate),
                    BookClosingBalance= bankReconcilationSummaryViewModel.BookClosingBalance,
                    CheqeusIssuedButNotPresentedInBankAmt=bankReconcilationSummaryViewModel.CheqeusIssuedButNotPresentedInBankAmt,
                    ChequesReceivedButNotInBankAmt=bankReconcilationSummaryViewModel.ChequesReceivedButNotInBankAmt,
                    ChequeReceivedInBankButNotInBooksAmt=bankReconcilationSummaryViewModel.ChequeReceivedInBankButNotInBooksAmt,
                    ChequeDebitedPaidByBankButNotInBookAmt=bankReconcilationSummaryViewModel.ChequeDebitedPaidByBankButNotInBookAmt,
                    BankStatementClosingBalanceAmt=bankReconcilationSummaryViewModel.BankStatementClosingBalanceAmt,
                    ClosingBalAsPerBankStatementAmt=bankReconcilationSummaryViewModel.ClosingBalAsPerBankStatementAmt,
                    ClosingBalOfBankAsPerBankReco= bankReconcilationSummaryViewModel.ClosingBalOfBankAsPerBankReco
                };
                responseOut = sqlDbInterface.AddEditBankReconcilation(bankReconcilation, bankReconcilationDetails, bankReconcilationSummary);
            }

            catch (Exception ex)
            {
                responseOut.status = ActionStatus.Fail;
                responseOut.message = ActionMessage.ApplicationException;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }

            return responseOut;
        }

        public List<BankReconcilationDetailViewModel> GetBankReconcilationDetailList(int bankBookId = 0, string fromDate = "", string toDate = "", int companyId = 0, int companyBranchId = 0, string trnType = "")
        {
            List<BankReconcilationDetailViewModel> bankReconcilations = new List<BankReconcilationDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtBankStatements = sqlDbInterface.GetBankReconcilationList(bankBookId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, companyBranchId, trnType);
                if (dtBankStatements != null && dtBankStatements.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtBankStatements.Rows)
                    {
                        bankReconcilations.Add(new BankReconcilationDetailViewModel
                        {
                            BankRecoNarration = Convert.ToString(dr["Narration"]),
                            ChequeRefNo = Convert.ToString(dr["ChequeNo"]),
                            Amount = Convert.ToDecimal(dr["Amount"]),
                            RefNo = string.IsNullOrEmpty(Convert.ToString(dr["VoucherNo"])) ? " " : Convert.ToString(dr["VoucherNo"]),
                            RefDate = Convert.ToString(dr["VoucherDate"]),
                            TrnType = Convert.ToString(dr["TrnType"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bankReconcilations;
        }

        public BankReconcilationViewModel GetBankReconcilationDetail(int bankRecoId = 0)
        {
            BankReconcilationViewModel bankReconcilation = new BankReconcilationViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtBanks = sqlDbInterface.GetBankReconcilationDetail(bankRecoId);
                if (dtBanks != null && dtBanks.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtBanks.Rows)
                    {
                        bankReconcilation = new BankReconcilationViewModel
                        {
                            BankRecoID = Convert.ToInt32(dr["BankRecoID"]),
                            BankRecoNo = Convert.ToString(dr["BankRecoNo"]),
                            BankRecoDate = Convert.ToString(dr["BankRecoDate"]),
                            BankBookId = Convert.ToInt32(dr["BankBookId"]),
                            BankBookName = Convert.ToString(dr["BookName"]),
                            BankBranch = Convert.ToString(dr["BankBranch"]),
                            BankRecoFromDate = Convert.ToString(dr["BankRecoFromDate"]),
                            BankRecoToDate = Convert.ToString(dr["BankRecoToDate"]),
                            BookClosingBalance = Convert.ToDecimal(dr["BookClosingBalance"]),
                            BookClosingRemarks = Convert.ToString(dr["BookClosingRemarks"]).Trim(),
                            StatementClosingBalance = Convert.ToDecimal(dr["StatementClosingBalance"]),
                            StatementClosingRemarks = Convert.ToString(dr["StatementClosingRemarks"]).Trim(),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"].ToString() == "" ? "0" : dr["CreatedBy"].ToString()),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),
                            ModifiedByName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            BankRecoStatus = Convert.ToString(dr["BankRecoStatus"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bankReconcilation;
        }

        public List<BankReconcilationViewModel> GetBankReconcilationList(string bankRecoNo = "", int bankBookId = 0, int companyBranchId = 0, string fromDate = "", string toDate = "", int companyId = 0, string bankRecoStatus = "")
        {
            List<BankReconcilationViewModel> bankReconcilations = new List<BankReconcilationViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtBankRecos = sqlDbInterface.GetBankReconcilationList(bankRecoNo, bankBookId, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, bankRecoStatus);
                if (dtBankRecos != null && dtBankRecos.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtBankRecos.Rows)
                    {
                        bankReconcilations.Add(new BankReconcilationViewModel
                        {
                            BankRecoID = Convert.ToInt32(dr["BankRecoID"]),
                            BankRecoNo = Convert.ToString(dr["BankRecoNo"]),
                            BankRecoDate = Convert.ToString(dr["BankRecoDate"]),
                            BankBookId = Convert.ToInt32(dr["BankBookId"]),
                            BankBookName = Convert.ToString(dr["BookName"]),
                            BankBranch = Convert.ToString(dr["BankBranch"]),
                            BankRecoFromDate = Convert.ToString(dr["BankRecoFromDate"]),
                            BankRecoToDate = Convert.ToString(dr["BankRecoToDate"]),
                            BookClosingBalance = Convert.ToDecimal(dr["BookClosingBalance"]),
                            BookClosingRemarks = Convert.ToString(dr["BookClosingRemarks"]),
                            StatementClosingBalance = Convert.ToDecimal(dr["StatementClosingBalance"]),
                            StatementClosingRemarks = Convert.ToString(dr["StatementClosingRemarks"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"].ToString() == "" ? "0" : dr["CreatedBy"].ToString()),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),
                            ModifiedByName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            BankRecoStatus = Convert.ToString(dr["BankRecoStatus"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bankReconcilations;
        }

        public decimal GetBankClosingBalance(int bookId, string fromDate, string ToDate, int companyId)
        {
            decimal bankClosingBalance = 0;
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtBal = sqlDbInterface.GetBankClosingBalance(bookId, Convert.ToDateTime(fromDate), Convert.ToDateTime(ToDate), companyId);
                if (dtBal != null && dtBal.Rows.Count > 0)
                {

                    bankClosingBalance = Convert.ToDecimal(dtBal.Rows[0]["Balance"]);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bankClosingBalance;
        }

        public DataTable GetBankReconcilationDetailDataTable(long bankRecoId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtBankReconcilationDetail = new DataTable();
            try
            {
                dtBankReconcilationDetail= sqlDbInterface.GetBankReconcilationDetail(bankRecoId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtBankReconcilationDetail;
        }

        public DataTable GetBankReconcilationDetailListDataTable(long bankRecoId=0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtBankReconcilationDetailList = new DataTable();
            try
            {
                dtBankReconcilationDetailList = sqlDbInterface.GetBankReconcilationDetailList(bankRecoId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtBankReconcilationDetailList;
        }

        public DataTable GetBankReconcilationSummaryList(long bankRecoId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtBankReconcilationDetailList = new DataTable();
            try
            {
                dtBankReconcilationDetailList = sqlDbInterface.GetBankReconcilationSummaryList(bankRecoId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtBankReconcilationDetailList;
        }
    }
}
