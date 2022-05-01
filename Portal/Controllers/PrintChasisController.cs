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
using Microsoft.ReportingServices;
using System.IO;
using System.Data;
using System.Text;
namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class PrintChasisController : BaseController
    {
        #region PrintChasis

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PrintChasis, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditPrintChasis(int printID = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (printID != 0)
                {
                    ViewData["printID"] = printID;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["printID"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PrintChasis, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditPrintChasis(PrintChasisViewModel printChasisViewModel, List<PrintChasisDetailViewModel> printChasisDetailViewModel)
        {
            ResponseOut responseOut = new ResponseOut();                               
            PrintChasisBL printChasisBL = new PrintChasisBL();
            try
            {                            
                if (printChasisViewModel != null)
                {
                    printChasisViewModel.CreatedBy = ContextUser.UserId;
                    printChasisViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = printChasisBL.AddEditPrintChasis(printChasisViewModel, printChasisDetailViewModel);
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PrintChasis, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListPrintChasis()
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
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
        public PartialViewResult GetPrintChasisList(string printNo = "", int companyBranchId = 0,  string fromDate = "", string toDate = "", string chasisSerialStatus = "")
        {
            List<PrintChasisViewModel> printChasisViewModel = new List<PrintChasisViewModel>();                  
            PrintChasisBL printChasisBL = new PrintChasisBL();
            try
            {
                printChasisViewModel = printChasisBL.GetPrintChasisList(printNo, companyBranchId, fromDate, toDate, ContextUser.CompanyId, chasisSerialStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(printChasisViewModel);
        }
       

        [HttpGet]
        public JsonResult GetPrintChasisDetail(long printID= 0)
        {                             
            PrintChasisViewModel printChasisViewModel = new PrintChasisViewModel();
            PrintChasisBL printChasisBL = new PrintChasisBL();
            try
            {
                printChasisViewModel = printChasisBL.GetPrintChasisDetail(printID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(printChasisViewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public PartialViewResult GetPrintChasisProductList(List<PrintChasisDetailViewModel> printChasisDetails, long companyBranchId=0,int chasisMonth=0)
        {                     
            PrintChasisBL printChasisBL = new PrintChasisBL();
            try
            {
                if (printChasisDetails == null)
                {
                    printChasisDetails = printChasisBL.GetPrintChasisProductsList(companyBranchId, chasisMonth);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(printChasisDetails);
        }
       
        public ActionResult PrintChasis(long printID, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();                   
            PrintChasisBL printChasisBL = new PrintChasisBL();

            string path = Path.Combine(Server.MapPath("~/RDLC"), "PrintChasisReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("ListPrintChasis");
            }
            DataTable dt = new DataTable();
            dt = printChasisBL.GetPrintChasisDetailPrint(printID);
            ReportDataSource rd = new ReportDataSource("PrintChasisDataSet", dt);
            ReportDataSource rdPackingList = new ReportDataSource("PrintChasisProductsDataSet", printChasisBL.GetPrintChasisProductsPrint(printID));          
            lr.DataSources.Add(rd);
            lr.DataSources.Add(rdPackingList);        
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                        "  <OutputFormat>" + reportType + "</OutputFormat>" +
                        "  <PageWidth>8.5in</PageWidth>" +
                        "  <PageHeight>11in</PageHeight>" +
                        "  <MarginTop>0.50in</MarginTop>" +
                        "  <MarginLeft>.15in</MarginLeft>" +
                        "  <MarginRight>.05in</MarginRight>" +
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

        [HttpPost]
        public PartialViewResult GetPrintChasisProducts(List<PrintChasisDetailViewModel> printChasisDetails, long printID = 0)
        {
            PrintChasisBL printChasisBL = new PrintChasisBL();
            try
            {
                if (printChasisDetails == null)
                {
                    printChasisDetails = printChasisBL.GetPrintChasisProducts(printID);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(printChasisDetails);
        }

        #endregion

    }
}
