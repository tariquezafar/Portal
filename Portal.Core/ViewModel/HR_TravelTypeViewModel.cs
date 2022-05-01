using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class HR_TravelTypeViewModel : IModel
    {
        public int TravelTypeId { get; set; }
        public string TravelTypeName { get; set; }
        public bool TravelType_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    
}
