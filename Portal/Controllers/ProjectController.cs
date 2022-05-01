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
    public class ProjectController : BaseController
    {
        //
        // GET: /Project/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Project, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditProject(int projectId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (projectId != 0)
                {
                    ViewData["projectId"] = projectId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["projectId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Project, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditProject(ProjectViewModel projectViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            ProjectBL projectBL = new ProjectBL();
            try
            {
                if (projectViewModel != null)
                { 
                    projectViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = projectBL.AddEditProject(projectViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Project, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListProject()
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
        public JsonResult GetCustomerBranchListByID(int customerId)
        {
            CustomerBL customerBL = new CustomerBL();
            List<CustomerBranchViewModel> customerBranchList = new List<CustomerBranchViewModel>();
            try
            {
                customerBranchList = customerBL.GetCustomerBranches(customerId); //productBL.GetProductMainGroupNameByProductID(productId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(customerBranchList, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetProjectDetail(int projectId)
        {

            ProjectBL projectBL = new ProjectBL();
            ProjectViewModel project = new ProjectViewModel();
            try
            {

                project = projectBL.GetProjectDetail(projectId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(project, JsonRequestBehavior.AllowGet);
        }

        [HttpGet] 
        public PartialViewResult GetProjectList(string projectName = "", int projectCode = 0, int customerId = 0 ,int customerBranchId=0,string projectStatus="",int companyBranchId=0)
        {
            List<ProjectViewModel> projects = new List<ProjectViewModel>();
            ProjectBL projectBL = new ProjectBL();
            try
            {
                projects = projectBL.GetProjectList(projectName, projectCode, customerId, customerBranchId, projectStatus, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(projects);
        }
    }
}
