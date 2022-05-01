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
  public class VendorPaymentRegisterBL
    {
        DBInterface dbInterface;
        public VendorPaymentRegisterBL()
        {
            dbInterface = new DBInterface();

        }
        public List<VendorPaymentViewModel> GetVendorPaymentRegisterList(int vendorId, int paymentModeId, string sortBy, string sortOrder, string fromDate, string toDate, int companyId)
        {
            List<VendorPaymentViewModel> cformregister = new List<VendorPaymentViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtVendorPaymentRegisters = sqlDbInterface.GetVendorPaymentRegisterList(vendorId, paymentModeId, sortBy, sortOrder, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);
                if (dtVendorPaymentRegisters != null && dtVendorPaymentRegisters.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtVendorPaymentRegisters.Rows)
                    {
                        cformregister.Add(new VendorPaymentViewModel
                        {
                            PaymentTrnId = Convert.ToInt32(dr["PaymentTrnId"]),
                            PaymentNo = Convert.ToString(dr["PaymentNo"]),
                            InvoiceId= Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            PaymentModeId=Convert.ToInt32(dr["PaymentModeId"]),
                            PaymentModeName=Convert.ToString(dr["PaymentModeName"]),
                            PaymentDate = Convert.ToString(dr["PaymentDate"]),
                            BookId = Convert.ToInt32(dr["BookId"]),
                            BookCode = Convert.ToString(dr["BookCode"]),
                            BookType = Convert.ToString(dr["BookType"]),
                            BookName = Convert.ToString(dr["BookName"]), 
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
