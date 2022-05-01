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
    public class PackingMaterialBOMController : BaseController
    {
        #region Packing Material BOM

        //Packing Material BOM       
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PackingMaterialBOM, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditPackingMaterialBOM(int pMBId = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (pMBId != 0)
                {
                    ViewData["pMBId"] = pMBId;
                    ViewData["accessMode"] = accessMode;
                    
                }
                else
                {
                    ViewData["pMBId"] = 0;
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

        /*Save form data with products */
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PackingMaterialBOM, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditPackingMaterialBOM(PackingMaterialBOMViewModel packingMaterialBOMViewModel, List<PackingMaterialBOMProductViewModel> packingMaterialBOMProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            PackingMaterialBOMBL packingMaterialBOMBL = new PackingMaterialBOMBL();
            try
            {
                if (packingMaterialBOMViewModel != null)
                {
                    packingMaterialBOMViewModel.CreatedBy = ContextUser.UserId;
                    packingMaterialBOMViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = packingMaterialBOMBL.AddEditPackingMaterialBOM(packingMaterialBOMViewModel, packingMaterialBOMProducts);

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PackingMaterialBOM, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListPackingMaterialBOM()
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

        /*Get packing material BOM List*/
        [HttpGet]
        public PartialViewResult GetPackingMaterialBOMList(string pMBNo = "", int packingListTypeId = 0, int productSubGroupId=0, string fromDate = "", string toDate = "",int companyBranchId=0)
        {
            List<PackingMaterialBOMViewModel> packingMaterialBOMs = new List<PackingMaterialBOMViewModel>();
            PackingMaterialBOMBL packingMaterialBOMBL = new PackingMaterialBOMBL();
            try
            {
                packingMaterialBOMs = packingMaterialBOMBL.GetPackingMaterialBOMList(pMBNo, packingListTypeId, productSubGroupId, fromDate, toDate, ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(packingMaterialBOMs);
        }

        /*Get packing material BOM Details BY Id*/
        [HttpGet]
        public JsonResult PackingMaterialBOMDetail(long pMBId)
        {
            PackingMaterialBOMBL packingMaterialBOMBL = new PackingMaterialBOMBL();
            PackingMaterialBOMViewModel packingMaterialBOM = new PackingMaterialBOMViewModel();
            try
            {
                packingMaterialBOM = packingMaterialBOMBL.GetPackingMaterialBOMDetail(pMBId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(packingMaterialBOM, JsonRequestBehavior.AllowGet);
        }

        /*Get packing material BOM proudct list*/
        [HttpPost]
        public PartialViewResult GetPackingMaterialBOMProductList(List<PackingMaterialBOMProductViewModel> packingMaterialBOMProducts, long pMBId)
        {
            PackingMaterialBOMBL packingMaterialBOMBL = new PackingMaterialBOMBL();
            try
            {
                if (packingMaterialBOMProducts == null)
                {
                    packingMaterialBOMProducts = packingMaterialBOMBL.GetPackingMaterialBOMProductList(pMBId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(packingMaterialBOMProducts);
        }
        
        /*Generate Packing Material BOM Report*/
        public ActionResult Report(long pMBId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            PackingMaterialBOMBL packingMaterialBOMBL = new PackingMaterialBOMBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "PackingMaterialBOMReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            DataTable dt = new DataTable();
            dt = packingMaterialBOMBL.GetPackingMaterialBOMDataTable(pMBId);
            ReportDataSource rd = new ReportDataSource("PackingMaterialBOMDataSet", dt);
            ReportDataSource rdProduct = new ReportDataSource("PackingMaterialBOMProductsDataSet", packingMaterialBOMBL.GetPackingMaterialBOMProductListDataTable(pMBId));
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
        public JsonResult GetProductSubGroupListForPMB()
        {
            PackingMaterialBOMBL packingMaterialBOMBL = new PackingMaterialBOMBL();
            List<ProductSubGroupViewModel> allProductSubGroup = new List<ProductSubGroupViewModel>();
            try
            {
                allProductSubGroup = packingMaterialBOMBL.GetProductSubGroupListForPMB();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(allProductSubGroup, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
