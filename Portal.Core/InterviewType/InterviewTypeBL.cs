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
    public class InterviewTypeBL
    {
        HRMSDBInterface dbInterface;
        public InterviewTypeBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditInterviewType(InterviewTypeViewModel interviewtypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_InterviewType interviewtype = new HR_InterviewType
                {
                    InterviewTypeId = interviewtypeViewModel.InterviewTypeId,
                    InterviewTypeName = interviewtypeViewModel.InterviewTypeName, 
                    Status = interviewtypeViewModel.InterviewType_Status
                };
                responseOut = dbInterface.AddEditInterviewType(interviewtype);
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

        public List<InterviewTypeViewModel> GetInterviewTypeList(string interviewtypeName = "", string Status = "")
        {
            List<InterviewTypeViewModel> interviewtypes = new List<InterviewTypeViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtInterviewTypes = sqlDbInterface.GetInterviewTypeList(interviewtypeName, Status);
                if (dtInterviewTypes != null && dtInterviewTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtInterviewTypes.Rows)
                    {
                        interviewtypes.Add(new InterviewTypeViewModel
                        {
                            InterviewTypeId = Convert.ToInt32(dr["InterviewTypeId"]),
                            InterviewTypeName = Convert.ToString(dr["InterviewTypeName"]), 
                            InterviewType_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return interviewtypes;
        }

        public InterviewTypeViewModel GetInterviewTypeDetail(int interviewtypeId = 0)
        {
            InterviewTypeViewModel interviewtype = new InterviewTypeViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtInterviewTypes = sqlDbInterface.GetInterviewTypeDetail(interviewtypeId);
                if (dtInterviewTypes != null && dtInterviewTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtInterviewTypes.Rows)
                    {
                        interviewtype = new InterviewTypeViewModel
                        {
                            InterviewTypeId = Convert.ToInt32(dr["InterviewTypeId"]),
                            InterviewTypeName = Convert.ToString(dr["InterviewTypeName"]),
                            InterviewType_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return interviewtype;
        }
         
    }
}
