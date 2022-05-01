using Portal.Common;
using Portal.Core;
using Portal.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class ThoughtController : BaseController
    {
        //
        // GET: /Thought/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Thought, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditThought(int thoughtId = 0, int accessMode = 3)
        {

            try
            {
                if (thoughtId != 0)
                {
                    ViewData["thoughtId"] = thoughtId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["thoughtId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Thought, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditThought(ThoughtViewModel thoughtViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            ThoughtBL thoughtBL = new ThoughtBL();
            try
            {
                if (thoughtViewModel != null)
                {
                    responseOut = thoughtBL.AddEditThought(thoughtViewModel);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Thought, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListThought()
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
        public PartialViewResult GetThoughtList(string thoughtName = "", string thoughtType = "")
        {
            List<ThoughtViewModel> thoughts = new List<ThoughtViewModel>();
            ThoughtBL thoughtBL = new ThoughtBL();
            try
            {
                thoughts = thoughtBL.GetThoughtList(thoughtName,thoughtType);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(thoughts);
        }

        [HttpGet]
        public JsonResult GetThoughtDetail(int thoughtId)
        {
            ThoughtBL thoughtBL = new ThoughtBL();
            ThoughtViewModel thought = new ThoughtViewModel();
            try
            {
                thought = thoughtBL.GetThoughtDetail(thoughtId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(thought, JsonRequestBehavior.AllowGet);
        }
    }
}
