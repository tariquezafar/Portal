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
    public class PMS_SectionController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PMS_Section, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditSection(int sectionId = 0, int accessMode = 3)
        {

            try
            {
                if (sectionId != 0)
                {
                    ViewData["sectionId"] = sectionId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["sectionId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PMS_Section, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditSection(PMS_SectionViewModel pmssectionViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            PMS_SectionBL pmssectionBL = new PMS_SectionBL();
            try
            {
                if (pmssectionViewModel != null)
                {
                    responseOut = pmssectionBL.AddEditSection(pmssectionViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PMS_Section, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListSection()
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
        public PartialViewResult GetSectionList(string sectionName = "", string sectionStatus = "")
        {
            List<PMS_SectionViewModel> pmssections = new List<PMS_SectionViewModel>();
            PMS_SectionBL pmssectionBL = new PMS_SectionBL();
            try
            {
                pmssections = pmssectionBL.GetSectionList(sectionName, sectionStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(pmssections);
        }


        [HttpGet]
        public JsonResult GetSectionDetail(int sectionId)
        {
            PMS_SectionBL pmssectionBL = new PMS_SectionBL();
            PMS_SectionViewModel pmssection = new PMS_SectionViewModel();
            try
            {
                pmssection = pmssectionBL.GetSectionDetail(sectionId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(pmssection, JsonRequestBehavior.AllowGet);
        }

    }
}
