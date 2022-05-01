using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class UOMViewModel:IModel
    {
        public int UOMId { get; set; }
        public string UOMName { get; set; }
        public string UOMDesc { get; set; }
        public bool UOM_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    
}
