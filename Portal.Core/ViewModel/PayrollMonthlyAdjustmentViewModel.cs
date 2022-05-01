using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class PayrollMonthlyAdjustmentViewModel: IModel
    {
        public long PayrollAdjustmentId { get; set; }
        public int PayrollProcessingPeriodId { get; set; }
        public string PayrollProcessingStartDate { get; set; }
        public string PayrollProcessingEndDate { get; set; }
        public string PayrollProcessDate { get; set; }        
        public int MonthId { get; set; }
        public string MonthShortName { get; set; }
        public int CompanyId { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }        
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }       
        public decimal BasicPay { get; set; }
        public decimal ConveyanceAllow { get; set; }
        public decimal SpecialAllow { get; set; }
        public decimal OtherAllow { get; set; }
        public decimal MedicalAllow { get; set; }
        public decimal ChildEduAllow { get; set; }
        public decimal LTA { get; set; }
        public decimal EmployeePF { get; set; }
        public decimal EmployeeESI { get; set; }
        public decimal OtherDeduction { get; set; }
        public decimal ProfessionalTax { get; set; }
        public decimal AdhocAllowance { get; set; }
        public decimal AnnualBonus { get; set; }
        public decimal Exgratia { get; set; }
        public decimal LeaveEncashment { get; set; }
        public decimal SalaryAdvancePayable { get; set; }
        public decimal NoticePayPayable { get; set; }
        public decimal SalaryAdvanceRecv { get; set; }
        public decimal NoticePayRecv { get; set; }
        public decimal LoanPayable { get; set; }
        public decimal LoanRecv { get; set; }
        public decimal IncomeTax { get; set; }    
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public string message { get; set; }
        public bool status { get; set; } 
    }
    
}
