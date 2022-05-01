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
    public class QuotationSettingBL
    {
        DBInterface dbInterface;
        public QuotationSettingBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditQuotationSetting(QuotationSettingViewModel quotationsettingViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                QuotationSetting quotationsetting = new QuotationSetting
                {
                    QuotationSettingId = quotationsettingViewModel.QuotationSettingId,
                    NormalApprovalRequired = quotationsettingViewModel.NormalApprovalRequired,
                    CompanyId = quotationsettingViewModel.CompanyId,
                    NormalApprovalByUserId = quotationsettingViewModel.NormalApprovalByUserId,
                    RevisedApprovalRequired = quotationsettingViewModel.RevisedApprovalRequired,
                    RevisedApprovalByUserId = quotationsettingViewModel.RevisedApprovalByUserId, 
                    CreatedBy = quotationsettingViewModel.CreatedBy,
                    Status = quotationsettingViewModel.QuotationSetting_Status
                };
                responseOut = dbInterface.AddEditQuotationSetting(quotationsetting);
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



        public List<QuotationSettingViewModel> GetQuotationSettingList( int companyId = 0, string status = "")
        {
            List<QuotationSettingViewModel> quotationsettings = new List<QuotationSettingViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotationSettings = sqlDbInterface.GetQuotationSettingList(companyId, status);
                if (dtQuotationSettings != null && dtQuotationSettings.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotationSettings.Rows)
                    {
                        quotationsettings.Add(new QuotationSettingViewModel
                        {

                            QuotationSettingId = Convert.ToInt32(dr["QuotationSettingId"]),
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
                            QuotationSetting_Status = Convert.ToBoolean(dr["Status"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return quotationsettings;
        }

        public QuotationSettingViewModel GetQuotationSettingDetail(int quotationsettingId = 0)
        {
            QuotationSettingViewModel quotationsetting = new QuotationSettingViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotationSettings = sqlDbInterface.GetQuotationSettingDetail(quotationsettingId);
                if (dtQuotationSettings != null && dtQuotationSettings.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotationSettings.Rows)
                    {
                        quotationsetting = new QuotationSettingViewModel
                        {
                            QuotationSettingId = Convert.ToInt32(dr["QuotationSettingId"]),
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
                            QuotationSetting_Status = Convert.ToBoolean(dr["Status"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return quotationsetting;
        }


        public List<UserViewModel> GetUserAutoCompleteList(string searchTerm)
        {
            List<UserViewModel> users = new List<UserViewModel>();
            try
            {
                List<User>   userList = dbInterface.GetUserAutoCompleteList(searchTerm);

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








