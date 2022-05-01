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
    public class ClearanceTemplateBL
    {
        HRMSDBInterface dbInterface;
        public ClearanceTemplateBL()
        {
            dbInterface = new HRMSDBInterface();
        }
        public ResponseOut AddEditClearanceTemplate(ClearanceTemplateViewModel clearancetemplateViewModel, List<ClearanceTemplateDetailViewModel> templateDetails)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                HR_ClearanceTemplate clearancetemplate = new HR_ClearanceTemplate
                {
                    ClearanceTemplateId = clearancetemplateViewModel.ClearanceTemplateId,
                    ClearanceTemplateName = clearancetemplateViewModel.ClearanceTemplateName,
                    DepartmentId = clearancetemplateViewModel.DepartmentId, 
                    Designationid = clearancetemplateViewModel.DesignationId,
                    SeparationCategoryId = clearancetemplateViewModel.SeparationCategoryId, 
                    CompanyId = clearancetemplateViewModel.CompanyId,
                    CreatedBy = clearancetemplateViewModel.CreatedBy,
                    Status = clearancetemplateViewModel.ClearanceTemplate_Status, 
                };
               
                List<HR_ClearanceTemplateDetail> templatedetailList = new List<HR_ClearanceTemplateDetail>();
                if (templateDetails != null && templateDetails.Count > 0)
                {
                    foreach (ClearanceTemplateDetailViewModel item in templateDetails)
                    {
                        templatedetailList.Add(new HR_ClearanceTemplateDetail
                        {
                            SeparationClearListId = item.SeparationClearListId,
                            ClearanceByUserId = item.ClearanceByUserId
                        });
                    }
                } 
                responseOut = sqlDbInterface.AddEditClearanceTemplate(clearancetemplate, templatedetailList); 

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
         
 

        public ClearanceTemplateViewModel GetClearanceTemplateDetail(long clearancetemplateId = 0)
        {
            ClearanceTemplateViewModel clearanceTemplate = new ClearanceTemplateViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetClearanceTemplateDetail(clearancetemplateId);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        clearanceTemplate = new ClearanceTemplateViewModel
                        {
                            ClearanceTemplateId = Convert.ToInt32(dr["ClearanceTemplateId"]),
                            ClearanceTemplateName = Convert.ToString(dr["ClearanceTemplateName"]),
                            DepartmentId = Convert.ToInt32(dr["DepartmentId"]),
                            DesignationId = Convert.ToInt32(dr["DesignationId"]),
                            SeparationCategoryId = Convert.ToInt32(dr["SeparationCategoryId"]),
                            ClearanceTemplate_Status = Convert.ToBoolean(dr["Status"]),
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
        public List<ClearanceTemplateViewModel> GetClearanceTemplateLists(string clearancetemplateName, int department, int designation, int separationCategory, int companyId,string clearancetemplateStatus)
        {
            List<ClearanceTemplateViewModel> clearancetemplates = new List<ClearanceTemplateViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetClearanceTemplateLists(clearancetemplateName, department, designation, separationCategory, companyId, clearancetemplateStatus);
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        clearancetemplates.Add(new ClearanceTemplateViewModel
                        {
                            ClearanceTemplateId = Convert.ToInt32(dr["ClearanceTemplateId"]),
                            ClearanceTemplateName = Convert.ToString(dr["ClearanceTemplateName"]),
                            DepartmentId = Convert.ToInt32(dr["DepartmentId"]),
                            DepartmentName = Convert.ToString(dr["DepartmentName"]),
                            DesignationId = Convert.ToInt32(dr["DesignationId"]),
                            DesignationName = Convert.ToString(dr["DesignationName"]),
                            SeparationCategoryId = Convert.ToInt32(dr["SeparationCategoryId"]),
                            SeparationCategoryName = Convert.ToString(dr["SeparationCategoryName"]),
                            ClearanceTemplate_Status = Convert.ToBoolean(dr["Status"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByUserName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]), 
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByUserName"]),
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



    }
}
