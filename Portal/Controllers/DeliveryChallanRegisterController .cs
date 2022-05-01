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


namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class DeliveryChallanRegisterController : BaseController
    {
      

        [ValidateRequest(true, UserInterfaceHelper.Add_DeliveryChallanRegister, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListDeliveryChallanRegister()
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
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
        public PartialViewResult GetDeliveryChallanRegisterList(int customerId = 0, int stateId = 0, int shippingstateId = 0, string fromDate = "", string toDate = "", int createdBy = 0, string sortBy = "", string sortOrder = "",int companyBranchId=0)
        {           
            DeliveryChallanRegisterBL deliveryChallanRegisterBL = new DeliveryChallanRegisterBL();
            List<DeliveryChallanViewModel> deliveryChallanViewModel = new List<DeliveryChallanViewModel>();
            try
            {
                deliveryChallanViewModel = deliveryChallanRegisterBL.GetDeliveryChallanRegisterList(customerId, stateId, shippingstateId, fromDate, toDate, createdBy, ContextUser.CompanyId, sortBy, sortOrder, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(deliveryChallanViewModel);
        }


        public ActionResult GenerateDeliveryChallanRegisterReports(int customerId = 0, int stateId = 0, int shippingstateId = 0, string fromDate = "", string toDate = "", int createdBy = 0, string sortBy = "", string sortOrder = "",int companyBranchId=0, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            DeliveryChallanRegisterBL deliveryChallanRegisterBL = new DeliveryChallanRegisterBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "DeliveryChallanRegisterReports.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            ReportDataSource rd = new ReportDataSource("DeliveryChallanRegisterReportsDataSet", deliveryChallanRegisterBL.GenerateDeliveryChallanRegisterReports(customerId, stateId, shippingstateId, fromDate, toDate, createdBy, ContextUser.CompanyId, sortBy, sortOrder, companyBranchId));
            lr.DataSources.Add(rd);

            ReportParameter rp3 = new ReportParameter("fromdate", fromDate);
            ReportParameter rp4 = new ReportParameter("todate", toDate);
            lr.SetParameters(rp3);
            lr.SetParameters(rp4);

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =

             "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>26.5in</PageWidth>" +
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

    }
}
