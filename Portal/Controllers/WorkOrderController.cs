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
    public class WorkOrderController : BaseController
    {
        #region Work Order

        //
        // GET: /Quotation/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_WorkOrder, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditWorkOrder(int workOrderId = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (workOrderId != 0)
                {
                    ViewData["workOrderId"] = workOrderId;
                    ViewData["accessMode"] = accessMode;
                    
                }
                else
                {
                    ViewData["workOrderId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_WorkOrder, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditWorkOrder(WorkOrderViewModel workOrderViewModel, List<WorkOrderProductViewModel> workOrderProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            WorkOrderBL workOrderBL = new WorkOrderBL();
            try
            {
                if (workOrderViewModel != null)
                {
                    workOrderViewModel.CreatedBy = ContextUser.UserId;
                    workOrderViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = workOrderBL.AddEditWorkOrder(workOrderViewModel, workOrderProducts);

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_WorkOrder, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListWorkOrder(string pendingStatus="")
        {
            try
            {

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["pendingStatus"] = pendingStatus;
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
               // ViewData["fromDate"] = "01-Apr-2018";
               // ViewData["toDate"] = "31-Apr-2018";
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
        public PartialViewResult GetWorkOrderList(string workOrderNo = "", int companyBranchId=0, string fromDate = "", string toDate = "",string approvalStatus="")
        {
            List<WorkOrderViewModel> workOrders = new List<WorkOrderViewModel>();
            WorkOrderBL workOrderBL = new WorkOrderBL();
            companyBranchId = Session[SessionKey.CompanyBranchId] != null ? Convert.ToInt32(((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId) : 0;
            try
            {
                workOrders = workOrderBL.GetWorkOrderList(workOrderNo, companyBranchId,fromDate, toDate, ContextUser.CompanyId,"", approvalStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(workOrders);
        }

        [HttpGet]
        public JsonResult GetWorkOrderDetail(long workOrderId)
        {
            WorkOrderBL workOrderBL = new WorkOrderBL();
            WorkOrderViewModel workOrder = new WorkOrderViewModel();
            try
            {
                workOrder = workOrderBL.GetWorkOrderDetail(workOrderId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(workOrder, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public PartialViewResult GetWorkOrderProductList(List<WorkOrderProductViewModel> workOrderProducts, long workOrderId)
        {
            WorkOrderBL workOrderBL = new WorkOrderBL();
            try
            {
                if (workOrderProducts == null)
                {
                    workOrderProducts = workOrderBL.GetWorkOrderProductList(workOrderId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(workOrderProducts);
        }

        public ActionResult Report(long workOrderId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            WorkOrderBL workOrderBL = new WorkOrderBL();           
            string path = Path.Combine(Server.MapPath("~/RDLC"), "WorkOrderReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            DataTable dt = new DataTable();
            dt = workOrderBL.GetWorkOrderDataTable(workOrderId);           
            ReportDataSource rd = new ReportDataSource("WorkOrderDetailDataSet", dt);
            ReportDataSource rdProduct = new ReportDataSource("WorkOrderProductsDataSet", workOrderBL.GetWorkOrderProductListDataTable(workOrderId));
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


        [HttpGet]
        public JsonResult GetProductAutoCompleteBOMList(string term)
        {
            WorkOrderBL workOrderBL = new WorkOrderBL();
            List<ProductViewModel> productList = new List<ProductViewModel>();
            try
            {
                productList = workOrderBL.GetProductAutoCompleteBOMList(term, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetWorkOrderAutoCompleteList(string term)
        {
            WorkOrderBL workOrderBL = new WorkOrderBL();
            List<WorkOrderViewModel> workorderLists = new List<WorkOrderViewModel>();
            try
            {
                workorderLists = workOrderBL.GetWorkOrderAutoCompleteList(term);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(workorderLists, JsonRequestBehavior.AllowGet);
        }

        [ValidateRequest(true, UserInterfaceHelper.WIP_Report, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListWIPReport()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        public ActionResult WIPReport(long workOrderid,  string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();          
            WorkOrderBL workOrderBL = new WorkOrderBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "WIPReports.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("PrintStockLedger");
            }

            DataTable dt = new DataTable();
            dt = workOrderBL.GetWIPReport(workOrderid,ContextUser.CompanyId);
            ReportDataSource rd = new ReportDataSource("WIPDataSet", dt);
            lr.DataSources.Add(rd);
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>10.2in</PageWidth>" +
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
        public PartialViewResult GetSaleOrderList(string soNo = "", string customerName = "", string refNo = "", string fromDate = "", string toDate = "", string approvalStatus = "", string displayType = "",int companyBranchId=0)
        {
            List<SOViewModel> soList = new List<SOViewModel>();
            WorkOrderBL workOrderBL = new WorkOrderBL();

            try
            {
                soList = workOrderBL.GetWOSOList(soNo, customerName, refNo, fromDate, toDate, ContextUser.CompanyId, companyBranchId, approvalStatus, displayType);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(soList);
        }

        [HttpPost]
        public PartialViewResult GetWOSOProductList(List<SOProductViewModel> soProducts, long soId)
        {

            WorkOrderBL WorkOrderBL = new WorkOrderBL();
            try
            {
                if (soProducts == null)
                {
                    soProducts = WorkOrderBL.GetWOSOProductList(soId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(soProducts);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_WorkOrder, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult CancelWO(int workOrderId = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                if (workOrderId != 0)
                {
                    ViewData["workOrderId"] = workOrderId;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["workOrderId"] = 0;
                    ViewData["accessMode"] = 3;

                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }


        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_WorkOrder, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult CancelWO(long workOrderId, string workOrderNo, string cancelReason)
        {
            ResponseOut responseOut = new ResponseOut();
            WorkOrderBL workOrderBL = new WorkOrderBL();
            WorkOrderViewModel wOViewModel = new WorkOrderViewModel();
           

            try
            {
                if (wOViewModel != null)
                {
                    wOViewModel.WorkOrderId = workOrderId;
                    wOViewModel.WorkOrderNo = workOrderNo;
                    wOViewModel.CancelReason = cancelReason;
                    wOViewModel.CreatedBy = ContextUser.UserId;
                    wOViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = workOrderBL.CancelWO(wOViewModel);
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
        #endregion

        #region Work Order Cost
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_WorkOrder, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListWorkOrderCostReport()
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
        public PartialViewResult GetWorkOrderCostList(string workOrderNo, int companyBranchId, string fromDate, string toDate, string displayType = "", string approvalStatus = "")
        {
            List<WorkOrderViewModel> workOrders = new List<WorkOrderViewModel>();
            WorkOrderBL workOrderBL = new WorkOrderBL();

            try
            {
                workOrders = workOrderBL.GetCostWorkOrderList(workOrderNo, companyBranchId, fromDate, toDate, ContextUser.CompanyId, displayType, approvalStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(workOrders);
        }

        public ActionResult WoCostReport(long workOrderId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            WorkOrderBL workOrderBL = new WorkOrderBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "WorkOrderCostReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            DataTable dt = new DataTable();
            dt = workOrderBL.GetWorkOrderCostDetail(workOrderId);
            ReportDataSource rd = new ReportDataSource("WorkOrderDetailDataSet", dt);
            lr.DataSources.Add(rd);

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
        #endregion
    }
}
