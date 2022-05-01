using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class LeadStatusViewModel:IModel
    {
        public int LeadStatusId { get; set; }
        public string LeadStatusName { get; set; } 
        public int CompanyId { get; set; }
        public bool Lead_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    
}
