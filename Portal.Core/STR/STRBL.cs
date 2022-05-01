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
  public class STRBL
    {
        DBInterface dbInterface;
        public STRBL()
        {
            dbInterface = new DBInterface();
        }
        public List<STRProductDetailViewModel> GetSTRProductList(long strId)
        {
            List<STRProductDetailViewModel> strProducts = new List<STRProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetSTRProductList(strId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {

                        strProducts.Add(new STRProductDetailViewModel
                        {
                            STRProductDetailId = Convert.ToInt32(dr["STRProductDetailId"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Price = Convert.ToDecimal(dr["Price"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            TotalPrice=Convert.ToDecimal(dr["TotalPrice"]),
                            SequenceNo = Convert.ToInt32(dr["SequenceNo"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return strProducts;
        }
        public ResponseOut AddEditSTR(STRViewModel strViewModel, List<STRProductDetailViewModel> strProducts,List<STRSupportingDocumentViewModel> strDocuments,List<STRProductSerialDetailViewModel> strProductChasisList)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                STR str = new STR
                {
                    STRId = strViewModel.STRId,
                    STRDate = Convert.ToDateTime(strViewModel.STRDate),
                    STNId= strViewModel.STNId,
                    STNNo = Convert.ToString(strViewModel.STNNo),
                    GRNo = string.IsNullOrEmpty(strViewModel.GRNo) ? "" : strViewModel.GRNo,
                    GRDate = string.IsNullOrEmpty(strViewModel.GRDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(strViewModel.GRDate),
                    
                    //ContactPerson = strViewModel.ContactPerson,
                    FromWarehouseId = strViewModel.FromWarehouseId,
                    ToWarehouseId = strViewModel.ToWarehouseId,

                    DispatchRefNo = strViewModel.DispatchRefNo,
                    DispatchRefDate = string.IsNullOrEmpty(strViewModel.DispatchRefDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(strViewModel.DispatchRefDate),
                    LRNo = strViewModel.LRNo,
                    LRDate = string.IsNullOrEmpty(strViewModel.LRDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(strViewModel.LRDate),
                    TransportVia = strViewModel.TransportVia,
                    NoOfPackets = strViewModel.NoOfPackets,
                    BasicValue = strViewModel.BasicValue,
                    TotalValue= strViewModel.TotalValue,
                    FreightValue= strViewModel.FreightValue,
                    LoadingValue= strViewModel.LoadingValue,

                    Remarks1 = strViewModel.Remarks1,
                    Remarks2 = strViewModel.Remarks2,
                    FinYearId = strViewModel.FinYearId,
                    CompanyId = strViewModel.CompanyId,
                    CreatedBy = strViewModel.CreatedBy,
                    ApprovalStatus=strViewModel.ApprovalStatus

                };
                List<STRProductDetail> strProductList = new List<STRProductDetail>();
                if (strProducts != null && strProducts.Count > 0)
                {
                    foreach (STRProductDetailViewModel item in strProducts)
                    {
                        strProductList.Add(new STRProductDetail
                        {
                            ProductId = item.ProductId,
                            ProductShortDesc = item.ProductShortDesc,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            TotalPrice=item.TotalPrice
                        });
                    }
                }

                List<STRSupportingDocument> strDocumentList = new List<STRSupportingDocument>();
                if (strDocuments != null && strDocuments.Count > 0)
                {
                    foreach (STRSupportingDocumentViewModel item in strDocuments)
                    {
                        strDocumentList.Add(new STRSupportingDocument
                        {
                            DocumentTypeId = item.DocumentTypeId,
                            DocumentName = item.DocumentName,
                            DocumentPath = item.DocumentPath
                        });
                    }
                }


                List<STRProductSerialDetail> sTRProductSerialDetail = new List<STRProductSerialDetail>();
                if (strProductChasisList != null && strProductChasisList.Count > 0)
                {
                    foreach (STRProductSerialDetailViewModel item in strProductChasisList)
                    {
                        sTRProductSerialDetail.Add(new STRProductSerialDetail
                        {
                            ProductId = item.ProductId,
                            ChasisSerialNo = item.ChasisNo,
                            
                        });
                    }
                }

                responseOut = sqlDbInterface.AddEditSTR(str, strProductList, strDocumentList, sTRProductSerialDetail);

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

        public List<STRViewModel> GetSTRList(string strNo, string grNo, int fromLocation, int toLocation, string fromDate, string toDate, int companyId,string approvalStatus)
        {
            List<STRViewModel> strs = new List<STRViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSTRs = sqlDbInterface.GetSTRList(strNo, grNo, fromLocation, toLocation, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, approvalStatus);


                if (dtSTRs != null && dtSTRs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSTRs.Rows)
                    {

                        strs.Add(new STRViewModel
                        {
                            STRId = Convert.ToInt32(dr["STRId"]),
                            STRNo = Convert.ToString(dr["STRNo"]),
                            STRDate = Convert.ToString(dr["STRDate"]),
                            GRNo = Convert.ToString(dr["GRNo"]),
                            FormLocationName = Convert.ToString(dr["FormLocation"]),
                            ToLocationName = Convert.ToString(dr["ToLocation"]),
                            DispatchRefNo = Convert.ToString(dr["DispatchRefNo"]),
                            DispatchRefDate = Convert.ToString(dr["DispatchRefDate"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
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
            return strs;
        }

        public STRViewModel GetSTRDetail(long strId = 0)
        {
            STRViewModel str = new STRViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtstns = sqlDbInterface.GetSTRDetail(strId);
                if (dtstns != null && dtstns.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtstns.Rows)
                    {
                        str = new STRViewModel
                        {
                            STRId = Convert.ToInt32(dr["STRId"]),
                            STRNo = Convert.ToString(dr["STRNo"]),
                            STRDate = Convert.ToString(dr["STRDate"]),
                            STNId = Convert.ToInt32(dr["STNId"]),
                            STNNo =Convert.ToString(dr["STNNo"]),
                            STNDate=Convert.ToString(dr["STNDate"]),
                            GRNo = Convert.ToString(dr["GRNo"]),
                            GRDate = Convert.ToString(dr["GRDate"]),
                          
                            FromWarehouseId = Convert.ToInt32(dr["FromWarehouseId"]),
                            ToWarehouseId = Convert.ToInt32(dr["ToWarehouseId"]),

                            DispatchRefNo = Convert.ToString(dr["DispatchRefNo"]),
                            DispatchRefDate = Convert.ToString(dr["DispatchRefDate"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),


                            LRNo = Convert.ToString(dr["LRNo"]),
                            LRDate = Convert.ToString(dr["LRDate"]),

                            TransportVia = Convert.ToString(dr["TransportVia"]),
                            NoOfPackets = Convert.ToInt32(dr["NoOfPackets"]),

                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            FreightValue = Convert.ToDecimal(dr["FreightValue"]),
                            LoadingValue = Convert.ToDecimal(dr["LoadingValue"]),


                            Remarks1 = Convert.ToString(dr["Remarks1"]),
                            Remarks2 = Convert.ToString(dr["Remarks2"]),

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
            return str;
        }

        public DataTable GetSTRDetailDataTable(long strId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtSTR = new DataTable();
            try
            {
                dtSTR = sqlDbInterface.GetSTRDetail(strId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtSTR;
        }

        public DataTable GetSTRProductListDataTable(long strId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetSTRProductList(strId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }
        public List<STRSupportingDocumentViewModel> GetSTRSupportingDocumentList(long strId)
        {
            List<STRSupportingDocumentViewModel> strDocuments = new List<STRSupportingDocumentViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDocument = sqlDbInterface.GetSTRSupportingDocumentList(strId);
                if (dtDocument != null && dtDocument.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDocument.Rows)
                    {
                        strDocuments.Add(new STRSupportingDocumentViewModel
                        {
                            STRDocId = Convert.ToInt32(dr["STRDocId"]),
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
            return strDocuments;
        }

        public List<STNProductDetailViewModel> GetSTRSTNProductList(long stnId)
        {
            List<STNProductDetailViewModel> stnProducts = new List<STNProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetSTRSTNProductList(stnId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {

                        stnProducts.Add(new STNProductDetailViewModel
                        {
                            STNProductDetailId = Convert.ToInt32(dr["STNProductDetailId"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Price = Convert.ToDecimal(dr["Price"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            TotalPrice = Convert.ToDecimal(dr["TotalPrice"]),
                            SequenceNo = Convert.ToInt32(dr["SequenceNo"])

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return stnProducts;
        }
        public List<STNChasisProductSerialDetailViewModel> Get_STR_STN_ChasisList(long stnId,int mode)
        {
            List<STNChasisProductSerialDetailViewModel> sTNChasisProductSerialDetailViewModel = new List<STNChasisProductSerialDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.Get_STR_STN_ChasisList(stnId, mode);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        sTNChasisProductSerialDetailViewModel.Add(new STNChasisProductSerialDetailViewModel
                        {
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            RefSerial1 = Convert.ToString(dr["ChasisSerialNo"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sTNChasisProductSerialDetailViewModel;
        }
    }
}
