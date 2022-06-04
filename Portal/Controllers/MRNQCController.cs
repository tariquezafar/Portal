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
    public class MRNQCController : BaseController
    {        
        #region MRN QC

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_MRNQC, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditMRNQC(int mrnId = 0, int accessMode = 3)
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_MRNQC, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditMRNQC(MRNViewModel mrnViewModel, List<MRNProductDetailViewModel> mrnProducts, List<MRNSupportingDocumentViewModel> mrnDocuments)
        {
            ResponseOut responseOut = new ResponseOut();          
            MRNQCBL mRNQCBL = new MRNQCBL();
            try
            {
                if (mrnViewModel != null)
                {
                    mrnViewModel.CreatedBy = ContextUser.UserId;
                    mrnViewModel.CompanyId = ContextUser.CompanyId;
                    mrnViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = mRNQCBL.AddEditMRNQC(mrnViewModel, mrnProducts, mrnDocuments);
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_MRNQCLIST, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListMRNQC()
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
        public PartialViewResult GetMRNQCList(string mrnNo = "",string qCNO="", string vendorName = "", string dispatchrefNo = "", string fromDate = "", string toDate = "", string approvalStatus = "",string companyBranch="",int locationID = 0)
        {
            List<MRNViewModel> mrns = new List<MRNViewModel>();
            MRNQCBL mRNQCBL = new MRNQCBL();
            try
            {
                mrns = mRNQCBL.GetMRNQCList(mrnNo, qCNO, vendorName, dispatchrefNo, fromDate, toDate, ContextUser.CompanyId, approvalStatus, companyBranch, locationID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(mrns);
        }
        [HttpGet]
        public PartialViewResult GetQCList(string qcNo = "", string gateInNo = "", string refNo="", string fromDate="", string toDate="",string companyBranch="")
        {
            List<QualityCheckViewModel> qcOViewModel = new List<QualityCheckViewModel>();
            MRNQCBL mRNQCBL = new MRNQCBL();
            try
            {

                qcOViewModel = mRNQCBL.GetQCList(qcNo, gateInNo, fromDate, toDate, ContextUser.CompanyId, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(qcOViewModel);
        }

        [HttpPost]
        public PartialViewResult GetMRNQCProductList(List<MRNProductDetailViewModel> mrnProducts, long mrnId)
        {           
            MRNQCBL mRNQCBL = new MRNQCBL();
            try
            {
                if (mrnProducts == null)
                {
                    mrnProducts = mRNQCBL.GetMRNProductList(mrnId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(mrnProducts);
        }

        [HttpGet]
        public JsonResult GetMRNQCDetail(long mrnId)
        {
         
            MRNViewModel mrn = new MRNViewModel();          
            MRNQCBL mRNQCBL = new MRNQCBL();
            try
            {
                mrn = mRNQCBL.GetMRNQCDetail(mrnId);
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
            MRNPOBL mRNPOBL = new MRNPOBL();           
            string path = Path.Combine(Server.MapPath("~/RDLC"), "MRNPOPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }

            DataTable dt = new DataTable();
            dt = mRNPOBL.GetMRNDetailDataTable(mrnId);
          
            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            ReportDataSource rdProduct = new ReportDataSource("DataSet2", mRNPOBL.GetMRNProductListDataTable(mrnId));
            
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

        [HttpPost]
        public PartialViewResult GetQCMRNProductList(List<QualityCheckProductDetailViewModel> qcProducts, long qualityCheckId)
        {            
            MRNQCBL mRNQCBL = new MRNQCBL();
            try
            {
                if (qcProducts == null)
                {
                    qcProducts = mRNQCBL.GetQCMRNProductList(qualityCheckId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(qcProducts);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_MRN, (int)AccessMode.CancelAccess, (int)RequestMode.GetPost)]
        public ActionResult CancelMRNPO(int mrnId = 0, int accessMode = 4)
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
        public ActionResult CancelMRNPO(long mrnId,long pOId, string cancelReason,List<MRNProductDetailViewModel> mrnProductLists)
        {
            ResponseOut responseOut = new ResponseOut();
            MRNPOBL mRNPOBL = new MRNPOBL();
            MRNViewModel mrnViewModel = new MRNViewModel();
            try
            {
                if (mrnViewModel != null)
                {
                    mrnViewModel.MRNId = mrnId;
                    mrnViewModel.POId = pOId;
                    mrnViewModel.CancelReason = cancelReason;
                    mrnViewModel.CreatedBy = ContextUser.UserId;

                    responseOut = mRNPOBL.CancelMRNPO(mrnViewModel, mrnProductLists);

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
