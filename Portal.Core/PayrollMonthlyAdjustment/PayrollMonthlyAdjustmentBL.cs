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
    public class PayrollMonthlyAdjustmentBL
    {
        HRMSDBInterface dbInterface;
        public PayrollMonthlyAdjustmentBL()
        {
            dbInterface = new HRMSDBInterface();
        }
        public ResponseOut AddEditPayrollMonthlyAdjustment(PayrollMonthlyAdjustmentViewModel payrollMonthlyAdjustmentViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                PR_PayrollMonthlyAdjustment payrollMonthlyAdjustments = new PR_PayrollMonthlyAdjustment
                {
                    PayrollAdjustmentId = payrollMonthlyAdjustmentViewModel.PayrollAdjustmentId,
                    PayrollProcessingPeriodId = payrollMonthlyAdjustmentViewModel.PayrollProcessingPeriodId,
                    PayrollProcessingStartDate = Convert.ToDateTime(payrollMonthlyAdjustmentViewModel.PayrollProcessingStartDate),
                    PayrollProcessingEndDate = Convert.ToDateTime(payrollMonthlyAdjustmentViewModel.PayrollProcessingEndDate),
                    MonthId = payrollMonthlyAdjustmentViewModel.MonthId,
                    CompanyId = payrollMonthlyAdjustmentViewModel.CompanyId,
                    CompanyBranchId = payrollMonthlyAdjustmentViewModel.CompanyBranchId,
                    DepartmentId = payrollMonthlyAdjustmentViewModel.DepartmentId,
                    EmployeeId = payrollMonthlyAdjustmentViewModel.EmployeeId,
                    BasicPay = payrollMonthlyAdjustmentViewModel.BasicPay,
                    ConveyanceAllow = payrollMonthlyAdjustmentViewModel.ConveyanceAllow,
                    SpecialAllow = payrollMonthlyAdjustmentViewModel.SpecialAllow,
                    OtherAllow = payrollMonthlyAdjustmentViewModel.OtherAllow,
                    MedicalAllow = payrollMonthlyAdjustmentViewModel.MedicalAllow,
                    ChildEduAllow = payrollMonthlyAdjustmentViewModel.ChildEduAllow,
                    LTA = payrollMonthlyAdjustmentViewModel.LTA,
                    EmployeePF = payrollMonthlyAdjustmentViewModel.EmployeePF,
                    EmployeeESI = payrollMonthlyAdjustmentViewModel.EmployeeESI,
                    OtherDeduction = payrollMonthlyAdjustmentViewModel.OtherDeduction,
                    ProfessionalTax = payrollMonthlyAdjustmentViewModel.ProfessionalTax,
                    AdhocAllowance = payrollMonthlyAdjustmentViewModel.AdhocAllowance,
                    AnnualBonus = payrollMonthlyAdjustmentViewModel.AnnualBonus,
                    Exgratia = payrollMonthlyAdjustmentViewModel.Exgratia,
                    LeaveEncashment = payrollMonthlyAdjustmentViewModel.LeaveEncashment,
                    SalaryAdvancePayable = payrollMonthlyAdjustmentViewModel.SalaryAdvancePayable,
                    NoticePayPayable = payrollMonthlyAdjustmentViewModel.NoticePayPayable,
                    SalaryAdvanceRecv = payrollMonthlyAdjustmentViewModel.SalaryAdvanceRecv,
                    NoticePayRecv = payrollMonthlyAdjustmentViewModel.NoticePayRecv,
                    LoanPayable = payrollMonthlyAdjustmentViewModel.LoanPayable,
                    LoanRecv = payrollMonthlyAdjustmentViewModel.LoanRecv,
                    IncomeTax = payrollMonthlyAdjustmentViewModel.IncomeTax,
                    CreatedBy = payrollMonthlyAdjustmentViewModel.CreatedBy
                };
                
                responseOut = sqlDbInterface.AddEditPayrollMonthlyAdjustment(payrollMonthlyAdjustments);

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

        public List<PayrollMonthlyAdjustmentViewModel> GetPayrollMonthlyAdjustmentList(int payrollProcessingPeriodId, int employeeId, int departmentID, int companyBranchID, int companyId)
        {
            List<PayrollMonthlyAdjustmentViewModel> payrollMonthlyAdjustmentViewModel = new List<PayrollMonthlyAdjustmentViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtPayrollProcessPeriod = sqlDbInterface.GetPayrollMonthlyAdjustmentList(payrollProcessingPeriodId, employeeId, departmentID, companyBranchID, companyId);
                if (dtPayrollProcessPeriod != null && dtPayrollProcessPeriod.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPayrollProcessPeriod.Rows)
                    {
                        payrollMonthlyAdjustmentViewModel.Add(new PayrollMonthlyAdjustmentViewModel
                        {
                            PayrollAdjustmentId = Convert.ToInt32(dr["PayrollAdjustmentId"]),
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
            return payrollMonthlyAdjustmentViewModel;
        }

        public PayrollMonthlyAdjustmentViewModel GetPayrollMonthlyAdjustmentDetail(long payrollAdjustmentId = 0)
        {
            PayrollMonthlyAdjustmentViewModel payrollMonthlyAdjustmentViewModel = new PayrollMonthlyAdjustmentViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtPayrollProcessPeriod = sqlDbInterface.GetPayrollMonthlyAdjustmentDetail(payrollAdjustmentId);
                if (dtPayrollProcessPeriod != null && dtPayrollProcessPeriod.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPayrollProcessPeriod.Rows)
                    {
                        payrollMonthlyAdjustmentViewModel = new PayrollMonthlyAdjustmentViewModel
                        {
                            PayrollAdjustmentId = Convert.ToInt32(dr["PayrollAdjustmentId"]),
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
                            BasicPay = Convert.ToDecimal(dr["BasicPay"]),
                            ConveyanceAllow = Convert.ToDecimal(dr["ConveyanceAllow"]),
                            SpecialAllow = Convert.ToDecimal(dr["SpecialAllow"]),
                            OtherAllow = Convert.ToDecimal(dr["OtherAllow"]),
                            MedicalAllow = Convert.ToDecimal(dr["MedicalAllow"]),
                            ChildEduAllow = Convert.ToDecimal(dr["ChildEduAllow"]),
                            LeaveEncashment = Convert.ToDecimal(dr["LeaveEncashment"]),
                            NoticePayPayable = Convert.ToDecimal(dr["NoticePayPayable"]),
                            LTA = Convert.ToDecimal(dr["LTA"]),
                            EmployeeESI = Convert.ToDecimal(dr["EmployeeESI"]),
                            EmployeePF = Convert.ToDecimal(dr["EmployeePF"]),
                            OtherDeduction = Convert.ToDecimal(dr["OtherDeduction"]),
                            ProfessionalTax = Convert.ToDecimal(dr["ProfessionalTax"]),
                            AdhocAllowance = Convert.ToDecimal(dr["AdhocAllowance"]),
                            AnnualBonus = Convert.ToDecimal(dr["AnnualBonus"]),
                            Exgratia = Convert.ToDecimal(dr["Exgratia"]),
                            IncomeTax = Convert.ToDecimal(dr["IncomeTax"]),
                            SalaryAdvancePayable = Convert.ToDecimal(dr["SalaryAdvancePayable"]),
                            SalaryAdvanceRecv = Convert.ToDecimal(dr["SalaryAdvanceRecv"]),
                            NoticePayRecv = Convert.ToDecimal(dr["NoticePayRecv"]),
                            LoanPayable = Convert.ToDecimal(dr["LoanPayable"]),
                            LoanRecv = Convert.ToDecimal(dr["LoanRecv"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return payrollMonthlyAdjustmentViewModel;
        }

    }
}








