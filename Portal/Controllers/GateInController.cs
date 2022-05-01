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
    public class GateInController :BaseController
    {
        //
        // GET: /GateIn/

        #region GateIn
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_GateIn, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditGateIn(int gateInId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                if (gateInId != 0)
                {
                    ViewData["gateInId"] = gateInId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["gateInId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_GateIn, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditGateIn(GateInViewModel gateinViewModel, List<GateInProductDetailViewModel> gateinProducts, List<GateInSupportingDocumentViewModel> gateinDocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            GateInBL gateinBL = new GateInBL();
            try
            {
                if (gateinViewModel != null)
                {
                    gateinViewModel.CreatedBy = ContextUser.UserId;
                    gateinViewModel.CompanyId = ContextUser.CompanyId;
                    gateinViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = gateinBL.AddEditGateIn(gateinViewModel, gateinProducts, gateinDocuments);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_GateIn, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListGateIn()
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
        public PartialViewResult GetGateInList(string gateInNo = "", string vendorName = "", string dispatchrefNo = "", string fromDate = "", string toDate = "", string approvalStatus = "",string companyBranch="")
        {
            List<GateInViewModel> gates = new List<GateInViewModel>();
            GateInBL gateInBL = new GateInBL();
            
            try
            {
                gates = gateInBL.GetGateInList(gateInNo, vendorName, dispatchrefNo, fromDate, toDate, ContextUser.CompanyId, approvalStatus, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(gates);
        }
        [HttpGet]
        public PartialViewResult GetGateInPOList(string poNo="", string vendorName="", string refNo="", string fromDate="", string toDate="",string companyBranch="")
        {
            List<POViewModel> pos = new List<POViewModel>();
            GateInBL gateinBL = new GateInBL();
            try
            {
                pos = gateinBL.GetGateInPOList(poNo, vendorName, refNo, fromDate, toDate, ContextUser.CompanyId, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(pos);
        }
        [HttpGet]
        public PartialViewResult GetPOList(string poNo = "", string vendorName = "", string refNo = "", string fromDate = "", string toDate = "")
        {
            List<POViewModel> pOViewModel = new List<POViewModel>();
            GateInBL gateInBL = new GateInBL();
            try
            {

                pOViewModel = gateInBL.GetGateInPOList(poNo, vendorName, refNo, fromDate, toDate, ContextUser.CompanyId,"0");
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(pOViewModel);
        }

        [HttpPost]
        public PartialViewResult GetPOGateInProductList(List<POProductViewModel> poProducts, long poId)
        {
           
            GateInBL gateInBL = new GateInBL();
            try
            {
                if (poProducts == null)
                {
                    poProducts = gateInBL.GetPOGateInProductList(poId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(poProducts);
        }

        [HttpPost]
        public PartialViewResult GetGateInProductList(List<GateInProductDetailViewModel> gateProducts, long gateInId)
        {
            GateInBL gateInBL = new GateInBL();
            
            try
            {
                if (gateProducts == null)
                {
                    gateProducts = gateInBL.GetGateInProductList(gateInId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(gateProducts);
        }

        [HttpPost]
        public PartialViewResult GetGateInSupportingDocumentList(List<GateInSupportingDocumentViewModel> gateDocuments, long gateInId)
        {
            GateInBL gateInBL = new GateInBL();
            try
            {
                if (gateDocuments == null)
                {
                    gateDocuments = gateInBL.GetGateInSupportingDocumentList(gateInId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(gateDocuments);
        }

        public FileResult DocumentDownload(string fileName)
        {
            var path = Path.Combine(Server.MapPath("~/Images/GateInDocument"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpGet]
        public JsonResult GetGateInDetail(long gateinId)
        {
            GateInBL gateBL = new GateInBL();
            GateInViewModel gate = new GateInViewModel();
            try
            {
                gate = gateBL.GetGateInDetail(gateinId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(gate, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Report(long gateinId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            GateInBL gateInBL = new GateInBL();
           
            string path = Path.Combine(Server.MapPath("~/RDLC"), "GateInPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }

            DataTable dt = new DataTable();
            dt = gateInBL.GetGateInDetailDataTable(gateinId);

            //decimal totalInvoiceAmount = 0;
            //totalInvoiceAmount = Convert.ToDecimal(dt.Rows[0]["TotalValue"].ToString());
            //string totalWords = CommonHelper.changeToWords(totalInvoiceAmount.ToString());

            ReportDataSource rd = new ReportDataSource("GateInDetailDataSet", dt);
            ReportDataSource rdProduct = new ReportDataSource("GateInProductDetailDataSet", gateInBL.GetGateInProductListDataTable(gateinId));

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
                        var path = Path.Combine(Server.MapPath("~/Images/GateInDocument"), newFileName);
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
        

        
        

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_MRN, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult CancelGateIn(int gateinId = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                if (gateinId != 0)
                {
                    ViewData["gateinId"] = gateinId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["gateinId"] = 0;
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
        public ActionResult CancelGateIn(long gateinId, string cancelReason)
        {
            ResponseOut responseOut = new ResponseOut();
            GateInBL gateInBL = new GateInBL();
            GateInViewModel gateinViewModel = new GateInViewModel();
            try
            {
                if (gateinViewModel != null)
                {
                    gateinViewModel.GateInId =gateinId;
                    gateinViewModel.CancelReason = cancelReason;
                    gateinViewModel.CreatedBy = ContextUser.UserId;

                    responseOut = gateInBL.CancelGateIn(gateinViewModel);

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
