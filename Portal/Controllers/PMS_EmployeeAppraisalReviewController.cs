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
using System.Text;
using System.Data;

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class PMS_EmployeeAppraisalReviewController : BaseController
    {
        #region PMS_EmployeeAppraisalReview

        //
        // GET: /PMS_EmployeeAppraisalTemplateMappingController/
        public ActionResult Index()
        {
            return View();
        }
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PMS_EmployeeAppraisalReview, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmployeeAppraisalReview(int pMSReviewId = 0, int accessMode = 3)
        { 
            try
            { 
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (pMSReviewId != 0)
                {
                    ViewData["pMSReviewId"] = pMSReviewId;
                    ViewData["accessMode"] = accessMode;                   
                }
                else
                {
                    ViewData["pMSReviewId"] = 0;
                    ViewData["accessMode"] = 3;                  
                } 

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PMS_EmployeeAppraisalReview, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmployeeAppraisalReview(PMS_EmployeeAppraisalReviewViewModel employeeAppraisalReviewViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            PMS_EmployeeAppraisalReviewBL employeeAppraisalReviewBL = new PMS_EmployeeAppraisalReviewBL();
            try
            {
                if (employeeAppraisalReviewViewModel != null)
                {
                    employeeAppraisalReviewViewModel.CreatedBy = ContextUser.UserId;
                    employeeAppraisalReviewViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = employeeAppraisalReviewBL.AddEditEmployeeAppraisalReview(employeeAppraisalReviewViewModel);

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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PMS_EmployeeAppraisalReview, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListEmployeeAppraisalReview()
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpGet]
        public JsonResult GetEmployeeAppraisalReviewDetail(long pmsReviewId)
        {

            PMS_EmployeeAppraisalReviewBL empReviewBL = new PMS_EmployeeAppraisalReviewBL();
            PMS_EmployeeAppraisalReviewViewModel empReview = new PMS_EmployeeAppraisalReviewViewModel();
            try
            {
                empReview = empReviewBL.GetEmployeeAppraisalReviewDetail(pmsReviewId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(empReview, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public PartialViewResult GetEmployeeAppraisalReviewList(string employeeName,string pmsFinalStatus,int companyBranchId)
        {
            List<PMS_EmployeeAppraisalReviewViewModel> empReviews = new List<PMS_EmployeeAppraisalReviewViewModel>();
            PMS_EmployeeAppraisalReviewBL empReviewBL = new PMS_EmployeeAppraisalReviewBL();
            try
            {
                empReviews = empReviewBL.GetEmployeeAppraisalReviewList(employeeName, pmsFinalStatus,companyBranchId,ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(empReviews);
        }


        [HttpGet]
        public JsonResult GetFinancialYearForEmployeeAppraisalTemplateList()
        {
            FinYearBL finyearBL = new FinYearBL(); 
            List<FinYearViewModel> finyearViewModel = new List<FinYearViewModel>(); 
            try
            {
                finyearViewModel = finyearBL.GetFinancialYearForEmployeeAppraisalTemplateList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(finyearViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult GetFinalAssessmentList(string templateName = "", string employeeName = "", string employeeMapping_Status = "1")
        {
            List<PMS_EmployeeAppraisalTemplateMappingViewModel> appraisalTemplates = new List<PMS_EmployeeAppraisalTemplateMappingViewModel>();
            PMS_EmployeeAppraisalTemplateMappingBL appraisalTemplateBL = new PMS_EmployeeAppraisalTemplateMappingBL();
            try
            {
                appraisalTemplates = appraisalTemplateBL.GetEmployeeAppraisalTemplateMappingList(templateName, employeeName, ContextUser.CompanyId, "1",0);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(appraisalTemplates);
        }

        public ActionResult PMSReport(long empAppraisalTemplateMappingId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            PMS_EmployeeAppraisalReviewBL empReviewBL = new PMS_EmployeeAppraisalReviewBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "PMS_EmployeeAssessmentPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            DataTable dt = new DataTable();
            dt = empReviewBL.GetPMS_EmployeeDetail(empAppraisalTemplateMappingId);
            
            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            ReportDataSource rdDetail = new ReportDataSource("DataSet2", empReviewBL.GetPMS_EmployeeAssessmentDetail(empAppraisalTemplateMappingId));
            ReportDataSource rdFooter = new ReportDataSource("DataSet3", empReviewBL.GetPMS_EmployeeAssessmentFooterDetail(empAppraisalTemplateMappingId));

            lr.DataSources.Add(rd);
            lr.DataSources.Add(rdDetail);
            lr.DataSources.Add(rdFooter);


            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo = "<DeviceInfo>" +
                        "  <OutputFormat>" + reportType + "</OutputFormat>" +
                        "  <PageWidth>18in</PageWidth>" +
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
