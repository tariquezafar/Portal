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
    public class SLBalanceTransferBL
    {
        DBInterface dbInterface;

        public SLBalanceTransferBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut SLBalanceClosingTransfer(SLTransferClosingBalanceViewModel sLTransferClosingBalanceViewModel, List<SLDetailViewModel> sLDetailViewModels)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                SLTransferClosingBalance sltransferClosingBalance = new SLTransferClosingBalance
                {
                    FromFinYearID = sLTransferClosingBalanceViewModel.FromFinYearID,
                    ToFinYearID = sLTransferClosingBalanceViewModel.ToFinYearID,
                    CompanyBranchId = sLTransferClosingBalanceViewModel.CompanyBranchId,
                    CompanyId = sLTransferClosingBalanceViewModel.CompanyId,
                    CreatedBy = sLTransferClosingBalanceViewModel.CreatedBy,                                  

                };
              
                List<SLDetail> sLDetailList = new List<SLDetail>();

                if (sLDetailViewModels != null && sLDetailViewModels.Count > 0)
                {
                    foreach (SLDetailViewModel item in sLDetailViewModels)
                    {
                        sLDetailList.Add(new SLDetail
                        {
                            GLId = item.GLId,
                            SLId = item.SLId,
                            OpeningBalanceDebit = item.OpeningBalanceDebit,
                            OpeningBalanceCredit= item.OpeningBalanceCredit,
                        });
                    }
                }
                responseOut = sqlDbInterface.SLBalanceClosingTransfer(sltransferClosingBalance, sLDetailList);

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
     
        public List<TempSLHeadClosingBalanceTransferViewModel> GetSLBalanceTransfer(int companyId, int finYearId, DateTime endDate, int reportUserId, int pSLTypeId, int pGLId)
        {
            List<TempSLHeadClosingBalanceTransferViewModel> tempSLHeadClosingBalanceTransferViewModel = new List<TempSLHeadClosingBalanceTransferViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtStockLedgers = sqlDbInterface.GetSLBalanceTransfer(companyId, finYearId, endDate, reportUserId, pSLTypeId, pGLId);
                if (dtStockLedgers != null && dtStockLedgers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtStockLedgers.Rows)
                    {
                        tempSLHeadClosingBalanceTransferViewModel.Add(new TempSLHeadClosingBalanceTransferViewModel
                        {

                            GLId = Convert.ToInt32(dr["GLId"]),                         
                            GLHead = Convert.ToString(dr["GLHead"]),    
                            SLId = Convert.ToInt32(dr["SLId"]),
                            SLHead = Convert.ToString(dr["SLHead"]),
                            SLCode = Convert.ToString(dr["SLCode"]),
                            GLCode = Convert.ToString(dr["GLCode"]),
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
            return tempSLHeadClosingBalanceTransferViewModel;
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

       
        public List<SLTransferClosingBalanceViewModel> GetSLBalanceTransferList(int companyBranchId, int fromFinYearID, int toFinYearID, string createdBy="")
        {
            List<SLTransferClosingBalanceViewModel> sLbalanceTransferViewModel = new List<SLTransferClosingBalanceViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtStockLedgers = sqlDbInterface.GetSLBalanceTransferList(companyBranchId, fromFinYearID, toFinYearID, createdBy);
                if (dtStockLedgers != null && dtStockLedgers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtStockLedgers.Rows)
                    {
                        sLbalanceTransferViewModel.Add(new SLTransferClosingBalanceViewModel
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
            return sLbalanceTransferViewModel;
        }
    }
}
