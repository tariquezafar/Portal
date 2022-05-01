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
    public class PayHeadGLMappingBL
    {
        HRMSDBInterface dbInterface;
        public PayHeadGLMappingBL()
        {
            dbInterface = new HRMSDBInterface();
        }
        public ResponseOut AddEditPayHeadGLMapping(PayHeadGLMappingViewModel payHeadGLMappingViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                PR_PayHeadGLMapping payHeadGLMapping = new PR_PayHeadGLMapping
                {
                    PayHeadMappingId = payHeadGLMappingViewModel.PayHeadMappingId,
                    PayHeadName = payHeadGLMappingViewModel.PayHeadName,
                    CompanyId = payHeadGLMappingViewModel.CompanyId, 
                    GLId = payHeadGLMappingViewModel.GLId,
                    GLCode = payHeadGLMappingViewModel.GLCode,
                    SLId = payHeadGLMappingViewModel.SLId,
                    SLCode = payHeadGLMappingViewModel.SLCode, 
                    Status = payHeadGLMappingViewModel.PayHeadGLMapping_Status,
                    CompanyBranchId= payHeadGLMappingViewModel.CompanyBranch
                };
                responseOut = dbInterface.AddEditPayHeadGLMapping(payHeadGLMapping);

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
        public List<PayHeadGLMappingViewModel> GetPayHeadGLMappingList(string payHeadName = "", int companyId = 0, string status = "",string companyBranch="")
        {
            List<PayHeadGLMappingViewModel> payHeadGLMappingViewModels = new List<PayHeadGLMappingViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtTaxes = sqlDbInterface.GetPayHeadGLMappingList(payHeadName, companyId, status, companyBranch);
                if (dtTaxes != null && dtTaxes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTaxes.Rows)
                    {
                        payHeadGLMappingViewModels.Add(new PayHeadGLMappingViewModel
                        {
                            PayHeadMappingId = Convert.ToInt32(dr["PayHeadMappingId"]),
                            PayHeadName = Convert.ToString(dr["PayHeadName"]),  
                            PayHeadGLMapping_Status = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchName= Convert.ToString(dr["BranchName"])

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return payHeadGLMappingViewModels;
        }

        public PayHeadGLMappingViewModel GetPayHeadGLMappingDetail(int payHeadMappingId = 0)
        {
            PayHeadGLMappingViewModel payHeadGLMappingViewModel = new PayHeadGLMappingViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtPayHeads = sqlDbInterface.GetPayHeadGLMappingDetail(payHeadMappingId);
                if (dtPayHeads != null && dtPayHeads.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPayHeads.Rows)
                    {
                        payHeadGLMappingViewModel = new PayHeadGLMappingViewModel
                        {
                            PayHeadMappingId = Convert.ToInt32(dr["PayHeadMappingId"]),
                            PayHeadName = Convert.ToString(dr["PayHeadName"]),
                            GLCode = Convert.ToString(dr["GLCode"]),
                            SLCode = Convert.ToString(dr["SLCode"]),
                            GLId = Convert.ToInt32(dr["GLId"]),
                            SLId = Convert.ToInt32(dr["SLId"]),
                            TaxGLHead = Convert.ToString(dr["TaxGLHead"]),
                            TaxSLHead = Convert.ToString(dr["TaxSLHead"]), 
                            PayHeadGLMapping_Status = Convert.ToBoolean(dr["Status"]),
                            CompanyBranch = Convert.ToInt32(dr["CompanyBranchId"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return payHeadGLMappingViewModel;
        }
     


    }
}








