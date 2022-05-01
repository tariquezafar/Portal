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
    public class LeadSourceBL
    {
        DBInterface dbInterface;
        public LeadSourceBL()
        {
            dbInterface = new DBInterface();
        } 

        public List<LeadSourceViewModel> GetLeadSourceList()
        {
            List<LeadSourceViewModel> leadsourceList = new List<LeadSourceViewModel>();
            try
            {
                List<Portal.DAL.LeadSource> leadsources = dbInterface.GetLeadSourceList();
                if (leadsources != null && leadsources.Count > 0)
                {
                    foreach (Portal.DAL.LeadSource leadsource in leadsources)
                    {
                        leadsourceList.Add(new LeadSourceViewModel { LeadSourceId = leadsource.LeadSourceId, LeadSourceName = leadsource.LeadSourceName});
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leadsourceList;
        }

       


        public ResponseOut AddEditLeadSource(LeadSourceViewModel leadsourceViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                LeadSource leadsource = new LeadSource
                {
                    LeadSourceId = leadsourceViewModel.LeadSourceId,
                    LeadSourceName = leadsourceViewModel.LeadSourceName,
                    CompanyId = leadsourceViewModel.CompanyId,
                    Status = leadsourceViewModel.LeadSource_Status,
                    CompanyBranchId= leadsourceViewModel.CompanyBranchId,

                };
                responseOut = dbInterface.AddEditLeadSource(leadsource);
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


        public List<LeadSourceViewModel> GetLeadSourceList(string leadsourceName = "", string Status = "", int companyId = 0, string CompanyBranch = "")
        {
            List<LeadSourceViewModel> leadsources = new List<LeadSourceViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtLeadSources = sqlDbInterface.GetLeadSourceList(leadsourceName, Status, companyId, CompanyBranch);
                if (dtLeadSources != null && dtLeadSources.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtLeadSources.Rows)
                    {
                        leadsources.Add(new LeadSourceViewModel
                        {
                            LeadSourceId = Convert.ToInt32(dr["LeadSourceId"]),
                            LeadSourceName = Convert.ToString(dr["LeadSourceName"]),
                            LeadSource_Status = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchName = Convert.ToString(dr["BranchName"])

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leadsources;
        }

        public LeadSourceViewModel GetLeadSourceDetail(int leadsourceId = 0)
        {
            LeadSourceViewModel leadsource = new LeadSourceViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtLeadSources = sqlDbInterface.GetLeadSourceDetail(leadsourceId);
                if (dtLeadSources != null && dtLeadSources.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtLeadSources.Rows)
                    {
                        leadsource = new LeadSourceViewModel
                        {
                            LeadSourceId = Convert.ToInt32(dr["LeadSourceId"]),
                            LeadSourceName = Convert.ToString(dr["LeadSourceName"]),
                            LeadSource_Status = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchId= Convert.ToInt32(dr["CompanyBranchId"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leadsource;
        }


    }
}








