using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.IO;
namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class BalanceSheetDrillDownController : BaseController
    {
        
        #region Trial Balance Sheet Drill Down
        [ValidateRequest(true, UserInterfaceHelper.BalanceSheetDrillDown, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult BalanceSheetDrillDown()
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.BalanceSheetDrillDown, (int)AccessMode.ViewAccess, (int)RequestMode.Ajax)]
        public ActionResult BalanceSheetDrillDownGenerate(string fromDate, string toDate, int companyBranchId)
        {
            ResponseOut responseOut = new ResponseOut();
            BalanceSheetDrillDownBL balanceSheetDrillDownBL = new BalanceSheetDrillDownBL();
            try
            {
                    
                    int finYearId= Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = balanceSheetDrillDownBL.GenerateTrialBalanceDrillDown(ContextUser.CompanyId, finYearId,Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate),ContextUser.UserId,Session.SessionID, companyBranchId);
            }
            catch (Exception ex)
            {
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }

        [ValidateRequest(true, UserInterfaceHelper.BalanceSheetDrillDown, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult MainGroupWiseBSDrillDown(string reportLevel, string fromDate,string toDate,int gLMainGroupId=0,int companyBranchId=0)
        {
           
            BalanceSheetDrillDownBL balanceSheetDrillDownBL = new BalanceSheetDrillDownBL();
            TBDrillDownViewModel tbDrillDownViewModel = new TBDrillDownViewModel();
            try
            {
                ViewData["CompanyBranchId"] = companyBranchId;
                tbDrillDownViewModel.GLTypeList = balanceSheetDrillDownBL.GetTBDrillDown_GLTypeList(ContextUser.UserId, Session.SessionID, companyBranchId);
                tbDrillDownViewModel.MainGroupList= balanceSheetDrillDownBL.GetTBDrillDown_MainGroupList(ContextUser.UserId, Session.SessionID, companyBranchId);
                ViewData["reportLevel"] = reportLevel;
                ViewData["toDate"] = toDate;
                ViewData["fromDate"] = fromDate;
                ViewData["gLMainGroupId"] = gLMainGroupId;

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View(tbDrillDownViewModel);
        }

        [ValidateRequest(true, UserInterfaceHelper.BalanceSheetDrillDown, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult SubGroupWiseBSDrillDown(string reportLevel, string fromDate,string toDate,string startInterface="",int gLMainGroupId=0, int gLSubGroupId = 0,int companyBranchId =0)
        {
            BalanceSheetDrillDownBL balanceSheetDrillDownBL = new BalanceSheetDrillDownBL();
            TBDrillDownViewModel tbDrillDownViewModel = new TBDrillDownViewModel();
            try
            {
                ViewData["CompanyBranchId"] = companyBranchId;
                tbDrillDownViewModel.GLTypeList = balanceSheetDrillDownBL.GetTBDrillDown_GLTypeList(ContextUser.UserId, Session.SessionID, companyBranchId, gLMainGroupId);
                tbDrillDownViewModel.MainGroupList = balanceSheetDrillDownBL.GetTBDrillDown_MainGroupList(ContextUser.UserId, Session.SessionID, companyBranchId, gLMainGroupId);
                tbDrillDownViewModel.SubGroupList = balanceSheetDrillDownBL.GetTBDrillDown_SubGroupList(ContextUser.UserId, Session.SessionID, companyBranchId, gLMainGroupId);

                ViewData["reportLevel"] = reportLevel;
                ViewData["fromDate"] = fromDate;
                ViewData["toDate"] = toDate;
                ViewData["startInterface"] = startInterface==""?"SubGroup":startInterface;
                ViewData["gLMainGroupId"] = gLMainGroupId;
                ViewData["gLSubGroupId"] = gLSubGroupId;


            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View(tbDrillDownViewModel);
        }

        [ValidateRequest(true, UserInterfaceHelper.TBDrillDown, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult GLWiseBSDrillDown(string reportLevel,string fromDate, string toDate, string startInterface = "", int gLMainGroupId = 0, int gLSubGroupId = 0, int glId = 0,int companyBranchId = 0)
        {
            BalanceSheetDrillDownBL balanceSheetDrillDownBL = new BalanceSheetDrillDownBL();
            TBDrillDownViewModel tbDrillDownViewModel = new TBDrillDownViewModel();
            try
            {
                ViewData["CompanyBranchId"] = companyBranchId;
                tbDrillDownViewModel.GLTypeList = balanceSheetDrillDownBL.GetTBDrillDown_GLTypeList(ContextUser.UserId, Session.SessionID, companyBranchId, gLMainGroupId,gLSubGroupId);
                tbDrillDownViewModel.MainGroupList = balanceSheetDrillDownBL.GetTBDrillDown_MainGroupList(ContextUser.UserId, Session.SessionID, companyBranchId, gLMainGroupId,gLSubGroupId);
                tbDrillDownViewModel.SubGroupList = balanceSheetDrillDownBL.GetTBDrillDown_SubGroupList(ContextUser.UserId, Session.SessionID, companyBranchId, gLMainGroupId, gLSubGroupId);
                tbDrillDownViewModel.GLWiseList= balanceSheetDrillDownBL.GetTBDrillDown_GLWiseList(ContextUser.UserId, Session.SessionID, companyBranchId, gLMainGroupId, gLSubGroupId,glId);
                ViewData["reportLevel"] = reportLevel;              
                ViewData["toDate"] = toDate;
                ViewData["fromDate"] = fromDate;
                ViewData["startInterface"] = startInterface == "" ? "GLWise" : startInterface;
                ViewData["gLMainGroupId"] = gLMainGroupId;
                ViewData["gLSubGroupId"] = gLSubGroupId;
                ViewData["glId"] = glId;


            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View(tbDrillDownViewModel);
        }


        [ValidateRequest(true, UserInterfaceHelper.TBDrillDown, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult SLWiseBSDrillDown(string reportLevel, string fromDate, string toDate, string startInterface = "", int gLMainGroupId = 0, int gLSubGroupId = 0, int glId = 0,Int32 slId=0,int companyBranchId = 0)
        {
            BalanceSheetDrillDownBL balanceSheetDrillDownBL = new BalanceSheetDrillDownBL();
            TBDrillDownViewModel tbDrillDownViewModel = new TBDrillDownViewModel();
            try
            {
                ViewData["CompanyBranchId"] = companyBranchId;
                tbDrillDownViewModel.GLTypeList = balanceSheetDrillDownBL.GetTBDrillDown_GLTypeList(ContextUser.UserId, Session.SessionID, companyBranchId, gLMainGroupId, gLSubGroupId,glId);
                tbDrillDownViewModel.MainGroupList = balanceSheetDrillDownBL.GetTBDrillDown_MainGroupList(ContextUser.UserId, Session.SessionID, companyBranchId, gLMainGroupId, gLSubGroupId,glId);
                tbDrillDownViewModel.SubGroupList = balanceSheetDrillDownBL.GetTBDrillDown_SubGroupList(ContextUser.UserId, Session.SessionID, companyBranchId, gLMainGroupId, gLSubGroupId,glId);
                tbDrillDownViewModel.GLWiseList = balanceSheetDrillDownBL.GetTBDrillDown_GLWiseList(ContextUser.UserId, Session.SessionID, companyBranchId, gLMainGroupId, gLSubGroupId, glId);
                tbDrillDownViewModel.SLWiseList = balanceSheetDrillDownBL.GetTBDrillDown_SLWiseList(ContextUser.UserId, Session.SessionID, companyBranchId, gLMainGroupId, gLSubGroupId, glId);
                ViewData["reportLevel"] = reportLevel;
                ViewData["fromDate"] = fromDate;
                ViewData["toDate"] = toDate;
                ViewData["startInterface"] = startInterface == "" ? "SLWise" : startInterface;
                ViewData["gLMainGroupId"] = gLMainGroupId;
                ViewData["gLSubGroupId"] = gLSubGroupId;
                ViewData["glId"] = glId;
                ViewData["slId"] = slId;


            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View(tbDrillDownViewModel);
        }

        [ValidateRequest(true, UserInterfaceHelper.TBDrillDown, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult GLLedgerDrillDown(string reportLevel, string fromDate, string toDate, string startInterface = "", int gLMainGroupId = 0, int gLSubGroupId = 0, int glId = 0)
        {
            TBDrillDownBL tbBL = new TBDrillDownBL();
            GLDrillDownViewModel glDrillDownViewModel = new GLDrillDownViewModel();
            ResponseOut responseOut = new ResponseOut();
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                responseOut = tbBL.GenerateGLLedgerDrillDown(glId, ContextUser.CompanyId, finYearId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.UserId, Session.SessionID);

                glDrillDownViewModel.GLOpeningList = tbBL.GetGLDrillDown_GLOpening(ContextUser.UserId, Session.SessionID, glId);
                glDrillDownViewModel.GLLedgerList= tbBL.GetGLDrillDown_GLLedger(ContextUser.UserId, Session.SessionID, glId);
                //ViewData["CompanyBranchId"] = companyBranchId;
                ViewData["reportLevel"] = reportLevel;
                ViewData["fromDate"] = fromDate;
                ViewData["toDate"] = toDate;
                ViewData["startInterface"] = startInterface == "" ? "GLWise" : startInterface;
                ViewData["gLMainGroupId"] = gLMainGroupId;
                ViewData["gLSubGroupId"] = gLSubGroupId;
                ViewData["glId"] = glId;
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View(glDrillDownViewModel);
        }

        [ValidateRequest(true, UserInterfaceHelper.TBDrillDown, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult SLLedgerDrillDown(string reportLevel, string fromDate, string toDate, string startInterface = "", int gLMainGroupId = 0, int gLSubGroupId = 0, int glId = 0,Int32 slId=0)
        {
            BalanceSheetDrillDownBL balanceSheetDrillDownBL = new BalanceSheetDrillDownBL();
            SLDrillDownViewModel slDrillDownViewModel = new SLDrillDownViewModel();
            ResponseOut responseOut = new ResponseOut();
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;

                responseOut = balanceSheetDrillDownBL.GenerateSubLedgerDrillDown(glId,slId, ContextUser.CompanyId, finYearId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.UserId, Session.SessionID);

                slDrillDownViewModel.SLOpeningList = balanceSheetDrillDownBL.GetSLDrillDown_SLOpening(ContextUser.UserId, Session.SessionID, glId,slId);
                slDrillDownViewModel.SLLedgerList = balanceSheetDrillDownBL.GetSLDrillDown_SLLedger(ContextUser.UserId, Session.SessionID, glId,slId);

                ViewData["reportLevel"] = reportLevel;
                ViewData["fromDate"] = fromDate;
                ViewData["toDate"] = toDate;
                ViewData["startInterface"] = startInterface == "" ? "SLWise" : startInterface;
                ViewData["gLMainGroupId"] = gLMainGroupId;
                ViewData["gLSubGroupId"] = gLSubGroupId;
                ViewData["glId"] = glId;
                ViewData["slId"] = slId;
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View(slDrillDownViewModel);
        }


    

        #endregion


    }
}
