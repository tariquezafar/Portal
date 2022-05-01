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
    public class VendorPaymentBL
    {
        DBInterface dbInterface;
        public VendorPaymentBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditVendorPayment(VendorPaymentViewModel vendorpaymentViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                VendorPayment vendorpayment = new VendorPayment
                {
                    PaymentTrnId = vendorpaymentViewModel.PaymentTrnId,
                    PaymentDate = Convert.ToDateTime(vendorpaymentViewModel.PaymentDate), 
                    VendorId = vendorpaymentViewModel.VendorId,
                    InvoiceId = vendorpaymentViewModel.InvoiceId,
                    BookId = vendorpaymentViewModel.BookId,
                    PaymentModeId = vendorpaymentViewModel.PaymentModeId, 
                    RefNo = vendorpaymentViewModel.RefNo,
                    RefDate = string.IsNullOrEmpty(vendorpaymentViewModel.RefDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(vendorpaymentViewModel.RefDate),
                    Amount = vendorpaymentViewModel.Amount, 
                    ValueDate = string.IsNullOrEmpty(vendorpaymentViewModel.ValueDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(vendorpaymentViewModel.ValueDate),
                    Remarks = vendorpaymentViewModel.Remarks, 
                    FinYearId = vendorpaymentViewModel.FinYearId,
                    CompanyId = vendorpaymentViewModel.CompanyId,
                    Status = vendorpaymentViewModel.VendorPayment_Status,
                    CreatedBy = vendorpaymentViewModel.CreatedBy 

                }; 
                responseOut = sqldbinterface.AddEditVendorPayment(vendorpayment); 

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

        public List<VendorPaymentViewModel> GetVendorPaymentList(string paymentNo, string vendorName, string invoiceNo, string refNo, string fromDate, string toDate, int companyId)
        {
            List<VendorPaymentViewModel> vendorpayments = new List<VendorPaymentViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtVendorPayments = sqlDbInterface.GetVendorPaymentList(paymentNo, vendorName, invoiceNo, refNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);
                if (dtVendorPayments != null && dtVendorPayments.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtVendorPayments.Rows)
                    {
                        vendorpayments.Add(new VendorPaymentViewModel
                        {
                            PaymentTrnId = Convert.ToInt32(dr["PaymentTrnId"]),
                            PaymentNo = Convert.ToString(dr["PaymentNo"]),
                            PaymentDate = Convert.ToString(dr["PaymentDate"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
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
            return vendorpayments;
        }
        public VendorPaymentViewModel GetVendorPaymentDetail(long paymenttrnId = 0)
        {
            VendorPaymentViewModel vendorpayment = new VendorPaymentViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetVendorPaymentDetail(paymenttrnId);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        vendorpayment = new VendorPaymentViewModel
                        {
                            PaymentTrnId = Convert.ToInt32(dr["PaymentTrnId"]),
                            PaymentNo = Convert.ToString(dr["PaymentNo"]),
                            PaymentDate = Convert.ToString(dr["PaymentDate"]),
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),

                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            VendorName = Convert.ToString(dr["VendorName"]),

                            PaymentModeId = Convert.ToInt32(dr["PaymentModeId"].ToString() == "" ? "0" : dr["PaymentModeId"]),
                            PaymentModeName = Convert.ToString(dr["PaymentModeName"]),

                            BookId = Convert.ToInt32(dr["BookId"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),

                            Amount = Convert.ToDecimal(dr["Amount"]),
                            ValueDate = Convert.ToString(dr["ValueDate"]),

                            Remarks = Convert.ToString(dr["Remarks"]),
                            VendorPayment_Status = Convert.ToBoolean(dr["Status"]),
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
            return vendorpayment;
        }



        //public List<CustomerViewModel> GetCustomerAutoCompleteList(string searchTerm, int companyId)
        //{
        //    List<CustomerViewModel> customers = new List<CustomerViewModel>();
        //    try
        //    {
        //        List<Customer> customerList = dbInterface.GetCustomerAutoCompleteList(searchTerm, companyId);

        //        if (customerList != null && customerList.Count > 0)
        //        {
        //            foreach (Customer customer in customerList)
        //            {
        //                customers.Add(new CustomerViewModel { CustomerId = customer.CustomerId, CustomerName = customer.CustomerName, CustomerCode = customer.CustomerCode, PrimaryAddress = customer.PrimaryAddress });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
        //        throw ex;
        //    }
        //    return customers;
        //}


    }
}
