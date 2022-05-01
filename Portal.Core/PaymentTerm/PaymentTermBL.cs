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

namespace Portal.Core
{
    public class PaymentTermBL
    {
        DBInterface dbInterface;
        public PaymentTermBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditPaymentTerm(PaymentTermViewModel paymenttermViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                PaymentTerm paymentterm = new PaymentTerm
                {
                   PaymentTermId = paymenttermViewModel.PaymentTermId,
                    PaymentTermDesc = paymenttermViewModel.PaymentTermDesc,
                    CompanyId = paymenttermViewModel.CompanyId,
                    Status = paymenttermViewModel.PaymentTerm_Status,

                };
                responseOut = dbInterface.AddEditPaymentTerm(paymentterm);
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


        public List<PaymentTermViewModel> GetPaymentTermList(string paymenttermDesc = "", string Status = "", int companyId = 0)
        {
            List<PaymentTermViewModel> paymentterm = new List<PaymentTermViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPaymentTerms = sqlDbInterface.GetPaymentTermList(paymenttermDesc, Status, companyId);
                if (dtPaymentTerms != null && dtPaymentTerms.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPaymentTerms.Rows)
                    {
                        paymentterm.Add(new PaymentTermViewModel
                        {
                            PaymentTermId = Convert.ToInt32(dr["PaymentTermId"]),
                            PaymentTermDesc = Convert.ToString(dr["PaymentTermDesc"]),
                            PaymentTerm_Status = Convert.ToBoolean(dr["Status"]),

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return paymentterm;
        }

        public PaymentTermViewModel GetPaymentTermDetail(int paymenttermId = 0)
        {
            PaymentTermViewModel paymentterm = new PaymentTermViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPaymentTerms = sqlDbInterface.GetPaymentTermDetail(paymenttermId);
                if (dtPaymentTerms != null && dtPaymentTerms.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPaymentTerms.Rows)
                    {
                        paymentterm = new PaymentTermViewModel
                        {
                            PaymentTermId = Convert.ToInt32(dr["PaymentTermId"]),
                            PaymentTermDesc = Convert.ToString(dr["PaymentTermDesc"]),
                            PaymentTerm_Status = Convert.ToBoolean(dr["Status"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return paymentterm;
        }


    }
}








