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
    public class EducationBL
    {
        HRMSDBInterface dbInterface;
        public EducationBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditEducation(HR_EducationViewModel educationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_Education educationtype = new HR_Education
                {
                    EducationId = educationViewModel.EducationId,
                    EducationName = educationViewModel.EducationName,                 
                    Status = educationViewModel.Education_Status
                };
                responseOut = dbInterface.AddEditEducation(educationtype);
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

        public List<HR_EducationViewModel> GetEducationList(string educationName = "", string Status = "")
        {
            List<HR_EducationViewModel> educationViewModel = new List<HR_EducationViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dteducation = sqlDbInterface.GetEducationList(educationName, Status);
                if (dteducation != null && dteducation.Rows.Count > 0)
                {
                    foreach (DataRow dr in dteducation.Rows)
                    {
                        educationViewModel.Add(new HR_EducationViewModel
                        {
                            EducationId = Convert.ToInt32(dr["EducationId"]),
                            EducationName = Convert.ToString(dr["EducationName"]),                         
                            Education_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return educationViewModel;
        }

        public HR_EducationViewModel GetEducationDetail(int educationId = 0)
        {
            HR_EducationViewModel educationViewModel = new HR_EducationViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dteducation = sqlDbInterface.GetEducationDetail(educationId);
                if (dteducation != null && dteducation.Rows.Count > 0)
                {
                    foreach (DataRow dr in dteducation.Rows)
                    {
                        educationViewModel = new HR_EducationViewModel
                        {
                            EducationId = Convert.ToInt32(dr["EducationId"]),
                            EducationName = Convert.ToString(dr["EducationName"]),
                            Education_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return educationViewModel;
        }

        public List<HR_EducationViewModel> GetEducationList()
        {
            List<HR_EducationViewModel> educations = new List<HR_EducationViewModel>();
            try
            {
                List<HR_Education> educationList = dbInterface.GetEducationList();
                if (educationList != null && educationList.Count > 0)
                {
                    foreach (HR_Education education in educationList)
                    {
                        educations.Add(new HR_EducationViewModel { EducationId = education.EducationId, EducationName = education.EducationName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return educations;
        }


    }
}
