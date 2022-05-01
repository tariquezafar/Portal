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
    public class ReligionBL
    {
        HRMSDBInterface dbInterface;
        public ReligionBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditReligion(HR_ReligionViewModel religionViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_Religion hR_Religion = new HR_Religion
                {
                    ReligionId = religionViewModel.ReligionId,
                    ReligionName = religionViewModel.ReligionName,                 
                    Status = religionViewModel.Religion_Status
                };
                responseOut = dbInterface.AddEditReligion(hR_Religion);
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

        public List<HR_ReligionViewModel> GetReligionList(string religionName = "", string Status = "")
        {
            List<HR_ReligionViewModel> religionViewModel = new List<HR_ReligionViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dteducation = sqlDbInterface.GetReligionList(religionName, Status);
                if (dteducation != null && dteducation.Rows.Count > 0)
                {
                    foreach (DataRow dr in dteducation.Rows)
                    {
                        religionViewModel.Add(new HR_ReligionViewModel
                        {
                            ReligionId = Convert.ToInt32(dr["ReligionId"]),
                            ReligionName = Convert.ToString(dr["ReligionName"]),                         
                            Religion_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return religionViewModel;
        }

        public HR_ReligionViewModel GetReligionDetail(int religionId = 0)
        {
            HR_ReligionViewModel religionViewModel = new HR_ReligionViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dteducation = sqlDbInterface.GetReligionDetail(religionId);
                if (dteducation != null && dteducation.Rows.Count > 0)
                {
                    foreach (DataRow dr in dteducation.Rows)
                    {
                        religionViewModel = new HR_ReligionViewModel
                        {
                            ReligionId = Convert.ToInt32(dr["ReligionId"]),
                            ReligionName = Convert.ToString(dr["ReligionName"]),
                            Religion_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return religionViewModel;
        }

      

    }
}
