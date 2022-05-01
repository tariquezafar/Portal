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
    public class AssemblingProcessController : BaseController
    {
        #region AssemblingProcess

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_AssemblingProcess, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditAssemblingProcess(int assemblingProcessId = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;


                if (assemblingProcessId != 0)
                {
                    ViewData["assemblingProcessId"] = assemblingProcessId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["assemblingProcessId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_AssemblingProcess, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditAssemblingProcess(AssemblingProcessViewModel assemblingProcessViewModel, List<AssemblingProcessProductViewModel> assemblingProcessProducts,List<AssemblingProcessChasisSerialViewModel> assemblingProcessChasisSerialProducts)
        {
            ResponseOut responseOut = new ResponseOut();

            AssemblingProcessBL assemblingProcessBL = new AssemblingProcessBL();
            try
            {                            
                if (assemblingProcessViewModel != null)
                {
                    assemblingProcessViewModel.CreatedBy = ContextUser.UserId;
                    assemblingProcessViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = assemblingProcessBL.AddEditAssemblingProcess(assemblingProcessViewModel, assemblingProcessProducts, assemblingProcessChasisSerialProducts);

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_AssemblingProcess, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListAssemblingProcess()
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
        public PartialViewResult GetAssemblingProcessList(string assemblingProcessNo = "", string workOrderNo = "", int companyBranchId = 0, string fromDate = "", string toDate = "",string assemblingProcessStatus = "")
        {
            List<AssemblingProcessViewModel> assemblingProcess = new List<AssemblingProcessViewModel>();
            AssemblingProcessBL assemblingProcessBL = new AssemblingProcessBL();
            try
            {
                assemblingProcess = assemblingProcessBL.GetAssemblingProcessList(assemblingProcessNo,workOrderNo, companyBranchId, fromDate, toDate, ContextUser.CompanyId, assemblingProcessStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(assemblingProcess);
        }

        [HttpGet]
        public JsonResult GetAssemblingProcessDetail(long assemblingProcessId)
        {
            AssemblingProcessBL assemblingProcessBL = new AssemblingProcessBL();
            AssemblingProcessViewModel assemblingProcessViewModel = new AssemblingProcessViewModel();
            try
            {
                assemblingProcessViewModel = assemblingProcessBL.GetAssemblingProcessDetail(assemblingProcessId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(assemblingProcessViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult GetAssemblingProcessProductList(List<AssemblingProcessProductViewModel> assemblingProcessProducts, long assemblingProcessId)
        {
            AssemblingProcessBL assemblingProcessBL = new AssemblingProcessBL();
            try
            {
                if (assemblingProcessProducts == null)
                {
                    assemblingProcessProducts = assemblingProcessBL.GetAssemblingProcessProductList(assemblingProcessId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(assemblingProcessProducts);
        }

        [HttpGet]
        public PartialViewResult GetAssemblingProcessWorkOrderList(string workOrderNo, int companyBranchId, string fromDate, string toDate, string displayType = "", string approvalStatus = "")
        {
            List<WorkOrderViewModel> workOrders = new List<WorkOrderViewModel>();
            AssemblingProcessBL assemblingProcessBL = new AssemblingProcessBL();
            try
            {
                workOrders = assemblingProcessBL.GetAssemblingProcessWorkOrderList(workOrderNo, companyBranchId, fromDate, toDate, ContextUser.CompanyId, displayType, approvalStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(workOrders);
        }

        [HttpPost]
        public PartialViewResult GetWorkOrderProductList(List<WorkOrderProductViewModel> assemblingProcessProducts, long workOrderId)
        {
            WorkOrderBL workOrderBL = new WorkOrderBL();
            try
            {
                if (assemblingProcessProducts == null)
                {
                    assemblingProcessProducts = workOrderBL.GetWorkOrderProductList(workOrderId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(assemblingProcessProducts);
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

        public ActionResult Report(long assemblingProcessId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            AssemblingProcessBL assemblingProcessBL = new AssemblingProcessBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "AssemblingProcessReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            DataTable dt = new DataTable();
            dt = assemblingProcessBL.GetAssemblingProcessDataTable(assemblingProcessId);
            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            ReportDataSource rdProduct = new ReportDataSource("DataSet2", assemblingProcessBL.GetAssemblingProcessProductListDataTable(assemblingProcessId));
            ReportDataSource rdProductChasis = new ReportDataSource("DataSet3", assemblingProcessBL.GetAssemblingProcessChasisPrint(assemblingProcessId));
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
        public PartialViewResult GetAssemblingProcessPaintProcessList(string paintProcessNo, int companyBranchId, string fromDate, string toDate, string displayType = "", string approvalStatus = "")
        {
            List<PaintProcessViewModel> paintProcess = new List<PaintProcessViewModel>();
            AssemblingProcessBL assemblingProcessBL = new AssemblingProcessBL();
            try
            {
                paintProcess = assemblingProcessBL.GetAssemblingProcessPaintProcessList(paintProcessNo, companyBranchId, fromDate, toDate, ContextUser.CompanyId, displayType, approvalStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(paintProcess);
        }

        [HttpPost]
        public PartialViewResult GetPaintProcessProductList(List<PaintProcessProductViewModel> assemblingProcessProducts, long paintProcessId)
        {           
            AssemblingProcessBL assemblingProcessBL = new AssemblingProcessBL();
            try
            {
                if (assemblingProcessProducts == null)
                {
                    assemblingProcessProducts = assemblingProcessBL.GetPaintProcessProductList(paintProcessId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(assemblingProcessProducts);
        }
        [HttpGet]
        public JsonResult GetAssemblingProducedQuantityAgainstWorkOrder(long workOrderId)
        {
            decimal productQuantity = 0;         
            AssemblingProcessBL assemblingProcessBL = new AssemblingProcessBL();
            try
            {
                productQuantity = assemblingProcessBL.GetAssemblingProcessProducedQuantityAgainstWorkOrder(workOrderId);
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
          
            AssemblingProcessBL assemblingProcessBL = new AssemblingProcessBL();
            try
            {
                if (productSerialProducts == null)
                {
                    productSerialProducts = assemblingProcessBL.GetProductSerialProduct();
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(productSerialProducts);
        }

        [HttpPost]
        public PartialViewResult GetAssemblingProcessProductSerialProductList(List<AssemblingProcessChasisSerialViewModel> assemblingProcessChasisSerialProducts, long assemblingProcessId=0)
        {           
            AssemblingProcessBL assemblingProcessBL = new AssemblingProcessBL();
            try
            {
                if (assemblingProcessChasisSerialProducts == null)
                {
                    assemblingProcessChasisSerialProducts = assemblingProcessBL.GetAssemblingProcessProductSerialProductList(assemblingProcessId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(assemblingProcessChasisSerialProducts);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_AssemblingProcess, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult CancelAP(int assemblingProcessId = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                if (assemblingProcessId != 0)
                {
                    ViewData["assemblingProcessId"] = assemblingProcessId;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["assemblingProcessId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_AssemblingProcess, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult CancelAP(long assemblingProcessId, string assemblingProcessNo, int companyBranchId, string cancelReason)
        {
            ResponseOut responseOut = new ResponseOut();    
            AssemblingProcessBL assemblingProcessBL = new AssemblingProcessBL();
            AssemblingProcessViewModel assemblingProcessViewModel = new AssemblingProcessViewModel();
            try
            {
                if (assemblingProcessViewModel != null)
                {
                    assemblingProcessViewModel.AssemblingProcessId = assemblingProcessId;
                    assemblingProcessViewModel.AssemblingProcessNo = assemblingProcessNo;
                    assemblingProcessViewModel.CancelReason = cancelReason;
                    assemblingProcessViewModel.CreatedBy = ContextUser.UserId;
                    assemblingProcessViewModel.CompanyId = ContextUser.CompanyId;
                    assemblingProcessViewModel.CompanyBranchId = companyBranchId;
                    responseOut = assemblingProcessBL.CancelAP(assemblingProcessViewModel);
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
