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
    public class DispatchPlanController : BaseController
    {
        //
        // GET: /ComplaintService/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_DispatchPlan, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditDispatchPlan(int dispatchPlanID = 0, int accessMode = 3)
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
                if (dispatchPlanID != 0)
                {
                    ViewData["dispatchPlanID"] = dispatchPlanID;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["dispatchPlanID"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_DispatchPlan, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditDispatchPlan(DispatchPlanViewModel dispatchPlanViewModel, List<DispatchPlanProductDetailViewModel> dispatchPlanProductDetails)
        {
            ResponseOut responseOut = new ResponseOut();
            DispatchPlanBL dispatchPlanBL = new DispatchPlanBL();
            try
            {
                if (dispatchPlanViewModel != null)
                {
                    responseOut = dispatchPlanBL.AddEditDispatchPlan(dispatchPlanViewModel, dispatchPlanProductDetails);
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
        public JsonResult GetCustomerAutoCompletewithSaleOrderList(string term)
        {
            CustomerBL customerBL = new CustomerBL();
            List<SelectListModel> lstSelectListModel = new List<SelectListModel>();       
            try
            {
                lstSelectListModel = customerBL.GetCustomerAutoCompletewithSaleOrderList(term, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(lstSelectListModel, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_DispatchPlan, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListDispatchPlan()
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
        public PartialViewResult GetCustomerSOList(int customerID = 0, string soNo = "", string quotationNo = "", string fromDate = "", string toDate =  "", int companyBranchId = 0)
        {
            List<CustomerSOViewModel> customerSOViewModellst = new List<CustomerSOViewModel>();
            DispatchPlanBL dispatchPlanBL = new DispatchPlanBL();
            try
            {
                customerSOViewModellst = dispatchPlanBL.GetSOList(customerID, soNo, quotationNo, fromDate, toDate, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(customerSOViewModellst);
        }
       
       
        [HttpGet]
        public PartialViewResult GetDispatchPlanList(string dispatchPlanNo = "", string customerName = "", int companyBranchId = 0, string fromDate = "", string toDate = "", string approvalStatus = "")
        {
            List<DispatchPlanViewModel> dispatchPlans = new List<DispatchPlanViewModel>();
            DispatchPlanBL dispatchPlanBL = new DispatchPlanBL();

            try
            {
                dispatchPlans = dispatchPlanBL.GetDispatchPlanList(dispatchPlanNo, customerName, companyBranchId, fromDate, toDate, approvalStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(dispatchPlans);
        }


        public JsonResult GetDispatchPlanDetail(int dispatchPlanID)
        {
            DispatchPlanBL dispatchPlanBL = new DispatchPlanBL();
            DispatchPlanViewModel dispatchPlanViewModel = new DispatchPlanViewModel();
            try
            {
                dispatchPlanViewModel = dispatchPlanBL.GetDispatchPlanDetail(dispatchPlanID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(dispatchPlanViewModel, JsonRequestBehavior.AllowGet);
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

       
        [HttpGet]
        public PartialViewResult GetApproveDispatchPlanList(string dispatchPlanNo = "", string customerName = "", int companyBranchId = 0, string fromDate = "", string toDate = "", string approvalStatus = "")
        {
            List<DispatchPlanViewModel> dispatchPlans = new List<DispatchPlanViewModel>();
            DispatchPlanBL dispatchPlanBL = new DispatchPlanBL();

            try
            {
                dispatchPlans = dispatchPlanBL.GetApproveDispatchPlanList(dispatchPlanNo, customerName, companyBranchId, fromDate, toDate, approvalStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(dispatchPlans);
        }

        [HttpGet]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Dispatch, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListApproveDP()
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


    }
}
