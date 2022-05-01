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
    public class PaymentModeBL
    {
        DBInterface dbInterface;
        public PaymentModeBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditPaymentMode(PaymentModeViewModel paymentModeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                PaymentMode paymentModeId = new PaymentMode
                {
                    PaymentModeId = paymentModeViewModel.PaymentModeId,
                    PaymentModeName = paymentModeViewModel.PaymentModeName,
                    Status = paymentModeViewModel.PaymentMode_Status,
                };
                responseOut = dbInterface.AddEditPaymentMode(paymentModeId);
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

        public List<PaymentModeViewModel> GetPaymentModeList(string paymentModeName = "", string Status = "")
        {
            List<PaymentModeViewModel> paymentModeList = new List<PaymentModeViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtpaymentModes = sqlDbInterface.GetPaymentModeList(paymentModeName, Status);
                if (dtpaymentModes != null && dtpaymentModes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtpaymentModes.Rows)
                    {
                        paymentModeList.Add(new PaymentModeViewModel
                        {
                            PaymentModeId = Convert.ToInt32(dr["PaymentModeId"]),
                            PaymentModeName = Convert.ToString(dr["PaymentModeName"]),
                            PaymentMode_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return paymentModeList;
        }
   
        public PaymentModeViewModel GetPaymentModeDetail(int paymentModeId = 0)
        {
            PaymentModeViewModel paymentMode = new PaymentModeViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPaymentModes = sqlDbInterface.GetPaymentModeDetail(paymentModeId);
                if (dtPaymentModes != null && dtPaymentModes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPaymentModes.Rows)
                    {
                        paymentMode = new PaymentModeViewModel
                        {
                            PaymentModeId = Convert.ToInt32(dr["PaymentModeId"]),
                            PaymentModeName = Convert.ToString(dr["PaymentModeName"]),
                            PaymentMode_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return paymentMode;
        }


        public List<PaymentModeViewModel> GetPaymentModeList()
        {
            List<PaymentModeViewModel> paymentmodeList = new List<PaymentModeViewModel>();
            try
            {
                List<Portal.DAL.PaymentMode> paymentmodes = dbInterface.GetPaymentModeList();
                if (paymentmodes != null && paymentmodes.Count > 0)
                {
                    foreach (Portal.DAL.PaymentMode paymentmode in paymentmodes)
                    {
                        paymentmodeList.Add(new PaymentModeViewModel { PaymentModeId = paymentmode.PaymentModeId, PaymentModeName = paymentmode.PaymentModeName });
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return paymentmodeList;
        }


    }
}
