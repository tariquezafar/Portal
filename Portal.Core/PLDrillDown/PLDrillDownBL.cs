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
    public class PLDrillDownBL
    {
        DBInterface dbInterface;
        public PLDrillDownBL()
        {
            dbInterface = new DBInterface();
        }
        #region Profit And Loss Drill Down
        public ResponseOut GenerateTrialBalanceDrillDown(int companyId, int finYearId, DateTime fromDate, DateTime toDate, int reportUserId, string sessionId,int companyBranchId)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                responseOut = sqlDbInterface.GeneratePLDrillDown(companyId, finYearId, fromDate,toDate, reportUserId, sessionId, companyBranchId);
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
        public DataTable GetTrialBalanceDrillDownDataTable(int reportUserId, string sessionId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtTrialBalance = new DataTable();
            try
            {
                dtTrialBalance = sqlDbInterface.GetTrialBalanceDrillDownDetail(reportUserId,sessionId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtTrialBalance;
        }

        public List<TBDrillDown_GLTypeViewModel> GetTBDrillDown_GLTypeList(int reportUserId, string sessionId, int companyBranchId = 0, int glMainGroupId = 0,int glSubGroupId=0,int glId=0)
        {
            List<TBDrillDown_GLTypeViewModel> glTypeList = new List<TBDrillDown_GLTypeViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtGLTypeList = sqlDbInterface.GetPLDrillDown_GLTypeList(reportUserId, sessionId, companyBranchId,glMainGroupId, glSubGroupId,glId);
                if (dtGLTypeList != null && dtGLTypeList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtGLTypeList.Rows)
                    {
                        glTypeList.Add(new TBDrillDown_GLTypeViewModel
                        {
                            GLTYPE = Convert.ToString(dr["GLTYPE"]),
                            GLTYPE_ORDER = Convert.ToInt16(dr["GLTYPE_ORDER"]),
                            CLOSING = Convert.ToDecimal(dr["CLOSING"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return glTypeList;
        }
        public List<TBDrillDown_MainGroupViewModel> GetTBDrillDown_MainGroupList(int reportUserId, string sessionId, int companyBranchId = 0, int glMainGroupId = 0,int glSubGroupId=0, int glId = 0)
        {
            List<TBDrillDown_MainGroupViewModel> mainGroupList = new List<TBDrillDown_MainGroupViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtMainGroupList = sqlDbInterface.GetPLDrillDown_MainGroupList(reportUserId, sessionId, companyBranchId, glMainGroupId, glSubGroupId,glId);
                if (dtMainGroupList != null && dtMainGroupList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtMainGroupList.Rows)
                    {
                        mainGroupList.Add(new TBDrillDown_MainGroupViewModel
                        {
                            GLTYPE = Convert.ToString(dr["GLTYPE"]),
                            GLMainGroupId= Convert.ToInt32(dr["GLMainGroupId"]),
                            GLMainGroupName = Convert.ToString(dr["GLMainGroupName"]),
                            CompanyBranchId=Convert.ToInt32(dr["CompanyBranchId"]),
                            CLOSING = Convert.ToDecimal(dr["CLOSING"])
                           
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return mainGroupList;
        }
        public List<TBDrillDown_SubGroupViewModel> GetTBDrillDown_SubGroupList(int reportUserId, string sessionId,int companyBranchId, int glMainGroupId = 0, int glSubGroupId = 0,int glId=0)
        {
            List<TBDrillDown_SubGroupViewModel> subGroupList = new List<TBDrillDown_SubGroupViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSubGroupList = sqlDbInterface.GetPLDrillDown_SubGroupList(reportUserId, sessionId, companyBranchId, glMainGroupId,glSubGroupId,glId);
                if (dtSubGroupList != null && dtSubGroupList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSubGroupList.Rows)
                    {
                        subGroupList.Add(new TBDrillDown_SubGroupViewModel
                        {
                            GLMainGroupId = Convert.ToInt32(dr["GLMainGroupId"]),
                            GLSubGroupId = Convert.ToInt32(dr["GLSubGroupId"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            ScheduleNo = Convert.ToInt16(dr["ScheduleNo"]),
                            GLSubGroupName = Convert.ToString(dr["GLSubGroupName"]),                            
                            CLOSING = Convert.ToDecimal(dr["CLOSING"]),
                           
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return subGroupList;
        }
        public List<TBDrillDown_GLWiseViewModel> GetTBDrillDown_GLWiseList(int reportUserId, string sessionId,int companyBranchId, int glMainGroupId = 0, int glSubGroupId = 0, int glId = 0)
        {
            List<TBDrillDown_GLWiseViewModel> glWiseList = new List<TBDrillDown_GLWiseViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtGLWiseList = sqlDbInterface.GetPLDrillDown_GLWiseList(reportUserId, sessionId, companyBranchId, glMainGroupId, glSubGroupId, glId);
                if (dtGLWiseList != null && dtGLWiseList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtGLWiseList.Rows)
                    {
                        glWiseList.Add(new TBDrillDown_GLWiseViewModel
                        {
                            GLMainGroupId = Convert.ToInt32(dr["GLMainGroupId"]),
                            GLSubGroupId = Convert.ToInt32(dr["GLSubGroupId"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            SLTypeId = Convert.ToInt16(dr["SLTypeId"]),                          
                            GLId = Convert.ToInt32(dr["GLId"]),
                            GLCode = Convert.ToString(dr["GLCode"]),
                            GLHead = Convert.ToString(dr["GLHead"]),                            
                            CLOSING = Convert.ToDecimal(dr["CLOSING"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return glWiseList;
        }
        public List<TBDrillDown_SLWiseViewModel> GetTBDrillDown_SLWiseList(int reportUserId, string sessionId,int companyBranchId, int glMainGroupId = 0, int glSubGroupId = 0, int glId = 0)
        {
            List<TBDrillDown_SLWiseViewModel> slWiseList = new List<TBDrillDown_SLWiseViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSLWiseList = sqlDbInterface.GetPLDrillDown_SLWiseList(reportUserId, sessionId, companyBranchId, glMainGroupId, glSubGroupId, glId);
                if (dtSLWiseList != null && dtSLWiseList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSLWiseList.Rows)
                    {
                        slWiseList.Add(new TBDrillDown_SLWiseViewModel
                        {
                            GLId = Convert.ToInt32(dr["GLId"]),
                            SLId = Convert.ToInt64(dr["SLId"]),
                            SLCode = Convert.ToString(dr["SLCode"]),
                            SLHead = Convert.ToString(dr["SLHead"]),
                            DEBIT = Convert.ToDecimal(dr["DEBIT"]),
                            CREDIT = Convert.ToDecimal(dr["CREDIT"]),
                            YEAROPENINGBALANCEDEBIT = Convert.ToDecimal(dr["YEAROPENINGBALANCEDEBIT"]),
                            YEAROPENINGBALANCECREDIT = Convert.ToDecimal(dr["YEAROPENINGBALANCECREDIT"]),
                            CLOSINGBALANCEDEBIT = Convert.ToDecimal(dr["CLOSINGBALANCEDEBIT"]),
                            CLOSINGBALANCECREDIT = Convert.ToDecimal(dr["CLOSINGBALANCECREDIT"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return slWiseList;
        }

        public ResponseOut GenerateGLLedgerDrillDown(int glId,int companyId, int finYearId, DateTime fromDate, DateTime toDate, int reportUserId, string sessionId)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                responseOut = sqlDbInterface.GenerateGLLedgerDrillDown(glId, companyId, finYearId, fromDate, toDate, reportUserId, sessionId);
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
        public List<GLDrillDown_GLOpeningViewModel> GetGLDrillDown_GLOpening(int reportUserId, string sessionId, int glId = 0)
        {
            List<GLDrillDown_GLOpeningViewModel> glWiseList = new List<GLDrillDown_GLOpeningViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtGLWiseList = sqlDbInterface.GetGLDrillDown_GLOpening(reportUserId, sessionId, glId);
                if (dtGLWiseList != null && dtGLWiseList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtGLWiseList.Rows)
                    {
                        glWiseList.Add(new GLDrillDown_GLOpeningViewModel
                        {
                            GLId = Convert.ToInt32(dr["GLId"]),
                            GLCode = Convert.ToString(dr["GLCode"]),
                            GLHead = Convert.ToString(dr["GLHead"]),
                            DBAmount= Convert.ToDecimal(dr["DBAmount"]),
                            CRAmount = Convert.ToDecimal(dr["CRAmount"]),
                            Balance = Convert.ToDecimal(dr["Balance"]),
                            BalanceDRCR = Convert.ToString(dr["BalanceDRCR"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return glWiseList;
        }
        public List<GLDrillDown_GLLedgerViewModel> GetGLDrillDown_GLLedger(int reportUserId, string sessionId, int glId = 0)
        {
            List<GLDrillDown_GLLedgerViewModel> glWiseList = new List<GLDrillDown_GLLedgerViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtGLWiseList = sqlDbInterface.GetGLDrillDown_GLLedger(reportUserId, sessionId, glId);
                if (dtGLWiseList != null && dtGLWiseList.Rows.Count > 0)
                {
                    decimal runningBalance = 0;
                    string runningBalanceDrCR = "";
                    foreach (DataRow dr in dtGLWiseList.Rows)
                    {
                        runningBalance = Convert.ToDecimal(dr["DBAmount"]) + (0 - Convert.ToDecimal(dr["CRAmount"])) + runningBalance;
                        runningBalanceDrCR = runningBalance < 0 ? "Cr" : "Dr";
                        glWiseList.Add(new GLDrillDown_GLLedgerViewModel
                        {
                            VoucherId= Convert.ToInt64(dr["VoucherId"]),
                            VoucherNo = Convert.ToString(dr["VoucherNo"]),
                            VoucherDate = Convert.ToString(dr["VoucherDate"]),
                            VoucherType = Convert.ToString(dr["VoucherType"]),
                            Narration = Convert.ToString(dr["Narration"]),
                            BillNo = Convert.ToString(dr["BillNo"]),
                            BillDate = Convert.ToString(dr["BillDate"])== "01-Jan-1900"? "": Convert.ToString(dr["BillDate"]),
                            PayeeName = Convert.ToString(dr["PayeeName"]),
                            DBAmount = Convert.ToDecimal(dr["DBAmount"]),
                            CRAmount = Convert.ToDecimal(dr["CRAmount"]),
                            BookId = Convert.ToInt32(dr["BookId"]),
                            VoucherBookType= Convert.ToString(dr["VoucherBookType"]),
                            VoucherViewPagePath = Convert.ToString(dr["VoucherViewPagePath"]),
                            GLId = Convert.ToInt32(dr["GLId"]),
                            GLCode = Convert.ToString(dr["GLCode"]),
                            GLHead = Convert.ToString(dr["GLHead"]),
                            RunningBalance=Math.Abs(runningBalance),
                            RunningBalanceDrCr=runningBalanceDrCR
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return glWiseList;
        }

        public ResponseOut GenerateSubLedgerDrillDown(int glId,Int32 slId, int companyId, int finYearId, DateTime fromDate, DateTime toDate, int reportUserId, string sessionId)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                responseOut = sqlDbInterface.GenerateSubLedgerDrillDownPL(glId,slId, companyId, finYearId, fromDate, toDate, reportUserId, sessionId);
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
        public List<SLDrillDown_SLOpeningViewModel> GetSLDrillDown_SLOpening(int reportUserId, string sessionId, int glId = 0,int slId=0)
        {
            List<SLDrillDown_SLOpeningViewModel> slWiseList = new List<SLDrillDown_SLOpeningViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtGLWiseList = sqlDbInterface.GetSLDrillDown_SLOpening(reportUserId, sessionId, glId,slId);
                if (dtGLWiseList != null && dtGLWiseList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtGLWiseList.Rows)
                    {
                        slWiseList.Add(new SLDrillDown_SLOpeningViewModel
                        {
                            GLId = Convert.ToInt32(dr["GLId"]),
                            GLCode = Convert.ToString(dr["GLCode"]),
                            GLHead = Convert.ToString(dr["GLHead"]),
                            SLId = Convert.ToInt32(dr["SLId"]),
                            SLCode = Convert.ToString(dr["SLCode"]),
                            SLHead = Convert.ToString(dr["SLHead"]),
                            DBAmount = Convert.ToDecimal(dr["DBAmount"]),
                            CRAmount = Convert.ToDecimal(dr["CRAmount"]),
                            Balance = Convert.ToDecimal(dr["Balance"]),
                            BalanceDRCR = Convert.ToString(dr["BalanceDRCR"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return slWiseList;
        }
        public List<SLDrillDown_SLLedgerViewModel> GetSLDrillDown_SLLedger(int reportUserId, string sessionId, int glId = 0,int slId=0)
        {
            List<SLDrillDown_SLLedgerViewModel> slWiseList = new List<SLDrillDown_SLLedgerViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtGLWiseList = sqlDbInterface.GetSLDrillDown_SLLedger(reportUserId, sessionId, glId,slId);
                if (dtGLWiseList != null && dtGLWiseList.Rows.Count > 0)
                {
                    decimal runningBalance = 0;
                    string runningBalanceDrCR = "";
                    foreach (DataRow dr in dtGLWiseList.Rows)
                    {
                        runningBalance = Convert.ToDecimal(dr["DBAmount"]) + (0 - Convert.ToDecimal(dr["CRAmount"])) + runningBalance;
                        runningBalanceDrCR = runningBalance < 0 ? "Cr" : "Dr";
                        slWiseList.Add(new SLDrillDown_SLLedgerViewModel
                        {
                            VoucherId = Convert.ToInt64(dr["VoucherId"]),
                            VoucherNo = Convert.ToString(dr["VoucherNo"]),
                            VoucherDate = Convert.ToString(dr["VoucherDate"]),
                            VoucherType = Convert.ToString(dr["VoucherType"]),
                            Narration = Convert.ToString(dr["Narration"]),
                            BillNo = Convert.ToString(dr["BillNo"]),
                            BillDate = Convert.ToString(dr["BillDate"]) == "01-Jan-1900" ? "" : Convert.ToString(dr["BillDate"]),
                            PayeeName = Convert.ToString(dr["PayeeName"]),
                            ChequeRefNo = Convert.ToString(dr["ChequeRefNo"]),
                            ChequeRefDate = Convert.ToString(dr["ChequeRefDate"]) == "01-Jan-1900" ? "" : Convert.ToString(dr["ChequeRefDate"]),
                            ValueDate = Convert.ToString(dr["ValueDate"]) == "01-Jan-1900" ? "" : Convert.ToString(dr["ValueDate"]),
                            DBAmount = Convert.ToDecimal(dr["DBAmount"]),
                            CRAmount = Convert.ToDecimal(dr["CRAmount"]),
                            BookId = Convert.ToInt32(dr["BookId"]),
                            VoucherBookType = Convert.ToString(dr["VoucherBookType"]),
                            VoucherViewPagePath = Convert.ToString(dr["VoucherViewPagePath"]),
                            GLId = Convert.ToInt32(dr["GLId"]),
                            GLCode = Convert.ToString(dr["GLCode"]),
                            GLHead = Convert.ToString(dr["GLHead"]),
                            SLId = Convert.ToInt32(dr["SLId"]),
                            SLCode = Convert.ToString(dr["SLCode"]),
                            SLHead = Convert.ToString(dr["SLHead"]),
                            RunningBalance = Math.Abs(runningBalance),
                            RunningBalanceDrCr = runningBalanceDrCR
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return slWiseList;
        }

        #endregion
    }
}








