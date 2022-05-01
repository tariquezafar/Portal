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
    public class ResourceRequisitionController : BaseController
    {
        #region Resource Requisition
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ResourceRequisition, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditResourceRequisition(int resourcerequisitionId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (resourcerequisitionId != 0)
                {
                    ViewData["resourcerequisitionId"] = resourcerequisitionId;
                    ViewData["accessMode"] = accessMode;
                    
                }
                else
                {
                    ViewData["resourcerequisitionId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ResourceRequisition, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditResourceRequisition(ResourceRequisitionViewModel resourceRequisitionViewModel, List<ResourceRequisitionSkillViewModel> skills,List<HR_ResourceRequisitionInterviewStageViewModel> interviews)
        {
            ResponseOut responseOut = new ResponseOut();
            ResourceRequisitionBL rrBL = new ResourceRequisitionBL();
            try
            {
                if (resourceRequisitionViewModel != null)
                {
                    resourceRequisitionViewModel.CompanyId = ContextUser.CompanyId;
                    resourceRequisitionViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = rrBL.AddEditResourceRequisition(resourceRequisitionViewModel, skills, interviews);

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ResourceRequisition, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListResourceRequisition()
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;

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
        public PartialViewResult GetResourceRequisitionList(string requisitionNo = "", int positionLevelId = 0, string priorityLevel = "", int positionTypeId = 0, int departmentId = 0, string approvalStatus = "0", string fromDate = "",string toDate="",int companyBranchId=0)
        {
            List<ResourceRequisitionViewModel> resourceRequisitions = new List<ResourceRequisitionViewModel>();
            ResourceRequisitionBL rrBL = new ResourceRequisitionBL();
            try
            {
                resourceRequisitions = rrBL.GetResourceRequisitionList(requisitionNo, positionLevelId, priorityLevel, positionTypeId, departmentId, approvalStatus, fromDate, toDate,ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(resourceRequisitions);
        }

        [HttpGet]
        public JsonResult GetResourceRequisitionDetail(long requisitionId)
        {
            ResourceRequisitionBL requisitionBL = new ResourceRequisitionBL();
            ResourceRequisitionViewModel requisition = new ResourceRequisitionViewModel();
            try
            {
                requisition = requisitionBL.GetResourceRequisitionDetail(requisitionId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(requisition, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult GetSkillList(List<ResourceRequisitionSkillViewModel> skills, long resourcerequisitionId)
        {
            ResourceRequisitionBL rrBL = new ResourceRequisitionBL();
            try
            {
                if (skills == null)
                {
                    skills = rrBL.GetResourceRequisitionSkillList(resourcerequisitionId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(skills);
        }

        [HttpPost]
        public PartialViewResult GetInterviewList(List<HR_ResourceRequisitionInterviewStageViewModel> interviews, long resourceRequisitionId)
        {
            ResourceRequisitionBL rrBL = new ResourceRequisitionBL();
            try
            {
                if (interviews == null)
                {
                    interviews = rrBL.GetResourceRequisitionInterviewStageList(resourceRequisitionId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(interviews);
        }





        #endregion
        #region Resource Requisition Approval
        [ValidateRequest(true, UserInterfaceHelper.ApproveResourceRequisition, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListResourceRequisitionApproval()
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;

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
        public PartialViewResult GetResourceRequisitionApprovalList(string requisitionNo = "", int positionLevelId = 0, string priorityLevel = "", int positionTypeId = 0, int departmentId = 0, string approvalStatus = "0", string fromDate = "", string toDate = "")
        {
            List<ResourceRequisitionViewModel> resourceRequisitions = new List<ResourceRequisitionViewModel>();
            ResourceRequisitionBL rrBL = new ResourceRequisitionBL();
            try
            {
                resourceRequisitions = rrBL.GetResourceRequisitionApprovalList(requisitionNo, positionLevelId, priorityLevel, positionTypeId, departmentId, approvalStatus, fromDate, toDate, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(resourceRequisitions);
        }
        [ValidateRequest(true, UserInterfaceHelper.ApproveResourceRequisition, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ApproveResourceRequisition(int resourcerequisitionId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (resourcerequisitionId != 0)
                {
                    ViewData["resourcerequisitionId"] = resourcerequisitionId;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["resourcerequisitionId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.ApproveResourceRequisition, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult ApproveResourceRequisition(ResourceRequisitionViewModel resourceRequisitionViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            ResourceRequisitionBL rrBL = new ResourceRequisitionBL();
            try
            {
                if (resourceRequisitionViewModel != null)
                {
                    resourceRequisitionViewModel.CompanyId = ContextUser.CompanyId;
                    resourceRequisitionViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = rrBL.ApproveRejectResourceRequisition(resourceRequisitionViewModel);

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


        #endregion

        #region Helper Method

        [HttpGet]
        public JsonResult GetPositionTypeList()
        {
            ResourceRequisitionBL rrBL = new ResourceRequisitionBL();
            List<PositionTypeViewModel> ptList = new List<PositionTypeViewModel>();
            try
            {
                ptList = rrBL.GetPositionTypeList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(ptList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPositionLevelList()
        {
            ResourceRequisitionBL rrBL = new ResourceRequisitionBL();
            List<PositionLevelViewModel> plList = new List<PositionLevelViewModel>();
            try
            {
                plList = rrBL.GetPositionLevelList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(plList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetInterviewTypeList()
        {
            ResourceRequisitionBL rrBL = new ResourceRequisitionBL();
            List<HR_InterviewTypeViewModel> interviewtypeList = new List<HR_InterviewTypeViewModel>();
            try
            {
                interviewtypeList = rrBL.GetInterviewTypeList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(interviewtypeList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEducationList()
        {
            ResourceRequisitionBL rrBL = new ResourceRequisitionBL();
            List<HR_EducationViewModel> educationList = new List<HR_EducationViewModel>();
            try
            {
                educationList = rrBL.GetEducationList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(educationList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetSkillAutoCompleteList(string term)
        {
            ResourceRequisitionBL requisitionBL = new ResourceRequisitionBL();
            List<SkillViewModel> skillList = new List<SkillViewModel>();
            try
            {
                skillList = requisitionBL.GetSkillAutoCompleteList(term);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(skillList, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
