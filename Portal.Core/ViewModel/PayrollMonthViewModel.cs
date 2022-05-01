using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class PayrollMonthViewModel:IModel
    {
        public int MonthId { get; set; }
        public string MonthName { get; set; }
        public string MonthShortName { get; set; }
        public int MonthNo { get; set; }
        public string message { get; set; }
        public bool status { get; set; } 
    }
    
}
