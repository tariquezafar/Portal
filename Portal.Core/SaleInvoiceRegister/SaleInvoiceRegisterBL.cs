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
    public class SaleInvoiceRegisterBL
    {
        DBInterface dbInterface;
        public SaleInvoiceRegisterBL()
        {
            dbInterface = new DBInterface();
        } 
  
        public List<SaleInvoiceViewModel> GetSaleInvoiceRegisterList(int customerId, int stateId, int shippingstateId, string fromDate, string toDate, int companyId, int createdBy, string sortBy, string sortOrder,int companyBranchId)
        {
            List<SaleInvoiceViewModel> saleinvoices = new List<SaleInvoiceViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSaleInvoices = sqlDbInterface.GetSaleInvoiceRegisterList(customerId, stateId, shippingstateId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, createdBy, sortBy, sortOrder, companyBranchId);
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
                            SODate = Convert.ToString(dr["SODate"]),
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            ConsigneeName = Convert.ToString(dr["ConsigneeName"]),
                            ConsigneeCode = Convert.ToString(dr["ConsigneeCode"]),
                            ContactPerson  = Convert.ToString(dr["ContactPerson"]),
                            BillingAddress = Convert.ToString(dr["BillingAddress"]),
                            ShippingBillingAddress = Convert.ToString(dr["ShippingBillingAddress"]),
                            ShippingCity = Convert.ToString(dr["ShippingCity"]),
                            ShippingStateName = Convert.ToString(dr["ShippingStateName"]),
                            ShippingPinCode = Convert.ToString(dr["ShippingPinCode"]),
                            ShippingTINNo = Convert.ToString(dr["ShippingTINNo"]),
                            PinCode = Convert.ToString(dr["PinCode"]),
                            CSTNo = Convert.ToString(dr["CSTNo"]),
                            TINNo = Convert.ToString(dr["TINNo"]),  
                            PayToBookBranch = Convert.ToString(dr["PayToBookBranch"]),
                            PayToBookName = Convert.ToString(dr["PayToBookName"]),
                            TotalQuantity = Convert.ToDecimal(dr["TotalQuantity"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            FreightValue = Convert.ToDecimal(dr["FreightValue"].ToString() == "" ? "0" : dr["FreightValue"].ToString()),
                            LoadingValue = Convert.ToDecimal(dr["LoadingValue"].ToString() == "" ? "0" : dr["LoadingValue"].ToString()),
                            InsuranceValue = Convert.ToDecimal(dr["InsuranceValue"]),
                            FreightCGST_Amt = Convert.ToDecimal(dr["FreightCGST_Amt"]),
                            FreightSGST_Amt = Convert.ToDecimal(dr["FreightSGST_Amt"]),
                            FreightIGST_Amt = Convert.ToDecimal(dr["FreightIGST_Amt"]),
                            LoadingCGST_Amt = Convert.ToDecimal(dr["LoadingCGST_Amt"]),
                            LoadingSGST_Amt = Convert.ToDecimal(dr["LoadingSGST_Amt"]),
                            LoadingIGST_Amt = Convert.ToDecimal(dr["LoadingIGST_Amt"]),
                            InsuranceCGST_Amt = Convert.ToDecimal(dr["InsuranceCGST_Amt"]),
                            InsuranceSGST_Amt = Convert.ToDecimal(dr["InsuranceSGST_Amt"]),
                            InsuranceIGST_Amt = Convert.ToDecimal(dr["InsuranceIGST_Amt"]),
                            CGST_Amount = Convert.ToDecimal(dr["CGSTAmount"]),
                            SGST_Amount = Convert.ToDecimal(dr["SGSTAmount"]),
                            IGST_Amount = Convert.ToDecimal(dr["IGSTAmount"]),
                            ReverseChargeAmount = Convert.ToDecimal(dr["ReverseChargeAmount"]),
                            City = Convert.ToString(dr["City"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),                          
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchName=Convert.ToString(dr["CompanyBranchName"])
                            
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
        public List<SaleSummaryRegisterViewModel> GetSaleSummaryRegister(int customerId, int userId, int stateId, int companyId, DateTime fromDate, DateTime toDate, string InvoiceNo,int companyBranchId)
        {
            List<SaleSummaryRegisterViewModel> saleInvoices = new List<SaleSummaryRegisterViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSaleInvoices = sqlDbInterface.GetSaleSummaryRegister(customerId,userId, stateId,companyId,fromDate,toDate,InvoiceNo, companyBranchId);
                if (dtSaleInvoices != null && dtSaleInvoices.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSaleInvoices.Rows)
                    {
                        saleInvoices.Add(new SaleSummaryRegisterViewModel
                        {
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            InvoiceType = Convert.ToString(dr["InvoiceType"]),
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            StateId = Convert.ToInt32(dr["StateId"]),
                            BasicAmt=Convert.ToDecimal(dr["BasicAmt"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            FreightValue = Convert.ToDecimal(dr["FreightValue"]),
                            LoadingValue = Convert.ToDecimal(dr["LoadingValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            AmountPaid = Convert.ToDecimal(dr["AmountPaid"]),
                            AmountPending = Convert.ToDecimal(dr["AmountPending"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CompanyBranchName=Convert.ToString(dr["CompanyBranchName"])


                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return saleInvoices;
        }

        public DataTable GenerateSaleInvoiceRegisterReports(int customerId, int stateId, int shippingstateId, string fromDate, string toDate,  int companyId, int createdBy, string sortBy, string sortOrder,int companyBranchId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtVoucher = new DataTable();
            try
            {
                dtVoucher = sqlDbInterface.GenerateSaleInvoiceRegisterReports(customerId, stateId, shippingstateId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, createdBy, sortBy, sortOrder, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtVoucher;
        }

        public DataTable GenerateSaleInvoiceSummaryReports(int customerId, int userId, int stateId, int companyId, DateTime fromDate, DateTime toDate,int companyBranchId,string invoiceNo="")
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtVoucher = new DataTable();
            try
            {
                dtVoucher = sqlDbInterface.GenerateSaleInvoiceSummaryReports(customerId, userId, stateId, companyId, fromDate, toDate,invoiceNo, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtVoucher;
        }

        public List<SaleSummaryRegisterViewModel> GetSaleUnpaidInvoiceSummaryRegister(int customerId, int userId, int stateId, int companyId, DateTime fromDate, DateTime toDate, string InvoiceNo, int companyBranchId)
        {
            List<SaleSummaryRegisterViewModel> saleInvoices = new List<SaleSummaryRegisterViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSaleInvoices = sqlDbInterface.GetSaleUnpaidInvoiceSummaryRegister(customerId, userId, stateId, companyId, fromDate, toDate, InvoiceNo, companyBranchId);
                if (dtSaleInvoices != null && dtSaleInvoices.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSaleInvoices.Rows)
                    {
                        saleInvoices.Add(new SaleSummaryRegisterViewModel
                        {
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            InvoiceType = Convert.ToString(dr["InvoiceType"]),
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            StateId = Convert.ToInt32(dr["StateId"]),
                            BasicAmt = Convert.ToDecimal(dr["BasicAmt"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            FreightValue = Convert.ToDecimal(dr["FreightValue"]),
                            LoadingValue = Convert.ToDecimal(dr["LoadingValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            AmountPaid = Convert.ToDecimal(dr["AmountPaid"]),
                            AmountPending = Convert.ToDecimal(dr["AmountPending"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"])


                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return saleInvoices;
        }
    }
}
