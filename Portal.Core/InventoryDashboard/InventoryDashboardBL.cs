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
    public class InventoryDashboardBL
    {
        DBInterface dbInterface;
        public InventoryDashboardBL()
        {
            dbInterface = new DBInterface();
        }
        public List<StoreRequisitionViewModel> GetSRPending(int companyId)
        {
            List<StoreRequisitionViewModel> requisitions = new List<StoreRequisitionViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtRequisitions = sqlDbInterface.GetPendingSRListCount(companyId);
                if (dtRequisitions != null && dtRequisitions.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRequisitions.Rows)
                    {
                        requisitions.Add(new StoreRequisitionViewModel
                        {

                            RequisitionNo = Convert.ToString(dr["RequisitionNo"]),
                            BranchName = Convert.ToString(dr["BranchName"]),
                            WorkOrderNo = Convert.ToString(dr["WorkOrderNo"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return requisitions;
        }
        public List<ReorderPointProductCountViewModel> GetReorderPointProductCountList(int companyId,int finYearId)
        {
            
            List<ReorderPointProductCountViewModel> reorderProductCountList = new List<ReorderPointProductCountViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCount= sqlDbInterface.GetDashboard_ReorderPointProductCount(companyId,finYearId);
                if (dtCount != null && dtCount.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCount.Rows)
                    {
                        reorderProductCountList.Add(new ReorderPointProductCountViewModel
                        {
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ReorderQty = Convert.ToInt32(dr["ReorderQty"]),
                            AvailableStock = Convert.ToInt32(dr["AvailableStock"])
                        });
                    }
                }
               
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return reorderProductCountList;
        }

        public List<ProductQuantityCountViewModel> GetInOutProductQuantityCountList(int companyId, int companyBrachId)
        {

            List<ProductQuantityCountViewModel> inOutProductQuantityCountList = new List<ProductQuantityCountViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCount = sqlDbInterface.GetDashboard_InOutProductQuantityCount(companyId, companyBrachId);
                if (dtCount != null && dtCount.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCount.Rows)
                    {
                        inOutProductQuantityCountList.Add(new ProductQuantityCountViewModel
                        {
                           
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            OpeningQty = Convert.ToInt32(dr["OpeningQty"]),
                            PurchaseQty = Convert.ToInt32(dr["PurchaseQty"]),
                            SaleQty = Convert.ToInt32(dr["SaleQty"]),
                            STNQty = Convert.ToInt32(dr["STNQty"]) ,
                            STRQty = Convert.ToInt32(dr["STRQty"]),
                            ClosingQty = Convert.ToInt32(dr["ClosingQty"]),                            
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return inOutProductQuantityCountList;
        }


        public List<SINProductQuantityCountViewModel> GetSINProductQuantityCountList(int companyId, int companyBranchId)
        {

            List<SINProductQuantityCountViewModel> sINProductQuantityCountList = new List<SINProductQuantityCountViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCount = sqlDbInterface.GetDashboard_SINProductQuantityCount(companyId, companyBranchId);
                if (dtCount != null && dtCount.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCount.Rows)
                    {
                        sINProductQuantityCountList.Add(new SINProductQuantityCountViewModel
                        {
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),                           
                            Qty = Convert.ToInt32(dr["Quantity"]),                          
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sINProductQuantityCountList;
        }


        public int GetTotalProductCount(int CompanyId)
        {
            int totalProductCount = 0;
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                totalProductCount = sqldbinterface.GetTotalProductCount(CompanyId);              
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return totalProductCount;
        }

        public int GetTodayProduct(int CompanyId)
        {
            int todayProductCount = 0;
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                todayProductCount = sqldbinterface.GetTodayProduct(CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return todayProductCount;
        }


        public List<QualityCheckViewModel> GetMRNPending(int companyId)
        {
            List<QualityCheckViewModel> qcs = new List<QualityCheckViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtRequisitions = sqlDbInterface.GetPendingMRNListCount(companyId);
                if (dtRequisitions != null && dtRequisitions.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRequisitions.Rows)
                    {
                        qcs.Add(new QualityCheckViewModel
                        {

                            QualityCheckId = Convert.ToInt32(dr["QualityCheckId"]),
                            QualityCheckNo = Convert.ToString(dr["QualityCheckNo"]),
                            QualityCheckDate = Convert.ToString(dr["QualityCheckDate"]),
                            CreatedByUserName = Convert.ToString(dr["UserName"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return qcs;
        }

        public List<JobOrderViewModel> GetJobMRNPending(int companyId)
        {
            List<JobOrderViewModel> jos = new List<JobOrderViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtRequisitions = sqlDbInterface.GetPendingJobWorkMRNListCount(companyId);
                if (dtRequisitions != null && dtRequisitions.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRequisitions.Rows)
                    {
                        jos.Add(new JobOrderViewModel
                        {

                            JobWorkId = Convert.ToInt32(dr["JobWorkId"]),
                            JobWorkNo = Convert.ToString(dr["JobWorkNo"]),
                            JobWorkDate = Convert.ToString(dr["JobWorkDate"]),
                            CreatedByUserName = Convert.ToString(dr["UserName"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return jos;
        }

        public ProductionCommanCountViewModel GetTodayPendingWorkOrderCount(int companyId, int companyBranchId)
        {
            ProductionCommanCountViewModel productionCommanCount = new ProductionCommanCountViewModel();
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            DataTable dtProductions = sqldbinterface.GetWOPendingCount(companyId,companyBranchId);
            try
            {
                if (dtProductions != null && dtProductions.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProductions.Rows)
                    {
                        productionCommanCount = new ProductionCommanCountViewModel()
                        {
                            TodayCount =Convert.ToString(dr["TodayPendingWOCount"]),
                            TotalCount=Convert.ToString(dr["TotalPendingWOCount"])
                        };
                    }
                }
                   
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productionCommanCount;
        }

        public ProductionCommanCountViewModel GetDashboardProductBOMCount(int companyId, int companyBranchId)
        {
            ProductionCommanCountViewModel productionCommanCount = new ProductionCommanCountViewModel();
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            DataTable dtProducts = sqldbinterface.GetDashboardProductBOM(companyId, companyBranchId);
            try
            {
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        productionCommanCount = new ProductionCommanCountViewModel()
                        {
                            TodayCount = Convert.ToString(dr["TodayProductBOMCount"]),
                            TotalCount = Convert.ToString(dr["TotalProductBOMCount"])
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productionCommanCount;
        }

        public ProductionCommanCountViewModel GetDashboardFinishedGoodCount(int companyId, int companyBranchId)
        {
            ProductionCommanCountViewModel productionCommanCount = new ProductionCommanCountViewModel();
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            DataTable dtProducts = sqldbinterface.GetDashboardFinishedGoodCount(companyId, companyBranchId);
            try
            {
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        productionCommanCount = new ProductionCommanCountViewModel()
                        {
                            TodayCount = Convert.ToString(dr["TodayFinishedGoodCount"]),
                            TotalCount = Convert.ToString(dr["TotalFinishedGoodCount"])
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productionCommanCount;
        }

        public ProductionCommanCountViewModel GetDashboardFabricationCount(int companyId, int companyBranchId)
        {
            ProductionCommanCountViewModel productionCommanCount = new ProductionCommanCountViewModel();
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            DataTable dtProducts = sqldbinterface.GetDashboardFabricationCount(companyId, companyBranchId);
            try
            {
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        productionCommanCount = new ProductionCommanCountViewModel()
                        {
                            TodayCount = Convert.ToString(dr["TodayFabricationCount"]),
                            TotalCount = Convert.ToString(dr["TotalFabricationCount"])
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productionCommanCount;
        }

        public int GetTodayMRNCount(int CompanyId,int companyBranchId)
        {
            int todayMRNCount = 0;
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                todayMRNCount = sqldbinterface.GetTodayMRNCount(CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return todayMRNCount;
        }

        public int GetTotalJobWorkCount(int CompanyId,int companyBranchId)
        {
            int todayJobWorkCount = 0;
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                todayJobWorkCount = sqldbinterface.GetTotalJobWorkCount(CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return todayJobWorkCount;
        }

        public int GetTotalGateInCount(int CompanyId,int companyBranchId)
        {
            int todayJobWorkCount = 0;
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                todayJobWorkCount = sqldbinterface.GetTotalGateInCount(CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return todayJobWorkCount;
        }

        public int GetTotalStoreRequisitionCount(int CompanyId, int companyBranchId)
        {
            int todayStoreRequisitionCount = 0;
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                todayStoreRequisitionCount = sqldbinterface.GetTotalStoreRequisitionCount(CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return todayStoreRequisitionCount;
        }

        public int GetTotalStockIssueCount(int CompanyId,int companyBranchId)
        {
            int todayStockIssueCount = 0;
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                todayStockIssueCount = sqldbinterface.GetTotalStockIssueCount(CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return todayStockIssueCount;
        }

        public int GetTotalStockTransferCount(int CompanyId)
        {
            int todayStockIssueCount = 0;
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                todayStockIssueCount = sqldbinterface.GetTotalStockTransferCount(CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return todayStockIssueCount;
        }

        public int GetTotalStockReceiveCount(int CompanyId,int companyBranchId=0)
        {
            int stockReceiveCount = 0;
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                stockReceiveCount = sqldbinterface.GetTotalStockReceiveCount(CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return stockReceiveCount;
        }
        

        public int GetTotalPendingJobWorkMRNCount(int CompanyId,int companyBranchId)
        {
            int pendingjobworkmrncount = 0;
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                pendingjobworkmrncount = sqldbinterface.GetTotalPendingJobWorkMRNCount(CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pendingjobworkmrncount;
        }

        
        public int GetTotalQCPendingForMRNCount(int CompanyId, int companyBranchId)
        {
            int qcpendingformrncount = 0;
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                qcpendingformrncount = sqldbinterface.GetTotalQCPendingForMRNCount(CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return qcpendingformrncount;
        }


        public int GetTotalGateInPendingforQCCount(int CompanyId,int companyBranchId)
        {
            int totalGateInPendingforQCCount = 0;
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                totalGateInPendingforQCCount = sqldbinterface.GetTotalGateInPendingforQCCount(CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return totalGateInPendingforQCCount;
        }
        

            public int GetTotalpendingRequistionforSINCount(int CompanyId, int companyBranchId)
            {
            int totalpendingRequistionforSINCount = 0;
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                totalpendingRequistionforSINCount = sqldbinterface.GetTotalpendingRequistionforSINCount(CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return totalpendingRequistionforSINCount;
        }

        public int GetTotalmostConsumeProductMTDCount(int finYearId, int CompanyId)
        {
            int totalmostConsumeProductMTDCount = 0;
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                totalmostConsumeProductMTDCount = sqldbinterface.GetTotalmostConsumeProductMTDCount(finYearId, CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return totalmostConsumeProductMTDCount;
        }

        public int GetTotalPendingProductCount(int finYearId,int CompanyId)
        {
            int totalPendingProductCount = 0;
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                totalPendingProductCount = sqldbinterface.GetTotalPendingProductCount(finYearId,CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return totalPendingProductCount;
        }
       

        public string GetPhysicalAsOnDate(int CompanyId)
        {
            string physicalAsOnDate = "";
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                physicalAsOnDate = sqldbinterface.GetPhysicalAsOnDate(CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return physicalAsOnDate;
        }





        public POCountViewModel GetPODashboard(int companyId,int finYearId,int userId,int mode)
        {
          
            POCountViewModel pocount = new POCountViewModel();
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                DataTable dtPoList = sqldbinterface.GetPODashboard(companyId, finYearId, userId, mode);

                if (dtPoList != null && dtPoList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPoList.Rows)
                    {
                        pocount = new POCountViewModel
                        {
                            POTodayCount = Convert.ToInt32(dr["poCount"]),
                            TodayPOSumAmount = Convert.ToDecimal(dr["amount"]).ToString("0.00")
                        };

                       
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pocount;
        }


        public SICountViewModel GetSaleQutationDashboard(int companyId, int finYearId, int userId,int companyBranchId, int mode)
        {

            SICountViewModel siCount = new SICountViewModel();
            SQLDbInterface sqldbinterface = new SQLDbInterface();

            try
            {
                DataTable dtPoList = sqldbinterface.GetSaleQutationDashboard(companyId, finYearId, userId, companyBranchId, mode);

                if (dtPoList != null && dtPoList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPoList.Rows)
                    {
                        siCount = new SICountViewModel
                        {
                            SITotalCount = Convert.ToInt32(dr["sCount"]).ToString(),
                            SITotalAmountSum = Convert.ToDecimal(dr["amount"]).ToString("0.00")
                        };


                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return siCount;
        }

        public List<CompanyBranchViewModel> GetCompanyBranchList(int companyId)
        {
            List<CompanyBranchViewModel> companyBranches = new List<CompanyBranchViewModel>();
            try
            {
                List<ComapnyBranch> companyBranchList = dbInterface.GetCompanyBranchList(companyId);
                if (companyBranchList != null && companyBranchList.Count > 0)
                {
                    foreach (ComapnyBranch companyBranch in companyBranchList)
                    {
                        companyBranches.Add(new CompanyBranchViewModel { CompanyBranchId = companyBranch.CompanyBranchId, BranchName = companyBranch.BranchName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return companyBranches;
        }

        public MRNCountViewModel GetMRNInventoryDashboard(int companyId, int finYearId, int userId, int companyBranchId, int mode)
        {
            MRNCountViewModel mRNCount = new MRNCountViewModel();
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                DataTable dtMRNList = sqldbinterface.GetInventoryMRNDashboard(companyId, finYearId, userId, companyBranchId, mode);

                if (dtMRNList != null && dtMRNList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtMRNList.Rows)
                    {
                        mRNCount = new MRNCountViewModel
                        {
                            MRNCount_Head = Convert.ToInt32(dr["sCount"]).ToString()
                        };


                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return mRNCount;
        }

        public SINCountViewModel GetSINInventoryDashboard(int companyId, int finYearId, int userId, int companyBranchId, int mode)
        {
            SINCountViewModel sINCount = new SINCountViewModel();
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                DataTable dtMRNList = sqldbinterface.GetInventoryMRNDashboard(companyId, finYearId, userId, companyBranchId, mode);

                if (dtMRNList != null && dtMRNList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtMRNList.Rows)
                    {
                        sINCount = new SINCountViewModel
                        {
                            SINCount_Head = Convert.ToInt32(dr["sCount"]).ToString()
                        };


                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sINCount;
        }

        public STNCountViewModel GetSTNInventoryDashboard(int companyId, int finYearId, int userId, int companyBranchId, int mode)
        {
         
            STNCountViewModel sTNCount = new STNCountViewModel();
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                DataTable dtMRNList = sqldbinterface.GetInventoryMRNDashboard(companyId, finYearId, userId, companyBranchId, mode);

                if (dtMRNList != null && dtMRNList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtMRNList.Rows)
                    {
                        sTNCount = new STNCountViewModel
                        {
                            STNCount_Head = Convert.ToInt32(dr["sCount"]).ToString()
                        };


                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sTNCount;
        }

        public int GetTotalmostConsumeProductCount(int finYearId, int companyId, int companyBranchId, int mode)
        {
            int totalmostConsumeProductCount = 0;
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                totalmostConsumeProductCount = sqldbinterface.GetTotalmostConsumeProductCount(finYearId, companyId, companyBranchId, mode);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return totalmostConsumeProductCount;
        }

        public decimal GetAllProductTotalPrice(int productType, int finYearId, int companyId, int companyBranchId)
        {
            decimal totalProductProductPrice = 0;
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                totalProductProductPrice = sqldbinterface.GetAllProductTotalPrice(productType, finYearId, companyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return totalProductProductPrice;
        }
        public SalePendingPaymentCountViewModel GetSalePendingPaymentCountDashboard(int companyId, int finYearId, int userId, int companyBranchId, int mode)
        {
            SalePendingPaymentCountViewModel salePendingPaymentCount = new SalePendingPaymentCountViewModel();
            SQLDbInterface sqldbinterface = new SQLDbInterface();

            try
            {
                DataTable dtPoList = sqldbinterface.GetDashboardSalePendingPaymentCount(companyId, finYearId,userId, companyBranchId,mode);

                if (dtPoList != null && dtPoList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPoList.Rows)
                    {
                        salePendingPaymentCount = new SalePendingPaymentCountViewModel
                        {
                            salePendingInvoiceCount= Convert.ToInt32(dr["TotalPendingPayment"]).ToString(),
                            salePendingInvoiceAmount=Convert.ToDecimal(dr["AmountPending"]).ToString("0.00")
                        };


                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return salePendingPaymentCount;
        }
        public SaleDashboardTargetAmountViewModel GetSalePendingTargetCountDashboard(int companyId, int finYearId, int companyBranchId)
        {

            SaleDashboardTargetAmountViewModel saleDashboardTargetAmount = new SaleDashboardTargetAmountViewModel();
            SQLDbInterface sqldbinterface = new SQLDbInterface();

            try
            {
                DataTable dtPoList = sqldbinterface.GetDashboardSaleGetTargetAmount(companyId, finYearId, companyBranchId);

                if (dtPoList != null && dtPoList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPoList.Rows)
                    {
                        saleDashboardTargetAmount = new SaleDashboardTargetAmountViewModel
                        {
                            TargetAmount = Convert.ToInt32(dr["TargetAmount"]).ToString("0.00"),
                            TotalInvoiceAmount = Convert.ToDecimal(dr["TotalInvoiceAmount"]).ToString("0.00")
                        };


                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return saleDashboardTargetAmount;
        }
    }
}








