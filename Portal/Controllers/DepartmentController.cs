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
    public class DepartmentController : BaseController
    {
        //
        // GET: /User/
        #region Department
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Department_HR, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditDepartment(int departmentId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                if (departmentId != 0)
                {

                    ViewData["departmentId"] = departmentId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["departmentId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Department_HR, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditDepartment(DepartmentViewModel departmentViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            DepartmentBL departmentBL = new DepartmentBL();
            try
            {
                if (departmentViewModel != null)
                {
                    departmentViewModel.CompanyId = ContextUser.CompanyId;
                    departmentViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = departmentBL.AddEditDepartment(departmentViewModel);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Department_HR, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListDepartment()
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
        public PartialViewResult GetDepartmentList(string departmentName = "", string departmentCode = "", string Status = "",int companyBranchId=0)
        {
            List<DepartmentViewModel> department = new List<DepartmentViewModel>();
            DepartmentBL departmentBL = new DepartmentBL();
            try
            {
                department = departmentBL.GetDepartmentList(departmentName, departmentCode, Status, ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(department);
        }


        [HttpGet]
        public JsonResult GetDepartmentDetail(int departmentId)
        {
            DepartmentBL departmentBL = new DepartmentBL();
            DepartmentViewModel department = new DepartmentViewModel();
            try
            {
                department = departmentBL.GetDepartmentDetail(departmentId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(department, JsonRequestBehavior.AllowGet);
        }



   
         
        #endregion


    }
}