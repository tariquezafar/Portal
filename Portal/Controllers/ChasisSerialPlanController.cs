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
    public class ChasisSerialPlanController : BaseController
    {
        #region ChasisSerialPlan

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ChasisSerialPlan, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditChasisSerialPlan(int chasisSerialPlanID = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (chasisSerialPlanID != 0)
                {
                    ViewData["chasisSerialPlanID"] = chasisSerialPlanID;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["chasisSerialPlanID"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ChasisSerialPlan, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditChasisSerialPlan(ChasisSerialPlanViewModel chasisSerialPlanViewModel, List<ChasisSerialPlanDetailViewModel> chasisSerialPlanDetailProducts,List<ChasisSerialModelDetailViewModel> ChasisSerialModelDetailViewModels)
        {
            ResponseOut responseOut = new ResponseOut();                     
            ChasisSerialPlanBL chasisSerialPlanBL = new ChasisSerialPlanBL();
            try
            {                            
                if (chasisSerialPlanViewModel != null)
                {
                    chasisSerialPlanViewModel.CreatedBy = ContextUser.UserId;
                    chasisSerialPlanViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = chasisSerialPlanBL.AddEditChasisSerialPlan(chasisSerialPlanViewModel, chasisSerialPlanDetailProducts, ChasisSerialModelDetailViewModels);
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ChasisSerialPlan, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListChasisSerialPlan()
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
        public PartialViewResult GetChasisSerialPlanList(string chasisSerialPlanNo = "", int chasisYear = 0, int chasisMonth = 0, string fromDate = "", string toDate = "", string chasisSerialStatus = "",int companyBranchId=0)
        {
            List<ChasisSerialPlanViewModel> chasisSerialPlans = new List<ChasisSerialPlanViewModel>();         
            ChasisSerialPlanBL chasisSerialPlanBL = new ChasisSerialPlanBL();
            try
            {
                chasisSerialPlans = chasisSerialPlanBL.GetChasisSerialPlanList(chasisSerialPlanNo, chasisYear, chasisMonth, fromDate, toDate, ContextUser.CompanyId, chasisSerialStatus, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(chasisSerialPlans);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ChasisSerialPlan, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListChasisNoDetail()
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



        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ChasisSerialPlan, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListChasisSerialTracking()
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


        public ActionResult ChassisNoDetailedReport(string saleInvoiceNo = "", string chasisNo = "", string partyName = "", string fromDate = "", string toDate = "",int companyBranchId=0, string reportType = "PDF")
        {
           
            ChasisSerialPlanBL chasisSerialPlanBL = new ChasisSerialPlanBL();
            LocalReport lr = new LocalReport();
          
            string path = Path.Combine(Server.MapPath("~/RDLC"), "ChassisNoDetailedReports.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("ListChasisNoDetail");
            }

            DataTable dt = new DataTable();
           
                dt = chasisSerialPlanBL.GetChasisNoDetailedList(saleInvoiceNo, chasisNo, partyName, fromDate, toDate, ContextUser.CompanyId, companyBranchId);


            ReportDataSource rd = new ReportDataSource("DataTable1", dt);
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
            "  <PageWidth>15in</PageWidth>" +
            "  <PageHeight>8.5in</PageHeight>" +
            "  <MarginTop>0.50in</MarginTop>" +
            "  <MarginLeft>.2in</MarginLeft>" +
            "  <MarginRight>.2in</MarginRight>" +
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


        public ActionResult ChassisNoTrackingDetailedReport(string chasisNo = "", string reportType = "PDF")
        {

            ChasisSerialPlanBL chasisSerialPlanBL = new ChasisSerialPlanBL();
            LocalReport lr = new LocalReport();

            string path = Path.Combine(Server.MapPath("~/RDLC"), "ChassisNoTrackingDetailedReports.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("ListChasisSerialTracking");
            }

            DataTable dt = new DataTable();

            dt = chasisSerialPlanBL.GetChasisSerialNoTrackingReport(chasisNo);


            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            lr.DataSources.Add(rd);

           
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>15in</PageWidth>" +
            "  <PageHeight>8.5in</PageHeight>" +
            "  <MarginTop>0.50in</MarginTop>" +
            "  <MarginLeft>.2in</MarginLeft>" +
            "  <MarginRight>.2in</MarginRight>" +
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

        [HttpGet]
        public PartialViewResult GetChasisModelProductList(long chasisSerialPlanID=0,string status="", int year=0,int month=0)
        {
            List<ChasisSerialPlanProductViewModel> chasisSerialPlanProductViewModels = new List<ChasisSerialPlanProductViewModel>();
            ChasisSerialPlanBL chasisSerialPlanBL = new ChasisSerialPlanBL();
            try
            {
                chasisSerialPlanProductViewModels = chasisSerialPlanBL.GetChasisModelProductList(chasisSerialPlanID, status,year, month);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(chasisSerialPlanProductViewModels);
        }

        [HttpGet]
        public JsonResult GetChasisSerialPlanDetail(long chasisSerialPlanID)
        {                  
            ChasisSerialPlanBL chasisSerialPlanBL = new ChasisSerialPlanBL();
            ChasisSerialPlanViewModel chasisSerialPlanViewModel = new ChasisSerialPlanViewModel();
            try
            {
                chasisSerialPlanViewModel = chasisSerialPlanBL.GetChasisSerialPlanDetail(chasisSerialPlanID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(chasisSerialPlanViewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public PartialViewResult GetChasisSerialPlanProductList(List<ChasisSerialPlanDetailViewModel> ChasisSerialPlanDetails, long chasisSerialPlanID)
        {           
            ChasisSerialPlanBL chasisSerialPlanBL = new ChasisSerialPlanBL();
            try
            {
                if (ChasisSerialPlanDetails == null)
                {
                    ChasisSerialPlanDetails = chasisSerialPlanBL.GetChasisSerialPlanProductList(chasisSerialPlanID);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(ChasisSerialPlanDetails);
        }

        [HttpGet]
        public JsonResult GetLastIncrement()
        {
            ChasisSerialPlanBL chasisSerialPlanBL = new ChasisSerialPlanBL();
            ChasisSerialPlanViewModel chasisSerialPlanViewModel = new ChasisSerialPlanViewModel();
            try
            {
                chasisSerialPlanViewModel = chasisSerialPlanBL.GetLastIncrement();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(chasisSerialPlanViewModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChasisSerialPlanPrint(long chasisSerialPlanID, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();          
            ChasisSerialPlanBL chasisSerialPlanBL = new ChasisSerialPlanBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "ChasisSerialPlanReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            DataTable dt = new DataTable();
            dt = chasisSerialPlanBL.GetChasisSerialPlanDetailPrint(chasisSerialPlanID);
            ReportDataSource rd = new ReportDataSource("ChasisSerialPlanDataSet", dt);
            ReportDataSource rdPackingList = new ReportDataSource("ChasisSerialPlanDetailDataSet", chasisSerialPlanBL.GetChasisSerialPlanDetailProducts(chasisSerialPlanID));
            ReportDataSource rdPackingListTypeName = new ReportDataSource("ChasisModelProductDataSet", chasisSerialPlanBL.GetChasisSerialPlanMOdelDetailProducts(chasisSerialPlanID));
            lr.DataSources.Add(rd);
            lr.DataSources.Add(rdPackingList);
            lr.DataSources.Add(rdPackingListTypeName);
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

        [HttpGet]
        public JsonResult GetManufactorLocationCode(long companyBranchId=0)
        {
            string manufactorLocationCode = "";
            ChasisSerialPlanBL chasisSerialPlanBL = new ChasisSerialPlanBL();        
            try
            {
                manufactorLocationCode = chasisSerialPlanBL.GetManufactorLocationCode(companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(manufactorLocationCode, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetChasisSerialAutoCompleteList(string term)
        {
            ChasisSerialPlanBL chasisSerialPlanBL = new ChasisSerialPlanBL();
            ProductBL productBL = new ProductBL();
            List<ProductViewModel> productList = new List<ProductViewModel>();
            try
            {
                productList = productBL.GetProductAutoCompleteList(term, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productList, JsonRequestBehavior.AllowGet);
        }


        #endregion

    }
}
