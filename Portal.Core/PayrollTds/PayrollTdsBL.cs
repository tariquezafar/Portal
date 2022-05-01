using Portal.Common;
using Portal.Core.ViewModel;
using Portal.DAL;
using Portal.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Portal.Core
{
    public  class PayrollTdsBL
    {
        public ResponseOut AddEditPayrollTds(PayRollTdsViewModel PayRollTdsViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                PR_PayrollTdsSlab PayrollTdsSlab = new PR_PayrollTdsSlab
                {
                      TdsSlaBid = PayRollTdsViewModel.TdsSlaBid,
                      Companyid= PayRollTdsViewModel.Companyid,
                      CompanyBranchid= PayRollTdsViewModel.CompanyBranchid,
                      FromDate = Convert.ToDateTime(PayRollTdsViewModel.FromDate),
                      ToDate = Convert.ToDateTime(PayRollTdsViewModel.ToDate),
                      FromAmount = PayRollTdsViewModel.FromAmount,
                      ToAmount = PayRollTdsViewModel.ToAmount,
                      Category= PayRollTdsViewModel.Category,
                      TDSPerc= PayRollTdsViewModel.TDSPerc,
                      CessPerc= PayRollTdsViewModel.CessPerc,
                      SurcharegePerc= PayRollTdsViewModel.SurcharegePerc,
                      SurchargePerc2= PayRollTdsViewModel.SurchargePerc2,
                      SurchargePerc3= PayRollTdsViewModel.SurchargePerc3,
                      YearlyDiscount= PayRollTdsViewModel.YearlyDiscount,
                      MonthlyDiscount= PayRollTdsViewModel.MonthlyDiscount,
                      CreatedBy= PayRollTdsViewModel.CreatedBy,
                      CreatedDate= Convert.ToDateTime(PayRollTdsViewModel.CreatedDate),
                      Modifiedby= PayRollTdsViewModel.Modifiedby,
                      ModifiedDate= Convert.ToDateTime(PayRollTdsViewModel.ModifiedDate)


                };
                responseOut = sqlDbInterface.AddEditPayrollTds(PayrollTdsSlab);

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
                            Category= Convert.ToString(dr["category"]),
                            TDSPerc = Convert.ToDecimal(dr["TDSPerc"]),
                            CessPerc = Convert.ToDecimal(dr["CessPerc"]),
                            YearlyDiscount = Convert.ToDecimal(dr["YearlyDiscount"]),
                            MonthlyDiscount = Convert.ToDecimal(dr["MonthlyDiscount"]),
                            TdsSlaBid= Convert.ToInt32(dr["TdsSlaBid"]),
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
                            CompanyBranchid=  Convert.ToInt32(dr["CompanyBranchid"])
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
