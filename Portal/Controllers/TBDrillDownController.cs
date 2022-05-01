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
    public class TBDrillDownController : BaseController
    {
        
        #region Trial Balance Drill Down
        [ValidateRequest(true, UserInterfaceHelper.TBDrillDown, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult TBDrillDown()
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
        [ValidateRequest(true, UserInterfaceHelper.TBDrillDown, (int)AccessMode.ViewAccess, (int)RequestMode.Ajax)]
        public ActionResult TrialBalanceDrillDownGenerate(string fromDate, string toDate, int companyBranchId)
        {
            ResponseOut responseOut = new ResponseOut();
            TBDrillDownBL tbBL = new TBDrillDownBL();
            try
            {
                    
                    int finYearId= Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = tbBL.GenerateTrialBalanceDrillDown(ContextUser.CompanyId, finYearId, Convert.ToDateTime(fromDate),Convert.ToDateTime(toDate),ContextUser.UserId,Session.SessionID, companyBranchId);
            }
            catch (Exception ex)
            {
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }

        [ValidateRequest(true, UserInterfaceHelper.TBDrillDown, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult MainGroupWiseTBDrillDown(string reportLevel,string fromDate,string toDate,int gLMainGroupId=0,int companyBranchId=0)
        {
            TBDrillDownBL tbBL = new TBDrillDownBL();
            TBDrillDownViewModel tbDrillDownViewModel = new TBDrillDownViewModel();
            try
            {
                ViewData["CompanyBranchId"] = companyBranchId;
                tbDrillDownViewModel.GLTypeList = tbBL.GetTBDrillDown_GLTypeList(ContextUser.UserId, Session.SessionID, companyBranchId);
                tbDrillDownViewModel.MainGroupList= tbBL.GetTBDrillDown_MainGroupList(ContextUser.UserId, Session.SessionID, companyBranchId);
                ViewData["reportLevel"] = reportLevel;
                ViewData["fromDate"] = fromDate;
                ViewData["toDate"] = toDate;
                ViewData["gLMainGroupId"] = gLMainGroupId;

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View(tbDrillDownViewModel);
        }

        [ValidateRequest(true, UserInterfaceHelper.TBDrillDown, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult SubGroupWiseTBDrillDown(string reportLevel, string fromDate, string toDate,string startInterface="",int gLMainGroupId=0, int gLSubGroupId = 0,int companyBranchId = 0)
        {
            TBDrillDownBL tbBL = new TBDrillDownBL();
            TBDrillDownViewModel tbDrillDownViewModel = new TBDrillDownViewModel();
            try
            {
                ViewData["CompanyBranchId"] = companyBranchId;
                tbDrillDownViewModel.GLTypeList = tbBL.GetTBDrillDown_GLTypeList(ContextUser.UserId, Session.SessionID, companyBranchId, gLMainGroupId);
                tbDrillDownViewModel.MainGroupList = tbBL.GetTBDrillDown_MainGroupList(ContextUser.UserId, Session.SessionID, companyBranchId, gLMainGroupId);
                tbDrillDownViewModel.SubGroupList = tbBL.GetTBDrillDown_SubGroupList(ContextUser.UserId, Session.SessionID, companyBranchId, gLMainGroupId);
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
        public ActionResult GLWiseTBDrillDown(string reportLevel, string fromDate, string toDate, string startInterface = "", int gLMainGroupId = 0, int gLSubGroupId = 0, int glId = 0,int companyBranchId = 0)
        {
            TBDrillDownBL tbBL = new TBDrillDownBL();
            TBDrillDownViewModel tbDrillDownViewModel = new TBDrillDownViewModel();
            try
            {
                ViewData["CompanyBranchId"] = companyBranchId;
                tbDrillDownViewModel.GLTypeList = tbBL.GetTBDrillDown_GLTypeList(ContextUser.UserId, Session.SessionID, companyBranchId, gLMainGroupId,gLSubGroupId);
                tbDrillDownViewModel.MainGroupList = tbBL.GetTBDrillDown_MainGroupList(ContextUser.UserId, Session.SessionID, companyBranchId, gLMainGroupId,gLSubGroupId);
                tbDrillDownViewModel.SubGroupList = tbBL.GetTBDrillDown_SubGroupList(ContextUser.UserId, Session.SessionID, companyBranchId, gLMainGroupId, gLSubGroupId);
                tbDrillDownViewModel.GLWiseList= tbBL.GetTBDrillDown_GLWiseList(ContextUser.UserId, Session.SessionID, companyBranchId, gLMainGroupId, gLSubGroupId,glId);
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
            return View(tbDrillDownViewModel);
        }


        [ValidateRequest(true, UserInterfaceHelper.TBDrillDown, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult SLWiseTBDrillDown(string reportLevel, string fromDate, string toDate, string startInterface = "", int gLMainGroupId = 0, int gLSubGroupId = 0, int glId = 0,Int32 slId=0,int companyBranchId = 0)
        {
            TBDrillDownBL tbBL = new TBDrillDownBL();
            TBDrillDownViewModel tbDrillDownViewModel = new TBDrillDownViewModel();
            try
            {
                ViewData["CompanyBranchId"] = companyBranchId;
                tbDrillDownViewModel.GLTypeList = tbBL.GetTBDrillDown_GLTypeList(ContextUser.UserId, Session.SessionID, companyBranchId, gLMainGroupId, gLSubGroupId,glId);
                tbDrillDownViewModel.MainGroupList = tbBL.GetTBDrillDown_MainGroupList(ContextUser.UserId, Session.SessionID, companyBranchId, gLMainGroupId, gLSubGroupId,glId);
                tbDrillDownViewModel.SubGroupList = tbBL.GetTBDrillDown_SubGroupList(ContextUser.UserId, Session.SessionID, companyBranchId, gLMainGroupId, gLSubGroupId,glId);
                tbDrillDownViewModel.GLWiseList = tbBL.GetTBDrillDown_GLWiseList(ContextUser.UserId, Session.SessionID, companyBranchId, gLMainGroupId, gLSubGroupId, glId);
                tbDrillDownViewModel.SLWiseList = tbBL.GetTBDrillDown_SLWiseList(ContextUser.UserId, Session.SessionID, companyBranchId, gLMainGroupId, gLSubGroupId, glId);
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
            TBDrillDownBL tbBL = new TBDrillDownBL();
            SLDrillDownViewModel slDrillDownViewModel = new SLDrillDownViewModel();
            ResponseOut responseOut = new ResponseOut();
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                responseOut = tbBL.GenerateSubLedgerDrillDown(glId,slId, ContextUser.CompanyId, finYearId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.UserId, Session.SessionID);

                slDrillDownViewModel.SLOpeningList = tbBL.GetSLDrillDown_SLOpening(ContextUser.UserId, Session.SessionID, glId,slId);
                slDrillDownViewModel.SLLedgerList = tbBL.GetSLDrillDown_SLLedger(ContextUser.UserId, Session.SessionID, glId,slId);

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


        //public ActionResult GeneralLedgerWoFyReport(string fromDate,string toDate,string reportType = "PDF")
        //{
        //    LocalReport lr = new LocalReport();
        //    GeneralLedgerWOFYPrintBL glPrintBL = new GeneralLedgerWOFYPrintBL();
        //    CompanyBL companyBL = new CompanyBL();
        //    string companyName = companyBL.GetCompanyDetail(ContextUser.CompanyId).CompanyName;


        //    string path = Path.Combine(Server.MapPath("~/RDLC"), "GeneralLedgerWoFyPrint.rdlc");
        //    if (System.IO.File.Exists(path))
        //    {
        //        lr.ReportPath = path;
        //    }
        //    else
        //    {
        //        return View("GeneralLedgerPrint");
        //    }

        //    DataTable dt = new DataTable();
        //    dt = glPrintBL.GetGeneralLedgerDetailDataTable(ContextUser.UserId,Session.SessionID);

        //    ReportDataSource rd = new ReportDataSource("DataSet1", dt);
        //    lr.DataSources.Add(rd);




        //    ReportParameter rp1 = new ReportParameter("CompanyName", companyName);
        //    ReportParameter rp2 = new ReportParameter("FromDate", fromDate);
        //    ReportParameter rp3 = new ReportParameter("ToDate", toDate);


        //    lr.SetParameters(rp1);
        //    lr.SetParameters(rp2);
        //    lr.SetParameters(rp3);

        //    string mimeType;
        //    string encoding;
        //    string fileNameExtension;



        //    string deviceInfo = "<DeviceInfo>" +
        //    "  <OutputFormat>" + reportType + "</OutputFormat>" +
        //    "  <PageWidth>8.5in</PageWidth>" +
        //    "  <PageHeight>11in</PageHeight>" +
        //    "  <MarginTop>0.50in</MarginTop>" +
        //    "  <MarginLeft>.2in</MarginLeft>" +
        //    "  <MarginRight>.2in</MarginRight>" +
        //    "  <MarginBottom>0.5in</MarginBottom>" +
        //    "</DeviceInfo>";

        //    Warning[] warnings;
        //    string[] streams;
        //    byte[] renderedBytes;

        //    renderedBytes = lr.Render(
        //        reportType,
        //        deviceInfo,
        //        out mimeType,
        //        out encoding,
        //        out fileNameExtension,
        //        out streams,
        //        out warnings);


        //    return File(renderedBytes, mimeType);
        //}



        #endregion


    }
}
