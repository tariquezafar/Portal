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
using System.Web.Mvc;
using System.IO;
using static Portal.Common.SendMail;

namespace Portal.Core   
{
   public class CustomMailBL
    {
        DBInterface dbInterface;
        public CustomMailBL()
        {
            dbInterface = new DBInterface();
        }

        public ResponseOut SendCustom(CustomMailViewModel customMailViewModel, List<MailSupportingDocumentViewModel> mailDocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                List<CustomMailSupportingDocumentViewModel> customMailSupportingDocuments = new List<CustomMailSupportingDocumentViewModel>();
                if (mailDocuments != null)
                {
                    foreach (MailSupportingDocumentViewModel items in mailDocuments)
                    {
                        CustomMailSupportingDocumentViewModel customMailSupportingDocumentViewModel = new CustomMailSupportingDocumentViewModel
                        {
                            DocumentName = items.DocumentName,
                            file = items.file,
                        };
                        customMailSupportingDocuments.Add(customMailSupportingDocumentViewModel);
                    }
                }

                StringBuilder mailBody = new StringBuilder(" ");
                SendMail sendMail = new SendMail();
                mailBody.Append("<html><head></head><body>");
                mailBody.Append(customMailViewModel.MailBody);
                mailBody.Append("</body></html>");
                UserEmailSettingBL userEmailSettingBL = new UserEmailSettingBL();
                DataTable userEmailSetting = userEmailSettingBL.GetUserEmailSettingDetailDataTable(customMailViewModel.CreatedBy);
                bool sendMailStatus = false;
                if (userEmailSetting.Rows.Count > 0)
                {
                    sendMailStatus = sendMail.SendEmail(userEmailSetting.Rows[0]["SmtpUser"].ToString(), customMailViewModel.MailTo, customMailViewModel.MailSubject, mailBody.ToString(), customMailSupportingDocuments,customMailViewModel.MailSubject,userEmailSetting.Rows[0]["SmtpPass"].ToString(), userEmailSetting.Rows[0]["SmtpDisplayName"].ToString(), userEmailSetting.Rows[0]["SmtpServer"].ToString(),Convert.ToInt32(userEmailSetting.Rows[0]["SmtpPort"]), Convert.ToBoolean(userEmailSetting.Rows[0]["EnableSsl"]), customMailViewModel.MailCC, customMailViewModel.MailBCC);
                }
                else
                {
                    sendMailStatus = sendMail.SendEmail(customMailViewModel.MailFrom, customMailViewModel.MailTo, customMailViewModel.MailSubject, mailBody.ToString());
                }

                if (sendMailStatus)
                {

                    MailLog maillog = new MailLog
                    {
                        MailLogId = customMailViewModel.MailLogId,
                        MailFrom = customMailViewModel.MailFrom,
                        MailTo = customMailViewModel.MailTo,
                        MailCC = customMailViewModel.MailCC,
                        MailBCC = customMailViewModel.MailBCC,
                        MailSubject = customMailViewModel.MailSubject,
                        MailTypeId = string.IsNullOrEmpty(customMailViewModel.MailTypeId.ToString()) ? 0 : customMailViewModel.MailTypeId,
                        MailBody = customMailViewModel.MailBody,
                        WithAttachment = string.IsNullOrEmpty(customMailViewModel.WithAttachment.ToString()) ? true : false,
                        CompanyId = customMailViewModel.CompanyId,
                        CreatedBy = customMailViewModel.CreatedBy,
                    };
                    responseOut = dbInterface.AddCustomMailLog(maillog);
                    responseOut.message = "Mail Sent Successfully";
                    responseOut.status = ActionStatus.Success;
                }
                else
                {
                    responseOut.message = "Problem in Sending Mail!!!";
                    responseOut.status = ActionStatus.Fail;

                }

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
        public List<MailSupportingDocumentViewModel> GetMailSupportingDocumentList(MailSupportingDocumentViewModel mailDocuments)
        {
            List<MailSupportingDocumentViewModel> mailDocumentList = new List<MailSupportingDocumentViewModel>();
            try
            {
                mailDocumentList.Add(new MailSupportingDocumentViewModel
                {
                    DocumentSequenceNo = mailDocuments.DocumentSequenceNo,
                    DocumentTypeDesc = mailDocuments.DocumentTypeDesc,
                    DocumentName = mailDocuments.DocumentName,
                    DocumentPath = mailDocuments.DocumentPath
                });
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return mailDocumentList;
        }
    }
}

