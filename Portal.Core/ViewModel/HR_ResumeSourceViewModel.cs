using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class HR_ResumeSourceViewModel : IModel
    {
        public int ResumeSourceId { get; set; }
        public string ResumeSourceName { get; set; }
        public bool ResumeSource_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    
}
