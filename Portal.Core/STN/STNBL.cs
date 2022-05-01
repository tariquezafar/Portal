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
  public class STNBL
    {
        DBInterface dbInterface;
        public STNBL()
        {
            dbInterface = new DBInterface();
        }
        public List<STNProductDetailViewModel> GetSTNProductList(long stnId)
        {
            List<STNProductDetailViewModel> stnProducts = new List<STNProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetSTNProductList(stnId);
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
                            TotalPrice=Convert.ToDecimal(dr["TotalPrice"]),
                            IsSerializedProduct= Convert.ToString(dr["IsSerializedProduct"]),
                            SequenceNo= Convert.ToInt32(dr["SequenceNo"])

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
        public ResponseOut AddEditSTN(STNViewModel stnViewModel, List<STNProductDetailViewModel> stnProducts,List<STNSupportingDocumentViewModel> stnDocuments, List<STNChasisProductSerialDetailViewModel> stnChasisProductSerialDetail)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                STN stn = new STN
                {
                    STNId = stnViewModel.STNId,
                    STNDate = Convert.ToDateTime(stnViewModel.STNDate),
                    GRNo = string.IsNullOrEmpty(stnViewModel.GRNo) ? "" : stnViewModel.GRNo,
                    GRDate = string.IsNullOrEmpty(stnViewModel.GRDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(stnViewModel.GRDate),                   

                    ContactPerson = stnViewModel.ContactPerson,
                    FromWarehouseId = stnViewModel.FromWarehouseId,
                    ToWarehouseId = stnViewModel.ToWarehouseId,

                    DispatchRefNo = stnViewModel.DispatchRefNo,
                    DispatchRefDate = string.IsNullOrEmpty(stnViewModel.DispatchRefDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(stnViewModel.DispatchRefDate),
                    LRNo = stnViewModel.LRNo,
                    LRDate = string.IsNullOrEmpty(stnViewModel.LRDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(stnViewModel.LRDate),
                    TransportVia = stnViewModel.TransportVia,
                    NoOfPackets = stnViewModel.NoOfPackets,

                    BasicValue = stnViewModel.BasicValue,
                    TotalValue=stnViewModel.TotalValue,
                    FreightValue=stnViewModel.FreightValue,
                    LoadingValue=stnViewModel.LoadingValue,

                    Remarks1 = stnViewModel.Remarks1,
                    Remarks2 = stnViewModel.Remarks2,
                    FinYearId = stnViewModel.FinYearId,
                    CompanyId = stnViewModel.CompanyId,
                    CreatedBy = stnViewModel.CreatedBy,
                    ApprovalStatus=stnViewModel.ApprovalStatus

                };
                List<STNProductDetail> stnProductList = new List<STNProductDetail>();
                if (stnProducts != null && stnProducts.Count > 0)
                {
                    foreach (STNProductDetailViewModel item in stnProducts)
                    {
                        stnProductList.Add(new STNProductDetail
                        {
                            ProductId = item.ProductId,
                            ProductShortDesc = item.ProductShortDesc,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            TotalPrice=item.TotalPrice
                        });
                    }
                }

                List<STNSupportingDocument> stnDocumentList = new List<STNSupportingDocument>();
                if (stnDocuments != null && stnDocuments.Count > 0)
                {
                    foreach (STNSupportingDocumentViewModel item in stnDocuments)
                    {
                        stnDocumentList.Add(new STNSupportingDocument
                        {
                            DocumentTypeId = item.DocumentTypeId,
                            DocumentName = item.DocumentName,
                            DocumentPath = item.DocumentPath
                        });
                    }
                }

                List<StnProductSerialDetail> stnChasisProductSerialDetailList = new List<StnProductSerialDetail>();
                if(stnChasisProductSerialDetail!=null && stnChasisProductSerialDetail.Count>0)
                {
                    foreach (STNChasisProductSerialDetailViewModel item in stnChasisProductSerialDetail)
                    {
                        stnChasisProductSerialDetailList.Add(new StnProductSerialDetail
                        {
                            
                            STNProductDetailId=Convert.ToInt64(item.STNProductDetailId),
                            STNId= Convert.ToInt64(item.StnId),
                          
                            ProductId = Convert.ToInt64(item.ProductId),
                            ChasisSerialNo= Convert.ToString(item.RefSerial1),

                        });
                    }
                }
                responseOut = sqlDbInterface.AddEditSTN(stn, stnProductList, stnDocumentList, stnChasisProductSerialDetailList);

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



        public List<STNViewModel> GetSTNList(string stnNo, string glNo, int fromLocation, int toLocation, string fromDate, string toDate, int companyId, string displayType,string approvalStatus)
        {
            List<STNViewModel> stns = new List<STNViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSTNs = sqlDbInterface.GetSTNList(stnNo, glNo, fromLocation, toLocation, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, displayType,approvalStatus);

                    
                if (dtSTNs != null && dtSTNs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSTNs.Rows)
                    {

                        stns.Add(new STNViewModel
                        {
                            STNId = Convert.ToInt32(dr["STNId"]),
                            STNNo = Convert.ToString(dr["STNNo"]),
                            STNDate = Convert.ToString(dr["STNDate"]),
                            GRNo = Convert.ToString(dr["GRNo"]),
                            FromWarehouseId=Convert.ToInt32(dr["FromWarehouseId"]),
                            ToWarehouseId=Convert.ToInt32(dr["ToWarehouseId"]),
                            FormLocationName = Convert.ToString(dr["FormLocation"]),
                            ToLocationName=Convert.ToString(dr["ToLocation"]),
                            DispatchRefNo = Convert.ToString(dr["DispatchRefNo"]),
                            DispatchRefDate = Convert.ToString(dr["DispatchRefDate"]),
                            ApprovalStatus=Convert.ToString(dr["ApprovalStatus"]),
                            TotalValue=Convert.ToDecimal(dr["Totalvalue"]),
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

        public STNViewModel GetSTNDetail(long stnId = 0)
        {
            STNViewModel stn = new STNViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtstns = sqlDbInterface.GetSTNDetail(stnId);
                if (dtstns != null && dtstns.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtstns.Rows)
                    {
                        stn = new STNViewModel
                        {
                            STNId = Convert.ToInt32(dr["STNId"]),
                            STNNo = Convert.ToString(dr["STNNo"]),
                            STNDate = Convert.ToString(dr["STNDate"]),
                            GRNo = Convert.ToString(dr["GRNo"]),
                            GRDate = Convert.ToString(dr["GRDate"]),
                            ContactPerson=Convert.ToString(dr["ContactPerson"]),
                            FromWarehouseId=Convert.ToInt32(dr["FromWarehouseId"]),
                            ToWarehouseId=Convert.ToInt32(dr["ToWarehouseId"]),

                            DispatchRefNo = Convert.ToString(dr["DispatchRefNo"]),
                            DispatchRefDate = Convert.ToString(dr["DispatchRefDate"]),

                            LRNo = Convert.ToString(dr["LRNo"]),
                            LRDate = Convert.ToString(dr["LRDate"]),

                            TransportVia = Convert.ToString(dr["TransportVia"]),
                            NoOfPackets = Convert.ToInt32(dr["NoOfPackets"]),

                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            FreightValue= Convert.ToDecimal(dr["FreightValue"]),
                            LoadingValue= Convert.ToDecimal(dr["LoadingValue"]),
                            

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
            return stn;
        }

        public DataTable GetSTNDetailDataTable(long stnId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtSTN = new DataTable();
            try
            {
                dtSTN = sqlDbInterface.GetSTNDetail(stnId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtSTN;
        }


        

              public DataTable GetSTNProductChasisSerialList(long stnId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProductChasisSerialNos = new DataTable();
            try
            {
                dtProductChasisSerialNos = sqlDbInterface.GetSTNProductChasisSerialList(stnId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProductChasisSerialNos;
        }


        public DataTable GetSTNProductListDataTable(long stnId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetSTNProductList(stnId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }

        public List<STNSupportingDocumentViewModel> GetSTNSupportingDocumentList(long stnId)
        {
            List<STNSupportingDocumentViewModel> stnDocuments = new List<STNSupportingDocumentViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDocument = sqlDbInterface.GetSTNSupportingDocumentList(stnId);
                if (dtDocument != null && dtDocument.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDocument.Rows)
                    {
                        stnDocuments.Add(new STNSupportingDocumentViewModel
                        {
                            STNDocId = Convert.ToInt32(dr["STNDocId"]),
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
            return stnDocuments;
        }


        public List<STNChasisProductSerialDetailViewModel> GetSTNChasisProductList(long productId,string chasisSerialNo,int createdBy,int mode,int companyBranch)
        {
            List<STNChasisProductSerialDetailViewModel> sTNChasisProductSerialDetailViewModel = new List<STNChasisProductSerialDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetSTNChasisProductList(productId, chasisSerialNo, createdBy, mode, companyBranch);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        sTNChasisProductSerialDetailViewModel.Add(new STNChasisProductSerialDetailViewModel
                        {
                            SequenceNo = Convert.ToInt32(dr["SequenceNo"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            RefSerial1 = Convert.ToString(dr["RefSerial1"]),
                            StnId = Convert.ToInt32(dr["STNId"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            Status = Convert.ToInt32(dr["status"]),
                            Price=Convert.ToDecimal(dr["Price"]),
                            IsSerializedProduct=Convert.ToString(dr["IsSerializedProduct"])
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

        public List<STNChasisProductSerialDetailViewModel> GetSTNChasisProductListAll(List<STNChasisProductSerialDetailViewModel> sTNChasisProductSerialDetailViewModelData)
        {
            int SequenceNo = 0;
            List<STNChasisProductSerialDetailViewModel> sTNChasisProductSerialDetailViewModel = new List<STNChasisProductSerialDetailViewModel>();

          
            foreach (var data in sTNChasisProductSerialDetailViewModelData)
            {
                SequenceNo++;
                sTNChasisProductSerialDetailViewModel.Add(new STNChasisProductSerialDetailViewModel
                {
                    SequenceNo = Convert.ToInt32(SequenceNo),
                    ProductName = Convert.ToString(data.ProductName),
                    RefSerial1 = Convert.ToString(data.RefSerial1),
                    StnId = Convert.ToInt32(data.StnId),
                    ProductId = Convert.ToInt32(data.ProductId),
                    Status = Convert.ToInt32(data.Status),
                    Price = Convert.ToDecimal(data.Price),
                    IsSerializedProduct= Convert.ToString(data.IsSerializedProduct)
                });
            }
            
            return sTNChasisProductSerialDetailViewModel;
        }
    }


}
