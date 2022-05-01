using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class HR_ClaimTypeViewModel: IModel
    {
        public int ClaimTypeId { get; set; }
        public string ClaimTypeName { get; set; }
        public string ClaimNature { get; set; }
        public bool ClaimType_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    
}
