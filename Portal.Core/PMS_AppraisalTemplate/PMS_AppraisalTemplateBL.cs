using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Core.ViewModel;
using Portal.DAL;
using Portal.Common;
using System.Reflection;
using System.Data;
using Portal.DAL.Infrastructure;
using System.Transactions;
namespace Portal.Core
{
    public class PMS_AppraisalTemplateBL
    {
        HRMSDBInterface dbInterface;
        public PMS_AppraisalTemplateBL()
        {
            dbInterface = new HRMSDBInterface();
        }
        public ResponseOut AddEditAppraisalTemplate(PMS_AppraisalTemplateViewModel templateViewModel, List<PMS_AppraisalTemplateGoalViewModel> templateGoals)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                HR_PMS_AppraisalTemplate appraisaltemplate = new HR_PMS_AppraisalTemplate
                {
                    TemplateId = templateViewModel.TemplateId,
                    TemplateName = templateViewModel.TemplateName,
                    DepartmentId = templateViewModel.DepartmentId, 
                    DesignationId = templateViewModel.DesignationId,  
                    CompanyId = templateViewModel.CompanyId,
                    CreatedBy = templateViewModel.CreatedBy,
                    Status = templateViewModel.AppraisalTemplate_Status,
                    CompanyBranchId=templateViewModel.CompanyBranchId 
                };
               
                List<HR_PMS_AppraisalTemplateGoal> templateGoalsList = new List<HR_PMS_AppraisalTemplateGoal>();
                if (templateGoals != null && templateGoals.Count > 0)
                {
                    foreach (PMS_AppraisalTemplateGoalViewModel item in templateGoals)
                    {
                        templateGoalsList.Add(new HR_PMS_AppraisalTemplateGoal
                        {
                            GoalId = item.GoalId,
                            Status = item.Goal_Status
                        });
                    }
                } 
                responseOut = sqlDbInterface.AddEditAppraisalTemplate(appraisaltemplate, templateGoalsList); 

            }

            catch (Exception ex)
            {
                responseOut.status = ActionStatus.Fail;
                responseOut.message = ActionMessage.ApplicationException;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }

            return responseOut;
        }
         
        public List<PMS_AppraisalTemplateGoalViewModel> GetAppraisalTemplateGoalList(long templateId)
        {
            List<PMS_AppraisalTemplateGoalViewModel> templateGoals = new List<PMS_AppraisalTemplateGoalViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetAppraisalTemplateGoalList(templateId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        templateGoals.Add(new PMS_AppraisalTemplateGoalViewModel
                        {
                            TemplateGoalId = Convert.ToInt32(dr["TemplateGoalId"]),
                            TaxSequenceNo= Convert.ToInt32(dr["SNo"]),
                            GoalId = Convert.ToInt32(dr["GoalId"]),
                            GoalName = Convert.ToString(dr["GoalName"]), 
                            Goal_Status=Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return templateGoals;
        }


        public PMS_AppraisalTemplateViewModel GetAppraisalTemplateDetail(long templateId = 0)
        {
            PMS_AppraisalTemplateViewModel appraisalTemplate = new PMS_AppraisalTemplateViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetAppraisalTemplateDetail(templateId);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        appraisalTemplate = new PMS_AppraisalTemplateViewModel
                        { 
                            TemplateId = Convert.ToInt32(dr["TemplateId"]),
                            TemplateName = Convert.ToString(dr["TemplateName"]),
                            DepartmentId = Convert.ToInt32(dr["DepartmentId"]),
                            DesignationId = Convert.ToInt32(dr["DesignationId"]),
                            AppraisalTemplate_Status = Convert.ToBoolean(dr["Status"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchId= Convert.ToInt32(dr["CompanyBranchId"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return appraisalTemplate;
        }

         
        public List<PMS_AppraisalTemplateViewModel> GetAppraisalTemplateLists(string templateName, int department, int designation, int companyId,string appraisaltemplateStatus,int companyBranchId)
        {
            List<PMS_AppraisalTemplateViewModel> appraisaltemplates = new List<PMS_AppraisalTemplateViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetAppraisalTemplateLists(templateName, department, designation,  companyId, appraisaltemplateStatus, companyBranchId);
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        appraisaltemplates.Add(new PMS_AppraisalTemplateViewModel
                        {
                            TemplateId = Convert.ToInt32(dr["TemplateId"]),
                            TemplateName = Convert.ToString(dr["TemplateName"]),
                            DepartmentId = Convert.ToInt32(dr["DepartmentId"]),
                            DepartmentName = Convert.ToString(dr["DepartmentName"]),
                            DesignationId = Convert.ToInt32(dr["DesignationId"]),
                            DesignationName = Convert.ToString(dr["DesignationName"]),
                            AppraisalTemplate_Status = Convert.ToBoolean(dr["Status"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]), 
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchName= Convert.ToString(dr["CompanyBranchName"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return appraisaltemplates;
        }

        public List<PMS_AppraisalTemplateViewModel> GetAppraisalTemplateDetailList(string templateName, int department, int designation, int companyId)
        {
            List<PMS_AppraisalTemplateViewModel> appraisaltemplates = new List<PMS_AppraisalTemplateViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetAppraisalTemplateDetailList(templateName, department, designation, companyId);
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        appraisaltemplates.Add(new PMS_AppraisalTemplateViewModel
                        {
                            TemplateId = Convert.ToInt32(dr["TemplateId"]),
                            TemplateName = Convert.ToString(dr["TemplateName"]),
                            DepartmentId = Convert.ToInt32(dr["DepartmentId"]),
                            DepartmentName = Convert.ToString(dr["DepartmentName"]),
                            DesignationId = Convert.ToInt32(dr["DesignationId"]),
                            DesignationName = Convert.ToString(dr["DesignationName"]),
                            AppraisalTemplate_Status = Convert.ToBoolean(dr["Status"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return appraisaltemplates;
        }



        //public List<PMS_AppraisalTemplateGoalViewModel> GetGoalTemplateList(long templateId)
        //{
        //    List<PMS_AppraisalTemplateGoalViewModel> goals = new List<PMS_AppraisalTemplateGoalViewModel>();
        //    HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
        //    try
        //    {
        //        DataTable dtCustomers = sqlDbInterface.GetGoalTemplateList(templateId);
        //        if (dtCustomers != null && dtCustomers.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dtCustomers.Rows)
        //            {
        //                goals.Add(new PMS_AppraisalTemplateGoalViewModel
        //                {
        //                    GoalId = Convert.ToInt32(dr["GoalId"]),
        //                    GoalName = Convert.ToString(dr["GoalName"]), 
        //                });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
        //        throw ex;
        //    }
        //    return goals;
        //}


    }
}
