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
    public class GeneralLedgerWOFYPrintBL
    {
        DBInterface dbInterface;
        public GeneralLedgerWOFYPrintBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut GenerateGeneralLedger(int glId, int companyId, DateTime fromDate, DateTime toDate, int reportUserId, string sessionId,int companyBranchId)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                responseOut = sqlDbInterface.GenerateGeneralLedgerWithoutFinalcialYear(glId, companyId,  fromDate,  toDate, reportUserId, sessionId, companyBranchId);
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

        public DataTable GetGeneralLedgerDetailDataTable(int reportUserId, string sessionId,int companyBranchId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtGeneralLedger = new DataTable();
            try
            {
                dtGeneralLedger = sqlDbInterface.GetGeneralLedgerWithoutFinancialYearDetail(reportUserId,sessionId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtGeneralLedger;
        }
        public List<GLViewModel> GetGLAutoCompleteList(string searchTerm, int companyId)
        {
            List<GLViewModel> gls = new List<GLViewModel>();
            try
            {
                List<GL> glList = dbInterface.GetAllGLAutoCompleteList(searchTerm, companyId);
                if (glList != null && glList.Count > 0)
                {
                    foreach (GL gl in glList)
                    {
                        gls.Add(new GLViewModel { GLId = gl.GLId, GLHead = gl.GLHead, GLCode = gl.GLCode, SLTypeId = Convert.ToInt16(gl.SLTypeId) });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return gls;
        }

        public decimal GetGeneralLedgerBalance(int glId, int companyId, int finYearId)
        {
            decimal glBalance = 0;
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                glBalance = sqlDbInterface.GetGeneralLedgerBalance(glId, companyId,finYearId);
            }

            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }

            return glBalance;
        }


    }
}








