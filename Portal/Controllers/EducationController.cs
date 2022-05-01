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
    public class EducationController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Education_HR, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEducation(int educationId = 0, int accessMode = 3)
        {

            try
            {
                if (educationId != 0)
                {
                    ViewData["educationId"] = educationId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["educationId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Education_HR, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditEducation(HR_EducationViewModel eductaionViewModel)
        {
            ResponseOut responseOut = new ResponseOut();          
          
            EducationBL educationBL = new EducationBL();
            try
            {
                if (eductaionViewModel != null)
                {
                    responseOut = educationBL.AddEditEducation(eductaionViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Education_HR, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListEducationType()
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
        public PartialViewResult GetEducationList(string educationName = "", string educationStatus = "")
        {
            List<HR_EducationViewModel> educationViewModel = new List<HR_EducationViewModel>();

            EducationBL educationBL = new EducationBL();
            try
            {
                educationViewModel = educationBL.GetEducationList(educationName, educationStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(educationViewModel);
        }     
        [HttpGet]
        public JsonResult GetEducationDetail(int educationId)
        {
            
         
            EducationBL educationBL = new EducationBL();
            HR_EducationViewModel educationViewModel = new HR_EducationViewModel();
            try
            {
                educationViewModel = educationBL.GetEducationDetail(educationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(educationViewModel, JsonRequestBehavior.AllowGet);
        }

    }
}
