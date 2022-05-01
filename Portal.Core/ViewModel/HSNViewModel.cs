using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class HSNViewModel
    {
        public int HSNID { get; set; }
        public string HSNCode { get; set; }
        public decimal CGST_Perc { get; set; }
        public decimal SGST_Perc { get; set; }
        public decimal IGST_Perc { get; set; }



        public bool HSN_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        

    }

 
}
