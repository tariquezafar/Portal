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

namespace Portal.Core
{
    public class LeadStatusBL
    {
        DBInterface dbInterface;
        public LeadStatusBL()
        {
            dbInterface = new DBInterface();
        }


        public List<LeadStatusViewModel> GetLeadStatusList()
        {
            List<LeadStatusViewModel> leadstatusList = new List<LeadStatusViewModel>();
            try
            {
                List<Portal.DAL.LeadStatu> leadstatuses = dbInterface.GetLeadStatusList();
                if (leadstatuses != null && leadstatuses.Count > 0)
                {
                    foreach (Portal.DAL.LeadStatu leadstatus in leadstatuses)
                    {
                        leadstatusList.Add(new LeadStatusViewModel { LeadStatusId = leadstatus.LeadStatusId, LeadStatusName = leadstatus.LeadStatusName });
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leadstatusList;
        }




        public ResponseOut AddEditLeadStatus(LeadStatusViewModel leadstatusViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                LeadStatu leadstatus = new LeadStatu
                {
                    LeadStatusId = leadstatusViewModel.LeadStatusId,
                    LeadStatusName = leadstatusViewModel.LeadStatusName,
                    CompanyId = leadstatusViewModel.CompanyId,
                    Status = leadstatusViewModel.Lead_Status,

                };
                responseOut = dbInterface.AddEditLeadStatus(leadstatus);
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


        public List<LeadStatusViewModel> GetLeadStatusList(string leadstatusName = "", string Status = "", int companyId = 0)
        {
            List<LeadStatusViewModel> leadstatuses = new List<LeadStatusViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtLeadStatuses = sqlDbInterface.GetLeadStatusList(leadstatusName, Status, companyId);
                if (dtLeadStatuses != null && dtLeadStatuses.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtLeadStatuses.Rows)
                    {
                        leadstatuses.Add(new LeadStatusViewModel
                        {
                            LeadStatusId = Convert.ToInt32(dr["LeadStatusId"]),
                            LeadStatusName = Convert.ToString(dr["LeadStatusName"]),
                            Lead_Status = Convert.ToBoolean(dr["Status"]),

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leadstatuses;
        }

        public LeadStatusViewModel GetLeadStatusDetail(int leadstatusId = 0)
        {
            LeadStatusViewModel leadstatus = new LeadStatusViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtLeadStatuses = sqlDbInterface.GetLeadStatusDetail(leadstatusId);
                if (dtLeadStatuses != null && dtLeadStatuses.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtLeadStatuses.Rows)
                    {
                        leadstatus = new LeadStatusViewModel
                        {
                            LeadStatusId = Convert.ToInt32(dr["LeadStatusId"]),
                            LeadStatusName = Convert.ToString(dr["LeadStatusName"]),
                            Lead_Status = Convert.ToBoolean(dr["Status"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leadstatus;
        }


    }
}








