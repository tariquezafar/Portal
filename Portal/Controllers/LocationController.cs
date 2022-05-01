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
using System.Data;
using System.Text;
namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class LocationController : BaseController
    {
        #region Location
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Location, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditLocation(int locationId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                if (locationId != 0)
                {
                    ViewData["locationId"] = locationId;
                    ViewData["accessMode"] = accessMode;
                    
                }
                else
                {
                    ViewData["locationId"] = 0;
                    ViewData["accessMode"] = 3;
                    
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Location, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditLocation(LocationViewModel locationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            LocationBL locationBL = new LocationBL();
            try
            {
                if (locationViewModel != null)
                {
                    locationViewModel.CompanyId = ContextUser.CompanyId;
                    locationViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = locationBL.AddEditLocation(locationViewModel);

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Location, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListLocation()
        {
            try
            {
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
        public PartialViewResult GetLocationList(string locationName = "", string locationCode = "", int companybranchId = 0, string locationStatus = "")
        {
            List<LocationViewModel> locations = new List<LocationViewModel>();
            LocationBL locationBL = new LocationBL();
            try
            {
                locations = locationBL.GetLocationList(locationName, locationCode, companybranchId, ContextUser.CompanyId, locationStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(locations);
        }

        [HttpGet]
        public JsonResult GetLocationDetail(int locationId)
        {
            LocationBL locationBL = new LocationBL();
            LocationViewModel locations = new LocationViewModel();
            try
            {
                locations = locationBL.GetLocationDetail(locationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(locations, JsonRequestBehavior.AllowGet);
        }


        #endregion


    }
}
