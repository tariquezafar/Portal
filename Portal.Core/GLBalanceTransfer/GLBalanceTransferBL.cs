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
    public class GLBalanceTransferBL
    {
        DBInterface dbInterface;

        public GLBalanceTransferBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut GLBalanceClosingTransfer(GLTransferClosingBalanceViewModel gLTransferClosingBalanceViewModel, List<GLDetailViewModel> gLDetailViewModels)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                GLTransferClosingBalance gltransferClosingBalance = new GLTransferClosingBalance
                {
                    FromFinYearID = gLTransferClosingBalanceViewModel.FromFinYearID,
                    ToFinYearID = gLTransferClosingBalanceViewModel.ToFinYearID,
                    CompanyBranchId = gLTransferClosingBalanceViewModel.CompanyBranchId,
                    CompanyId = gLTransferClosingBalanceViewModel.CompanyId,
                    CreatedBy = gLTransferClosingBalanceViewModel.CreatedBy,                                  

                };
              
                List<GLDetail> gLDetailList = new List<GLDetail>();

                if (gLDetailViewModels != null && gLDetailViewModels.Count > 0)
                {
                    foreach (GLDetailViewModel item in gLDetailViewModels)
                    {
                        gLDetailList.Add(new GLDetail
                        {
                            GLId = item.GLId,
                            OpeningBalanceDebit = item.OpeningBalanceDebit,
                            OpeningBalanceCredit= item.OpeningBalanceCredit,
                        });
                    }
                }
                responseOut = sqlDbInterface.GLBalanceClosingTransfer(gltransferClosingBalance, gLDetailList);

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

        public FinYearViewModel GetFinYearDetail()
        {
            FinYearViewModel finYearViewModel = new FinYearViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetFinYearDetail();
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        finYearViewModel = new FinYearViewModel
                        {
                            PreviousFinYearId= Convert.ToInt32(dr["PreviousFinYearId"]),
                            CurrentFinYearID = Convert.ToInt32(dr["CurrentFinYearID"]),
                            PreviousFinYearCode = Convert.ToString(dr["PreviousFinYearCode"]),
                            CurrentFinYearCode = Convert.ToString(dr["CurrentFinYearCode"]),
                            StartDate = Convert.ToString(dr["StartDate"]),
                            EndDate = Convert.ToString(dr["EndDate"]),                                                   
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return finYearViewModel;
        }


        public List<TempGLHeadClosingBalanceTransferViewModel> GetGLBalanceTransfer(int companyId, int finYearId, DateTime endDate, int reportUserId)
        {
            List<TempGLHeadClosingBalanceTransferViewModel> tempGLHeadClosingBalanceTransferViewModel = new List<TempGLHeadClosingBalanceTransferViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtStockLedgers = sqlDbInterface.GetGLBalanceTransfer(companyId, finYearId, endDate, reportUserId);
                if (dtStockLedgers != null && dtStockLedgers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtStockLedgers.Rows)
                    {
                        tempGLHeadClosingBalanceTransferViewModel.Add(new TempGLHeadClosingBalanceTransferViewModel
                        {

                            GLId = Convert.ToInt32(dr["GLId"]),
                            GLCode = Convert.ToString(dr["GLCode"]),
                            GLHead = Convert.ToString(dr["GLHead"]),                         
                            ClosingBalanceDebit = Convert.ToDecimal(dr["ClosingDebitBalance"]),
                            ClosingBalanceCredit = Convert.ToDecimal(dr["ClosingCreditBalance"]),

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return tempGLHeadClosingBalanceTransferViewModel;
        }

        public DataTable GenerateTransferReport(int companyBranchId, DateTime fromDate, DateTime toDate, int companyId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtTransfer = new DataTable();
            try
            {
                dtTransfer = sqlDbInterface.GetFinYearProducts(companyBranchId, fromDate, toDate, companyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtTransfer;
        }

        public ResponseOut ReversedClosingTransfer(TransferClosingBalanceViewModel balanceTransferViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                TransferClosingBalance transferClosingBalance = new TransferClosingBalance
                {
                   
                    ToFinYearID = balanceTransferViewModel.ToFinYearID,
                   
                };              
                responseOut = sqlDbInterface.ReversedClosingTransfer(transferClosingBalance);

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
        public List<GLTransferClosingBalanceViewModel> GetGLBalanceTransferList(int companyBranchId, int fromFinYearID, int toFinYearID, string createdBy="")
        {
            List<GLTransferClosingBalanceViewModel> balanceTransferViewModel = new List<GLTransferClosingBalanceViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtStockLedgers = sqlDbInterface.GetGLBalanceTransferList(companyBranchId, fromFinYearID, toFinYearID, createdBy);
                if (dtStockLedgers != null && dtStockLedgers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtStockLedgers.Rows)
                    {
                        balanceTransferViewModel.Add(new GLTransferClosingBalanceViewModel
                        {

                            FromFinYearID = Convert.ToInt32(dr["FromFinYearID"]),
                            ToFinYearID = Convert.ToInt32(dr["ToFinYearID"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            BranchName = Convert.ToString(dr["BranchName"]),
                            UserName = Convert.ToString(dr["UserName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return balanceTransferViewModel;
        }
    }
}
