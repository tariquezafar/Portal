using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class EmployeeAdvanceApplicationViewModel
    {
        public long ApplicationId { get; set; }       
        public string ApplicationNo { get; set; }
        public string ApplicationDate { get; set; }
        public int CompanyId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string AdvanceTypeName{ get; set; }      
        public int AdvanceTypeId { get; set; }           
        public string AdvanceReason { get; set; }
        public string AdvanceStatus { get; set; }
        public decimal AdvanceAmount { get; set; }
        public string message { get; set; }
        public string status { get; set; }
     
    }
   
}
