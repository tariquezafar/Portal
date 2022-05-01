using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{

    public class HolidayActivityCalenderViewModel : IModel
    {
        public long id { get; set; }

        public string start { get; set; }
        public string title { get; set; }
        public string end { get; set; }

        public string message { get; set; }
        public string status { get; set; }

    }
    
}
