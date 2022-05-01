using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class HR_EmployeeTravelApplicationViewModel : IModel
    {
        public int ApplicationId { get; set; }
        public string ApplicationNo { get; set; }
        public string ApplicationDate { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } 
        public int TravelTypeId { get; set; }
        public string TravelTypeName { get; set; }
        public string TravelDestination { get; set; }
        public string TravelReason { get; set; }  
        public string TravelStatus { get; set; } 
        public string TravelStartDate { get; set; }
        public string TravelEndDate { get; set; }
        public int CompanyId { get; set; } 
        public int ApproveBy { get; set; }
        public string ApprovedByUserName { get; set; }
        public string ApproveDate { get; set; } 
        public int RejectBy { get; set; }
        public string RejectByUserName { get; set; }
        public string RejectDate { get; set; }
        public string RejectReason { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    
}
