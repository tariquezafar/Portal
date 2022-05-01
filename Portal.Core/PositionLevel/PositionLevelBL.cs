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
    public class PositionLevelBL
    {
        HRMSDBInterface dbInterface;
        public PositionLevelBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditPositionLevel(PositionLevelViewModel positionlevelViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_PositionLevel positionlevel = new HR_PositionLevel
                {
                    PositionLevelId = positionlevelViewModel.PositionLevelId,
                    PositionLevelName = positionlevelViewModel.PositionLevelName,
                    PositionLevelCode = positionlevelViewModel.PositionLevelCode,
                    Status = positionlevelViewModel.PositionLevel_Status
                };
                responseOut = dbInterface.AddEditPositionLevel(positionlevel);
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

        public List<PositionLevelViewModel> GetPositionLevelList(string positionlevelName = "", string positionlevelCode = "", string Status = "")
        {
            List<PositionLevelViewModel> positionlevels = new List<PositionLevelViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPositionlevels = sqlDbInterface.GetPositionLevelList(positionlevelName, positionlevelCode, Status);
                if (dtPositionlevels != null && dtPositionlevels.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPositionlevels.Rows)
                    {
                        positionlevels.Add(new PositionLevelViewModel
                        {
                            PositionLevelId = Convert.ToInt32(dr["PositionLevelId"]),
                            PositionLevelName = Convert.ToString(dr["PositionLevelName"]),
                            PositionLevelCode = Convert.ToString(dr["PositionLevelCode"]),
                            PositionLevel_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return positionlevels;
        }

        public PositionLevelViewModel GetPositionLevelDetail(int positionlevelId = 0)
        {
            PositionLevelViewModel positionlevel = new PositionLevelViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPositionLevels = sqlDbInterface.GetPositionLevelDetail(positionlevelId);
                if (dtPositionLevels != null && dtPositionLevels.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPositionLevels.Rows)
                    {
                        positionlevel = new PositionLevelViewModel
                        {
                            PositionLevelId = Convert.ToInt32(dr["PositionLevelId"]),
                            PositionLevelName = Convert.ToString(dr["PositionLevelName"]),
                            PositionLevelCode = Convert.ToString(dr["PositionLevelCode"]),
                            PositionLevel_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return positionlevel;
        }

      


    }
}
