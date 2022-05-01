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
  public class PurchaseVoucherBL
    {
        DBInterface dbInterface;
        public  PurchaseVoucherBL()
        {
            dbInterface = new DBInterface();
        }


        public List<VoucherDetailViewModel> GetPurchaseVoucherEntryList(long voucherId)
        {
            List<VoucherDetailViewModel> voucherEntries = new List<VoucherDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtVoucherEntry = sqlDbInterface.GetPurchaseVoucherEntryDetail(voucherId);
                if (dtVoucherEntry != null && dtVoucherEntry.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtVoucherEntry.Rows)
                    {

                        voucherEntries.Add(new VoucherDetailViewModel
                        {
                            VoucherDetailId = Convert.ToInt32(dr["VoucherDetailId"]),
                            VoucherId = Convert.ToInt32(dr["VoucherId"]),
                            SequenceNo = Convert.ToInt16(dr["SequenceNo"]),
                            EntryMode = Convert.ToString(dr["EntryMode"]),
                            GLId = Convert.ToInt32(dr["GLId"]),
                            GLCode = Convert.ToString(dr["GLCode"]),
                            GLHead = Convert.ToString(dr["GLHead"]),
                            SLTypeId = Convert.ToInt16(dr["SLTypeId"]),
                            SLTypeName = Convert.ToString(dr["SLTypeName"]),
                            SLId = Convert.ToInt32(dr["SLId"]),
                            SLCode = Convert.ToString(dr["SLCode"]),
                            SLHead = Convert.ToString(dr["SLHead"]),
                            Narration = Convert.ToString(dr["Narration"]),
                            PaymentModeId = Convert.ToInt32(dr["PaymentModeId"]),
                            PaymentModeName = Convert.ToString(dr["PaymentModeName"]),
                            ChequeRefNo = Convert.ToString(dr["ChequeRefNo"]),
                            ChequeRefDate = Convert.ToString(dr["ChequeRefDate"]),
                            Amount = Convert.ToDecimal(dr["Amount"]),
                            CostCenterId = Convert.ToInt32(dr["CostCenterId"]),
                            CostCenterName = Convert.ToString(dr["CostCenterName"]),
                            ValueDate = Convert.ToString(dr["ValueDate"]),
                            DrawnOnBank = Convert.ToString(dr["DrawnOnBank"]),
                            PO_SONo = Convert.ToString(dr["PO_SONo"]),
                            BillNo = Convert.ToString(dr["BillNo"]),
                            BillDate = Convert.ToString(dr["BillDate"]),
                            PayeeId = Convert.ToInt32(dr["PayeeId"]),
                            PayeeName = Convert.ToString(dr["PayeeName"]),
                            AutoEntry = Convert.ToBoolean(dr["AutoEntry"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return voucherEntries;
        }
       

    


   

         



    }
}
