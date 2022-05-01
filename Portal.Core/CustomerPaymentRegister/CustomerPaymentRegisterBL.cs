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
  public class CustomerPaymentRegisterBL
    {
        DBInterface dbInterface;
        public CustomerPaymentRegisterBL()
        {
            dbInterface = new DBInterface();

        }
        public List<CustomerPaymentViewModel> GetCustomerPaymentRegisterList(int customerId, int paymentModeId, string sortBy, string sortOrder, string fromDate, string toDate, int companyId)
        {
            List<CustomerPaymentViewModel> cformregister = new List<CustomerPaymentViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomerPaymentRegisters = sqlDbInterface.GetCustomerPaymentRegisterList(customerId,paymentModeId, sortBy, sortOrder, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);
                if (dtCustomerPaymentRegisters != null && dtCustomerPaymentRegisters.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomerPaymentRegisters.Rows)
                    {
                        cformregister.Add(new CustomerPaymentViewModel
                        {
                            PaymentTrnId = Convert.ToInt32(dr["PaymentTrnId"]),
                            PaymentNo = Convert.ToString(dr["PaymentNo"]),
                            InvoiceId= Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            PaymentModeId=Convert.ToInt32(dr["PaymentModeId"]),
                            PaymentModeName=Convert.ToString(dr["PaymentModeName"]),
                            PaymentDate = Convert.ToString(dr["PaymentDate"]),
                            BookId = Convert.ToInt32(dr["BookId"]),
                            BookCode = Convert.ToString(dr["BookCode"]),
                            BookType = Convert.ToString(dr["BookType"]),
                            BookName = Convert.ToString(dr["BookName"]),
                            BankBranch=Convert.ToString(dr["BankBranch"]),
                            BankIFSCCode = Convert.ToString(dr["IFSC"]),
                            
                            Remarks = Convert.ToString(dr["Remarks"]),
                            Amount = Convert.ToInt32(dr["Amount"]),
                            
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            CreatedByUserName = string.IsNullOrEmpty(Convert.ToString(dr["CreatedByName"])) ? "" : Convert.ToString(dr["CreatedByName"]),
                            ModifiedByUserName = string.IsNullOrEmpty(Convert.ToString(dr["ModifiedByName"])) ? "" : Convert.ToString(dr["ModifiedByName"]),
                            CreatedDate = string.IsNullOrEmpty(Convert.ToString(dr["CreatedDate"])) ? "" : Convert.ToString(dr["CreatedDate"]),
                            ModifiedDate = string.IsNullOrEmpty(Convert.ToString(dr["ModifiedDate"])) ? "" : Convert.ToString(dr["ModifiedDate"])
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
    }
}
