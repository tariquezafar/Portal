using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;


namespace Portal.Controllers
{
    public class ServiceController : BaseController
    {
        #region Service
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Service, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditService(int serviceId = 0, int accessMode =0 )
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                if (serviceId != 0)
                {
                    ViewData["serviceId"] = serviceId;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["serviceId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Service, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditService(ServicViewModel servicViewModel ,List<ServiceViewModel> serviceViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            ServiceBL serviceBL = new ServiceBL();
            try
            {
                if (servicViewModel != null)
                {
                    servicViewModel.CreatedBy = ContextUser.UserId;
                    servicViewModel.CompanyId = ContextUser.CompanyId;                    
                    responseOut = serviceBL.AddEditService(servicViewModel, serviceViewModel);
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
        public PartialViewResult GetServiceProductList(List<ServiceViewModel> serviceProducts, long serviceId)
        {
            ServiceBL serviceBL = new ServiceBL();
            try
            {
                if (serviceProducts == null)
                {
                    serviceProducts = serviceBL.GetServiceProductList(serviceId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(serviceProducts);
        }

        [HttpGet]
        public JsonResult GetServiceDetail(long serviceId)
        {
            ServiceBL serviceBL = new ServiceBL();
            ServicViewModel servicViewModel = new ServicViewModel();
            try
            {
                servicViewModel = serviceBL.GetServiceDetail(serviceId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(servicViewModel, JsonRequestBehavior.AllowGet);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Service, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListService()
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();                
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [HttpGet]
        public PartialViewResult GetServiceList(string serviceNo = "", string approvalStatus = "",string fromDate = "", string toDate = "")
        {
            List<ServicViewModel> servicViewModel = new List<ServicViewModel>();
            ServiceBL serviceBL = new ServiceBL();
            try
            {
                servicViewModel = serviceBL.GetServiceList(serviceNo, approvalStatus, fromDate, toDate, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(servicViewModel);
        }
        #endregion
    }
}
