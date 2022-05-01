using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;
using System.IO;
namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class ChasisSerialMappingController : BaseController
    {
        #region  ChasisSerialMapping
        //
        // GET: /ChasisSerialMapping/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ChasisSerialMapping, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditChasisSerialMapping(int chasisSerialMappingId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (chasisSerialMappingId != 0)
                {
                    ViewData["chasisSerialMappingId"] = chasisSerialMappingId;
                    ViewData["accessMode"] = accessMode;
                    
                }
                else
                {
                    ViewData["chasisSerialMappingId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ChasisSerialMapping, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditChasisSerialMapping(ChasisSerialMappingViewModel chasisSerialMappingViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            ChasisSerialMappingBL chasisSerialMappingBL = new ChasisSerialMappingBL();
            try
            {
                if (chasisSerialMappingViewModel != null)
                {
                    chasisSerialMappingViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = chasisSerialMappingBL.AddEditChasisSerialMapping(chasisSerialMappingViewModel);

                    ChasisSerialSingleton.ResetChasisSerialSingleton();
                    ChasisSerialSingleton pro = ChasisSerialSingleton.Instance();
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ChasisSerialMapping, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListChasisSerial()
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
      
        [HttpGet]
        public PartialViewResult GetChasisSerialList(int productId, string ChasisSerialNo, string MotorNo, string ControllerNo,int companyBranchId=0,string serialStatus="")
        {
            List<ChasisSerialMappingViewModel> chasisSerialMappings = new List<ChasisSerialMappingViewModel>();
            
            ChasisSerialMappingBL chasisSerialMappingBL = new ChasisSerialMappingBL();
            try
            {
                chasisSerialMappings = chasisSerialMappingBL.GetChasisSerialList(productId, ChasisSerialNo,MotorNo,ControllerNo, companyBranchId, serialStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(chasisSerialMappings);
        }


        [HttpGet]
        public JsonResult GetChasisSerialAutoCompleteList(string term,int companyBranchId)
        {
           

            ChasisSerialPlanBL chasisSerialPlanBL = new ChasisSerialPlanBL();
            List<ChasisSerialMappingViewModel> chasisSerialList = new List<ChasisSerialMappingViewModel>();
            try
            {
                ChasisSerialSingleton.ResetChasisSerialSingleton();
                ChasisSerialSingleton pro = ChasisSerialSingleton.Instance();
                //chasisSerialList = chasisSerialPlanBL.GetChasisSerialNoAutoCompleteList(term);
                chasisSerialList = ChasisSerialSingleton.GetChasisSerials(term, companyBranchId);
                    
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(chasisSerialList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetChasisSerialMappingDetail(long mappingId)
        {
            ChasisSerialMappingBL chasisSerialMappingBL = new ChasisSerialMappingBL();
            ChasisSerialMappingViewModel chasisSerialMapping = new ChasisSerialMappingViewModel();
            try
            {
                chasisSerialMapping = chasisSerialMappingBL.GetChasisSerialMappingDetail(mappingId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(chasisSerialMapping, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
