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
using Portal.Common.ViewModel;

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class DispatchController : BaseController
    {
        //
        // GET: /ComplaintService/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Dispatch, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditDispatch(int dispatchID = 0, int accessMode = 3)
        {

            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                ViewData["RoleId"] = ContextUser.RoleId;
                if (dispatchID != 0)
                {
                    ViewData["dispatchID"] = dispatchID;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["dispatchID"] = 0;
                    ViewData["accessMode"] = 1;
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Dispatch, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditDispatch(DispatchViewModel dispatchViewModel, List<DispatchProductDetailViewModel> dispatchProductDetails)
        {
            ResponseOut responseOut = new ResponseOut();
            DispatchBL dispatchBL = new DispatchBL();
            try
            {
                if (dispatchViewModel != null)
                {
                    dispatchViewModel.CreatedBy = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                    responseOut = dispatchBL.AddEditDispatch(dispatchViewModel, dispatchProductDetails);
                }
                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_DispatchPlan, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListDispatch()
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
        public PartialViewResult GetDispatchList(string dispatchNo = "" , string dispatchPlanNo = "", int companyBranchId = 0, string fromDate = "", string toDate = "", string approvalStatus = "")
        {
            List<DispatchViewModel> dispatchPlans = new List<DispatchViewModel>();
            DispatchBL dispatchPlanBL = new DispatchBL();

            try
            {
                dispatchPlans = dispatchPlanBL.GetDispatchList(dispatchNo, dispatchPlanNo, companyBranchId, fromDate, toDate, approvalStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(dispatchPlans);
        }

       
        public JsonResult GetDispatchDetail(int dispatchID)
        {
            DispatchBL dispatchBL = new DispatchBL();
            DispatchViewModel dispatch = new DispatchViewModel();
            try
            {
                dispatch = dispatchBL.GetDispatchDetail(dispatchID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(dispatch, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult GetDispatchProductList(int dispatchID)
        {
            DispatchBL dispatchBL = new DispatchBL();
            List<DispatchProductDetailViewModel> dispatchProductDetailViewModels = new List<DispatchProductDetailViewModel>();
            try
            {
                dispatchProductDetailViewModels = dispatchBL.GetDispatchProductList(dispatchID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(dispatchProductDetailViewModels);
        }

        [HttpGet]
        public PartialViewResult GetDispatchPlanList(string dispatchPlanNo = "", string customerName = "", int companyBranchId = 0, string fromDate = "", string toDate = "", string approvalStatus = "")
        {
            List<DispatchPlanViewModel> dispatchPlans = new List<DispatchPlanViewModel>();
            DispatchPlanBL dispatchPlanBL = new DispatchPlanBL();

            try
            {
                dispatchPlans = dispatchPlanBL.GetDispatchPlanListForDispatch(dispatchPlanNo, customerName, companyBranchId, fromDate, toDate, approvalStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(dispatchPlans);
        }

        [HttpPost]
        public PartialViewResult GetCustomerSOProductList(List<SOProductViewModel> sOProductViewModellst, string sOIds, bool isDispatchPlan)
        {
            DispatchPlanBL dispatchPlanBL = new DispatchPlanBL();
            try
            {
                if (sOProductViewModellst == null)
                {
                    sOProductViewModellst = dispatchPlanBL.GetCustomerSOProductList(sOIds, isDispatchPlan);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(sOProductViewModellst);
        }


    }
}
