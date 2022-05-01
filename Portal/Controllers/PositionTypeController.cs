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
    public class PositionTypeController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PositionType, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditPositionType(int positiontypeId = 0, int accessMode = 3)
        {

            try
            {
                if (positiontypeId != 0)
                {
                    ViewData["positiontypeId"] = positiontypeId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["positiontypeId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PositionType, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditPositionType(PositionTypeViewModel positiontypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            PositionTypeBL producttypeBL = new PositionTypeBL();
            try
            {
                if (positiontypeViewModel != null)
                {
                    responseOut = producttypeBL.AddEditPositionType(positiontypeViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PositionType, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListPositionType()
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
        public PartialViewResult GetPositionTypeList(string positiontypeName = "", string positiontypeCode = "", string positiontypeStatus = "")
        {
            List<PositionTypeViewModel> positiontypes = new List<PositionTypeViewModel>();
            PositionTypeBL positiontypeBL = new PositionTypeBL();
            try
            {
                positiontypes = positiontypeBL.GetPositionTypeList(positiontypeName, positiontypeCode, positiontypeStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(positiontypes);
        }
         

        [HttpGet]
        public JsonResult GetPositionTypeDetail(int positiontypeId)
        {
            PositionTypeBL positiontypeBL = new PositionTypeBL();
            PositionTypeViewModel positiontype = new PositionTypeViewModel();
            try
            {
                positiontype = positiontypeBL.GetPositionTypeDetail(positiontypeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(positiontype, JsonRequestBehavior.AllowGet);
        }

    }
}
