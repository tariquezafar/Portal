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
    public class DeliveryChallanRegisterBL
    {
        DBInterface dbInterface;
        public DeliveryChallanRegisterBL()
        {
            dbInterface = new DBInterface();
        }
        
        public List<DeliveryChallanViewModel> GetDeliveryChallanRegisterList(int customerId, int stateId, int shippingstateId, string fromDate, string toDate, int createdBy, int companyId, string sortBy, string sortOrder,int companyBranchId,int DealerId=0)
        {            
            List<DeliveryChallanViewModel> deliveryChallanViewModel = new List<DeliveryChallanViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSOs = sqlDbInterface.GetDeliveryChallanRegisterList(customerId, stateId, shippingstateId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), createdBy, companyId, sortBy, sortOrder, companyBranchId,DealerId);
                if (dtSOs != null && dtSOs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSOs.Rows)
                    {
                        deliveryChallanViewModel.Add(new DeliveryChallanViewModel
                        {
                            ChallanId = Convert.ToInt32(dr["ChallanId"]),
                            ChallanNo = Convert.ToString(dr["ChallanNo"]),
                            ChallanDate = Convert.ToString(dr["ChallanDate"]),                           
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            ContactPerson = Convert.ToString(dr["ContactPerson"]),
                            StateName= Convert.ToString(dr["StateName"]),
                            ShippingBillingAddress = Convert.ToString(dr["ShippingBillingAddress"]),
                            ShippingCity = Convert.ToString(dr["ShippingCity"]),
                            ShippingStateName = Convert.ToString(dr["ShippingStateName"]),
                            ShippingPinCode = Convert.ToString(dr["ShippingPinCode"]),
                            ShippingTINNo = Convert.ToString(dr["ShippingTINNo"]),
                            ShippingEmail = Convert.ToString(dr["ShippingEmail"]),
                            ShippingMobileNo = Convert.ToString(dr["ShippingMobileNo"]),
                            ShippingFax = Convert.ToString(dr["ShippingFax"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            FinYearId = Convert.ToInt32(dr["FinYearId"]),
                            NoOfPackets = Convert.ToInt32(dr["NoOfPackets"]),
                            LRNo = Convert.ToString(dr["LRNo"]),
                            DispatchRefNo = Convert.ToString(dr["DispatchRefNo"]),
                           // CompanyBranchName = Convert.ToString(dr["BranchName"]),                           
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            FreightValue = Convert.ToDecimal(dr["FreightValue"]),
                            LoadingValue = Convert.ToDecimal(dr["LoadingValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]), 
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return deliveryChallanViewModel;
        }

        public DataTable GenerateDeliveryChallanRegisterReports(int customerId, int stateId, int shippingstateId, string fromDate, string toDate,  int createdBy, int companyId, string sortBy, string sortOrder,int companyBranchId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtVoucher = new DataTable();
            try
            {
                dtVoucher = sqlDbInterface.GenerateDeliveryChallanRegisterReports(customerId, stateId, shippingstateId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), createdBy, companyId, sortBy, sortOrder, companyBranchId);
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
