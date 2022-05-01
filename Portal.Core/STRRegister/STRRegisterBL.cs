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
  public class STRRegisterBL
    {
        DBInterface dbInterface;
        public STRRegisterBL()
        {
            dbInterface = new DBInterface();
        } 
        public List<STRViewModel> GetSTRRegisterList(string strNo, string glNo, int fromLocation, int toLocation, string fromDate, string toDate, int companyId, int createdBy, string sortBy, string sortOrder)
        {
            List<STRViewModel> strs = new List<STRViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSTRs = sqlDbInterface.GetSTRRegisterList(strNo, glNo, fromLocation, toLocation, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, createdBy, sortBy, sortOrder); 
                    
                if (dtSTRs != null && dtSTRs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSTRs.Rows)
                    { 
                        strs.Add(new STRViewModel
                        {
                            STRId = Convert.ToInt32(dr["STRId"]),
                            STRNo = Convert.ToString(dr["STRNo"]),
                            FromWarehouseId = Convert.ToInt32(dr["FromWarehouseId"]),
                            FormPrimaryAddress = Convert.ToString(dr["FormPrimaryAddress"]),
                            FormLocationName = Convert.ToString(dr["FormLocation"]),
                            FormCity = Convert.ToString(dr["FormCity"]),
                            FormPinCode = Convert.ToString(dr["FormPinCode"]),
                            FormCSTNo = Convert.ToString(dr["FormCSTNo"]),
                            FormTINNo = Convert.ToString(dr["FormTINNo"]),
                            ToWarehouseId = Convert.ToInt32(dr["ToWarehouseId"]), 
                            ToLocationName = Convert.ToString(dr["ToLocation"]),
                            ToPrimaryAddress = Convert.ToString(dr["ToPrimaryAddress"]),
                            ToCity = Convert.ToString(dr["ToCity"]),
                            ToPinCode = Convert.ToString(dr["ToPinCode"]),
                            ToCSTNo = Convert.ToString(dr["ToCSTNo"]),
                            ToTINNo = Convert.ToString(dr["ToTINNo"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            FreightValue = Convert.ToDecimal(dr["FreightValue"]),
                            LoadingValue = Convert.ToDecimal(dr["LoadingValue"]),
                            Remarks1 = Convert.ToString(dr["Remarks1"]),
                            Remarks2 = Convert.ToString(dr["Remarks2"]),
                            STRDate = Convert.ToString(dr["STRDate"]),
                            GRNo = Convert.ToString(dr["GRNo"]), 
                            DispatchRefNo = Convert.ToString(dr["DispatchRefNo"]),
                            DispatchRefDate = Convert.ToString(dr["DispatchRefDate"]),
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
            return strs;
        }

        
    }
}
