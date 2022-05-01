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
    public class SeparationCategoryBL
    {
        HRMSDBInterface dbInterface;
        public SeparationCategoryBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditSeparationCategory(SeparationCategoryViewModel separationcategoryViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_SeparationCategory separationclearlist = new HR_SeparationCategory
                {
                    SeparationCategoryId = separationcategoryViewModel.SeparationCategoryId,
                    SeparationCategoryName = separationcategoryViewModel.SeparationCategoryName, 
                    status = separationcategoryViewModel.SeparationCategory_Status
                };
                responseOut = dbInterface.AddEditSeparationCategory(separationclearlist);
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

        public List<SeparationCategoryViewModel> GetSeparationCategory(string separationCategoryName = "", string Status = "")
        {
            List<SeparationCategoryViewModel> separationcategorylists = new List<SeparationCategoryViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSeparationCategorys = sqlDbInterface.GetSeparationCategory(separationCategoryName, Status);
                if (dtSeparationCategorys != null && dtSeparationCategorys.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSeparationCategorys.Rows)
                    {
                        separationcategorylists.Add(new SeparationCategoryViewModel
                        {
                           SeparationCategoryId = Convert.ToInt32(dr["SeparationCategoryId"]),
                            SeparationCategoryName = Convert.ToString(dr["SeparationCategoryName"]), 
                            SeparationCategory_Status= Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return separationcategorylists;
        }

        public SeparationCategoryViewModel GetSeparationCategoryDetail(int separationcategoryId = 0)
        {
            SeparationCategoryViewModel separationcategory = new SeparationCategoryViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSeparationCategorys = sqlDbInterface.GetSeparationCategoryDetail(separationcategoryId);
                if (dtSeparationCategorys != null && dtSeparationCategorys.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSeparationCategorys.Rows)
                    {
                        separationcategory = new SeparationCategoryViewModel
                        {
                            SeparationCategoryId = Convert.ToInt32(dr["SeparationCategoryId"]),
                            SeparationCategoryName = Convert.ToString(dr["SeparationCategoryName"]), 
                            SeparationCategory_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return separationcategory;
        }


        public List<SeparationCategoryViewModel> GetSeparationCategoryForSeparationApplicationList()
        {
            List<SeparationCategoryViewModel> separationcategorys = new List<SeparationCategoryViewModel>();
            try
            {
                List<HR_SeparationCategory> separationCategoryList = dbInterface.GetSeparationCategoryForSeparationApplicationList();
                if (separationCategoryList != null && separationCategoryList.Count > 0)
                {
                    foreach (HR_SeparationCategory advance in separationCategoryList)
                    {
                        separationcategorys.Add(new SeparationCategoryViewModel { SeparationCategoryId = advance.SeparationCategoryId, SeparationCategoryName = advance.SeparationCategoryName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return separationcategorys;
        }



    }
}
