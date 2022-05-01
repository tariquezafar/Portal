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
namespace Portal.Core
{
    public class LanguageBL
    {
        HRMSDBInterface dbInterface;
        public LanguageBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditLanguage(HR_LanguageViewModel languageViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_Language languagetype = new HR_Language
                {
                    LanguageId = languageViewModel.LanguageId,
                    LanguageName = languageViewModel.LanguageName,                 
                    Status = languageViewModel.Language_Status
                };
                responseOut = dbInterface.AddEditLanguage(languagetype);
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

        public List<HR_LanguageViewModel> GetLanguageList(string languageName = "", string Status = "")
        {
            List<HR_LanguageViewModel> languageViewModel = new List<HR_LanguageViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dteducation = sqlDbInterface.GetLanguageList(languageName, Status);
                if (dteducation != null && dteducation.Rows.Count > 0)
                {
                    foreach (DataRow dr in dteducation.Rows)
                    {
                        languageViewModel.Add(new HR_LanguageViewModel
                        {
                            LanguageId = Convert.ToInt32(dr["LanguageId"]),
                            LanguageName = Convert.ToString(dr["LanguageName"]),                         
                            Language_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return languageViewModel;
        }

        public HR_LanguageViewModel GetLanguageDetail(int educationId = 0)
        {
            HR_LanguageViewModel languageViewModel = new HR_LanguageViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dteducation = sqlDbInterface.GetLanguageDetail(educationId);
                if (dteducation != null && dteducation.Rows.Count > 0)
                {
                    foreach (DataRow dr in dteducation.Rows)
                    {
                        languageViewModel = new HR_LanguageViewModel
                        {
                            LanguageId = Convert.ToInt32(dr["LanguageId"]),
                            LanguageName = Convert.ToString(dr["LanguageName"]),
                            Language_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return languageViewModel;
        }

      

    }
}
