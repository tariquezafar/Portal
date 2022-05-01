using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class FormTypeViewModel : IModel
    {
        public int FormTypeId { get; set; }
        public string FormTypeDesc { get; set; }    
        public int CompanyId { get; set; }     
        public bool FormType_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    
}
