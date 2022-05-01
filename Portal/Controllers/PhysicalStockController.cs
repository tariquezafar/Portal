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
using System.Globalization;
using System.Reflection;
using System.IO;
using Microsoft.Reporting.WebForms;

namespace Portal.Controllers
{
    public class PhysicalStockController : BaseController
    {
        #region Physical Stock

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PhysicalStock, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult AddEditPhysicalStock(int physicalStockID = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                ViewData["monthStartDate"] = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd-MMM-yyyy");
                ViewData["monthEndDate"] = new DateTime(DateTime.Now.Year, DateTime.Now.Month, daysInMonth).ToString("dd-MMM-yyyy");
                if (physicalStockID != 0)
                {
                    ViewData["physicalStockID"] = physicalStockID;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["physicalStockID"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PhysicalStock, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditPhysicalStock(PhysicalStockViewModel physicalStockViewModel, List<PhysicalStockProductDetailViewModel> physicalStockProductDetails)
        {
            ResponseOut responseOut = new ResponseOut();          
            PhysicalStockBL physicalStockBL = new PhysicalStockBL();
            try
            {
                if (physicalStockViewModel != null)
                {
                    physicalStockViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = physicalStockBL.AddEditPhysicalStock(physicalStockViewModel, physicalStockProductDetails);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PhysicalStock, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListPhysicalStock()
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpPost]
        public PartialViewResult SavePhysicalStockDetail()
        {
           
            UploadUtilityBL utilityBL = new UploadUtilityBL();
             List<PhysicalStockProductDetailViewModel> physicalStockProductDetailList = new List<PhysicalStockProductDetailViewModel>();
            ResponseOut responseOut = new ResponseOut();
            HttpFileCollectionBase files = Request.Files;
            int companyBranchId = Convert.ToInt32(Request.Form["companyBranchId"].ToString());
            string todate = Convert.ToString(Request.Form["todate"].ToString());           
            Random rnd = new Random();
            try
            {
                           
                if (files != null && files.Count > 0 && Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                {
                    HttpPostedFileBase file = files[0];
                    string fname;                 
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
                        string extension = System.IO.Path.GetExtension(fname).ToLower();                       
                        string connString = "";
                        string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                        string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/PhysicalStock"), fname);
                        if (!Directory.Exists(path1))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/Content/PhysicalStock"));
                        }
                        if (validFileTypes.Contains(extension))
                        {
                            DataTable dt = new DataTable();
                            if (System.IO.File.Exists(path1))
                            { System.IO.File.Delete(path1); }
                            file.SaveAs(path1);
                            if (extension == ".csv")
                            {
                                dt = CommonHelper.ConvertCSVtoDataTable(path1);
                                ViewBag.Data = dt;
                            }
                             
                            else if (extension.Trim() == ".xls")
                            {
                                connString = "Provider=Microsoft.ACE.OLEDB.8.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                                dt = CommonHelper.ConvertXSLXtoDataTable(path1, connString);
                                ViewBag.Data = dt;
                            }
                            else if (extension.Trim() == ".xlsx")
                            {
                                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                                dt = CommonHelper.ConvertXSLXtoDataTable(path1, connString);
                                ViewBag.Data = dt;
                            }
                           
                            StringBuilder strErrMsg = new StringBuilder(" ");

                            if (dt.Rows.Count > 0)
                            {
                                int UOMId = 0;
                                int productTypeId = 0;
                                int rowCounter = 1;
                                int productMainGroupID = 0;
                                int productSubGroupID = 0;
                                decimal availableStock = 0;
                                long productid = 0;                             
                                PhysicalStockProductDetailViewModel physicalStockProductDetailViewModel;                               
                                dt.Columns.Add("UploadStatus", typeof(bool));
                                bool rowVerifyStatus = true;
                                foreach (DataRow dr in dt.Rows)
                                {
                                    physicalStockProductDetailViewModel = new PhysicalStockProductDetailViewModel();                                   
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ProductName"]).Trim()))
                                    { 
                                        strErrMsg.Append("Product Name not proper format. " + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ProductCode"]).Trim()))
                                    {
                                        strErrMsg.Append("Product Code not proper format. " + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ProductTypeName"]).Trim()))
                                    {

                                        dr["ProductTypeName"] ="";
                                        //strErrMsg.Append("Product Type not proper format. " + rowCounter.ToString() + Environment.NewLine);
                                        //rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ProductMainGroupName"]).Trim()))
                                    {
                                        strErrMsg.Append("Product Main Group Name not proper format. " + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ProductSubGroupName"]).Trim()))
                                    {
                                        strErrMsg.Append("Product Sub Group Name not proper format. " + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["UOMName"]).Trim()))
                                    {
                                        strErrMsg.Append("UOM Name not proper format. " + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["PhysicalStockQty"]).Trim()))                                        
                                    {

                                        dr["PhysicalStockQty"] = 0;
                                        //strErrMsg.Append("Physical Stock QTY not proper format. " + rowCounter.ToString() + Environment.NewLine);
                                        //rowVerifyStatus = false;
                                    }
                                    
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["ProductName"])))
                                    {
                                        productid = utilityBL.GetIdByAllProductName(Convert.ToString(dr["ProductName"]));
                                        if (productid == 0)
                                        {
                                            strErrMsg.Append("Invalid Product Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                       
                                    }


                                   

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["ProductTypeName"])))
                                    {
                                        productTypeId = utilityBL.GetIdByProductTypeName(Convert.ToString(dr["ProductTypeName"]));
                                        if (productTypeId == 0)
                                        {
                                            //strErrMsg.Append("Invalid Product Type Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            //rowVerifyStatus = false;

                                        }
                                    }
                                   
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["ProductMainGroupName"])))
                                    {
                                        productMainGroupID = utilityBL.GetIdByProductMainGroupName(Convert.ToString(dr["ProductMainGroupName"]));
                                        if (productMainGroupID == 0)
                                        {
                                            strErrMsg.Append("Invalid Product MainGroup Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter MainGroup Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["ProductSubGroupName"])))
                                    {
                                        productSubGroupID = utilityBL.GetIdByProductSubGroupName(Convert.ToString(dr["ProductSubGroupName"]));
                                        if (productSubGroupID == 0)
                                        {
                                            strErrMsg.Append("Invalid Product SubGroup Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["UOMName"])))
                                    {
                                        UOMId = utilityBL.GetIdByUOMName(Convert.ToString(dr["UOMName"]));
                                        if (UOMId == 0)
                                        {
                                            strErrMsg.Append("Invalid UOM Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                            rowVerifyStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        strErrMsg.Append("Please Enter UOM Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;

                                    }
                                    FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                                    string   formdate = finYear.StartDate;                                    
                                    int companyId = ContextUser.CompanyId;
                                    int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                                    PhysicalStockBL physicalStockBL = new PhysicalStockBL();
                                    availableStock = physicalStockBL.GetProductSystemStock(productid, finYearId, companyId, companyBranchId, formdate, todate);

                                    if (rowVerifyStatus == true)
                                    {
                                       
                                        physicalStockProductDetailList.Add(new PhysicalStockProductDetailViewModel
                                        {
                                            Productid =Convert.ToInt32(productid),
                                            ProductName = string.IsNullOrEmpty(Convert.ToString(dr["ProductName"]).Trim()) ? "" : Convert.ToString(dr["ProductName"]).Trim(),
                                            ProductTypeId =Convert.ToInt32(productTypeId),
                                            ProductTypeName = string.IsNullOrEmpty(Convert.ToString(dr["ProductTypeName"])) ? "" : Convert.ToString(dr["ProductTypeName"].ToString().Replace(",", "")),
                                            ProductMainGroupId = Convert.ToInt32(productMainGroupID),
                                            ProductCode = string.IsNullOrEmpty(Convert.ToString(dr["ProductCode"])) ? "" : Convert.ToString(dr["ProductCode"]).ToString().Trim(),
                                            ProductMainGroupName = string.IsNullOrEmpty(Convert.ToString(dr["ProductMainGroupName"])) ? "" : Convert.ToString(dr["ProductMainGroupName"]).ToString().Trim(),
                                            ProductSubGroupName = string.IsNullOrEmpty(Convert.ToString(dr["ProductSubGroupName"])) ? "" : Convert.ToString(dr["ProductSubGroupName"]).ToString().Trim(),
                                            ProductSubGroupId = Convert.ToInt32(productSubGroupID),
                                            UOMId = Convert.ToInt32(UOMId),
                                            UOMName = string.IsNullOrEmpty(Convert.ToString(dr["UOMName"])) ? "" : Convert.ToString(dr["UOMName"]).ToString().Trim(),
                                            SystemQTY = Convert.ToDecimal(availableStock),
                                            PhysicalQTY =  Convert.ToDecimal(dr["PhysicalStockQty"]),

                                        });
                                        dr["UploadStatus"] = true;

                                    }
                                    else
                                    {
                                        dr["UploadStatus"] = false;


                                    }
                                    rowCounter += 1;

                                }
                                dt.AcceptChanges();
                            }
                            else
                            {
                                strErrMsg.Append("Import not found");                                
                            }

                            ViewBag.Error = strErrMsg.ToString();                          
                        }
                        else
                        {
                            ViewBag.Error = "Please Upload Files in .xls, .xlsx or .csv format";

                        }

                    }
                }
                else
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = "";
                }
            }
            catch (Exception ex)
            {
                responseOut.message = "";
                responseOut.status = ActionStatus.Fail;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView("GetPhysicalStockDetailList", physicalStockProductDetailList);
        }

        [HttpPost]
        public PartialViewResult GetPhysicalStockDetailList(List<PhysicalStockProductDetailViewModel> physicalStockProductDetail, int physicalStockID = 0)
        {                      
            PhysicalStockBL physicalStockBL = new PhysicalStockBL();
            try
            {
                if (physicalStockProductDetail == null)
                {
                    physicalStockProductDetail = physicalStockBL.GetPhysicalStockDetailList(physicalStockID);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(physicalStockProductDetail);
        }

        public JsonResult GetPhysicalStockDetail(int physicalStockID)
        {           
            PhysicalStockBL physicalStockBL = new PhysicalStockBL();
            PhysicalStockViewModel physicalStockViewModel = new PhysicalStockViewModel();
            try
            {
                physicalStockViewModel = physicalStockBL.GetPhysicalStockDetail(physicalStockID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(physicalStockViewModel, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetPhysicalStockList(string physicalStockNo = "", int companyBranchId = 0, string fromDate = "", string toDate = "", string approvalStatus = "")
        {
            List<PhysicalStockViewModel> physicalStockViewModel = new List<PhysicalStockViewModel>();
            PhysicalStockBL physicalStockBL = new PhysicalStockBL();
            try
            {
                physicalStockViewModel = physicalStockBL.GetPhysicalStockList(physicalStockNo,  companyBranchId, fromDate, toDate, ContextUser.CompanyId, approvalStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(physicalStockViewModel);
        }
        public ActionResult PhysicalStockReport(int companyBranchId= 0, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            ProductBL productBL = new ProductBL();
            int companyId = ContextUser.CompanyId;
            FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
            string path = Path.Combine(Server.MapPath("~/RDLC"), "PhysicalStockReports.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            ReportDataSource rd = new ReportDataSource("PhysicalStockDataSet", productBL.GeneratePhysicalStock(companyId));
            lr.DataSources.Add(rd);

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


        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Report(long physicalStockID, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            PhysicalStockBL physicalStockBL = new PhysicalStockBL();

            string path = Path.Combine(Server.MapPath("~/RDLC"), "PhysicalStockPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }




            ReportDataSource rd = new ReportDataSource("DataSet1", physicalStockBL.GetPhysicalStockDetailReport(Convert.ToInt32(physicalStockID)));
            ReportDataSource rdProduct = new ReportDataSource("DataSet2", physicalStockBL.GetPhysicalStockDetailListReport(Convert.ToInt32(physicalStockID)));
            lr.DataSources.Add(rd);
            lr.DataSources.Add(rdProduct);
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo = "<DeviceInfo>" +
                        "  <OutputFormat>" + reportType + "</OutputFormat>" +
                        "  <PageWidth>8.5in</PageWidth>" +
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
        #endregion
    }
}
