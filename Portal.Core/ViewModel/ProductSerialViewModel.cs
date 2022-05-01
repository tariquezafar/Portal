using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class ProductSerialViewModel
    {
        public long ProductSerialId { get; set; }
        public Nullable<long> ProductId { get; set; }
        public string ProductSerialNo { get; set; }
        public string Serial1 { get; set; }
        public string Serial2 { get; set; }
        public string Serial3 { get; set; }
        public string ProductSerialStatus { get; set; }

    }
}
