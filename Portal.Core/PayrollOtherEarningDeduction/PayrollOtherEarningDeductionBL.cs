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
    public class PayrollOtherEarningDeductionBL
    {
        HRMSDBInterface dbInterface;
        public PayrollOtherEarningDeductionBL()
        {
            dbInterface = new HRMSDBInterface();
        }
        public ResponseOut AddEditPayrollOtherEarningDeduction(PayrollOtherEarningDeductionViewModel payrollOtherEarningDeductionViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                PR_PayrollOtherEarningDeduction payrollOtherEarningDeductions = new PR_PayrollOtherEarningDeduction
                {
                    MonthlyInputId= payrollOtherEarningDeductionViewModel.MonthlyInputId,
                    PayrollProcessingPeriodId = payrollOtherEarningDeductionViewModel.PayrollProcessingPeriodId,
                    PayrollProcessingStartDate = Convert.ToDateTime(payrollOtherEarningDeductionViewModel.PayrollProcessingStartDate),
                    PayrollProcessingEndDate = Convert.ToDateTime(payrollOtherEarningDeductionViewModel.PayrollProcessingEndDate),
                    MonthId = payrollOtherEarningDeductionViewModel.MonthId,
                    CompanyId = payrollOtherEarningDeductionViewModel.CompanyId,
                    CompanyBranchId = payrollOtherEarningDeductionViewModel.CompanyBranchId,
                    DepartmentId = payrollOtherEarningDeductionViewModel.DepartmentId,
                    EmployeeId = payrollOtherEarningDeductionViewModel.EmployeeId,                                       
                    TDSApplicable = payrollOtherEarningDeductionViewModel.TDSApplicable == "1"?true:false,
                    IncomeTax =payrollOtherEarningDeductionViewModel.IncomeTax,
                    AnnualBonus = payrollOtherEarningDeductionViewModel.AnnualBonus,
                    Exgretia = payrollOtherEarningDeductionViewModel.Exgretia,
                    Incentive = payrollOtherEarningDeductionViewModel.Incentive,
                    LeaveEncashment = payrollOtherEarningDeductionViewModel.LeaveEncashment,
                    NoticePayPayable = payrollOtherEarningDeductionViewModel.NoticePayPayable,
                    OverTimeAllow = payrollOtherEarningDeductionViewModel.OverTimeAllow,
                    VariablePay = payrollOtherEarningDeductionViewModel.VariablePay,
                    OtherDeduction = payrollOtherEarningDeductionViewModel.OtherDeduction,
                    OtherAllowance = payrollOtherEarningDeductionViewModel.OtherAllowance,
                    LoanPayable = payrollOtherEarningDeductionViewModel.LoanPayable,
                    LoanRecv = payrollOtherEarningDeductionViewModel.LoanRecv,
                    AdvancePayable = payrollOtherEarningDeductionViewModel.AdvancePayable,
                    AdvanceRecv = payrollOtherEarningDeductionViewModel.AdvanceRecv
                };
                
                responseOut = sqlDbInterface.AddEditPayrollOtherEarningDeduction(payrollOtherEarningDeductions);

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

        public List<PayrollOtherEarningDeductionViewModel> GetPayrollOtherEarningDeductionList(int payrollProcessingPeriodId, int employeeId, int departmentID, int companyBranchID, int companyId)
        {
            List<PayrollOtherEarningDeductionViewModel> payrollOtherEarningDeductionViewModel = new List<PayrollOtherEarningDeductionViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtPayrollProcessPeriod = sqlDbInterface.GetPayrollOtherEarningDeductionList(payrollProcessingPeriodId, employeeId, departmentID, companyBranchID, companyId);
                if (dtPayrollProcessPeriod != null && dtPayrollProcessPeriod.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPayrollProcessPeriod.Rows)
                    {
                        payrollOtherEarningDeductionViewModel.Add(new PayrollOtherEarningDeductionViewModel
                        {
                            MonthlyInputId = Convert.ToInt32(dr["MonthlyInputId"]),
                            PayrollProcessingPeriodId = Convert.ToInt32(dr["PayrollProcessingPeriodId"]),
                            PayrollProcessingStartDate = Convert.ToString(dr["PayrollProcessingStartDate"]),
                            PayrollProcessingEndDate = Convert.ToString(dr["PayrollProcessingEndDate"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            DepartmentId = Convert.ToInt32(dr["DepartmentId"]),
                            DepartmentName = Convert.ToString(dr["DepartmentName"]),
                            DesignationName= Convert.ToString(dr["DesignationName"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["BranchName"]),
                            IncomeTax = Convert.ToDecimal(dr["IncomeTax"]),
                            AnnualBonus = Convert.ToDecimal(dr["AnnualBonus"]),
                            Exgretia = Convert.ToDecimal(dr["Exgretia"]),
                            Incentive = Convert.ToDecimal(dr["Incentive"]),
                            OverTimeAllow = Convert.ToDecimal(dr["OverTimeAllow"]),
                            OtherDeduction = Convert.ToDecimal(dr["OtherDeduction"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return payrollOtherEarningDeductionViewModel;
        }

        public PayrollOtherEarningDeductionViewModel GetPayrollOtherEarningDeductionDetail(long monthlyInputId = 0)
        {
            PayrollOtherEarningDeductionViewModel payrollOtherEarningDeductionViewModel = new PayrollOtherEarningDeductionViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtPayrollProcessPeriod = sqlDbInterface.GetPayrollOtherEarningDeductionDetail(monthlyInputId);
                if (dtPayrollProcessPeriod != null && dtPayrollProcessPeriod.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPayrollProcessPeriod.Rows)
                    {
                        payrollOtherEarningDeductionViewModel = new PayrollOtherEarningDeductionViewModel
                        {
                            MonthlyInputId = Convert.ToInt32(dr["MonthlyInputId"]),
                            PayrollProcessingPeriodId = Convert.ToInt32(dr["PayrollProcessingPeriodId"]),
                            PayrollProcessingStartDate = Convert.ToString(dr["PayrollProcessingStartDate"]),
                            PayrollProcessingEndDate = Convert.ToString(dr["PayrollProcessingEndDate"]),
                            PayrollProcessDate= Convert.ToString(dr["PayrollProcessDate"]),
                            MonthId = Convert.ToInt32(dr["MonthId"]),                           
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            DepartmentId = Convert.ToInt32(dr["DepartmentId"]),
                            DesignationId= Convert.ToInt32(dr["DesignationId"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                            TDSApplicable = Convert.ToString(dr["TDSApplicable"]),                        
                            IncomeTax = Convert.ToDecimal(dr["IncomeTax"]),
                            Incentive = Convert.ToDecimal(dr["Incentive"]),
                            VariablePay = Convert.ToDecimal(dr["VariablePay"]),
                            AnnualBonus = Convert.ToDecimal(dr["AnnualBonus"]),
                            Exgretia = Convert.ToDecimal(dr["Exgretia"]),
                            LeaveEncashment = Convert.ToDecimal(dr["LeaveEncashment"]),
                            NoticePayPayable = Convert.ToDecimal(dr["NoticePayPayable"]),
                            OtherDeduction= Convert.ToDecimal(dr["OtherDeduction"]),
                            OverTimeAllow = Convert.ToDecimal(dr["OverTimeAllow"]),
                            OtherAllowance = Convert.ToDecimal(dr["OtherAllowance"]),
                            LoanPayable = Convert.ToDecimal(dr["LoanPayable"]),
                            LoanRecv = Convert.ToDecimal(dr["LoanRecv"]),
                            AdvancePayable = Convert.ToDecimal(dr["AdvancePayable"]),
                            AdvanceRecv = Convert.ToDecimal(dr["AdvanceRecv"])                          
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return payrollOtherEarningDeductionViewModel;
        }

    }
}








