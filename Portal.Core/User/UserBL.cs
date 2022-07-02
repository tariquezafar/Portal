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
    public class UserBL
    {
        DBInterface dbInterface;
        public UserBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditUser(UserViewModel userViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                User user = new User
                {
                    UserId = userViewModel.UserId,
                    UserName = userViewModel.UserName,
                    FullName = userViewModel.FullName,
                    MobileNo = userViewModel.MobileNo,
                    Email = userViewModel.Email,
                    Password = EnDecrypt.Encrypt(userViewModel.Password), //Matching Encrypted password
                    RoleId = userViewModel.RoleId,
                    CompanyId = userViewModel.CompanyId,
                    CreatedBy = userViewModel.CreatedBy,
                    CreatedDate = DateTime.Now,
                    ExpiryDate = Convert.ToDateTime(userViewModel.ExpiryDate),
                    Status = userViewModel.UserStatus,
                    CompanyBranchId= userViewModel.CompanyBranchId, //For CompanyBranch Id
                    FK_CustomerId=userViewModel.FK_CustomerId
                };
                responseOut = dbInterface.AddEditUser(user);
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
        public ResponseOut UpdateUserPic(UserViewModel userViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                User user = new User
                {
                    UserId = userViewModel.UserId,
                    UserPicName  = userViewModel.UserPicName,
                    UserPicPath = userViewModel.UserPicPath
                };
                responseOut = dbInterface.UpdateUserPic(user);
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
            public UserViewModel AuthenticateUser(string userName, string password)
            {
                UserViewModel userViewModel = new UserViewModel();
                try
                {
                    User user = dbInterface.AuthenticateUser(userName, EnDecrypt.Encrypt(password));                                
                    userViewModel = new UserViewModel
                    {
                        UserId = user.UserId,
                        UserName = user.UserName,
                        FullName = user.FullName,
                        MobileNo = user.MobileNo,
                        Email = user.Email,
                        Password = user.Password,
                        RoleId = user.RoleId,
                        CompanyId = user.CompanyId,
                        ExpiryDate = user.ExpiryDate == null ? "" : Convert.ToDateTime(user.ExpiryDate).ToString("dd-MMM-yyyy"),
                        UserStatus = user.Status,
                        UserPicName=user.UserPicName,
                        CompanyBranchId=Convert.ToInt32(user.CompanyBranchId),
                        FK_CustomerId=Convert.ToInt32(user.FK_CustomerId)
                    };

                    if (userViewModel.UserId != 0)
                    {
                        if (userViewModel.RoleId == (int)Roles.SuperAdmin)
                        {
                            userViewModel.status = ActionStatus.Success;
                            userViewModel.message = "";
                        }
                        else
                        {
                            if (!userViewModel.UserStatus)
                            {
                                userViewModel.status = ActionStatus.Fail;
                                userViewModel.message = ActionMessage.UserNotActive;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(userViewModel.ExpiryDate) && Convert.ToDateTime(userViewModel.ExpiryDate).Date < DateTime.Now.Date)
                                {
                                    userViewModel.status = ActionStatus.Fail;
                                    userViewModel.message = ActionMessage.UserLoginExpired;
                                }
                                else
                                {
                                    userViewModel.status = ActionStatus.Success;
                                    userViewModel.message = "";
                                }
                            }
                        }

                    }
                    else
                    {
                        userViewModel.status = ActionStatus.Fail;
                        userViewModel.message = ActionMessage.InvalidCredential;
                    }
                
                    if (userViewModel.status == ActionStatus.Success)
                    {
                        SessionWrapper session = new SessionWrapper();
                        FinYearBL finYearBL = new FinYearBL();
                        FinYearViewModel currentFinYear = finYearBL.GetCurrentFinancialYear(0);
                        Employee employee = dbInterface.GetEmployee(userViewModel.Email);
                        EmployeeViewModel employeeViewModel = new EmployeeViewModel {
                            EmployeeId=employee.EmployeeId,
                            EmployeeCode=employee.EmployeeCode,
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Email=employee.Email,
                        };
                        session.SetInSession(SessionKey.IsAdmin, dbInterface.GetIsAdminByRoleId(userViewModel.RoleId));
                        session.SetInSession(SessionKey.CurrentUser, userViewModel);
                        session.SetInSession(SessionKey.IsAuthenticated, true);
                        session.SetInSession(SessionKey.UserFullName, userViewModel.FullName);
                        session.SetInSession(SessionKey.UserPicName, userViewModel.UserPicName);
                        session.SetInSession(SessionKey.CurrentFinYear, currentFinYear);
                        session.SetInSession(SessionKey.UserEmail, userViewModel.Email);
                        session.SetInSession(SessionKey.CurrentEmployee, employeeViewModel);
                        session.SetInSession(SessionKey.EmployeeId, employeeViewModel.EmployeeId);
                        session.SetInSession(SessionKey.EmployeeFullName, employeeViewModel.FirstName+" "+employeeViewModel.LastName);
                        session.SetInSession(SessionKey.CompanyBranchId, userViewModel);
                        session.SetInSession(SessionKey.UserId, userViewModel);
                        session.SetInSession(SessionKey.RoleId, userViewModel);
                    session.SetInSession(SessionKey.CustomerId, userViewModel.FK_CustomerId);

                        session.SetInSession(SessionKey.CompanyBranchName, dbInterface.GetCompanyBranchName(userViewModel.CompanyBranchId));

                        List<ParentMenu> parentMenuList = new List<ParentMenu>();
                        List<proc_GetRoleWiseParentUI_Result> parentUIList = dbInterface.GetRoleWiseParentUI(userViewModel.RoleId);

                        ParentMenu parentmenu;
                        List<ChildMenu> childMenuList;
                        foreach (proc_GetRoleWiseParentUI_Result UIItem in parentUIList)
                        {
                            parentmenu = new ParentMenu();
                            parentmenu.InterfaceId = Convert.ToInt32(UIItem.InterfaceId);
                            parentmenu.InterfaceName = UIItem.InterfaceName;
                            parentmenu.InterfaceURL = UIItem.InterfaceURL;
                            parentmenu.InterfaceType = UIItem.InterfaceType;
                            parentmenu.InterfaceSubType = UIItem.InterfaceSubType;
                            if (string.IsNullOrEmpty(UIItem.InterfaceURL))
                            {
                                childMenuList = new List<ChildMenu>();
                                List<proc_GetRoleWiseChildUI_Result> childUIList = dbInterface.GetRoleWiseChildUI(userViewModel.RoleId, Convert.ToInt32(UIItem.InterfaceId));
                                foreach (proc_GetRoleWiseChildUI_Result childUIItem in childUIList)
                                {
                                    childMenuList.Add(new ChildMenu { InterfaceId = Convert.ToInt32(childUIItem.InterfaceId), InterfaceName = childUIItem.InterfaceName, InterfaceURL = childUIItem.InterfaceURL, InterfaceType = childUIItem.InterfaceType, InterfaceSubType = childUIItem.InterfaceSubType });
                                }

                            }
                            else
                            {
                                childMenuList = new List<ChildMenu>();
                            }
                            parentmenu.childMenuList = childMenuList;
                            parentMenuList.Add(parentmenu);
                        }
                        session.SetInSession(SessionKey.RoleWiseUIMatrix, parentMenuList);
                        List<CompanyBranchViewModel> companyBranches = new List<CompanyBranchViewModel>();
                        List<ComapnyBranch> companyBranchList = dbInterface.GetCompanyBranchList(userViewModel.CompanyId);
                        if (companyBranchList != null && companyBranchList.Count > 0)
                        {
                            foreach (ComapnyBranch companyBranch in companyBranchList)
                            {
                                companyBranches.Add(new CompanyBranchViewModel { CompanyBranchId = companyBranch.CompanyBranchId, BranchName = companyBranch.BranchName });
                            }
                        }
                        session.SetInSession(SessionKey.CompanyBranchList, companyBranches);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                    throw ex;
                }
                return userViewModel;
            }

        public bool GetIsAdminByRoleId(int RoleId)
        {
            bool IsAdmin;
            try
            {
                IsAdmin = dbInterface.GetIsAdminByRoleId(RoleId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return IsAdmin;
        }

        public UserViewModel GetDetails(int userId)
        {
            UserViewModel usermodel = new UserViewModel();
            User user = dbInterface.GetDetails(userId);
            usermodel.UserId = user.UserId;
            usermodel.UserName = user.UserName;
            return usermodel;
        }
            public List<UserViewModel> GetUserList(string userName = "", int companyId = 0, int roleId = 0, string fullName = "", string phoneNo = "", string email = "", int userRole = 0,int companyBranchId=0)
        {
            List<UserViewModel> users = new List<UserViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtusers = sqlDbInterface.GetUserList(userName, companyId, roleId, fullName, phoneNo, email, userRole, companyBranchId);
                if (dtusers != null && dtusers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtusers.Rows)
                    {
                        users.Add(new UserViewModel
                        {
                            UserId = Convert.ToInt32(dr["UserId"]),
                            UserName = Convert.ToString(dr["UserName"]),
                            FullName = Convert.ToString(dr["FullName"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            Email = Convert.ToString(dr["Email"]),
                            RoleId = Convert.ToInt32(dr["RoleId"]),
                            RoleName = Convert.ToString(dr["RoleName"]),
                            CompanyId = Convert.ToInt32(dr["CompanyId"]),
                            CompanyName = Convert.ToString(dr["CompanyName"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),
                            ModifiedByName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            ExpiryDate = Convert.ToString(dr["ExpiryDate"]),
                            UserStatus = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchName=Convert.ToString(dr["CompanyBranchName"])
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
        public UserViewModel GetUserDetail(int userId = 0)
        {
            UserViewModel user = new UserViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtUsers = sqlDbInterface.GetUserDetail(userId);
                if (dtUsers != null && dtUsers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtUsers.Rows)
                    {
                        user = new UserViewModel
                        {
                            UserId = Convert.ToInt32(dr["UserId"]),
                            UserName = Convert.ToString(dr["UserName"]),
                            FullName = Convert.ToString(dr["FullName"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            Email = Convert.ToString(dr["Email"]),
                            RoleId = Convert.ToInt32(dr["RoleId"]),
                            RoleName = Convert.ToString(dr["RoleName"]),
                            CompanyId = Convert.ToInt32(dr["CompanyId"]),
                            CompanyName = Convert.ToString(dr["CompanyName"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),
                            ModifiedByName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            ExpiryDate = Convert.ToString(dr["ExpiryDate"]),
                            UserStatus = Convert.ToBoolean(dr["Status"]),
                            UserPicPath = Convert.ToString(dr["UserPicPath"]),
                            UserPicName = Convert.ToString(dr["UserPicName"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
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

        public UserViewModel GetUserFromEmail(string userEmail)
        {
            UserViewModel userViewModel = new UserViewModel();
            User user = new User();
            try
            {
                user = dbInterface.GetUserFromEmail(userEmail);
                userViewModel.FullName = user.FullName;
                userViewModel.Password = user.Password;
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return userViewModel;
        }

        public ResponseOut ChangePassword(UserViewModel userViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                responseOut = dbInterface.VerifyOldPassword(userViewModel.UserId, EnDecrypt.Encrypt(userViewModel.OldPassword));
                if (responseOut.status == ActionStatus.Success)
                {
                    User user = new User
                    {
                        UserId = userViewModel.UserId,
                        Password = EnDecrypt.Encrypt(userViewModel.Password),
                        CreatedBy = userViewModel.CreatedBy,
                        CreatedDate = DateTime.Now,
                    };
                    responseOut = dbInterface.ChangePassword(user);
                }
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

        public List<UserViewModel> GetDashboardUser()
        {
            List<UserViewModel> users = new List<UserViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtusers = sqlDbInterface.GetDashboardUser();
                if (dtusers != null && dtusers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtusers.Rows)
                    {
                        users.Add(new UserViewModel
                        {
                            UserId = Convert.ToInt32(dr["UserId"]),
                            UserName = Convert.ToString(dr["UserName"]),
                            FullName = Convert.ToString(dr["FullName"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            Email = Convert.ToString(dr["Email"]),
                            RoleId = Convert.ToInt32(dr["RoleId"]),
                            RoleName = Convert.ToString(dr["RoleName"]),
                            CompanyId = Convert.ToInt32(dr["CompanyId"]),
                            CompanyName = Convert.ToString(dr["CompanyName"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),
                            ModifiedByName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            ExpiryDate = Convert.ToString(dr["ExpiryDate"]),
                            UserStatus = Convert.ToBoolean(dr["Status"])
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

        public List<UserViewModel> GetDashboardUsersDetails(int CompanyId,int companyBranchId)
        {
            List<UserViewModel> users = new List<UserViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtusers = sqlDbInterface.GetDashboardUsersDetails(CompanyId, companyBranchId);
                if (dtusers != null && dtusers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtusers.Rows)
                    {
                        users.Add(new UserViewModel
                        {
                            FullName = Convert.ToString(dr["FullName"]),
                            Email = Convert.ToString(dr["Email"]),
                            RoleName = Convert.ToString(dr["RoleName"])
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

        public int GetTodayUsersCount(int CompanyId)
        {
            int todayUsersCount = 0;
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                todayUsersCount = sqldbinterface.GetTodayUsersCount(CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return todayUsersCount;
        }

        public int GetTotalUsersCount(int CompanyId)
        {
            int totalUsersCount = 0;
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                totalUsersCount = sqldbinterface.GetTotalUsersCount(CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return totalUsersCount;
        }

        public List<UserViewModel> GetUserAutoCompleteList(string searchTerm, int companyId)
        {
            List<UserViewModel> userViewModel = new List<UserViewModel>();
            try
            {
                List<User> userList = dbInterface.GetUserAutoCompleteList(searchTerm, companyId);
                if (userList != null && userList.Count > 0)
                {
                    foreach (User user in userList)
                    {
                        userViewModel.Add(new UserViewModel { UserId = user.UserId, FullName = user.FullName});
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return userViewModel;
        }



        public ResponseOut RemoveImage(long userId)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                responseOut = dbInterface.RemoveImageUser(userId);
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

        public List<CompanyBranchViewModel> GetCompanyBranchList(int companyId)
        {
            List<CompanyBranchViewModel> companyBranches = new List<CompanyBranchViewModel>();
            try
            {
                List<ComapnyBranch> companyBranchList = dbInterface.GetCompanyBranchList(companyId);
                if (companyBranchList != null && companyBranchList.Count > 0)
                {
                    foreach (ComapnyBranch companyBranch in companyBranchList)
                    {
                        companyBranches.Add(new CompanyBranchViewModel { CompanyBranchId = companyBranch.CompanyBranchId, BranchName = companyBranch.BranchName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return companyBranches;
        }
    }
}
