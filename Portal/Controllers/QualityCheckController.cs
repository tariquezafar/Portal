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
    public class QualityCheckController : BaseController
    {
        //
        // GET: /QualityCheck/

        #region QualityCheck
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_QualityCheck, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditQualityCheck(int qualityCheckId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                if (qualityCheckId != 0)
                {
                    ViewData["qualityCheckId"] = qualityCheckId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["qualityCheckId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_QualityCheck, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditQualityCheck(QualityCheckViewModel qcViewModel, List<QualityCheckProductDetailViewModel> qcProductList, List<QualityCheckSupportingDocumentViewModel> qcDocumentList)
        {
            ResponseOut responseOut = new ResponseOut();           
            QualityCheckBL qualityCheckBL = new QualityCheckBL();
            try
            {
                if (qcViewModel != null)
                {
                    qcViewModel.CreatedBy = ContextUser.UserId;
                    qcViewModel.CompanyId = ContextUser.CompanyId;
                    qcViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = qualityCheckBL.AddEditQualityCheck(qcViewModel, qcProductList, qcDocumentList);
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_QualityCheck, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListQualityCheck(string pendingStatus="0")
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["pendingStatus"] = pendingStatus;

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }



        [HttpGet]
        public PartialViewResult GetQualityCheckList(string qualityCheckNo = "", string gateInNo = "", string poNo = "", string fromDate = "", string toDate = "", string approvalStatus = "",string companyBranch="")
        {
            List<QualityCheckViewModel> qcs = new List<QualityCheckViewModel>();
            QualityCheckBL qualityCheckBL = new QualityCheckBL();
            try
            {
                qcs = qualityCheckBL.GetQualityCheckList(qualityCheckNo, gateInNo, poNo, fromDate, toDate, ContextUser.CompanyId, approvalStatus, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(qcs);
        }
        [HttpGet]
        public PartialViewResult GetQualityCheckGateInList(string gateInNO, string poNo = "", string fromDate = "", string toDate = "",string companyBranch = "")
        {
            List<QualityCheckViewModel> qcs = new List<QualityCheckViewModel>();            
            QualityCheckBL qualityCheckBL = new QualityCheckBL();
            try
            {
                qcs = qualityCheckBL.GetQualityCheckGateInList(gateInNO, poNo, fromDate, toDate, ContextUser.CompanyId, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(qcs);
        }
      
        [HttpPost]
        public PartialViewResult GetQualityCheckGateInProductList(List<GateInProductDetailViewModel> giProducts, long gateInId)
        {           
            QualityCheckBL qualityCheckBL = new QualityCheckBL();
            try
            {
                if (giProducts == null)
                {
                    giProducts = qualityCheckBL.GetQualityCheckGateInProductList(gateInId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(giProducts);
        }

        [HttpPost]
        public PartialViewResult GetQualityCheckProductList(List<QualityCheckProductDetailViewModel> qualityCheckProducts, long qualityCheckId)
        {           
            QualityCheckBL qualityCheckBL = new QualityCheckBL();
            try
            {
                if (qualityCheckProducts == null)
                {
                    qualityCheckProducts = qualityCheckBL.GetQualityCheckProductList(qualityCheckId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(qualityCheckProducts);
        }

        [HttpPost]
        public PartialViewResult GetQualityCheckSupportingDocumentList(List<QualityCheckSupportingDocumentViewModel> qualityCheckDocuments, long qualityCheckId)
        {           
            QualityCheckBL qualityCheckBL = new QualityCheckBL();
            try
            {
                if (qualityCheckDocuments == null)
                {
                    qualityCheckDocuments = qualityCheckBL.GetQualityCheckSupportingDocumentList(qualityCheckId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(qualityCheckDocuments);
        }

        public FileResult DocumentDownload(string fileName)
        {
            var path = Path.Combine(Server.MapPath("~/Images/QCDocument"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpGet]
        public JsonResult GetQualityCheckDetail(long qualityCheckId)
        {
            QualityCheckBL qualityCheckBL = new QualityCheckBL();
            QualityCheckViewModel qcViewModel = new QualityCheckViewModel();
            try
            {
                qcViewModel = qualityCheckBL.GetQualityCheckDetail(qualityCheckId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(qcViewModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Report(long qualityCheckId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            QualityCheckBL qualityCheckBL = new QualityCheckBL();

            string path = Path.Combine(Server.MapPath("~/RDLC"), "QualitycheckPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }

          

           
            ReportDataSource rd = new ReportDataSource("DataSet1", qualityCheckBL.GetQualityCheckDetailReport(qualityCheckId));
            ReportDataSource rdProduct = new ReportDataSource("DataSet2", qualityCheckBL.GetQualityCheckProductListReport(qualityCheckId));
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
                    gateinViewModel.GateInId = gateinId;
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
