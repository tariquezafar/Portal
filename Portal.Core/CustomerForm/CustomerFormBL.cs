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
    public class CustomerFormBL
    {
        DBInterface dbInterface;
        public CustomerFormBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditCustomerForm(CustomerFormViewModel customerFormViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                CustomerForm customerForm = new CustomerForm
                {
                    CustomerFormTrnId = customerFormViewModel.CustomerFormTrnId,                  
                    CustomerId = customerFormViewModel.CustomerId,
                    InvoiceId = customerFormViewModel.InvoiceId,
                    FormTypeId = customerFormViewModel.FormTypeId,
                    FormStatus = customerFormViewModel.FormStatus, 
                    RefNo = customerFormViewModel.RefNo,
                    RefDate = string.IsNullOrEmpty(customerFormViewModel.RefDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(customerFormViewModel.RefDate),
                    Amount = customerFormViewModel.Amount,                     
                    Remarks = customerFormViewModel.Remarks,                    
                    CompanyId = customerFormViewModel.CompanyId,
                    Status = customerFormViewModel.CustomerForm_Status,
                    CreatedBy = customerFormViewModel.CreatedBy 

                }; 
                responseOut = dbInterface.AddEditCustomerForm(customerForm); 

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
       
        public ResponseOut RemoveCustomerProduct(long mappingId)
        {

            ResponseOut responseOut = new ResponseOut();
            try
            {
                responseOut = dbInterface.RemoveCustomerProduct(mappingId);
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

        public List<CustomerFormViewModel> GetCustomerFormList(string formStatus, string customerName, string invoiceNo, string refNo, string fromDate, string toDate, int companyId)
        {        
            List<CustomerFormViewModel> customerFormViewModel = new List<CustomerFormViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomerPayments = sqlDbInterface.GetCustomerFormList(formStatus, customerName, invoiceNo, refNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);
                if (dtCustomerPayments != null && dtCustomerPayments.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomerPayments.Rows)
                    {
                        customerFormViewModel.Add(new CustomerFormViewModel
                        {
                            CustomerFormTrnId = Convert.ToInt32(dr["CustomerFormTrnId"]),                          
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            FormTypeId = Convert.ToInt32(dr["FormTypeId"]),
                            FormStatus = Convert.ToString(dr["FormStatus"]),
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            Amount = Convert.ToDecimal(dr["Amount"]),
                            CustomerForm_Status = Convert.ToBoolean(dr["Status"]), 
                            RefDate = Convert.ToString(dr["RefDate"]), 
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"].ToString() == "" ? "0" : dr["CreatedDate"].ToString()), 
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"].ToString() == "" ? "0" : dr["ModifiedDate"].ToString()), 
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return customerFormViewModel;
        }
        public CustomerFormViewModel GetCustomerFormDetail(long customerFormTrnId = 0)
        {         
            CustomerFormViewModel customerFormViewModel = new CustomerFormViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetCustomerFormDetail(customerFormTrnId);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        customerFormViewModel = new CustomerFormViewModel
                        {
                            CustomerFormTrnId = Convert.ToInt32(dr["CustomerFormTrnId"]),
                            FormTypeId = Convert.ToInt32(dr["FormTypeId"]),
                            FormStatus = Convert.ToString(dr["FormStatus"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            CustomerId=Convert.ToInt32(dr["CustomerId"]),
                            InvoiceId=Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            Remarks=Convert.ToString(dr["Remarks"]),
                            Amount=Convert.ToDecimal(dr["Amount"]),
                            CustomerForm_Status=Convert.ToBoolean(dr["Status"]), 
                            RefDate = Convert.ToString(dr["RefDate"]),
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
            return customerFormViewModel;
        }

        public List<CustomerViewModel> GetCustomerAutoCompleteList(string searchTerm, int companyId)
        {
            List<CustomerViewModel> customers = new List<CustomerViewModel>();
            try
            {
                List<Customer> customerList = dbInterface.GetCustomerAutoCompleteList(searchTerm, companyId);

                if (customerList != null && customerList.Count > 0)
                {
                    foreach (Customer customer in customerList)
                    {
                        customers.Add(new CustomerViewModel { CustomerId = customer.CustomerId, CustomerName = customer.CustomerName, CustomerCode = customer.CustomerCode, PrimaryAddress = customer.PrimaryAddress });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return customers;
        }


    }
}
