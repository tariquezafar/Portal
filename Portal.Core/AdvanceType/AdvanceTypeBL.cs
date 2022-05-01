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
    public class AdvanceTypeBL
    {
        HRMSDBInterface dbInterface;
        public AdvanceTypeBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditAdvanceType(HR_AdvanceTypeViewModel advancetypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_AdvanceType advancetype = new HR_AdvanceType
                {
                    AdvanceTypeId = advancetypeViewModel.AdvanceTypeId,
                    AdvanceTypeName = advancetypeViewModel.AdvanceTypeName, 
                    Status = advancetypeViewModel.AdvanceType_Status
                };
                responseOut = dbInterface.AddEditAdvanceType(advancetype);
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

        public List<HR_AdvanceTypeViewModel> GetAdvanceTypeList(string advancetypeName = "", string Status = "")
        {
            List<HR_AdvanceTypeViewModel> advancetypes = new List<HR_AdvanceTypeViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtAdvanceTypes = sqlDbInterface.GetAdvanceTypeList(advancetypeName, Status);
                if (dtAdvanceTypes != null && dtAdvanceTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAdvanceTypes.Rows)
                    {
                        advancetypes.Add(new HR_AdvanceTypeViewModel
                        {
                           AdvanceTypeId = Convert.ToInt32(dr["AdvanceTypeId"]),
                            AdvanceTypeName = Convert.ToString(dr["AdvanceTypeName"]), 
                            AdvanceType_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return advancetypes;
        }

        public HR_AdvanceTypeViewModel GetAdvanceTypeDetail(int advancetypeId = 0)
        {
            HR_AdvanceTypeViewModel advancetype = new HR_AdvanceTypeViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtAdvanceTypes = sqlDbInterface.GetAdvanceTypeDetail(advancetypeId);
                if (dtAdvanceTypes != null && dtAdvanceTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAdvanceTypes.Rows)
                    {
                        advancetype = new HR_AdvanceTypeViewModel
                        {
                            AdvanceTypeId = Convert.ToInt32(dr["AdvanceTypeId"]),
                            AdvanceTypeName = Convert.ToString(dr["AdvanceTypeName"]),
                            AdvanceType_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return advancetype;
        }

        public List<HR_AdvanceTypeViewModel> GetAdvanceTypeForEmpolyeeAdvanceAppList()
        {
            List<HR_AdvanceTypeViewModel> advanceapplications = new List<HR_AdvanceTypeViewModel>();
            try
            {
                List<HR_AdvanceType> advanceList = dbInterface.GetAdvanceTypeForEmpolyeeAdvanceAppList();
                if (advanceList != null && advanceList.Count > 0)
                {
                    foreach (HR_AdvanceType advance in advanceList)
                    {
                        advanceapplications.Add(new HR_AdvanceTypeViewModel { AdvanceTypeId = advance.AdvanceTypeId, AdvanceTypeName = advance.AdvanceTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return advanceapplications;
        }

        public List<EmployeeAdvanceApplicationViewModel> GetEmployeeAdvanceApplicationDetails(int companyId, int userId, int reportingUserId, int reportingRoleId)
        {

            List<EmployeeAdvanceApplicationViewModel> advanceApplicationList = new List<EmployeeAdvanceApplicationViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtList = sqlDbInterface.GetEmployeeAdvanceApplicationDetails(companyId, userId, reportingUserId, reportingRoleId);
                if (dtList != null && dtList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtList.Rows)
                    {
                        advanceApplicationList.Add(new EmployeeAdvanceApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt64(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            AdvanceAmount = Convert.ToDecimal(dr["AdvanceAmount"]),
                            AdvanceStatus = Convert.ToString(dr["AdvanceStatus"]),
                            AdvanceTypeName = Convert.ToString(dr["AdvanceTypeName"]),
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
            return advanceApplicationList;
        }


    }
}
