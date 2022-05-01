using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class CalenderViewModel:IModel
    {

        public int CalenderId { get; set; }
        public string CalenderName { get; set; }
        public Nullable<int> CalenderYear { get; set; }
        public bool Calender_Status { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedName { get; set; }
        public string ModifiedDate { get; set; }
        public Nullable<bool> Status { get; set; }
          
       

    }
    
}
