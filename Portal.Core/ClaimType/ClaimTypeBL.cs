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
    public class ClaimTypeBL
    {
        HRMSDBInterface dbInterface;
        public ClaimTypeBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditClaimType(HR_ClaimTypeViewModel claimtypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_ClaimType claimtype = new HR_ClaimType
                {
                    ClaimTypeId = claimtypeViewModel.ClaimTypeId,
                    ClaimTypeName = claimtypeViewModel.ClaimTypeName,
                    ClaimNature = claimtypeViewModel.ClaimNature,
                    Status = claimtypeViewModel.ClaimType_Status
                };
                responseOut = dbInterface.AddEditClaimType(claimtype);
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

        public List<HR_ClaimTypeViewModel> GetClaimTypeList(string claimtypeName = "", string claimNature = "", string Status = "")
        {
            List<HR_ClaimTypeViewModel> claimtypes = new List<HR_ClaimTypeViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCLaimTypes = sqlDbInterface.GetClaimTypeList(claimtypeName, claimNature, Status);
                if (dtCLaimTypes != null && dtCLaimTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCLaimTypes.Rows)
                    {
                        claimtypes.Add(new HR_ClaimTypeViewModel
                        {
                            ClaimTypeId = Convert.ToInt32(dr["ClaimTypeId"]),
                            ClaimTypeName = Convert.ToString(dr["ClaimTypeName"]),
                            ClaimNature= Convert.ToString(dr["ClaimNature"]),
                            ClaimType_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return claimtypes;
        }

        public HR_ClaimTypeViewModel GetClaimTypeDetail(int claimtypeId = 0)
        {
            HR_ClaimTypeViewModel claimtype = new HR_ClaimTypeViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtClaimTypes = sqlDbInterface.GetClaimTypeDetail(claimtypeId);
                if (dtClaimTypes != null && dtClaimTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtClaimTypes.Rows)
                    {
                        claimtype = new HR_ClaimTypeViewModel
                        {
                            ClaimTypeId = Convert.ToInt32(dr["ClaimTypeId"]),
                            ClaimTypeName = Convert.ToString(dr["ClaimTypeName"]),
                            ClaimNature = Convert.ToString(dr["ClaimNature"]),
                            ClaimType_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return claimtype;
        }

        public List<HR_ClaimTypeViewModel> GetEmployeeClaimApplicationTypeList()
        {
            List<HR_ClaimTypeViewModel> claimTypeViewModel = new List<HR_ClaimTypeViewModel>();
            try
            {
                List<HR_ClaimType> claimTypeList = dbInterface.GetClaimTypeList();
                if (claimTypeList != null && claimTypeList.Count > 0)
                {
                    foreach (HR_ClaimType advance in claimTypeList)
                    {
                        claimTypeViewModel.Add(new HR_ClaimTypeViewModel { ClaimTypeId = advance.ClaimTypeId, ClaimTypeName = advance.ClaimTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return claimTypeViewModel;
        }

        public List<EmployeeClaimApplicationViewModel> GetEmployeeClaimApplicationDetails(int companyId, int userId, int reportingUserId, int reportingRoleId)
        {

            List<EmployeeClaimApplicationViewModel> claimApplicationList = new List<EmployeeClaimApplicationViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtList = sqlDbInterface.GetEmployeeClaimApplicationDetails(companyId, userId, reportingUserId, reportingRoleId);
                if (dtList != null && dtList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtList.Rows)
                    {
                        claimApplicationList.Add(new EmployeeClaimApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            ClaimTypeName = Convert.ToString(dr["ClaimTypeName"]),
                            ClaimReason = Convert.ToString(dr["ClaimReason"]),
                            ClaimStatus = Convert.ToString(dr["ClaimStatus"]),
                            EmployeeId = Convert.ToInt64(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"])
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return claimApplicationList;
        }





    }
}
