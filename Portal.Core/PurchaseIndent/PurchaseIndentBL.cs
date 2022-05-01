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
   public class PurchaseIndentBL
    {
        DBInterface dbInterface;
        public PurchaseIndentBL()
        {
            dbInterface = new DBInterface();
        }

        #region PurchaseIndent
        public ResponseOut AddEditPurchaseIndent(PurchaseIndentViewModel purchaseIndentViewModel, List<PurchaseIndentProductDetailViewModel> purchaseIndentProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                PurchaseIndent purchaseIndent = new PurchaseIndent
                {
                    IndentId = purchaseIndentViewModel.IndentId,
                    IndentDate =Convert.ToDateTime(purchaseIndentViewModel.IndentDate),
                    RequisitionId = purchaseIndentViewModel.RequisitionId,
                    RequisitionNo = purchaseIndentViewModel.RequisitionNo,
                    IndentType = purchaseIndentViewModel.IndentType,
                    CompanyBranchId = purchaseIndentViewModel.CompanyBranchId,
                    IndentByUserId = purchaseIndentViewModel.IndentByUserId,
                    CustomerId = purchaseIndentViewModel.CustomerId,
                    CustomerBranchId = purchaseIndentViewModel.CustomerBranchId,
                    Remarks1= purchaseIndentViewModel.Remarks1,
                    Remarks2 = purchaseIndentViewModel.Remarks2,
                    FinYearId = purchaseIndentViewModel.FinYearId,
                    CompanyId = purchaseIndentViewModel.CompanyId,
                    CreatedBy= purchaseIndentViewModel.CreatedBy,
                    IndentStatus= purchaseIndentViewModel.IndentStatus
                };

               
                List<PurchaseIndentProductDetail> indentProductList = new List<PurchaseIndentProductDetail>();
                if (purchaseIndentProducts != null && purchaseIndentProducts.Count > 0)
                {
                    foreach (PurchaseIndentProductDetailViewModel item in purchaseIndentProducts)
                    {
                        indentProductList.Add(new PurchaseIndentProductDetail
                        {
                            ProductId = item.ProductId,
                            ProductShortDesc= item.ProductShortDesc,
                            Quantity=item.Quantity

                        });
                       
                    }
                }
              responseOut = sqlDbInterface.AddEditPurchaseIndent(purchaseIndent, indentProductList);
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

        public List<PurchaseIndentProductDetailViewModel> GetPurchaseIndentProductList(long indentId)
        {
            List<PurchaseIndentProductDetailViewModel> indentProducts = new List<PurchaseIndentProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtIndentProducts = sqlDbInterface.GetPurchaseIndentProductList(indentId);
                if (dtIndentProducts != null && dtIndentProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtIndentProducts.Rows)
                    {
                        indentProducts.Add(new PurchaseIndentProductDetailViewModel
                        {
                            IndentProductDetailId = Convert.ToInt32(dr["IndentProductDetailId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            Price= Convert.ToDecimal(dr["PurchasePrice"]),
                            TotalPrice = Convert.ToDecimal(dr["TotalPrice"])
                            
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return indentProducts;
        }

        public PurchaseIndentViewModel GetPurchaseIndentDetail(long indentId = 0)
        {
            PurchaseIndentViewModel purchaseIndent = new PurchaseIndentViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPurchaseIndent = sqlDbInterface.GetPurchaseIndentDetail(indentId);
                if (dtPurchaseIndent != null && dtPurchaseIndent.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPurchaseIndent.Rows)
                    {
                        purchaseIndent = new PurchaseIndentViewModel
                        {
                            IndentId = Convert.ToInt32(dr["IndentId"]),
                            IndentNo = Convert.ToString(dr["IndentNo"]),
                            IndentDate = Convert.ToString(dr["IndentDate"]),
                            IndentByUserId = Convert.ToInt32(dr["IndentByUserId"]),
                            FullName = Convert.ToString(dr["FullName"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            LocationId = Convert.ToInt32(dr["LocationId"]),
                            IndentType = Convert.ToString(dr["IndentType"]),
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            CustomerBranchId = Convert.ToInt32(dr["CustomerBranchId"]),
                            IndentStatus = Convert.ToString(dr["IndentStatus"]),
                            Remarks1 = Convert.ToString(dr["Remarks1"]),
                            Remarks2 = Convert.ToString(dr["Remarks2"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = string.IsNullOrEmpty(Convert.ToString(dr["ModifiedByName"])) ? "" : Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = string.IsNullOrEmpty(Convert.ToString(dr["ModifiedDate"])) ? "" : Convert.ToString(dr["ModifiedDate"]),

                            ApprovalStatus = string.IsNullOrEmpty(Convert.ToString(dr["ApprovalStatus"])) ? "" : Convert.ToString(dr["ApprovalStatus"]),

                            ApprovedByUserName = string.IsNullOrEmpty(Convert.ToString(dr["ApprovedByName"])) ? "" : Convert.ToString(dr["ApprovedByName"]),
                            ApprovedDate = string.IsNullOrEmpty(Convert.ToString(dr["ApprovedDate"])) ? "" : Convert.ToString(dr["ApprovedDate"]),

                            RejectedByUserName = string.IsNullOrEmpty(Convert.ToString(dr["RejectedByName"])) ? "" : Convert.ToString(dr["RejectedByName"]),
                            RejectedDate = string.IsNullOrEmpty(Convert.ToString(dr["RejectedDate"])) ? "" : Convert.ToString(dr["RejectedDate"]),
                            RejectedReason = string.IsNullOrEmpty(Convert.ToString(dr["RejectedReason"])) ? "" : Convert.ToString(dr["RejectedReason"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return purchaseIndent;
        }

        public List<PurchaseIndentViewModel> GetPurchaseIndentList(string indentNo, string indentType, string customerName, int companyBranchId, DateTime fromDate, DateTime toDate, int companyId, string displayType = "", string approvalStatus = "0",string CreatedByUserName="")
        {
            List<PurchaseIndentViewModel> indents = new List<PurchaseIndentViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtIndents = sqlDbInterface.GetPurchaseIndentList(indentNo, indentType, customerName, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, displayType, approvalStatus, CreatedByUserName);
                if (dtIndents != null && dtIndents.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtIndents.Rows)
                    {
                        indents.Add(new PurchaseIndentViewModel
                        {
                            IndentId = Convert.ToInt32(dr["IndentId"]),
                            IndentNo = Convert.ToString(dr["IndentNo"]),
                            IndentDate = Convert.ToString(dr["IndentDate"]),
                            IndentType = Convert.ToString(dr["IndentType"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            CustomerId = Convert.ToInt32(dr["CustomerID"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            IndentStatus = string.IsNullOrEmpty(Convert.ToString(dr["IndentStatus"])) ? "" : Convert.ToString(dr["IndentStatus"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchName= Convert.ToString(dr["BranchName"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return indents;
        }

        public DataTable GetPurchaseIndentDataTable(long indentId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtPurchaseIndent = new DataTable();
            try
            {
                dtPurchaseIndent = sqlDbInterface.GetPurchaseIndentDetail(indentId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtPurchaseIndent;
        }

        public DataTable GetPurchaseIndentProductsDataTable(long indentId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetPurchaseIndentProductList(indentId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }

        #endregion

        #region  Approval Purchase Indent

        public ResponseOut ApproveRejectPurchaseIndent(PurchaseIndentViewModel purchaseIndentViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            
            try
            {
                PurchaseIndent purchaseIndent = new PurchaseIndent
                {
                    IndentId = purchaseIndentViewModel.IndentId,
                    RejectedReason = purchaseIndentViewModel.RejectedReason,
                    ApprovedBy = purchaseIndentViewModel.ApprovedBy,
                    ApprovalStatus = purchaseIndentViewModel.ApprovalStatus
                    
                };

                responseOut = dbInterface.ApproveRejectPurchaseIndent(purchaseIndent);
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

        public List<PurchaseIndentViewModel> GetPurchaseIndentApprovelList(string indentNo, string indentType, string customerName, int companyBranchId, DateTime fromDate, DateTime toDate, int companyId, string displayType = "", string approvalStatus = "0")
        {
            List<PurchaseIndentViewModel> indents = new List<PurchaseIndentViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtIndents = sqlDbInterface.GetPurchaseIndentApprovelList(indentNo, indentType, customerName, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, displayType, approvalStatus);
                if (dtIndents != null && dtIndents.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtIndents.Rows)
                    {
                        indents.Add(new PurchaseIndentViewModel
                        {
                            IndentId = Convert.ToInt32(dr["IndentId"]),
                            IndentNo = Convert.ToString(dr["IndentNo"]),
                            IndentDate = Convert.ToString(dr["IndentDate"]),
                            IndentType = Convert.ToString(dr["IndentType"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            CustomerId = Convert.ToInt32(dr["CustomerID"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            ApprovedByUserName = string.IsNullOrEmpty(Convert.ToString(dr["ApprovedByName"]))?"": Convert.ToString(dr["ApprovedByName"]),
                            ApprovedDate = string.IsNullOrEmpty(Convert.ToString(dr["ApprovedDate"]))?"": Convert.ToString(dr["ApprovedDate"]),
                            ApprovalStatus = string.IsNullOrEmpty(Convert.ToString(dr["ApprovalStatus"])) ? "" : Convert.ToString(dr["ApprovalStatus"]),
                            RejectedByUserName = string.IsNullOrEmpty(Convert.ToString(dr["RejectedByName"]))?"": Convert.ToString(dr["RejectedByName"]),
                            RejectedDate = string.IsNullOrEmpty(Convert.ToString(dr["RejectedDate"]))?"": Convert.ToString(dr["RejectedDate"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return indents;
        }

        public DataTable GetApprovedIndentReport(string indentNo, string poNo, string vendorName, int companyId, DateTime fromDate, DateTime toDate,string companyBranch)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtApprovedIndent = new DataTable();
            try
            {
                dtApprovedIndent = sqlDbInterface.GetAppovedPurcahseIndentDetails(indentNo, poNo, vendorName, companyId,fromDate, toDate, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtApprovedIndent;
        }

        #endregion Approval Purchase
    }
}
