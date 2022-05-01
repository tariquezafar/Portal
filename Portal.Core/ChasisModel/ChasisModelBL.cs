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
    public class ChasisModelBL
    {
        DBInterface dbInterface;
        public ChasisModelBL()
        {
            dbInterface = new DBInterface();
        }

        public ResponseOut AddEditChasisModel(ChasisModelViewModel chasisModelViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                ChasisModel chasisModel = new ChasisModel
                {
                    ChasisModelID = chasisModelViewModel.ChasisModelID,
                    ChasisModelName = chasisModelViewModel.ChasisModelName,
                    ChasisModelCode = chasisModelViewModel.ChasisModelCode,
                    MotorModelCode = chasisModelViewModel.MotorModelCode,
                    ProductSubGroupId= chasisModelViewModel.ProductSubGroupId,
                    ChasisModelStatus = chasisModelViewModel.ChasisModelStatus,
                    CompanyBranchId = chasisModelViewModel.CompanyBranchId
                };
                responseOut = dbInterface.AddEditChasisModel(chasisModel);
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

        public List<ChasisModelViewModel> GetChasisModelList(string chasisModelName = "", string chasisModelCode = "", string motorModelCode = "", int productSubGroupId = 0, string ChasisModelStatus = "", int companyBranchId=0)
        {
            List<ChasisModelViewModel> chasisModelViewModels = new List<ChasisModelViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProductSubGroups = sqlDbInterface.GetChasisModelList(chasisModelName, chasisModelCode, motorModelCode, productSubGroupId, ChasisModelStatus, companyBranchId);
                if (dtProductSubGroups != null && dtProductSubGroups.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProductSubGroups.Rows)
                    {
                        chasisModelViewModels.Add(new ChasisModelViewModel
                        {
                            ChasisModelID = Convert.ToInt32(dr["ChasisModelID"]),
                            ProductSubGroupId = Convert.ToInt32(dr["ProductSubGroupId"]),
                            ProductSubGroupName = Convert.ToString(dr["ProductSubGroupName"]),
                            ChasisModelName = Convert.ToString(dr["ChasisModelName"]),
                            ChasisModelCode = Convert.ToString(dr["ChasisModelCode"]),
                            MotorModelCode = Convert.ToString(dr["MotorModelCode"]),
                            ChasisModelStatus = Convert.ToBoolean(dr["ChasisModelStatus"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return chasisModelViewModels;
        }

        public ChasisModelViewModel GetChasisModelDetail(long chasisModelID = 0)
        {
            ChasisModelViewModel chasisModelViewModel = new ChasisModelViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProductSubGroups = sqlDbInterface.GetChasisModelDetail(chasisModelID);
                if (dtProductSubGroups != null && dtProductSubGroups.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProductSubGroups.Rows)
                    {
                        chasisModelViewModel = new ChasisModelViewModel
                        {
                            ChasisModelID = Convert.ToInt32(dr["ChasisModelID"]),
                            ProductSubGroupId = Convert.ToInt32(dr["ProductSubGroupId"]),
                            ProductSubGroupName = Convert.ToString(dr["ProductSubGroupName"]),
                            ChasisModelName = Convert.ToString(dr["ChasisModelName"]),
                            ChasisModelCode = Convert.ToString(dr["ChasisModelCode"]),
                            MotorModelCode = Convert.ToString(dr["MotorModelCode"]),
                            ChasisModelStatus = Convert.ToBoolean(dr["ChasisModelStatus"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName= Convert.ToString(dr["CompanyBranchName"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return chasisModelViewModel;
        }

        public DataTable ChasisModelPrint(string chasisModelName = "", string chasisModelCode = "", string motorModelCode = "", int productSubGroupId = 0, string ChasisModelStatus = "")
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtChasisModel = new DataTable();
            try
            {
                dtChasisModel = sqlDbInterface.ChasisModelPrint(chasisModelName, chasisModelCode, motorModelCode, productSubGroupId, ChasisModelStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtChasisModel;
        }

    }
}
