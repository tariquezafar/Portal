using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
   public class SLTypeViewModel:IModel
    {
        public int SLTypeId { get; set; }
        public string SLTypeName { get; set; }
        public bool SLType_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }
    }
}
