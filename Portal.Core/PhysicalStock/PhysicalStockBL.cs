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
    public class PhysicalStockBL
    {
        DBInterface dbInterface;
        public PhysicalStockBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditPhysicalStock(PhysicalStockViewModel physicalStockViewModel, List<PhysicalStockProductDetailViewModel> physicalStockProductDetails)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                PhysicalStock physicalStock = new PhysicalStock
                {
                    PhysicalStockID = physicalStockViewModel.PhysicalStockID,
                    PhysicalStockDate = Convert.ToDateTime(physicalStockViewModel.PhysicalStockDate),
                    PhysicalAsOnDate = Convert.ToDateTime(physicalStockViewModel.PhysicalAsOnDate),
                    CompanyId = physicalStockViewModel.CompanyId,
                    CompanyBranchId=Convert.ToInt32(physicalStockViewModel.CompanyBranchId)  ,
                    ApprovalStatus = physicalStockViewModel.ApprovalStatus,
                };
                List<PhysicalStockProductDetail> physicalStockProductDetailList = new List<PhysicalStockProductDetail>();
                if (physicalStockProductDetails != null && physicalStockProductDetails.Count > 0)
                {
                    foreach (PhysicalStockProductDetailViewModel item in physicalStockProductDetails)
                    {


                        physicalStockProductDetailList.Add(new PhysicalStockProductDetail
                        {
                            PhysicalStockDetailID = Convert.ToInt32(item.PhysicalStockDetailID),
                            PhysicalStockID = Convert.ToInt32(item.PhysicalStockID),
                            Productid = Convert.ToInt32(item.Productid),
                            ProductName = Convert.ToString(item.ProductName),
                            ProductCode = Convert.ToString(item.ProductCode),
                            CompanyId = item.CompanyId,
                            ProductTypeId = Convert.ToInt32(item.ProductTypeId),
                            ProductMainGroupId = Convert.ToInt32(item.ProductMainGroupId),
                            ProductSubGroupId = Convert.ToInt32(item.ProductSubGroupId),
                            UOMId = Convert.ToInt32(item.UOMId),
                            AssemblyType = Convert.ToString(item.AssemblyType),
                            PhysicalQTY = Convert.ToDecimal(item.PhysicalQTY),
                            SystemQTY = Convert.ToDecimal(item.SystemQTY),
                            DiffQTY = Convert.ToDecimal(item.DiffQTY),
                            TransferProductID=Convert.ToInt32(item.TransferTo)
                        });
                    }
                }
            
              responseOut = sqlDbInterface.AddEditPhysicalStock(physicalStock, physicalStockProductDetailList);
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


        public DataTable GetPhysicalStockDetailListReport(int physicalStockID = 0)
        {
            DataTable PhysicalStockDetailList;
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                PhysicalStockDetailList = sqlDbInterface.GetPhysicalStockDetailList(physicalStockID);

               
                
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return PhysicalStockDetailList;
        }





        public List<PhysicalStockProductDetailViewModel> GetPhysicalStockDetailList(int physicalStockID = 0)
        {
            List<PhysicalStockProductDetailViewModel> physicalStockProductDetails = new List<PhysicalStockProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtBankStatementDetails = sqlDbInterface.GetPhysicalStockDetailList(physicalStockID);

                if (dtBankStatementDetails != null && dtBankStatementDetails.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtBankStatementDetails.Rows)
                    {
                        physicalStockProductDetails.Add(new PhysicalStockProductDetailViewModel
                        {
                            Productid = Convert.ToInt32(dr["Productid"]),
                            ProductTypeId = Convert.ToInt32(dr["ProductTypeId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductMainGroupId = Convert.ToInt32(dr["ProductMainGroupId"]),
                            ProductMainGroupName = Convert.ToString(dr["ProductMainGroupName"]),
                            ProductSubGroupId = Convert.ToInt32(dr["ProductSubGroupId"]),
                            ProductSubGroupName = Convert.ToString(dr["ProductSubGroupName"]),
                            AssemblyType = Convert.ToString(dr["AssemblyType"]),
                            UOMId = Convert.ToInt32(dr["UOMId"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            PhysicalQTY = Convert.ToDecimal(dr["PhysicalQTY"]),
                            SystemQTY = Convert.ToDecimal(dr["SystemQTY"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return physicalStockProductDetails;
        }

        public PhysicalStockViewModel GetPhysicalStockDetail(int physicalStockID)
        {
            PhysicalStockViewModel physicalStockViewModel = new PhysicalStockViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtBanks = sqlDbInterface.GetPhysicalStockDetail(physicalStockID);
                if (dtBanks != null && dtBanks.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtBanks.Rows)
                    {
                        physicalStockViewModel = new PhysicalStockViewModel
                        {
                            PhysicalStockID = Convert.ToInt32(dr["PhysicalStockID"]),
                            PhysicalStockNo = Convert.ToString(dr["PhysicalStockNo"]),
                            PhysicalStockDate = Convert.ToString(dr["PhysicalStockDate"]),
                            PhysicalAsOnDate = Convert.ToString(dr["PhysicalAsOnDate"]),                           
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return physicalStockViewModel;
        }



        public DataTable GetPhysicalStockDetailReport(int physicalStockID)
        {
            DataTable physicalStock;
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                physicalStock = sqlDbInterface.GetPhysicalStockDetail(physicalStockID);
              
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return physicalStock;
        }



        public List<PhysicalStockViewModel> GetPhysicalStockList(string physicalStockNo = "",  int companyBranchId=0, string fromDate="", string toDate="", int companyId=0, string approvalStatus = "")
        {
            List<PhysicalStockViewModel> physicalStockViewModelList = new List<PhysicalStockViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtBankStatements = sqlDbInterface.GetPhysicalStockList(physicalStockNo, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, approvalStatus);
                if (dtBankStatements != null && dtBankStatements.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtBankStatements.Rows)
                    {
                        physicalStockViewModelList.Add(new PhysicalStockViewModel
                        {
                            PhysicalStockID = Convert.ToInt32(dr["PhysicalStockID"]),
                            PhysicalStockNo = Convert.ToString(dr["PhysicalStockNo"]),
                            PhysicalStockDate = Convert.ToString(dr["PhysicalStockDate"]),
                            PhysicalAsOnDate = Convert.ToString(dr["PhysicalAsOnDate"]),                            
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return physicalStockViewModelList;
        }
        public decimal GetProductSystemStock(long productId, int finYearId, int companyId, int companyBranchId, string fromDate = "", string toDate = "")
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            decimal availableStock = 0;
            try
            {
                availableStock = sqlDbInterface.GetProductSystemStock(productId, finYearId, companyId, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate));
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return availableStock;
        }
    }
}








