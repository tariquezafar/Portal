using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class HR_AdvanceTypeViewModel : IModel
    {
        public int AdvanceTypeId { get; set; }
        public string AdvanceTypeName { get; set; }
        public bool AdvanceType_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    
}
