using Portal.Common;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class EmployeeStateMapController : BaseController
    {
        //
        // GET: /EmployeeStateMap/

        #region Employee State Mapping
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Employee_State_Mapping_CRM, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmpStateMapping(int accessMode = 3)
        {
            try
            {
                ViewData["accessMode"] = accessMode;
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Employee_State_Mapping_CRM, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditEmpStateMapping(List<EmployeeStateMappingViewModel> employeeStateMappingList)
        {
            ResponseOut responseOut = new ResponseOut();
            EmployeeStateMappingBL employeeStateMappingBL = new EmployeeStateMappingBL();
            try
            {
                if (employeeStateMappingList != null && employeeStateMappingList.Count > 0)
                {
                    foreach (var item in employeeStateMappingList)
                    {
                        EmployeeStateMapping empStateMapping = new EmployeeStateMapping
                        {
                            EmployeeStateMappingId = item.EmployeeStateMappingId,
                            EmployeeId = item.EmployeeId,
                            StateId = item.StateId,
                            CreatedBy = ContextUser.UserId,
                            CreatedDate= DateTime.Now,
                            Status = item.SelectState
                        };
                        responseOut = employeeStateMappingBL.AddEditEmpStateMapping(empStateMapping);
                    }
                    
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
        public PartialViewResult GetStateList(int employeeId = 0)
        {
            List<EmployeeStateMappingViewModel> states = new List<EmployeeStateMappingViewModel>();
            EmployeeStateMappingBL employeeStateMappingBL = new EmployeeStateMappingBL();
            try
            {
                states = employeeStateMappingBL.GetStateList(employeeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(states);
        }

        [HttpGet]
        public JsonResult GetEmployeeAutoCompleteList(string term)
        {
            EmployeeStateMappingBL employeeStateMappingBL = new EmployeeStateMappingBL();

            List<EmployeeViewModel> employeeList = new List<EmployeeViewModel>();
            try
            {
                employeeList = employeeStateMappingBL.GetEmployeeAutoCompleteList(term);
                
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(employeeList, JsonRequestBehavior.AllowGet);
        }
       

       

        #endregion

    }   
}
