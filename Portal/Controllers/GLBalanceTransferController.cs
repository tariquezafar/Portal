using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;
using System.IO;
using Microsoft.Reporting.WebForms;

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class GLBalanceTransferController : BaseController
    {
       
        
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_BalanceTransferGL, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult GLBalanceTransfer(int transferId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (transferId != 0)
                {
                    ViewData["transferId"] = transferId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["transferId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_BalanceTransferGL, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult GLBalanceTransfer(GLTransferClosingBalanceViewModel gLtransferClosingBalanceViewModel, List<GLDetailViewModel> gLDetailViewModel)
        {
            ResponseOut responseOut = new ResponseOut();          
            GLBalanceTransferBL gLBalanceTransferBL = new GLBalanceTransferBL();
            try
            {
                if (gLtransferClosingBalanceViewModel != null)
                {
                    gLtransferClosingBalanceViewModel.CreatedBy = ContextUser.UserId;
                    gLtransferClosingBalanceViewModel.CompanyId = ContextUser.CompanyId;                 
                    responseOut = gLBalanceTransferBL.GLBalanceClosingTransfer(gLtransferClosingBalanceViewModel, gLDetailViewModel);
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_BalanceTransferGL, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListGLBalanceTransfer()
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
        public JsonResult GetFinYearDetail()
        {
            GLBalanceTransferBL glBalanceTransferBL = new GLBalanceTransferBL();
            FinYearViewModel finYearViewModel = new FinYearViewModel();
            try
            {
                finYearViewModel = glBalanceTransferBL.GetFinYearDetail();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(finYearViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult GetGLBalanceTransfer(int finYearId, DateTime endDate)
        {
            List<TempGLHeadClosingBalanceTransferViewModel> tempGLHeadClosingBalanceTransferViewModel = new List<TempGLHeadClosingBalanceTransferViewModel>();           
            GLBalanceTransferBL gLBalanceTransferBL = new GLBalanceTransferBL();
            try
            {
              
                tempGLHeadClosingBalanceTransferViewModel = gLBalanceTransferBL.GetGLBalanceTransfer(ContextUser.CompanyId, finYearId, Convert.ToDateTime(endDate), ContextUser.UserId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(tempGLHeadClosingBalanceTransferViewModel);
        }

        public ActionResult GenerateTransferReport(int companyBranchId, string fromDate, string toDate, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            BalanceTransferBL balanceTransferBL = new BalanceTransferBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "BalanceTransferReports.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            ReportDataSource rd = new ReportDataSource("BalanceTransferDataSet", balanceTransferBL.GetFinYearProducts(companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.CompanyId));
            lr.DataSources.Add(rd);

            ReportParameter rp3 = new ReportParameter("fromdate", fromDate);
            ReportParameter rp4 = new ReportParameter("todate", toDate);
            lr.SetParameters(rp3);
            lr.SetParameters(rp4);

            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

             "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>9.9in</PageWidth>" +
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

        public ActionResult ReversedClosingTransfer(int toFinYearID)
        {
            ResponseOut responseOut = new ResponseOut();
            BalanceTransferBL balanceTransferBL = new BalanceTransferBL();
            TransferClosingBalanceViewModel transferClosingBalanceViewModel = new TransferClosingBalanceViewModel();
            try
            {
                if (transferClosingBalanceViewModel != null)
                {
                    transferClosingBalanceViewModel.ToFinYearID = toFinYearID;                 
                    responseOut = balanceTransferBL.ReversedClosingTransfer(transferClosingBalanceViewModel);
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
        public PartialViewResult GetGLBalanceTransferList(int companyBranchId=0, int fromFinYearID=0, int toFinYearID=0, string createdBy = "")
        {
            List<GLTransferClosingBalanceViewModel> balanceTransferViewModel = new List<GLTransferClosingBalanceViewModel>();         
            GLBalanceTransferBL gLBalanceTransferBL = new GLBalanceTransferBL();
            try
            {               
                balanceTransferViewModel = gLBalanceTransferBL.GetGLBalanceTransferList(companyBranchId, fromFinYearID, toFinYearID, createdBy);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(balanceTransferViewModel);
        }

    }
}
