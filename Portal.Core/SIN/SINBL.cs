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
  public class SINBL
    {
        DBInterface dbInterface;
        public SINBL()
        {
            dbInterface = new DBInterface();
        }


         public ResponseOut CancelSIN(SINViewModel sINViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                StockIssueNote stockIssueNote = new StockIssueNote
                {
                    SINId = sINViewModel.SINId,
                    CancelStatus = "Cancelled",
                    SINStatus = "Cancelled",
                    CreatedBy = sINViewModel.CreatedBy,
                    CompanyId= sINViewModel.CompanyId,
                    CompanyBranchId= sINViewModel.CompanyBranchId,
                    CancelReason = sINViewModel.CancelReason,
                    FinYearId= sINViewModel.FinYearId
                };
                responseOut = sqlDbInterface.CancelSIN(stockIssueNote);
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



        public List<SINProductDetailViewModel> GetSINProductList(long stnId)
        {
            List<SINProductDetailViewModel> sinProducts = new List<SINProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetSINProductList(stnId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {

                        sinProducts.Add(new SINProductDetailViewModel
                        {
                            SINProductDetailId = Convert.ToInt32(dr["SINProductDetailId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            IssuedQuantity = Convert.ToDecimal(dr["IssuedQuantity"]),
                            BalanceQuantity = Convert.ToDecimal(dr["BalanceQuantity"]),
                            AvailableStock = Convert.ToDecimal(dr["AvailableStock"]),
                            IssueQuantity = Convert.ToDecimal(dr["IssueQuantity"])

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sinProducts;
        }
        public ResponseOut AddEditSIN(SINViewModel sinViewModel, List<SINProductDetailViewModel> sinProducts,List<SINSupportingDocumentViewModel> sinDocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                StockIssueNote sin = new StockIssueNote
                {
                    SINId = sinViewModel.SINId,
                    SINDate = Convert.ToDateTime(sinViewModel.SINDate),
                    RequisitionId=sinViewModel.RequisitionId,
                    RequisitionNo=sinViewModel.RequisitionNo,
                    JobId = sinViewModel.JobId,
                    JobNo = sinViewModel.JobNo,
                    JobDate = string.IsNullOrEmpty(sinViewModel.JobDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(sinViewModel.JobDate),
                    CompanyBranchId = sinViewModel.CompanyBranchId,
                    FromLocationId = sinViewModel.FromLocationId,
                    ToLocationId = sinViewModel.ToLocationId,
                    EmployeeName=sinViewModel.EmployeeName,
                    RefNo = sinViewModel.RefNo,
                    RefDate = string.IsNullOrEmpty(sinViewModel.RefDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(sinViewModel.RefDate),
                    Remarks1 = string.IsNullOrEmpty(sinViewModel.Remarks1)?"":sinViewModel.Remarks1,
                    Remarks2 = string.IsNullOrEmpty(sinViewModel.Remarks2) ? "" : sinViewModel.Remarks2,
                    ReceivedByUserId = sinViewModel.ReceivedByUserId,
                    FinYearId = sinViewModel.FinYearId,
                    CompanyId = sinViewModel.CompanyId,
                    CreatedBy = sinViewModel.CreatedBy,
                    SINStatus=sinViewModel.SINStatus

                };
                List<StockIssueNoteProductDetail> sinProductList = new List<StockIssueNoteProductDetail>();
                if (sinProducts != null && sinProducts.Count > 0)
                {
                    foreach (SINProductDetailViewModel item in sinProducts)
                    {
                        sinProductList.Add(new StockIssueNoteProductDetail
                        {
                            ProductId = item.ProductId,
                            ProductShortDesc = item.ProductShortDesc,                          
                            Quantity = item.IssueQuantity
                          
                        });
                    }
                }

                List<StockIssueNoteSupportingDocument> sinDocumentList = new List<StockIssueNoteSupportingDocument>();
                if (sinDocuments != null && sinDocuments.Count > 0)
                {
                    foreach (SINSupportingDocumentViewModel item in sinDocuments)
                    {
                        sinDocumentList.Add(new StockIssueNoteSupportingDocument
                        {
                            DocumentTypeId = item.DocumentTypeId,
                            DocumentName = item.DocumentName,
                            DocumentPath = item.DocumentPath
                        });
                    }
                }
                responseOut = sqlDbInterface.AddEditSIN(sin, sinProductList, sinDocumentList);

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

        public List<SINViewModel> GetSINList(string sinNo, string requisitionNo, string jobno, int companyBranchId, string fromDate, string toDate, int companyId,string sINStatus)
        {
            List<SINViewModel> stns = new List<SINViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSTNs = sqlDbInterface.GetSINList(sinNo, requisitionNo, jobno, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, sINStatus);

                    
                if (dtSTNs != null && dtSTNs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSTNs.Rows)
                    {

                        stns.Add(new SINViewModel
                        {
                            SINId = Convert.ToInt32(dr["SINId"]),
                            SINNo = Convert.ToString(dr["SINNo"]),
                            SINDate = Convert.ToString(dr["SINDate"]),
                            RequisitionNo = Convert.ToString(dr["RequisitionNo"]),
                            JobNo = Convert.ToString(dr["JobNo"]),
                            BranchName = Convert.ToString(dr["BranchName"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            FromLocationName= Convert.ToString(dr["FromLocationName"]),
                            ToLocationName = Convert.ToString(dr["ToLocationName"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            SINStatus = Convert.ToString(dr["SINStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return stns;
        }

        public SINViewModel GetSINDetail(long sinId = 0)
        {
            SINViewModel sin = new SINViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtstns = sqlDbInterface.GetSINDetail(sinId);
                if (dtstns != null && dtstns.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtstns.Rows)
                    {
                        sin = new SINViewModel
                        {
                            SINId = Convert.ToInt32(dr["SINId"]),
                            SINNo = Convert.ToString(dr["SINNo"]),
                            SINDate=Convert.ToString(dr["SINDate"]),
                            RequisitionId = Convert.ToInt32(dr["RequisitionId"]),
                            RequisitionNo = Convert.ToString(dr["RequisitionNo"]),
                            RequisitionDate = Convert.ToString(dr["RequisitionDate"]),
                            JobId = Convert.ToInt32(dr["JobId"]),
                            JobNo = Convert.ToString(dr["JobNo"]),
                            JobDate = Convert.ToString(dr["JobDate"]),
                            CompanyBranchId=Convert.ToInt32(dr["CompanyBranchId"]),
                            FromLocationId=Convert.ToInt32(dr["FromLocationId"]),
                            ToLocationId=Convert.ToInt32(dr["ToLocationId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            Remarks1 = Convert.ToString(dr["Remarks1"]),
                            Remarks2 = Convert.ToString(dr["Remarks2"]),
                            ReceivedByUserId=Convert.ToInt32(dr["ReceivedByUserId"]),
                            SINStatus=Convert.ToString(dr["SINStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CancelReason = Convert.ToString(dr["CancelReason"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sin;
        }

        public DataTable GetSINDetailDataTable(long stnId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtSIN = new DataTable();
            try
            {
                dtSIN = sqlDbInterface.GetSINDetail(stnId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtSIN;
        }

        public DataTable GetSINProductListDataTable(long sinId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetSINProductList(sinId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }

        public List<SINSupportingDocumentViewModel> GetSINSupportingDocumentList(long sinId)
        {
            List<SINSupportingDocumentViewModel> sinDocuments = new List<SINSupportingDocumentViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDocument = sqlDbInterface.GetSINSupportingDocumentList(sinId);
                if (dtDocument != null && dtDocument.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDocument.Rows)
                    {
                        sinDocuments.Add(new SINSupportingDocumentViewModel
                        {
                            SINDocId = Convert.ToInt32(dr["SINDocId"]),
                            DocumentSequenceNo = Convert.ToInt32(dr["SNo"]),
                            DocumentTypeId = Convert.ToInt32(dr["DocumentTypeId"]),
                            DocumentTypeDesc = Convert.ToString(dr["DocumentTypeDesc"]),
                            DocumentName = Convert.ToString(dr["DocumentName"]),
                            DocumentPath = Convert.ToString(dr["DocumentPath"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sinDocuments;
        }


        public List<StoreRequisitionViewModel> GetStoreRequisitionList(string requisitionNo, string workOrderNo, string requisitionType, int companyBranchId, DateTime fromDate, DateTime toDate, int companyId, string displayType = "", string approvalStatus = "")
        {
            List<StoreRequisitionViewModel> requisitions = new List<StoreRequisitionViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtRequisitions = sqlDbInterface.GetSINStoreRequisitionList(requisitionNo, workOrderNo, requisitionType, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, displayType, approvalStatus);
                if (dtRequisitions != null && dtRequisitions.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRequisitions.Rows)
                    {
                        requisitions.Add(new StoreRequisitionViewModel
                        {
                            RequisitionId = Convert.ToInt32(dr["RequisitionId"]),
                            RequisitionNo = Convert.ToString(dr["RequisitionNo"]),
                            RequisitionDate = Convert.ToString(dr["RequisitionDate"]),
                            RequisitionType = Convert.ToString(dr["RequisitionType"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            BranchName = Convert.ToString(dr["BranchName"]),
                            LocationId = Convert.ToInt32(dr["LocationId"]),
                            LocationName = Convert.ToString(dr["LocationName"]),
                            WorkOrderId = Convert.ToInt32(dr["WorkOrderId"]),
                            WorkOrderNo = Convert.ToString(dr["WorkOrderNo"]),
                            WorkOrderDate = Convert.ToString(dr["WorkOrderDate"])
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

        public List<SINProductDetailViewModel> GetSINStoreRequisitionProductList(long requisitionId,int finYear)
        {
            List<SINProductDetailViewModel> requisitionProducts = new List<SINProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtRequisitionProducts = sqlDbInterface.GetSINStoreRequisitionProductList(requisitionId, finYear);
                if (dtRequisitionProducts != null && dtRequisitionProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRequisitionProducts.Rows)
                    {
                        requisitionProducts.Add(new SINProductDetailViewModel
                        {
                            SINProductDetailId = 0,
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            IssuedQuantity = Convert.ToDecimal(dr["IssuedQuantity"]),
                            BalanceQuantity = Convert.ToDecimal(dr["BalanceQuantity"]),
                            AvailableStock = Convert.ToDecimal(dr["AvailableStock"]),
                            IssueQuantity = Convert.ToDecimal(dr["IssueQuantity"]),
                            IndentQuantity = Convert.ToDecimal(dr["IndentQuantity"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return requisitionProducts;
        }
    }
}
