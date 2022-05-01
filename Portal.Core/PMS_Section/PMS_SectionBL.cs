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
    public class PMS_SectionBL
    {
        HRMSDBInterface dbInterface;
        public PMS_SectionBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditSection(PMS_SectionViewModel pmssectionViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_PMS_Section pmssection = new HR_PMS_Section
                {
                    SectionId = pmssectionViewModel.SectionId,
                    SectionName = pmssectionViewModel.SectionName, 
                    Status = pmssectionViewModel.Section_Status
                };
                responseOut = dbInterface.AddEditSection(pmssection);
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

        public List<PMS_SectionViewModel> GetSectionList(string sectionName = "", string Status = "")
        {
            List<PMS_SectionViewModel> pmssections = new List<PMS_SectionViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPMSSections = sqlDbInterface.GetSectionList(sectionName, Status);
                if (dtPMSSections != null && dtPMSSections.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPMSSections.Rows)
                    {
                        pmssections.Add(new PMS_SectionViewModel
                        {
                            SectionId = Convert.ToInt32(dr["SectionId"]),
                            SectionName = Convert.ToString(dr["SectionName"]), 
                            Section_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pmssections;
        }

        public PMS_SectionViewModel GetSectionDetail(int sectionId = 0)
        {
            PMS_SectionViewModel pmssection = new PMS_SectionViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtIPMSSections = sqlDbInterface.GetSectionDetail(sectionId);
                if (dtIPMSSections != null && dtIPMSSections.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtIPMSSections.Rows)
                    {
                        pmssection = new PMS_SectionViewModel
                        {
                            SectionId = Convert.ToInt32(dr["SectionId"]),
                            SectionName = Convert.ToString(dr["SectionName"]),
                            Section_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pmssection;
        }


        public List<PMS_SectionViewModel> GetPMSSectionList()
        {
            List<PMS_SectionViewModel> pMSSectionViewModel = new List<PMS_SectionViewModel>();
            try
            {
                List<HR_PMS_Section> pMSSectionList = dbInterface.GetPMSSectionList();
                if (pMSSectionList != null && pMSSectionList.Count > 0)
                {
                    foreach (HR_PMS_Section advance in pMSSectionList)
                    {
                        pMSSectionViewModel.Add(new PMS_SectionViewModel { SectionId = advance.SectionId, SectionName = advance.SectionName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pMSSectionViewModel;
        }

    }
}
