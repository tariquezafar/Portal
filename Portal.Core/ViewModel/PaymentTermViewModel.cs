using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class PaymentTermViewModel:IModel
    {
        public int PaymentTermId { get; set; }
        public string PaymentTermDesc { get; set; } 
        public int CompanyId { get; set; }
        public bool PaymentTerm_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    
}
