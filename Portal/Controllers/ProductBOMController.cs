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

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class ProductBOMController : BaseController
    {
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ProductBOM, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditProductBOM(int bomId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (bomId != 0)
                {
                    ViewData["bomId"] = bomId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["bomId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ProductBOM, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditProductBOM(ProductBOMViewModel productBOMViewModel, ProductBomManufacturingExpenseViewModel productBomManufacturingExpenseViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            ProductBOMBL productBOMBL = new ProductBOMBL();
            try
            {
                if (productBOMViewModel != null)
                {
                    productBOMViewModel.CompanyId = ContextUser.CompanyId;
                    productBOMViewModel.CompanyBranchId=Convert.ToInt32( ContextUser.CompanyBranchId);
                    productBOMViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = productBOMBL.AddEditProductBOM(productBOMViewModel, productBomManufacturingExpenseViewModel);
                }
                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
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

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ProductBOM, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult CopyProductBOM(long copyFromAssemblyId, long copyToAssemblyId)
        {
            ResponseOut responseOut = new ResponseOut();
            ProductBOMBL productBOMBL = new ProductBOMBL();
            try
            {
                if (copyFromAssemblyId != 0 && copyToAssemblyId != 0)
                {
                    responseOut = productBOMBL.CopyProductBOM(copyFromAssemblyId, copyToAssemblyId, ContextUser.UserId);
                }
                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ProductBOM, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListProductBOM()
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
        public PartialViewResult GetAssemblyList(string assemblyName, string assemblyType, int companyBranchId = 0)
        {
            List<ProductBOMViewModel> productBOMs = new List<ProductBOMViewModel>();
            ProductBOMBL productBOMBL = new ProductBOMBL();
            try
            {

                productBOMs = productBOMBL.GetAssemblyList(assemblyName, assemblyType, ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(productBOMs);
        }
        [HttpGet]
        public PartialViewResult GetAssemblyBOMList(long assemblyID)
        {
            List<ProductBOMViewModel> productBOMs = new List<ProductBOMViewModel>();
            ProductBOMBL productBOMBL = new ProductBOMBL();
            try
            {

                productBOMs = productBOMBL.GetAssemblyBOMList(assemblyID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(productBOMs);
        }
        [HttpGet]
        public JsonResult GetProductBOMDetail(long bomId)
        {
            ProductBOMBL productBOMBL = new ProductBOMBL();
            ProductBOMViewModel productBOM = new ProductBOMViewModel();
            try
            {
                productBOM = productBOMBL.GetProductBOMDetail(bomId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productBOM, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetProductAutoCompleteList(string term, string assemblyType)
        {
            ProductBL productBL = new ProductBL();
            List<ProductViewModel> productList = new List<ProductViewModel>();
            try
            {
                productList = productBL.GetProductAutoCompleteList(term, ContextUser.CompanyId, assemblyType);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetSubAssemblyAutoCompleteList(string term)
        {
            ProductBL productBL = new ProductBL();
            List<ProductViewModel> productList = new List<ProductViewModel>();
            try
            {
                productList = productBL.GetSubAssemblyAutoCompleteList(term, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetProductMainGroupNameByProductID(long productId)
        {
            ProductBOMBL productBL = new ProductBOMBL();
            ProductViewModel productList = new ProductViewModel();
            try
            {
                productList = productBL.GetProductMainGroupNameByProductID(productId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productList, JsonRequestBehavior.AllowGet);
        }
        [ValidateRequest(true, UserInterfaceHelper.ProductBOM_Report, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListProductBOMReport()
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
        public ActionResult Report(long assemblyId, string assemblyType, int companyBranchId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            ProductBOMBL productBOMBL = new ProductBOMBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "ProductBOMReports.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("PrintStockLedger");
            }

            DataTable dt = new DataTable();
            dt = productBOMBL.GetBOMListReport(assemblyId, assemblyType, ContextUser.CompanyId, companyBranchId);
            ReportDataSource rd = new ReportDataSource("ProductBOMDataSet", dt);
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
        public JsonResult GetLabourWagesofassemblyId(long assemblyId)
        {
            ProductBOMBL productBL = new ProductBOMBL();
            ProductBomManufacturingExpenseViewModel productBomManufacturingExpenseList = new ProductBomManufacturingExpenseViewModel();
            try
            {
                productBomManufacturingExpenseList = productBL.GetLabourWagesofassemblyId(assemblyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productBomManufacturingExpenseList, JsonRequestBehavior.AllowGet);
        }
    }
}
