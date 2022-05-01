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
    public class SINController : BaseController
    {
     
        // GET: /SIN/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SIN, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditSIN(int sINId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                if (sINId != 0)
                {
                    ViewData["sINId"] = sINId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["sINId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SIN, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditSIN(SINViewModel sinViewModel, List<SINProductDetailViewModel> sinProductDetailViewModel, List<SINSupportingDocumentViewModel> sinDocuments)
        {
            ResponseOut responseOut = new ResponseOut();                 
            SINBL sINBL = new SINBL();
            try
            {
                if (sinViewModel != null)
                {
                    sinViewModel.CreatedBy = ContextUser.UserId;
                    sinViewModel.CompanyId = ContextUser.CompanyId;
                    sinViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = sINBL.AddEditSIN(sinViewModel,sinProductDetailViewModel, sinDocuments);

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

        [HttpGet]
        public JsonResult GetCompanyBranchList()
        {
            CompanyBL companyBL = new CompanyBL();
            List<CompanyBranchViewModel> companyBranchList = new List<CompanyBranchViewModel>();
            try
            {
                companyBranchList = companyBL.GetCompanyBranchList(ContextUser.CompanyId);
                
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(companyBranchList, JsonRequestBehavior.AllowGet);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SIN, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListSIN()
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
        public PartialViewResult GetSINList(string sinNo = "",string requisitionNo="", string jobno = "", int companyBranchId=0,string fromDate = "", string toDate = "",string sINStatus="")
        {
            List<SINViewModel> stns = new List<SINViewModel>();                     
            SINBL sINBL = new SINBL();
            try
            {
                stns = sINBL.GetSINList(sinNo, requisitionNo, jobno, companyBranchId, fromDate, toDate, ContextUser.CompanyId, sINStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(stns);
        }

        [HttpPost]
        public PartialViewResult GetSINProductList(List<SINProductDetailViewModel> sinProducts, long sinId)
        {
            SINBL sINBL = new SINBL();
            try
            {
                if (sinProducts == null)
                {
                    sinProducts = sINBL.GetSINProductList(sinId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(sinProducts);
        }
        public JsonResult GetSINDetail(long sinId)
        {    
            SINBL sINBL = new SINBL();            
            SINViewModel sin = new SINViewModel();
            try
            {
                sin = sINBL.GetSINDetail(sinId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(sin, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Report(long sINId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            SINBL sINBL = new SINBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "SINPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }

            DataTable dt = new DataTable();
            dt = sINBL.GetSINDetailDataTable(sINId);           
            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            ReportDataSource rdProduct = new ReportDataSource("DataSetProduct", sINBL.GetSINProductListDataTable(sINId));

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
                        var path = Path.Combine(Server.MapPath("~/Images/SINDocument"), newFileName);
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
            var path = Path.Combine(Server.MapPath("~/Images/SINDocument"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        public PartialViewResult GetSINSupportingDocumentList(List<SINSupportingDocumentViewModel> sinDocuments, long sinId)
        {
            SINBL sINBL = new SINBL();
            try
            {
                if (sinDocuments == null)
                {
                    sinDocuments = sINBL.GetSINSupportingDocumentList(sinId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(sinDocuments);
        }

        [HttpGet]
        public JsonResult GetUserAutoCompleteList(string term)
        {         
            UserBL userBL = new UserBL();
            List<UserViewModel> userList = new List<UserViewModel>();
            try
            {
                userList = userBL.GetUserAutoCompleteList(term, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(userList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFromLocationList(int companyBranchID)
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

        [HttpGet]
        public PartialViewResult GetStoreRequisitionList(string requisitionNo = "", string workOrderNo = "", string requisitionType = "", int companyBranchId = 0, string fromDate = "", string toDate = "", string displayType = "", string approvalStatus = "")
        {
            List<StoreRequisitionViewModel> requisitions = new List<StoreRequisitionViewModel>();
            StoreRequisitionBL storeRequisitionBL = new StoreRequisitionBL();
            SINBL sINBL = new SINBL();
            try
            {

               
                requisitions = sINBL.GetStoreRequisitionList(requisitionNo, workOrderNo, requisitionType, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.CompanyId, displayType, approvalStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(requisitions);
        }


        [HttpPost]
        public PartialViewResult GetStoreRequisitionProductList(List<SINProductDetailViewModel> sinProducts, long requisitionId)
        {
          
            SINBL sINBL = new SINBL();
            try
            {
                if (sinProducts == null)
                {
                    FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                    sinProducts = sINBL.GetSINStoreRequisitionProductList(requisitionId, finYear.FinYearId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(sinProducts);
        }


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SIN, (int)AccessMode.CancelAccess, (int)RequestMode.GetPost)]
        public ActionResult CancelSIN(int sinid = 0, int accessMode = 4)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                if (sinid != 0)
                {
                    ViewData["sinid"] = sinid;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["sinid"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SIN, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult CancelSIN(long sinId,int companyBranchId, string cancelReason)
        {
            ResponseOut responseOut = new ResponseOut();
            SINBL sINBL = new SINBL();
            SINViewModel sINViewModel = new SINViewModel();
            FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
            try
            {
                if (sINViewModel != null)
                {

                    sINViewModel.SINId = sinId;

                    sINViewModel.CancelReason = cancelReason;
                    sINViewModel.CreatedBy = ContextUser.UserId;
                    sINViewModel.CompanyId = ContextUser.CompanyId;
                    sINViewModel.CompanyBranchId = companyBranchId;
                    sINViewModel.FinYearId = finYear.FinYearId;
                    responseOut = sINBL.CancelSIN(sINViewModel);
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


       
    }
}
