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
    public class PMS_EmployeeAppraisalTemplateMappingBL
    {
        HRMSDBInterface dbInterface;
        public PMS_EmployeeAppraisalTemplateMappingBL()
        {
            dbInterface = new HRMSDBInterface();
        }
        public ResponseOut AddEditEmployeeAppraisalTemplateMapping(PMS_EmployeeAppraisalTemplateMappingViewModel employeeAppraisalTemplateMappingViewModel, List<PMS_EmployeeGoalsViewModel> employeeGoals)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                HR_PMS_EmployeeAppraisalTemplateMapping employeeAppraisalTemplateMapping = new HR_PMS_EmployeeAppraisalTemplateMapping
                {
                    EmpAppraisalTemplateMappingId = employeeAppraisalTemplateMappingViewModel.EmpAppraisalTemplateMappingId,
                    EmployeeId = employeeAppraisalTemplateMappingViewModel.EmployeeId,
                    TemplateId = employeeAppraisalTemplateMappingViewModel.TemplateId,
                    PerformanceCycleId = employeeAppraisalTemplateMappingViewModel.PerformanceCycleId,
                    FinYearId = employeeAppraisalTemplateMappingViewModel.FinYearId,
                    CompanyId = employeeAppraisalTemplateMappingViewModel.CompanyId,
                    CreatedBy = employeeAppraisalTemplateMappingViewModel.CreatedBy,
                    Status = employeeAppraisalTemplateMappingViewModel.EmpAppraisalTemplateMapping_Status,
                    CompanyBranchId = employeeAppraisalTemplateMappingViewModel.CompanyBranchId
                };
               
                List<HR_PMS_EmployeeGoals> employeeGoalsList = new List<HR_PMS_EmployeeGoals>();
                if (employeeGoals != null && employeeGoals.Count > 0)
                {
                    foreach (PMS_EmployeeGoalsViewModel item in employeeGoals)
                    {
                        employeeGoalsList.Add(new HR_PMS_EmployeeGoals
                        {
                            GoalId = item.GoalId,
                            GoalName = item.GoalName,
                            GoalDescription = item.GoalDescription,
                            SectionId = item.SectionId,
                            GoalCategoryId = item.GoalCategoryId,
                            EvalutionCriteria = item.EvalutionCriteria,
                            StartDate =Convert.ToDateTime(item.StartDate),
                            DueDate = Convert.ToDateTime(item.DueDate),
                            Weight = item.Weight,
                            Status = item.EmployeeGoal_Status,
                            FixedDyanmic = item.FixedDyanmic
                        });
                    }
                } 
                responseOut = sqlDbInterface.AddEditEmployeeAppraisalTemplateMapping(employeeAppraisalTemplateMapping, employeeGoalsList); 

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


        public PMS_EmployeeAppraisalTemplateMappingViewModel GetEmployeeAppraisalTemplateMappingDetail(long empAppraisalTemplateMappingId = 0)
        {
            PMS_EmployeeAppraisalTemplateMappingViewModel empAppraisalTemplate = new PMS_EmployeeAppraisalTemplateMappingViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtEmpAppraisalTemplate = sqlDbInterface.GetEmployeeAppraisalTemplateMappingDetail(empAppraisalTemplateMappingId);
                if (dtEmpAppraisalTemplate != null && dtEmpAppraisalTemplate.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEmpAppraisalTemplate.Rows)
                    {
                        empAppraisalTemplate = new PMS_EmployeeAppraisalTemplateMappingViewModel
                        {
                            EmpAppraisalTemplateMappingId = Convert.ToInt32(dr["EmpAppraisalTemplateMappingId"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            TemplateId = Convert.ToInt32(dr["TemplateId"]),
                            TemplateName = Convert.ToString(dr["TemplateName"]),
                            PerformanceCycleId = Convert.ToInt32(dr["PerformanceCycleId"]),
                            FinYearId = Convert.ToInt32(dr["FinYearId"]),
                            DepartmentId = Convert.ToInt32(dr["DepartmentId"]),
                            DepartmentName = Convert.ToString(dr["DepartmentName"]),
                            DesignationId = Convert.ToInt32(dr["DesignationId"]),
                            DesignationName = Convert.ToString(dr["DesignationName"]),
                            EmpAppraisalTemplateMapping_Status = Convert.ToBoolean(dr["Status"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return empAppraisalTemplate;
        }

         
        public List<PMS_EmployeeAppraisalTemplateMappingViewModel> GetEmployeeAppraisalTemplateMappingList(string templateName, string employeeName, int companyId,string employeeMapping_Status,int companyBranchId)
        {
            List<PMS_EmployeeAppraisalTemplateMappingViewModel> appraisaltemplates = new List<PMS_EmployeeAppraisalTemplateMappingViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetEmployeeAppraisalTemplateMappingLists(templateName,employeeName,companyId, employeeMapping_Status, companyBranchId);
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        appraisaltemplates.Add(new PMS_EmployeeAppraisalTemplateMappingViewModel
                        {
                            EmpAppraisalTemplateMappingId = Convert.ToInt32(dr["EmpAppraisalTemplateMappingId"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            TemplateId = Convert.ToInt32(dr["TemplateId"]),
                            TemplateName = Convert.ToString(dr["TemplateName"]),
                            PerformanceCycleId = Convert.ToInt32(dr["PerformanceCycleId"]),
                            PerformanceCycleName = Convert.ToString(dr["PerformanceCycleName"]),
                            FinYearId = Convert.ToInt32(dr["FinYearId"]),
                            FinYearDesc = Convert.ToString(dr["FinYearCode"]),
                            DepartmentId = Convert.ToInt32(dr["DepartmentId"]),
                            DepartmentName = Convert.ToString(dr["DepartmentName"]),
                            DesignationId = Convert.ToInt32(dr["DesignationId"]),
                            DesignationName = Convert.ToString(dr["DesignationName"]),

                            EmpAppraisalTemplateMapping_Status = Convert.ToBoolean(dr["Status"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]), 
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchName= Convert.ToString(dr["CompanyBranchName"])
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



        public List<PMS_EmployeeGoalsViewModel> GetAppraisalTemplateGoalDetailList(long templateId)
        {
            List<PMS_EmployeeGoalsViewModel> goals = new List<PMS_EmployeeGoalsViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtGoals= sqlDbInterface.GetAppriasalTemplateGoalDetailList(templateId);
                if (dtGoals != null && dtGoals.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtGoals.Rows)
                    {
                        goals.Add(new PMS_EmployeeGoalsViewModel
                        {
                            EmployeeGoal_SequenceNo = Convert.ToInt32(dr["EmployeeGoal_SequenceNo"]),
                            EmployeeGoalId = Convert.ToInt32(dr["EmployeeGoalId"]),
                            GoalId = Convert.ToInt32(dr["GoalId"]),
                            GoalName = Convert.ToString(dr["GoalName"]),
                            GoalDescription = Convert.ToString(dr["GoalDescription"]),
                            SectionId = Convert.ToInt32(dr["SectionId"]),
                            SectionName = Convert.ToString(dr["SectionName"]),
                            GoalCategoryId = Convert.ToInt32(dr["GoalCategoryId"]),
                            GoalCategoryName = Convert.ToString(dr["GoalCategoryName"]),
                            EvalutionCriteria = Convert.ToString(dr["EvalutionCriteria"]),
                            StartDate = Convert.ToString(dr["StartDate"]),
                            DueDate = Convert.ToString(dr["DueDate"]),
                            Weight = Convert.ToDecimal(dr["Weight"]),
                            EmployeeGoal_Status = Convert.ToBoolean(dr["Status"]),
                            FixedDyanmic="F"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return goals;
        }

        public List<PMS_EmployeeGoalsViewModel> GetEmployeeGoalList(long empAppraisalTemplateMappingId)
        {
            List<PMS_EmployeeGoalsViewModel> goals = new List<PMS_EmployeeGoalsViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtGoals = sqlDbInterface.GetEmployeeGoalList(empAppraisalTemplateMappingId);
                if (dtGoals != null && dtGoals.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtGoals.Rows)
                    {
                        goals.Add(new PMS_EmployeeGoalsViewModel
                        {
                            EmployeeGoal_SequenceNo = Convert.ToInt32(dr["EmployeeGoal_SequenceNo"]),
                            EmployeeGoalId = Convert.ToInt32(dr["EmployeeGoalId"]),
                            GoalId = Convert.ToInt32(dr["GoalId"]),
                            GoalName = Convert.ToString(dr["GoalName"]),
                            GoalDescription = Convert.ToString(dr["GoalDescription"]),
                            SectionId = Convert.ToInt32(dr["SectionId"]),
                            SectionName = Convert.ToString(dr["SectionName"]),
                            GoalCategoryId = Convert.ToInt32(dr["GoalCategoryId"]),
                            GoalCategoryName = Convert.ToString(dr["GoalCategoryName"]),
                            EvalutionCriteria = Convert.ToString(dr["EvalutionCriteria"]),
                            StartDate = Convert.ToString(dr["StartDate"]),
                            DueDate = Convert.ToString(dr["DueDate"]),
                            Weight = Convert.ToDecimal(dr["Weight"]),
                            SelfScore= Convert.ToDecimal(dr["SelfScore"]),
                            SelfRemarks= Convert.ToString(dr["SelfRemarks"]),
                            AppraiserScore = Convert.ToDecimal(dr["AppraiserScore"]),
                            AppraiserRemarks= Convert.ToString(dr["AppraiserRemarks"]),
                            ReviewScore= Convert.ToDecimal(dr["ReviewScore"]),
                            ReviewRemarks= Convert.ToString(dr["ReviewRemarks"]),
                            EmployeeGoal_Status = Convert.ToBoolean(dr["Status"]),
                            FixedDyanmic=Convert.ToString(dr["FixedDyanmic"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return goals;
        }

        public ResponseOut AddEditEmployeeAssessment(PMS_EmployeeAppraisalTemplateMappingViewModel employeeAppraisalTemplateMappingViewModel, List<PMS_EmployeeGoalsViewModel> employeeGoals)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                HR_PMS_EmployeeAppraisalTemplateMapping employeeAppraisalTemplateMapping = new HR_PMS_EmployeeAppraisalTemplateMapping
                {
                    EmpAppraisalTemplateMappingId = employeeAppraisalTemplateMappingViewModel.EmpAppraisalTemplateMappingId,
                    EmployeeId = employeeAppraisalTemplateMappingViewModel.EmployeeId,
                    TemplateId = employeeAppraisalTemplateMappingViewModel.TemplateId,
                    PerformanceCycleId = employeeAppraisalTemplateMappingViewModel.PerformanceCycleId,
                    FinYearId = employeeAppraisalTemplateMappingViewModel.FinYearId,
                    CompanyId = employeeAppraisalTemplateMappingViewModel.CompanyId,
                    CreatedBy = employeeAppraisalTemplateMappingViewModel.CreatedBy,
                    Status = employeeAppraisalTemplateMappingViewModel.EmpAppraisalTemplateMapping_Status,
                    CompanyBranchId= employeeAppraisalTemplateMappingViewModel.CompanyBranchId,
                };

                List<HR_PMS_EmployeeGoals> employeeGoalsList = new List<HR_PMS_EmployeeGoals>();
                if (employeeGoals != null && employeeGoals.Count > 0)
                {
                    foreach (PMS_EmployeeGoalsViewModel item in employeeGoals)
                    {
                        employeeGoalsList.Add(new HR_PMS_EmployeeGoals
                        {
                            GoalId = item.GoalId,
                            GoalName = item.GoalName,
                            GoalDescription = item.GoalDescription,
                            SectionId = item.SectionId,
                            GoalCategoryId = item.GoalCategoryId,
                            EvalutionCriteria = item.EvalutionCriteria,
                            StartDate = Convert.ToDateTime(item.StartDate),
                            DueDate = Convert.ToDateTime(item.DueDate),
                            Weight = item.Weight,
                            Status = item.EmployeeGoal_Status,
                            SelfScore=item.SelfScore,
                            SelfRemarks=item.SelfRemarks,
                            AppraiserScore=item.AppraiserScore,
                            AppraiserRemarks=item.AppraiserRemarks,
                            ReviewScore=item.ReviewScore,
                            ReviewRemarks=item.ReviewRemarks,
                            FixedDyanmic = item.FixedDyanmic
                        });
                    }
                }
                responseOut = sqlDbInterface.AddEditEmployeeAssessment(employeeAppraisalTemplateMapping, employeeGoalsList);

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


    }
}
