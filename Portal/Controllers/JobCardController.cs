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
using System.Web.Script.Serialization;

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class JobCardController : BaseController
    {
        
        #region JobCard

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_JobCard, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditJobCard(int jobCardId = 0, int accessMode = 0)
        {
            try
            {

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                var date = DateTime.Now;
                ViewData["Hour"] = date.Hour;
                ViewData["Minute"] = date.Minute;

                if (jobCardId != 0)
                {
                    ViewData["jobCardId"] = jobCardId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["jobCardId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_JobCard, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditJobCard(JobCardViewModel jobCardViewModel, List<JobCardProductDetailViewModel> jobCardProductDetailViewModel,List<JobCardDetailViewModel> jobProductList)
        {
            ResponseOut responseOut = new ResponseOut();
            JobCardBL jobCardBL = new JobCardBL();
            try
            {
                if (jobCardViewModel != null)
                {
                    jobCardViewModel.CreatedBy = ContextUser.UserId;
                    jobCardViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = jobCardBL.AddEditJobCard(jobCardViewModel, jobCardProductDetailViewModel, jobProductList);
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
        public PartialViewResult GetJobCardProductList(List<JobCardProductDetailViewModel> jobCardProducts, long jobCardId)
        {
            JobCardBL jobCardBL = new JobCardBL();
            try
            {
                if (jobCardProducts == null)
                {
                    jobCardProducts = jobCardBL.GetJobCardProductList(jobCardId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(jobCardProducts);
        }

        [HttpPost]
        public PartialViewResult GetJobCardProduct(List<JobCardDetailViewModel> jobCardProductList, long jobCardId)
        {
            JobCardBL jobCardBL = new JobCardBL();
            try
            {
                if (jobCardProductList == null)
                {
                    jobCardProductList = jobCardBL.GetJobCardProduct(jobCardId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(jobCardProductList);
        }

        [HttpGet]
        public JsonResult GetJobCardDetail(long jobCardId)
        {
            JobCardBL jobCardBL = new JobCardBL();
            JobCardViewModel jobCardViewModel = new JobCardViewModel();
            try
            {
                jobCardViewModel = jobCardBL.GetJobCardDetail(jobCardId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(jobCardViewModel, JsonRequestBehavior.AllowGet);
        }
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_JobCard, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListJobCard()
        {
            try
            {
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
        public PartialViewResult GetJobCardList(string jobCardNo = "", string customerName = "", string approvalStatus="", string modelName="", string engineNo="", string regNo="", string keyNo="", string fromDate = "", string toDate = "")
        {
            List<JobCardViewModel> jobCardViewModel = new List<JobCardViewModel>();
            JobCardBL jobCardBL = new JobCardBL();
            try
            {
                jobCardViewModel = jobCardBL.GetJobCardList(jobCardNo, customerName, approvalStatus, modelName, engineNo, regNo, keyNo, fromDate, toDate, ContextUser.CompanyId, Convert.ToInt32(Session[SessionKey.CustomerId]));
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(jobCardViewModel);
        }
        public ActionResult Report(long jobCardId=0, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            JobCardBL jobCardBL = new JobCardBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "JobCard.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }

            DataTable dt = new DataTable();
            dt = jobCardBL.GetJobCardDetailDataTable(jobCardId);

            ReportDataSource rd = new ReportDataSource("JobCardDataSet", dt);
            ReportDataSource rdProduct = new ReportDataSource("JobCardProductDataSet", jobCardBL.GetJobCardProductDataTable(jobCardId));
            ReportDataSource rdProduct1 = new ReportDataSource("JobCardDataSet1", jobCardBL.GetJobCardProductDataTable1(jobCardId));

            lr.DataSources.Add(rd);
            lr.DataSources.Add(rdProduct);
            lr.DataSources.Add(rdProduct1);

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
