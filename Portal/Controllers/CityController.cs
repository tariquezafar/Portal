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
    public class CityController : BaseController
    {
        //
        // GET: /City/

       [ValidateRequest(true, UserInterfaceHelper.Add_Edit_City_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditCity(int cityId = 0, int accessMode = 3)
        {

            try
            {
                if (cityId != 0)
                {
                    ViewData["cityId"] =cityId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["cityId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_City_CP, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditCity(CityViewModel cityViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            CityBL cityBL = new CityBL();
            try
            {
                if (cityViewModel != null)
                {
                    responseOut = cityBL.AddEditCity(cityViewModel);
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



        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_City_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListCity()
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
        public PartialViewResult GetCityList(string cityName = "",  int stateId = 0, int countryId=0)
        {
            List<CityViewModel> cities = new List<CityViewModel>();
            CityBL cityBL = new CityBL();
            try
            {
                cities = cityBL.GetCityList(cityName,stateId,countryId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(cities);
        }
        
        [HttpGet]
        public JsonResult GetCityDetail(int cityId)
        {
            
            CityBL cityBL = new CityBL();
            CityViewModel city = new CityViewModel();
            try
            {
                
                city = cityBL.GetCityDetail(cityId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(city, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetCountryList()
        {
            CountryBL countryBL = new CountryBL();
            List<CountryViewModel> countryList = new List<CountryViewModel>();
            try
            {
                countryList = countryBL.GetCountryList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(countryList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStateList(int countryId)
        {
            StateBL stateBL = new StateBL();
            List<StateViewModel> stateList = new List<StateViewModel>();
            try
            {
                stateList = stateBL.GetStateList(countryId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(stateList, JsonRequestBehavior.AllowGet);
        }
    }
}
