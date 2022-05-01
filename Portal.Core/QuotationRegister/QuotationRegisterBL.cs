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
    public class QuotationRegisterBL
    {
        DBInterface dbInterface;
        public QuotationRegisterBL()
        {
            dbInterface = new DBInterface();
        } 
        public List<QuotationViewModel> GetQuotationRegisterList(int customerId, int stateId, string fromDate, string toDate, int companyId, int createdBy, string sortBy, string sortOrder, int companyBranchId)
        {
            List<QuotationViewModel> quotations = new List<QuotationViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetQuotationRegisterList(customerId, stateId, Convert.ToDateTime(fromDate),Convert.ToDateTime(toDate), companyId, createdBy, sortBy, sortOrder, companyBranchId);
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        quotations.Add(new QuotationViewModel
                        {
                            QuotationId = Convert.ToInt32(dr["QuotationId"]),
                            QuotationNo = Convert.ToString(dr["QuotationNo"]),
                            QuotationDate = Convert.ToString(dr["QuotationDate"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            ContactPersonName = Convert.ToString(dr["ContactPersonName"]),
                            BillingAddress = Convert.ToString(dr["BillingAddress"]),
                            City = Convert.ToString(dr["City"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            PinCode = Convert.ToString(dr["PinCode"]),
                            TINNo = Convert.ToString(dr["TINNo"]),
                            QuotationRevisedStatus = Convert.ToBoolean( string.IsNullOrEmpty(dr["QuotationRevisedStatus"].ToString())? "false" : dr["QuotationRevisedStatus"].ToString()), 
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]), 
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchName= Convert.ToString(dr["CompanyBranchName"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return quotations;
        }

        public DataTable GenerateQuotationRegisterReports(int customerId, int stateId, string fromDate, string toDate, int companyId, int createdBy, string sortBy, string sortOrder,int companyBranchId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtVoucher = new DataTable();
            try
            {
                dtVoucher = sqlDbInterface.GenerateQuotationRegisterReports(customerId, stateId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, createdBy, sortBy, sortOrder, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtVoucher;
        }

    }
}
