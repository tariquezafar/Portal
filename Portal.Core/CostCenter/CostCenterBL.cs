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
    public class CostCenterBL
    {
        DBInterface dbInterface;
        public CostCenterBL()
        {
            dbInterface = new DBInterface();
        }


        public ResponseOut AddEditCostCenter(CostCenterViewModel costcenterViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                CostCenter costcenter = new CostCenter
                {
                    CostCenterId = costcenterViewModel.CostCenterId,
                    CostCenterName = costcenterViewModel.CostCenterName,
                    CompanyId = costcenterViewModel.CompanyId, 
                    CreatedBy = costcenterViewModel.CreatedBy,
                    Status = costcenterViewModel.CostCenter_Status,
                    CompanyBranchId=costcenterViewModel.CompanyBranchId
                };
                responseOut = dbInterface.AddEditCostCenter(costcenter);
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


        public List<CostCenterViewModel> GetCostCenterList(string costcenterName = "",int companyId = 0, string status = "",int companyBranchId=0)
        {
            List<CostCenterViewModel> costcenters = new List<CostCenterViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            { 
                DataTable dtCostCenters = sqlDbInterface.GetCostCenterList(costcenterName, companyId, status, companyBranchId);
                if (dtCostCenters != null && dtCostCenters.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCostCenters.Rows)
                    {
                        costcenters.Add(new CostCenterViewModel
                        {
                            CostCenterId = Convert.ToInt32(dr["CostCenterId"]),
                            CostCenterName = Convert.ToString(dr["CostCenterName"]), 
                            CostCenter_Status = Convert.ToBoolean(dr["Status"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"].ToString() == "" ? "0" : dr["CreatedBy"].ToString()),
                            CreatedName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),
                            ModifiedName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return costcenters;
        }

        public CostCenterViewModel GetCostCenterDetail(int costcenterId = 0)
        {
            CostCenterViewModel costcenter = new CostCenterViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCostCenters = sqlDbInterface.GetCostCenterDetail(costcenterId);
                if (dtCostCenters != null && dtCostCenters.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCostCenters.Rows)
                    {
                        costcenter = new CostCenterViewModel
                        {
                            CostCenterId = Convert.ToInt32(dr["CostCenterId"]),
                            CostCenterName = Convert.ToString(dr["CostCenterName"]), 
                            CostCenter_Status = Convert.ToBoolean(dr["Status"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"].ToString() == "" ? "0" : dr["CreatedBy"].ToString()),
                            CreatedName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),
                            ModifiedName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
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
            return costcenter;
        } 



        public List<CostCenterViewModel> GetCostCenterList(int companyId)
        {
            List<CostCenterViewModel> costCenters = new List<CostCenterViewModel>();
            try
            {
                List<CostCenter> costCenterList = dbInterface.GetCostCenterList(companyId);
                if (costCenterList != null && costCenterList.Count > 0)
                {
                    foreach (CostCenter costCenter in costCenterList)
                    {
                        costCenters.Add(new CostCenterViewModel { CostCenterId = costCenter.CostCenterId, CostCenterName = costCenter.CostCenterName});
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return costCenters;
        }
    }
}
