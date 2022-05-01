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
    public class BankStatementBL
    {
        DBInterface dbInterface;
        public BankStatementBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditBankStatement(BankStatementViewModel bankStatementViewModel, List<BankStatementDetailViewModel> bankStatementDetails)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                BankStatement bankStatement = new BankStatement
                {
                    BankStatementID= bankStatementViewModel.BankStatementID,
                    BankStatementDate =Convert.ToDateTime(bankStatementViewModel.BankStatementDate),
                    BankBookId = Convert.ToInt32(bankStatementViewModel.BankBookId),
                    BankBranch=Convert.ToString(bankStatementViewModel.BankBranch),
                    BankStatementFromDate=Convert.ToDateTime(bankStatementViewModel.BankStatementFromDate),
                    BankStatementToDate= Convert.ToDateTime(bankStatementViewModel.BankStatementToDate),
                    Remarks = bankStatementViewModel.Remarks,
                    CompanyId = bankStatementViewModel.CompanyId,
                    CompanyBranchId=Convert.ToInt32(bankStatementViewModel.CompanyBranchId),
                    CreatedBy = bankStatementViewModel.CreatedBy,
                    BankStatementStatus= bankStatementViewModel.BankStatementStatus

                };
                List<BankStatementDetail> bankStatementDetailList = new List<BankStatementDetail>();
                if (bankStatementDetails != null && bankStatementDetails.Count > 0)
                {
                    foreach (BankStatementDetailViewModel item in bankStatementDetails)
                    {

                        DateTime dt;
                        DateTime.TryParse(item.TransactionDate, out dt);
                        bankStatementDetailList.Add(new BankStatementDetail
                        {
                        BankStatementDetailId= Convert.ToInt32(item.BankStatementDetailId),
                          TransactionDate =dt,
                          ChequeNumber = item.ChequeNumber,
                          Withdrawal = Convert.ToDecimal(item.Withdrawal),
                          Deposit = Convert.ToDecimal(item.Deposit),
                          Balance = Convert.ToDecimal(item.Balance),
                          Narration = Convert.ToString(item.Narration)
                        });
                    }
                }
            
              responseOut = sqlDbInterface.AddEditBankStatement(bankStatement, bankStatementDetailList);
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
        public List<BankStatementDetailViewModel> GetBankStatementDetailList(int bankRecoId = 0)
        {
            List<BankStatementDetailViewModel> bankStatementDetailList = new List<BankStatementDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtBankStatementDetails = sqlDbInterface.GetBankStatementDetailList(bankRecoId);

                if (dtBankStatementDetails != null && dtBankStatementDetails.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtBankStatementDetails.Rows)
                    {
                        bankStatementDetailList.Add(new BankStatementDetailViewModel
                        {
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            BankStatementDetailId = Convert.ToInt32(dr["BankStatementDetailId"]),
                            TransactionDate = Convert.ToString(dr["TransactionDate"]),
                            ChequeNumber = Convert.ToString(dr["ChequeNumber"]),
                            Withdrawal=Convert.ToDecimal(dr["Withdrawal"]),
                            Deposit = Convert.ToDecimal(dr["Deposit"]),
                            Balance = Convert.ToDecimal(dr["Balance"]),
                            Narration = Convert.ToString(dr["Narration"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bankStatementDetailList;
        }

        public BankStatementViewModel GetBankStatementDetail(int bankStatementId = 0)
        {
            BankStatementViewModel bankStatement = new BankStatementViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtBanks = sqlDbInterface.GetBankStatementDetail(bankStatementId);
                if (dtBanks != null && dtBanks.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtBanks.Rows)
                    {
                        bankStatement = new BankStatementViewModel
                        {
                            BankStatementID = Convert.ToInt32(dr["BankStatementID"]),
                            BankStatementNo = Convert.ToString(dr["BankStatementNo"]),
                            BankStatementDate = Convert.ToString(dr["BankStatementDate"]),
                            BankBookId = Convert.ToInt32(dr["BankBookId"]),
                            BankBookName = Convert.ToString(dr["BookName"]),
                            BankBranch = Convert.ToString(dr["BankBranch"]),
                            BankStatementFromDate = Convert.ToString(dr["BankStatementFromDate"]),
                            BankStatementToDate = Convert.ToString(dr["BankStatementToDate"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"].ToString() == "" ? "0" : dr["CreatedBy"].ToString()),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),
                            ModifiedByName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            BankStatementStatus = Convert.ToString(dr["BankStatementStatus"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bankStatement;
        }

        public List<BankStatementViewModel> GetBankStatementList(string bankStatementNo="", int bankBookId=0, int companyBranchId=0, string fromDate="", string toDate="", int companyId=0, string bankStatementStatus = "")
        {
            List<BankStatementViewModel> bankStatements = new List<BankStatementViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtBankStatements = sqlDbInterface.GetBankStatementList(bankStatementNo, bankBookId, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, bankStatementStatus);
                if (dtBankStatements != null && dtBankStatements.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtBankStatements.Rows)
                    {
                        bankStatements.Add(new BankStatementViewModel
                        {
                            BankStatementID = Convert.ToInt32(dr["BankStatementID"]),
                            BankStatementNo = Convert.ToString(dr["BankStatementNo"]),
                            BankStatementDate = Convert.ToString(dr["BankStatementDate"]),
                            BankBookId = Convert.ToInt32(dr["BankBookId"]),
                            BankBookName = Convert.ToString(dr["BookName"]),
                            BankBranch = Convert.ToString(dr["BankBranch"]),
                            BankStatementFromDate = Convert.ToString(dr["BankStatementFromDate"]),
                            BankStatementToDate = Convert.ToString(dr["BankStatementToDate"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName= Convert.ToString(dr["CompanyBranchName"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"].ToString() == "" ? "0" : dr["CreatedBy"].ToString()),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),
                            ModifiedByName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            BankStatementStatus = Convert.ToString(dr["BankStatementStatus"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bankStatements;
        }
    }
}








