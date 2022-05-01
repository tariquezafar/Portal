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
    public class ProfitLossStatementPrintBL
    {
        DBInterface dbInterface;
        public ProfitLossStatementPrintBL()
        {
            dbInterface = new DBInterface();
        }
        #region Profit Loss Statement
        public ResponseOut GenerateProfitLossStatement(int companyId, int finYearId, DateTime fromDate,DateTime toDate, int reportUserId, string sessionId,int companyBranchId)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                responseOut = sqlDbInterface.GenerateProfitLossStatement(companyId, finYearId, fromDate,toDate, reportUserId, sessionId, companyBranchId);
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
        public DataTable GetProfitLossStatementDataTable(int reportUserId, string sessionId,int CompanyBranchId,string aSLIINEX)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtPL = new DataTable();
            try
            {
                dtPL = sqlDbInterface.GetProfitLossStatementDetail(reportUserId,sessionId, CompanyBranchId, aSLIINEX);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtPL;
        }
        #endregion
        #region Balance Sheet
        public ResponseOut GenerateBalanceSheet(int companyId, int finYearId, DateTime asOnDate, int reportUserId, string sessionId,int companyBranchId)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                responseOut = sqlDbInterface.GenerateBalanceSheet(companyId, finYearId, asOnDate, reportUserId, sessionId, companyBranchId);
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
        public DataTable GetBalanceSheetDataTable(int reportUserId, string sessionId,int CompanyBranchId, string aSLIINEX)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtPL = new DataTable();
            try
            {
                dtPL = sqlDbInterface.GetBalanceSheetDetail(reportUserId, sessionId, CompanyBranchId, aSLIINEX);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtPL;
        }
        public decimal GetNetProfitLoss(int companyId, int finYearId, DateTime fromDate,DateTime toDate, int reportUserId, string sessionId,int CompanyBranchId)
        {
            decimal netProfitLoss = 0;
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                netProfitLoss = sqlDbInterface.GetNetProfitLoss(companyId, finYearId, fromDate,toDate, reportUserId, sessionId, CompanyBranchId);
            }

            catch (Exception ex)
            {
                netProfitLoss = 0;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }

            return netProfitLoss;
        }

        #endregion
    }
}








