using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.IO;

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class MRNController : BaseController
    {
      
        #region MRN
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_MRN, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditMRN(int mrnId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                if (mrnId != 0)
                {
                    ViewData["mrnId"] = mrnId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["mrnId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_MRN, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditMRN(MRNViewModel mrnViewModel, List<MRNProductDetailViewModel> mrnProducts, List<MRNSupportingDocumentViewModel> mrnDocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            MRNBL mrnBL = new MRNBL();
            try
            {
                if (mrnViewModel != null)
                {
                    mrnViewModel.CreatedBy = ContextUser.UserId;
                    mrnViewModel.CompanyId = ContextUser.CompanyId;
                    mrnViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = mrnBL.AddEditMRN(mrnViewModel, mrnProducts, mrnDocuments);
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_MRN, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListMRN()
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


        [HttpGet]
        public PartialViewResult GetMRNList(string mrnNo = "", string vendorName = "", string dispatchrefNo = "", string fromDate = "", string toDate = "", string approvalStatus = "",string companyBranch="")
        {
            List<MRNViewModel> mrns = new List<MRNViewModel>();
            MRNBL mrnBL = new MRNBL();
            try
            {
                mrns = mrnBL.GetMRNList(mrnNo, vendorName, dispatchrefNo, fromDate, toDate, ContextUser.CompanyId, approvalStatus, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(mrns);
        }
        [HttpGet]
        public PartialViewResult GetPurchaseInvoiceList(string piNo="", string vendorName="", string refNo="", string fromDate="", string toDate="", string approvalStatus = "", string displayType = "", string purchasetype ="0",string companyBranch="")
        {
            List<PurchaseInvoiceViewModel> invoices = new List<PurchaseInvoiceViewModel>();
            
            PurchaseInvoiceBL purchaseInvoiceBL = new PurchaseInvoiceBL();
            try
            {
                
               invoices = purchaseInvoiceBL.GetPIList(piNo,vendorName, refNo, fromDate, toDate, ContextUser.CompanyId,approvalStatus, displayType,"", purchasetype,  "","" ,companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(invoices);
        }

        [HttpPost]
        public PartialViewResult GetMRNProductList(List<MRNProductDetailViewModel> mrnProducts, long mrnId)
        {
            MRNBL mrnBL = new MRNBL();
            try
            {
                if (mrnProducts == null)
                {
                    mrnProducts = mrnBL.GetMRNProductList(mrnId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(mrnProducts);
        }

        [HttpGet]
        public JsonResult GetMRNDetail(long mrnId)
        {
            MRNBL mrnBL = new MRNBL();
            MRNViewModel mrn = new MRNViewModel();
            try
            {
                mrn = mrnBL.GetMRNDetail(mrnId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(mrn, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Report(long mrnId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            MRNBL mrnBL = new MRNBL();
            PurchaseInvoiceBL purchaseInvoiceBL = new PurchaseInvoiceBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "MRNPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }

            DataTable dt = new DataTable();
            dt = mrnBL.GetMRNDetailDataTable(mrnId);

            //decimal totalInvoiceAmount = 0;
            //totalInvoiceAmount = Convert.ToDecimal(dt.Rows[0]["TotalValue"].ToString());
            //string totalWords = CommonHelper.changeToWords(totalInvoiceAmount.ToString());

            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            ReportDataSource rdProduct = new ReportDataSource("DataSetProduct",mrnBL.GetMRNProductListDataTable(mrnId));
            
            lr.DataSources.Add(rd);
            lr.DataSources.Add(rdProduct);
            

            //ReportParameter rp1 = new ReportParameter("AmountInWords", totalWords);
            //ReportParameter rp2 = new ReportParameter("ReportOption", reportOption);
            //lr.SetParameters(rp1);
            //lr.SetParameters(rp2);

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

        [HttpPost]
        public ActionResult SaveSupportingDocument()
        {
            ResponseOut responseOut = new ResponseOut();
            HttpFileCollectionBase files = Request.Files;
            Random rnd = new Random();
            try
            {
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
                        string newFileName = "";
                        var fileName = Path.GetFileName(file.FileName);
                        newFileName = Convert.ToString(rnd.Next(10000, 99999)) + "_" + fileName;
                        var path = Path.Combine(Server.MapPath("~/Images/MRNDocument"), newFileName);
                        file.SaveAs(path);
                        responseOut.status = ActionStatus.Success;
                        responseOut.message = newFileName;
                    }
                    else
                    {
                        responseOut.status = ActionStatus.Fail;
                        responseOut.message = "";
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
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }
        public FileResult DocumentDownload(string fileName)
        {
            var path = Path.Combine(Server.MapPath("~/Images/MRNDocument"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        public PartialViewResult GetMRNSupportingDocumentList(List<MRNSupportingDocumentViewModel> mrnDocuments, long mrnId)
        {

          
            MRNBL mRNBL = new MRNBL();
            try
            {
                if (mrnDocuments == null)
                {
                    mrnDocuments = mRNBL.GetMRNSupportingDocumentList(mrnId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(mrnDocuments);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_MRN, (int)AccessMode.CancelAccess, (int)RequestMode.GetPost)]
        public ActionResult CancelMRN(int mrnId = 0, int accessMode = 4)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                if (mrnId != 0)
                {
                    ViewData["mrnId"] = mrnId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["mrnId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_MRN, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult CancelMRN(long mrnId, string cancelReason,int companyBranchId)
        {
            ResponseOut responseOut = new ResponseOut();
            MRNBL mrnBL = new MRNBL();
            MRNViewModel mrnViewModel = new MRNViewModel();
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                if (mrnViewModel != null)
                {
                    mrnViewModel.MRNId = mrnId;
                    mrnViewModel.CancelReason = cancelReason;
                    mrnViewModel.CreatedBy = ContextUser.UserId;
                    mrnViewModel.CompanyBranchId = companyBranchId;
                    mrnViewModel.CompanyId = ContextUser.CompanyId;
                    mrnViewModel.FinYearId = finYear.FinYearId;
                    responseOut = mrnBL.CancelMRN(mrnViewModel);
                    
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
