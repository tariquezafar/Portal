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
    public class SeparationOrderController : BaseController
    {
        #region Separation Order
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SeparationOrder, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditSeparationOrder(int separationorderId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                if (separationorderId != 0)
                {
                    ViewData["separationorderId"] = separationorderId;
                    ViewData["accessMode"] = accessMode;
                    
                }
                else
                {
                    ViewData["separationorderId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SeparationOrder, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditSeparationOrder(SeparationOrderViewModel separationorderViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SeparationOrderBL separationorderBL = new SeparationOrderBL();
            try
            {
                if (separationorderViewModel != null)
                {
                    separationorderViewModel.CompanyId = ContextUser.CompanyId;
                    separationorderViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = separationorderBL.AddEditSeparationOrder(separationorderViewModel);

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SeparationOrder, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListSeparationOrder()
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
        public PartialViewResult GetSeparationOrderList(string separationorderNo = "", int employeeId = 0, string employeeClearanceNo = "", string exitInterviewNo = "", string separationStatus = "",  string fromDate = "", string toDate = "")
        {
            List<SeparationOrderViewModel> separationOrders = new List<SeparationOrderViewModel>();
            SeparationOrderBL separationOrderBL = new SeparationOrderBL();
            try
            {
                separationOrders = separationOrderBL.GetSeparationOrderList(separationorderNo, employeeId, employeeClearanceNo, exitInterviewNo, separationStatus, fromDate, toDate);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(separationOrders);
        }

        [HttpGet]
        public JsonResult GetSeparationOrderDetail(long separationorderId)
        {
            SeparationOrderBL separationOrderBL = new SeparationOrderBL();
            SeparationOrderViewModel separationOrders = new SeparationOrderViewModel();
            try
            {
                separationOrders = separationOrderBL.GetSeparationOrderDetail(separationorderId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(separationOrders, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetExitInterviewForSeparationOrderList()
        {
            ExitInterviewBL exitInterviewBL = new ExitInterviewBL();
            List<ExitInterviewViewModel> exitinterviews = new List<ExitInterviewViewModel>();
            try
            {
                exitinterviews = exitInterviewBL.GetExitInterviewForSeparationOrderList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(exitinterviews, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetClearanceTemplateForSeparationOrderList()
        {
            ClearanceTemplateBL clearanceTemplateBL = new ClearanceTemplateBL(); 
            List<ClearanceTemplateViewModel> clearanceTemplates = new List<ClearanceTemplateViewModel>();
            try
            {
                clearanceTemplates = clearanceTemplateBL.GetClearanceTemplateList(ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(clearanceTemplates, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEmployeeInterviewClearanceDetail(long employeeId)
        {
            SeparationOrderBL separationOrderBL = new SeparationOrderBL();
            SeparationOrderViewModel separationOrder = new SeparationOrderViewModel();
            try
            {
                separationOrder = separationOrderBL.GetEmployeeInterviewClearanceDetail(employeeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(separationOrder, JsonRequestBehavior.AllowGet);
        }
        #endregion


    }
}
