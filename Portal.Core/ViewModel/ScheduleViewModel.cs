using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class ScheduleViewModel:IModel
    {
        public int ScheduleId { get; set; }
        public string ScheduleName { get; set; } 
        public int ScheduleNo { get; set; }
        public string GLType { get; set; }
        public int SequenceNo { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedName { get; set; } 
        public int ModifiedBy { get; set; }
        public string ModifiedDate { get; set; } 
        public string ModifiedName { get; set; }
        public bool Schedule_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    
}
