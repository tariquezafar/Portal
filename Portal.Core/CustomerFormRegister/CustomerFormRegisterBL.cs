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
    public class CustomerFormRegisterBL
    {
        DBInterface dbInterface;
        public CustomerFormRegisterBL()
        {
            dbInterface = new DBInterface();
        } 
  
        public List<CustomerFormViewModel> GetCustomerFormRegisterList(string formStatus, int customerId, string invoiceNo, string refNo, string fromDate, string toDate, int companyId, int createdBy, string sortBy, string sortOrder)
        {
            List<CustomerFormViewModel> cformregister = new List<CustomerFormViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCFormRegisters = sqlDbInterface.GetCFormRegisterList(formStatus, customerId, invoiceNo, refNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate),companyId,createdBy, sortBy, sortOrder);
                if (dtCFormRegisters != null && dtCFormRegisters.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCFormRegisters.Rows)
                    {
                        cformregister.Add(new CustomerFormViewModel
                        {
                            CustomerFormTrnId=Convert.ToInt32(dr["CustomerFormTrnId"]),
                            CustomerId=Convert.ToInt32(dr["CustomerId"]),
                            CustomerName=Convert.ToString(dr["CustomerName"]),
                            CustomerCode=Convert.ToString(dr["CustomerCode"]),
                            FormStatus =Convert.ToString(dr["FormStatus"]),
                            FormTypeId=Convert.ToInt32(dr["FormTypeId"]),
                            FormTypeDesc=Convert.ToString(dr["FormTypeDesc"]),
                            Remarks =Convert.ToString(dr["Remarks"]),
                            Amount=Convert.ToInt32(dr["Amount"]),
                            CustomerForm_Status =Convert.ToBoolean(dr["Status"]),
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate =Convert.ToString(dr["InvoiceDate"]),
                            RefNo=Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            CreatedByUserName = string.IsNullOrEmpty(Convert.ToString(dr["CreatedByName"]))?"":Convert.ToString(dr["CreatedByName"]),
                           
                            ModifiedByUserName= string.IsNullOrEmpty(Convert.ToString(dr["ModifiedByName"]))?"": Convert.ToString(dr["ModifiedByName"]),
                            CreatedDate = string.IsNullOrEmpty(Convert.ToString(dr["CreatedDate"]))?"": Convert.ToString(dr["CreatedDate"]),
                            ModifiedDate = string.IsNullOrEmpty(Convert.ToString(dr["ModifiedDate"]))?"": Convert.ToString(dr["ModifiedDate"]),
                            
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return cformregister;
        }

        public DataTable GenerateCustomerFormRegisterReports(string formStatus, int customerId, string invoiceNo, string refNo, string fromDate, string toDate, int companyId, int createdBy, string sortBy, string sortOrder)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtVoucher = new DataTable();
            try
            {
                dtVoucher = sqlDbInterface.GenerateCustomerFormRegisterReports(formStatus, customerId, invoiceNo, refNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, createdBy, sortBy, sortOrder);
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
