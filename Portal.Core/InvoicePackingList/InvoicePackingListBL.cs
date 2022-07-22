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
    public class InvoicePackingListBL
    {
        DBInterface dbInterface;
        public InvoicePackingListBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditInvoicePackingList(InvoicePackingListViewModel invoicePackingListViewModel, List<InvoicePackingListProductDetailViewModel> invoicePackingListProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                InvoicePackingList invoicePackingList = new InvoicePackingList
                {
                    InvoicePackingListId = invoicePackingListViewModel.InvoicePackingListId,
                    InvoicePackingListDate = Convert.ToDateTime(invoicePackingListViewModel.InvoicePackingListDate),
                    InvoiceID = invoicePackingListViewModel.InvoiceId,
                    InvoiceNo = invoicePackingListViewModel.InvoiceNo,
                    PackingListTypeID = invoicePackingListViewModel.PackingListTypeID,
                    Remarks = invoicePackingListViewModel.Remarks,
                    InvoicePackingListStatus = invoicePackingListViewModel.InvoicePackingListStatus,
                    FinYearId = invoicePackingListViewModel.FinYearId,
                    CompanyId = invoicePackingListViewModel.CompanyId,
                    CompanyBranchId= invoicePackingListViewModel.CompanyBranchId,
                    CreatedBy = invoicePackingListViewModel.CreatedBy
                };
                List<InvoicePackingListProductDetail> invoicePackingListProductList = new List<InvoicePackingListProductDetail>();
                if (invoicePackingListProducts != null && invoicePackingListProducts.Count > 0)
                {
                    foreach (InvoicePackingListProductDetailViewModel item in invoicePackingListProducts)
                    {
                        invoicePackingListProductList.Add(new InvoicePackingListProductDetail
                        {
                            ProductId = item.ProductId,
                            ProductShortDesc = item.ProductShortDesc,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            TotalPrice = item.TotalPrice,
                            PackingProductType=item.PackingProductType,
                            IsWarrantyProduct=item.IsWarrantyProduct,
                            WarrantyInMonth=Convert.ToInt32(item.WarrantyInMonth)
                        });
                    }
                }
                responseOut = sqlDbInterface.AddEditInvoicePackingList(invoicePackingList, invoicePackingListProductList);
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

        public List<InvoicePackingListViewModel> GetInvoicePackingList(string invoicePackingListNo, string invoiceNo, Int32 packingListType, string fromDate, string toDate, string approvalStatus, int companyId,string CreatedByUserName,int companyBranchId, int CustomerId=0)
        {
            List<InvoicePackingListViewModel> packingLists = new List<InvoicePackingListViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPackingLists = sqlDbInterface.GetInvoicePackingLists(invoicePackingListNo, invoiceNo, packingListType, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), approvalStatus, companyId, CreatedByUserName, companyBranchId,CustomerId);
                if (dtPackingLists != null && dtPackingLists.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPackingLists.Rows)
                    {
                        packingLists.Add(new InvoicePackingListViewModel
                        {
                            InvoicePackingListId = Convert.ToInt32(dr["InvoicePackingListId"]),
                            InvoicePackingListNo = Convert.ToString(dr["InvoicePackingListNo"]),
                            InvoicePackingListDate = Convert.ToString(dr["InvoicePackingListDate"]),
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            PackingListTypeName = Convert.ToString(dr["PackingListTypeName"]),
                            InvoicePackingListStatus = Convert.ToString(dr["InvoicePackingListStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName= Convert.ToString(dr["CompanyBranchName"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return packingLists;
        }
        public InvoicePackingListViewModel GetInvoicePackingListDetail(long invoicePackingListId = 0)
        {
            InvoicePackingListViewModel packingList = new InvoicePackingListViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPackingList = sqlDbInterface.GetInvoicePackingListDetail(invoicePackingListId);
                if (dtPackingList != null && dtPackingList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPackingList.Rows)
                    {
                        packingList = new InvoicePackingListViewModel
                        {
                            InvoicePackingListId = Convert.ToInt32(dr["InvoicePackingListId"]),
                            InvoicePackingListNo = Convert.ToString(dr["InvoicePackingListNo"]),
                            InvoicePackingListDate = Convert.ToString(dr["InvoicePackingListDate"]),
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            PackingListTypeID = Convert.ToInt32(dr["PackingListTypeID"]),
                            InvoicePackingListStatus = Convert.ToString(dr["InvoicePackingListStatus"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
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
            return packingList;
        }
        public List<InvoicePackingListProductDetailViewModel> GetInvoicePackingListProductList(long invoicePackingListId)
        {
            List<InvoicePackingListProductDetailViewModel> packingListProducts = new List<InvoicePackingListProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPackingListProducts= sqlDbInterface.GetInvoicePackingListProductList(invoicePackingListId);
                if (dtPackingListProducts != null && dtPackingListProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPackingListProducts.Rows)
                    {
                        packingListProducts.Add(new InvoicePackingListProductDetailViewModel
                        {
                            InvoicePackingListProductDetailId = Convert.ToInt32(dr["InvoicePackingListProductDetailId"]),
                            SequenceNo=Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Price = Convert.ToDecimal(dr["Price"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            TotalPrice = Convert.ToDecimal(dr["TotalPrice"]),
                            PackingProductType = Convert.ToString(dr["PackingProductType"]),
                            IsWarrantyProduct = Convert.ToString(dr["IsWarrantyProduct"]),
                            WarrantyInMonth = Convert.ToString(dr["WarrantyInMonth"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return packingListProducts;
        }
        public List<InvoicePackingListProductDetailViewModel> GetPackingListTypeProductList(long invoiceId, long packingListTypeId)
        {
            List<InvoicePackingListProductDetailViewModel> packingListProducts = new List<InvoicePackingListProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPackingListProducts = sqlDbInterface.GetPackingListTypeProductList(invoiceId, packingListTypeId);
                if (dtPackingListProducts != null && dtPackingListProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPackingListProducts.Rows)
                    {
                        packingListProducts.Add(new InvoicePackingListProductDetailViewModel
                        {
                            InvoicePackingListProductDetailId = 0,
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Price = 0,
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            TotalPrice = 0,
                            PackingProductType = Convert.ToString(dr["PackingProductType"]),
                            IsWarrantyProduct = Convert.ToString(dr["IsWarrantyProduct"]),
                            WarrantyInMonth = Convert.ToString(dr["WarrantyInMonth"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return packingListProducts;
        }
        public DataTable GetInvoicePackingListDetailDataTable(long invoicePackingListId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetInvoicePackingListDetail(invoicePackingListId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }

        public DataTable GetInvoicePackingListProductListDataTable(long invoicePackingListId)

        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtFinishedGoodProcess = new DataTable();
            try
            {
                dtFinishedGoodProcess = sqlDbInterface.GetInvoicePackingListProductList(invoicePackingListId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtFinishedGoodProcess;
        }


        public List<SaleInvoiceViewModel> GetPLSaleInvoiceList(string saleinvoiceNo, string customerName, string refNo, string fromDate, string toDate, int companyId, string invoiceType = "", string displayType = "", string approvalStatus = "", string customerCode = "", int companyBranchId=0)
        {
            List<SaleInvoiceViewModel> saleinvoices = new List<SaleInvoiceViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSaleInvoices = sqlDbInterface.GetPLSaleInvoiceList(saleinvoiceNo, customerName, refNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, invoiceType, displayType, approvalStatus, customerCode, companyBranchId);
                if (dtSaleInvoices != null && dtSaleInvoices.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSaleInvoices.Rows)
                    {
                        saleinvoices.Add(new SaleInvoiceViewModel
                        {
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            SONo = Convert.ToString(dr["SONo"]),
                            InvoiceType = Convert.ToString(dr["InvoiceType"]),
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            ConsigneeId = Convert.ToInt32(dr["ConsigneeId"]),
                            ConsigneeName = Convert.ToString(dr["ConsigneeName"]),
                            ConsigneeCode = Convert.ToString(dr["ConsigneeCode"]),
                            CompanyBranchName = Convert.ToString(dr["BranchName"]),
                            City = Convert.ToString(dr["City"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
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
            return saleinvoices;
        }


        public DataTable GetChasisSerialNoList(long invoiceId,int packingListTypeid)

        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtChasisNo = new DataTable();
            try
            {
                dtChasisNo = sqlDbInterface.GetChasisSerialNoList(invoiceId, packingListTypeid);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtChasisNo;
        }

        public DataTable GetPackingListProduct(long invoicepackingListId)

        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtPackingList = new DataTable();
            try
            {
                dtPackingList = sqlDbInterface.GetPackingListProduct(invoicepackingListId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtPackingList;
        }
        public DataTable GetPackingListName(long invoicepackingListId)

        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtPackingList = new DataTable();
            try
            {
                dtPackingList = sqlDbInterface.GetPackingListName(invoicepackingListId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtPackingList;
        }
    }
}
