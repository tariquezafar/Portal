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
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class ServicesController : BaseController
    {
        //
        // GET: /Services/
        #region Services
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Services_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditServices(int servicesId = 0, int accessMode = 3)
        {

            try
            {
                if (servicesId != 0)
                {

                    ViewData["servicesId"] = servicesId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["servicesId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Services_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditServices(ServicesViewModel servicesViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            ServicesBL servicesBL = new ServicesBL();
            try
            {
                if (servicesViewModel != null)
                {
                    // paymentModeViewModel.PaymentModeId= ContextUser.CompanyId;
                    // paymentModeViewModel.CreatedBy = ContextUser.UserId;
                   responseOut = servicesBL.AddEditServices(servicesViewModel);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Services_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListServices()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [HttpGet]
        public PartialViewResult GetServicesList(string servicesName = "", string Status = "")
        {
            List<ServicesViewModel> services = new List<ServicesViewModel>();
            ServicesBL servicesBL = new ServicesBL();
            try
            {
                services = servicesBL.GetServicesList(servicesName, Status);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(services);
        }


        [HttpGet]
        public JsonResult GetServicesDetail(int servicesId)
        {
            ServicesBL servicesBL = new ServicesBL();
            ServicesViewModel services = new ServicesViewModel();
            try
            {
                services = servicesBL.GetServicesDetail(servicesId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(services, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetServiceList(string servicesName = "", string Status = "")
        {
            List<ServicesViewModel> services = new List<ServicesViewModel>();
            ServicesBL servicesBL = new ServicesBL();
            try
            {
                services = servicesBL.GetServicesList(servicesName, Status);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(services, JsonRequestBehavior.AllowGet);
        }

        #endregion



    }
}
