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
using System.Transactions;
namespace Portal.Core
{
    public class StockLedgerBL
    {
        
        public StockLedgerBL()
        {
        
        }   
        public DataTable GetStockLedgerDataTable(int productTypeId, string assemblyType, int productMainGroupId, int productSubGroupId, long productId, int customerBranchId, DateTime fromDate, DateTime toDate, int companyId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtStockLedger = new DataTable();
            try
            {
                dtStockLedger = sqlDbInterface.GetStockLedgerDetail(productTypeId,  assemblyType,  productMainGroupId, productSubGroupId,  productId, customerBranchId, fromDate, toDate,companyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtStockLedger;
        }
        public DataTable GetStockSummaryDataTable(int productTypeId, string assemblyType, int productMainGroupId, int productSubGroupId, long productId, int customerBranchId, DateTime fromDate, DateTime toDate, int companyId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtStockLedger = new DataTable();
            try
            {
                dtStockLedger = sqlDbInterface.GetStockLedgerSummary(productTypeId, assemblyType, productMainGroupId, productSubGroupId, productId, customerBranchId, fromDate, toDate, companyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtStockLedger;
        }
        public decimal GetProductAvailableStock(long productId, int finYearId, int companyId, int companyBranchId, int trnId, string trnType)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            decimal availableStock = 0;
            try
            {
                availableStock = sqlDbInterface.GetProductAvailableStock(productId, finYearId,  companyId,  companyBranchId, trnId, trnType);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return availableStock;
        }

        public decimal GetProductAvailableStockBranchWise(long productId, int finYearId, int companyId, int companyBranchId, int trnId, string trnType)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            decimal availableStock = 0;
            try
            {
                availableStock = sqlDbInterface.GetProductAvailableStockBranchWise(productId, finYearId, companyId, companyBranchId, trnId, trnType);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return availableStock;
        }
        public List<StockLedgerViewModel> GetStockLedgerList(int productTypeId, string assemblyType, int productMainGroupId, int productSubGroupId, long productId, int customerBranchId, DateTime fromDate, DateTime toDate,int companyId)
        {
            List<StockLedgerViewModel> stockLedgerViewModel = new List<StockLedgerViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtStockLedgers = sqlDbInterface.GetStockLedgerList(productTypeId, assemblyType, productMainGroupId, productSubGroupId, productId, customerBranchId,fromDate, toDate, companyId);
                if (dtStockLedgers != null && dtStockLedgers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtStockLedgers.Rows)
                    {
                        stockLedgerViewModel.Add(new StockLedgerViewModel
                        {
                            TrnId = Convert.ToInt32(dr["TrnId"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),                           
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            ProductTypeId = Convert.ToInt32(dr["ProductTypeId"]),
                            ProductTypeName = Convert.ToString(dr["ProductTypeName"]),
                            ProductMainGroupId = Convert.ToInt32(dr["ProductMainGroupId"]),
                            ProductMainGroupName = Convert.ToString(dr["ProductMainGroupName"]),
                            ProductSubGroupId = Convert.ToInt32(dr["ProductSubGroupId"]),
                            ProductSubGroupName = Convert.ToString(dr["ProductSubGroupName"]),                          
                            AssemblyType = Convert.ToString(dr["AssemblyType"]),
                            UOMId = Convert.ToInt32(dr["UOMId"]),
                            UOMName = Convert.ToString(dr["UOMName"]),                            
                            OpeningQty = Convert.ToDecimal(dr["OpeningQty"]),
                            PurchaseQty = Convert.ToDecimal(dr["PurchaseQty"]),
                            SaleQty =Math.Abs(Convert.ToDecimal(dr["SaleQty"])),
                            StockInQty = Convert.ToDecimal(dr["STRQty"]),
                            StockOutQty =Math.Abs(Convert.ToDecimal(dr["STNQty"])),
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
            return stockLedgerViewModel;
        }

        public DataTable GetStockSummaryReports(int productTypeId, string assemblyType, int productMainGroupId, int productSubGroupId, long productId, int customerBranchId, DateTime fromDate, DateTime toDate, int companyId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtStockLedger = new DataTable();
            try
            {
                dtStockLedger = sqlDbInterface.GetStockLedgerReports(productTypeId, assemblyType, productMainGroupId, productSubGroupId, productId, customerBranchId, fromDate, toDate, companyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtStockLedger;
        }

        public DataTable GetStockLedgerReports(long productId, DateTime fromDate, DateTime toDate, int companyId)
        {
           
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtStockLedgers = new DataTable();
            try
            {
                dtStockLedgers = sqlDbInterface.GetStockLedgerReport(productId, fromDate, toDate, companyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtStockLedgers;
        }
        public List<StockLedgerViewModel> GetStockLedgerDrilDownList(long productId, DateTime fromDate, DateTime toDate, int companyId,int customerBranchId)
        {
            List<StockLedgerViewModel> stockLedgers = new List<StockLedgerViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetStockLedgerDrilDownList(productId, fromDate, toDate, companyId, customerBranchId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        stockLedgers.Add(new StockLedgerViewModel
                        {
                            ProductId= Convert.ToInt32(dr["ProductId"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            BranchName = Convert.ToString(dr["BranchName"]),
                            TrnType = Convert.ToString(dr["TrnType"]),
                            TrnTypeName=Convert.ToString(dr["TrnTypeName"]),
                            TrnQty = Math.Abs(Convert.ToDecimal(dr["TrnQty"])),
                            TrnDate = Convert.ToString(dr["TrnDate"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            PartyName = Convert.ToString(dr["PartyName"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return stockLedgers;
        }

        public string GetBranchName(int companyBranchID)
        {
            string str = "";
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                str = sqldbinterface.GetBranchName(companyBranchID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return str;
        }
    }
}
