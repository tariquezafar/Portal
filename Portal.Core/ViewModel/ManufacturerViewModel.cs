using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class ManufacturerViewModel
    {
        public int ManufacturerId { get; set; }
        public string ManufacturerCode { get; set; }
        public string ManufacturerName { get; set; }
        
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public bool Manufacturer_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        

    }

 
}
