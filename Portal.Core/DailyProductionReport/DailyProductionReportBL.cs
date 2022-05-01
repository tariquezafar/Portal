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
    public class DailyProductionReportBL
    {
        DBInterface dbInterface;
        public DailyProductionReportBL()
        {
            dbInterface = new DBInterface();
        }

        public DataTable GetDailyProductionSummaryList(string productName, string processType, string fromDate, string toDate, int companyId)
        {
            DataTable dtDailyProductionDetailed;
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                dtDailyProductionDetailed = sqlDbInterface.GetDailyProductionSummaryList(productName, processType, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtDailyProductionDetailed;
        }

        public DataTable GetDailyProductionDetailedList(string productName, string processType, string fromDate, string toDate, int companyId, string chassisno)
        {
            DataTable dtDailyProductionDetailed;
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                dtDailyProductionDetailed = sqlDbInterface.GetDailyProductionDetailedList(productName, processType, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId,chassisno);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtDailyProductionDetailed;
        }


    }
}
