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
    public class SaleTargetController : BaseController
    {
        //
        // GET: /SaleTarget/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SaleTarget, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditSaleTarget(int saleTargetId = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                if (saleTargetId != 0)
                {
                    ViewData["saleTargetId"] = saleTargetId;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["saleTargetId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SaleTarget, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditSaleTarget(TargetViewModel targetViewModel, List<TargetDetailViewModel> targetDetails)
        {
            ResponseOut responseOut = new ResponseOut();
            SaleTargetBL saleTargetBL = new SaleTargetBL();
            
            try
            {
                if (targetViewModel != null)
                {
                    targetViewModel.CreatedBy = ContextUser.UserId;
                    targetViewModel.CompanyId = ContextUser.CompanyId;
                    targetViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = saleTargetBL.AddEditTarget(targetViewModel, targetDetails);

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

        public ActionResult Report(int targetId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();

            SaleTargetBL saleTargetBL = new SaleTargetBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "TargetReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            DataTable dt = new DataTable();
            dt = saleTargetBL.GetTargetDataTable(targetId);
            ReportDataSource rd = new ReportDataSource("TargetDataSet", dt);
            ReportDataSource rdProduct = new ReportDataSource("TargetDetailDataSet", saleTargetBL.GetTargetDetailDataTable(targetId));
            lr.DataSources.Add(rd);
            lr.DataSources.Add(rdProduct);
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                        "  <OutputFormat>" + reportType + "</OutputFormat>" +
                        "  <PageWidth>8.5in</PageWidth>" +
                        "  <PageHeight>9in</PageHeight>" +
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SaleTarget, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListSaleTarget(string totalSaleTargetList="false")
        {
            try
            {
                
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["totalSaleTargetList"] = totalSaleTargetList;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpGet]
        public JsonResult GetSaleTargetDetail(long targetId)
        {
            SaleTargetBL saleTargetBL = new SaleTargetBL();
           
            TargetViewModel target = new TargetViewModel();
            try
            {
                target = saleTargetBL.GetTargetDetail(targetId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(target, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult GetSaleTargetList(string targetNo = "", int companyBranchId = 0, string fromDate = "", string toDate = "", string approvalStatus = "")
        {
            List<TargetViewModel> targets = new List<TargetViewModel>();
            SaleTargetBL saleTargetBL = new SaleTargetBL();
            try
            {
                targets = saleTargetBL.GetTargetList(targetNo, companyBranchId, fromDate, toDate, ContextUser.CompanyId, "", approvalStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(targets);
        }


        [HttpPost]
        public PartialViewResult GetSaleTargetDetailList(List<TargetDetailViewModel> targetDetails, long targetId)
        {
            SaleTargetBL saleTargetBL = new SaleTargetBL();
             
            try
            {
                if (targetDetails == null)
                {
                    targetDetails = saleTargetBL.GetTargetDetailList(targetId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(targetDetails);
        }

        [HttpGet]
        public JsonResult GetEmployeeAutoCompleteList(string term,long companyBranchId)
        {
           SaleTargetBL saleTargetBL = new SaleTargetBL();
            
            List<EmployeeViewModel> employeeList = new List<EmployeeViewModel>();
            try
            {
                employeeList = saleTargetBL.GetEmployeeAutoCompleteList(term, companyBranchId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(employeeList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStateList()
        {
            StateBL stateBL = new StateBL();
            List<StateViewModel> stateList = new List<StateViewModel>();
            try
            {
                stateList = stateBL.GetStateList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(stateList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetCityAutoCompleteList(string term,int stateId)
        {
            SaleTargetBL saleTargetBL = new SaleTargetBL();

            List<CityViewModel> cityList = new List<CityViewModel>();
            try
            {
                cityList = saleTargetBL.GetCityAutoCompleteList(term, stateId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(cityList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTargetTypeList()
        {
            TargetTypeBL targetTypeBL = new TargetTypeBL();
            List<TargetTypeViewModel> targetTypeList = new List<TargetTypeViewModel>();
            try
            {
                targetTypeList = targetTypeBL.GetTargetTypeList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(targetTypeList, JsonRequestBehavior.AllowGet);
        }



        [ValidateRequest(true, UserInterfaceHelper.SalesTargetType_Report, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListSalesTargetReport(string totalSaleTargetList = "false")
        {
            try
            {

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }


        public ActionResult SalesTargetTypeReport(int salesEmpid,string fromDate,string toDate,string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();

            SaleTargetBL saleTargetBL = new SaleTargetBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "SalesTargetTypeReports.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            DataTable dt = new DataTable();
            dt = saleTargetBL.GetSaleTargetTypeDataTable(salesEmpid, ContextUser.UserId,fromDate, toDate,ContextUser.CompanyId);
            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            
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
            "  <MarginLeft>1in</MarginLeft>" +
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
        public JsonResult GetEmployeeList()
        {
            SaleTargetBL saleTargetBL = new SaleTargetBL();
            List<UserViewModel> userList = new List<UserViewModel>();
            try
            {
                userList = saleTargetBL.GetEmployeeList(ContextUser.UserId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(userList, JsonRequestBehavior.AllowGet);
        }
    }
}
