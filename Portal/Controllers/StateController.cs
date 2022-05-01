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
    public class StateController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_State_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditState(int stateId = 0, int accessMode = 3)
        {

            try
            {
                if (stateId != 0)
                {
                    ViewData["stateId"] = stateId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["stateId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_State_CP, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditState(StateViewModel stateViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            StateBL stateBL = new StateBL();
            try
            {
                if (stateViewModel != null)
                {
                    responseOut = stateBL.AddEditState(stateViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_State_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListState()
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
        public PartialViewResult GetStateList(string stateName = "", string stateCode = "", int countryId = 0)
        {
            List<StateViewModel> states = new List<StateViewModel>();
            StateBL stateBL = new StateBL();
            try
            {
                states = stateBL.GetStateList(stateName, stateCode, countryId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(states);
        }

        //[ValidateRequest(true, UserInterfaceHelper.Add_Edit_Company_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        //public ActionResult SuperAdminDashboard()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
        //    }
        //    return View();
        //}


        [HttpGet]
        public JsonResult GetStateDetail(int stateId)
        {
            StateBL stateBL = new StateBL();
            StateViewModel state = new StateViewModel();
            try
            {
                state = stateBL.GetStateDetail(stateId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(state, JsonRequestBehavior.AllowGet);
        }

    }
}
