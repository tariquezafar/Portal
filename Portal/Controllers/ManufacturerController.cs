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
    public class ManufacturerController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Manufacturer, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditManufacturer(int manufacturerId = 0, int accessMode = 3)
        {

            try
            {
                if (manufacturerId != 0)
                {
                    ViewData["manufacturerId"] = manufacturerId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["manufacturerId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Manufacturer, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditManufacturer(ManufacturerViewModel manufacturerViewModel)
        {
            ResponseOut responseOut = new ResponseOut();                     
            ManufacturerBL ManufacturerBL = new ManufacturerBL();
            try
            {
                if (manufacturerViewModel != null)
                {

                    manufacturerViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = ManufacturerBL.AddEditManufacturer(manufacturerViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Manufacturer, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListManufacturer()
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
        public PartialViewResult GetManufacturerList(string manufacturerName = "", string manufacturerCode = "", string manufacturerStatus = "")
        {
            List<ManufacturerViewModel> manufacturerViewModels = new List<ManufacturerViewModel>();

            ManufacturerBL manufacturerBL = new ManufacturerBL();
            try
            {
                manufacturerViewModels = manufacturerBL.GetManufacturerList(manufacturerName, manufacturerCode, manufacturerStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(manufacturerViewModels);
        }     
        [HttpGet]
        public JsonResult GetManufacturereDetail(int manufacturerId)
        {                       
            ManufacturerBL manufacturerBL = new ManufacturerBL();
            ManufacturerViewModel manufacturerViewModel = new ManufacturerViewModel();
            try
            {
                manufacturerViewModel = manufacturerBL.GetManufacturereDetail(manufacturerId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(manufacturerViewModel, JsonRequestBehavior.AllowGet);
        }

    }
}
