using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class ActivityCalenderViewModel
    {
        public long ActivityCalenderId { get; set; }
        public int CalenderId { get; set; }
        public string ActivityDate { get; set; }
        public string ActivityDescription { get; set; }  
        public string CalenderName { get; set; }  
        public bool ActivityStatus { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
    }
   
}
