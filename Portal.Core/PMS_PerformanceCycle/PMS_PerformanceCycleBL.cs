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
    public class PMS_PerformanceCycleBL
    {
        HRMSDBInterface dbInterface;
        public PMS_PerformanceCycleBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditPerformanceCycle(PMS_PerformanceCycleViewModel pmssectionViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_PMS_PerformanceCycle pmsperformancecycle = new HR_PMS_PerformanceCycle
                {
                    PerformanceCycleId = pmssectionViewModel.PerformanceCycleId,
                    PerformanceCycleName = pmssectionViewModel.PerformanceCycleName, 
                    Status = pmssectionViewModel.PerformanceCycle_Status
                };
                responseOut = dbInterface.AddEditPerformanceCycle(pmsperformancecycle);
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

        public List<PMS_PerformanceCycleViewModel> GetPerformanceCycleList(string performancecycleName = "", string Status = "")
        {
            List<PMS_PerformanceCycleViewModel> pmsperformancecycles = new List<PMS_PerformanceCycleViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPMSPerformanceCycles = sqlDbInterface.GetPerformanceCycleList(performancecycleName, Status);
                if (dtPMSPerformanceCycles != null && dtPMSPerformanceCycles.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPMSPerformanceCycles.Rows)
                    {
                        pmsperformancecycles.Add(new PMS_PerformanceCycleViewModel
                        {
                            PerformanceCycleId = Convert.ToInt32(dr["PerformanceCycleId"]),
                            PerformanceCycleName = Convert.ToString(dr["PerformanceCycleName"]),
                            PerformanceCycle_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pmsperformancecycles;
        }

        public PMS_PerformanceCycleViewModel GetPerformanceCycleDetail(int performancecycleId = 0)
        {
            PMS_PerformanceCycleViewModel pmsperformancecycle = new PMS_PerformanceCycleViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtIPMSSections = sqlDbInterface.GetPerformanceCycleDetail(performancecycleId);
                if (dtIPMSSections != null && dtIPMSSections.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtIPMSSections.Rows)
                    {
                        pmsperformancecycle = new PMS_PerformanceCycleViewModel
                        {
                            PerformanceCycleId = Convert.ToInt32(dr["PerformanceCycleId"]),
                            PerformanceCycleName = Convert.ToString(dr["PerformanceCycleName"]),
                            PerformanceCycle_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pmsperformancecycle;
        }


        public List<PMS_PerformanceCycleViewModel> GetPMSPerformanceCycleList()
        {
            List<PMS_PerformanceCycleViewModel> pMSPerformanceCycleViewModel = new List<PMS_PerformanceCycleViewModel>();
            try
            {
                List<HR_PMS_PerformanceCycle> pMSPerformanceCycleList = dbInterface.GetPMSPerformanceCycleList();
                if (pMSPerformanceCycleList != null && pMSPerformanceCycleList.Count > 0)
                {
                    foreach (HR_PMS_PerformanceCycle advance in pMSPerformanceCycleList)
                    {
                        pMSPerformanceCycleViewModel.Add(new PMS_PerformanceCycleViewModel { PerformanceCycleId = advance.PerformanceCycleId, PerformanceCycleName = advance.PerformanceCycleName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pMSPerformanceCycleViewModel;
        }
    }
}
