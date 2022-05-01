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
    public class BalanceTransferBL
    {
        DBInterface dbInterface;

        public BalanceTransferBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditBalanceClosingTransfer(TransferClosingBalanceViewModel balanceTransferViewModel, List<ProductOpeningViewModel> productOpeningStock)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                TransferClosingBalance transferClosingBalance = new TransferClosingBalance
                {
                    FromFinYearID = balanceTransferViewModel.FromFinYearID,
                    ToFinYearID = balanceTransferViewModel.ToFinYearID,
                    CompanyBranchId = balanceTransferViewModel.CompanyBranchId,
                    CompanyId = balanceTransferViewModel.CompanyId,
                    CreatedBy = balanceTransferViewModel.CreatedBy,                                  

                };
              
                List<ProductOpeningStock> productOpeningStockList = new List<ProductOpeningStock>();

                if (productOpeningStock != null && productOpeningStock.Count > 0)
                {
                    foreach (ProductOpeningViewModel item in productOpeningStock)
                    {
                        productOpeningStockList.Add(new ProductOpeningStock
                        {
                            ProductId = item.ProductId,
                            OpeningQty = item.OpeningQty
                        });
                    }
                }
                responseOut = sqlDbInterface.AddEditBalanceClosingTransfer(transferClosingBalance, productOpeningStockList);

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


        public List<BalanceTransferViewModel> GetFinYearProducts(int companyBranchId, DateTime fromDate, DateTime toDate, int companyId)
        {
            List<BalanceTransferViewModel> balanceTransferViewModel = new List<BalanceTransferViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtStockLedgers = sqlDbInterface.GetFinYearProducts(companyBranchId, fromDate, toDate, companyId);
                if (dtStockLedgers != null && dtStockLedgers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtStockLedgers.Rows)
                    {
                        balanceTransferViewModel.Add(new BalanceTransferViewModel
                        {
                           
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),                           
                            ProductMainGroupId = Convert.ToInt32(dr["ProductMainGroupId"]),
                            ProductMainGroupName = Convert.ToString(dr["ProductMainGroupName"]),
                            ProductSubGroupId = Convert.ToInt32(dr["ProductSubGroupId"]),
                            ProductSubGroupName = Convert.ToString(dr["ProductSubGroupName"]),                          
                            ClosingQty = Convert.ToDecimal(dr["ClosingQty"])

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
        public List<TransferClosingBalanceViewModel> GetBalanceTransferList(int companyBranchId, int fromFinYearID, int toFinYearID, string createdBy="")
        {
            List<TransferClosingBalanceViewModel> balanceTransferViewModel = new List<TransferClosingBalanceViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtStockLedgers = sqlDbInterface.GetBalanceTransferList(companyBranchId, fromFinYearID, toFinYearID, createdBy);
                if (dtStockLedgers != null && dtStockLedgers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtStockLedgers.Rows)
                    {
                        balanceTransferViewModel.Add(new TransferClosingBalanceViewModel
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
