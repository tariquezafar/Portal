using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Common
{
    public class ResponseOut
    {
        public string message { get; set; }
        public string status { get; set; }
        public long trnId { get; set; }
        public string indentMessage { get; set; }
    }
}
