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
    public class EmployeeController : BaseController
    {
        //
        // GET: /Employee/
        #region Employee
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Employee, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmployee(int employeeId = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (employeeId != 0)
                {
                    ViewData["employeeId"] = employeeId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["employeeId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Employee, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditEmployee(EmployeeViewModel employeeViewModel, List<EmployeeSupportingDocumentViewModel> employeeDocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            EmployeeBL employeeBL = new EmployeeBL();
            try
            {
                if (employeeViewModel != null)
                {
                    employeeViewModel.CreatedBy = ContextUser.UserId;
                    employeeViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = employeeBL.AddEditEmployee(employeeViewModel, employeeDocuments);
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
        [HttpPost]
        public ActionResult UpdateEmployeePic()
        {
            ResponseOut responseOut = new ResponseOut();
            EmployeeBL employeeBL = new EmployeeBL();
            HttpFileCollectionBase files = Request.Files;
            EmployeeViewModel employeeViewModel = new EmployeeViewModel();
            try
            {
                employeeViewModel.EmployeeId = Convert.ToInt32(Request["employeeId"]);
                //  Get all files from Request object  
                if (files != null && files.Count > 0 && Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                {
                    HttpPostedFileBase file = files[0];
                    string fname;
                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        employeeViewModel.EmployeePicName = employeeViewModel.EmployeeId.ToString() + "_" + fileName;
                        var path = Path.Combine(Server.MapPath("~/Images/EmployeeImages"), employeeViewModel.EmployeePicName);
                        file.SaveAs(path);
                        employeeViewModel.EmployeePicPath = path;

                        //queryDetail.QueryAttachment = new byte[file.ContentLength];
                        //file.InputStream.Read(queryDetail.QueryAttachment, 0, file.ContentLength);
                    }
                }

                if (employeeViewModel != null && !string.IsNullOrEmpty(employeeViewModel.EmployeePicPath))
                {
                    responseOut = employeeBL.UpdateEmployeePic(employeeViewModel);
                }
                else
                {
                    responseOut.message = "";
                    responseOut.status = ActionStatus.Success;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Employee, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListEmployee()
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
        public PartialViewResult GetEmployeeList(string employeeName = "", string employeeCode = "", string mobileNo="",string email="",string panNo="",int departmentId=0,string employeeType="0",string currentStatus="0",  string employeeStatus = "",int companyBranchId=0)
        {
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();
            EmployeeBL employeeBL = new EmployeeBL();
            try
            {
                employees = employeeBL.GetEmployeeList(employeeName, employeeCode, mobileNo, email, panNo, departmentId, employeeType, currentStatus, ContextUser.CompanyId, employeeStatus, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employees);
        }

        [HttpGet]
        public JsonResult GetEmployeeDetail(int employeeId)
        {
            EmployeeBL employeeBL = new EmployeeBL();
            EmployeeViewModel employee = new EmployeeViewModel();
            try
            {
                employee = employeeBL.GetEmployeeDetail(employeeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(employee, JsonRequestBehavior.AllowGet);
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

        [HttpGet]
        public JsonResult GetDesignationList(int departmentId)
        {
            DesignationBL designationBL = new DesignationBL();
            List<DesignationViewModel> designations = new List<DesignationViewModel>();
            try
            {

                designations = designationBL.GetDesignationList(departmentId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(designations, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult GetShortlistApplicantList(string applicantNo, string projectNo, string firstName, string lastName, Int32 resumeSource, Int32 designation, string fromDate, string toDate, string applicantShortlistStatus = "Shortlist")
        {
            List<ApplicantViewModel> applicants = new List<ApplicantViewModel>();
            ApplicantBL applicantBL = new ApplicantBL();
            try
            {
                applicants = applicantBL.GetShortlistApplicantList(applicantNo, projectNo, firstName, lastName, resumeSource, designation, fromDate, toDate, ContextUser.CompanyId, applicantShortlistStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(applicants);
        }


        [HttpGet]
        public JsonResult GetEmployeeAutoCompleteList(string term,long companyBranchId=0)
        {
            SaleTargetBL saleTargetBL = new SaleTargetBL();

            List<EmployeeViewModel> employeeList = new List<EmployeeViewModel>();
            try
            {
                employeeList = saleTargetBL.GetEmployeeAutoCompleteList(term, companyBranchId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(employeeList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Employee, (int)AccessMode.EditAccess, (int)RequestMode.Ajax)]
        public JsonResult RemoveImage(long employeeId)
        {
            ResponseOut responseOut = new ResponseOut();
            EmployeeBL employeeBL = new EmployeeBL();
            try
            {
                if (employeeId != 0)
                {
                    responseOut = employeeBL.RemoveImage(employeeId);
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


        /// <summary>
        /// This method is used to get employee list based on department.
        /// Author by : Dheeraj on 14 May,2022
        /// </summary>     
        /// <returns>
        /// This method retruns list of the object.
        /// </returns>
        [HttpGet]
        public JsonResult GetDesignationByDepartmentID()
        {
            List<EmployeeDesignationModel> lstEmployeeDesignations = new List<EmployeeDesignationModel>();
            DesignationBL designationBL = new DesignationBL();
            try
            {

                lstEmployeeDesignations = designationBL.GetDesignationByDepartmentID(3);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(lstEmployeeDesignations, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region EmployeeProfile

        [HttpGet]
        //[ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeProfile, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmployeeProfile(int employeeId = 0, int accessMode = 3)
        {
            try
            {
                if (employeeId != 0)
                {
                    ViewData["employeeId"] = employeeId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["employeeId"] = 0;
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
        //[ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeProfile, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditEmployeeProfile(EmployeeViewModel employeeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            EmployeeBL employeeBL = new EmployeeBL();
            try
            {
                if (employeeViewModel != null)
                {
                    employeeViewModel.CreatedBy = ContextUser.UserId;
                    employeeViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = employeeBL.AddEditEmployee(employeeViewModel,null);
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
        public JsonResult GetEmployeeProfile(int employeeId)
        {
            EmployeeBL employeeBL = new EmployeeBL();
            EmployeeViewModel employee = new EmployeeViewModel();
            try
            {
                employee = employeeBL.GetEmployeeDetail(employeeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(employee, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveSupportingDocument()
        {
            ResponseOut responseOut = new ResponseOut();
            HttpFileCollectionBase files = Request.Files;
            Random rnd = new Random();
            try
            {
                //  Get all files from Request object  
                if (files != null && files.Count > 0 && Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                {
                    HttpPostedFileBase file = files[0];
                    string fname;
                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    if (file != null && file.ContentLength > 0)
                    {
                        string newFileName = "";
                        var fileName = Path.GetFileName(file.FileName);
                        newFileName = Convert.ToString(rnd.Next(10000, 99999)) + "_" + fileName;
                        var path = Path.Combine(Server.MapPath("~/Images/EmployeeDocument"), newFileName);
                        file.SaveAs(path);
                        responseOut.status = ActionStatus.Success;
                        responseOut.message = newFileName;
                    }
                    else
                    {
                        responseOut.status = ActionStatus.Fail;
                        responseOut.message = "";
                    }
                }
                else
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = "";
                }


            }
            catch (Exception ex)
            {
                responseOut.message = "";
                responseOut.status = ActionStatus.Fail;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult GetEmployeeSupportingDocumentList(List<EmployeeSupportingDocumentViewModel> employeeDocuments, int employeeID)
        {

            EmployeeBL employeeBL = new EmployeeBL();
            try
            {
                if (employeeDocuments == null)
                {
                    employeeDocuments = employeeBL.GetEmployeeSupportingDocumentList(employeeID);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employeeDocuments);
        }

        public FileResult DocumentDownload(string fileName)
        {
            var path = Path.Combine(Server.MapPath("~/Images/EmployeeDocument"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        #endregion

    }
}
