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
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class CarryForwardChasisController : BaseController
    {
        #region CarryForwardChasis

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_CarryForwardChasis, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditCarryForwardChasis(int carryForwardID = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;


                if (carryForwardID != 0)
                {
                    ViewData["carryForwardID"] = carryForwardID;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["carryForwardID"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_CarryForwardChasis, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditCarryForwardChasis(CarryForwardChasissViewModel carryForwardChasissViewModel, List<CarryForwardChasisDetailViewModel> carryForwardChasisDetailViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            CarryForwardChasisBL carryForwardChasisBL = new CarryForwardChasisBL();
            try
            {                            
                if (carryForwardChasissViewModel != null)
                {
                    carryForwardChasissViewModel.CreatedBy = ContextUser.UserId;
                    carryForwardChasissViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = carryForwardChasisBL.AddEditCarryForwardChasis(carryForwardChasissViewModel, carryForwardChasisDetailViewModel);
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_CarryForwardChasis, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListCarryForwardChasis()
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
        public PartialViewResult GetCarryForwardChasisList(string carryForwardNo = "", int companyBranchId = 0, int prevddlMonth = 0, int prevddlYear = 0, int carryddlMonth = 0, int carryddlYear = 0,  string fromDate = "", string toDate = "", string approvalStatus = "")
        {
            List<CarryForwardChasissViewModel> carryForwardChasissViewModel = new List<CarryForwardChasissViewModel>();                          
            CarryForwardChasisBL carryForwardChasisBL = new CarryForwardChasisBL();
            try
            {
                carryForwardChasissViewModel = carryForwardChasisBL.GetCarryForwardChasisList(carryForwardNo, companyBranchId, prevddlMonth, prevddlYear, carryddlMonth, carryddlYear,  Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.CompanyId, approvalStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(carryForwardChasissViewModel);
        }
       
        [HttpGet]
        public JsonResult GetCarryForwardChasisDetail(long carryForwardID= 0)
        {
            CarryForwardChasissViewModel carryForwardChasissViewModel = new CarryForwardChasissViewModel();
            CarryForwardChasisBL carryForwardChasisBL = new CarryForwardChasisBL();
            try
            {
                carryForwardChasissViewModel = carryForwardChasisBL.GetCarryForwardChasisDetail(carryForwardID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(carryForwardChasissViewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public PartialViewResult GetCarryForwardChasisProductList(List<CarryForwardChasisDetailViewModel> carryForwardChasisDetails, int prevYear = 0, int prevMonth = 0)
        {                              
            CarryForwardChasisBL carryForwardChasisBL = new CarryForwardChasisBL();
            try
            {
                if (carryForwardChasisDetails == null)
                {
                    carryForwardChasisDetails = carryForwardChasisBL.GetCarryForwardChasisProductsList(prevYear, prevMonth);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(carryForwardChasisDetails);
        }
       
        public ActionResult PrintCarryForward(long carryForwardID, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            CarryForwardChasisBL carryForwardChasisBL = new CarryForwardChasisBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "CarryForwardReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("ListPrintChasis");
            }
            DataTable dt = new DataTable();
            dt = carryForwardChasisBL.GetPrintCarryForwardChasisPrint(carryForwardID);
            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            ReportDataSource rdPackingList = new ReportDataSource("DataSet2", carryForwardChasisBL.GetPrintCarryForwardProductsPrint(carryForwardID));          
            lr.DataSources.Add(rd);
            lr.DataSources.Add(rdPackingList);        
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                        "  <OutputFormat>" + reportType + "</OutputFormat>" +
                        "  <PageWidth>8.5in</PageWidth>" +
                        "  <PageHeight>11in</PageHeight>" +
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
        public PartialViewResult GetCarryForwardChasisProducts(List<CarryForwardChasisDetailViewModel> carryForwardChasisDetails, long carryForwardID = 0)
        {          
            CarryForwardChasisBL carryForwardChasisBL = new CarryForwardChasisBL();
            try
            {
                if (carryForwardChasisDetails == null)
                {
                    carryForwardChasisDetails = carryForwardChasisBL.GetCarryForwardChasisProducts(carryForwardID);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(carryForwardChasisDetails);
        }

        #endregion

    }
}
