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
   public class UserEmailSettingBL
    {
        DBInterface dbInterface;
        public UserEmailSettingBL()
        {
            dbInterface = new DBInterface();
        }

        public ResponseOut AddEditUserEmailSetting(UserEmailSettingViewModel userEmailViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                UserEmailSetting userEmailsetting = new UserEmailSetting
                {
                    SettingId = userEmailViewModel.SettingId,
                    UserId = userEmailViewModel.UserId,
                    SmtpUser= userEmailViewModel.SmtpUser,
                    SmtpPass = userEmailViewModel.SmtpPass,
                    SmtpServer= userEmailViewModel.SmtpServer,
                    EnableSsl = userEmailViewModel.EnableSsl,
                    SmtpPort= userEmailViewModel.SmtpPort,
                    SmtpDisplayName = userEmailViewModel.SmtpDisplayName,
                    Status = userEmailViewModel.UserStatus,
                    CreatedBy = userEmailViewModel.CreatedBy,
                    CreatedDate = DateTime.Now,
                    CompanyBranchId=userEmailViewModel.CompanyBranchId
                };
                responseOut = dbInterface.AddEditUserEmailSetting(userEmailsetting);
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
        public List<UserEmailSettingViewModel> GetUserEmailSettingList(string fullName="",string userName = "",int companyBranchId=0)
        {
            List<UserEmailSettingViewModel> users = new List<UserEmailSettingViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtusers = sqlDbInterface.GetUserEmailSettingList(fullName,userName, companyBranchId);
                if (dtusers != null && dtusers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtusers.Rows)
                    {
                        users.Add(new UserEmailSettingViewModel
                        {
                            SettingId=Convert.ToInt32(dr["SettingId"]),
                            UserId = Convert.ToInt32(dr["UserId"]),
                            SmtpUser = Convert.ToString(dr["SmtpUser"]),
                            FullName = Convert.ToString(dr["FullName"]),
                            SmtpPass=Convert.ToString(dr["SmtpPass"]),
                            SmtpPort=Convert.ToInt32(dr["SmtpPort"]),
                            SmtpServer = Convert.ToString(dr["SmtpServer"]),
                            EnableSsl = Convert.ToBoolean(dr["EnableSsl"]),
                            SmtpDisplayName=Convert.ToString(dr["SmtpDisplayName"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),
                            ModifiedByName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            UserStatus = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchName= Convert.ToString(dr["CompanyBranchName"]),
                        });
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
        public UserEmailSettingViewModel GetUserEmailSettingDetail(int settingId = 0)
        {
            UserEmailSettingViewModel user = new UserEmailSettingViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtUsers = sqlDbInterface.GetUserEmailSettingDetail(settingId);
                if (dtUsers != null && dtUsers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtUsers.Rows)
                    {
                        user = new UserEmailSettingViewModel
                        {
                            SettingId = Convert.ToInt32(dr["SettingId"]),
                            UserId = Convert.ToInt32(dr["UserId"]),
                            FullName = Convert.ToString(dr["FullName"]),
                            SmtpUser = Convert.ToString(dr["UserName"]),
                            EnableSsl=Convert.ToBoolean(dr["EnableSsl"]),
                            SmtpServer= Convert.ToString(dr["SmtpServer"]),
                            SmtpPort=Convert.ToInt32(dr["SmtpPort"]),
                            SmtpDisplayName=Convert.ToString(dr["SmtpDisplayName"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),
                            ModifiedByName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            UserStatus = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchId=Convert.ToInt32(dr["CompanyBranchId"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return user;
        }
        public List<UserViewModel> GetUserEmailAutoCompleteList(string searchTerm)
        {

            List<UserViewModel> users = new List<UserViewModel>();
            try
            {
                List<User> userList = dbInterface.GetUserEmailAutoCompleteList(searchTerm);

                if (userList != null && userList.Count > 0)
                {
                    foreach (User user in userList)
                    {
                        users.Add(new UserViewModel
                        {

                            FullName = user.FullName,
                            UserName = user.UserName,
                            UserId = user.UserId,
                            MobileNo = user.MobileNo,
                            Email=user.Email
                        });
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

        public DataTable GetUserEmailSettingDetailDataTable(int userId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtUserEmailSetting = new DataTable();
            try
            {
                dtUserEmailSetting = sqlDbInterface.GetUserEmailSettingDetailByUserId(userId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtUserEmailSetting;
        }
    }
}
