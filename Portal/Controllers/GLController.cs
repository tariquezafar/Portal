using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;
using Microsoft.Reporting.WebForms;
using System.IO;
namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class GLController : BaseController
    {       
        [ValidateRequest(true, UserInterfaceHelper.AddEdit_GL, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditGL(int GLId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                if (GLId != 0)
                {
                    ViewData["GLId"] = GLId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["GLId"] = 0;
                    ViewData["accessMode"] = 0;
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

      


        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.AddEdit_GL, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditGL(GLViewModel glViewModel,GLDetailViewModel glDetailViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            GLBL glBL = new GLBL();
            try
            {
                if (glViewModel != null && glDetailViewModel!=null)
                {
                    //FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                    //ViewData["fromDate"] = finYear.StartDate;
                    //ViewData["toDate"] = finYear.EndDate;
                    //ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                    glDetailViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    glViewModel.CreatedBy = ContextUser.UserId;
                    glViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = glBL.AddEditGL(glViewModel,glDetailViewModel);
                }

                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
                    responseOut.trnId = 0;
                }

            }
            catch (Exception ex)
            {
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [ValidateRequest(true, UserInterfaceHelper.AddEdit_GL, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult GLMappingOpening(int gLId=0,int companyBranchId=0,decimal openingBalance=0,decimal openingBalanceCredit=0,decimal openingBalanceDebit=0 ,string gLHead="")
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                ViewData["gLId"] = gLId;
                ViewData["ViewcompanyBranchId"] = companyBranchId;
                ViewData["openingBalance"] = openingBalance;
                ViewData["openingBalanceCredit"] = openingBalanceCredit;
                ViewData["openingBalanceDebit"] = openingBalanceDebit;
                ViewData["gLHead"] = gLHead;
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }


        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.AddEdit_GL, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult GLMappingOpening(GLDetailViewModel glDetailViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            GLBL glBL = new GLBL();
            try
            {
                if (glDetailViewModel != null)
                {
                    //FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                    //ViewData["fromDate"] = finYear.StartDate;
                    //ViewData["toDate"] = finYear.EndDate;
                    //ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                    glDetailViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    glDetailViewModel.CreatedBy = ContextUser.UserId;
                    glDetailViewModel.CompanyId = ContextUser.CompanyId;
                   
                    responseOut = glBL.AddEditGLMapping(glDetailViewModel);
                }

                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
                    responseOut.trnId = 0;
                }

            }
            catch (Exception ex)
            {
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        [ValidateRequest(true, UserInterfaceHelper.AddEdit_GL, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListGL()
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpGet]
        [ValidateRequest(true, UserInterfaceHelper.AddEdit_GL, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListGLOpening()
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [HttpGet]
        public PartialViewResult GetGLList(string GLHead = "", string GLCode = "", string GLType = "", int GLMainGroupId = 0, int GLSubGroupId = 0, int SLTypeId = 0,int companyBranchId=0)
        {
            List<GLViewModel> gls = new List<GLViewModel>();
            GLBL glBL = new GLBL();

            try
            {
                int FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                gls = glBL.GetGLList(GLHead ,GLCode, GLType, GLMainGroupId, GLSubGroupId, SLTypeId, ContextUser.CompanyId, FinYearId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(gls);
        }


        public PartialViewResult GLMappingOpeningList(string GLHead = "", string GLCode = "", string GLType = "", int GLMainGroupId = 0, int GLSubGroupId = 0, int SLTypeId = 0, int companyBranchId = 0)
        {
            List<GLViewModel> gls = new List<GLViewModel>();
            GLBL glBL = new GLBL();

            try
            {
                int FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                gls = glBL.GetGLList(GLHead, GLCode, GLType, GLMainGroupId, GLSubGroupId, SLTypeId, ContextUser.CompanyId, FinYearId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(gls);
        }

        public PartialViewResult GetGLMappingOpeningList(string GLHead = "", string GLCode = "", string GLType = "", int GLMainGroupId = 0, int GLSubGroupId = 0, int SLTypeId = 0, int companyBranchId = 0)
        {
            List<GLViewModel> gls = new List<GLViewModel>();
            GLBL glBL = new GLBL();

            try
            {
                int FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                gls = glBL.GetGLOpeningList(GLHead, GLCode, GLType, GLMainGroupId, GLSubGroupId, SLTypeId, ContextUser.CompanyId, FinYearId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(gls);
        }
        public JsonResult GetGLDetail(int GLId)
        {
            GLBL glBL = new GLBL();
            GLViewModel gLViewModel = new GLViewModel();
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;

                gLViewModel = glBL.GetGLDetail(GLId, finYearId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(gLViewModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGLDetailOpening(int GLId,long companyBranchId)
        {
            GLBL glBL = new GLBL();
            GLViewModel gLViewModel = new GLViewModel();
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;

                gLViewModel = glBL.GetGLDetailOpening(GLId, finYearId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(gLViewModel, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetGLSubGroupList(int MainGroupId)
        {
            GLBL gLBL = new GLBL();
            List<GLSubGroupViewModel> GLSubGroupList = new List<GLSubGroupViewModel>();
            try
            {
                GLSubGroupList = gLBL.GetGLSubGroupList(MainGroupId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(GLSubGroupList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GLReoprt(string GLHead = "", string GLCode = "", string GLType = "", int GLMainGroupId = 0, int GLSubGroupId = 0, int SLTypeId = 0, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            GLBL gLBL = new GLBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "GLReportReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("ListGL");
            }
            int FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
            ReportDataSource rd = new ReportDataSource("GLReportDataSet", gLBL.GLReport(GLHead, GLCode, GLType, GLMainGroupId, GLSubGroupId, SLTypeId, ContextUser.CompanyId, FinYearId));
            lr.DataSources.Add(rd);          
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =

             "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>12.8in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.50in</MarginTop>" +
            "  <MarginLeft>.1in</MarginLeft>" +
            "  <MarginRight>.1in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedBytes, mimeType);
        }

    }
}
