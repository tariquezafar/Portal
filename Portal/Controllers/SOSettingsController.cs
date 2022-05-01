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
    public class SOSettingsController : BaseController
    {
        //
        // GET: /User/
        #region QuotationSetting
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SOSetting_SALE, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditSOSetting(int sosettingId = 0, int accessMode = 3)
        {

            try
            {
                if (sosettingId != 0)
                {
                    ViewData["sosettingId"] = sosettingId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["sosettingId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SOSetting_SALE, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditSOSetting(SOSettingViewModel sosettingViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SOSettingBL sosettingBL = new SOSettingBL();
            try
            {
                if (sosettingViewModel != null)
                {
                    sosettingViewModel.CreatedBy = ContextUser.UserId;
                    sosettingViewModel.CompanyId = ContextUser.CompanyId;

                    responseOut = sosettingBL.AddEditSOSetting(sosettingViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SOSetting_SALE, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListSOSetting()
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


        public PartialViewResult GetSOSettingList(string status = "")
        {
            List<SOSettingViewModel> sosettings = new List<SOSettingViewModel>();
            SOSettingBL sosettingBL = new SOSettingBL();
            try
            {
                sosettings = sosettingBL.GetSOSettingList(ContextUser.CompanyId, status);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(sosettings);
        }


        public JsonResult GetSOSettingDetail(int sosettingId)
        {
            SOSettingBL sosettingBL = new SOSettingBL();
           SOSettingViewModel sosetting = new SOSettingViewModel();
            try
            {
                sosetting = sosettingBL.GetSOSettingDetail(sosettingId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(sosetting, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUserAutoCompleteList(string term)
        {
           SOSettingBL sosettingBL = new SOSettingBL();

            List<UserViewModel> userList = new List<UserViewModel>();
            try
            {
                userList = sosettingBL.GetUserAutoCompleteList(term);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(userList, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
