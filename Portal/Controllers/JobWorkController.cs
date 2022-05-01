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
    public class JobWorkController : BaseController
    {
        //
        // GET: /JobWork/


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_JobWork, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditJobWork(int jobWorkId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                if (jobWorkId != 0)
                {
                    ViewData["jobWorkId"] = jobWorkId;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["jobWorkId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_JobWork, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditJobWork(JobOrderViewModel jobWorkViewModel, List<JobWorkProductDetailViewModel> jobWorkProducts, List<JobWorkINProductDetailViewModel> jobWorkINProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            JobWorkBL jobWorkBL = new JobWorkBL();
          
            try
            {
                if (jobWorkViewModel != null)
                {
                    jobWorkViewModel.CreatedBy = ContextUser.UserId;
                    jobWorkViewModel.CompanyId = ContextUser.CompanyId;
                    jobWorkViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = jobWorkBL.AddEditJobWork(jobWorkViewModel, jobWorkProducts, jobWorkINProducts);

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

        [HttpPost]
        public PartialViewResult GetJobWorkProductList(List<JobWorkProductDetailViewModel> jobWorkProducts, long jobworkId)
        {
            JobWorkBL jobWorkBL = new JobWorkBL();
            try
            {
                if (jobWorkProducts == null)
                {
                   jobWorkProducts = jobWorkBL.GetJobWorkProductList(jobworkId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(jobWorkProducts);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_JobWork, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListJobWork(string pendingStatus="0")
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
        public PartialViewResult GetJobWorkList(string jobWorkNo = "", int companyBranchId = 0, string fromDate = "", string toDate = "", string jobWorkStatus = "")
        {
            List<JobOrderViewModel> workOrders = new List<JobOrderViewModel>();
            JobWorkBL jobWorkBL = new JobWorkBL();
            try
            {
                workOrders = jobWorkBL.GetJobWorkList(jobWorkNo, companyBranchId, fromDate, toDate, ContextUser.CompanyId,"", jobWorkStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(workOrders);
        }

        [HttpGet]
        public JsonResult GetJobWorkDetail(long jobWorkId)
        {
            JobWorkBL jobWorkBL = new JobWorkBL();
            JobOrderViewModel jobWork = new JobOrderViewModel();
            try
            {
                jobWork = jobWorkBL.GetJobWorkDetail(jobWorkId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(jobWork, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Report(long jobworkId, string reportType = "PDF", string reportOption = "Original")
        {
            LocalReport lr = new LocalReport();
            JobWorkBL jobWorkBL = new JobWorkBL();
            
            string path = Path.Combine(Server.MapPath("~/RDLC"), "JobWorkReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            DataTable dtTotalValue = new DataTable();
            dtTotalValue = jobWorkBL.GetJobWorkTotalValue(jobworkId);
            decimal totalInvoiceAmount = 0;
            totalInvoiceAmount = Convert.ToDecimal(dtTotalValue.Rows[0]["SumTotalValue"].ToString());
            string totalWords = CommonHelper.ConvertMyword(Convert.ToInt32(totalInvoiceAmount));

            DataTable dt = new DataTable();
            dt = jobWorkBL.GetJobWorkDataTable(jobworkId);


            ReportDataSource rd = new ReportDataSource("JobWorkDetailDataSet", dt);
            ReportDataSource rdProduct = new ReportDataSource("JobWorkProductsDataSet", jobWorkBL.GetJobWorkProductListDataTable(jobworkId));

            lr.DataSources.Add(rd);
            lr.DataSources.Add(rdProduct);
            ReportParameter rp1 = new ReportParameter("AmountInWords", totalWords);
            ReportParameter rp2 = new ReportParameter("ReportOption", reportOption);
            lr.SetParameters(rp1);
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
    }
}
