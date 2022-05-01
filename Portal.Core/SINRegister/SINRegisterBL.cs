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
  public class SINRegisterBL
    {
        DBInterface dbInterface;
        public SINRegisterBL()
        {
            dbInterface = new DBInterface();
        } 
            public List<SINViewModel> GetSINRegisterList(string sinNo,string requisitionNo, string jobNo,int companyBranchId, int fromLocation, int toLocation, string fromDate, string toDate, int companyId, string employee, string sortBy, string sortOrder)
            {
                List<SINViewModel> sins = new List<SINViewModel>();
                SQLDbInterface sqlDbInterface = new SQLDbInterface();
                try
                {
                    DataTable dtSTNs = sqlDbInterface.GetSINRegisterList(sinNo,requisitionNo, jobNo,companyBranchId, fromLocation, toLocation, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, employee, sortBy, sortOrder);

                    
                    if (dtSTNs != null && dtSTNs.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtSTNs.Rows)
                        {

                            sins.Add(new SINViewModel
                            {
                                SINId = Convert.ToInt32(dr["SINId"]),
                                SINNo = Convert.ToString(dr["SINNo"]),
                                RequisitionNo = Convert.ToString(dr["RequisitionNo"]),
                                EmployeeName =Convert.ToString(dr["EmployeeName"]),
                                FromLocationName = Convert.ToString(dr["FromLocationName"]),
                                ToLocationName = Convert.ToString(dr["ToLocationName"]),
                                Remarks1 = Convert.ToString(dr["Remarks1"]),
                                Remarks2 = Convert.ToString(dr["Remarks2"]),
                                SINDate = Convert.ToString(dr["SINDate"]),
                          
                                JobNo = Convert.ToString(dr["JobNo"]),                           
                                CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                                ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                                BranchName= Convert.ToString(dr["BranchName"])

                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                    throw ex;
                }
                return sins;
            }
      
     

            public DataTable GetSINRegisterReport(string sinNo, string requisitionNo, string jobNo, int companyBranchId, int fromLocation, int toLocation, string fromDate, string toDate, int companyId, string employee, string sortBy, string sortOrder)
            {
                SQLDbInterface sqlDbInterface = new SQLDbInterface();
                DataTable dtSINReport = new DataTable();
                try
                {
                    dtSINReport = sqlDbInterface.GetSINRegisterList(sinNo, requisitionNo, jobNo, companyBranchId, fromLocation, toLocation, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, employee, sortBy, sortOrder);
                }
                catch (Exception ex)
                {
                    Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                    throw ex;
                }
                return dtSINReport;
            }
    }
}
