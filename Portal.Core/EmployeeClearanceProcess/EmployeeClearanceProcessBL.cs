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
    public class EmployeeClearanceProcessBL
    {
        HRMSDBInterface dbInterface;
        public EmployeeClearanceProcessBL()
        {
            dbInterface = new HRMSDBInterface();
        }
        public ResponseOut AddEditEmployeeClearanceProcessMapping(EmployeeClearanceProcessViewModel employeeClearanceProcessViewModel, List<EmployeeClearanceProcessDetailViewModel> employeeClearanceProcessDetails)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                HR_EmployeeClearanceProcess empClearanceProcess = new HR_EmployeeClearanceProcess
                {
                    EmployeeClearanceId= employeeClearanceProcessViewModel.EmployeeClearanceId,
                    EmployeeId = employeeClearanceProcessViewModel.EmployeeId,
                    ClearanceTemplateId = employeeClearanceProcessViewModel.ClearanceTemplateId,
                    ClearanceFinalStatus = employeeClearanceProcessViewModel.ClearanceFinalStatus, 
                    CompanyId = employeeClearanceProcessViewModel.CompanyId,
                    CreatedBy = employeeClearanceProcessViewModel.CreatedBy
                };
               
                List<HR_EmployeeClearanceProcessDetail> employeeClearanceProcessDetailList = new List<HR_EmployeeClearanceProcessDetail>();
                if (employeeClearanceProcessDetails != null && employeeClearanceProcessDetails.Count > 0)
                {
                    foreach (EmployeeClearanceProcessDetailViewModel item in employeeClearanceProcessDetails)
                    {
                        employeeClearanceProcessDetailList.Add(new HR_EmployeeClearanceProcessDetail
                        {
                            SeparationClearListId = item.SeparationClearListId,
                            ClearanceByUserId = item.ClearanceByUserId,
                            ClearanceStatus="Pending",
                            ClearanceRemarks=item.ClearanceRemarks
                        });
                    }
                } 
                responseOut = sqlDbInterface.AddEditEmployeeClearanceProcessMapping(empClearanceProcess, employeeClearanceProcessDetailList); 

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


        public ResponseOut AddEditEmployeeClearance(EmployeeClearanceProcessViewModel employeeClearanceProcessViewModel, List<EmployeeClearanceProcessDetailViewModel> employeeClearanceProcessDetails)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                HR_EmployeeClearanceProcess empClearanceProcess = new HR_EmployeeClearanceProcess
                {
                    EmployeeClearanceId = employeeClearanceProcessViewModel.EmployeeClearanceId,
                    EmployeeId = employeeClearanceProcessViewModel.EmployeeId,
                    ClearanceTemplateId = employeeClearanceProcessViewModel.ClearanceTemplateId,
                    ClearanceFinalStatus = employeeClearanceProcessViewModel.ClearanceFinalStatus,
                    CompanyId = employeeClearanceProcessViewModel.CompanyId,
                    CreatedBy = employeeClearanceProcessViewModel.CreatedBy
                };

                List<HR_EmployeeClearanceProcessDetail> employeeClearanceProcessDetailList = new List<HR_EmployeeClearanceProcessDetail>();
                if (employeeClearanceProcessDetails != null && employeeClearanceProcessDetails.Count > 0)
                {
                    foreach (EmployeeClearanceProcessDetailViewModel item in employeeClearanceProcessDetails)
                    {
                        employeeClearanceProcessDetailList.Add(new HR_EmployeeClearanceProcessDetail
                        {
                            SeparationClearListId = item.SeparationClearListId,
                            ClearanceByUserId = item.ClearanceByUserId,
                            ClearanceStatus = item.ClearanceStatus,
                            ClearanceRemarks = item.ClearanceRemarks
                        });
                    }
                }
                responseOut = sqlDbInterface.AddEditEmployeeClearance(empClearanceProcess, employeeClearanceProcessDetailList);

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


        public EmployeeClearanceProcessViewModel GetEmployeeClearanceProcessDetail(long employeeClearanceId = 0)
        {
            EmployeeClearanceProcessViewModel clearanceTemplate = new EmployeeClearanceProcessViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetEmployeeClearanceProcessDetail(employeeClearanceId);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        clearanceTemplate = new EmployeeClearanceProcessViewModel
                        {
                            EmployeeClearanceId = Convert.ToInt32(dr["EmployeeClearanceId"]),
                            EmployeeClaaranceNo = Convert.ToString(dr["EmployeeClaaranceNo"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            ClearanceTemplateId = Convert.ToInt32(dr["ClearanceTemplateId"]),
                            ClearanceTemplateName = Convert.ToString(dr["ClearanceTemplateName"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByUserName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByUserName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            ClearanceFinalStatus = Convert.ToString(dr["ClearanceFinalStatus"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return clearanceTemplate;
        }

        public ClearanceTemplateDetailViewModel GetClearanceTemplateDetailListForClearanceProcess(long clearancetemplateId = 0)
        {
            ClearanceTemplateDetailViewModel clearanceTemplate = new ClearanceTemplateDetailViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetClearanceTemplateDetailList(clearancetemplateId);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        clearanceTemplate = new ClearanceTemplateDetailViewModel
                        {
                            SeparationClearListId = Convert.ToInt32(dr["SeparationClearListId"]),
                            SeparationClearListName = Convert.ToString(dr["SeparationClearListName"]),
                            ClearanceByUserId = Convert.ToInt32(dr["ClearanceByUserId"]),
                            ClearanceByUserName = Convert.ToString(dr["ClearanceByUserName"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return clearanceTemplate;
        }
        public List<EmployeeClearanceProcessViewModel> GetEmployeeClearanceProcessMappingList(string employeeClearanceNo, long employeeId, int clearanceTemplateId, int companyId)
        {
            List<EmployeeClearanceProcessViewModel> clearancetemplates = new List<EmployeeClearanceProcessViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetEmployeeClearanceProcessMappingList(employeeClearanceNo, employeeId, clearanceTemplateId, companyId);
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        clearancetemplates.Add(new EmployeeClearanceProcessViewModel
                        {
                            EmployeeClearanceId = Convert.ToInt32(dr["EmployeeClearanceId"]),
                            EmployeeClaaranceNo = Convert.ToString(dr["EmployeeClaaranceNo"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            ClearanceTemplateId = Convert.ToInt32(dr["ClearanceTemplateId"]),
                            ClearanceTemplateName = Convert.ToString(dr["ClearanceTemplateName"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByUserName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]), 
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByUserName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            ClearanceFinalStatus = Convert.ToString(dr["ClearanceFinalStatus"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return clearancetemplates;
        }

      



        public List<ClearanceTemplateDetailViewModel> GetClearanceTemplateDetailList(long ClearancetemplateId)
        {
            List<ClearanceTemplateDetailViewModel> details = new List<ClearanceTemplateDetailViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetClearanceTemplateDetailList(ClearancetemplateId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        details.Add(new ClearanceTemplateDetailViewModel
                        {
                            TaxSequenceNo= Convert.ToInt32(dr["SNo"]),
                            SeparationClearListId = Convert.ToInt32(dr["SeparationClearListId"]),
                            SeparationClearListName = Convert.ToString(dr["SeparationClearListName"]),
                            ClearanceByUserId = Convert.ToInt32(dr["ClearanceByUserId"]),
                            ClearanceByUserName = Convert.ToString(dr["ClearanceByUserName"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return details;
        }
 

        public List<ClearanceTemplateViewModel> GetClearanceTemplateList(int companyId=0)
        {
            List<ClearanceTemplateViewModel> clearancetemplates = new List<ClearanceTemplateViewModel>();
            try
            {
                List<HR_ClearanceTemplate>  clearancetemplateList = dbInterface.GetClearanceTemplateList(companyId);
                if (clearancetemplateList != null && clearancetemplateList.Count > 0)
                {
                    foreach (HR_ClearanceTemplate advance in clearancetemplateList)
                    {
                        clearancetemplates.Add(new ClearanceTemplateViewModel { ClearanceTemplateId = advance.ClearanceTemplateId, ClearanceTemplateName = advance.ClearanceTemplateName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return clearancetemplates;
        }

        public List<ClearanceTemplateDetailViewModel> GetClearanceProcessList(int clearancetemplateId, int separationclearlistId, int clearancebyuserId)
        {
            List<ClearanceTemplateDetailViewModel> clearancetemplates = new List<ClearanceTemplateDetailViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetClearanceProcessList(clearancetemplateId, separationclearlistId, clearancebyuserId);
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        clearancetemplates.Add(new ClearanceTemplateDetailViewModel
                        {

                            ClearanceTemplateId = Convert.ToInt32(dr["ClearanceTemplateId"]),
                            ClearanceTemplateName = Convert.ToString(dr["ClearanceTemplateName"]),
                            SeparationClearListId = Convert.ToInt32(dr["SeparationClearListId"]),
                            SeparationClearListName = Convert.ToString(dr["SeparationClearListName"]),
                            ClearanceByUserId = Convert.ToInt32(dr["ClearanceByUserId"]),
                            ClearanceByUserName = Convert.ToString(dr["ClearanceByUserName"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return clearancetemplates;
        }

        public List<EmployeeClearanceProcessDetailViewModel> GetEmployeeClearanceProcesses(long employeeClearanceId)
        {
            List<EmployeeClearanceProcessDetailViewModel> clearancetemplates = new List<EmployeeClearanceProcessDetailViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetEmployeeClearanceProcesses(employeeClearanceId);
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        clearancetemplates.Add(new EmployeeClearanceProcessDetailViewModel
                        {
                            TaxSequenceNo = Convert.ToInt32(dr["SNo"]),
                            EmployeeClearanceDetailId = Convert.ToInt32(dr["EmployeeClearanceDetailId"]),
                            SeparationClearListId = Convert.ToInt32(dr["SeparationClearListId"]),
                            SeparationClearListName = Convert.ToString(dr["SeparationClearListName"]),
                            ClearanceByUserId = Convert.ToInt32(dr["ClearanceByUserId"]),
                            ClearanceByUserName = Convert.ToString(dr["ClearanceByUserName"]),
                            ClearanceStatus = Convert.ToString(dr["ClearanceStatus"]),
                            ClearanceRemarks = Convert.ToString(dr["ClearanceRemarks"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return clearancetemplates;
        }


    }
}
