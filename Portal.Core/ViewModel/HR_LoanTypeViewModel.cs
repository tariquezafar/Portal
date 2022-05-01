using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class HR_LoanTypeViewModel : IModel
    {
        public int LoanTypeId { get; set; }
        public string LoanTypeName { get; set; }
        public decimal LoanInterestRate { get; set; }
        public string InterestCalcOn { get; set; }
        public bool LoanType_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    
}
