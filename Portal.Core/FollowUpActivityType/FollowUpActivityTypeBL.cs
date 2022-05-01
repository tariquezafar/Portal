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
    public class FollowUpActivityTypeBL
    {
        DBInterface dbInterface;
        public FollowUpActivityTypeBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditFollowUpActivityType(FollowUpActivityTypeViewModel followUpActivityTypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                FollowUpActivityType followUpActivityTypeId = new FollowUpActivityType
                {
                    FollowUpActivityTypeId = followUpActivityTypeViewModel.FollowUpActivityTypeId,
                    FollowUpActivityTypeName = followUpActivityTypeViewModel.FollowUpActivityTypeName,
                    Status = followUpActivityTypeViewModel.FollowUpActivityType_Status,
                };
                responseOut = dbInterface.AddEditFollowUpActivityType(followUpActivityTypeId);
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

        public List<FollowUpActivityTypeViewModel> GetFollowUpActivityTypeList(string followUpActivityTypeName = "", string Status = "")
        {
            List<FollowUpActivityTypeViewModel> followUpActivityTypeList = new List<FollowUpActivityTypeViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtfollowUpActivityTypes = sqlDbInterface.GetFollowUpActivityTypeList(followUpActivityTypeName, Status);
                if (dtfollowUpActivityTypes != null && dtfollowUpActivityTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtfollowUpActivityTypes.Rows)
                    {
                        followUpActivityTypeList.Add(new FollowUpActivityTypeViewModel
                        {
                            FollowUpActivityTypeId = Convert.ToInt32(dr["FollowUpActivityTypeId"]),
                            FollowUpActivityTypeName = Convert.ToString(dr["FollowUpActivityTypeName"]),
                            FollowUpActivityType_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return followUpActivityTypeList;
        }

        public FollowUpActivityTypeViewModel GetFollowUpActivityTypeDetail(int followUpActivityTypeId = 0)
        {
            FollowUpActivityTypeViewModel followUpActivityType = new FollowUpActivityTypeViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtfollowUpActivityTypes = sqlDbInterface.GetFollowUpActivityTypeDetail(followUpActivityTypeId);
                if (dtfollowUpActivityTypes != null && dtfollowUpActivityTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtfollowUpActivityTypes.Rows)
                    {
                        followUpActivityType = new FollowUpActivityTypeViewModel
                        {
                            FollowUpActivityTypeId = Convert.ToInt32(dr["FollowUpActivityTypeId"]),
                            FollowUpActivityTypeName = Convert.ToString(dr["FollowUpActivityTypeName"]),
                            FollowUpActivityType_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return followUpActivityType;
        }

    }
}
