using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class HR_VerificationAgencyViewModel : IModel
    {
        public int VerificationAgencyId { get; set; }
        public string VerificationAgencyName { get; set; }
        public bool VerificationAgency_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    
}
