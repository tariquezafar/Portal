using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    
    public class SizeViewModel : IModel
    {
        public int SizeId { get; set; }
        public string SizeDesc { get; set; }
        public string SizeCode { get; set; }

        public int ProductMainGroupId { get; set; }
        public string ProductMainGroupName { get; set; }
        public int ProductSubGroupId { get; set; }
        public string ProductSubGroupName { get; set; }
        public bool Size_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    

}
