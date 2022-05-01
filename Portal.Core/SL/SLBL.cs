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
    public class SLBL
    {
        DBInterface dbInterface;
        public SLBL()
        {
            dbInterface = new DBInterface();
        }

        public ResponseOut AddEditSL(SLViewModel sLViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                SL sL= new SL
                {
                    SLId=sLViewModel.SLId,
                    SLCode=sLViewModel.SLCode,
                    SLHead=sLViewModel.SLHead,
                    RefCode=sLViewModel.RefCode,
                    SLTypeId = sLViewModel.SLTypeId,
                    PostingGLId = sLViewModel.PostingGLId,
                    CostCenterId =sLViewModel.CostCenterId,
                    CompanyId=sLViewModel.CompanyId,
                    CreatedBy =sLViewModel.CreatedBy,
                    Status = sLViewModel.SL_Status,
                    CompanyBranchId = sLViewModel.CompanyBranchId,
                };
                responseOut = dbInterface.AddEditSL(sL);
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



        public List<SLViewModel> GetSLList(string SLHead = "", string SLCode="", int SLTypeId=0, int CostCenterId=0, int CompanyId=0, string Status="",int CompanyBranchId=0)
        {
            List<SLViewModel> sLList = new List<SLViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtsL = sqlDbInterface.GetSLList(SLHead,SLCode, SLTypeId, CostCenterId, CompanyId, Status, CompanyBranchId);
                if (dtsL != null && dtsL.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtsL.Rows)
                    {
                        sLList.Add(new SLViewModel
                        {
                            SLId = Convert.ToInt32(dr["SLId"]),
                            SLCode = Convert.ToString(dr["SLCode"]),
                            SLHead = Convert.ToString(dr["SLHead"]),
                            RefCode = Convert.ToString(dr["RefCode"]),
                            SLTypeId = Convert.ToInt32(dr["SLTypeId"]),
                            SLTypeName = Convert.ToString(dr["SLTypeName"]),
                            CostCenterId = Convert.ToInt32(dr["CostCenterId"]),
                            CostCenterName = Convert.ToString(dr["CostCenterName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            CreatedName = Convert.ToString(dr["CreatedByName"]),
                            ModifiedDate = string.IsNullOrEmpty(Convert.ToString(dr["ModifiedDate"])) ? "" : Convert.ToString(dr["ModifiedDate"]),
                            SL_Status = Convert.ToBoolean(dr["Status"]),
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
            return sLList;
        }

        public SLViewModel GetSLDetail(int sLId = 0)
        {
            SLViewModel sL = new SLViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSLs = sqlDbInterface.GetSLDetail(sLId);
                if (dtSLs != null && dtSLs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSLs.Rows)
                    {
                        sL = new SLViewModel
                        {
                            SLId = Convert.ToInt32(dr["SLId"]),
                            SLCode = Convert.ToString(dr["SLCode"]),
                            SLHead = Convert.ToString(dr["SLHead"]),
                            RefCode = Convert.ToString(dr["RefCode"]),
                            SLTypeId = Convert.ToInt32(dr["SLTypeId"]),
                            GLHead = Convert.ToString(dr["GLHead"]),
                            PostingGLId = Convert.ToInt32(dr["PostingGLId"]),
                            SLTypeName = Convert.ToString(dr["SLTypeName"]),
                            CostCenterId = Convert.ToInt32(dr["CostCenterId"]),
                            CostCenterName = Convert.ToString(dr["CostCenterName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            CreatedName = Convert.ToString(dr["CreatedByName"]),
                            ModifiedDate = string.IsNullOrEmpty(Convert.ToString(dr["ModifiedDate"])) ? "" : Convert.ToString(dr["ModifiedDate"]),
                            SL_Status = Convert.ToBoolean(dr["Status"]),
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
            return sL ;
        }


        public List<SLTypeViewModel> GetSLTypeList()
        {
            List<SLTypeViewModel> slTypes = new List<SLTypeViewModel>();
            try
            {
                List<SLType> slTypeList = dbInterface.GetSLTList();
                if (slTypeList != null && slTypeList.Count > 0)
                {
                    foreach (SLType slType in slTypeList)
                    {
                        slTypes.Add(new SLTypeViewModel { SLTypeId = slType.SLTypeId, SLTypeName = slType.SLTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return slTypes;
        }

        public List<CostCenterViewModel> GetCostCenterList()
        {
            List<CostCenterViewModel> costCenters = new List<CostCenterViewModel>();
            try
            {
                List<CostCenter> costCenterList = dbInterface.GetCostCenterList();
                if (costCenterList != null && costCenterList.Count > 0)
                {
                    foreach (CostCenter item in costCenterList)
                    {
                        costCenters.Add(new CostCenterViewModel { CostCenterId = item.CostCenterId, CostCenterName = item.CostCenterName });
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

        public List<SLViewModel> GetSLAutoCompleteList(string searchTerm,int slTypeId, int companyId)
        {
            List<SLViewModel> sls = new List<SLViewModel>();
            try
            {
                List<SL> slList = dbInterface.GetSLAutoCompleteList(searchTerm,slTypeId, companyId);
                if (slList != null && slList.Count > 0)
                {
                    foreach (SL sl in slList)
                    {
                        sls.Add(new SLViewModel { SLId= sl.SLId, SLHead= sl.SLHead, SLCode= sl.SLCode});
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sls;
        }

        public List<SLViewModel> GetSLAutoCompleteListForTax(string term, int companyId)
        {
            List<SLViewModel> sls = new List<SLViewModel>();
            try
            {
                List<SL> slList = dbInterface.GetSLAutoCompleteListForTax(term, companyId);
                if (slList != null && slList.Count > 0)
                {
                    foreach (SL sl in slList)
                    {
                        sls.Add(new SLViewModel { SLId = sl.SLId, SLHead = sl.SLHead, SLCode = sl.SLCode });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sls;
        }

        public ResponseOut ImportSL(SLViewModel sLViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                SL sL = new SL
                {
                    SLId = sLViewModel.SLId,
                    SLCode = sLViewModel.SLCode,
                    SLHead = sLViewModel.SLHead,
                    RefCode = sLViewModel.RefCode,
                    SLTypeId = sLViewModel.SLTypeId,
                    PostingGLId = sLViewModel.PostingGLId,
                    CostCenterId = sLViewModel.CostCenterId,
                    CompanyId = sLViewModel.CompanyId,
                    CreatedBy = sLViewModel.CreatedBy,
                    Status = sLViewModel.SL_Status,
                };
                responseOut = dbInterface.AddEditSL(sL);
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

    }
}
