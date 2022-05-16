using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;
using System.IO;
using Microsoft.Reporting.WebForms;

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class ProductController : BaseController
    {
        //
        // GET: /Product/
        //
        // GET: /User/
        
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Product, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditProduct(int productId = 0, int accessMode = 3)
        {

            try
            {
                if (productId != 0)
                {
                    ViewData["productId"] = productId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["productId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Product, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditProduct(ProductViewModel productViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            ProductBL productBL = new ProductBL();
            try
            {
                if (productViewModel != null)
                {
                    productViewModel.CompanyId = ContextUser.CompanyId;
                    productViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = productBL.AddEditProduct(productViewModel);
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



        [HttpGet]
        public JsonResult GetSizeAutoCompleteList(string term, int productMainGroupId = 0, int productSubGroupId = 0)
        {
            SizeBL sizeBL = new SizeBL();
            List<SizeViewModel> sizeList = new List<SizeViewModel>();
            try
            {
                sizeList = sizeBL.GetSizeAutoCompleteList(term, productMainGroupId, productSubGroupId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(sizeList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetManufacturerAutoCompleteList(string term)
        {
            ManufacturerBL manufacturerBL = new ManufacturerBL();
            List<ManufacturerViewModel> manufacturerList = new List<ManufacturerViewModel>();
            try
            {
                manufacturerList = manufacturerBL.GetManufacturerAutoCompleteList(term);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(manufacturerList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateProductPic()
        {
            ResponseOut responseOut = new ResponseOut();
            ProductBL productBL = new ProductBL();
            HttpFileCollectionBase files = Request.Files;
            ProductViewModel productViewModel = new ProductViewModel();
            try
            {
                productViewModel.Productid = Convert.ToInt32(Request["productId"]);
                //  Get all files from Request object  
                if (files != null && files.Count > 0 && Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                {
                    HttpPostedFileBase file = files[0];
                    string fname;
                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        productViewModel.ProductPicName = productViewModel.Productid.ToString() + "_" + fileName;
                        var path = Path.Combine(Server.MapPath("~/Images/ProductImages"), productViewModel.ProductPicName);
                        file.SaveAs(path);
                        productViewModel.ProductPicPath = path;

                        //queryDetail.QueryAttachment = new byte[file.ContentLength];
                        //file.InputStream.Read(queryDetail.QueryAttachment, 0, file.ContentLength);
                    }
                }

                if (productViewModel != null && !string.IsNullOrEmpty(productViewModel.ProductPicPath))
                {
                    responseOut = productBL.UpdateProductPic(productViewModel);
                }
                else
                {
                    responseOut.message = "";
                    responseOut.status = ActionStatus.Success;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Product, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListProduct(string productListStatus="false",string mTDproduct= "")
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["todayproduct"] = productListStatus;
                ViewData["mTDproduct"] = mTDproduct;
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpGet]
        public PartialViewResult GetProductList(string productName, string productCode, string productShortDesc, string productFullDesc, int productTypeId, int productMainGroupId, int productSubGroupId = 0, string assemblyType = "MA", string brandName = "", string fromDate = "", string toDate = "" , string modelName = "", string hsnCode = "", string cc = "", string vendorName = "", string vendorCode = "", string localName = "", string compatibility = "", int companyBranchId=0)
        {
            List<ProductViewModel> products = new List<ProductViewModel>();
            ProductBL productBL = new ProductBL();
            try
            {

                products = productBL.GetProductList(productName, ContextUser.CompanyId, productCode, productShortDesc, productFullDesc, productTypeId, productMainGroupId, productSubGroupId, assemblyType, brandName, fromDate, toDate,modelName,hsnCode,cc, vendorName, vendorCode,localName, compatibility, companyBranchId);
            }
            catch (Exception ex)
                {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(products);
        }

        [HttpGet]
        public JsonResult GetProductDetail(long productid)
        {
            ProductBL productBL = new ProductBL();
            ProductViewModel product = new ProductViewModel();
            try
            {
                product = productBL.GetProductDetail(productid);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(product, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAutoCompleteProductDetail(long productid)
        {
            ProductBL productBL = new ProductBL();
            ProductViewModel product = new ProductViewModel();
            try
            {
                product = productBL.GetAutoCompleteProductDetail(productid);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(product, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetProductAutoCompleteList(string term)
        {
            ProductBL productBL = new ProductBL();
            List<ProductViewModel> productList = new List<ProductViewModel>();
            try
            {
                productList = productBL.GetProductAutoCompleteList(term,ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetProductSubGroupAutoCompleteList(string term)
        {
            ProductBL productSubGroupBL = new ProductBL();
            List<ProductSubGroupViewModel> productSubGroupList = new List<ProductSubGroupViewModel>();
            try
            {
                productSubGroupList = productSubGroupBL.GetProductSubGroupAutoCompleteList(term, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productSubGroupList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetProductTypeList()
        {
            ProductTypeBL productTypeBL = new ProductTypeBL();
            List<ProductTypeViewModel> productTypeList = new List<ProductTypeViewModel>();
            try
            {
                productTypeList = productTypeBL.GetProductTypeList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productTypeList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetProductMainGroupList()
        {
            ProductMainGroupBL productMainGroupBL = new ProductMainGroupBL();
            List<ProductMainGroupViewModel> productMainGroupList = new List<ProductMainGroupViewModel>();
            try
            {
                productMainGroupList = productMainGroupBL.GetProductMainGroupList();
            
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productMainGroupList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetProductSubGroupList(int productMainGroupId)
        {
            ProductSubGroupBL productSubGroupBL = new ProductSubGroupBL();
            List<ProductSubGroupViewModel> productSubGroupList = new List<ProductSubGroupViewModel>();
            try
            {
                productSubGroupList = productSubGroupBL.GetProductSubGroupList(productMainGroupId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productSubGroupList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetUOMList()
        {
            UOMBL uomBL = new UOMBL();
            List<UOMViewModel> uomList = new List<UOMViewModel>();
            try
            {
                uomList = uomBL.GetUOMList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(uomList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetFinYearList()
        {
            FinYearBL finYearBL = new FinYearBL();
            List<FinYearViewModel> finYearList = new List<FinYearViewModel>();
            try
            {
                finYearList = finYearBL.GetFinancialYearList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(finYearList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetProductAvailableStock(long productid, int companyBranchId, int trnId, string trnType)
        {
            StockLedgerBL stockBL = new StockLedgerBL();
            decimal availableStock = 0;
            try
            {
                int companyId = ContextUser.CompanyId;
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;

                availableStock = stockBL.GetProductAvailableStock(productid, finYearId,companyId,companyBranchId,trnId,trnType);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(availableStock, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetProductAvailableStockBranchWise(long productid, int companyBranchId, int trnId, string trnType)
        {
            StockLedgerBL stockBL = new StockLedgerBL();
            decimal availableStock = 0;
            try
            {
                int companyId = ContextUser.CompanyId;
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;

                availableStock = stockBL.GetProductAvailableStockBranchWise(productid, finYearId, companyId, companyBranchId, trnId, trnType);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(availableStock, JsonRequestBehavior.AllowGet);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ProductReorder, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListProductReorderQuantity( string pendingStatus="")
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                ViewData["pendingStatus"] = pendingStatus;
                ViewData["indentByUser"] = ContextUser.UserId;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        } 
        [HttpGet]
        public PartialViewResult GetProductReorderQuantityList(string productName, string productCode, string productShortDesc, string productFullDesc)
        {
            List<ReorderPointProductCountViewModel> products = new List<ReorderPointProductCountViewModel>();
            ProductBL productBL = new ProductBL();
            try
            {
                
                 int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                products = productBL.GetReorderPointProductCountList(productName, productCode, productShortDesc, productFullDesc, ContextUser.CompanyId,finYearId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(products);
        }

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Product, (int)AccessMode.EditAccess, (int)RequestMode.Ajax)]
        public JsonResult RemoveImage(long productId)
        {
            ResponseOut responseOut = new ResponseOut();
            ProductBL productBL = new ProductBL();
            try
            {
                if (productId != 0)
                { 
                    responseOut = productBL.RemoveImage(productId);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ProductReport, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListProductOpeningStockReport()
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
        public ActionResult GenerateProductsOpeningStockReports(int companyBranchId,string CompanyBranch, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            ProductBL productBL = new ProductBL();
            int companyId = ContextUser.CompanyId;
            FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
            int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
            string path = Path.Combine(Server.MapPath("~/RDLC"), "ProductsOpeningStockReports.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            string CompanyBranchl = "";
            if (CompanyBranch == "-Select Company Branch-")
            {
                CompanyBranchl = "All Company Branch";
            }
            else
            {
                CompanyBranchl = CompanyBranch;
            }
            ReportDataSource rd = new ReportDataSource("DataSet1", productBL.GenerateProductsOpeningStockReports(companyId, companyBranchId, finYearId));
            ReportParameter rp5 = new ReportParameter("CompanyBranch", CompanyBranchl);
            lr.DataSources.Add(rd);
            lr.SetParameters(rp5);
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =

             "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>24.6in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.50in</MarginTop>" +
            "  <MarginLeft>.1in</MarginLeft>" +
            "  <MarginRight>.1in</MarginRight>" +
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
        public JsonResult GetProductTypeBYProductAutoCompleteList(string term, int productTypeId = 0)
        {
            ProductBL productBL = new ProductBL();
            List<ProductViewModel> productList = new List<ProductViewModel>();
            try
            {
                productList = productBL.GetProductTypeBYProductAutoCompleteList(term, ContextUser.CompanyId, productTypeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public string UpdateAndCancelOnlineCode(long productid,string status= "")
        {
            ProductBL productBL = new ProductBL();
            string str = "";
            try
            {
                str = productBL.UpdateAndCancelOnlineCode(productid, status);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return str;
        }

    }
}
