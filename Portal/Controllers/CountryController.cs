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
    public class CountryController : BaseController
    {
        //
        // GET: /Country/
 
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Country_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditCountry(int countryId = 0, int accessMode = 3)
        {

            try
            {
                if (countryId != 0)
                {
                    ViewData["countryId"] = countryId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["countryId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Country_CP, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditCountry(CountryViewModel countryViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            CountryBL countryBL = new CountryBL();
            try
            {
                if (countryViewModel != null)
                {
                    responseOut = countryBL.AddEditCountry(countryViewModel);
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

        [HttpGet]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Country_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListCountry()
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
        public PartialViewResult GetCountryList(string countryName = "", string countryCode = "", string countryStatus = "")
        {
            List<CountryViewModel> countries = new List<CountryViewModel>();
            CountryBL countryBL = new CountryBL();
            try
            {
                countries = countryBL.GetCountryList(countryName, countryCode, countryStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(countries);
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

        //[HttpGet]
        //public JsonResult GetCountryList()
        //{
        //    CountryBL countryBL = new CountryBL();
        //    List<CountryViewModel> countryList = new List<CountryViewModel>();
        //    try
        //    {
        //        countryList = countryBL.GetCountryList();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
        //    }
        //    return Json(countryList, JsonRequestBehavior.AllowGet);
        //}
        //[HttpGet]
        //public JsonResult GetStateList(int countryId)
        //{
        //    StateBL stateBL = new StateBL();
        //    List<StateViewModel> stateList = new List<StateViewModel>();
        //    try
        //    {
        //        stateList = stateBL.GetStateList(countryId);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
        //    }
        //    return Json(stateList, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public JsonResult GetCountryDetail(int countryId)
        {
            CountryBL countryBL = new CountryBL();
            CountryViewModel country = new CountryViewModel();
            try
            {
                country = countryBL.GetCountryDetail(countryId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(country, JsonRequestBehavior.AllowGet);
        }

    }
}
