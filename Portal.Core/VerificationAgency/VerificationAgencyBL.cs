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
    public class VerificationAgencyBL
    {
        HRMSDBInterface dbInterface;
        public VerificationAgencyBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditVerificationAgency(HR_VerificationAgencyViewModel verificationagencyViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_VerificationAgency verificationagency = new HR_VerificationAgency
                {
                    VerificationAgencyId = verificationagencyViewModel.VerificationAgencyId,
                    VerificationAgencyName = verificationagencyViewModel.VerificationAgencyName, 
                    Status = verificationagencyViewModel.VerificationAgency_Status
                };
                responseOut = dbInterface.AddEditVerificationAgency(verificationagency);
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

        public List<HR_VerificationAgencyViewModel> GetVerificationAgencyList(string verificationagencyName = "", string Status = "")
        {
            List<HR_VerificationAgencyViewModel> verificationagencys = new List<HR_VerificationAgencyViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtVerificationAgencys = sqlDbInterface.GetVerificationAgencyList(verificationagencyName, Status);
                if (dtVerificationAgencys != null && dtVerificationAgencys.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtVerificationAgencys.Rows)
                    {
                        verificationagencys.Add(new HR_VerificationAgencyViewModel
                        {
                            VerificationAgencyId = Convert.ToInt32(dr["VerificationAgencyId"]),
                            VerificationAgencyName = Convert.ToString(dr["VerificationAgencyName"]),
                            VerificationAgency_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return verificationagencys;
        }

        public HR_VerificationAgencyViewModel GetVerificationAgencyDetail(int verificationagencyId = 0)
        {
            HR_VerificationAgencyViewModel verificationagency = new HR_VerificationAgencyViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtVerificationAgencys = sqlDbInterface.GetVerificationAgencyDetail(verificationagencyId);
                if (dtVerificationAgencys != null && dtVerificationAgencys.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtVerificationAgencys.Rows)
                    {
                        verificationagency = new HR_VerificationAgencyViewModel
                        {
                            VerificationAgencyId = Convert.ToInt32(dr["VerificationAgencyId"]),
                            VerificationAgencyName = Convert.ToString(dr["VerificationAgencyName"]),
                            VerificationAgency_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return verificationagency;
        }
        public List<HR_VerificationAgencyViewModel> GetVerificationAgencyList()
        {
            List<HR_VerificationAgencyViewModel> agencies = new List<HR_VerificationAgencyViewModel>();
            try
            {
                List<HR_VerificationAgency> agencyList = dbInterface.GetVerificationAgencyList();
                if (agencyList != null && agencyList.Count > 0)
                {
                    foreach (HR_VerificationAgency agency in agencyList)
                    {
                        agencies.Add(new HR_VerificationAgencyViewModel { VerificationAgencyId = agency.VerificationAgencyId, VerificationAgencyName = agency.VerificationAgencyName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return agencies;
        }
    }
}
