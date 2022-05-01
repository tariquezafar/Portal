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
    public class SaleReturnRegisterBL
    {
        DBInterface dbInterface;
        public SaleReturnRegisterBL()
        {
            dbInterface = new DBInterface();
        }


        public List<SaleReturnViewModel> GetSaleReturnRegisterList(string saleReturnNo, string customerName, string dispatchrefNo, string fromDate, string toDate, string approvalStatus, int companyId,string sortBy,string sortOrder,int companyBranchId)
        {
            List<SaleReturnViewModel> saleReturnViewModel = new List<SaleReturnViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDeliveryChallans = sqlDbInterface.GetSaleReturnRegisterList(saleReturnNo, customerName, dispatchrefNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), approvalStatus, companyId,sortBy,sortOrder, companyBranchId);
                if (dtDeliveryChallans != null && dtDeliveryChallans.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDeliveryChallans.Rows)
                    {
                        saleReturnViewModel.Add(new SaleReturnViewModel
                        {
                            SaleReturnId = Convert.ToInt32(dr["SaleReturnId"]),
                            SaleReturnNo = Convert.ToString(dr["SaleReturnNo"]),
                            SaleReturnDate = Convert.ToString(dr["SaleReturnDate"]),
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            BillingAddress = Convert.ToString(dr["BillingAddress"]),
                            City = Convert.ToString(dr["City"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            ReturnType = Convert.ToString(dr["ReturnType"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
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
            return saleReturnViewModel;
        }   
        public DataTable GenerateSaleReturnRegisterReports(string saleReturnNo, string customerName, string dispatchrefNo, string fromDate, string toDate, string approvalStatus, int companyId, string sortBy, string sortOrder,int companyBranchId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtVoucher = new DataTable();
            try
            {
                dtVoucher = sqlDbInterface.GenerateSaleReturnRegisterReports(saleReturnNo, customerName, dispatchrefNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), approvalStatus, companyId, sortBy, sortOrder, companyBranchId);
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
