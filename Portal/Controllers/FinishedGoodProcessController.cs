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
    public class FinishedGoodProcessController : BaseController
    {
        #region FinishedGoodProcessController

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_FinishedGoodProcess, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditFinishedGoodProcess(int finishedGoodProcessId = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (finishedGoodProcessId != 0)
                {
                    ViewData["finishedGoodProcessId"] = finishedGoodProcessId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["finishedGoodProcessId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_FinishedGoodProcess, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditFinishedGoodProcess(FinishedGoodProcessViewModel finishedGoodProcessViewModel, List<FinishedGoodProcessProductViewModel> finishedGoodProcessProducts,List<FinishedGoodProcessChasisSerialViewModel> finishedGoodProcessChasisSerialList)
        {
            ResponseOut responseOut = new ResponseOut();

            FinishedGoodProcessBL finishedGoodProcessBL = new FinishedGoodProcessBL();
            try
            {                            
                if (finishedGoodProcessViewModel != null)
                {
                    finishedGoodProcessViewModel.CreatedBy = ContextUser.UserId;
                    finishedGoodProcessViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = finishedGoodProcessBL.AddEditFinishedGoodProcess(finishedGoodProcessViewModel, finishedGoodProcessProducts, finishedGoodProcessChasisSerialList);

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_FinishedGoodProcess, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListFinishedGoodProcess(string listStatus="false")
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                ViewData["listStatus"] = listStatus;
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpGet]
        public PartialViewResult GetFinishedGoodProcessList(string finishedGoodProcessNo = "", string workOrderNo = "", int companyBranchId = 0, string fromDate = "", string toDate = "",string finishedGoodProcessStatus = "")
        {
            List<FinishedGoodProcessViewModel> finishedGoodProcess = new List<FinishedGoodProcessViewModel>();
            FinishedGoodProcessBL finishedGoodProcessBL = new FinishedGoodProcessBL();
            try
            {
                finishedGoodProcess = finishedGoodProcessBL.GetFinishedGoodProcessList(finishedGoodProcessNo,workOrderNo, companyBranchId, fromDate, toDate, ContextUser.CompanyId, finishedGoodProcessStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(finishedGoodProcess);
        }

        [HttpGet]
        public JsonResult GetFinishedGoodProcessDetail(long finishedGoodProcessId)
        {
            FinishedGoodProcessBL finishedGoodProcessBL = new FinishedGoodProcessBL();
            FinishedGoodProcessViewModel finishedGoodProcessViewModel = new FinishedGoodProcessViewModel();
            try
            {
                finishedGoodProcessViewModel = finishedGoodProcessBL.GetFinishedGoodProcessDetail(finishedGoodProcessId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(finishedGoodProcessViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult GetFinishedGoodProcessProductList(List<FinishedGoodProcessProductViewModel> finishedGoodProcessProducts, long finishedGoodProcessId)
        {
            FinishedGoodProcessBL finishedGoodProcessBL = new FinishedGoodProcessBL();
            try
            {
                if (finishedGoodProcessProducts == null)
                {
                    finishedGoodProcessProducts = finishedGoodProcessBL.GetFinishedGoodProductList(finishedGoodProcessId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(finishedGoodProcessProducts);
        }

        [HttpGet]
        public PartialViewResult GetFinishedGoodProcessWorkOrderList(string workOrderNo, int companyBranchId, string fromDate, string toDate, string displayType = "", string approvalStatus = "")
        {
            List<WorkOrderViewModel> workOrders = new List<WorkOrderViewModel>();
            FinishedGoodProcessBL finishedGoodProcessBL = new FinishedGoodProcessBL();
            try
            {
                workOrders = finishedGoodProcessBL.GetFinishedGoodProcessWorkOrderList(workOrderNo, companyBranchId, fromDate, toDate, ContextUser.CompanyId, displayType, approvalStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(workOrders);
        }

        [HttpPost]
        public PartialViewResult GetWorkOrderProductList(List<WorkOrderProductViewModel> finishedGoodProcessProducts, long workOrderId)
        {
            WorkOrderBL workOrderBL = new WorkOrderBL();
            try
            {
                if (finishedGoodProcessProducts == null)
                {
                    finishedGoodProcessProducts = workOrderBL.GetWorkOrderProductList(workOrderId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(finishedGoodProcessProducts);
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

        public ActionResult Report(long finishedGoodProcessId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            FinishedGoodProcessBL finishedGoodProcessBL = new FinishedGoodProcessBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "FinishedGoodProcessReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            DataTable dt = new DataTable(); 
             dt = finishedGoodProcessBL.GetFinishedGoodProcessDataTable(finishedGoodProcessId);
            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            ReportDataSource rdProduct = new ReportDataSource("DataSet2", finishedGoodProcessBL.GetFinishedGoodProcessProductListDataTable(finishedGoodProcessId));
            ReportDataSource rdProductChasis = new ReportDataSource("DataSet3", finishedGoodProcessBL.GetFinishedGoodProcessChasiSerialProductListPrint(finishedGoodProcessId));
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
        public PartialViewResult GetFinishedGoodAssemblingProcessList(string assemblingProcessNo, int companyBranchId, string fromDate, string toDate, string displayType = "", string approvalStatus = "")
        {
            List<AssemblingProcessViewModel> assemblingProcess = new List<AssemblingProcessViewModel>();           
            FinishedGoodProcessBL finishedGoodProcessBL = new FinishedGoodProcessBL();
            try
            {
                assemblingProcess = finishedGoodProcessBL.GetFinishedGoodAssemblingProcessList(assemblingProcessNo, companyBranchId, fromDate, toDate, ContextUser.CompanyId, displayType, approvalStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(assemblingProcess);
        }

        [HttpPost]
        public PartialViewResult GetAssemblingProcessProductList(List<AssemblingProcessProductViewModel> assemblingProcessProducts, long assemblingProcessId)
        {                    
            FinishedGoodProcessBL finishedGoodProcessBL = new FinishedGoodProcessBL();
            try
            {
                if (assemblingProcessProducts == null)
                {
                    assemblingProcessProducts = finishedGoodProcessBL.GetFinishedGoodAssemblingProcessProductList(assemblingProcessId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(assemblingProcessProducts);
        }

        [HttpPost]
        public PartialViewResult GetProductSerialProductList(List<ProductSerialDetailViewModel> productSerialProducts)
        {                     
            FinishedGoodProcessBL finishedGoodProcess = new FinishedGoodProcessBL();
            try
            {
                if (productSerialProducts == null)
                {
                    productSerialProducts = finishedGoodProcess.GetFinishedGoodProductSerialProduct();
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(productSerialProducts);
        }
        [HttpPost]
        public PartialViewResult GetFinishedGoodProductSerialProductList(List<FinishedGoodProcessChasisSerialViewModel> finishedProcessChasisSerialProducts, long finishedGoodProcessId = 0)
        {
            FinishedGoodProcessBL finishedGoodProcessBL = new FinishedGoodProcessBL();
            
            try
            {
                if (finishedProcessChasisSerialProducts == null)
                {
                    finishedProcessChasisSerialProducts = finishedGoodProcessBL.GetFinishedGoodProductSerialProductList(finishedGoodProcessId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(finishedProcessChasisSerialProducts);
        }
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_FinishedGoodProcess, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult CancelFGP(int finishedGoodProcessId = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                if (finishedGoodProcessId != 0)
                {
                    ViewData["finishedGoodProcessId"] = finishedGoodProcessId;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["finishedGoodProcessId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_FinishedGoodProcess, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult CancelFGP(long finishedGoodProcessId, string finishedGoodProcessNo, int companyBranchId, string cancelReason, List<FinishedGoodProcessProductViewModel> finishedGoodProcessProducts)
        {
            ResponseOut responseOut = new ResponseOut();          
            FinishedGoodProcessBL finishedGoodProcessBL = new FinishedGoodProcessBL();
            FinishedGoodProcessViewModel finishedGoodProcessViewModel = new FinishedGoodProcessViewModel();
            try
            {
                if (finishedGoodProcessViewModel != null)
                {
                    finishedGoodProcessViewModel.FinishedGoodProcessId = finishedGoodProcessId;
                    finishedGoodProcessViewModel.FinishedGoodProcessNo = finishedGoodProcessNo;
                    finishedGoodProcessViewModel.CancelReason = cancelReason;
                    finishedGoodProcessViewModel.CreatedBy = ContextUser.UserId;
                    finishedGoodProcessViewModel.CompanyId = ContextUser.CompanyId;
                    finishedGoodProcessViewModel.CompanyBranchId = companyBranchId;
                    responseOut = finishedGoodProcessBL.CancelFGP(finishedGoodProcessViewModel, finishedGoodProcessProducts);
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
