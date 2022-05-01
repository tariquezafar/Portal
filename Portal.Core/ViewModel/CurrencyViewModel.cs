using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core
{
   public class CurrencyViewModel
    {
        public Int32 CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public bool CurrencyStatus { get; set; }
        public string message { get; set; }
        public string status { get; set; }
    }

}
