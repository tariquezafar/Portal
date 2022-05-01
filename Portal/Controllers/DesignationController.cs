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
    public class DesignationController : BaseController
    {
        //
        // GET: /User/
        #region Designation
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Designation_HR, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditDesignation(int designationId = 0, int accessMode = 3)
        {

            try
            {
                if (designationId != 0)
                {

                    ViewData["designationId"] = designationId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["designationId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Designation_HR, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditDesignation(DesignationViewModel designationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            DesignationBL designationBL = new DesignationBL();
            try
            {
                if (designationViewModel != null)
                { 
                    designationViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = designationBL.AddEditDesignation(designationViewModel);
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
        public JsonResult GetDepartmentList()
        {
            DepartmentBL departmentBL = new DepartmentBL();
            List<DepartmentViewModel> department = new List<DepartmentViewModel>();
            try
            {

                department = departmentBL.GetDepartmentList(ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(department, JsonRequestBehavior.AllowGet);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Designation_HR, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListDesignation()
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
        public PartialViewResult GetDesignationList(string designationName = "", string designationCode = "", int departmentId = 0,  string Status = "")
        {
            List<DesignationViewModel> designation = new List<DesignationViewModel>();
            DesignationBL designationBL = new DesignationBL();
            try
            {
                designation = designationBL.GetDesignationList(designationName, designationCode, departmentId, Status);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(designation);
        }


        [HttpGet]
        public JsonResult GetDesignationDetail(int designationId)
        {
            DesignationBL designationBL = new DesignationBL();
            DesignationViewModel designation = new DesignationViewModel();
            try
            {
                designation = designationBL.GetDesignationDetail(designationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(designation, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}