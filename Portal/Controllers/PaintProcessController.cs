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
    public class PaintProcessController : BaseController
    {
        #region PaintProcess

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PaintProcess, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditPaintProcess(int paintProcessId = 0, int accessMode = 0)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (paintProcessId != 0)
                {
                    ViewData["paintProcessId"] = paintProcessId;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["paintProcessId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PaintProcess, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditPaintProcess(PaintProcessViewModel paintProcessViewModel, List<PaintProcessProductViewModel> paintProcessProducts,List<PaintProcessChasisSerialViewModel> paintProcessChasisSerialProducts)
        {
            ResponseOut responseOut = new ResponseOut();          
            PaintProcessBL paintProcessBL = new PaintProcessBL();
            try
            {                            
                if (paintProcessViewModel != null)
                {
                    paintProcessViewModel.CreatedBy = ContextUser.UserId;
                    paintProcessViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = paintProcessBL.AddEditPaintProcess(paintProcessViewModel, paintProcessProducts, paintProcessChasisSerialProducts);

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PaintProcess, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListPaintProcess()
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
        public PartialViewResult GetPaintProcessList(string paintProcessNo = "", string workOrderNo = "", int companyBranchId = 0, string fromDate = "", string toDate = "",string paintProcessStatus="")
        {
            List<PaintProcessViewModel> PaintProcesses = new List<PaintProcessViewModel>();
            PaintProcessBL paintProcessBL = new PaintProcessBL();
            try
            {
                PaintProcesses = paintProcessBL.GetPaintProcessList(paintProcessNo, workOrderNo, companyBranchId, fromDate, toDate, ContextUser.CompanyId, paintProcessStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(PaintProcesses);
        }

        [HttpGet]
        public JsonResult GetPaintProcessDetail(long paintProcessId)
        {
            PaintProcessBL paintProcessBL = new PaintProcessBL();
            PaintProcessViewModel paintProcessViewModel = new PaintProcessViewModel();
            try
            {
                paintProcessViewModel = paintProcessBL.GetPaintProcessDetail(paintProcessId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(paintProcessViewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public PartialViewResult GetPaintProcessProductList(List<PaintProcessProductViewModel> paintProcessProducts, long paintProcessId)
        {
            PaintProcessBL paintProcessBL = new PaintProcessBL();
            try
            {
                if (paintProcessProducts == null)
                {
                    paintProcessProducts = paintProcessBL.GetPaintProcessProductList(paintProcessId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(paintProcessProducts);
        }

        [HttpGet]
        public PartialViewResult GetPaintProcessWorkOrderList(string workOrderNo, int companyBranchId, string fromDate, string toDate, string displayType = "", string approvalStatus = "")
        {
            List<WorkOrderViewModel> workOrders = new List<WorkOrderViewModel>();         
            PaintProcessBL paintProcessBL = new PaintProcessBL();
            try
            {
                workOrders = paintProcessBL.GetPaintProcessWorkOrderList(workOrderNo, companyBranchId, fromDate, toDate, ContextUser.CompanyId, displayType, approvalStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(workOrders);
        }

        [HttpPost]
        public PartialViewResult GetWorkOrderProductList(List<WorkOrderProductViewModel> paintProcessProducts, long workOrderId)
        {
           
            PaintProcessBL paintProcessBL = new PaintProcessBL();
            try
            {
                if (paintProcessProducts == null)
                {
                    paintProcessProducts = paintProcessBL.GetPaintProcessWorkOrderProducts(workOrderId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(paintProcessProducts);
        }
        public JsonResult GetBranchLocationList(int companyBranchID)
        {
            LocationBL locationBL = new LocationBL();
            List<LocationViewModel> locationViewModel = new List<LocationViewModel>();
            try
            {
                locationViewModel = locationBL.GetFromLocationList(companyBranchID);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(locationViewModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Report(long paintProcessId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            PaintProcessBL paintProcessBL = new PaintProcessBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "PaintProcessReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            DataTable dt = new DataTable();
            dt = paintProcessBL.GetPaintProcessDataTable(paintProcessId);
            ReportDataSource rd = new ReportDataSource("DataSet1", dt); 
             ReportDataSource rdProduct = new ReportDataSource("DataSetPaintProduct", paintProcessBL.GetPaintProcessProductListDataTable(paintProcessId));
            ReportDataSource rdProductChasis = new ReportDataSource("DataSet2", paintProcessBL.GetPaintProcessChasisProductList(paintProcessId));
            lr.DataSources.Add(rd);
            lr.DataSources.Add(rdProduct);
            lr.DataSources.Add(rdProductChasis);
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
        public JsonResult GetPaintProcessProducedQuantityAgainstWorkOrder(long workOrderId)
        {
            decimal productQuantity = 0;            
            PaintProcessBL paintProcessBL = new PaintProcessBL();
            try
            {
                productQuantity = paintProcessBL.GetPaintProcessProducedQuantityAgainstWorkOrder(workOrderId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productQuantity, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public PartialViewResult GetProductSerialProductList(List<ProductSerialDetailViewModel> productSerialProducts)
        {
            PaintProcessBL paintProcessBL = new PaintProcessBL();
            try
            {
                if (productSerialProducts == null)
                {
                    productSerialProducts = paintProcessBL.GetProductSerialProduct();
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(productSerialProducts);
        }

        [HttpPost]
        public PartialViewResult GetPaintProcessProductSerialProductList(List<PaintProcessChasisSerialViewModel> paintProcessChasisSerialProducts, long paintProcessId)
        {
            PaintProcessBL paintProcessBL = new PaintProcessBL();
            try
            {
                if (paintProcessChasisSerialProducts == null)
                {
                    paintProcessChasisSerialProducts = paintProcessBL.GetPaintProcessProductSerialProductList(paintProcessId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(paintProcessChasisSerialProducts);
        }
        [HttpGet]
        public JsonResult GetWOProductList(long WorkOrderID=0)
        {           
            PaintProcessBL paintProcessBL = new PaintProcessBL();
            List<WorkOrderProductViewModel> productList = new List<WorkOrderProductViewModel>();
            try
            {
                productList = paintProcessBL.GetWOProductList(WorkOrderID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PaintProcess, (int)AccessMode.CancelAccess, (int)RequestMode.GetPost)]
        public ActionResult CancelPaintProcess(int paintProcessId = 0, int accessMode = 4)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                if (paintProcessId != 0)
                {
                    ViewData["paintProcessId"] = paintProcessId;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["paintProcessId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PaintProcess, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult CancelPaintProcess(long paintProcessId, string cancelReason)
        {
            ResponseOut responseOut = new ResponseOut();
            PaintProcessBL paintProcessBL = new PaintProcessBL();
            PaintProcessViewModel paintProcessViewModel = new PaintProcessViewModel();
            FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
            try
            {
                if (paintProcessViewModel != null)
                {
                    paintProcessViewModel.PaintProcessId = paintProcessId;
                    paintProcessViewModel.CancelReason = cancelReason;
                    paintProcessViewModel.CreatedBy = ContextUser.UserId;
                    paintProcessViewModel.CompanyId = ContextUser.CompanyId;
                    paintProcessViewModel.CompanyBranchId = Convert.ToInt32(ContextUser.CompanyBranchId);
                    paintProcessViewModel.FinYearId = finYear.FinYearId;
                    responseOut = paintProcessBL.CancelPaintProcess(paintProcessViewModel);
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

    }
}
