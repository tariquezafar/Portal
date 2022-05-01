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
 public class MRNPOBL
    {
        DBInterface dbInterface;
        public MRNPOBL()
        {
            dbInterface = new DBInterface();
        }
        public List<MRNProductDetailViewModel> GetMRNProductList(long mrnId)
        {
            List<MRNProductDetailViewModel> mrnProducts = new List<MRNProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetMRNPOProductList(mrnId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {

                        mrnProducts.Add(new MRNProductDetailViewModel
                        {
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            MRNProductDetailId = Convert.ToInt32(dr["MRNProductDetailId"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            Price = Convert.ToDecimal(dr["Price"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            ReceivedQuantity = Convert.ToDecimal(dr["ReceivedQuantity"]),
                            AcceptQuantity = Convert.ToDecimal(dr["AcceptQuantity"]),
                            RejectQuantity = Convert.ToDecimal(dr["RejectQuantity"]),
                            PendingQuantity = Convert.ToDecimal(dr["PendingQuantity"]),
                            TotalRecQuantity = Convert.ToDecimal(dr["TotalRecQuantity"]),
                            UOMName = Convert.ToString(dr["UOMName"])
,                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return mrnProducts;
        }

        public ResponseOut AddEditMRNPO(MRNViewModel mrnViewModel, List<MRNProductDetailViewModel> mrnProducts, List<MRNSupportingDocumentViewModel> mrnDocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                MRN mrn = new MRN
                {
                    MRNId= mrnViewModel.MRNId,
                    MRNDate=Convert.ToDateTime(mrnViewModel.MRNDate),
                    GRNo=mrnViewModel.GRNo,
                    GRDate=Convert.ToDateTime(mrnViewModel.GRDate),
                    PONo = mrnViewModel.PONo,
                    POId = mrnViewModel.POId,
                    VendorId= mrnViewModel.VendorId,
                    VendorName = mrnViewModel.VendorName,
                    ContactPerson = mrnViewModel.ContactPerson,
                    ShippingContactPerson = mrnViewModel.ShippingContactPerson,
                    ShippingBillingAddress = mrnViewModel.ShippingBillingAddress,
                    ShippingCity = mrnViewModel.ShippingCity,
                    ShippingStateId = mrnViewModel.ShippingStateId,
                    ShippingCountryId = mrnViewModel.ShippingCountryId,
                    ShippingPinCode = mrnViewModel.ShippingPinCode,
                    ShippingTINNo = mrnViewModel.ShippingTINNo,
                    ShippingEmail = mrnViewModel.ShippingEmail,
                    ShippingMobileNo = mrnViewModel.ShippingMobileNo,
                    ShippingContactNo = mrnViewModel.ShippingContactNo,
                    ShippingFax = mrnViewModel.ShippingFax,
                    CompanyBranchId=mrnViewModel.CompanyBranchId,
                    DispatchRefNo = mrnViewModel.DispatchRefNo,
                    DispatchRefDate = string.IsNullOrEmpty(mrnViewModel.DispatchRefDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(mrnViewModel.DispatchRefDate),
                    LRNo = mrnViewModel.LRNo,
                    LRDate = string.IsNullOrEmpty(mrnViewModel.LRDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(mrnViewModel.LRDate),
                    TransportVia = mrnViewModel.TransportVia,
                    NoOfPackets = mrnViewModel.NoOfPackets,
                    Remarks1 = mrnViewModel.Remarks1,
                    Remarks2 = mrnViewModel.Remarks2,
                    FinYearId = mrnViewModel.FinYearId,
                    CompanyId = mrnViewModel.CompanyId,
                    CreatedBy = mrnViewModel.CreatedBy,
                    ApprovalStatus=mrnViewModel.ApprovalStatus

                };
                List<MRNProductDetail> mrnProductList = new List<MRNProductDetail>();
                if (mrnProducts != null && mrnProducts.Count > 0)
                {
                    foreach (MRNProductDetailViewModel item in mrnProducts)
                    {
                        mrnProductList.Add(new MRNProductDetail
                        {
                            ProductId = item.ProductId,
                            ProductShortDesc = item.ProductShortDesc,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            ReceivedQuantity=item.ReceivedQuantity,
                            AcceptQuantity = item.AcceptQuantity,
                            RejectQuantity = item.RejectQuantity
                        });
                    }
                }

                List<MRNSupportingDocument> mrnDocumentList = new List<MRNSupportingDocument>();
                if (mrnDocuments != null && mrnDocuments.Count > 0)
                {
                    foreach (MRNSupportingDocumentViewModel item in mrnDocuments)
                    {
                        mrnDocumentList.Add(new MRNSupportingDocument
                        {
                            DocumentTypeId = item.DocumentTypeId,
                            DocumentName = item.DocumentName,
                            DocumentPath = item.DocumentPath
                        });
                    }
                }

                responseOut = sqlDbInterface.AddEditMRNPO(mrn, mrnProductList, mrnDocumentList);

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
        public List<MRNViewModel> GetMRNPOList(string mrnNo, string vendorName, string dispatchrefNo, string fromDate, string toDate, int companyId,string approvalStatus="",string companyBranch="", string list = "")
        {
            List<MRNViewModel> mrns = new List<MRNViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtMRNs = sqlDbInterface.GetMRNPOList(mrnNo, vendorName, dispatchrefNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, approvalStatus, companyBranch, list);
                if (dtMRNs != null && dtMRNs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtMRNs.Rows)
                    {

                        mrns.Add(new MRNViewModel
                        {
                            MRNId = Convert.ToInt32(dr["MRNId"]),
                            MRNNo = Convert.ToString(dr["MRNNo"]),
                            MRNDate = Convert.ToString(dr["MRNDate"]),
                            PONo = Convert.ToString(dr["PONo"]),
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            ShippingCity = Convert.ToString(dr["ShippingCity"]),
                            ShippingStateName = Convert.ToString(dr["StateName"]),
                            DispatchRefNo = Convert.ToString(dr["DispatchRefNo"]),
                            DispatchRefDate = Convert.ToString(dr["DispatchRefDate"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
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
            return mrns;
        }

        public MRNViewModel GetMRNPODetail(long mrnId = 0)
        {
            MRNViewModel mrn = new MRNViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtmrns = sqlDbInterface.GetMRNPODetail(mrnId);
                if (dtmrns != null && dtmrns.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtmrns.Rows)
                    {
                        mrn = new MRNViewModel
                        {
                            MRNId = Convert.ToInt32(dr["MRNId"]),
                            MRNNo = Convert.ToString(dr["MRNNo"]),
                            MRNDate = Convert.ToString(dr["MRNDate"]),
                            GRNo = Convert.ToString(dr["GRNo"]),
                            GRDate=Convert.ToString(dr["GRDate"]),
                            POId = Convert.ToInt32(dr["POId"]),
                            PONo = Convert.ToString(dr["PONo"]),
                            PODate = Convert.ToString(dr["PODate"]),

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
            return mrn;
        }


        public DataTable GetMRNDetailDataTable(long mrnId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtMRN = new DataTable();
            try
            {
                dtMRN = sqlDbInterface.GetMRNPODetail(mrnId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtMRN;
        }

        public DataTable GetMRNProductListDataTable(long mrnId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetPOMRNProductListPrint(mrnId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }


        public List<MRNSupportingDocumentViewModel> GetMRNSupportingDocumentList(long mrnId)
        {
            List<MRNSupportingDocumentViewModel> mrnDocuments = new List<MRNSupportingDocumentViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDocument = sqlDbInterface.GetMRNSupportingDocumentList(mrnId);
                if (dtDocument != null && dtDocument.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDocument.Rows)
                    {
                        mrnDocuments.Add(new MRNSupportingDocumentViewModel
                        {
                            MRNDocId = Convert.ToInt32(dr["MRNDocId"]),
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
            return mrnDocuments;
        }

        public List<POViewModel> GetMRNPOList(string poNo, string vendorName, string refNo, string fromDate, string toDate,int companyId,string companyBranch,string popup)
        {
            List<POViewModel> pos = new List<POViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPOs = sqlDbInterface.GetMRNPOList(poNo, vendorName, refNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, companyBranch, popup);
                if (dtPOs != null && dtPOs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPOs.Rows)
                    {
                        pos.Add(new POViewModel
                        {
                            POId = Convert.ToInt32(dr["POId"]),
                            PONo = Convert.ToString(dr["PONo"]),
                            PODate = Convert.ToString(dr["PODate"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            ConsigneeName = Convert.ToString(dr["ConsigneeName"]),
                            ConsigneeId = Convert.ToInt32(dr["ConsigneeId"]),
                            ConsigneeCode = Convert.ToString(dr["ConsigneeCode"]),
                            City = Convert.ToString(dr["City"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            POStatus = Convert.ToString(dr["POStatus"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            PORevisedStatus = Convert.ToBoolean(dr["PORevisedStatus"])
                            
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pos;
        }

        public List<POProductViewModel> GetPOMRNProductList(long poid)
        {
            List<POProductViewModel> piProducts = new List<POProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetPOMRNProductList(poid);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        piProducts.Add(new POProductViewModel
                        {
                            POProductDetailId = Convert.ToInt32(dr["POProductDetailId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Price = Convert.ToDecimal(dr["Price"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            TotalRecQuantity = Convert.ToDecimal(dr["TotalRecQuantity"]),
                            PendingQuantity = Convert.ToDecimal(dr["PendingQuantity"]),
                            ReceivedQuantity = Convert.ToDecimal(dr["ReceivedQuantity"]),
                            AcceptQuantity = Convert.ToDecimal(dr["AcceptQuantity"]),
                            RejectQuantity = Convert.ToDecimal(dr["RejectQuantity"]),                            
                            TotalPrice = Convert.ToDecimal(dr["TotalPrice"]),                           
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

        public ResponseOut CancelMRNPO(MRNViewModel mrnViewModel, List<MRNProductDetailViewModel> mrnProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                MRN mrn = new MRN
                {
                    MRNId = mrnViewModel.MRNId,
                    POId = mrnViewModel.POId,
                    ApprovalStatus = "Cancelled",
                    CreatedBy = mrnViewModel.CreatedBy,
                    CancelReason = mrnViewModel.CancelReason,
                    FinYearId = mrnViewModel.FinYearId,
                    CompanyBranchId= mrnViewModel.CompanyBranchId,
                    CompanyId=mrnViewModel.CompanyId
                };

                List<MRNProductDetail> mrnProductList = new List<MRNProductDetail>();
                if (mrnProducts != null && mrnProducts.Count > 0)
                {
                    foreach (MRNProductDetailViewModel item in mrnProducts)
                    {
                        mrnProductList.Add(new MRNProductDetail
                        {
                            ProductId = item.ProductId,                           
                            ReceivedQuantity = item.ReceivedQuantity,
                            AcceptQuantity = item.AcceptQuantity,
                            RejectQuantity = item.RejectQuantity
                        });
                    }
                }
                responseOut = sqlDbInterface.CancelMRNPO(mrn, mrnProductList);
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

        public List<MRNProductDetailViewModel> GetCancelMRNProductList(long mrnId)
        {
            List<MRNProductDetailViewModel> mrnProducts = new List<MRNProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetCancelMRNPOProductList(mrnId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {

                        mrnProducts.Add(new MRNProductDetailViewModel
                        {
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            MRNProductDetailId = Convert.ToInt32(dr["MRNProductDetailId"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            Price = Convert.ToDecimal(dr["Price"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            ReceivedQuantity = Convert.ToDecimal(dr["ReceivedQuantity"]),
                            AcceptQuantity = Convert.ToDecimal(dr["AcceptQuantity"]),
                            RejectQuantity = 0,
                            PendingQuantity = 0,
                            TotalRecQuantity = 0,
                            UOMName = Convert.ToString(dr["UOMName"])
,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return mrnProducts;
        }
    }
}
