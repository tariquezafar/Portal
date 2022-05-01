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
    public class CustomerPaymentBL
    {
        DBInterface dbInterface;
        public CustomerPaymentBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditCustomerPayment(CustomerPaymentViewModel customerpaymentViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                CustomerPayment customerpayment = new CustomerPayment
                {
                    PaymentTrnId = customerpaymentViewModel.PaymentTrnId,
                    PaymentDate = Convert.ToDateTime(customerpaymentViewModel.PaymentDate), 
                    CustomerId = customerpaymentViewModel.CustomerId,
                    InvoiceId = customerpaymentViewModel.InvoiceId,
                    BookId = customerpaymentViewModel.BookId,
                    PaymentModeId = customerpaymentViewModel.PaymentModeId, 
                    RefNo = customerpaymentViewModel.RefNo,
                    RefDate = string.IsNullOrEmpty(customerpaymentViewModel.RefDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(customerpaymentViewModel.RefDate),
                    Amount = customerpaymentViewModel.Amount, 
                    ValueDate = string.IsNullOrEmpty(customerpaymentViewModel.ValueDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(customerpaymentViewModel.ValueDate),
                    Remarks = customerpaymentViewModel.Remarks, 
                    FinYearId = customerpaymentViewModel.FinYearId,
                    CompanyId = customerpaymentViewModel.CompanyId,
                    Status = customerpaymentViewModel.CustomerPayment_Status,
                    CreatedBy = customerpaymentViewModel.CreatedBy 

                }; 
                responseOut = sqldbinterface.AddEditCustomerPayment(customerpayment); 

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

        public List<CustomerPaymentViewModel> GetCustomerPaymentList(string paymentNo, string customerName, string invoiceNo, string refNo, string fromDate, string toDate, int companyId)
        {
            List<CustomerPaymentViewModel> customerpayments = new List<CustomerPaymentViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomerPayments = sqlDbInterface.GetCustomerPaymentList(paymentNo, customerName, invoiceNo, refNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);
                if (dtCustomerPayments != null && dtCustomerPayments.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomerPayments.Rows)
                    {
                        customerpayments.Add(new CustomerPaymentViewModel
                        {
                            PaymentTrnId = Convert.ToInt32(dr["PaymentTrnId"]),
                            PaymentNo = Convert.ToString(dr["PaymentNo"]),
                            PaymentDate = Convert.ToString(dr["PaymentDate"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]), 
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
            return customerpayments;
        }
        public CustomerPaymentViewModel GetCustomerPaymentDetail(long paymenttrnId = 0)
        {
            CustomerPaymentViewModel customerpayment = new CustomerPaymentViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetCustomerPaymentDetail(paymenttrnId);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        customerpayment = new CustomerPaymentViewModel
                        {
                            PaymentTrnId = Convert.ToInt32(dr["PaymentTrnId"]),
                            PaymentNo = Convert.ToString(dr["PaymentNo"]),
                            PaymentDate = Convert.ToString(dr["PaymentDate"]),
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),

                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            BookId = Convert.ToInt32(dr["BookId"].ToString() == "" ? "0" : dr["BookId"]),
                            PaymentModeId = Convert.ToInt32(dr["PaymentModeId"].ToString() == "" ? "0" : dr["PaymentModeId"]),
                            PaymentModeName = Convert.ToString(dr["PaymentModeName"]),  

                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),

                            Amount = Convert.ToDecimal(dr["Amount"]),
                            ValueDate = Convert.ToString(dr["ValueDate"]), 
                            
                            Remarks = Convert.ToString(dr["Remarks"]),
                            CustomerPayment_Status = Convert.ToBoolean(dr["Status"]),
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
            return customerpayment;
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
