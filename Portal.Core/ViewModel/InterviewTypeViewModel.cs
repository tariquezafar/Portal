using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class InterviewTypeViewModel : IModel
    {
        public int InterviewTypeId { get; set; }
        public string InterviewTypeName { get; set; } 
        public bool InterviewType_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    
}
