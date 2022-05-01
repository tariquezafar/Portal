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
using System.Transactions;
namespace Portal.Core
{
    public class EmailTemplateBL
    {
        DBInterface dbInterface;
        public EmailTemplateBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditEmailTemplate(EmailTemplateViewModel emailTemplateViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
               EmailTemplate emailTemplate = new EmailTemplate
                {
                    EmailTemplateId = emailTemplateViewModel.EmailTemplateId,
                    EmailTemplateSubject=emailTemplateViewModel.EmailTemplateSubject,
                    EmailTemplateTypeId = emailTemplateViewModel.EmailTemplateTypeId,
                    EmailTemplateDesc = emailTemplateViewModel.EmailTemplateDesc,
                    CompanyId=emailTemplateViewModel.CompanyId,
                    CreatedBy=emailTemplateViewModel.CreatedBy,
                    CreatedDate=Convert.ToDateTime(emailTemplateViewModel.CreatedDate),
                    Status = emailTemplateViewModel.EmailTemplateStatus,
                    CompanyBranchId= emailTemplateViewModel.CompanyBranchId

               };
                responseOut = dbInterface.AddEditEmailTemplate(emailTemplate);
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
        public List<EmailTemplateTypeViewModel> GetEmailTemplateTypeList()
        {
            List<EmailTemplateTypeViewModel> emailTemplateTypeList = new List<EmailTemplateTypeViewModel>();
            try
            {
                List<EmailTemplateType> emailTemplateTypes = dbInterface.GetEmailTemplateType();
                if (emailTemplateTypes != null && emailTemplateTypes.Count > 0)
                {
                    foreach (EmailTemplateType emailTemplateList in emailTemplateTypes)
                    {
                        emailTemplateTypeList.Add(new EmailTemplateTypeViewModel
                        {
                          EmailTemplateTypeId = emailTemplateList.EmailTemplateTypeId,
                          EmailTemplateName = emailTemplateList.EmailTemplateName,
                          
                        });
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return emailTemplateTypeList;
        }

       

        public List<EmailTemplateViewModel> GetEmailTemplateList(string emailTemplateSubject="", int emailTemplateTypeId=0, int companyId=0, string status="",int companyBranchId=0)
        {
            List<EmailTemplateViewModel> emailTemplates = new List<EmailTemplateViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtEmailTemplates = sqlDbInterface.GetEmailTemplateList(emailTemplateSubject, emailTemplateTypeId, companyId, status, companyBranchId);
                if (dtEmailTemplates != null && dtEmailTemplates.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEmailTemplates.Rows)
                    {
                        emailTemplates.Add(new EmailTemplateViewModel
                        {

                            EmailTemplateId = Convert.ToInt32(dr["EmailTemplateId"]),
                            EmailTemplateSubject = Convert.ToString(dr["EmailTemplateSubject"]),
                            EmailTemplateTypeName = Convert.ToString(dr["EmailTemplateName"]),
                            EmailTemplateDesc = string.IsNullOrEmpty(Convert.ToString(dr["EmailTemplateDesc"]))?"": Convert.ToString(dr["EmailTemplateDesc"]),
                            CreatedByName = string.IsNullOrEmpty(Convert.ToString(dr["CreateByUserName"]))?"": Convert.ToString(dr["CreateByUserName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByName = string.IsNullOrEmpty(Convert.ToString(dr["ModifiedByUserName"])) ? "" : Convert.ToString(dr["ModifiedByUserName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            EmailTemplateStatus = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchName=Convert.ToString(dr["CompanyBranchName"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return emailTemplates;
        }
        public EmailTemplateViewModel GetEmailTemplateDetail(int emailTemplateId = 0)
        {
            EmailTemplateViewModel emailTemplate = new EmailTemplateViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtEmailTemplates = sqlDbInterface.GetEmailTemplateDetail(emailTemplateId);
                if (dtEmailTemplates != null && dtEmailTemplates.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEmailTemplates.Rows)
                    {
                       emailTemplate = new EmailTemplateViewModel
                        {

                            EmailTemplateId = Convert.ToInt32(dr["EmailTemplateId"]),
                            EmailTemplateSubject = Convert.ToString(dr["EmailTemplateSubject"]),
                            EmailTemplateTypeId = Convert.ToInt32(dr["EmailTemplateTypeId"]),
                            EmailTemplateTypeName = Convert.ToString(dr["EmailTemplateName"]),
                            EmailTemplateDesc = string.IsNullOrEmpty(Convert.ToString(dr["EmailTemplateDesc"])) ? "" : Convert.ToString(dr["EmailTemplateDesc"]),
                            CreatedByName = string.IsNullOrEmpty(Convert.ToString(dr["CreateByUserName"])) ? "" : Convert.ToString(dr["CreateByUserName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByName = string.IsNullOrEmpty(Convert.ToString(dr["ModifiedByUserName"])) ? "" : Convert.ToString(dr["ModifiedByUserName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            EmailTemplateStatus = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchId=Convert.ToInt32(dr["CompanyBranchId"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return emailTemplate;
        }

        public DataTable GetEmailTemplateDetailByEmailType(int emailTemplateTypeId=0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            
            DataTable dtEmail = new DataTable();
            try
            {
                dtEmail = sqlDbInterface.GetEmailTemplateDetailByEmailType(emailTemplateTypeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtEmail;
         }


    }
}
