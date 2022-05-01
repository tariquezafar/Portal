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
    public class TravelTypeController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_TravelType, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditTravelType(int traveltypeId = 0, int accessMode = 3)
        {

            try
            {
                if (traveltypeId != 0)
                {
                    ViewData["traveltypeId"] = traveltypeId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["traveltypeId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_TravelType, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditTravelType(HR_TravelTypeViewModel traveltypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            TravelTypeBL traveltypeBL = new TravelTypeBL();
            try
            {
                if (traveltypeViewModel != null)
                {
                    responseOut = traveltypeBL.AddEditTravelType(traveltypeViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_TravelType, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListTravelType()
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
        public PartialViewResult GetTravelTypeList(string traveltypeName = "", string traveltypeStatus = "")
        {
            List<HR_TravelTypeViewModel> traveltypes = new List<HR_TravelTypeViewModel>();
            TravelTypeBL traveltypeBL = new TravelTypeBL();
            try
            {
                traveltypes = traveltypeBL.GetTravelTypeList(traveltypeName, traveltypeStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(traveltypes);
        }


        [HttpGet]
        public JsonResult GetTravelTypeDetail(int traveltypeId)
        {
            TravelTypeBL traveltypeBL = new TravelTypeBL();
            HR_TravelTypeViewModel traveltype = new HR_TravelTypeViewModel();
            try
            {
                traveltype = traveltypeBL.GetTravelTypeDetail(traveltypeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(traveltype, JsonRequestBehavior.AllowGet);
        }

    }
}
