using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class EmployeeLoanApplicationViewModel
    {
        public long ApplicationId { get; set; }       
        public string ApplicationNo { get; set; }
        public int CompanyId { get; set; }

        public string EmployeeName { get; set; }
        public string LoanTypeName { get; set; }
        public string ApplicationDate { get; set; }
        public long EmployeeId { get; set; }        
        public int LoanTypeId { get; set; }
        public decimal LoanInterestRate { get; set; }
        public string InterestCalcOn { get; set; }
        public string ApprovedByUserName { get; set; }
        public decimal LoanAmount { get; set; }
        public string LoanStartDate { get; set; }
        public string LoanEndDate { get; set; }
        public decimal LoanInstallmentAmount { get; set; }
        public string LoanReason { get; set; }
        public string LoanStatus { get; set; }

        public int ApproveBy { get; set; }

        public string ApproveDate { get; set; }
        public int RejectBy { get; set; }
        public string RejectDate { get; set; }

        public string RejectReason { get; set; }
        public string message { get; set; }
        public string status { get; set; }
     
    }
   
}
