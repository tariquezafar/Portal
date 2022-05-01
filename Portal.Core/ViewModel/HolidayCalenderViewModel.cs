using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class HolidayCalenderViewModel
    {
        public long HolidayCalenderId { get; set; }
        public int CalenderId { get; set; }
        public string CalenderName { get; set; }
        public string HolidayDate { get; set; }
        public string HolidayDescription { get; set; }
        public int HolidayTypeId { get; set; }
        public string HolidayTypeName { get; set; } 
        public bool HolidayStatus { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }

    }
   
}
