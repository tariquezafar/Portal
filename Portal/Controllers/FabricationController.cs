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
    public class FabricationController : BaseController
    {
        #region Fabrication

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Fabrication, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditFabrication(int fabricationId = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (fabricationId != 0)
                {
                    ViewData["fabricationId"] = fabricationId;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["fabricationId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Fabrication, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditFabrication(FabricationViewModel fabricationViewModel, List<FabricationProductViewModel> fabricationProducts, List<FabricationChasisSerialViewModel> fabricationChasisSerialProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            ChasisSerialPlanBL chasisSerialPlanBL = new ChasisSerialPlanBL();          
            FabricationBL fabricationBL = new FabricationBL();
            try
            {                            
                if (fabricationViewModel != null)
                {
                    fabricationViewModel.CreatedBy = ContextUser.UserId;
                    fabricationViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = fabricationBL.AddEditFabrication(fabricationViewModel, fabricationProducts, fabricationChasisSerialProducts);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Fabrication, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListFabrication(string listStatus = "false")
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
        public PartialViewResult GetFabricationList(string fabricationNo = "", string workOrderNo = "", int companyBranchId = 0, string fromDate = "", string toDate = "",string fabricationStatus="")
        {
            List<FabricationViewModel> fabrications = new List<FabricationViewModel>();
            FabricationBL fabricationBL = new FabricationBL();
            try
            {
                fabrications = fabricationBL.GetFabricationList(fabricationNo,workOrderNo, companyBranchId, fromDate, toDate, ContextUser.CompanyId, fabricationStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(fabrications);
        }

        [HttpGet]
        public JsonResult GetFabricationDetail(long fabricationId)
        {
            FabricationBL fabricationBL = new FabricationBL();
            FabricationViewModel fabricationViewModel = new FabricationViewModel();
            try
            {
                fabricationViewModel = fabricationBL.GetFabricationDetail(fabricationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(fabricationViewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public PartialViewResult GetFabricationProductList(List<FabricationProductViewModel> fabricationProducts, long fabricationId)
        {          
            FabricationBL fabricationBL = new FabricationBL();
            try
            {
                if (fabricationProducts == null)
                {
                    fabricationProducts = fabricationBL.GetFabricationProductList(fabricationId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(fabricationProducts);
        }

        [HttpGet]
        public PartialViewResult GetFabricationWorkOrderList(string workOrderNo, int companyBranchId, string fromDate, string toDate, string displayType = "", string approvalStatus = "")
        {
            List<WorkOrderViewModel> workOrders = new List<WorkOrderViewModel>();
            
            FabricationBL fabricationBL = new FabricationBL();
            try
            {
                workOrders = fabricationBL.GetFabricationWorkOrderList(workOrderNo, companyBranchId, fromDate, toDate, ContextUser.CompanyId, displayType, approvalStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(workOrders);
        }

        [HttpPost]
        public PartialViewResult GetWorkOrderProductList(List<WorkOrderProductViewModel> fabricationProducts, long workOrderId)
        {          
            FabricationBL fabricationBL = new FabricationBL();
            try
            {
                if (fabricationProducts == null)
                {
                    fabricationProducts = fabricationBL.GetFabricationWorkOrderProductList(workOrderId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(fabricationProducts);
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

        public ActionResult Report(long fabricationId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();           
            FabricationBL fabricationBL = new FabricationBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "FabricationReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            DataTable dt = new DataTable();
            dt = fabricationBL.GetFabricationDataTable(fabricationId);
            ReportDataSource rd = new ReportDataSource("FabricationDetailDataSet", dt);
            ReportDataSource rdProduct = new ReportDataSource("FabricationProductsDataSet", fabricationBL.GetFabricationProductListDataTable(fabricationId));
            ReportDataSource rdProductChasis = new ReportDataSource("DataSet1", fabricationBL.GetFabricationChasisPrint(fabricationId));
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
        public JsonResult GetFabricationProducedQuantityAgainstWorkOrder(long workOrderId)
        {
            decimal productQuantity = 0;
            FabricationBL fabricationBL = new FabricationBL();          
            try
            {
                productQuantity = fabricationBL.GetFabricationProducedQuantityAgainstWorkOrder(workOrderId);
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
            FabricationBL fabricationBL = new FabricationBL();         
            try
            {
                if (productSerialProducts == null)
                {
                    productSerialProducts = fabricationBL.GetProductSerialProduct();
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(productSerialProducts);
        }
        

        [HttpPost]
        public PartialViewResult GetFabricationChasisSerials(List<FabricationChasisSerialViewModel> fabricationChasisSerialsDetails)
        {
            FabricationBL fabricationBL = new FabricationBL();
            try
            {
                if (fabricationChasisSerialsDetails == null)
                {
                    fabricationChasisSerialsDetails = fabricationBL.GetFabricationChasisSerials();
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(fabricationChasisSerialsDetails);
        }

        [HttpPost]
        public PartialViewResult GetFabricationChasisSerialProductList(List<FabricationChasisSerialViewModel> fabricationChasisSerialProducts, long fabricationId)
        {
            FabricationBL fabricationBL = new FabricationBL();
            try
            {
                if (fabricationChasisSerialProducts == null)
                {
                    fabricationChasisSerialProducts = fabricationBL.GetFabricationProductSerialProductList(fabricationId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(fabricationChasisSerialProducts);
        }
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Fabrication, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult CancelFabrication(int fabricationId = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                if (fabricationId != 0)
                {
                    ViewData["fabricationId"] = fabricationId;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["fabricationId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Fabrication, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult CancelFabrication(long fabricationId, string fabricationNo,int companyBranchId,  string cancelReason)
        {
            ResponseOut responseOut = new ResponseOut();  
            FabricationBL fabricationBL = new FabricationBL();
            FabricationViewModel fabricationViewModel = new FabricationViewModel();
            try
            {
                if (fabricationViewModel != null)
                {
                    fabricationViewModel.FabricationId = fabricationId;
                    fabricationViewModel.FabricationNo = fabricationNo;
                    fabricationViewModel.CancelReason = cancelReason;
                    fabricationViewModel.CreatedBy = ContextUser.UserId;
                    fabricationViewModel.CompanyId = ContextUser.CompanyId;
                    fabricationViewModel.CompanyBranchId = companyBranchId;
                    responseOut = fabricationBL.CancelFabrication(fabricationViewModel);
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
