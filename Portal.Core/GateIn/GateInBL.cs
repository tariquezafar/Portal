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
 public class GateInBL
    {
        DBInterface dbInterface;
        public GateInBL()
        {
            dbInterface = new DBInterface();
        }
        public List<GateInProductDetailViewModel> GetGateInProductList(long gateinId)
        {
            List<GateInProductDetailViewModel> gateinProducts = new List<GateInProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetGateInProductList(gateinId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {

                        gateinProducts.Add(new GateInProductDetailViewModel
                        {
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            GateInProductDetailId = Convert.ToInt32(dr["GateInProductDetailId"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            Price = Convert.ToDecimal(dr["Price"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            ReceivedQuantity= Convert.ToDecimal(dr["ReceivedQuantity"]),
                            AcceptQuantity= Convert.ToDecimal(dr["AcceptQuantity"]),
                            RejectQuantity = Convert.ToDecimal(dr["RejectQuantity"])
,                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return gateinProducts;
        }

        public ResponseOut AddEditGateIn(GateInViewModel gateinViewModel, List<GateInProductDetailViewModel> gateinProducts, List<GateInSupportingDocumentViewModel> gateinDocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                GateIn gatein = new GateIn
                {
                    GateInId = gateinViewModel.GateInId,
                    GateInDate =Convert.ToDateTime(gateinViewModel.GateInDate),
                    GRNo= gateinViewModel.GRNo,
                    GRDate=Convert.ToDateTime(gateinViewModel.GRDate),
                    PONo = gateinViewModel.PONo,
                    POId = gateinViewModel.POId,
                    VendorId= gateinViewModel.VendorId,
                    VendorName = gateinViewModel.VendorName,
                    ContactPerson = gateinViewModel.ContactPerson,
                    ShippingContactPerson = gateinViewModel.ShippingContactPerson,
                    ShippingBillingAddress = gateinViewModel.ShippingBillingAddress,
                    ShippingCity = gateinViewModel.ShippingCity,
                    ShippingStateId = gateinViewModel.ShippingStateId,
                    ShippingCountryId = gateinViewModel.ShippingCountryId,
                    ShippingPinCode = gateinViewModel.ShippingPinCode,
                    ShippingTINNo = gateinViewModel.ShippingTINNo,
                    ShippingEmail = gateinViewModel.ShippingEmail,
                    ShippingMobileNo = gateinViewModel.ShippingMobileNo,
                    ShippingContactNo = gateinViewModel.ShippingContactNo,
                    ShippingFax = gateinViewModel.ShippingFax,
                    CompanyBranchId= gateinViewModel.CompanyBranchId,
                    DispatchRefNo = gateinViewModel.DispatchRefNo,
                    DispatchRefDate = string.IsNullOrEmpty(gateinViewModel.DispatchRefDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(gateinViewModel.DispatchRefDate),
                    LRNo = gateinViewModel.LRNo,
                    LRDate = string.IsNullOrEmpty(gateinViewModel.LRDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(gateinViewModel.LRDate),
                    TransportVia = gateinViewModel.TransportVia,
                    NoOfPackets = gateinViewModel.NoOfPackets,
                    Remarks1 = gateinViewModel.Remarks1,
                    Remarks2 = gateinViewModel.Remarks2,
                    FinYearId = gateinViewModel.FinYearId,
                    CompanyId = gateinViewModel.CompanyId,
                    CreatedBy = gateinViewModel.CreatedBy,
                    ApprovalStatus= gateinViewModel.ApprovalStatus

                };
                List<GateInProductDetail> gateinProductList = new List<GateInProductDetail>();
                if (gateinProducts != null && gateinProducts.Count > 0)
                {
                    foreach (GateInProductDetailViewModel item in gateinProducts)
                    {
                        gateinProductList.Add(new GateInProductDetail
                        {
                            ProductId = item.ProductId,
                            ProductShortDesc = item.ProductShortDesc,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            ReceivedQuantity=item.ReceivedQuantity,
                            AcceptQuantity = item.AcceptQuantity,
                            RejectQuantity = item.RejectQuantity,
                        });
                    }
                }

                List<GateInSupportingDocument> gateinDocumentList = new List<GateInSupportingDocument>();
                if (gateinDocuments != null && gateinDocuments.Count > 0)
                {
                    foreach (GateInSupportingDocumentViewModel item in gateinDocuments)
                    {
                        gateinDocumentList.Add(new GateInSupportingDocument
                        {
                            DocumentTypeId = item.DocumentTypeId,
                            DocumentName = item.DocumentName,
                            DocumentPath = item.DocumentPath
                        });
                    }
                }

                responseOut = sqlDbInterface.AddEditGateIn(gatein, gateinProductList, gateinDocumentList);

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
        public List<GateInViewModel> GetGateInList(string gateinNo, string vendorName, string dispatchrefNo, string fromDate, string toDate, int companyId, string approvalStatus = "", string companyBranch = "")
        {
            List<GateInViewModel>  gateins = new List<GateInViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtGateIns = sqlDbInterface.GetGateInList(gateinNo, vendorName, dispatchrefNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, approvalStatus, companyBranch);
                if (dtGateIns != null && dtGateIns.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtGateIns.Rows)
                    {

                        gateins.Add(new GateInViewModel
                        {
                            GateInId = Convert.ToInt32(dr["GateInId"]),
                            GateInNo = Convert.ToString(dr["GateInNo"]),
                            GateInDate = Convert.ToString(dr["GateInDate"]),
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            PONo = Convert.ToString(dr["PONo"]),
                            PODate=Convert.ToString(dr["PODate"]),
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
                            CompanyBranchName = Convert.ToString(dr["BranchName"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return gateins;
        }

        public GateInViewModel GetGateInDetail(long gateinId = 0)
        {
            GateInViewModel gatein = new GateInViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtgateins = sqlDbInterface.GetGateInDetail(gateinId);
                if (dtgateins != null && dtgateins.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtgateins.Rows)
                    {
                        gatein = new GateInViewModel
                        {
                            GateInId = Convert.ToInt32(dr["GateInId"]),
                            GateInNo = Convert.ToString(dr["GateInNo"]),
                            GateInDate = Convert.ToString(dr["GateInDate"]),
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
            return gatein;
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


        public List<GateInSupportingDocumentViewModel> GetGateInSupportingDocumentList(long gateinId)
        {
            List<GateInSupportingDocumentViewModel> gateinDocuments = new List<GateInSupportingDocumentViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDocument = sqlDbInterface.GetGateInSupportingDocumentList(gateinId);
                if (dtDocument != null && dtDocument.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDocument.Rows)
                    {
                        gateinDocuments.Add(new GateInSupportingDocumentViewModel
                        {
                            GateInDocId = Convert.ToInt32(dr["GateInDocId"]),
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
            return gateinDocuments;
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

        public List<POViewModel> GetGateInPOList(string poNo, string vendorName, string refNo, string fromDate, string toDate, int companyId,string companyBranch)
        {
            List<POViewModel> pos = new List<POViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPOs = sqlDbInterface.GetGateInPOList(poNo, vendorName, refNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, companyBranch);
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
        public List<POProductViewModel> GetPOGateInProductList(long poid)
        {
            List<POProductViewModel> piProducts = new List<POProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetPOGateInProductList(poid);
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
    }
}
