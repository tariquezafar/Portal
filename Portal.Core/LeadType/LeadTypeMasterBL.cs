using Portal.Common;
using Portal.Core.ViewModel;
using Portal.DAL;
using Portal.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;


namespace Portal.Core.LeadType
{
   public  class LeadTypeMasterBL
    {
        public ResponseOut AddEditLeadTypeMaster(LeadTypeMasterViewModel leadTypeMasterViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                LeadMaster LeadTypeMaster = new LeadMaster
                {
                    LeadTypeId = leadTypeMasterViewModel.LeadTypeId,
                    LeadTypeName = leadTypeMasterViewModel.LeadTypeName,
                    Companyid = leadTypeMasterViewModel.Companyid,
                    CompanyBranchid = leadTypeMasterViewModel.CompanyBranchid,
                    CreatedBy = leadTypeMasterViewModel.CreatedBy,
                    CreatedDate = Convert.ToDateTime(leadTypeMasterViewModel.CreatedDate),
                    Modifiedby = leadTypeMasterViewModel.Modifiedby,
                    ModifiedDate = Convert.ToDateTime(leadTypeMasterViewModel.ModifiedDate),
                    status = Convert.ToBoolean(leadTypeMasterViewModel.status),
                    


               };
                responseOut = sqlDbInterface.AddEditLeadTypeMaster(LeadTypeMaster);

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

        public List<PayRollTdsViewModel> GetPayRollTdsList(string fromdate, string todate, string category)
        {
            List<PayRollTdsViewModel> payRollTdsViewModel = new List<PayRollTdsViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                if (category == "0")
                {
                    category = "";
                }
                DataTable dtPayrollTDS = sqlDbInterface.GetPayrollTDSList(fromdate, todate, category);
                if (dtPayrollTDS != null && dtPayrollTDS.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPayrollTDS.Rows)
                    {
                        payRollTdsViewModel.Add(new PayRollTdsViewModel
                        {
                            FromDate = Convert.ToString(dr["FromDate"]),
                            ToDate = Convert.ToString(dr["ToDate"]),
                            FromAmount = Convert.ToDecimal(dr["FromAmount"]),
                            ToAmount = Convert.ToDecimal(dr["ToAmount"]),
                            Category = Convert.ToString(dr["category"]),
                            TDSPerc = Convert.ToDecimal(dr["TDSPerc"]),
                            CessPerc = Convert.ToDecimal(dr["CessPerc"]),
                            YearlyDiscount = Convert.ToDecimal(dr["YearlyDiscount"]),
                            MonthlyDiscount = Convert.ToDecimal(dr["MonthlyDiscount"]),
                            TdsSlaBid = Convert.ToInt32(dr["TdsSlaBid"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedBy"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedbyUserName = Convert.ToString(dr["Modifiedby"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return payRollTdsViewModel;
        }

        public List<LeadTypeMasterViewModel> GetAllLeadList(string txtLeadType,string companyBranch)
        {
            List<LeadTypeMasterViewModel> leadTypeMasterViewModel = new List<LeadTypeMasterViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
               if(txtLeadType==null)
                {
                    txtLeadType = "";
                }
                DataTable dtLeadMaster = sqlDbInterface.GetLeadMasterList(txtLeadType, companyBranch);
                if (dtLeadMaster != null && dtLeadMaster.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtLeadMaster.Rows)
                    {
                        leadTypeMasterViewModel.Add(new LeadTypeMasterViewModel
                        {
                            LeadTypeName = Convert.ToString(dr["LeadTypeName"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedBy"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedbyUserName = Convert.ToString(dr["Modifiedby"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            LeadTypeId=Convert.ToInt32(dr["LeadTypeId"]),
                            CompanyBranchName = Convert.ToString(dr["BranchName"])

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leadTypeMasterViewModel;
        }

        public LeadTypeMasterViewModel GetLeadTypeDetails(int leadId)
        {
            LeadTypeMasterViewModel payrolltdsViewModel = new LeadTypeMasterViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable LeadDetails = sqlDbInterface.GetLeadDetails(leadId);
                if (LeadDetails != null && LeadDetails.Rows.Count > 0)
                {
                    foreach (DataRow dr in LeadDetails.Rows)
                    {
                        payrolltdsViewModel = new LeadTypeMasterViewModel
                        {
                            LeadTypeName = Convert.ToString(dr["LeadTypeName"]),
                            CompanyBranchid = Convert.ToInt32(dr["CompanyBranchid"])
                        };
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return payrolltdsViewModel;
        }
    

        public PayRollTdsViewModel GetPayrollTdsDetails(int payrollTds)
        {
            PayRollTdsViewModel payrolltdsViewModel = new PayRollTdsViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPayrollTDS = sqlDbInterface.GetPayrollTDSDetails(payrollTds);
                if (dtPayrollTDS != null && dtPayrollTDS.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPayrollTDS.Rows)
                    {
                        payrolltdsViewModel = new PayRollTdsViewModel
                        {
                            TdsSlaBid = Convert.ToInt32(dr["TdsSlaBid"]),
                            FromDate = Convert.ToString(dr["FromDate"]),
                            FromAmount = Convert.ToDecimal(dr["FromAmount"]),
                            ToDate = Convert.ToString(dr["ToDate"]),
                            ToAmount = Convert.ToDecimal(dr["ToAmount"]),
                            Category = Convert.ToString(dr["Category"]),
                            TDSPerc = Convert.ToDecimal(dr["TDSPerc"]),
                            CessPerc = Convert.ToDecimal(dr["CessPerc"]),
                            SurcharegePerc = Convert.ToDecimal(dr["SurcharegePerc"]),
                            SurchargePerc2 = Convert.ToDecimal(dr["SurchargePerc2"]),
                            SurchargePerc3 = Convert.ToDecimal(dr["SurchargePerc3"]),
                            YearlyDiscount = Convert.ToDecimal(dr["YearlyDiscount"]),
                            MonthlyDiscount = Convert.ToDecimal(dr["MonthlyDiscount"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return payrolltdsViewModel;
        }
    }
}
