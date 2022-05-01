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
    public class AssetTypeBL
    {
        HRMSDBInterface dbInterface;
        public AssetTypeBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditAssetType(HR_AssetTypeViewModel assettypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_AssetType assettype = new HR_AssetType
                {
                    AssetTypeId = assettypeViewModel.AssetTypeId,
                    AssetTypeName = assettypeViewModel.AssetTypeName, 
                    Status = assettypeViewModel.AssetType_Status
                };
                responseOut = dbInterface.AddEditAssetType(assettype);
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

        public List<HR_AssetTypeViewModel> GetAssetTypeList(string assettypeName = "", string Status = "")
        {
            List<HR_AssetTypeViewModel> assettypes = new List<HR_AssetTypeViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtAssetTypes = sqlDbInterface.GetAssetTypeList(assettypeName, Status);
                if (dtAssetTypes != null && dtAssetTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAssetTypes.Rows)
                    {
                        assettypes.Add(new HR_AssetTypeViewModel
                        {
                            AssetTypeId = Convert.ToInt32(dr["AssetTypeId"]),
                            AssetTypeName = Convert.ToString(dr["AssetTypeName"]), 
                            AssetType_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return assettypes;
        }

        public HR_AssetTypeViewModel GetAssetTypeDetail(int assettypeId = 0)
        {
            HR_AssetTypeViewModel assettype = new HR_AssetTypeViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtAssetTypes = sqlDbInterface.GetAssetTypeDetail(assettypeId);
                if (dtAssetTypes != null && dtAssetTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAssetTypes.Rows)
                    {
                        assettype = new HR_AssetTypeViewModel
                        {  
                            AssetTypeId = Convert.ToInt32(dr["AssetTypeId"]),
                            AssetTypeName = Convert.ToString(dr["AssetTypeName"]),
                            AssetType_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return assettype;
        }

        public List<HR_AssetTypeViewModel> GetEmployeeAssetApplicationTypeList()
        {
            List<HR_AssetTypeViewModel> assetTypeViewModel = new List<HR_AssetTypeViewModel>();
            try
            {
                List<HR_AssetType> assetTypeList = dbInterface.GetEmployeeAssetApplicationTypeList();
                if (assetTypeList != null && assetTypeList.Count > 0)
                {
                    foreach (HR_AssetType advance in assetTypeList)
                    {
                        assetTypeViewModel.Add(new HR_AssetTypeViewModel { AssetTypeId = advance.AssetTypeId, AssetTypeName = advance.AssetTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return assetTypeViewModel;
        }

        public List<EmployeeAssetApplicationViewModel> GetEmployeeAssetApplicationDetails(int companyId, int userId, int reportingUserId, int reportingRoleId)
        {

            List<EmployeeAssetApplicationViewModel> assetApplicationList = new List<EmployeeAssetApplicationViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtList = sqlDbInterface.GetEmployeeAssetApplicationDetails(companyId, userId, reportingUserId, reportingRoleId);
                if (dtList != null && dtList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtList.Rows)
                    {
                        assetApplicationList.Add(new EmployeeAssetApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            AssetTypeName = Convert.ToString(dr["AssetTypeName"]),
                            AssetReason = Convert.ToString(dr["AssetReason"]),
                            ApplicationStatus = Convert.ToString(dr["ApplicationStatus"]),
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
            return assetApplicationList;
        }




    }
}
