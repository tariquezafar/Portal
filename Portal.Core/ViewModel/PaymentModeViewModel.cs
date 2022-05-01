using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
   public class PaymentModeViewModel : IModel
    {
        public int PaymentModeId { get; set;}
        public string PaymentModeName { get; set;}
        public bool PaymentMode_Status { get; set;}
        public string message { get; set; }
        public string status { get; set; }
    }
}
