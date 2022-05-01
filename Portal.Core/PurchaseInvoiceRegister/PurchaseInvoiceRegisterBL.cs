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
    public class PurchaseInvoiceRegisterBL
    {
        DBInterface dbInterface;
        public PurchaseInvoiceRegisterBL()
        {
            dbInterface = new DBInterface();
        }
        public List<PurchaseInvoiceViewModel> GetPurchaseInvoiceRegisterList(string vendorId, int stateId, string fromDate, string toDate, int companyId, int createdBy, string sortBy, string sortOrder,string companyBranch)
        {
           
            List<PurchaseInvoiceViewModel> purchaseInvoiceViewModel = new List<PurchaseInvoiceViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPOs = sqlDbInterface.GetPurchaseInvoiceRegisterList(vendorId, stateId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, createdBy, sortBy, sortOrder, companyBranch);
                if (dtPOs != null && dtPOs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPOs.Rows)
                    {
                        purchaseInvoiceViewModel.Add(new PurchaseInvoiceViewModel
                        {
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo=Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            POId = Convert.ToInt32(dr["POId"]),
                            PONo = Convert.ToString(dr["PONo"]),
                            PODate = Convert.ToString(dr["PODate"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            City = Convert.ToString(dr["City"]),
                            BillingAddress = Convert.ToString(dr["BillingAddress"]),                            
                            StateName = Convert.ToString(dr["StateName"]),
                            PinCode = Convert.ToString(dr["PinCode"]),
                            CSTNo = Convert.ToString(dr["CSTNo"]),
                            TINNo = Convert.ToString(dr["TINNo"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            CurrencyCode = Convert.ToString(dr["CurrencyCode"]),
                            ConsigneeName = Convert.ToString(dr["ConsigneeName"]),
                            ConsigneeCode = Convert.ToString(dr["ConsigneeCode"]),
                            ShippingAddress = Convert.ToString(dr["ShippingAddress"]),
                            ShippingCity = Convert.ToString(dr["ShippingCity"]),
                            ShippingPinCode = Convert.ToString(dr["ShippingPinCode"]),
                            ConsigneeGSTNo = Convert.ToString(dr["ConsigneeGSTNo"]),
                            ShippingStateName = Convert.ToString(dr["ConsigneeStateName"]),
                            ShippingCountryName = Convert.ToString(dr["ConsigneeCountryName"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            FreightValue = Convert.ToDecimal(dr["FreightValue"]),
                            LoadingValue = Convert.ToDecimal(dr["LoadingValue"]),
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
                            TotalQuantity = Convert.ToDecimal(dr["TotalQuantity"]),
                            FinYearId =Convert.ToInt32(dr["FinYearId"]),                        
                            Remarks = Convert.ToString(dr["Remarks"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            companyBranch = Convert.ToString(dr["BranchName"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return purchaseInvoiceViewModel;
        }


        public List<PurchaseSummaryRegisterViewModel> GetPurchaseSummaryRegister(int vendorId, int userId, int stateId, int companyId, DateTime fromDate, DateTime toDate,string companyBranch)
        {
            List<PurchaseSummaryRegisterViewModel> purchaseInvoices = new List<PurchaseSummaryRegisterViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPurchaseInvoices = sqlDbInterface.GetPurchaseSummaryRegister(vendorId, userId, stateId, companyId, fromDate, toDate, companyBranch);
                if (dtPurchaseInvoices != null && dtPurchaseInvoices.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPurchaseInvoices.Rows)
                    {
                        purchaseInvoices.Add(new PurchaseSummaryRegisterViewModel
                        {
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]), 
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            StateId = Convert.ToInt32(dr["StateId"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]), 
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            AmountPaid = Convert.ToDecimal(dr["AmountPaid"]),
                            AmountPending = Convert.ToDecimal(dr["AmountPending"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            companyBranch = Convert.ToString(dr["BranchName"]),
                            


                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return purchaseInvoices;
        }

        public DataTable GeneratePurchaseSummaryReports(int vendorId, int userId, int stateId, int companyId, DateTime fromDate, DateTime toDate,string companyBranch)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtPSummaryRegister = new DataTable();
            try
            {
                dtPSummaryRegister = sqlDbInterface.GetPurchaseSummaryRegister(vendorId, userId, stateId, companyId, fromDate, toDate, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtPSummaryRegister;
        }

        public DataTable GeneratePIRegisterReports(string vendorId, int stateId, string fromDate, string toDate, int companyId, int createdBy, string sortBy, string sortOrder,string companyBranch)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtPIRegister = new DataTable();
            try
            {
                dtPIRegister = sqlDbInterface.GetPurchaseInvoiceRegisterList(vendorId, stateId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, createdBy, sortBy, sortOrder, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtPIRegister;
        }





    }
}
