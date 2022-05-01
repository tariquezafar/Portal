using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class HR_LeaveTypeViewModel : IModel
    {
        public int LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; }
        public string LeaveTypeCode { get; set; }
        public decimal LeavePeriod { get; set; }
        public decimal PayPeriod { get; set; }
        public decimal WorkPeriod { get; set; }
        public bool LeaveType_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    
}
