using Portal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
    public class ServicesViewModel:IModel
    {
        public int ServicesId { get; set; }
        public string ServicesName { get; set; }
        public bool Services_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }
    }
}
