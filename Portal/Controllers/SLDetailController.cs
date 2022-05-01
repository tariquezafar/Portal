using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;
using Portal.DAL;

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class SLDetailController :BaseController
    {
        //
        // GET: /SLDetail/2078

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SLDetail, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditSLDetail(int sLDetailId = 0, int accessMode = 3)
        {

            try
            {

                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (sLDetailId != 0)
                {

                    ViewData["sLDetailId"] = sLDetailId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["sLDetailId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SLDetail, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditSLDetail(List<SLDetailViewModel> sLDetailList)
        {
            ResponseOut responseOut = new ResponseOut();
            EmployeeStateMappingBL employeeStateMappingBL = new EmployeeStateMappingBL();
            SLDetailBL sLDetailBL = new SLDetailBL();
            int companyId = 0;
            int finYearID = 0;
            int createdBY = 0;
            try
            {

                //------Comment This Code By Dheeraj Date 20-Jun-2018
                //------Insert Data By Used Procedure Send Multiple Data 

                //if (sLDetailList != null && sLDetailList.Count > 0)
                //{
                //    foreach (var item in sLDetailList)
                //    {
                //        decimal openingBalance = Convert.ToDecimal(item.OpeningBalanceDebit) - Convert.ToDecimal(item.OpeningBalanceCredit);
                //        SLDetail sLDetail = new SLDetail
                //        {
                //         SLDetailId =item.SLDetailId,
                //         GLId =item.GLId,
                //         SLId = item.SLId,
                //         CompanyId =ContextUser.CompanyId,
                //         CompanyBranchId = item.CompanyBranchId,
                //         FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year,
                //         OpeningBalance =openingBalance,
                //         OpeningBalanceDebit=item.OpeningBalanceDebit,
                //         OpeningBalanceCredit=item.OpeningBalanceCredit,
                //         CreatedBy=ContextUser.UserId,
                //         CreatedDate=DateTime.Now,
                //         Status=item.SLDetailStatus
                //         };
                //        responseOut = sLDetailBL.AddEditSLDetail(sLDetail);
                //    }

                //}               
                //else
                //{
                //    responseOut.message = ActionMessage.ProbleminData;
                //    responseOut.status = ActionStatus.Fail;
                //}

                 companyId = ContextUser.CompanyId;
                 finYearID = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                 createdBY = ContextUser.UserId;
              
                responseOut = sLDetailBL.AddEditSLDetail(sLDetailList, companyId, finYearID, createdBY);

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
        public PartialViewResult GetSLDetailList(int SLTypeId = 0, int GLId = 0, int SLId = 0,int companyBranchId=0)
        {
            List<SLDetailViewModel> sL = new List<SLDetailViewModel>();
            SLDetailBL sLDetailBL = new SLDetailBL();
            try
            {
                int FinId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                sL = sLDetailBL.GetSLDetailList(SLTypeId, GLId, SLId ,FinId,ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(sL);
        }

        [HttpGet]
        public JsonResult GetGLAutoCompleteList(string term, int slTypeId)
        {
            GLBL glBL = new GLBL();
            List<GLViewModel> slList = new List<GLViewModel>();
            try
            {
                slList = glBL.GetGLAutoCompleteList(term, slTypeId, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(slList, JsonRequestBehavior.AllowGet);
        }
    }
}
