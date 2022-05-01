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
    public class SeparationClearListBL
    {
        HRMSDBInterface dbInterface;
        public SeparationClearListBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditSeparationClearList(SeparationClearListViewModel separationclearlistViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_SeparationClearList separationclearlist = new HR_SeparationClearList
                {
                    SeparationClearListId = separationclearlistViewModel.SeparationClearListId,
                    SeparationClearListName = separationclearlistViewModel.SeparationClearListName,
                    SeparationClearListDesc = separationclearlistViewModel.SeparationClearListDesc, 
                    Status = separationclearlistViewModel.SeparationClearList_Status
                };
                responseOut = dbInterface.AddEditSeparationClearList(separationclearlist);
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

        public List<SeparationClearListViewModel> GetSeparationClearList(string separationclearName = "", string Status = "")
        {
            List<SeparationClearListViewModel> separationclearlists = new List<SeparationClearListViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSeparationClearLists = sqlDbInterface.GetSeparationClearList(separationclearName, Status);
                if (dtSeparationClearLists != null && dtSeparationClearLists.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSeparationClearLists.Rows)
                    {
                        separationclearlists.Add(new SeparationClearListViewModel
                        {
                           SeparationClearListId = Convert.ToInt32(dr["SeparationClearListId"]),
                            SeparationClearListName = Convert.ToString(dr["SeparationClearListName"]),
                            SeparationClearListDesc = Convert.ToString(dr["SeparationClearListDesc"]),
                            SeparationClearList_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return separationclearlists;
        }

        public SeparationClearListViewModel GetSeparationClearListDetail(int separationclearlistId = 0)
        {
            SeparationClearListViewModel separationclearlist = new SeparationClearListViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSeparationClearLists = sqlDbInterface.GetSeparationClearListDetail(separationclearlistId);
                if (dtSeparationClearLists != null && dtSeparationClearLists.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSeparationClearLists.Rows)
                    {
                        separationclearlist = new SeparationClearListViewModel
                        {
                            SeparationClearListId = Convert.ToInt32(dr["SeparationClearListId"]),
                            SeparationClearListName = Convert.ToString(dr["SeparationClearListName"]),
                            SeparationClearListDesc  = Convert.ToString(dr["SeparationClearListDesc"]),
                            SeparationClearList_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return separationclearlist;
        }

        public List<SeparationClearListViewModel> GetSeparationClearListForClearanceTemplate()
        {
            List<SeparationClearListViewModel> separationclearlistViewModel = new List<SeparationClearListViewModel>();
            try
            {
                List<HR_SeparationClearList> separationclearlistList = dbInterface.GetSeparationClearListForClearanceTemplate();
                if (separationclearlistList != null && separationclearlistList.Count > 0)
                {
                    foreach (HR_SeparationClearList advance in separationclearlistList)
                    {
                        separationclearlistViewModel.Add(new SeparationClearListViewModel { SeparationClearListId = advance.SeparationClearListId, SeparationClearListName = advance.SeparationClearListName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return separationclearlistViewModel;
        }

    }
}
