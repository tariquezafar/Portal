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
    public class PORegisterBL
    {
        DBInterface dbInterface;
        public PORegisterBL()
        {
            dbInterface = new DBInterface();
        }
        public List<POViewModel> GetPORegisterList(string vendorId, int stateId, string fromDate, string toDate, int companyId, int createdBy, string sortBy, string sortOrder,string companyBranch)
        {
            List<POViewModel> pos = new List<POViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPOs = sqlDbInterface.GetPORegisterList(vendorId, stateId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, createdBy, sortBy, sortOrder, companyBranch);
                if (dtPOs != null && dtPOs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPOs.Rows)
                    {
                        pos.Add(new POViewModel
                        {
                            POId = Convert.ToInt32(dr["POId"]),
                            PONo = Convert.ToString(dr["PONo"]),
                            IndentNo = Convert.ToString(dr["IndentNo"]),
                            QuotationNo = Convert.ToString(dr["QuotationNo"]),
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
                            CGST_Amount=Convert.ToDecimal(dr["CGSTAmount"]),
                            SGST_Amount = Convert.ToDecimal(dr["SGSTAmount"]),
                            IGST_Amount = Convert.ToDecimal(dr["IGSTAmount"]),
                            TotalQuantity= Convert.ToDecimal(dr["TotalQuantity"]),
                            ReverseChargeAmount = Convert.ToDecimal(dr["ReverseChargeAmount"]),
                            Remarks1 = Convert.ToString(dr["Remarks1"]),
                            Remarks2 = Convert.ToString(dr["Remarks2"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            ApprovedByName = Convert.ToString(dr["ApprovedByName"]),
                            ApprovedDate = Convert.ToString(dr["ApprovedDate"]),
                            RejectionStatus = Convert.ToString(dr["RejectionStatus"]),
                            RejectedByUserName = Convert.ToString(dr["RejectedByName"]),
                            RejectedDate = Convert.ToString(dr["RejectedDate"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranch = Convert.ToString(dr["BranchName"]),
                            
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

        public DataTable GeneratePORegisterReports(string vendorId, int stateId, string fromDate, string toDate, int companyId, int createdBy, string sortBy, string sortOrder,string companyBranch)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtPORegister = new DataTable();
            try
            {
                dtPORegister = sqlDbInterface.GetPORegisterList(vendorId, stateId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, createdBy, sortBy, sortOrder, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtPORegister;
        }
    }
}
