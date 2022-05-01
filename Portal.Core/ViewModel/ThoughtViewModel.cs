using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{

    public class ThoughtViewModel : IModel
    {
        public int ThoughtId { get; set; }
        public string ThoughtMessage { get; set; }    
        public string ThoughtType { get; set; }     
        public string ExpiryDate { get; set; }
        public bool Thought_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    
}
