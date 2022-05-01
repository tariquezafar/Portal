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
    public class RoleBL
    {
        DBInterface dbInterface;
        public RoleBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditRole (RoleViewModel roleViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                Role role = new Role
                {
                    RoleId= roleViewModel.RoleId,
                    RoleName = roleViewModel.RoleName,
                    RoleDesc = roleViewModel.RoleDesc,
                    IsAdmin = roleViewModel.IsAdmin,
                    Status = roleViewModel.Role_Status,
                    CompanyId=roleViewModel.CompanyId,
                    CompanyBranchId=roleViewModel.CompanyBranchId,
                    UserTypeId= roleViewModel.UserTypeId
                };
                responseOut = dbInterface.AddEditRole(role);
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

        public List<RoleViewModel> GetRoleList(int companyId)
        {
            List<RoleViewModel> roles = new List<RoleViewModel>();
            try
            {
                List<Role> roleList = dbInterface.GetRoleList(companyId);
                if (roleList != null && roleList.Count > 0)
                {
                    foreach (Role role in roleList)
                    {
                        roles.Add(new RoleViewModel { RoleId = role.RoleId, RoleName= role.RoleName});
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return roles;
        }
        public List<RoleViewModel> GetRoleList(string roleName = "", string roleDesc = "", string isAdmin = "", string Status="",int companyId=0,int companyBranchId=0)
        {
            List<RoleViewModel> roles = new List<RoleViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtRoles = sqlDbInterface.GetRoleList(roleName, roleDesc, isAdmin, Status, companyId, companyBranchId);
                if (dtRoles != null && dtRoles.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRoles.Rows)
                    {
                        roles.Add(new RoleViewModel
                        {
                            RoleId = Convert.ToInt32(dr["RoleId"]),
                            RoleName = Convert.ToString(dr["RoleName"]),
                            RoleDesc = Convert.ToString(dr["RoleDesc"]), 
                            IsAdmin = Convert.ToBoolean(dr["IsAdmin"]),
                            Role_Status = Convert.ToBoolean(dr["Status"]),
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
            return roles;
        }

        public RoleViewModel GetRoleDetail(int roleId = 0)
        {
            RoleViewModel role = new RoleViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtRoles = sqlDbInterface.GetRoleDetail(roleId);
                if (dtRoles != null && dtRoles.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRoles.Rows)
                    {
                        role = new RoleViewModel
                        {
                            RoleId = Convert.ToInt32(dr["RoleId"]),
                            RoleName = Convert.ToString(dr["RoleName"]),
                            RoleDesc = Convert.ToString(dr["RoleDesc"]),
                            IsAdmin = Convert.ToBoolean(dr["IsAdmin"]),
                            Role_Status = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchId=Convert.ToInt32(dr["CompanyBranchId"])

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return role;
        }

        public List<RoleViewModel> GetDashboardRolesDetails(int CompanyId,int companyBranchId)
        {
            List<RoleViewModel> role = new List<RoleViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtRoles = sqlDbInterface.GetDashboardRolesDetails(CompanyId, companyBranchId);
                if (dtRoles != null && dtRoles.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRoles.Rows)
                    {
                        role.Add(new RoleViewModel
                        {
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
            return role;
        }

        public int GetTotalRolesCount(int CompanyId)
        {
            int totalRolesCount = 0;
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                totalRolesCount = sqldbinterface.GetTotalRolesCount(CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return totalRolesCount;
        }


    }
}
