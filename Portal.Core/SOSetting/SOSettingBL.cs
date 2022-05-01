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
    public class SOSettingBL
    {
        DBInterface dbInterface;
        public SOSettingBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditSOSetting(SOSettingViewModel sosettingViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                SOSetting sosetting = new SOSetting
                {
                   SOSettingId = sosettingViewModel.SOSettingId,
                    NormalApprovalRequired = sosettingViewModel.NormalApprovalRequired,
                    CompanyId = sosettingViewModel.CompanyId,
                    NormalApprovalByUserId = sosettingViewModel.NormalApprovalByUserId,
                    RevisedApprovalRequired = sosettingViewModel.RevisedApprovalRequired,
                    RevisedApprovalByUserId = sosettingViewModel.RevisedApprovalByUserId,
                    CreatedBy = sosettingViewModel.CreatedBy,
                    Status = sosettingViewModel.SOSetting_Status
                };
                responseOut = dbInterface.AddEditSOSetting(sosetting);
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



        public List<SOSettingViewModel> GetSOSettingList(int companyId = 0, string status = "")
        {
            List<SOSettingViewModel> sosettings = new List<SOSettingViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSOSettings = sqlDbInterface.GetSOSettingList(companyId, status);
                if (dtSOSettings != null && dtSOSettings.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSOSettings.Rows)
                    {
                        sosettings.Add(new SOSettingViewModel
                        {

                            SOSettingId = Convert.ToInt32(dr["SOSettingId"]),
                            NormalApprovalByUserId = Convert.ToInt32(dr["NormalApprovalByUserId"]),
                            NormalApprovalByUserName = Convert.ToString(dr["NormalApprovalByUserName"]),
                            NormalApprovalRequired = Convert.ToBoolean(dr["NormalApprovalRequired"]),
                            RevisedApprovalByUserId = Convert.ToInt32(dr["RevisedApprovalByUserId"]),
                            RevisedApprovalByUserName = Convert.ToString(dr["RevisedApprovalByUserName"]),
                            RevisedApprovalRequired = Convert.ToBoolean(dr["RevisedApprovalRequired"]),
                            CompanyId = Convert.ToInt32(dr["CompanyId"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"].ToString() == "" ? "0" : dr["CreatedBy"].ToString()),
                            CreatedName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),
                            ModifiedName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            SOSetting_Status = Convert.ToBoolean(dr["Status"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sosettings;
        }

        public SOSettingViewModel GetSOSettingDetail(int sosettingId = 0)
        {
            SOSettingViewModel sosetting = new SOSettingViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSOSettings = sqlDbInterface.GetSOSettingDetail(sosettingId);
                if (dtSOSettings != null && dtSOSettings.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSOSettings.Rows)
                    {
                        sosetting = new SOSettingViewModel
                        {
                            SOSettingId = Convert.ToInt32(dr["SOSettingId"]),
                            NormalApprovalByUserId = Convert.ToInt32(dr["NormalApprovalByUserId"]),
                            NormalApprovalByUserName = Convert.ToString(dr["NormalApprovalByUserName"]),
                            NormalApprovalRequired = Convert.ToBoolean(dr["NormalApprovalRequired"]),
                            RevisedApprovalByUserId = Convert.ToInt32(dr["RevisedApprovalByUserId"]),
                            RevisedApprovalByUserName = Convert.ToString(dr["RevisedApprovalByUserName"]),
                            RevisedApprovalRequired = Convert.ToBoolean(dr["RevisedApprovalRequired"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"].ToString() == "" ? "0" : dr["CreatedBy"].ToString()),
                            CreatedName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),
                            ModifiedName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            SOSetting_Status = Convert.ToBoolean(dr["Status"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sosetting;
        }


        public List<UserViewModel> GetUserAutoCompleteList(string searchTerm)
        {
            List<UserViewModel> users = new List<UserViewModel>();
            try
            {
                List<User> userList = dbInterface.GetUserAutoCompleteList(searchTerm);

                if (userList != null && userList.Count > 0)
                {
                    foreach (User user in userList)
                    {
                        users.Add(new UserViewModel { UserId = user.UserId, UserName = user.UserName, FullName = user.FullName, MobileNo = user.MobileNo });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return users;
        }


    }
}








