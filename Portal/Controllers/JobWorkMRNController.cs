using Microsoft.Reporting.WebForms;
using Portal.Common;
using Portal.Core;
using Portal.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web.Mvc;
namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class JobWorkMRNController : BaseController
    {
        #region JobWorkMRN

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_JobWorkMRN, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditJobWorkMRN(int jobWorkMRNId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;

                if (jobWorkMRNId != 0)
                {
                    ViewData["jobWorkMRNId"] = jobWorkMRNId;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["jobWorkMRNId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_JobWorkMRN, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditJobWorkMRN(JobWorkMRNViewModel jobWorkMRNViewModel, List<JobWorkMRNProductViewModel> jobWorkMRNProducts)
        {
            ResponseOut responseOut = new ResponseOut();                           
            JobWorkMRNBL jobWorkMRNBL = new JobWorkMRNBL();
            try
            {                            
                if (jobWorkMRNViewModel != null)
                {
                    jobWorkMRNViewModel.CreatedBy = ContextUser.UserId;
                    jobWorkMRNViewModel.CompanyId = ContextUser.CompanyId;
                    jobWorkMRNViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = jobWorkMRNBL.AddEditJobWorkMRN(jobWorkMRNViewModel, jobWorkMRNProducts);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_JobWorkMRN, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListJobWorkMRN(string pendingStatus="0")
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
        public PartialViewResult GetJobWorkMRNList(string jobWorkMRNNo = "", string jobOrderNo = "", int companyBranchId = 0, string fromDate = "", string toDate = "",string approvalStatus = "")
        {
            List<JobWorkMRNViewModel> jobWorkMRNs = new List<JobWorkMRNViewModel>();       
            JobWorkMRNBL jobWorkMRNBL = new JobWorkMRNBL();
            try
            {
                jobWorkMRNs = jobWorkMRNBL.GetJobWorkMRNList(jobWorkMRNNo, jobOrderNo, companyBranchId, fromDate, toDate, ContextUser.CompanyId, approvalStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(jobWorkMRNs);
        }

        [HttpGet]
        public JsonResult GetJobWorkMRNDetail(long jobWorkMRNId=0)
        {           
            JobWorkMRNViewModel JobWorkMRNViewModel = new JobWorkMRNViewModel();
            JobWorkMRNBL jobWorkMRNBL = new JobWorkMRNBL();
            try
            {
                JobWorkMRNViewModel = jobWorkMRNBL.GetJobWorkMRNDetail(jobWorkMRNId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(JobWorkMRNViewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public PartialViewResult GetJobWorkMRNProductList(List<JobWorkMRNProductViewModel> jobWorkMRNProducts, long jobWorkMRNId)
        {                      
            JobWorkMRNBL jobWorkMRNBL = new JobWorkMRNBL();
            try
            {
                if (jobWorkMRNProducts == null)
                {
                    jobWorkMRNProducts = jobWorkMRNBL.GetJobWorkMRNProductList(jobWorkMRNId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(jobWorkMRNProducts);
        }

        [HttpGet]
        public PartialViewResult GetJobWorkMRNJobOrderList(string jobWorkNo, int companyBranchId, string fromDate, string toDate)
        {
            List<JobOrderViewModel> JobOrders = new List<JobOrderViewModel>();
            JobWorkMRNBL jobWorkMRNBL = new JobWorkMRNBL();
           
            try
            {
                JobOrders = jobWorkMRNBL.GetJobWorkMRNJobOrderList(jobWorkNo, companyBranchId, fromDate, toDate, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(JobOrders);
        }

        [HttpPost]
        public PartialViewResult GetJobWorkMRNJobOrderProductList(List<JobWorkProductDetailViewModel> jobOrderMrnProducts, long jobOrderId)
        {                     
            JobWorkMRNBL jobWorkMRNBL = new JobWorkMRNBL();
            try
            {
                if (jobOrderMrnProducts == null)
                {
                    jobOrderMrnProducts = jobWorkMRNBL.GetJobWorkMRNJobOrderProductList(jobOrderId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(jobOrderMrnProducts);
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

        public ActionResult Report(long jobWorkMRNId, string reportType = "PDF", string reportOption = "Original")
        {
             LocalReport lr = new LocalReport();                       
            JobWorkMRNBL jobWorkMRNBL = new JobWorkMRNBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "JobWorkMRNReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            DataTable dt = new DataTable();
            dt = jobWorkMRNBL.GetJobWorkMRNDetailPrint(jobWorkMRNId);
            ReportDataSource rd = new ReportDataSource("DataSet1", dt);

            DataTable dtsum = new DataTable();
            dtsum = jobWorkMRNBL.GetJobWorkMRNProductPrint(jobWorkMRNId);
            decimal totalInvoiceAmount = 0;
            totalInvoiceAmount=Convert.ToDecimal(dtsum.Compute("Sum(TotalValue)", ""));
            string totalWords = CommonHelper.ConvertMyword(Convert.ToInt32(totalInvoiceAmount));

            ReportDataSource rdProduct = new ReportDataSource("DataSet2", dtsum);
            ReportParameter rp2 = new ReportParameter("ReportOption", reportOption);
            ReportParameter rp1 = new ReportParameter("AmountInWords", totalWords);
                        
            lr.SetParameters(rp1);
            lr.DataSources.Add(rd);
            lr.DataSources.Add(rdProduct);
            lr.SetParameters(rp2);
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

        [HttpPost]
        public PartialViewResult GetJobWorkProductInList(List<JobWorkINProductDetailViewModel> jobWorkProducts, long jobworkId)
        {
            JobWorkBL jobWorkBL = new JobWorkBL();
            try
            {
                if (jobWorkProducts == null)
                {
                    jobWorkProducts = jobWorkBL.GetJobWorkProductInList(jobworkId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(jobWorkProducts);
        }
        #endregion

    }
}
