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
    public class SubLedgerWOFYPrintBL
    {
        DBInterface dbInterface;
        public SubLedgerWOFYPrintBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut GenerateSubLedger(int slTypeId, int glId,long slId, int companyId, DateTime fromDate, DateTime toDate, int reportUserId, string sessionId,int companyBranchId)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                responseOut = sqlDbInterface.GenerateSubLedgerWithoutFinalcialYear(slTypeId, glId, slId, companyId,  fromDate,  toDate, reportUserId, sessionId, companyBranchId);
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
        public decimal GetSubLedgerBalance(int glId, long slId, int companyId, int finYearId)
        {
            decimal slBalance = 0;
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                slBalance = sqlDbInterface.GetSubLedgerBalance(glId, slId, companyId, finYearId);
            }

            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }

            return slBalance;
        }

        public DataTable GetSubLedgerDetailDataTable(int reportUserId, string sessionId,int companyBranchId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtSubLedger = new DataTable();
            try
            {
                dtSubLedger = sqlDbInterface.GetSubLedgerWithoutFinancialYearDetail(reportUserId,sessionId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtSubLedger;
        }
        
    }
}








