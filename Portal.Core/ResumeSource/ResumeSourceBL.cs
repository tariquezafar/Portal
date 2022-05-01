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
    public class ResumeSourceBL
    {
        HRMSDBInterface dbInterface;
        public ResumeSourceBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditResumeSource(HR_ResumeSourceViewModel resumesourceViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_ResumeSource resumesource = new HR_ResumeSource
                {
                    ResumeSourceId = resumesourceViewModel.ResumeSourceId,
                    ResumeSourceName = resumesourceViewModel.ResumeSourceName, 
                    Status = resumesourceViewModel.ResumeSource_Status
                };
                responseOut = dbInterface.AddEditResumeSource(resumesource);
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

        public List<HR_ResumeSourceViewModel> GetResumeSourceList(string resumesourceName = "", string Status = "")
        {
            List<HR_ResumeSourceViewModel> resumesources = new List<HR_ResumeSourceViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtResumeSources = sqlDbInterface.GetResumeSourceList(resumesourceName, Status);
                if (dtResumeSources != null && dtResumeSources.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtResumeSources.Rows)
                    {
                        resumesources.Add(new HR_ResumeSourceViewModel
                        {
                            ResumeSourceId = Convert.ToInt32(dr["ResumeSourceId"]),
                            ResumeSourceName = Convert.ToString(dr["ResumeSourceName"]),
                            ResumeSource_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return resumesources;
        }

        public HR_ResumeSourceViewModel GetResumeSourceDetail(int resumesourceId = 0)
        {
            HR_ResumeSourceViewModel resumesource = new HR_ResumeSourceViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtResumeSources = sqlDbInterface.GetResumeSourceDetail(resumesourceId);
                if (dtResumeSources != null && dtResumeSources.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtResumeSources.Rows)
                    {
                        resumesource = new HR_ResumeSourceViewModel
                        {
                            ResumeSourceId = Convert.ToInt32(dr["ResumeSourceId"]),
                            ResumeSourceName = Convert.ToString(dr["ResumeSourceName"]),
                            ResumeSource_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return resumesource;
        }
        public List<HR_ResumeSourceViewModel> GetResumeSourceList()
        {
            List<HR_ResumeSourceViewModel> resumeSources = new List<HR_ResumeSourceViewModel>();
            try
            {
                List<HR_ResumeSource> resumeSourceList = dbInterface.GetResumeSourceList();
                if (resumeSourceList != null && resumeSourceList.Count > 0)
                {
                    foreach (HR_ResumeSource resumeSource in resumeSourceList)
                    {
                        resumeSources.Add(new HR_ResumeSourceViewModel { ResumeSourceId = resumeSource.ResumeSourceId,ResumeSourceName=resumeSource.ResumeSourceName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return resumeSources;
        }
    }
}
