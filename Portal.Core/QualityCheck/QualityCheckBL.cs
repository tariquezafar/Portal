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
 public class QualityCheckBL
    {
        DBInterface dbInterface;
        public QualityCheckBL()
        {
            dbInterface = new DBInterface();
        }

        public DataTable GetQualityCheckDetailReport(long qualityCheckId = 0)
        {
            DataTable dtQualityCheckDetail;
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                dtQualityCheckDetail = sqlDbInterface.GetQualityCheckDetail(qualityCheckId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtQualityCheckDetail;
        }

        public DataTable GetQualityCheckProductListReport(long qualityCheckId = 0)
        {
            DataTable dtProducts;
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                dtProducts = sqlDbInterface.GetQualityCheckProductList(qualityCheckId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }


        public List<QualityCheckProductDetailViewModel> GetQualityCheckRejectProductList(long qualityCheckID)
        {
            List<QualityCheckProductDetailViewModel> qualityCheckProducts = new List<QualityCheckProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetQualityCheckRejectProductList(qualityCheckID);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {

                        qualityCheckProducts.Add(new QualityCheckProductDetailViewModel
                        {
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            Price = Convert.ToDecimal(dr["Price"]),
                            PendingQuantity = Convert.ToDecimal(dr["PendingQuantity"]),
                            ReceivedQuantity = Convert.ToDecimal(dr["ReceivedQuantity"]),
                            AcceptQuantity = Convert.ToDecimal(dr["AcceptQuantity"]),
                            RejectQuantity = Convert.ToDecimal(dr["RejectQuantity"]),
                            RejectRemakrs = Convert.ToString(dr["RejectRemakrs"]),
                            UOMName = Convert.ToString(dr["UOMName"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return qualityCheckProducts;
        }


        public List<QualityCheckProductDetailViewModel> GetQualityCheckProductList(long qualityCheckID)
        {
            List<QualityCheckProductDetailViewModel> qualityCheckProducts = new List<QualityCheckProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetQualityCheckProductList(qualityCheckID);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {

                        qualityCheckProducts.Add(new QualityCheckProductDetailViewModel
                        {
                            SequenceNo = Convert.ToInt32(dr["SNo"]),                        
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),                            
                            TotalRecQuantity = Convert.ToDecimal(dr["TotalRecQuantity"]),
                            PendingQuantity = Convert.ToDecimal(dr["PendingQuantity"]),
                            ReceivedQuantity = Convert.ToDecimal(dr["ReceivedQuantity"]),
                            AcceptQuantity = Convert.ToDecimal(dr["AcceptQuantity"]),
                            RejectQuantity = Convert.ToDecimal(dr["RejectQuantity"]),
                            RejectRemakrs = Convert.ToString(dr["RejectRemakrs"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return qualityCheckProducts;
        }

        public ResponseOut AddEditQualityCheck(QualityCheckViewModel qualityCheckViewModel, List<QualityCheckProductDetailViewModel> qcProducts, List<QualityCheckSupportingDocumentViewModel> qcDocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                QualityCheck qc = new QualityCheck
                {
                    QualityCheckId=qualityCheckViewModel.QualityCheckId,
                    QualityCheckDate =Convert.ToDateTime(qualityCheckViewModel.QualityCheckDate),
                    GateInId = qualityCheckViewModel.GateInId,
                    GateInNo = qualityCheckViewModel.GateInNo,
                    POID = qualityCheckViewModel.POId,
                    Remarks=qualityCheckViewModel.Remarks,
                    RejectRemarks=qualityCheckViewModel.RejectRemarks,   
                    CompanyBranchId=qualityCheckViewModel.CompanyBranchId,               
                    FinYearId = qualityCheckViewModel.FinYearId,
                    CompanyId = qualityCheckViewModel.CompanyId,
                    Inspectedby=qualityCheckViewModel.CreatedBy,
                    CreatedBy = qualityCheckViewModel.CreatedBy,
                    ApprovalStatus= qualityCheckViewModel.ApprovalStatus


                };
                List<QualityCheckProductDetail> qcProductList = new List<QualityCheckProductDetail>();
                if (qcProducts != null && qcProducts.Count > 0)
                {
                    foreach (QualityCheckProductDetailViewModel item in qcProducts)
                    {
                        qcProductList.Add(new QualityCheckProductDetail
                        {
                            ProductId = item.ProductId,
                            ReceivedQuantity = item.ReceivedQuantity,
                            AcceptQuantity = item.AcceptQuantity,
                            RejectQuantity = item.RejectQuantity,
                            RejectRemakrs=item.RejectRemakrs
                        });
                    }
                }

                List<QualityCheckSupportingDocument> qcDocumentList = new List<QualityCheckSupportingDocument>();
                if (qcDocuments != null && qcDocuments.Count > 0)
                {
                    foreach (QualityCheckSupportingDocumentViewModel item in qcDocuments)
                    {
                        qcDocumentList.Add(new QualityCheckSupportingDocument
                        {
                            DocumentTypeId = item.DocumentTypeId,
                            DocumentName = item.DocumentName,
                            DocumentPath = item.DocumentPath
                        });
                    }
                }

                responseOut = sqlDbInterface.AddEditQualityCheck(qc, qcProductList, qcDocumentList);

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
        public List<QualityCheckViewModel> GetQualityCheckList(string qualtityCheckNo, string gateinNo, string pono, string fromDate, string toDate, int companyId, string approvalStatus = "",string companyBranch="")
        {
            List<QualityCheckViewModel>  qcs = new List<QualityCheckViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtGateIns = sqlDbInterface.GetQualityCheckList(qualtityCheckNo, gateinNo, pono, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, approvalStatus, companyBranch);
                if (dtGateIns != null && dtGateIns.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtGateIns.Rows)
                    {

                        qcs.Add(new QualityCheckViewModel
                        {

                            QualityCheckId = Convert.ToInt32(dr["QualityCheckId"]),
                            QualityCheckNo = Convert.ToString(dr["QualityCheckNo"]),
                            QualityCheckDate = Convert.ToString(dr["QualityCheckDate"]),
                            GateInId = Convert.ToInt32(dr["GateInId"]),
                            GateInNo = Convert.ToString(dr["GateInNo"]),                           
                           // PONo = Convert.ToString(dr["PONo"]),
                            CompanyBranchName=Convert.ToString(dr["BranchName"]),                                                     
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                          

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

        public QualityCheckViewModel GetQualityCheckDetail(long qualityCheckId = 0)
        {
            QualityCheckViewModel qc = new QualityCheckViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtgateins = sqlDbInterface.GetQualityCheckDetail(qualityCheckId);
                if (dtgateins != null && dtgateins.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtgateins.Rows)
                    {
                        qc = new QualityCheckViewModel
                        {

                           
                            QualityCheckId = Convert.ToInt32(dr["QualityCheckId"]),
                            QualityCheckNo = Convert.ToString(dr["QualityCheckNo"]),
                            QualityCheckDate = Convert.ToString(dr["QualityCheckDate"]),
                            GateInId = Convert.ToInt32(dr["GateInId"]),
                            GateInNo = Convert.ToString(dr["GateInNo"]),
                            GateInDate = Convert.ToString(dr["GateInDate"]),
                            POId = Convert.ToInt32(dr["POId"]),
                            PONo = Convert.ToString(dr["PONo"]),
                            CompanyBranchId=Convert.ToInt32(dr["CompanyBranchId"]),                                                     
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            RejectRemarks = Convert.ToString(dr["RejectRemarks"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return qc;
        }


        public DataTable GetGateInDetailDataTable(long gateinId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtGateIn = new DataTable();
            try
            {
                dtGateIn = sqlDbInterface.GetGateInDetail(gateinId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtGateIn;
        }

        public DataTable GetGateInProductListDataTable(long gateinId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetGateInProductList(gateinId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }


        public List<QualityCheckSupportingDocumentViewModel> GetQualityCheckSupportingDocumentList(long gateinId)
        {
            List<QualityCheckSupportingDocumentViewModel> qualityCheckDocuments = new List<QualityCheckSupportingDocumentViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDocument = sqlDbInterface.GetQualityCheckSupportingDocumentList(gateinId);
                if (dtDocument != null && dtDocument.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDocument.Rows)
                    {
                        qualityCheckDocuments.Add(new QualityCheckSupportingDocumentViewModel
                        {
                            QualityCheckDocId = Convert.ToInt32(dr["QualityCheckDocId"]),
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
            return qualityCheckDocuments;
        }

    

        public ResponseOut CancelGateIn(GateInViewModel gateinViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                GateIn gatein = new GateIn
                {
                    GateInId = gateinViewModel.GateInId,                  
                    ApprovalStatus = "Cancelled",
                    CreatedBy = gateinViewModel.CreatedBy,
                    CancelReason = gateinViewModel.CancelReason
                };
                responseOut = sqlDbInterface.CancelGateIn(gatein);
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



        public List<QualityCheckViewModel> GetQualityCheckRejectList(string qualityCheckNo, string gateInNo, string poNo, string fromDate, string toDate, int companyId, string companyBranch)
        {
            List<QualityCheckViewModel> qcs = new List<QualityCheckViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQualityCheckReject = sqlDbInterface.GetQualityCheckRejectList(qualityCheckNo,gateInNo, poNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, companyBranch);
                if (dtQualityCheckReject != null && dtQualityCheckReject.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQualityCheckReject.Rows)
                    {
                        qcs.Add(new QualityCheckViewModel
                        {
                            QualityCheckId = Convert.ToInt32(dr["QualityCheckID"]),
                            QualityCheckNo = Convert.ToString(dr["QualityCheckNo"]),
                            QualityCheckDate = Convert.ToString(dr["QualityCheckDate"]),
                            GateInId = Convert.ToInt32(dr["GateInId"]),
                            GateInNo = Convert.ToString(dr["GateInNo"]),
                            GateInDate = Convert.ToString(dr["GateInDate"]),
                            POId = Convert.ToInt32(dr["POId"]),
                            PONo = Convert.ToString(dr["PONo"]),
                            PODate = Convert.ToString(dr["PODate"]),
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["BranchName"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"])
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

        public List<QualityCheckViewModel> GetQualityCheckGateInList(string gateInNo, string poNo, string fromDate, string toDate, int companyId,string companyBranch)
        {
            List<QualityCheckViewModel> qcs = new List<QualityCheckViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPOs = sqlDbInterface.GetQualityCheckGateInList(gateInNo,poNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, companyBranch);
                if (dtPOs != null && dtPOs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPOs.Rows)
                    {
                        qcs.Add(new QualityCheckViewModel
                        {
                            GateInId = Convert.ToInt32(dr["GateInId"]),
                            GateInNo = Convert.ToString(dr["GateInNo"]),
                            GateInDate = Convert.ToString(dr["GateInDate"]),
                            POId = Convert.ToInt32(dr["POId"]),
                            PONo = Convert.ToString(dr["PONo"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["BranchName"]),
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
            return qcs;
        }

        public GateInViewModel GetGateInPODetail(long gateinId = 0)
        {
            GateInViewModel gatein = new GateInViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtmrns = sqlDbInterface.GetGateInPODetail(gateinId);
                if (dtmrns != null && dtmrns.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtmrns.Rows)
                    {
                        gatein = new GateInViewModel
                        {
                            GateInId = Convert.ToInt32(dr["GateInId"]),
                            GateInNo = Convert.ToString(dr["GateInNo"]),
                            GateInDate = Convert.ToString(dr["GateInDate"]),
                            GRNo = Convert.ToString(dr["GRNo"]),
                            GRDate = Convert.ToString(dr["GRDate"]),
                            POId = Convert.ToInt32(dr["POId"]),
                            PONo = Convert.ToString(dr["PONo"]),
                            

                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            VendorName = Convert.ToString(dr["VendorName"]),

                            ShippingContactPerson = Convert.ToString(dr["ShippingContactPerson"]),
                            ShippingBillingAddress = Convert.ToString(dr["ShippingBillingAddress"]),
                            ShippingCity = Convert.ToString(dr["ShippingCity"]),
                            ShippingStateId = Convert.ToInt32(dr["ShippingStateId"]),
                            ShippingCountryId = Convert.ToInt32(dr["ShippingCountryId"]),
                            ShippingPinCode = Convert.ToString(dr["ShippingPinCode"]),
                            ShippingTINNo = Convert.ToString(dr["ShippingTINNo"]),

                            ShippingEmail = Convert.ToString(dr["ShippingEmail"]),
                            ShippingMobileNo = Convert.ToString(dr["ShippingMobileNo"]),
                            ShippingContactNo = Convert.ToString(dr["ShippingContactNo"]),
                            ShippingFax = Convert.ToString(dr["ShippingFax"]),

                            CompanyBranchId = Convert.ToInt32(string.IsNullOrEmpty(dr["CompanyBranchId"].ToString()) ? "0" : dr["CompanyBranchId"]),
                            DispatchRefNo = Convert.ToString(dr["DispatchRefNo"]),
                            DispatchRefDate = Convert.ToString(dr["DispatchRefDate"]),

                            LRNo = Convert.ToString(dr["LRNo"]),
                            LRDate = Convert.ToString(dr["LRDate"]),

                            TransportVia = Convert.ToString(dr["TransportVia"]),
                            NoOfPackets = Convert.ToInt32(dr["NoOfPackets"]),

                            Remarks1 = Convert.ToString(dr["Remarks1"]),
                            Remarks2 = Convert.ToString(dr["Remarks2"]),


                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),

                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return gatein;
        }
        public List<GateInProductDetailViewModel> GetQualityCheckGateInProductList(long gateInId)
        {
            List<GateInProductDetailViewModel> piProducts = new List<GateInProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetQualityCheckGateInProductList(gateInId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        piProducts.Add(new GateInProductDetailViewModel
                        {                            
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),                          
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            TotalRecQuantity = Convert.ToDecimal(dr["TotalRecQuantity"]),
                            PendingQuantity = Convert.ToDecimal(dr["PendingQuantity"]),
                            ReceivedQuantity = Convert.ToDecimal(dr["ReceivedQuantity"]),
                            AcceptQuantity = Convert.ToDecimal(dr["AcceptQuantity"]),
                            RejectQuantity = Convert.ToDecimal(dr["RejectQuantity"])                           
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return piProducts;
        }
    }
}
